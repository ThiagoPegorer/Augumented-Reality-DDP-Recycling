# --- audit pack: the exact table behind every figure, and a check on each -------
# Each figure in the section is paired here with the table it was drawn from, and each table is
# reconciled against the raw sheets of ARDPP_study_data.xlsx. A check that fails stops the cell
# rather than writing a workbook that disagrees with the data.

AUDIT = {}          # sheet name -> (figure file, what it draws, source, derivation, table)
CHECKS = []         # (label, passed)

def audit(key, figure, draws, source, derivation, table, *checks):
    AUDIT[key] = (figure, draws, source, derivation, table)
    for label, ok in checks:
        CHECKS.append((f"{key}: {label}", bool(ok)))

# --- F1, age group -------------------------------------------------------------
a = participants["age_group"].value_counts().sort_index()
t_age = pd.DataFrame({"age_group": a.index, "participants": a.values})
t_age["share_pct"] = (t_age["participants"] / t_age["participants"].sum() * 100).round(1)
audit("fig1_age", "fig_participants_age", "count of participants per age group",
      "participants sheet, column age_group", "value_counts, no other transformation", t_age,
      ("counts sum to n", t_age["participants"].sum() == len(participants)),
      ("no age group lost", set(t_age["age_group"]) == set(participants["age_group"])))

# --- F2 and F3, the two experience scales --------------------------------------
def t_exp(col):
    s = participants[col].astype(int).value_counts()
    return pd.DataFrame({
        "scale_point":  SCALE_POINTS,
        "label":        [EXP_LABEL[v] or "" for v in SCALE_POINTS],
        "participants": [int(s.get(v, 0)) for v in SCALE_POINTS]})

for key, fig_name, col, what in (
        ("fig2_headset_experience", "fig_participants_headset_experience",
         "prior_xr_experience", "prior experience with head-mounted displays"),
        ("fig3_disassembly_experience", "fig_participants_disassembly_experience",
         "prior_disassembly_experience", "prior experience disassembling electronic devices")):
    tt = t_exp(col)
    audit(key, fig_name, f"count of participants at each point of the {what} scale",
          f"participants sheet, column {col}",
          "count per scale point, zeros kept, labels read from experience_scale", tt,
          ("counts sum to n", tt["participants"].sum() == len(participants)),
          ("every scale point present", len(tt) == 5))

# --- F4, task completion time --------------------------------------------------
t_time = wide.reset_index()[["participant", "manual_s", "ar_s", "difference_s"]]
raw_sum = (times.assign(total=times[STEP_COLS].sum(axis=1))
                .pivot(index="participant", columns="condition", values="total"))
audit("fig4_completion_times", "fig_completion_times",
      "manual and Augmented Reality total time per participant, and the signed difference",
      "completion_times sheet, columns step_1_s to step_5_s",
      "total = sum of the five step values in both conditions; difference = ar minus manual",
      t_time,
      ("totals equal the sum of the raw steps",
       np.allclose(wide["ar_s"].values, raw_sum["ar"].reindex(wide.index).values, equal_nan=True)
       and np.allclose(wide["manual_s"].values, raw_sum["manual"].reindex(wide.index).values,
                       equal_nan=True)),
      ("difference equals ar minus manual",
       np.allclose((wide["ar_s"] - wide["manual_s"]).dropna(),
                   wide["difference_s"].dropna())),
      ("one row per participant with a timed run",
       len(t_time) == times["participant"].nunique()))

# --- F5, perceived usability ---------------------------------------------------
t_usab = usab.reset_index()[["participant", "manual", "augmented_reality", "difference"]]
recheck = (u_items.assign(score=u_items.apply(score_row, axis=1))
                  .pivot(index="participant", columns="condition", values="score"))
