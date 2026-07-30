"""
Product-system builder - creates the SEVEN persistent product systems (Sc1 + gross/credits
for Sc2/Sc3/Sc4) via IPC with default-provider linking. These are the objects the GUI needs
for contribution trees, Sankey, and Monte Carlo.
Idempotent (deletes same-named product systems first). Logs to ..\Outputs\ps_build_log.txt.
Prereqs: DB open, IPC 8080, all scenario processes built (Bigum-refined state).
Run:  py "C:\Claude\Projects\AR_DPP\LCA_Analysis\Scripts\3_impact_assessment\ps_build.py"
If IPC product-system creation is unsupported by the installed olca-ipc version, the log says
so explicitly and lists the exact GUI steps instead (right-click process -> Create product
system -> prefer default providers, unit processes).
"""
import pathlib, traceback
import olca_ipc as ipc, olca_schema as o

HERE = pathlib.Path(__file__).resolve().parent.parent.parent / "Outputs" / pathlib.Path(__file__).resolve().parent.name
HERE.mkdir(parents=True, exist_ok=True)
LOG = HERE / "ps_build_log.txt"; _l = []
def log(m): print(m); _l.append(str(m))

PROCESSES = [
    "VCU S5 EoL Sc1 (no recycling)",
    "VCU S5 EoL Sc2 (bulk recycling)",
    "VCU S5 EoL Sc2 credits (avoided virgin)",
    "VCU S5 EoL Sc3 (guided disassembly)",
    "VCU S5 EoL Sc3 credits (avoided virgin)",
    "VCU S5 EoL Sc4 (disassembly + reuse)",
    "VCU S5 EoL Sc4 credits (avoided virgin + components)",
]

client = ipc.Client(8080)

def main():
    log("Product-system build - 7 persistent systems, default-provider linking")

    # idempotence: remove prior product systems with the same names
    try:
        for d in client.get_descriptors(o.ProductSystem):
            if (d.name or "").split("|")[0].strip() in [p for p in PROCESSES]:
                try: client.delete(d); log(f"Deleted prior product system '{d.name}'.")
                except Exception as ex: log(f"(delete {d.name}: {ex})")
    except Exception as ex:
        log(f"(product-system listing failed: {ex})")

    config = None
    try:
        config = o.LinkingConfig(
            prefer_unit_processes=True,
            provider_linking=o.ProviderLinking.PREFER_DEFAULTS)
    except Exception as ex:
        log(f"!! olca_schema.LinkingConfig unavailable ({ex}) - GUI fallback below.")

    ok = 0
    for name in PROCESSES:
        proc = client.get(o.Process, name=name)
        if proc is None:
            log(f"!! process '{name}' not found - SKIPPED"); continue
        ref = None; err = None
        if config is not None:
            for call in (
                lambda: client.create_product_system(proc, config),
                lambda: client.create_product_system(
                    o.Ref(ref_type=o.RefType.Process, id=proc.id, name=proc.name), config),
                lambda: client.create_product_system(proc.id, config),
            ):
                try:
                    ref = call(); break
                except TypeError as ex: err = ex; continue
                except Exception as ex: err = ex; break
        if ref is not None:
            ok += 1
            log(f"  OK product system created for '{name}' [{getattr(ref, 'id', '?')}]")
        else:
            log(f"  !! could not create product system for '{name}' via IPC ({err})")

    if ok == len(PROCESSES):
        log(f"\nDONE - {ok}/{len(PROCESSES)} product systems created. F5 in openLCA;")
        log("they appear under 'Product systems'. Open one -> Model graph to sanity-check links.")
    else:
        log(f"\n{ok}/{len(PROCESSES)} created via IPC. For any missing ones, GUI steps:")
        log("  1. Navigator -> Processes -> right-click the process -> 'Create product system'")
        log("  2. Check 'Auto-link processes', Provider linking = 'Prefer default providers',")
        log("     Preferred process type = 'Unit process' -> Finish. Keep the default name.")
        log("  3. Repeat for each missing process above, then F5.")

if __name__ == "__main__":
    try: main()
    except Exception: log("FATAL:\n" + traceback.format_exc())
    finally: LOG.write_text("\n".join(_l), encoding="utf-8"); print(f"Log: {LOG}")
