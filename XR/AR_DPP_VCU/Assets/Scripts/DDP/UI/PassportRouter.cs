using UnityEngine;

namespace DPP.UI
{
    /// <summary>
    /// Two-level navigation inside a passport screen (spec 13 v2 §5, spec 14 v2):
    /// landing ⇄ one full-page detail shell at a time.
    ///
    /// Used TWICE — once on DPP Canva (5 tiles) and once on Composition &amp; impact
    /// (3 blocks). One class, one instance per screen.
    ///
    /// WHY Open1..Open6 INSTEAD OF Open(int): serialized UnityEvent listeners wired
    /// from an editor script are parameterless (see DPPUIBuilder.WireClick), so each
    /// slot needs its own no-arg entry point. Unused slots simply stay unassigned.
    ///
    /// The detail shells are currently CHROME ONLY — back arrow, icon, title — with
    /// the bodies deliberately unbuilt (Thiago, 2026-07-30: "build just the tabs, and
    /// later we populate the modals"). Building the shells now rather than leaving the
    /// chevrons dead means a tap always goes somewhere and can always come back.
    /// </summary>
    public class PassportRouter : MonoBehaviour
    {
        [SerializeField] private GameObject landing;
        [SerializeField] private GameObject detail1;
        [SerializeField] private GameObject detail2;
        [SerializeField] private GameObject detail3;
        [SerializeField] private GameObject detail4;
        [SerializeField] private GameObject detail5;
        [SerializeField] private GameObject detail6;

        /// <summary>Entering the screen always shows the landing view; which detail was
        /// last open does not persist (same rule as the RBv1.0 InfoTabRouter).</summary>
        private void OnEnable() => Back();

        public void Open1() => Open(detail1);
        public void Open2() => Open(detail2);
        public void Open3() => Open(detail3);
        public void Open4() => Open(detail4);
        public void Open5() => Open(detail5);
        public void Open6() => Open(detail6);
        public void Back()  => Open(null);

        private void Open(GameObject target)
        {
            SetActiveSafe(landing, target == null);
            SetActiveSafe(detail1, target != null && target == detail1);
            SetActiveSafe(detail2, target != null && target == detail2);
            SetActiveSafe(detail3, target != null && target == detail3);
            SetActiveSafe(detail4, target != null && target == detail4);
            SetActiveSafe(detail5, target != null && target == detail5);
            SetActiveSafe(detail6, target != null && target == detail6);
            if (target != null) Debug.Log($"[PassportRouter] Opening '{target.name}'.");
        }

        private static void SetActiveSafe(GameObject go, bool active)
        {
            if (go != null && go.activeSelf != active) go.SetActive(active);
        }
    }
}
