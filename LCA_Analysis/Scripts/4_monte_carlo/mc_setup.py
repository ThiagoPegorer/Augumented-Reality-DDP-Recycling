"""
Monte Carlo setup - writes uncertainty DISTRIBUTIONS onto the key exchanges, per the
[A]/[L]-range tags in LCA_framework_v4.md. Idempotent (overwrites distributions on re-run).
Declared limitation (thesis text): distributions are sampled per-exchange, independently -
parameter correlations (e.g. Lee arrival driving Au/Ag/Pd jointly) are not represented.
Run:  py "C:\Claude\Projects\AR_DPP\LCA_Analysis\Scripts\4_monte_carlo\mc_setup.py"
Afterwards: F5 in openLCA; spot-check one exchange (uncertainty column filled).
"""
import pathlib
import olca_ipc as ipc, olca_schema as o

HERE = pathlib.Path(__file__).resolve().parent.parent.parent / "Outputs" / pathlib.Path(__file__).resolve().parent.name
HERE.mkdir(parents=True, exist_ok=True)
LOG = HERE / "mc_setup_log.txt"; _l = []
def log(m): print(m); _l.append(str(m))

TRI = "tri"; UNI = "uni"
# (process, flow-name prefix, is_input, kind, p1, p2, p3, rationale)
TARGETS = [
    ("VCU S2 Hardware Assembly", "heat, district or industrial, natural gas", True,
     TRI, 0.85, 1.03, 1.18, "D&G 2004 total 6.5-9 MJ/kg scaled to gas share [L]"),
    ("VCU S2 Hardware Assembly", "electricity, medium voltage", True,
     TRI, 0.385, 0.47, 0.533, "D&G 2004 total 6.5-9 MJ/kg scaled to elec share [L]"),
    ("VCU S2 Hardware Assembly", "mounting, surface mount technology", True,
     UNI, 0.0158, 0.0315, None, "board-area vs per-side + density mismatch [A]"),
    ("VCU S3 Distribution", "transport, freight, lorry", True,
     TRI, 0.297, 0.323, 0.363, "distance 450-550 km [A]"),
    ("VCU S4 Use Phase", "electricity, low voltage", True,
     TRI, 54.0, 66.2, 189.0, "P 9-20 W x speed 35-45 km/h x eta 0.68-0.83 [L+A]"),
    ("VCU S5 EoL Sc3 (guided disassembly)", "electricity, low voltage", True,
     TRI, 0.002, 0.01, 0.03, "tools + AR headset [A]"),
    ("VCU S5 EoL Sc4 (disassembly + reuse)", "electricity, low voltage", True,
     TRI, 0.005, 0.02, 0.05, "tools + headset + test bench [A]"),
    ("VCU S5 EoL Sc2 credits (avoided virgin)", "aluminium, primary, ingot", True,
     TRI, 0.3003, 0.3003, 0.3611, "Al remelt 79-95% [L Bigum / clean-scrap upside]"),
    ("VCU S5 EoL Sc3 credits (avoided virgin)", "aluminium, primary, ingot", True,
     TRI, 0.3399, 0.3399, 0.4087, "Al remelt 79-95%"),
    ("VCU S5 EoL Sc3 credits (avoided virgin)", "gold", True,
     TRI, 8.11e-5, 8.66e-5, 8.84e-5, "Lee arrival 90-98% [L]"),
    ("VCU S5 EoL Sc4 credits (avoided virgin + components)", "aluminium, primary, ingot", True,
     TRI, 0.3399, 0.3399, 0.4087, "Al remelt 79-95%"),
    ("VCU S5 EoL Sc4 credits (avoided virgin + components)", "integrated circuit, logic type", True,
     TRI, 0.0030, 0.0042, 0.0054, "functional yield 0.5-0.9 [A] - Sc4's widest lever"),
    ("VCU S5 EoL Sc4 credits (avoided virgin + components)", "transistor, surface-mounted", True,
     TRI, 0.0034, 0.0047, 0.0061, "functional yield 0.5-0.9 [A]"),
]

client = ipc.Client(8080)

def make_unc(kind, p1, p2, p3):
    # olca-schema 2.x field names: minimum / mode / maximum (not parameter1/2/3)
    if kind == TRI:
        return o.Uncertainty(distribution_type=o.UncertaintyType.TRIANGLE_DISTRIBUTION,
                             minimum=p1, mode=p2, maximum=p3)
    return o.Uncertainty(distribution_type=o.UncertaintyType.UNIFORM_DISTRIBUTION,
                         minimum=p1, maximum=p2)

def main():
    log("MC setup - writing uncertainty distributions onto key exchanges")
    by_proc = {}
    for t in TARGETS: by_proc.setdefault(t[0], []).append(t)
    total_set = 0
    for proc_name, targets in by_proc.items():
        proc = client.get(o.Process, name=proc_name)
        if proc is None:
            log(f"!! '{proc_name}' not found - SKIPPED ({len(targets)} targets)"); continue
        changed = False
        for (_, prefix, is_input, kind, p1, p2, p3, why) in targets:
            hit = None
            for e in (proc.exchanges or []):
                if e.is_input == is_input and e.flow and \
                   (e.flow.name or "").lower().startswith(prefix.lower()):
                    hit = e; break
            if hit is None:
                log(f"  !! {proc_name}: no exchange starting '{prefix}' - REVIEW"); continue
            mode_ok = (kind == UNI) or (p1 <= p2 <= p3)
            amt_ok = (kind == UNI and p1 <= hit.amount <= p2) or \
                     (kind == TRI and abs(hit.amount - p2) < 1e-9)
            hit.uncertainty = make_unc(kind, p1, p2, p3)
            changed = True; total_set += 1
            shape = f"tri({p1:g}, {p2:g}, {p3:g})" if kind == TRI else f"uni({p1:g}, {p2:g})"
            flag = "" if (mode_ok and amt_ok) else "  ?? amount/mode mismatch - REVIEW"
            log(f"  OK {proc_name} :: {hit.flow.name[:45]} = {hit.amount:g} -> {shape}  [{why}]{flag}")
        if changed:
            client.put(proc); log(f"  -> saved '{proc_name}'")
    log(f"\nDONE - {total_set}/{len(TARGETS)} distributions set. F5 in openLCA; spot-check one.")
    log("Declared: per-exchange independent sampling (no parameter correlation) - thesis caveat.")

if __name__ == "__main__":
    try: main()
    finally:
        LOG.write_text("\n".join(_l), encoding="utf-8")
        print(f"Log: {LOG}")
