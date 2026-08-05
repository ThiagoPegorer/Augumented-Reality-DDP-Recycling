using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine.UI;
using DPP.UI;

namespace DPP.EditorTools
{
    /// <summary>
    /// RBv2_1/7 — Screen 03: STAKEHOLDER DECISION (spec `03_stakeholder_decision.md`,
    /// mock `drafts/03_v4_stakeholder.svg`, approved 2026-08-04).
    ///
    /// Sits between the QR scan and the DPP Canva: the app asks who is using it,
    /// and the answer decides whether the disassembly route is offered at all.
    ///
    /// Built as a CHILD SCREEN of DPPPanelCanvas (like DppCanva and
    /// ModelExploration), not as its own root canvas — ScreenRouter already owns
    /// show/hide for the panel screens, and the role has to survive the hop to the
    /// DPP Canva, which lives on the same canvas.
    ///
    /// Safe to re-run: destroys and rebuilds only "StakeholderDecision".
    /// </summary>
    public static partial class DPPUIBuilder
    {
        // Panel-local geometry, straight off the approved mock.
        private const float StkCardW = 290f, StkCardH = 170f, StkCardY = 134f;
        private const float StkIcon = 48f;      // 40 was too small: the recycling
                                                // arrows close up below ~44 px at
                                                // panel scale (same call as the 96 px logo)
        private const float StkPad = 20f, StkGap = 20f;
        private const float StkTop = 33f;       // (170 - (48 + 20 + 2*18)) / 2 — content centred

        [MenuItem("RBv2_1/7 — Stakeholder decision", false, 7)]
        public static void Build8_StakeholderDecision()
        {
            DPPSpriteFactory.GenerateAll();
            ResolveFonts();

            var canvasGO = GameObject.Find("DPPPanelCanvas");
            if (canvasGO == null)
            {
                Debug.LogError("[DPPUIBuilder] DPPPanelCanvas not found — run RBv2_1/1 first.");
                return;
            }
            var canvasRT = (RectTransform)canvasGO.transform;
            var router = canvasGO.GetComponent<ScreenRouter>();
            if (router == null)
                Debug.LogWarning("[DPPUIBuilder] No ScreenRouter on DPPPanelCanvas — the cards will not route.");

            DestroyChild(canvasRT, "StakeholderDecision");   // also clears the old CloseAppButton

            var screen = Stretch("StakeholderDecision", canvasRT);
            Undo.RegisterCreatedObjectUndo(screen.gameObject, "Build Stakeholder Decision");
            var select = screen.gameObject.AddComponent<StakeholderSelect>();
            AddImage(Stretch("PanelBG", screen), DPPSpriteFactory.RoundedR22, DPPTheme.NavyPanel, sliced: true);

            // Header: title centred in the band between the panel edge and the rule.
            AddText(TLCenter("Title", screen, 320, 38, 600, 40), "Select your role", 25,
                DPPTheme.TextOnNavy, bold: true, align: TextAlignmentOptions.Center);
            AddImage(TL("Separator", screen, 24, 76, 592, 1), null, DPPTheme.Hex("#1a335f"));

            // LEFT = Product user, RIGHT = Recycler (Thiago, 2026-08-04 — the
            // routine diagram has these the other way round; the screen wins).
            var userBtn = BuildRoleCard(screen, "ProductUserCard", 24f, "ic_product_user",
                "Product user",
                "Digital Product Passport access only – product data, materials, impact and more");
            var recBtn = BuildRoleCard(screen, "RecyclerCard", 326f, "ic_recycler",
                "Recycler",
                "Digital Product Passport access and Dismantling assist steps");

            // Destructive pill in the standard LEFT slot — identical geometry to the
            // Welcome canvas' Close app, so the hit target never moves (00 §5).
            var quitBtn = BuildPillButton(screen, "QuitButton", cx: 114, cy: 376, w: 180, h: 52,
                label: "Quit", labelSize: 16, primary: false, chevron: false, destructive: true);

            // ---- wiring ----
            SetRef(select, "router", router);
            SetRef(select, "welcome", Object.FindFirstObjectByType<WelcomeController>(FindObjectsInactive.Include));
            WireClick(userBtn, select, nameof(StakeholderSelect.ChooseProductUser));
            WireClick(recBtn, select, nameof(StakeholderSelect.ChooseRecycler));
            WireClick(quitBtn, select, nameof(StakeholderSelect.Quit));
            if (router != null) SetRef(router, "stakeholderDecision", screen.gameObject);

            screen.gameObject.SetActive(false);

            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            Debug.Log("[DPPUIBuilder] RBv2_1/7 — Stakeholder decision built. " +
                      "A successful scan now opens this screen instead of the passport directly. Save the scene.");
        }

