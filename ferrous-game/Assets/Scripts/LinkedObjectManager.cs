using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        lineMaterial = lr.material;
        startColor = lineMaterial.color;
        endColor = lineMaterial.color;
        endColor.a = 0;
        StartCoroutine(fadeLink());

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

    public static void DisableLinkLine(GameObject go)
    {
        // check if the hit object's parent has the link line script
        LinkedObjectManager linker = go.transform.parent.GetComponent<LinkedObjectManager>();
        if (linker != null)
        {
            if (go == linker.firstObj || go == linker.secondObj)
            {
                linker.disableLinkObj();
            }
        }
    }

    public void disableLinkObj()
    {
        // disable the script and the line renderer
        this.enabled = false;
        lr.enabled = false;
    }
}
