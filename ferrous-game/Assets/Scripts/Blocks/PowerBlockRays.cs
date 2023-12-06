using Ferrous.UI;
using UnityEngine;

namespace Ferrous.Blocks
{
    public class PowerBlockRays : MonoBehaviour
    {
        [Header("Ray Logic")]
        public LineRenderer magnetRay;
        public Transform raySpawnPoint;
        public float maxLength;

        private Camera cam;
        private GameObject outlinedGameObj;
        private Transform playerTransform;
        public GameObject inputMetal;
        private Transform inputMetalTransform;
        private bool inputMetalForceActedUpon;

        private float distance;
        private Vector3 heading;
        private Vector3 direction;


        // Keeps track of force being applied to object
        private Force force;

        [Header("Constants")]
        private Color blueColour = new Color(0, 191, 156);
        private Color redColour = new Color(191, 0, 0);
        private Color purpleColour = new Color(10, 0, 191);

        [Header("Materials")]
        public Material blueMaterial;
        public Material redMaterial;
        public Material purpleMaterial;

        [Header("InputChecks")]
        private bool _pushInput;
        private bool _pullInput;
        private Vector2 _mousePos;
        private bool _stasisInput;

        // Force enum to keep track of force modes
        public enum Force
        {
            Pull, //0
            Push, //1
            Stasis, //2
            None //3
        }

        private void Awake()
        {
            magnetRay.enabled = false;
            cam = Camera.main;
        }

          private void Start()
        {
            playerTransform = GameObject.Find("Player").transform;
            inputMetalForceActedUpon = inputMetal.GetComponent<Outline>().forceActedUpon;
            inputMetalTransform = inputMetal.GetComponent<Transform>();
        }

        private void PlayerInput()
        {
            _pushInput = InputManager.instance.PushInput;
            _pullInput = InputManager.instance.PullInput;
            _stasisInput = InputManager.instance.StasisInput;
        }

        // Turn on magnetic ray
        public void Activate(Force force)
        {

            if (force == Force.None) return;

            if (force == Force.Pull) magnetRay.material = blueMaterial;
            else if (force == Force.Push) magnetRay.material = redMaterial;
            else if (force == Force.Stasis) magnetRay.material = purpleMaterial;

            magnetRay.enabled = true;
        }

        // Turn off magnetic ray
        private void Deactivate()
        {
            magnetRay.enabled = false;
            magnetRay.SetPosition(0, raySpawnPoint.position);
            magnetRay.SetPosition(1, raySpawnPoint.position);
        }

        // Update is called once per frame
        void Update()
        {
            inputMetalForceActedUpon = inputMetal.GetComponent<Outline>().forceActedUpon;
            if (!PauseMenu.IsPaused)
            {
                PlayerInput();
                // Integer to track force push or pull. Pull = 0, Push = 1, No Force = -1
                force = Force.None;

                if (_pullInput) force = Force.Pull;
                else if (_pushInput) force = Force.Push;
                else if (_stasisInput) force = Force.Stasis;
                else if (!_pullInput && !_pushInput && !_stasisInput) Deactivate();

                // Calculate ray logic
                RaycastHit hit;
                heading = inputMetalTransform.position - playerTransform.position ;
                distance = heading.magnitude;
                direction = heading / distance;
                direction = GetInteractDirectionNormalized(direction);
                bool cast = Physics.Raycast(transform.position, direction, out hit);
                Vector3 hitPosition = cast ? hit.point : raySpawnPoint.position + raySpawnPoint.forward * maxLength;

                if (hit.collider != null && (!inputMetalForceActedUpon || !hit.collider.gameObject.CompareTag("Metal")))
                {
                    Deactivate();
                    return;
                }
                // Activate ray with force integer (push / pull / stasis)
                Activate(force);

                if (!magnetRay.enabled) return;

                // Line renderer set positions
                magnetRay.SetPosition(0, raySpawnPoint.position);
                magnetRay.SetPosition(1, hitPosition);
            }
            // TODO: MAKE THE STASIS RAY LAST LONGER INSTEAD OF DOING DOWN
        }

        private Vector3 GetInteractDirectionNormalized(Vector3 direction)
        {
            float dotX = Vector3.Dot(direction, Vector3.right);
            float dotZ = Vector3.Dot(direction, Vector3.forward);
            float dotY = Vector3.Dot(direction, Vector3.up);

            Vector3 nearestAxisDirection;

            if (Mathf.Abs(dotY) > Mathf.Abs(dotX) && Mathf.Abs(dotY) > Mathf.Abs(dotZ))
                nearestAxisDirection = dotY > 0 ? Vector3.up : Vector3.down;
            else if (Mathf.Abs(dotZ) > Mathf.Abs(dotX))
                nearestAxisDirection = dotZ > 0 ? Vector3.forward : Vector3.back;
            else
                nearestAxisDirection = dotX > 0 ? Vector3.right : Vector3.left;

            // RaycastHit hit;
            // Ray ray = new Ray(playerTransform.position, Vector3.down);
            // if (Physics.Raycast(ray, out hit, 2 * 0.5f + 0.2f))
            //
            // {
            //     if (hit.collider.CompareTag("Metal"))
            //     {
            //         nearestAxisDirection = Vector3.down;
            //     }
            // }
            return nearestAxisDirection;
        }
    }
}
