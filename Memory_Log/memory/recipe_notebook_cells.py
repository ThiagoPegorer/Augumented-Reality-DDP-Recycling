# =============================================================================
#  ReCiPe 2016 SECTION — notebook cells, ready to paste after cell 56
#  Written to match the conventions already established in LCA_explorer.ipynb:
#  SC_COLOR / SC_NAME / SCEN, BLUE-AMBER-GREEN for identities, save(), wrap(), sig().
#
#  DESIGN PRINCIPLE — read before adapting:
#  Do NOT mirror the EF chapter. EF 3.1 answers "which categories matter and by
#  how much" (prioritisation). ReCiPe answers exactly ONE question: "does the
#  conclusion survive a different method?" (robustness). Robustness is a
#  COMPARATIVE question, so every chart below is comparative or aggregate — a
#  second set of standalone scenario bars would double the page count and imply a
#  second, contradictory prioritisation story.
# =============================================================================


# %% [markdown]
# # **Impact ReCiPe 2016 — method cross-check**
#
# EF 3.1 is the primary method and carries the prioritisation (§4.2 / §5).
# ReCiPe 2016 (openLCA LCIA pack; Huijbregts et al., 2017) is used **at
# characterised level only**, to test whether the scenario conclusions depend on
# the choice of impact assessment method. ReCiPe normalisation and weighting are
# deliberately NOT used for prioritisation — the reasoning, with numbers, is in
# framework §4.2.1 and is visualised in figure R3 below.


# %%
# ---- load the ReCiPe layer -------------------------------------------------
rec_mid = pd.read_csv(BASE / "impact_ReCiPe_mid.csv")
rec_aop = pd.read_csv(BASE / "impact_ReCiPe_end_aop.csv")
rec_scr = pd.read_csv(BASE / "impact_screening_ReCiPe_mid.csv")
ef      = pd.read_csv(BASE / "impact_EF31.csv")          # already loaded earlier; re-read is cheap

def red_pct(df, cat_col="category"):
    """Sc2/Sc3/Sc4 net reduction vs Sc1, in %, indexed by category."""
    d = df.set_index(cat_col)
    return pd.DataFrame({s: 100 * (d[f"{s}_net"] - d["sc1"]) / d["sc1"]
                         for s in ("sc2", "sc3", "sc4")})

# EF <-> ReCiPe category pairing. These are ANALOGOUS indicators, not identical
# ones: they share an environmental mechanism but differ in characterisation
# model and reference substance. That difference is the point of the comparison,
# so the pairing is declared explicitly rather than matched by fuzzy string.
PAIRS = [
    ("Climate change",                               "Global warming"),
    ("Resource use minerals and metals",             "Mineral resource scarcity"),
    ("Resource use fossils",                         "Fossil resource scarcity"),
    ("Acidification",                                "Terrestrial acidification"),
    ("Eutrophication freshwater",                    "Freshwater eutrophication"),
    ("Eutrophication marine",                        "Marine eutrophication"),
    ("Ecotoxicity freshwater",                       "Freshwater ecotoxicity"),
    ("Human toxicity cancer",                        "Human carcinogenic toxicity"),
    ("Human toxicity non-cancer",                    "Human non-carcinogenic toxicity"),
    ("Ionising radiation (human health)",            "Ionizing radiation"),
    ("Land use",                                     "Land use"),
    ("Ozone depletion",                              "Stratospheric ozone depletion"),
    ("Particulate matter",                           "Fine particulate matter formation"),
    ("Photochemical ozone formation (human health)", "Ozone formation, Human health"),
    ("Water use",                                    "Water consumption"),
]

ef_red, rec_red = red_pct(ef), red_pct(rec_mid)
cmp_ = pd.DataFrame(
    [{"ef_category": a, "recipe_category": b,
      "ef_unit":  ef.set_index("category").loc[a, "unit"],
      "rec_unit": rec_mid.set_index("category").loc[b, "unit"],
      "ef_sc4_pct":  ef_red.loc[a, "sc4"],
      "rec_sc4_pct": rec_red.loc[b, "sc4"]}
     for a, b in PAIRS])
cmp_["delta_pp"] = cmp_.rec_sc4_pct - cmp_.ef_sc4_pct
cmp_ = cmp_.sort_values("ef_sc4_pct")
print(f"mean |disagreement| = {cmp_.delta_pp.abs().mean():.1f} pp   "
      f"max = {cmp_.delta_pp.abs().max():.1f} pp "
      f"({cmp_.loc[cmp_.delta_pp.abs().idxmax(), 'ef_category']})")
cmp_.round(1)


# %% [markdown]
# ### R1 — Do the two methods agree? (the core robustness figure)
# One row per paired indicator, showing the Sc4 net reduction under each method.
# If the thesis conclusion is method-dependent, it shows up here as long
# connectors. This is the figure that answers the supervisor's cross-check request.

