# 5.2 Digital Product Passport and AR prototype discussion, draft v2 (Session 43, 2026-08-26)

Thiago's own version, with three corrections: the data-storage paragraph rewritten for grammar, the
physical-model paragraph restored with its premise (the glued joints), and the closing claim changed
from an asserted outcome to a stated aim.

PROSE. At risk for affidavit denoting.

---

The passport structure is satisfiable. What the record could not supply was the values. Those values
come from four different places, and the record states which. Certificates and safety carries
manufacturer data taken from the reference product documentation, including the conformity marking
and the two declared substances of very high concern. Product specifications mixes the reference
product with the built prototype, since the manufacturer, model and type come from the datasheet
while the serial number and production date belong to the unit made for this study. Usage and
service, and the repair history with it, are assumptions made for an electric passenger car.
Environmental impact is modelled output from the life cycle assessment. All fifteen components carry
their material data as assumed, and none of it is verified. Every field declares its own basis, so a
reader can see which kind of value they are looking at instead of guessing.

The limitations of the passport also include data storage and management. Determining the best way to
store the record, so that multiple stakeholders can reach it, was not in the scope of this work. The
data is served from a simple JSON file on a local host. For this application a local file proved
sufficient, but scaling the approach would require established data storage practices.

The main limitation of the physical model is that it is not a replica of the reference unit. It is a
generic teardown model inspired by the Bosch MS 50.4, printed in PETG, and it holds no electronic
components. It has no glued joints, no conformal coating, and no fastener behavior taken from a real
device. Peitzmeier et al. (2025) found that two non-demountable glued joints were enough to prevent
an automotive control unit from being reused or refurbished as a spare part. The model therefore
removes the barrier that study identifies as decisive. What participants opened was a device designed
to come apart, so the study measures how well the interface communicates a procedure, and not how
hard the work is on a real unit.

The prototype RBv2.1.1 is an Augmented Reality guided assistant for disassembly, built on a Digital
Product Passport interface. This version is a minimum viable product rather than a tool for a working
recycler. It shows what a later version could do: give an end-of-life stakeholder an understanding of
the product before it is opened, guide that person through the disassembly steps, and highlight the
components whose material composition carries the highest environmental impact. Whether doing so
changes recovery in practice is a question this study does not answer.

---

## Open flags

- `[CITATION NEEDED: page locator, Peitzmeier et al. (2025)]` for the glued-joint finding. Chapter 1
  cites the same finding without a page.
- ✅ RESOLVED: the earlier draft quoted the 22-attribute coverage scoreboard. Thiago removed the
  counts, so no number enters the Discussion that Methodology does not already publish.
- 🔴 **Chapter 1 must be fixed before this section is pasted.** 1.3 still says the teardown model
  "reproduces the geometry", which contradicts the third paragraph here. Replacement wording is in
  `thesis_review_backlog_2026-08-26.md`, item A1.
- Naming RBv2.1.1 here is consistent with 4.2, which reports the build by version. The
  no-version rule applies to the study sections.
