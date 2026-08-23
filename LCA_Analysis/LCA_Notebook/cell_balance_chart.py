# --- chart: gross burden and avoided impact on one axis, with the balance -----
# The supervisor's reporting convention: the gross burden is drawn ABOVE the
# zero line, the avoided impact BELOW it, and the two are never added into a
# single bar. The balance line is gross minus avoided, drawn as a line rather
# than a bar so it reads as a derived quantity and not as a third measurement.
# Sc1 recovers nothing, so it has no avoided impact and its balance equals its
# gross burden.
GROSS_C, AVOID_C, BAL_C = BLUE, AMBER, "#222222"

fig, axes = plt.subplots(1, len(REPORTING), figsize=(8.4, 3.9))

for ax, cat in zip(axes, REPORTING):
    r     = ef.loc[cat]
    gross = np.array([r["sc1"]] + [r[f"{sc}_gross"]  for sc in SCEN[1:]])
    avoid = np.array([0.0]      + [r[f"{sc}_saving"] for sc in SCEN[1:]])
    bal   = gross - avoid

    # The identity is asserted rather than trusted: the balance must reproduce
    # the model's own subtraction in every category.
    chk = np.array([r["sc1"]] + [r[f"{sc}_net"] for sc in SCEN[1:]])
    assert np.allclose(bal, chk, rtol=0, atol=1e-12), f"balance mismatch in {cat}"

    ax.bar(range(4),  gross, .62, color=GROSS_C, zorder=3)
    ax.bar(range(4), -avoid, .62, color=AVOID_C, zorder=3)
    ax.axhline(0, color="#555", lw=1.0, zorder=4)

    ax.plot(range(4), bal, color=BAL_C, lw=1.4, marker="o", ms=4.5,
            markerfacecolor="white", markeredgewidth=1.1, zorder=6)

    for x, (g, a, b) in enumerate(zip(gross, avoid, bal)):
        ax.annotate(sig(g, 3), (x, g), (0, 4), textcoords="offset points",
                    ha="center", fontsize=6.5, color="#444")
        if a > 0:
            ax.annotate(sig(a, 3), (x, -a), (0, -10), textcoords="offset points",
                        ha="center", fontsize=6.5, color="#7a5400")
        # Sc1 has no credit, so its balance equals its gross burden and the
        # label would print on top of the one above. Marker only for Sc1.
        if x > 0:
            ax.annotate(sig(b, 3), (x, b), (0, -11), textcoords="offset points",
                        ha="center", fontsize=6.5, color=BAL_C, fontweight="bold")

    ax.set_title(wrap(cat))
    ax.set_ylabel(UNIT[cat], fontsize=8, color="#666")
    ax.set_xticks(range(4))
    ax.set_xticklabels([s.replace("sc", "Sc") for s in SCEN])
    top, bot = gross.max(), avoid.max()
    ax.set_ylim(-bot * 1.55, top * 1.18)
    ax.grid(axis="x", visible=False)

handles = [plt.Rectangle((0, 0), 1, 1, color=GROSS_C),
           plt.Rectangle((0, 0), 1, 1, color=AVOID_C),
           plt.Line2D([0], [0], color=BAL_C, lw=1.4, marker="o", ms=4.5,
                      markerfacecolor="white")]
fig.legend(handles, ["gross burden", "avoided impact", "balance"],
           loc="lower center", ncol=3, bbox_to_anchor=(.5, -.08), fontsize=8)
fig.suptitle("Gross burden and avoided impact per scenario, EF 3.1",
             x=0.5, y=1.03, fontsize=10, fontweight="bold")
fig.tight_layout(w_pad=2.2)

save(fig, "ef31_balance_by_scenario")
plt.show()
