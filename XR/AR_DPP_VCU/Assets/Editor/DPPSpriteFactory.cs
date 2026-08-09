using System.IO;
using UnityEditor;
using UnityEngine;

namespace DPP.EditorTools
{
    /// <summary>
    /// Generates the reusable UI sprites (rounded rects, circle, recycling
    /// glyph, capsule pill, grip) procedurally and saves them as PNGs under
    /// Assets/Textures/UI. Run via the DPP menu or implicitly by the screen
    /// builders. Idempotent — always regenerates, safe to re-run.
    ///
    /// All shapes are rendered white-on-transparent so the UI tints them via
    /// Image.color with the DPPTheme tokens.
    /// </summary>
    public static class DPPSpriteFactory
    {
        public const string SpriteDir = "Assets/Textures/UI";

        // Names used by the builders.
        public const string RoundedR22 = "ui_rounded_r22"; // 9-sliced — panel, hover outlines
        public const string RoundedR20 = "ui_rounded_r20"; // 9-sliced — choice cards
        public const string RoundedR13 = "ui_rounded_r13"; // 9-sliced — accordion rows
        public const string RoundedR3  = "ui_rounded_r3";  // 9-sliced — small bars (recovery, scrollbar)
        public const string Pill       = "ui_pill";        // capsule — grabber bar, tab pills
        public const string Grip       = "ui_grip";        // capsule — grip + chevron bars
        public const string Circle64   = "ui_circle_64";   // icon circles
        public const string CircleRing = "ui_circle_ring";  // RING — the "not provided" status dot
        public const string Recycle    = "ui_icon_recycle";// recycling glyph
        public const string FadeV      = "ui_fade_v";      // vertical fade (alpha 1 top → 0 bottom)

        // Stroke icons (white, transparent bg, tint via Image.color).
        public const string IcHouse   = "ui_ic_house";
        public const string IcPerson  = "ui_ic_person";
        public const string IcCube    = "ui_ic_cube";    // v8 - Mechanical data
        public const string IcBolt    = "ui_ic_bolt";    // v8 - Electrical data
        public const string IcPlus    = "ui_ic_plus";    // v8 - "open this page"
        public const string IcLayers  = "ui_ic_layers";
        public const string IcWarning = "ui_ic_warning";
        public const string IcShield  = "ui_ic_shield";
        public const string IcLeaf    = "ui_ic_leaf";
        public const string IcChevron = "ui_ic_chevron";   // points DOWN; rotate 180 for up, +90 for right
        public const string IcBack    = "ui_ic_back";      // ← arrow (modal back button)
        public const string IcWrench  = "ui_ic_wrench";    // Service & repair tile (spec 13 v2)

        // Step action icons (v0.4 keywords, spec 04 §8). "recycle" maps to Recycle.
        public const string IcCross   = "ui_ic_cross";
        public const string IcUp      = "ui_ic_up";
        public const string IcPins    = "ui_ic_pins";
        public const string IcUsb     = "ui_ic_usb";
        public const string IcLever   = "ui_ic_lever";
        public const string IcBoard   = "ui_ic_board";
        public const string IcMagnify = "ui_ic_magnify";
        public const string IcChip    = "ui_ic_chip";
        public const string IcLabel   = "ui_ic_label";
        public const string IcClock   = "ui_ic_clock";    // completion summary time card
        public const string IcStar    = "ui_ic_star";     // gold recovery cards
        public const string IcCheck   = "ui_ic_check";    // done header