        /// <summary>
        /// One role card: 290 x 170, icon + title on a row, description wrapping the
        /// full inner width below. THE WHOLE CARD IS THE BUTTON — chrome is correct
        /// here and required, because under the chrome = touchable rule (00 §4) a
        /// bordered card promises tappability, and this one delivers.
        /// </summary>
        private static Button BuildRoleCard(RectTransform parent, string name, float x,
            string iconAsset, string title, string description)
        {
            var card = TL(name, parent, x, StkCardY, StkCardW, StkCardH);

            var outline = AddImage(CenterIn("HoverOutline", card, StkCardW + HoverHalo, StkCardH + HoverHalo),
                DPPSpriteFactory.RoundedR13, Color.white, sliced: true);
            outline.gameObject.SetActive(false);                       // hover only (00 §4)

            AddImage(CenterIn("Stroke", card, StkCardW + 2f, StkCardH + 2f),
                DPPSpriteFactory.RoundedR13, DPPTheme.Hex("#21407a"), sliced: true);
            var fill = AddImage(CenterIn("Fill", card, StkCardW, StkCardH),
                DPPSpriteFactory.RoundedR13, DPPTheme.RowFill, sliced: true, raycast: true);

            // Authored PNG, rendered as drawn — both icons are already green, so no tint.
            var iconRT = TL("Icon", card, StkPad, StkTop, StkIcon, StkIcon);
            var sprite = LoadTileIcon(iconAsset);
            if (sprite != null)
            {
                var img = iconRT.gameObject.AddComponent<Image>();
                img.sprite = sprite;
                img.preserveAspect = true;
                img.raycastTarget = false;
            }
            else
            {
                // A card must never render with an empty hole where art failed to
                // load: drop the icon and give the title the full width instead.
                Debug.LogWarning($"[DPPUIBuilder] Role icon '{iconAsset}' not found — card '{name}' drawn without one.");
                Object.DestroyImmediate(iconRT.gameObject);
            }

            float titleX = sprite != null ? StkPad + StkIcon + 16f : StkPad;
            AddText(TL("Title", card, titleX, StkTop + StkIcon * 0.5f - 10f, StkCardW - titleX - StkPad, 20),
                title, 16, DPPTheme.TextOnNavy, bold: true);

            // AddText defaults to NoWrap (every other label in the project is one
            // line). This block is the exception: it must wrap inside the card.
            var desc = AddText(TL("Description", card, StkPad, StkTop + StkIcon + StkGap, StkCardW - 2f * StkPad, 40),
                description, 13, DPPTheme.TextSecondary, bold: false, align: TextAlignmentOptions.TopLeft);
            desc.textWrappingMode = TextWrappingModes.Normal;

            var button = card.gameObject.AddComponent<Button>();
            button.transition = Selectable.Transition.None;
            button.targetGraphic = fill;

            var hover = card.gameObject.AddComponent<HoverHighlight>();
            SetRef(hover, "highlightOutline", outline.gameObject);
            SetRef(hover, "lift", card);

            return button;
        }
    }
}
