"""
Sc3 provider resolver - READS ONLY. Enumerates ecoinvent 3.8 APOS candidates for the
Sc3 (guided disassembly) build before any amounts are specced (§0.2 discipline -
dataset names are never asserted from memory, only from the live DB).
Writes ..\Outputs\s5_sc3_candidates.txt.
Run:  py "C:\Claude\Projects\AR_DPP\LCA_Analysis\Scripts\2_eol_scenarios\s5_sc3_resolve.py"
"""
import pathlib
import olca_ipc as ipc, olca_schema as o

HERE = pathlib.Path(__file__).resolve().parent.parent.parent / "Outputs" / pathlib.Path(__file__).resolve().parent.name
HERE.mkdir(parents=True, exist_ok=True)
TXT = HERE / "s5_sc3_candidates.txt"; _l = []
def log(m): print(m); _l.append(str(m))

GROUPS = [
    ("Manual dismantling (does an electronics/WEEE manual-dismantling activity exist?)",
     ["manual dismantling", "manual treatment", "dismantling"]),
    ("Used/waste printed wiring board treatment (clean-board smelter-feed families)",
     ["used printed wiring board", "printed wiring board, for recycling",
      "waste printed wiring", "printed wiring boards"]),
    ("Electronics scrap / metals recovery routes",
     ["electronics scrap", "metals recovery", "in copper smelter", "precious metal"]),
    ("Aluminium scrap remelt (secondary-Al burden question)",
     ["aluminium scrap", "aluminium, secondary", "secondary aluminium"]),
    ("Copper scrap routes (contacts fraction)",
     ["copper scrap", "copper, secondary", "treatment of scrap copper"]),
]

client = ipc.Client(8080)

def main():
    procs = client.get_descriptors(o.Process)
    log(f"Loaded {len(procs)} process descriptors.\n")
    for title, terms in GROUPS:
        log(f"=== {title} ===")
        hits, seen = [], set()
        for t in terms:
            tl = t.lower()
            for d in procs:
                n = (d.name or "").lower()
                if tl in n and d.id not in seen:
                    seen.add(d.id); hits.append(d)
        hits.sort(key=lambda d: (0 if "market for" in (d.name or "").lower() else 1,
                                 len(d.name or "")))
        if not hits: log("  (no candidates)")
        for d in hits[:15]:
            log(f"  {d.name}")
            log(f"      id={d.id}")
        log("")
    TXT.write_text("\n".join(_l), encoding="utf-8")
    print(f"Wrote {TXT}")

if __name__ == "__main__":
    main()