# %%
fig, ax = plt.subplots(figsize=(8, 5.2))
y = np.arange(len(cmp_))

# connector first, so the markers sit on top
ax.hlines(y, cmp_.ef_sc4_pct, cmp_.rec_sc4_pct, color="#c9d6e2", lw=2.4, zorder=1)
ax.scatter(cmp_.ef_sc4_pct,  y, s=42, color=BLUE,  zorder=3, label="EF 3.1")
ax.scatter(cmp_.rec_sc4_pct, y, s=42, color=AMBER, zorder=3, label="ReCiPe 2016 midpoint (H)")

# Call out minerals explicitly — it is the thesis-relevant indicator, and the gap
# is substantive rather than incidental (abiotic depletion in Sb-eq vs surplus ore
# in Cu-eq). Note this is NOT automatically the largest gap: marine eutrophication
# disagrees more, but on a category that carries no weight in the argument.
_m = cmp_.reset_index(drop=True)
_i = _m.index[_m.ef_category == "Resource use minerals and metals"][0]
_r = _m.loc[_i]
ax.annotate(f"{abs(_r.delta_pp):.1f} pp apart — scarcity modelled\ndifferently (Sb-eq vs Cu-eq)",
            (max(_r.ef_sc4_pct, _r.rec_sc4_pct), _i), (16, 6),
            textcoords="offset points", fontsize=7.5, color="#5a5a5a",
            va="center", ha="left",
            arrowprops=dict(arrowstyle="-", color="#b0b0b0", lw=.8))

ax.set_yticks(y); ax.set_yticklabels(cmp_.ef_category, fontsize=7.5)
ax.set_xlabel("Sc4 net reduction vs Sc1 (%)")
ax.set_title("R1 · Method agreement on the Sc4 conclusion — EF 3.1 vs ReCiPe 2016")
ax.legend(loc="upper left", fontsize=8)
ax.grid(axis="y", visible=False)
fig.tight_layout(); save(fig, "p3_01_method_agreement"); plt.show()


# %% [markdown]
# ### R2 — Damage view: the three areas of protection
# ReCiPe 2016 Endpoint reports damage in DALY, species·yr and USD2013. These are
# summed within their native units and reported **as they are** — no
# normalisation, no weighting, no single score (framework §4.2.1). Each panel has
# its own axis because the units are not commensurable; that is the honest form.

# %%
fig, axes = plt.subplots(1, 3, figsize=(9, 3.4))
for ax, (_, r) in zip(axes, rec_aop.iterrows()):
    vals = [r["sc1"], r["sc2_net"], r["sc3_net"], r["sc4_net"]]
    bars = ax.bar(range(4), vals, .72, color=[SC_COLOR[s] for s in SCEN])
    for i, (b, v) in enumerate(zip(bars, vals)):
        if i == 0: continue                      # Sc1 absolute goes in the title
        ax.annotate(f"{100*(v-vals[0])/vals[0]:+.0f} %",
                    (b.get_x() + b.get_width()/2, v), (0, 3),
                    textcoords="offset points", ha="center", fontsize=7.5)
    ax.set_xticks(range(4)); ax.set_xticklabels(["Sc1", "Sc2", "Sc3", "Sc4"], fontsize=8)
    ax.set_title(f"{r['area_of_protection']}\nSc1 = {vals[0]:.3g} {r['unit']}", fontsize=9)
    ax.set_ylim(0, max(vals) * 1.18)
    ax.ticklabel_format(axis="y", style="sci", scilimits=(-2, 3))
fig.suptitle("R2 · ReCiPe 2016 endpoint — damage per area of protection (characterised)",
             x=0.01, ha="left", fontsize=10, fontweight="bold")
fig.tight_layout(); save(fig, "p3_02_endpoint_aop"); plt.show()


# %% [markdown]
# ### R3 — Why normalisation was not used for prioritisation
# The methodology figure. Both panels rank the same inventory; the two methods
# select **completely disjoint** reporting sets. Under ReCiPe, mineral resource
# scarcity — the subject of this thesis — scores 0.0 %, because the VCU's
# 2.2 kg Cu-eq is negligible against a per-capita reference of 1.2 × 10⁵ kg Cu-eq.
# That is a property of ReCiPe's reference inventory, not of the device.

# %%
ef_s  = pd.read_csv(BASE / "impact_screening.csv").sort_values("share_pct", ascending=False)
rec_s = rec_scr.sort_values("share_pct", ascending=False)
for d, c in ((ef_s, "in_reporting_set"), (rec_s, "in_reporting_set")):
    if d[c].dtype == object:
        d[c] = d[c].astype(str).str.strip().str.lower() == "true"

