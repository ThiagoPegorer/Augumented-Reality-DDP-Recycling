"""
Sc3 dataset inspector - READS ONLY. Prints reference flow, unit, location, description
and the full exchange list for the decisive Sc3 candidate datasets, so the choice can be
double-checked (log here + manual check in the openLCA GUI documentation tabs).
Writes ..\Outputs\s5_sc3_inspect.txt.
Run:  py "C:\Claude\Projects\AR_DPP\LCA_Analysis\Scripts\2_eol_scenarios\s5_sc3_inspect.py"
"""
import pathlib
import olca_ipc as ipc, olca_schema as o

HERE = pathlib.Path(__file__).resolve().parent.parent.parent / "Outputs" / pathlib.Path(__file__).resolve().parent.name
HERE.mkdir(parents=True, exist_ok=True)
TXT = HERE / "s5_sc3_inspect.txt"; _l = []
def log(m): print(m); _l.append(str(m))

TARGETS = [
    ("A1", "6205b74b-39cf-3631-a6a4-c93c2e9c1135",
     "treatment of electronics scrap from control units (loc 1)"),
    ("A2", "51ff0c91-d103-3e02-8424-cdab1b06b2bc",
     "treatment of electronics scrap from control units (loc 2)"),
    ("B1", "82c2ca40-3537-3588-bb73-5d3e9c8c6e05",
     "treatment of electronics scrap, metals recovery in copper smelter (ref: electronics scrap, loc 1)"),
    ("B2", "01e00045-86d0-373b-890b-da147da7ab66",
     "treatment of electronics scrap, metals recovery in copper smelter (ref: electronics scrap, loc 2)"),
    ("C1", "db7889d7-cd6b-3ad8-abdb-8bc3de90e3d4",
     "treatment of used laptop computer, manual dismantling"),
    ("D1", "bedea540-5447-3b99-b423-afe33b1d1da1",
     "market for used printed wiring boards"),
    ("E1", "fda221b3-9efd-3cdb-a01a-b7ff1dde5974",
     "market for electronics scrap from control units"),
]

client = ipc.Client(8080)

def show(tag, uid, label):
    log("=" * 100)
    log(f"[{tag}] {label}")
    log(f"    uuid: {uid}")
    p = client.get(o.Process, uid=uid)
    if p is None:
        log("    !! NOT FOUND"); return
    loc = p.location.name if p.location else "-"
    log(f"    name:     {p.name}")
    log(f"    location: {loc}")
    desc = (p.description or "").strip()
    if desc:
        log(f"    description ({len(desc)} chars, first 1200):")
        for line in desc[:1200].splitlines():
            log(f"      | {line}")
    exchanges = p.exchanges or []
    ref = next((e for e in exchanges if getattr(e, "is_quantitative_reference", False)), None)
    if ref is not None:
        un = ref.unit.name if getattr(ref, "unit", None) else "?"
        d = "INPUT" if ref.is_input else "OUTPUT"
        log(f"    REFERENCE: {d} {ref.amount} {un}  '{ref.flow.name if ref.flow else '?'}'")
        log("      (waste-treatment convention if reference is an INPUT waste flow)")
    prods, elems = [], 0
    for e in exchanges:
        ftype = str(getattr(e.flow, "flow_type", "") or "")
        if "ELEMENTARY" in ftype.upper():
            elems += 1; continue
        prods.append(e)
    log(f"    exchanges: {len(exchanges)} total ({elems} elementary suppressed)")
    for e in prods:
        if getattr(e, "is_quantitative_reference", False): continue
        d = "IN " if e.is_input else "OUT"
        un = e.unit.name if getattr(e, "unit", None) else "?"
        fn = e.flow.name if e.flow else "?"
        prov = ""
        if getattr(e, "default_provider", None) is not None:
            prov = f"  <- {e.default_provider.name}"
        log(f"      {d} {e.amount:>14.6g} {un:<8} {fn}{prov}")
    log("")

def main():
    log("Sc3 candidate inspection - reference flows, units, descriptions, exchanges\n")
    for tag, uid, label in TARGETS:
        try: show(tag, uid, label)
        except Exception as ex: log(f"[{tag}] ERROR: {ex}")
    log("Reading guide:")
    log("- A1/A2: does the control-units treatment model the WHOLE current EoL route (shredder-")
    log("  based?) or a smelter feed? Check description wording + what it consumes/produces.")
    log("- B1/B2: reference should be INPUT 'electronics scrap' (waste convention). Check which")
    log("  co-product outputs exist (Cu anode, Pb, PM in slime) - APOS allocation question:")
    log("  if metals appear as OUTPUTS here, our explicit credits must NOT also be applied")
    log("  without declaring the overlap.")
    log("- C1: unit basis (per kg or per unit laptop?) and what burdens it carries (electricity,")
    log("  facility, transport) - decides proxy use vs declared ~0 dismantling burden.")
    log("- D1/E1: where do these markets route the board/scrap - which treatments consume them?")

if __name__ == "__main__":
    try: main()
    finally:
        TXT.write_text("\n".join(_l), encoding="utf-8")
        print(f"\nWrote {TXT}")