audit("fig5_usability_scores", "fig_usability_scores",
      "questionnaire score per participant in each condition, and the signed difference",
      "usability_items sheet, columns q1 to q10, with usability_item_text.polarity",
      "positive items score value minus 1, negative items score 5 minus value, "
      "summed and multiplied by 2.5; difference = Augmented Reality minus manual",
      t_usab,
      ("scores reproduce from the raw items",
       np.allclose(usab["manual"].values, recheck["manual"].reindex(usab.index).values)
       and np.allclose(usab["augmented_reality"].values,
                       recheck["ar"].reindex(usab.index).values)),
      ("every score within 0 to 100",
       bool(t_usab[["manual", "augmented_reality"]].stack().between(0, 100).all())),
      ("difference equals the two columns",
       np.allclose(t_usab["augmented_reality"] - t_usab["manual"], t_usab["difference"])))

# --- F5 supplement, the scoring key -------------------------------------------
# Half the items are worded negatively, so agreement on those means WORSE usability.
# Their contribution is inverted before the sum, and this block states the rule item by
# item, works one participant through it, and checks that the inversion is balanced.
def contribution(item, value):
    """Points an item contributes, 0 worst to 4 best, whichever way it is worded."""
    return (value - 1) if pol[item] == "positive" else (5 - value)

t_key = u_text.set_index("item").loc[Q_COLS, ["polarity", "wording_ar"]].copy()
t_key["rule"]           = np.where(t_key["polarity"] == "positive", "value - 1", "5 - value")
t_key["a 1 scores"]     = [contribution(q, 1) for q in Q_COLS]
t_key["a 5 scores"]     = [contribution(q, 5) for q in Q_COLS]
t_key["best answer is"] = np.where(t_key["polarity"] == "positive", 5, 1)

# a flat answer must land on the midpoint whichever value is used, or the halves are unbalanced
flat = {v: sum(contribution(q, v) for q in Q_COLS) * 2.5 for v in range(1, 6)}
best = sum(contribution(q, 5 if pol[q] == "positive" else 1) for q in Q_COLS) * 2.5
worst = sum(contribution(q, 1 if pol[q] == "positive" else 5) for q in Q_COLS) * 2.5
straight = u_items[u_items[Q_COLS].nunique(axis=1) == 1]

# one participant worked through, item by item
WORKED = "P02"
t_worked = pd.DataFrame({"item": Q_COLS, "polarity": [pol[q] for q in Q_COLS]})
for cond, label in (("manual", "manual"), ("ar", "augmented_reality")):
    row = u_items[(u_items["participant"] == WORKED) &
                  (u_items["condition"] == cond)].iloc[0]
    t_worked[f"{label}_answer"] = [int(row[q]) for q in Q_COLS]
    t_worked[f"{label}_points"] = [contribution(q, row[q]) for q in Q_COLS]

audit("fig5b_scoring_key", "fig_usability_scores",
      "the rule converting each item response into points, and one participant worked through it",
      "usability_item_text sheet, column polarity",
      "positive items contribute value minus 1, negative items contribute 5 minus value; "
      "both give 0 for the worst answer and 4 for the best, so the two halves carry equal weight",
      t_key.reset_index(),
      ("five items worded positively and five negatively",
       list(t_key["polarity"]).count("positive") == 5
       and list(t_key["polarity"]).count("negative") == 5),
      ("every item contributes 0 for its worst answer and 4 for its best",
       all(min(contribution(q, 1), contribution(q, 5)) == 0
           and max(contribution(q, 1), contribution(q, 5)) == 4 for q in Q_COLS)),
      ("the same value on all ten items scores 50.0, whichever value it is",
       set(flat.values()) == {50.0}),
      ("the best possible answer scores 100 and the worst 0", (best, worst) == (100.0, 0.0)),
      ("no participant answered the same value on all ten items", len(straight) == 0))

audit("fig5c_worked_example", "fig_usability_scores",
      f"{WORKED} worked through item by item, raw answer and points, in both conditions",
      "usability_items sheet, one row per participant per condition",
      "points per item as in the scoring key; the ten points are summed and multiplied by 2.5",
      t_worked,
      ("the worked totals equal the plotted scores",
       t_worked["manual_points"].sum() * 2.5 == usab.loc[WORKED, "manual"]
       and t_worked["augmented_reality_points"].sum() * 2.5
           == usab.loc[WORKED, "augmented_reality"]))