        [MenuItem("RBv2_1_1/Tools/Generate UI sprites", false, 102)]
        public static void GenerateAll()
        {
            Directory.CreateDirectory(SpriteDir);

            MakeRoundedRectSliced(RoundedR22, 22);
            MakeRoundedRectSliced(RoundedR20, 20);
            MakeRoundedRectSliced(RoundedR13, 13);
            MakeRoundedRectSliced(RoundedR3, 3);
            MakeRoundedRectExact(Pill, 400, 44, 22);   // displayed at 200x22 → r11
            MakeRoundedRectExact(Grip, 176, 16, 8);    // displayed at 44x4  → r2
            MakeCircle(Circle64, 64);
            MakeRecycleIcon(Recycle, 128);
            MakeVerticalFade(FadeV);

            MakeStrokeIcon(IcHouse, 2.0f,
                Poly(-7,1, 0,-6, 7,1), Poly(-5,0, -5,7, 5,7, 5,0));
            MakeStrokeIcon(IcPerson, 2.0f,
                CirclePts(0,-4.5f,3.6f), ArcPts(0,9,6,7,180,0)); // shoulders: upper semi-ellipse
            MakeStrokeIcon(IcLayers, 2.0f,
                Poly(-5,-5, 5,-5, 5,5, -5,5, -5,-5), Poly(-2,-1, 2,-1), Poly(-2,2, 2,2));
            MakeStrokeIcon(IcWarning, 2.0f,
                Poly(0,-8, 8,6, -8,6, 0,-8), Poly(0,-2, 0,1.5f), Poly(0,4.4f, 0,4.5f));
            MakeStrokeIcon(IcShield, 2.0f,
                Poly(-5,-6, 5,-6, 5,6, -5,6, -5,-6), Poly(-2,0, 0,2, 3,-3));
            // "Life cycle" glyph: two opposing circular arrows (clearer at 24 px
            // than the previous arc+arrow which read as a blob).
            MakeStrokeIcon(IcLeaf, 2.0f,
                ArcPts(0,0,7,7,150,30),  Poly(6.1f,-3.5f, 6.1f,-6.3f),  Poly(6.1f,-3.5f, 3.7f,-4.9f),
                ArcPts(0,0,7,7,330,210), Poly(-6.1f,3.5f, -6.1f,6.3f),  Poly(-6.1f,3.5f, -3.7f,4.9f));
            MakeStrokeIcon(IcChevron, 2.2f,
                Poly(-7,-3.5f, 0,3.5f, 7,-3.5f));
            MakeStrokeIcon(IcBack, 2.0f,
                Poly(3,-6, -4,0, 3,6), Poly(-4,0, 8,0));
            // Spanner: diagonal handle + open C head. The gap faces the +x side so the
            // head still reads as open at the 22 px the tiles draw it at.
            MakeStrokeIcon(IcWrench, 2.0f,
                Poly(-7.5f,7, 0.5f,-1), ArcPts(3.5f,-4, 4.4f,4.4f, 55, 325));
            // v8 glyphs. Cube = isometric box (mechanical), bolt = electrical, plus =
            // "there is a page behind this". Drawn, not typed: 00 §3 keeps text to the
            // SF Pro SDF atlas and a UI affordance should not depend on a font glyph.
            MakeStrokeIcon(IcCube, 1.8f,
                Poly(-7,-3, 0,-7, 7,-3, 0,1, -7,-3), Poly(-7,-3, -7,4), Poly(7,-3, 7,4),
                Poly(-7,4, 0,8, 7,4), Poly(0,1, 0,8));
            MakeStrokeIcon(IcBolt, 1.8f,
                Poly(2,-8, -4,1, 0,1, -2,8, 4,-1, 0,-1, 2,-8));
            MakeStrokeIcon(IcPlus, 2.4f,
                Poly(0,-6.5f, 0,6.5f), Poly(-6.5f,0, 6.5f,0));
            // Hollow status dot. A ring must be its own sprite: Circle64 is a plain
            // disc with no 9-slice border, so Image.fillCenter=false renders nothing.
            MakeStrokeIcon(CircleRing, 2.6f, CirclePts(0,0,7.2f));

            // Step action icons (scaled to read inside r15 card circles).
            MakeStrokeIcon(IcCross, 2.2f,
                Poly(-5,-5, 5,5), Poly(5,-5, -5,5));
            MakeStrokeIcon(IcUp, 2.2f,
                Poly(0,7, 0,-6), Poly(-5,-1, 0,-6, 5,-1));
            MakeStrokeIcon(IcPins, 2.0f,
                CirclePts(-5,0,3f), CirclePts(2.5f,-4,3f), CirclePts(2.5f,4,3f));
            MakeStrokeIcon(IcUsb, 2.0f,
                Poly(-6,-5, 6,-5, 6,3, -6,3, -6,-5), Poly(-2.5f,3, -2.5f,7), Poly(2.5f,3, 2.5f,7));
            MakeStrokeIcon(IcLever, 2.2f,
                Poly(-6,5, 6,-5), Poly(-6,5, -1,6.2f));
            MakeStrokeIcon(IcBoard, 2.0f,
                Poly(-7,-5, 7,-5, 7,5, -7,5, -7,-5), Poly(-3.5f,-5, -3.5f,5), Poly(0,-5, 0,5), Poly(3.5f,-5, 3.5f,5));
            MakeStrokeIcon(IcMagnify, 2.0f,
                CirclePts(-1.5f,-1.5f,4.6f), Poly(2.2f,2.2f, 7,7));
            MakeStrokeIcon(IcChip, 1.8f,
                Poly(-4.5f,-4.5f, 4.5f,-4.5f, 4.5f,4.5f, -4.5f,4.5f, -4.5f,-4.5f),
                Poly(-4.5f,-2, -7.5f,-2), Poly(-4.5f,2, -7.5f,2), Poly(4.5f,-2, 7.5f,-2), Poly(4.5f,2, 7.5f,2),
                Poly(-2,-4.5f, -2,-7.5f), Poly(2,-4.5f, 2,-7.5f), Poly(-2,4.5f, -2,7.5f), Poly(2,4.5f, 2,7.5f));
            MakeStrokeIcon(IcLabel, 2.0f,
                Poly(-6,-6, 6,-6, 6,2, 2,6, -6,6, -6,-6), Poly(6,2, 2,2, 2,6));
            MakeStrokeIcon(IcClock, 1.8f,
                CirclePts(0,0,8f), Poly(0,-4.5f, 0,0, 3,2.2f));
            MakeStrokeIcon(IcStar, 1.8f,
                Poly(0,-8, 2.4f,-2.6f, 8,-2.2f, 3.8f,1.6f, 5,7.4f, 0,4.2f, -5,7.4f, -3.8f,1.6f, -8,-2.2f, -2.4f,-2.6f, 0,-8));
            MakeStrokeIcon(IcCheck, 2.6f,
                Poly(-7,0, -2,5, 7,-5));

            AssetDatabase.Refresh();
            Debug.Log($"[DPPSpriteFactory] UI sprites generated in {SpriteDir}.");
        }

