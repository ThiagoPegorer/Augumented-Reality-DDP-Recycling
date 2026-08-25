# =============================================================================
# Figure — contribution of life cycle stages 1 to 4, three reporting categories
# Placed in section 4.1 after Table 8 and the sentence
# "In every scenario, stages 1 to 4 are identical."
# Basis: the stage 1-4 subtotal = 100 %. Stage 5 is EXCLUDED and keeps its own
# table. The footnote states what the subtotal is worth against the gross, so
# the two bases can be reconciled by the reader.
# =============================================================================
import numpy as np, pandas as pd, matplotlib.pyplot as plt
from pathlib import Path

# --- locate the stage contribution file -------------------------------------
_cands = [Path("Outputs/3_impact_assessment/impact_stage_contributions.csv"),
          Path("../Outputs/3_impact_assessment/impact_stage_contributions.csv"),
          Path("../../Outputs/3_impact_assessment/impact_stage_contributions.csv")]
STAGE_CSV = next((p for p in _cands if p.exists()), None)
assert STAGE_CSV is not None, "impact_stage_contributions.csv not found - set STAGE_CSV by hand"
stg = pd.read_csv(STAGE_CSV)
stg["category"] = stg["category"].str.strip()

# --- the three reporting categories, exactly as spelled in that file ---------
CATS = [("Resource use minerals and metals", "Resource use, minerals and metals", "kg Sb eq"),
        ("Climate change",                   "Climate change",                   "kg CO2 eq"),
        ("Eutrophication freshwater",        "Eutrophication, freshwater",       "kg P eq")]

STAGES = [("s1", "S1 Materials and construction"),
          ("s2", "S2 Hardware assembly"),
          ("s3", "S3 Distribution"),
          ("s4", "S4 Use phase")]

# Okabe-Ito, colour-vision-deficiency safe. First three match the notebook palette.
STAGE_COLOR = {"s1": "#0072B2", "s2": "#E69F00", "s3": "#009E73", "s4": "#CC79A7"}

LABEL_FLOOR = 1.0        # slices below this are not labelled on the pie
SF          = 4          # significant figures in the table

# --- build the table ---------------------------------------------------------
rows = []
for key, nice, unit in CATS:
    r = stg.loc[stg["category"] == key]
    assert len(r) == 1, f"category not matched uniquely: {key}"
    r = r.iloc[0]
    vals   = np.array([float(r[s]) for s, _ in STAGES])
    sub14  = vals.sum()
    gross  = float(r["total_sc1"])
    shares = 100.0 * vals / sub14
    for (s, sname), v, sh in zip(STAGES, vals, shares):
        rows.append({"Category": nice, "Unit": unit, "Stage": sname,
                     "Impact": v, "Share of stages 1-4 (%)": sh})
    rows.append({"Category": nice, "Unit": unit, "Stage": "Total stages 1-4",
                 "Impact": sub14, "Share of stages 1-4 (%)": 100.0})
    rows.append({"Category": nice, "Unit": unit, "Stage": "Stages 1-4 as share of gross (%)",
                 "Impact": np.nan, "Share of stages 1-4 (%)": 100.0 * sub14 / gross})

stage_tbl = pd.DataFrame(rows)

# --- verification, refuses to draw a chart that does not add up --------------
for key, nice, unit in CATS:
    r = stg.loc[stg["category"] == key].iloc[0]
    vals = np.array([float(r[s]) for s, _ in STAGES])
    sub  = stage_tbl.loc[(stage_tbl["Category"] == nice) &
                         (stage_tbl["Stage"] == "Total stages 1-4"), "Impact"].iloc[0]
    assert np.isclose(vals.sum(), sub, rtol=0, atol=1e-15), f"subtotal mismatch: {nice}"
    sh = stage_tbl.loc[(stage_tbl["Category"] == nice) &
                       (stage_tbl["Stage"].isin([n for _, n in STAGES])),
                       "Share of stages 1-4 (%)"].sum()
    assert abs(sh - 100.0) < 1e-9, f"shares do not close on 100: {nice} = {sh}"
    # stage 5 must NOT be inside this basis
    assert "s5_sc1" in stg.columns and float(r["s5_sc1"]) not in vals, "stage 5 leaked into the pie"
print("checks passed: three categories, four stages, shares close on 100 %")

# --- the figure --------------------------------------------------------------
INSIDE_FLOOR  = 5.0      # labelled inside the wedge
OUTSIDE_FLOOR = 0.1      # labelled outside with a leader; below this, table only

fig, axes = plt.subplots(1, 3, figsize=(13.2, 5.9))
plt.subplots_adjust(bottom=0.30, top=0.86, wspace=0.15)

