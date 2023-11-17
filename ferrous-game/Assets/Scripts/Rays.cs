using Ferrous.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ferrous
{
    public class Rays : MonoBehaviour
    {
        [Header("Ray Logic")]
        public LineRenderer magnetRay;
        public Transform raySpawnPoint;
        public float maxLength;

        private Camera cam;
        private GameObject outlinedGameObj;
    
        // Keeps track of force being applied to object
        private Force force;

        [Header("Constants")]
        private Color blueColour = new Color(0, 191, 156); 
        private Color redColour = new Color(191, 0, 0);
        private Color purpleColour = new Color(10, 0, 191);
        private Color defaultOutlineColor;
        private bool defaultSet = false;

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

        private void PlayerInput()
        {
            _pushInput = InputManager.instance.PushInput;
            _pullInput = InputManager.instance.PullInput;
            _mousePos = Mouse.current.position.ReadValue();
            _stasisInput = InputManager.instance.StasisInput;
        }

        // Turn on magnetic ray
        private void Activate(Force force, Collider collider)
        {
            if (collider)
            {
                GameObject obj = collider.gameObject;
                if (obj)
                {
                    if (force == Force.None) return;

                    if (force == Force.Pull) magnetRay.material = blueMaterial;
                    else if (force == Force.Push) magnetRay.material = redMaterial;
                    else if (force == Force.Stasis) magnetRay.material = purpleMaterial;

                    magnetRay.enabled = true;

                    // Show outline on target object
                    ShowOutline(obj, force);
                }
           
            }
        
        }

        // Turn off magnetic ray
        private void Deactivate(GameObject obj)
        {
            if (obj == null) return;

            // Hide outline on target object
            HideOutline(obj);

            magnetRay.enabled = false;
            magnetRay.SetPosition(0, raySpawnPoint.position);
            magnetRay.SetPosition(1, raySpawnPoint.position);
        }

        // Outline logic for objects
        private void ShowOutline(GameObject obj, Force force)
        {
            if(obj == null) return;

            outlinedGameObj = obj;
            Outline outline = obj.GetComponent<Outline>();
            if (defaultSet == false){ 
                defaultSet = true;
                defaultOutlineColor = outline.OutlineColor;
                }



            // get linked object
            GameObject linkedObj = LinkedObjectManager.GetLinkedObject(obj);


            if (outline.OutlineColor != purpleColour)
            {
                if (force == Force.Pull)
                {
                    outline.OutlineColor = blueColour;
                    if (linkedObj)
                    {
                        Outline linkedOutline = linkedObj.GetComponent<Outline>();
                        linkedOutline.OutlineColor = blueColour;
                    }
                }
                else if (force == Force.Push)
                {
                    outline.OutlineColor = redColour;
                    if (linkedObj)
                    {
                        Outline linkedOutline = linkedObj.GetComponent<Outline>();
                        linkedOutline.OutlineColor = redColour;
                    }
                }
            }
        }

        // Hiding outline after being deselected
        private void HideOutline(GameObject obj)
        {
            if (obj == null) return;

            Outline outline = obj.GetComponent<Outline>();
            if (defaultSet == true){ 
                defaultSet = false;
                }
            GameObject linkedObj = LinkedObjectManager.GetLinkedObject(obj);
            if (outline.OutlineColor != purpleColour) 
            { 
                outline.OutlineColor = defaultOutlineColor; 
                if (linkedObj)
                {
                    Outline linkedOutline = linkedObj.GetComponent<Outline>();
                    linkedOutline.OutlineColor = defaultOutlineColor;
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (!PauseMenu.IsPaused)
            {
                PlayerInput();
                // Integer to track force push or pull. Pull = 0, Push = 1, No Force = -1
                force = Force.None;

                if (_pullInput) force = Force.Pull;
                else if (_pushInput) force = Force.Push;
                else if (_stasisInput) force = Force.Stasis;
                else if (!_pullInput && !_pushInput && !_stasisInput) Deactivate(outlinedGameObj);


                // Calculate ray logic
                Ray ray = cam.ScreenPointToRay(_mousePos);
                bool cast = Physics.Raycast(ray, out RaycastHit hit, maxLength);
                Vector3 hitPosition = cast ? hit.point : raySpawnPoint.position + raySpawnPoint.forward * maxLength;

                if (hit.collider && hit.collider.gameObject && !hit.collider.gameObject.CompareTag("Metal"))
                {
                    Deactivate(outlinedGameObj);
                    return;
                }
                // Activate ray with force integer (push / pull / stasis)
                Activate(force, hit.collider);

                if (!magnetRay.enabled) return;

                // Line renderer set positions
                magnetRay.SetPosition(0, raySpawnPoint.position);
                magnetRay.SetPosition(1, hitPosition);
            }
            // TODO: MAKE THE STASIS RAY LAST LONGER INSTEAD OF DOING DOWN
        }
    }
}
