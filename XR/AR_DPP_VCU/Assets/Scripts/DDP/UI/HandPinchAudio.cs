using UnityEngine;

namespace DPP.UI
{
    /// <summary>
    /// Object-aware grab/drag audio (P02 feedback; reworked on Thiago's review,
    /// 2026-08-01: "add the sound just when the user drags some object; make the
    /// loop a WIND with a different equalizer per drag direction").
    ///
    /// v1 polled the PXR hands itself, so an AIR pinch that grabbed nothing also
    /// sounded, and a fixed ripple loop ran for as long as any pinch was held.
    /// This version inverts the flow: the interaction scripts REPORT what they
    /// are doing and this component only renders audio.
    ///
    ///   ObjectGrabbed(rightHand)        → one water-drop "bloop" (R higher-pitched
    ///                                     + panned right, L lower + left)
    ///   DragTick(rightHand, worldDelta) → wind loop, called every frame BY the
    ///                                     drag code with the object's motion
    ///
    /// The wind is the "equalizer per direction":
    ///   speed            → volume (an object held still is silent) + slight pitch
    ///   drag UP          → bright, airy (low-pass opens, high-pass rises)
    ///   drag DOWN        → dark rumble (low-pass closes, high-pass drops)
    ///   drag LEFT/RIGHT  → stereo pan sweep + small pitch bend
    /// Direction is measured on the head-camera plane, so "up" is the user's up.
    ///
    /// When ticks stop arriving (release, or the caller dies) the loop fades out
    /// after tickTimeout — no release call exists, so a dropped pinch can never
    /// leave wind blowing forever. Statics are null-safe before scene load.
    /// Callers: ZonePartInteraction (part drag + list scroll), PanelGrabHandle
    /// (panel drag), PinchScrollArea (list scroll), ScrollbarGrabHandle.
    /// </summary>
    public class HandPinchAudio : MonoBehaviour
    {
        [Header("Clips (set by builder)")]
        [SerializeField] private AudioClip pinchRight;      // grab drop, right hand
        [SerializeField] private AudioClip pinchLeft;       // grab drop, left hand
        [SerializeField] private AudioClip dragLoopRight;   // wind loop, right hand
        [SerializeField] private AudioClip dragLoopLeft;    // wind loop, left hand

        [Header("Levels")]
        [Range(0f, 1f)] [SerializeField] private float pinchVolume = 0.5f;
        [Tooltip("Wind volume at full drag speed.")]
        [Range(0f, 1f)] [SerializeField] private float windVolume = 0.4f;
        [Tooltip("Drag speed (m/s) that reaches full wind volume.")]
        [SerializeField] private float fullSpeed = 0.35f;
        [Tooltip("Stereo home position of each hand (0 = centre, 1 = hard-panned).")]
        [Range(0f, 1f)] [SerializeField] private float pan = 0.6f;

        [Header("Directional EQ")]
        [Tooltip("Low-pass cutoff dragging DOWN (dark) … UP (bright), Hz.")]
        [SerializeField] private float lowPassDown = 380f;
        [SerializeField] private float lowPassUp = 7500f;
        [Tooltip("High-pass cutoff dragging DOWN … UP, Hz — down keeps the rumble, up thins it out.")]
        [SerializeField] private float highPassDown = 30f;
        [SerializeField] private float highPassUp = 420f;
        [Tooltip("Sideways pitch bend at full left/right drag (right = up).")]
        [Range(0f, 0.3f)] [SerializeField] private float sidePitchBend = 0.10f;

        [Header("Timing")]
        [Tooltip("Seconds without a DragTick before the wind fades out.")]
        [SerializeField] private float tickTimeout = 0.15f;
        [Tooltip("Seconds of wind fade-in / fade-out.")]
        [SerializeField] private float fadeTime = 0.12f;
        [Tooltip("Smoothing time for direction / speed changes (anti-zipper).")]
        [SerializeField] private float eqSmoothTime = 0.10f;

        private static HandPinchAudio _instance;

        private AudioSource _shotR, _shotL;
        private Wind _windR, _windL;

        private class Wind
        {
            public AudioSource src;
            public AudioLowPassFilter lp;
            public AudioHighPassFilter hp;
            public float basePan;
            public float lastTick = -999f;
            public float gain;             // 0..1 fade
            public Vector2 dir;            // smoothed camera-plane direction
            public Vector2 dirVel;
            public float speed01;          // smoothed 0..1 normalized speed
            public float speedVel;
        }

        private void Awake()
        {
            _instance = this;
            _shotR = MakeShot(+pan);
            _shotL = MakeShot(-pan);
            _windR = MakeWind("WindR", +pan, dragLoopRight);
            _windL = MakeWind("WindL", -pan, dragLoopLeft);
        }

        private void OnDestroy()
        {
            if (_instance == this) _instance = null;
        }

        private AudioSource MakeShot(float stereoPan)
        {
            var s = gameObject.AddComponent<AudioSource>();
            s.playOnAwake = false;
            s.spatialBlend = 0f;           // UI feedback, not a world sound
            s.panStereo = stereoPan;
            return s;
        }

