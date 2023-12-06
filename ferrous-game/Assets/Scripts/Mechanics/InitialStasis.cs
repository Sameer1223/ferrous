using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ferrous
{
    public class InitialStasis : MonoBehaviour
    {
        // Start is called before the first frame update
        
        private Color purpleColour = new Color(10, 0, 191);
        void Start()
        {
            SetOutlineColor(gameObject, purpleColour);
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
        
        private void SetOutlineColor(GameObject obj, Color color)
        {
            if (obj == null) return;
            Outline outline = obj.GetComponent<Outline>();
            outline.OutlineColor = color;
        }
    }
}
