using TMPro;
using UnityEngine;

namespace Ferrous.UI
{
    public class InGameCanvas : MonoBehaviour
    {

        public TextMeshProUGUI fps;

        // Update is called once per frame
        void Update()
        {
            fps.text = ((int)(1.0f / Time.deltaTime)).ToString();
        }
    }
}
