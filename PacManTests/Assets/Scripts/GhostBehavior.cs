using UnityEngine;

namespace PacManGame
{
    [RequireComponent(typeof(Ghost))]
    public abstract class GhostBehavior : MonoBehaviour
    {
        public Ghost ghost { get; set; }
        public float duration;

        private void Awake()
        {
            ghost = GetComponent<Ghost>();
        }

        public void Enable()
        {
            Enable(duration);
        }

        public virtual void Enable(float duration)
        {
            enabled = true;
            CancelInvoke();
            Invoke(nameof(Disable), duration);
        }

        public virtual void Disable()
        {
            enabled = false;
            CancelInvoke();
        }
    }
}