using UnityEngine;

namespace DPP.UI
{
    /// <summary>
    /// Single source of truth for the DPP UI color tokens and type scale.
    /// Mirrors DPP_UI_Specs/00_design_standards.md §2–§3. When the spec
    /// changes, change it here — every screen builder reads these values.
    ///
    /// CLEANUP 2026-07-30: removed three tokens spec 00 §2 already listed as
    /// retired and which nothing referenced — the light Informations card pair
    /// (switched to blue 2026-06-10) and the 3D canvas navy (the action zone has
    /// been transparent since v3).
    /// </summary>
    public static class DPPTheme
    {
        // ---- Surfaces ----
        public static readonly Color NavyPanel      = Hex("#0a1f44");
        public static readonly Color RowFill        = Hex("#0e2950");
        public static readonly Color CardBlue       = Hex("#13366b");
        public static readonly Color RowStroke      = Hex("#21407a");

        // ---- Tabs ----
        public static readonly Color TabActiveFill   = Hex("#0d2a57");
        public static readonly Color TabActiveStroke = Hex("#2e5aa0");
        public static readonly Color TabInactiveFill = Hex("#324a6d");
        public static readonly Color TabInactiveText = Hex("#c2cee0");

        // ---- Teal accents ----
        public static readonly Color TealAccent = Hex("#1d9e75");
        public static readonly Color TealLight  = Hex("#5dcaa5");
        public static readonly Color TealText   = Hex("#9fe1cb");
        public static readonly Color TealMuted  = Hex("#7fb89e");

        // ---- Safety ----
        public static readonly Color SafetyFill    = Hex("#3a1d22");
        public static readonly Color SafetyFillRow = Hex("#2a1d2e");
        public static readonly Color SafetyStroke  = Hex("#e24b4a");
        public static readonly Color SafetyText    = Hex("#f3b0b0");

        // ---- Gold (recoverable value) ----
        public static readonly Color GoldPartFill   = Hex("#3a2c12");
        public static readonly Color GoldPartStroke = Hex("#b7842f");

        // ---- Text ----
        public static readonly Color TextOnNavy        = Color.white;
        public static readonly Color TextOnGrey        = Hex("#0a1f44");
        public static readonly Color TextSecondary     = Hex("#9fb3d1");
        public static readonly Color TextLabel         = Hex("#8ba3c4");
        public static readonly Color TextCaption       = Hex("#7f9bc4");
        public static readonly Color TextTip           = Hex("#6f86a8");
        public static readonly Color TextSubtitleLight = Hex("#5a6b85"); // on grey card
        public static readonly Color TextSubtitleNavy  = Hex("#aac4e6"); // on blue card

        // ---- Scroll / progress ----
        public static readonly Color ScrollTrack = Hex("#16335f");

        // ---- Grabber bar ----
        public static readonly Color GrabberFill   = Hex("#0a0e16");
        public static readonly Color GrabberStroke = Hex("#2a3344");
        public static readonly Color GrabberGrip   = Hex("#6b7686");

        // ---- Buttons ----
        public static readonly Color SecondaryButtonFill = Hex("#1a2740");

        /// <summary>Parses "#rrggbb" (or "#rrggbbaa") into a Color. Falls back to magenta.</summary>
        public static Color Hex(string hex)
        {
            if (ColorUtility.TryParseHtmlString(hex, out Color c)) return c;
            Debug.LogWarning($"[DPPTheme] Could not parse color '{hex}'.");
            return Color.magenta;
        }
    }
}
