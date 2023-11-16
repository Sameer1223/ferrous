using System.Collections;
using UnityEngine;

namespace Ferrous
{
    public class LinkedObjectManager : MonoBehaviour
    {
        private LineRenderer lr;
        private Material lineMaterial;
  

        [Header("Objects")]
        public GameObject firstObj;
        public GameObject secondObj;

        [Header("FadeControl")]
        public float fadeDuration;
        private Color startColor;
        private Color endColor;

        public bool _madeLighter;


        // Start is called before the first frame update
        void Start()
        {
            lr = GetComponent<LineRenderer>();
            lineMaterial = lr.material;
            startColor = lineMaterial.color;
            endColor = lineMaterial.color;
            endColor.a = 0.6f;
            StartCoroutine(fadeLink());
            // TODO: give the link object a material, give first and secondObj outline = material on start

        }

        // Update is called once per frame
        void Update()
        {
            setPosition();
        }

        private void setPosition()
        {
            lr.SetPosition(0, firstObj.transform.position);
            lr.SetPosition(1, secondObj.transform.position);
        }
    
        private IEnumerator fadeLink()
        {
            // continually run this coroutine
            while (true)
            {

                float timer = 0f;
                // fading out
                while (timer < fadeDuration)
                {
                    lineMaterial.color = Color.Lerp(startColor, endColor, timer / fadeDuration);
                    timer += Time.deltaTime;
                    yield return null;
                }

                // fading in
                Color temp = startColor;
                startColor = endColor;
                endColor = temp;

                yield return new WaitForSeconds(timer);
            }
        }

        public static GameObject GetLinkedObject(GameObject go)
        {
            // check if the hit object's parent has the link line script
            if (go.transform.parent)
            {
                LinkedObjectManager linker = go.transform.parent.GetComponent<LinkedObjectManager>();
                if (linker != null)
                {
                    if (go == linker.firstObj)
                    {
                        return linker.secondObj;
                    }
                    else if (go == linker.secondObj)
                    {
                        return linker.firstObj;
                    }
                }
                return null;
            }
            return null;
        
        }

        public static void LightenLinkLine(GameObject go)
        {
            // check if the hit object's parent has the link line script
            if (go.transform.parent)
            {
                LinkedObjectManager linker = go.transform.parent.GetComponent<LinkedObjectManager>();
                if (linker != null)
                {
                    if ((go == linker.firstObj || go == linker.secondObj) && !linker._madeLighter)
                    {
                        linker.lightenLinkLine();
                        linker._madeLighter = true;
                    }
                }
            }
        }

        public static void DisableLinkLine(GameObject go)
        {
            // check if the hit object's parent has the link line script
            if (go.transform.parent)
            {
                LinkedObjectManager linker = go.transform.parent.GetComponent<LinkedObjectManager>();
                if (linker != null)
                {
                    if (go == linker.firstObj || go == linker.secondObj)
                    {
                        linker.disableLinkObj();
                    }
                }
            }
        
        }

        private void disableLinkObj()
        {
            // disable the script and the line renderer
            this.enabled = false;
            lr.enabled = false;
        }

        public void lightenLinkLine()
        {
            startColor = new Color(startColor.r, startColor.b, startColor.g, 0.75f);
            endColor = new Color(endColor.r, endColor.b, endColor.g, 0);
        }
    }
}
