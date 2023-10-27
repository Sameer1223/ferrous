using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class CanvasControl : MonoBehaviour
{
    [SerializeField] private GameObject Settingmenu;
    [SerializeField] private GameObject Helpmenu;
    [SerializeField] private GameObject MusicSlider;
    [SerializeField] private GameObject _mainFirst;
    [SerializeField] private GameObject _controlsMenu;
    [SerializeField] private GameObject _controlsBtn;
    [SerializeField] private GameObject _controlsFirst;

    [SerializeField] private float alphaSpeed = 1.0f;
    private bool isFade = false;
    private bool isFadeHelp = false;
    private CanvasGroup canvasGroupSetting;
    private CanvasGroup canvasGroupHelp;
    private CanvasGroup canvasGroup2;

    public AudioSource button;
    // Start is called before the first frame update
    void Start()
    {
        canvasGroupSetting = Settingmenu.GetComponent<CanvasGroup>();
        canvasGroup2 = MusicSlider.GetComponent<CanvasGroup>();
        canvasGroupHelp = Helpmenu.GetComponent<CanvasGroup>();
        EventSystem.current.SetSelectedGameObject(_mainFirst);
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
        if (isFadeHelp)
        {
            FadeInHelp();
        }
        else
        {
            FadeOutHelp();
        }


    }
    private void FadeOut()
    {
        canvasGroupSetting.alpha -= alphaSpeed * Time.deltaTime;
        canvasGroupSetting.interactable = false;
        canvasGroupSetting.blocksRaycasts = false;
        canvasGroup2.alpha -= alphaSpeed * Time.deltaTime;
        canvasGroup2.interactable = false;
        canvasGroup2.blocksRaycasts = false;
    }

    private void FadeIn()
    {
        canvasGroupSetting.alpha += alphaSpeed * Time.deltaTime;
        canvasGroupSetting.interactable = true;
        canvasGroupSetting.blocksRaycasts = true;
        canvasGroup2.alpha += alphaSpeed * Time.deltaTime;
        canvasGroup2.interactable = true;
        canvasGroup2.blocksRaycasts = true;
    }
    private void FadeOutHelp()
    {
        canvasGroupHelp.alpha -= alphaSpeed * Time.deltaTime;
        canvasGroupHelp.interactable = false;
        canvasGroupHelp.blocksRaycasts = false;
    }

    private void FadeInHelp()
    {
        canvasGroupHelp.alpha += alphaSpeed * Time.deltaTime;
        canvasGroupHelp.interactable = true;
        canvasGroupHelp.blocksRaycasts = true;
    }
    public void OnClickBtnActiveSetting()
    {
        //button.Play();
        isFade = !isFade;
    }

    public void OnClickBtnActiveHelp()
    {
        //button.Play();
        isFadeHelp = !isFadeHelp;
    }

    public void onStartGame()
    {

        SceneManager.LoadScene(1);
    }

    public void OnClickBtnBackMainScene()
    {
        SceneManager.LoadScene(0);
    }

    public void OnControlsOpen()
    {
        _controlsMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_controlsFirst);
    }

    public void OnControlsClose()
    {
        _controlsMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(_controlsBtn);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }






}