print("=" * 100)
print("SCORING KEY, how the negatively worded items are handled")
print(t_key.reset_index().to_string(index=False))
print(f"\nanswering the same value to all ten items scores: "
      + ", ".join(f"{v} -> {s}" for v, s in flat.items()))
print(f"best possible answer -> {best}   ·   worst possible answer -> {worst}")
print(f"\n{WORKED} worked through item by item")
print(t_worked.to_string(index=False))
print(f"  manual  {t_worked['manual_points'].sum():.0f} points x 2.5 = "
      f"{t_worked['manual_points'].sum() * 2.5}")
print(f"  AR      {t_worked['augmented_reality_points'].sum():.0f} points x 2.5 = "
      f"{t_worked['augmented_reality_points'].sum() * 2.5}")
print()

# --- F6, the comparative items -------------------------------------------------
t_comp = comp[C_COLS].T.copy()
t_comp.insert(0, "short_label", c_text.set_index("item")["short_label"])
t_comp.insert(1, "wording",     c_text.set_index("item")["wording"])
t_comp["answered_4_or_5"] = (comp[C_COLS] >= 4).sum().values
t_comp["answered_3"]      = (comp[C_COLS] == 3).sum().values
t_comp["answered_1_or_2"] = (comp[C_COLS] <= 2).sum().values
t_comp["median"]          = comp[C_COLS].median().values
raw_c = (c_items.set_index("participant")[C_COLS]
                .reindex(sorted(c_items["participant"])).astype(float))
audit("fig6_comparative_items", "fig_comparative_items",
      "every response on the ten comparative items, one cell per participant per item",
      "comparative_items sheet, columns c1 to c10",
      "no aggregation; the three band counts and the median are computed across the nine responses",
      t_comp.reset_index().rename(columns={"index": "item"}),
      ("every drawn cell equals the raw sheet",
       bool((comp[C_COLS].values == raw_c.values).all())),
      ("ninety responses drawn", comp[C_COLS].size == 90),
      ("bands account for every response",
       bool(((t_comp["answered_4_or_5"] + t_comp["answered_3"]
              + t_comp["answered_1_or_2"]) == len(comp)).all())))

# --- report --------------------------------------------------------------------
for key, (fig_name, draws, source, derivation, table) in AUDIT.items():
    print("=" * 100)
    print(f"{key}   ->   {fig_name}.png / .svg")
    print(f"  draws      : {draws}")
    print(f"  source     : {source}")
    print(f"  derivation : {derivation}")
    print(table.to_string(index=False))
    print()

print("=" * 100)
print("RECONCILIATION AGAINST THE RAW SHEETS")
for label, ok in CHECKS:
    print(f"  {'PASS' if ok else 'FAIL'}  {label}")
failed = [l for l, ok in CHECKS if not ok]
assert not failed, f"audit failed: {failed}"

# --- write the audit workbook --------------------------------------------------
AUDIT_FILE = "ARDPP_figure_audit.xlsx"
with pd.ExcelWriter(AUDIT_FILE) as xl:
    pd.DataFrame({
        "figure sheet": list(AUDIT),
        "figure file":  [f"{v[0]}.png / .svg" for v in AUDIT.values()],
        "draws":        [v[1] for v in AUDIT.values()],
        "raw source":   [v[2] for v in AUDIT.values()],
        "derivation":   [v[3] for v in AUDIT.values()],
    }).to_excel(xl, sheet_name="README", index=False)
    for key, (fig_name, draws, source, derivation, table) in AUDIT.items():
        # header block first, then the table, so the sheet is self-describing
        pd.DataFrame([["figure",     f"{fig_name}.png / .svg"],
                      ["draws",      draws],
                      ["raw source", source],
                      ["derivation", derivation],
                      ["checked",    "reconciled against the raw sheets on this run"]]
                     ).to_excel(xl, sheet_name=key, index=False, header=False, startrow=0)
        table.to_excel(xl, sheet_name=key, index=False, startrow=6)

print(f"\nwritten: {AUDIT_FILE}  ({len(AUDIT)} figure sheets, "
      f"{len(CHECKS)} checks, all passed)")
