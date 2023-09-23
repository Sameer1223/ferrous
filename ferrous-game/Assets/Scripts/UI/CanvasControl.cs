using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class CanvasControl : MonoBehaviour
{
    [SerializeField] private GameObject Settingmenu;
    [SerializeField] private GameObject MusicSlider;
    [SerializeField] private float alphaSpeed = 1.0f;
    private bool isFade = false;
    private CanvasGroup canvasGroup;
    private CanvasGroup canvasGroup2;
    public AudioSource button;
    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = Settingmenu.GetComponent<CanvasGroup>();
        canvasGroup2 = MusicSlider.GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isFade)
        {
            FadeIn();
        }
        else
        {
            FadeOut();
        }
        

    }
    private void FadeOut()
    {
        canvasGroup.alpha -= alphaSpeed * Time.deltaTime;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        canvasGroup2.alpha -= alphaSpeed * Time.deltaTime;
        canvasGroup2.interactable = false;
        canvasGroup2.blocksRaycasts = false;
    }

    private void FadeIn()
    {
        canvasGroup.alpha += alphaSpeed * Time.deltaTime;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        canvasGroup2.alpha += alphaSpeed * Time.deltaTime;
        canvasGroup2.interactable = true;
        canvasGroup2.blocksRaycasts = true;
    }
    public void OnClickBtnActiveSetting()
    {
        //button.Play();
        isFade = !isFade;
    }

    public void OnClickBtnNextScene()
    {

        SceneManager.LoadScene(1);
    }

    public void OnClickBtnBackMainScene()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }






}
