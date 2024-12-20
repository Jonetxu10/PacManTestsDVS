using UnityEngine;

namespace PacManGame
{
    [DefaultExecutionOrder(-10)]
    [RequireComponent(typeof(Movement))]
    public class Ghost : MonoBehaviour
    {
        public Movement movement { get; set; }
        public GhostHome home { get; set; }
        public GhostScatter scatter { get; set; }
        public GhostChase chase { get; set; }
        public GhostFrightened frightened { get; set; }
        public GhostBehavior initialBehavior;
        public Transform target;
        public int points = 200;

        public void Awake()
        {
            movement = GetComponent<Movement>();
            home = GetComponent<GhostHome>();
            scatter = GetComponent<GhostScatter>();
            chase = GetComponent<GhostChase>();
            frightened = GetComponent<GhostFrightened>();
        }

        public void Start()
        {
            ResetState();
        }

        public void ResetState()
        {
            gameObject.SetActive(true);
            movement.ResetState();

            frightened.Disable();
            chase.Disable();
            scatter.Enable();

            if (home != initialBehavior)
            {
                home.Disable();
            }

            if (initialBehavior != null)
            {
                initialBehavior.Enable();
            }
        }

        public void SetPosition(Vector3 position)
        {
            position.z = transform.position.z;
            transform.position = position;
        }

        public void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
            {
                if (frightened.enabled)
                {
                    FindObjectOfType<GameManager>().GhostEaten(this);
                }
                else
                {
                    FindObjectOfType<GameManager>().PacmanEaten();
                }
            }
        }
    }
}