        // The wind lives on its own CHILD object because Unity audio filters apply
        // to every AudioSource on their GameObject — on the root they would also
        // muffle the grab drops.
        private Wind MakeWind(string name, float stereoPan, AudioClip clip)
        {
            var go = new GameObject(name);
            go.transform.SetParent(transform, false);
            var w = new Wind();
            w.src = go.AddComponent<AudioSource>();
            w.src.playOnAwake = false;
            w.src.spatialBlend = 0f;
            w.src.loop = true;
            w.src.clip = clip;
            w.src.volume = 0f;
            w.lp = go.AddComponent<AudioLowPassFilter>();
            w.hp = go.AddComponent<AudioHighPassFilter>();
            w.lp.cutoffFrequency = 22000f;
            w.hp.cutoffFrequency = 10f;
            w.basePan = stereoPan;
            return w;
        }

        // ------------- static API (null-safe before the scene object exists) -------------

        /// <summary>An interaction script actually took hold of something — a part,
        /// a panel, a scrollable list. Air pinches never call this.</summary>
        public static void ObjectGrabbed(bool rightHand)
        {
            if (_instance != null) _instance.PlayGrab(rightHand);
        }

        /// <summary>Per-frame while an object is being dragged; worldDelta is the
        /// object's world-space motion this frame. Zero delta is fine (wind idles
        /// silently); stopping the calls is the release.</summary>
        public static void DragTick(bool rightHand, Vector3 worldDelta)
        {
            if (_instance != null) _instance.Tick(rightHand, worldDelta);
        }

        // ------------- instance -------------

        private void PlayGrab(bool right)
        {
            var clip = right ? pinchRight : pinchLeft;
            var src = right ? _shotR : _shotL;
            if (clip != null && src != null) src.PlayOneShot(clip, pinchVolume);
        }

        private void Tick(bool right, Vector3 worldDelta)
        {
            var w = right ? _windR : _windL;
            if (w == null || w.src == null || w.src.clip == null) return;

            w.lastTick = Time.unscaledTime;
            if (!w.src.isPlaying)
            {
                // Random start offset so the two hands never phase-lock on the same gust.
                w.src.time = Random.value * Mathf.Max(0.01f, w.src.clip.length - 0.05f);
                w.src.Play();
            }

            float dt = Mathf.Max(Time.deltaTime, 1e-4f);
            float target01 = fullSpeed > 0f ? Mathf.Clamp01(worldDelta.magnitude / dt / fullSpeed) : 1f;
            w.speed01 = Mathf.SmoothDamp(w.speed01, target01, ref w.speedVel, eqSmoothTime);

            // Direction on the user's view plane, so "up" is up on their screen.
            Transform cam = Camera.main != null ? Camera.main.transform : null;
            Vector2 tdir = cam != null
                ? new Vector2(Vector3.Dot(worldDelta, cam.right), Vector3.Dot(worldDelta, cam.up))
                : new Vector2(worldDelta.x, worldDelta.y);
            if (tdir.sqrMagnitude > 1e-10f) tdir.Normalize(); else tdir = w.dir;
            w.dir = new Vector2(
                Mathf.SmoothDamp(w.dir.x, tdir.x, ref w.dirVel.x, eqSmoothTime),
                Mathf.SmoothDamp(w.dir.y, tdir.y, ref w.dirVel.y, eqSmoothTime));
        }

        private void Update()
        {
            Sculpt(_windR);
            Sculpt(_windL);
        }

        /// <summary>Fade + the per-direction EQ, every frame the wind exists.</summary>
        private void Sculpt(Wind w)
        {
            if (w == null || w.src == null) return;

            bool alive = Time.unscaledTime - w.lastTick <= tickTimeout;
            float step = fadeTime > 0f ? Time.deltaTime / fadeTime : 1f;
            w.gain = Mathf.Clamp01(w.gain + (alive ? step : -step));

            if (!alive && w.gain <= 0f)
            {
                if (w.src.isPlaying) w.src.Stop();
                w.speed01 = 0f;
                w.speedVel = 0f;
                return;
            }
            if (!w.src.isPlaying) return;

            float h = Mathf.Clamp(w.dir.x, -1f, 1f);   // −1 left  … +1 right
            float v = Mathf.Clamp(w.dir.y, -1f, 1f);   // −1 down … +1 up
            float t = (v + 1f) * 0.5f;

            // Perceptual (log-domain) sweep between the down- and up-cutoffs.
            w.lp.cutoffFrequency = Mathf.Exp(Mathf.Lerp(Mathf.Log(lowPassDown), Mathf.Log(lowPassUp), t));
            w.hp.cutoffFrequency = Mathf.Exp(Mathf.Lerp(Mathf.Log(highPassDown), Mathf.Log(highPassUp), t));

            w.src.volume = windVolume * w.speed01 * w.gain;
            w.src.panStereo = Mathf.Clamp(w.basePan * 0.35f + h * 0.65f, -1f, 1f);
            w.src.pitch = 1f + h * sidePitchBend + 0.06f * w.speed01;
        }
    }
}
