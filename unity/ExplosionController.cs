using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace DPP
{
    /// <summary>
    /// Animates an exploded view of the VCU. Attach to the parent GameObject containing all
    /// component children. Each child is moved outward from the local origin by `explodeDistance`.
    ///
    /// Requires DOTween (free) — install from Unity Asset Store.
    /// </summary>
    public class ExplosionController : MonoBehaviour
    {
        [SerializeField] private float explodeDistance = 0.15f;
        [SerializeField] private float duration = 1.0f;
        [SerializeField] private Ease explodeEase = Ease.OutQuad;
        [SerializeField] private Ease collapseEase = Ease.InOutQuad;

        private readonly List<Transform> components = new List<Transform>();
        private readonly List<Vector3> originalPositions = new List<Vector3>();

        void Awake()
        {
            // Capture original local positions of immediate child components.
            foreach (Transform child in transform)
            {
                components.Add(child);
                originalPositions.Add(child.localPosition);
            }
        }

        public void Explode()
        {
            for (int i = 0; i < components.Count; i++)
            {
                Vector3 dir = originalPositions[i].sqrMagnitude > 0.0001f
                    ? originalPositions[i].normalized
                    : Vector3.up; // fallback for components at the origin

                Vector3 target = originalPositions[i] + dir * explodeDistance;
                components[i].DOLocalMove(target, duration).SetEase(explodeEase);
            }
        }

        public void Collapse()
        {
            for (int i = 0; i < components.Count; i++)
            {
                components[i].DOLocalMove(originalPositions[i], duration).SetEase(collapseEase);
            }
        }
    }
}
