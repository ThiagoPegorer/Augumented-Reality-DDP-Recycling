using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DPP.UI;

namespace DPP.EditorTools
{
    /// <summary>
    /// Shared MODAL CHROME for the passport category pages — the back arrow +
    /// title header and the label/value field rows defined by spec 02 v3 §5.
    ///
    /// HISTORY (2026-07-30 cleanup): this file used to be the RBv1.0
    /// "Build Phase 2 — Information Tab" builder. That builder was DELETED
    /// because re-running it rebuilt the old `InformationTab` screen, which
    /// created a SECOND InfoTabView and re-pointed DPPManager.infoTab at it —
    /// leaving the RBv2.0 DPP Canva silently unpopulated. Removed with it:
    ///   · BuildPhase2            — the screen builder
    ///   · MakeTabHeader / BuildTabPill — the tab bar (cut in RBv2.0)
    ///   · BuildCategoryCards / MakeCategoryCard / CardParts — superseded by
    ///     MakeDppCard (RBv2_1_1/Legacy, 290×110 cards)
    ///   · BuildLcaModal          — the LCA is a screen now, not a modal
    ///
    /// What remains is used by RBv2_1_1/Legacy (DPPUIBuilder.DppCanva.cs) to build the
    /// four category modals, whose content is unchanged from spec 02 v3 §5.1.
    /// Suggested file name: RBv2_0_ModalChrome.cs.
    /// </summary>
    public static partial class DPPUIBuilder
    {
        private const float ModalContentX = 24f;   // content left edge
        private const float ModalContentW = 592f;  // content width (24 → 616)
        private const float ModalFieldStartY = 116f;
        private const float ModalFieldPitch = 34f;

        // =================================================================
        // Modal chrome (spec 02 v3 §5): back arrow + category icon + title.
        // The back circle sits at the same coordinates the RBv1.0 Home button
        // used, so the hand already knows the spot.
        // =================================================================
        private static RectTransform MakeModalPage(RectTransform screen, InfoTabRouter modalRouter,
            string name, string title, string iconSprite, Color iconColor)
        {
            var page = Stretch(name, screen);

            var back = TLCenter("BackButton", page, 42, 44, 40, 40);
            var backOutline = AddImage(CenterIn("HoverOutline", back, 46, 46), DPPSpriteFactory.Circle64, Color.white);
            backOutline.gameObject.SetActive(false);
            AddImage(CenterIn("Ring", back, 43, 43), DPPSpriteFactory.Circle64, DPPTheme.TabActiveStroke);
            var backFill = AddImage(CenterIn("Fill", back, 40, 40), DPPSpriteFactory.Circle64, DPPTheme.CardBlue, sliced: false, raycast: true);
            AddImage(CenterIn("Icon", back, 22, 22), DPPSpriteFactory.IcBack, Color.white);

            var backBtn = back.gameObject.AddComponent<Button>();
            backBtn.transition = Selectable.Transition.None;
            backBtn.targetGraphic = backFill;
            WireClick(backBtn, modalRouter, nameof(InfoTabRouter.Back));
            var backHover = back.gameObject.AddComponent<HoverHighlight>();
            SetRef(backHover, "highlightOutline", backOutline.gameObject);

            // No breadcrumb (removed after Editor test 2026-06-10) — title centers on the back button.
            AddImage(TLCenter("TitleIcon", page, 88, 44, 20, 20), iconSprite, iconColor);
            AddText(TL("Title", page, 102, 31, 440, 26), title, 19, DPPTheme.TextOnNavy, bold: true);
            AddImage(TL("Separator", page, 24, 76, 592, 1), null, DPPTheme.Hex("#1a335f"));

            page.gameObject.SetActive(false);
            return page;
        }

        /// <summary>Modal with simple label/value field rows. Wires each row's value into the view by field name.</summary>
        private static RectTransform BuildFieldModal(RectTransform screen, InfoTabRouter modalRouter,
            string name, string title, string iconSprite, Color iconColor,
            (string label, string demo, string viewField)[] rows, InfoTabView view,
            string tealValueField = null)
        {
            var page = MakeModalPage(screen, modalRouter, name, title, iconSprite, iconColor);

            for (int i = 0; i < rows.Length; i++)
            {
                float y = ModalFieldStartY + i * ModalFieldPitch;
                AddText(TL($"Label{i}", page, ModalContentX, y, 300, 20), rows[i].label, 13, DPPTheme.TextLabel, bold: false);
                var value = AddText(TL($"Value{i}", page, ModalContentX, y - 1, ModalContentW, 22), rows[i].demo, 13.5f,
                    rows[i].viewField == tealValueField ? DPPTheme.TealText : DPPTheme.TextOnNavy,
                    bold: false, align: TextAlignmentOptions.MidlineRight);
                SetRef(view, rows[i].viewField, value);
            }
            return page;
        }
    }
}