TOP = 8                                   # bars shown per panel
PANELS = [
    (ef_s,  "Resource use minerals and metals", "EF 3.1 — normalised + weighted",
     "share of weighted footprint (%)"),
    (rec_s, "Mineral resource scarcity",        "ReCiPe 2016 — normalised only",
     "share of normalised impact (%)"),
]
fig, axes = plt.subplots(1, 2, figsize=(10, 4.6))
for ax, (full, key, ttl, ylab) in zip(axes, PANELS):
    d = full.head(TOP).reset_index(drop=True)
    x = np.arange(len(d))
    ax.bar(x, d.share_pct, .7, color=[BLUE if k else "#c9d6e2" for k in d.in_reporting_set])
    for xi, v in zip(x, d.share_pct):
        ax.annotate(f"{v:.1f}", (xi, v), (0, 3), textcoords="offset points",
                    ha="center", fontsize=7)
    ax.set_xticks(x)
    ax.set_xticklabels([wrap(c, 18) for c in d.category], rotation=38, ha="right", fontsize=6.8)
    ax.set_title(ttl); ax.set_ylabel(ylab); ax.set_ylim(0, 82)

    # Flag the thesis category. Highlighting the tick label avoids the arrow/title
    # collisions that come with annotating inside a panel this dense.
    hit = d.index[d.category == key]
    if len(hit):                                    # visible in the top-N
        t = ax.get_xticklabels()[hit[0]]
        t.set_color(AMBER); t.set_fontweight("bold")
        ax.annotate("← the thesis category", (hit[0] + 0.45, d.share_pct.iloc[hit[0]] * 0.92),
                    fontsize=7.5, color=AMBER, fontweight="bold", ha="left", va="center")
    else:                                           # ranks outside the top-N
        rank = int(full.reset_index(drop=True).index[full.reset_index(drop=True).category == key][0]) + 1
        share = float(full.loc[full.category == key, "share_pct"].iloc[0])
        ax.text(0.97, 0.72,
                f"the thesis category\nranks {rank} of {len(full)} here\n({share:.3f} %)",
                transform=ax.transAxes, ha="right", va="top",
                fontsize=8, color=AMBER, fontweight="bold")

fig.suptitle("R3 · The same inventory, two value systems — disjoint reporting sets",
             x=0.01, ha="left", fontsize=10, fontweight="bold")
fig.tight_layout(rect=(0, 0, 1, 0.95))
save(fig, "p3_03_normalisation_disjoint"); plt.show()


# %% [markdown]
# ### R4 *(optional, appendix)* — all 18 ReCiPe midpoint categories
# The same heat-map grammar as the EF chapter, confirming that the ordering
# Sc1 > Sc2 > Sc3 > Sc4 holds in **every** ReCiPe category, not only the paired ones.

# %%
H = rec_red.loc[rec_mid.set_index("category").index]        # keep CSV order
fig, ax = plt.subplots(figsize=(5.6, 6.4))
im = ax.imshow(H.values, cmap="Blues_r", aspect="auto", vmin=H.values.min(), vmax=0)
ax.set_xticks(range(3)); ax.set_xticklabels([SC_NAME[s] for s in SCEN[1:]],
                                            rotation=20, ha="right", fontsize=7.5)
ax.set_yticks(range(len(H))); ax.set_yticklabels(H.index, fontsize=7)
for i in range(H.shape[0]):
    for j in range(H.shape[1]):
        v = H.values[i, j]
        ax.text(j, i, f"{v:.0f}", ha="center", va="center", fontsize=6.8,
                color="white" if v < H.values.min() * 0.45 else "#1a1a1a")
ax.set_title("R4 · ReCiPe 2016 midpoint — net reduction vs Sc1 (%), all 18 categories")
ax.grid(False)
fig.colorbar(im, ax=ax, shrink=.6, label="reduction vs Sc1 (%)")
fig.tight_layout(); save(fig, "p3_04_recipe_reduction_heatmap"); plt.show()


# %% [markdown]
# ### T1 — Method comparison table (for the results chapter)

# %%
T1 = cmp_.assign(
        EF=lambda d: d.ef_sc4_pct.round(1).astype(str) + " %",
        ReCiPe=lambda d: d.rec_sc4_pct.round(1).astype(str) + " %",
        Δ=lambda d: d.delta_pp.round(1).astype(str) + " pp",
     )[["ef_category", "ef_unit", "EF", "recipe_category", "rec_unit", "ReCiPe", "Δ"]]
T1.columns = ["EF 3.1 category", "unit", "Sc4 red.",
              "ReCiPe 2016 category", "unit", "Sc4 red.", "difference"]
T1.to_csv(FIGS.parent / "table_T1_method_comparison.csv", index=False)
T1
