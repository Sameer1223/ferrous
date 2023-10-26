using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkLineManager : MonoBehaviour
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

    public void disableLink()
    {
        // disable the script and the line renderer
        this.enabled = false;
        lr.enabled = false;
    }
}
