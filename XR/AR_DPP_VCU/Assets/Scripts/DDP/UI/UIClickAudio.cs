using UnityEngine;
using UnityEngine.UI;

namespace DPP.UI
{
    /// <summary>
    /// One click sound for every UI button (P02 feedback, 2026-08-01: "I missed
    /// sounds when clicking and interacting").
    ///
    /// A single scene object sweeps ALL Buttons once at Start — including inactive
    /// ones, because most passport screens start disabled — and adds a runtime
    /// listener. One sweep is sufficient: every Button in RBv2.0 is created by the
    /// editor builders and already exists in the scene; nothing instantiates
    /// Buttons at runtime. If that ever changes, call <see cref="Attach"/> on the
    /// new button.
    ///
    /// 2D audio (spatialBlend 0): a UI confirmation, not a world sound.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class UIClickAudio : MonoBehaviour
    {
        [Header("Wiring (set by builder)")]
        [SerializeField] private AudioClip clip;
        [Range(0f, 1f)]
        [SerializeField] private float volume = 0.9f;

        private AudioSource _source;

        private void Awake()
        {
            _source = GetComponent<AudioSource>();
            _source.playOnAwake = false;
            _source.spatialBlend = 0f;
            _source.loop = false;
        }

        private void Start()
        {
            if (clip == null)
            {
                Debug.LogWarning("[UIClickAudio] No clip assigned — UI clicks are silent.");
                return;
            }
            int count = 0;
            foreach (var button in FindObjectsByType<Button>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            {
                button.onClick.AddListener(Play);
                count++;
            }
            Debug.Log($"[UIClickAudio] Click sound attached to {count} buttons.");
        }

        /// <summary>For any button created after the Start sweep.</summary>
        public void Attach(Button button)
        {
            if (button != null) button.onClick.AddListener(Play);
        }

        public void Play()
        {
            if (clip != null) _source.PlayOneShot(clip, volume);
        }
    }
}