        // ---- icon path helpers (glyph space ~±10, y-down like SVG) ----

        private static Vector2[] Poly(params float[] xy)
        {
            var pts = new Vector2[xy.Length / 2];
            for (int i = 0; i < pts.Length; i++) pts[i] = new Vector2(xy[2 * i], xy[2 * i + 1]);
            return pts;
        }

        private static Vector2[] CirclePts(float cx, float cy, float r)
        {
            var pts = new Vector2[25];
            for (int i = 0; i <= 24; i++)
            {
                float a = i / 24f * Mathf.PI * 2f;
                pts[i] = new Vector2(cx + r * Mathf.Cos(a), cy + r * Mathf.Sin(a));
            }
            return pts;
        }

        /// <summary>Elliptical arc, angles in degrees (0° = +x, increasing sweeps through -y i.e. visually over the top in y-down glyph space).</summary>
        private static Vector2[] ArcPts(float cx, float cy, float rx, float ry, float fromDeg, float toDeg)
        {
            const int N = 24;
            var pts = new Vector2[N + 1];
            for (int i = 0; i <= N; i++)
            {
                float a = Mathf.Lerp(fromDeg, toDeg, i / (float)N) * Mathf.Deg2Rad;
                pts[i] = new Vector2(cx + rx * Mathf.Cos(a), cy - ry * Mathf.Sin(a));
            }
            return pts;
        }

