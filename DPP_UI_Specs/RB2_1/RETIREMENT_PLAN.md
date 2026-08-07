# RB2.0 / RB2.1 retirement plan — what can go, what cannot, in what order

> Written 2026-08-06 after the verifier reported five unset `ScreenRouter` references and
> Thiago asked to *"start to clean the 2.0 and 2.1 legacy pages… otherwise this will keep
> accumulating and generating issues."*
>
> **Nothing in this document has been deleted yet.** It is the order in which deletion is safe.

---

## 0. Why the verifier fired — it was not accumulation

`RBv2_1/1 — Panel canvas + router` is **destructive by design**: it deletes `DPPPanelCanvas` and
every screen under it. Re-running the whole chain therefore wipes the four disassembly screens,
because their phases (`RBv2_0/4`, `/5`, `/6`) were not in the list I gave.

**Fix right now:** run `RBv2_0/4`, `RBv2_0/5`, `RBv2_0/6`, then verify again.

`modelExploration` is the only one of the five that is genuinely dead — retired from the map.

## 1. The trap: the legacy file holds shared helpers

`DPPUIBuilder.DppCanva.cs` (1177 lines, RB2.0) defines **`DestroyChild`**, which
`DPPUIBuilder.DppPage.cs`, `DPPUIBuilder.ProductSpecs.cs` and `DPPUIBuilder.Stakeholder.cs` all
call. Deleting the file breaks the RB2.1.1 build immediately.

**So step one of any cleanup is a MOVE, not a delete:** lift `DestroyChild` (and `MakeScrollWindow`,
if `04a`'s scroll list wants it) into `DPPUIBuilder.cs` beside `Stretch`/`TL`/`AddImage`. Only then
does the legacy file become removable.

⚠ This is the general shape of the problem. Files were split by SCREEN, so shared code ended up
wherever it was first needed. Expect one or two more of these per deletion.

## 2. Status of every screen

| Screen | Phase | State | Verdict |
|---|---|---|---|
| Welcome + first run | `RBv2_1/3` | live | keep |
| QR scan | `RBv2_1/2` | live | keep |
| Stakeholder decision | `RBv2_1/7` | live | keep |
| **Super panel rig** | `RBv2_1_1/1` | live | keep |
| **Product specs tab** | `RBv2_1/9` → `/2` | live | keep |
| Disassembly intro | `RBv2_0/4` | **live** — the Recycler's teardown | keep until `04b` |
| Step flow 1–5 | `RBv2_0/5` | **live** | keep until `04b` |
| Completion summary | `RBv2_0/6` | **live** | keep until `08` |
| Exploded canvas | `RBv2_0/5` | **live** | keep until `04b` |
| DPP page, flat v1 | `RBv2_1/8` | superseded | **rollback insurance — keep until the super panel is signed off** |
| Model exploration | `RBv2_0/Legacy` | **dead** | retire (wave 1) |
| DPP Canva, RB2.0 | `RBv2_0/7` | **dead** | retire (wave 1) |
| Info tab | `RBv2_0` | **dead** | retire (wave 1) |
| Main page | RBv1.0 | **dead** | retire (wave 1) |

**The disassembly chain is legacy in NUMBERING ONLY.** `Continue to disassembly` runs straight
into it. Deleting it because it says `RBv2_0` would break the Recycler's entire second half.

## 3. Wave 1 — the genuinely dead (≈2 400 lines)

Safe once §1's move is done. All four are unreachable: nothing routes to them, and the super
panel's stage replaced the only one that had a route.

| File | Lines | Note |
|---|---|---|
| `Editor/DPPUIBuilder.DppCanva.cs` | 1177 | **move `DestroyChild` + `MakeScrollWindow` out first** |
| `Editor/DPPUIBuilder.InfoTab.cs` | 87 | |
| `Scripts/DDP/UI/PassportView.cs` | 813 | |
| `Scripts/DDP/UI/InfoTabView.cs` | 309 | |
| `Scripts/DDP/UI/InfoTabRouter.cs` | 44 | |
| `Scripts/DDP/UI/PassportRouter.cs` | 60 | |
| `Scripts/DDP/UI/MainPageView.cs` | 26 | RBv1.0 remnant |

Then, in this order:

1. `DPPManager` — drop `mainPage`, `infoTab`, `passport` fields and their `Populate` calls.
2. `ScreenRouter` — drop `modelExploration` and `ShowModelExploration()`.
3. `DPPUIBuilder.Verify.cs` — drop the `PassportView` / `InfoTabView` rows if present.
4. Scene — delete `DppCanva_RB2_0_legacy` by hand.
5. Run **`RBv2_0/Tools/Clean RBv1.0 leftovers`** — it exists precisely to strip serialized
   UnityEvents left pointing at deleted methods, which is the failure mode of steps 1–2.

⚠ **Order matters.** Deleting `ShowModelExploration()` before the legacy canva object is gone
leaves a serialized listener pointing at a missing method — the console reports it, but only when
the object is enabled, which may be never.

## 4. Wave 2 — after `04b` replaces the teardown

`DPPUIBuilder.StepFlow.cs`, `DPPUIBuilder.Intro.cs`, `DPPUIBuilder.Summary.cs` and their views.
Not before. These are 9 of the app's clickables and the whole second half of the Recycler journey.

## 5. Wave 3 — after the super panel is signed off on a participant

`DPPUIBuilder.DppPage.cs` + `DppPageView.cs` (the flat v1 page). Keep until then: clearing
`ScreenRouter.dppSuperPanel` falls straight back to it, and that one-field rollback is worth more
than 460 lines of tidiness during a study.

## 6. Standing rule to stop the accumulation

**A screen is retired in the VERIFIER MAP on the day it stops being routed to, not on the day its
files are deleted.** The map is what tells you whether an unset reference is a fault or a fossil,
and the last three false alarms were all fossils. Deleting files can wait; correcting the map
cannot.

---

*Created 2026-08-06 · Nothing deleted yet · Companion to `INDEX.md` §1*