for ax, (key, nice, unit) in zip(axes, CATS):
    r      = stg.loc[stg["category"] == key].iloc[0]
    vals   = np.array([float(r[s]) for s, _ in STAGES])
    shares = 100.0 * vals / vals.sum()
    frac   = 100.0 * vals.sum() / float(r["total_sc1"])

    wedges, _, autotexts = ax.pie(
        vals,
        colors=[STAGE_COLOR[s] for s, _ in STAGES],
        startangle=90, counterclock=False,
        autopct=lambda p: f"{p:.2f} %" if p >= INSIDE_FLOOR else "",
        pctdistance=0.62,
        wedgeprops=dict(edgecolor="white", linewidth=1.1),
        textprops=dict(fontsize=10),
    )
    for t in autotexts:
        t.set_color("white"); t.set_fontweight("bold")

    # slices too thin to label inside get a leader line to the outside,
    # de-collided vertically so two adjacent thin wedges do not print on top
    # of each other (minerals has two of them within one degree)
    ax.set_xlim(-1.85, 1.85); ax.set_ylim(-1.30, 1.45)
    outs = []
    for w, sh in zip(wedges, shares):
        if OUTSIDE_FLOOR <= sh < INSIDE_FLOOR:
            ang = np.deg2rad((w.theta1 + w.theta2) / 2.0)
            outs.append((np.cos(ang), np.sin(ang), sh))
    outs.sort(key=lambda t: -t[1])
    placed = []
    for x, y, sh in outs:
        tx, ty = 1.30 * x, 1.30 * y
        for px, py in placed:
            if abs(ty - py) < 0.24 and (tx >= 0) == (px >= 0):
                ty = py - 0.24
        placed.append((tx, ty))
        ax.annotate(f"{sh:.2f} %", xy=(0.94 * x, 0.94 * y), xytext=(tx, ty),
                    ha="left" if tx >= 0 else "right", va="center", fontsize=9,
                    arrowprops=dict(arrowstyle="-", lw=0.8, color="0.35",
                                    connectionstyle="arc3,rad=0.0"))

    ax.set_title(f"{nice}\n({unit})", fontsize=11, fontweight="bold", pad=10)

    # every stage listed under its pie, including the ones with no visible wedge
    block = "\n".join(f"{n:<30s}{sh:8.4f} %" for (_, n), sh in zip(STAGES, shares))
    ax.text(0.5, -0.14, block, transform=ax.transAxes, ha="center", va="top",
            fontsize=8.6, family="DejaVu Sans Mono", linespacing=1.5)
    ax.text(0.5, -0.44, f"stages 1 to 4 = {frac:.4f} % of the gross result",
            transform=ax.transAxes, ha="center", va="top", fontsize=8.6, style="italic")

fig.legend(handles=[plt.Rectangle((0, 0), 1, 1, facecolor=STAGE_COLOR[s], edgecolor="white")
                    for s, _ in STAGES],
           labels=[n for _, n in STAGES],
           loc="lower center", ncol=4, frameon=False, fontsize=9.5,
           bbox_to_anchor=(0.5, 0.085))
fig.suptitle("Contribution of life cycle stages 1 to 4, identical in all four scenarios",
             fontsize=12.5, y=0.96)
fig.text(0.5, 0.020,
         f"Wedges below {INSIDE_FLOOR:.0f} % are labelled outside; below {OUTSIDE_FLOOR:g} % "
         "they are listed beneath the chart only. Basis: the stage 1 to 4 subtotal = 100 %.",
         ha="center", fontsize=8.4, style="italic")

try:
    save(fig, "fig_stage_contribution_pies")     # notebook helper, cell 3
except NameError:
    fig.savefig("fig_stage_contribution_pies.png", dpi=300, bbox_inches="tight")
    fig.savefig("fig_stage_contribution_pies.svg", bbox_inches="tight")
plt.show()

# --- the table, printed and written for the thesis ---------------------------
# Wide layout, the shape a body table needs: one row per stage, one column pair
# per category. The long frame above is kept only for the checks.
wide = {}
for key, nice, unit in CATS:
    r      = stg.loc[stg["category"] == key].iloc[0]
    vals   = np.array([float(r[s_]) for s_, _ in STAGES])
    shares = 100.0 * vals / vals.sum()
    wide[(nice, f"Impact ({unit})")] = list(vals) + [vals.sum()]
    wide[(nice, "Share (%)")]        = list(shares) + [100.0]

stage_body = pd.DataFrame(wide, index=[n for _, n in STAGES] + ["Total stages 1 to 4"])
stage_body.index.name = "Life cycle stage"
stage_body.columns = pd.MultiIndex.from_tuples(stage_body.columns)

fmt = {}
for key, nice, unit in CATS:
    fmt[(nice, f"Impact ({unit})")] = "{:.4g}".format
    fmt[(nice, "Share (%)")]        = "{:.4f}".format
print(stage_body.to_string(formatters=fmt))
print()
for key, nice, unit in CATS:
    r = stg.loc[stg["category"] == key].iloc[0]
    sub = sum(float(r[s_]) for s_, _ in STAGES)
    print(f"{nice}: stages 1 to 4 = {100.0 * sub / float(r['total_sc1']):.4f} % of the gross result")

with pd.ExcelWriter("stage_1_to_4_contributions.xlsx") as xl:
    stage_body.to_excel(xl, sheet_name="body_table")
    stage_tbl.to_excel(xl, sheet_name="long_form", index=False)