        /// <summary>Rasterizes round-capped polylines (glyph space ~±10) into a 96px white icon (4×4 supersampled).</summary>
        private static void MakeStrokeIcon(string name, float strokeWidth, params Vector2[][] lines)
        {
            const int size = 96;
            const float scale = 4.0f;
            float half = size * 0.5f;
            float rPx = strokeWidth * 0.5f * scale;

            var tex = NewTexture(size, size);
            for (int y = 0; y < size; y++)
            for (int x = 0; x < size; x++)
            {
                float cov = 0f;
                for (int sy = 0; sy < 4; sy++)
                for (int sx = 0; sx < 4; sx++)
                {
                    float px = x + (sx + 0.5f) / 4f;
                    float py = y + (sy + 0.5f) / 4f;
                    // texture (row 0 = bottom) → glyph space (y-down), same mapping
                    // as MakeRecycleIcon: top rows ↔ negative glyph y.
                    var p = new Vector2((px - half) / scale, (half - py) / scale);

                    float d = float.MaxValue;
                    for (int l = 0; l < lines.Length; l++)
                    {
                        var pts = lines[l];
                        for (int i = 0; i < pts.Length - 1; i++)
                            d = Mathf.Min(d, DistToSegment(p, pts[i], pts[i + 1]));
                    }
                    if (d * scale <= rPx - 0.25f) cov += 1f;
                }
                tex.SetPixel(x, y, new Color(1f, 1f, 1f, cov / 16f));
            }
            tex.Apply();
            SavePng(name, tex, Vector4.zero);
        }

        private static float DistToSegment(Vector2 p, Vector2 a, Vector2 b)
        {
            Vector2 ab = b - a;
            float len2 = ab.sqrMagnitude;
            float t = len2 < 1e-6f ? 0f : Mathf.Clamp01(Vector2.Dot(p - a, ab) / len2);
            return Vector2.Distance(p, a + t * ab);
        }

        /// <summary>4×64 white texture, alpha 1 at top → 0 at bottom. Tint navy, flip for bottom fade.</summary>
        private static void MakeVerticalFade(string name)
        {
            var tex = NewTexture(4, 64);
            for (int y = 0; y < 64; y++)
            {
                float a = y / 63f; // row 0 = bottom = transparent... row 63 = top = opaque
                for (int x = 0; x < 4; x++) tex.SetPixel(x, y, new Color(1f, 1f, 1f, a));
            }
            tex.Apply();
            SavePng(name, tex, Vector4.zero);
        }

        public static Sprite Load(string name) =>
            AssetDatabase.LoadAssetAtPath<Sprite>($"{SpriteDir}/{name}.png");

        // ---------------------------------------------------------------
        // Shape renderers (4x4 supersampled coverage → smooth edges)
        // ---------------------------------------------------------------

        /// <summary>Rounded rect filling the whole texture; imported 9-sliced with border = radius+2.</summary>
        private static void MakeRoundedRectSliced(string name, int radius)
        {
            int size = 2 * (radius + 2) + 16;
            var tex = RenderRoundedRect(size, size, radius);
            SavePng(name, tex, new Vector4(radius + 2, radius + 2, radius + 2, radius + 2));
        }

        /// <summary>Rounded rect at an exact aspect ratio, used as a simple (non-sliced) stretched sprite.</summary>
        private static void MakeRoundedRectExact(string name, int w, int h, int radius)
        {
            var tex = RenderRoundedRect(w, h, radius);
            SavePng(name, tex, Vector4.zero);
        }

        private static Texture2D RenderRoundedRect(int w, int h, int radius)
        {
            var tex = NewTexture(w, h);
            float hw = w * 0.5f, hh = h * 0.5f;
            float rx = hw - radius, ry = hh - radius;

            for (int y = 0; y < h; y++)
            for (int x = 0; x < w; x++)
            {
                float cov = 0f;
                for (int sy = 0; sy < 4; sy++)
                for (int sx = 0; sx < 4; sx++)
                {
                    float px = x + (sx + 0.5f) / 4f - hw;
                    float py = y + (sy + 0.5f) / 4f - hh;
                    float qx = Mathf.Max(Mathf.Abs(px) - rx, 0f);
                    float qy = Mathf.Max(Mathf.Abs(py) - ry, 0f);
                    if (Mathf.Sqrt(qx * qx + qy * qy) <= radius - 0.5f) cov += 1f;
                }
                tex.SetPixel(x, y, new Color(1f, 1f, 1f, cov / 16f));
            }
            tex.Apply();
            return tex;
        }

