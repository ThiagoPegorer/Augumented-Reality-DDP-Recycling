"""
S1 builder v4 - 'VCU S1 Materials & Construction' per LCA_framework_v4.md §3 Stage-1 spec
(hybrid: component datasets + material markets; BOM_v4.md = source of truth).
Idempotent. Logs to ..\Outputs\s1_build_log.txt. Prereqs: DB open, IPC on 8080.
Run:  py "C:\Claude\Projects\AR_DPP\LCA_Analysis\Scripts\1_stage_builds\s1_build.py"
"""
import pathlib, traceback
import olca_ipc as ipc, olca_schema as o

HERE = pathlib.Path(__file__).resolve().parent.parent.parent / "Outputs" / pathlib.Path(__file__).resolve().parent.name
HERE.mkdir(parents=True, exist_ok=True)
LOG = HERE / "s1_build_log.txt"; _l=[]
def log(m): print(m); _l.append(str(m))

PROC="VCU S1 Materials & Construction"; REF="VCU materials (S1)"; REFKG=0.66
# (label, uid or None, amount_kg, search_terms_if_unresolved)
INPUTS=[
 ("ICs (proc+flash+xcvr+AFE+reg+IMU) [A proxy]","92416fbe-e108-3676-86f9-94a87b83d2a5",0.0147,None),
 ("Power stages 6x — RESOLVED AT RUNTIME",None,0.009,["market for transistor","transistor, surface-mounted","power semiconductor"]),
 ("Ta capacitors","5ef6e692-46b0-3eba-96a2-062752efb612",0.0025,None),
 ("MLCCs/passives [A proxy incl. R/L]","40ca41dd-86a2-3cc7-9ec0-3f2c0ba56bed",0.0449,None),
 ("Solder SAC (Sn95.5Ag3.9Cu0.6, Pb-free [R])","e24986bb-daff-37e5-99d3-40a42c4fe032",0.0032,None),
 ("Aluminium (housing+conn shells)","abb8f45f-8076-3728-a4c0-83ad3d103177",0.439,None),
 ("Copper (foils+contacts)","b103dac8-20d8-38dd-bde3-f275b3487bab",0.049,None),
 ("Stainless steel (fasteners)","5d5262a4-7975-31b7-a6c6-f35d9add4901",0.012,None),
 ("Epoxy resin (FR-4 resin+coating)","c059672a-3040-3425-9c5c-22e9d6572ead",0.023,None),
 ("Thermoplastic+labels [A proxy PE-HD]","79c4c37e-38d9-3a7c-853c-9d69c34781df",0.020,None),
 ("Glass fibre (GFRP proxy, Cu-free)","ce1ec6c0-cad2-3b2d-83b6-a4b49cb16576",0.029,None),
 ("Silicone (TIM+seals)","ae800255-a326-3c52-b5b0-127527c2425a",0.0125,None),
 ("Gold (board+conn plating)","b9a4f149-3f53-3445-a446-8ee24da098ab",9.2e-5,None),
 ("Silver","8434ffb4-c639-3af6-91ba-a239c053753b",5.9e-5,None),
 ("Palladium","174e04de-106d-3946-8b17-886fb2da6dee",5.5e-6,None),
 ("Tantalum (in caps, explicit)","d946ed26-628d-36ae-a023-a438b32eb5f9",9.0e-4,None),
 ("Nickel (connector underplating)","131d4e54-e0bc-32aa-bfa0-fc9a3d625112",5.0e-4,None),
]
DESC=("S1 per LCA_framework_v4 §3: hybrid component datasets + material markets; BOM_v4.1 "
 "source of truth. Trace metals explicit (declared conservative overlap <0.1% mass). "
 "PCB fab & assembly energy in S2 TODO. Runtime-resolved providers flagged in log for review.")

client=ipc.Client(8080)
def bundle(uid):
    p=client.get(o.Process,uid=uid)
    if p is None: return None
    ref=None
    for e in (p.exchanges or []):
        if getattr(e,"is_quantitative_reference",False): ref=e; break
    if ref is None or ref.flow is None: return None
    f=client.get(o.Flow,uid=ref.flow.id)
    loc=p.location.name if p.location else "-"
    un=ref.unit.name if getattr(ref,"unit",None) else "?"
    return (p,f,loc,un)

def resolve(terms):
    ds=client.get_descriptors(o.Process); hits=[]
    for t in terms:
        tl=t.lower()
        for d in ds:
            n=(d.name or "").lower()
            if tl in n: hits.append(d)
    hits.sort(key=lambda d:(0 if "market for" in (d.name or "").lower() else 1,len(d.name or "")))
    return hits[:5]

def main():
    log(f"S1 v4 build - '{PROC}'")
    for d in client.get_descriptors(o.Process):
        if d.name==PROC:
            try: client.delete(d); log("Deleted prior process.")
            except Exception as ex: log(f"(delete: {ex})")
    for d in client.get_descriptors(o.Flow):
        if d.name==REF:
            try: client.delete(d)
            except Exception: pass
    mass=client.get(o.FlowProperty,name="Mass")
    proc=o.new_process(PROC); proc.description=DESC
    rf=o.new_product(REF,mass); client.put(rf)
    out=o.new_output(proc,rf,REFKG); out.is_quantitative_reference=True
    ok=0
    for label,uid,amt,terms in INPUTS:
        try:
            if uid is None:
                cands=resolve(terms)
                if not cands: log(f"  !! {label}: NO candidates for {terms} - SKIPPED"); continue
                log(f"  ?? {label}: runtime-resolved, top candidates:")
                for c in cands: log(f"       - {c.name} [{c.id}]")
                uid=cands[0].id; log(f"     -> linked [0]; REVIEW at checkpoint")
            b=bundle(uid)
            if not b: log(f"  !! {label}: provider {uid} unusable - SKIPPED"); continue
            p,f,loc,un=b
            e=o.new_input(proc,f,amt)
            e.default_provider=o.Ref(ref_type=o.RefType.Process,id=p.id,name=p.name)
            ok+=1; log(f"  OK {label}: {amt} {un} -> {p.name} [{loc}]")
        except Exception as ex:
            log(f"  !! {label}: ERROR {ex}")
    client.put(proc)
    log(f"\nDONE - {ok}/{len(INPUTS)} inputs linked. F5 in openLCA; review '??' lines before S2.")

if __name__=="__main__":
    try: main()
    except Exception: log("FATAL:\n"+traceback.format_exc())
    finally: LOG.write_text("\n".join(_l),encoding="utf-8"); print(f"Log: {LOG}")