        private static void MakeCircle(string name, int size)
        {
            var tex = NewTexture(size, size);
            float c = size * 0.5f, r = size * 0.5f - 1f;

            for (int y = 0; y < size; y++)
            for (int x = 0; x < size; x++)
            {
                float cov = 0f;
                for (int sy = 0; sy < 4; sy++)
                for (int sx = 0; sx < 4; sx++)
                {
                    float dx = x + (sx + 0.5f) / 4f - c;
                    float dy = y + (sy + 0.5f) / 4f - c;
                    if (dx * dx + dy * dy <= r * r) cov += 1f;
                }
                tex.SetPixel(x, y, new Color(1f, 1f, 1f, cov / 16f));
            }
            tex.Apply();
            SavePng(name, tex, Vector4.zero);
        }

        /// <summary>
        /// The recycling glyph from the approved SVG (01_main_page.md §9):
        /// one arrow polygon repeated at 0°/120°/240°. Rendered by even-odd
        /// point-in-polygon tests against the base path in SVG coordinates.
        /// </summary>
        private static void MakeRecycleIcon(string name, int size)
        {
            // Base arrow polygon from the spec SVG (y-down coordinates).
            Vector2[] poly =
            {
                new Vector2(-1.8f, -9f), new Vector2(1.8f, -9f), new Vector2(5.2f, -3f),
                new Vector2(8.4f, -4.9f), new Vector2(7f, 2f), new Vector2(0.5f, 0.5f),
                new Vector2(3.7f, -1.3f), new Vector2(0f, -7.6f)
            };
            float scale = size / 21.4f; // glyph extent ~±9.6 → comfortable margin
            float half = size * 0.5f;

            var tex = NewTexture(size, size);
            for (int y = 0; y < size; y++)
            for (int x = 0; x < size; x++)
            {
                float cov = 0f;
                for (int sy = 0; sy < 4; sy++)
                for (int sx = 0; sx < 4; sx++)
                {
                    // Texture (y-up) → SVG glyph space (y-down).
                    float gx = (x + (sx + 0.5f) / 4f - half) / scale;
                    float gy = (half - (y + (sy + 0.5f) / 4f)) / scale;

                    var p = new Vector2(gx, gy);
                    if (InPoly(poly, p) ||
                        InPoly(poly, RotateSvg(p, -120f)) ||
                        InPoly(poly, RotateSvg(p, -240f)))
                        cov += 1f;
                }
                tex.SetPixel(x, y, new Color(1f, 1f, 1f, cov / 16f));
            }
            tex.Apply();
            SavePng(name, tex, Vector4.zero);
        }

        private static Vector2 RotateSvg(Vector2 p, float degrees)
        {
            float a = degrees * Mathf.Deg2Rad;
            float ca = Mathf.Cos(a), sa = Mathf.Sin(a);
            return new Vector2(p.x * ca - p.y * sa, p.x * sa + p.y * ca);
        }

        private static bool InPoly(Vector2[] poly, Vector2 p)
        {
            bool inside = false;
            for (int i = 0, j = poly.Length - 1; i < poly.Length; j = i++)
            {
                if ((poly[i].y > p.y) != (poly[j].y > p.y) &&
                    p.x < (poly[j].x - poly[i].x) * (p.y - poly[i].y) / (poly[j].y - poly[i].y) + poly[i].x)
                    inside = !inside;
            }
            return inside;
        }

        // ---------------------------------------------------------------
        // Asset plumbing
        // ---------------------------------------------------------------

        private static Texture2D NewTexture(int w, int h) =>
            new Texture2D(w, h, TextureFormat.RGBA32, false);

        private static void SavePng(string name, Texture2D tex, Vector4 border)
        {
            string path = $"{SpriteDir}/{name}.png";
            File.WriteAllBytes(path, tex.EncodeToPNG());
            Object.DestroyImmediate(tex);

            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceSynchronousImport);

            var importer = (TextureImporter)AssetImporter.GetAtPath(path);
            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Single;
            importer.spriteBorder = border;
            importer.spritePixelsPerUnit = 100f;
            importer.mipmapEnabled = false;
            importer.alphaIsTransparency = true;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.wrapMode = TextureWrapMode.Clamp;
            importer.filterMode = FilterMode.Bilinear;
            importer.SaveAndReimport();
        }
    }
}
