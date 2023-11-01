using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private GameObject _firstBtn;
    [SerializeField] private GameObject _controlsBtn;
    [SerializeField] private GameObject _quitBtn;


    [Header("Options")]
    [SerializeField] private GameObject _controlsCanvas;
    [SerializeField] private GameObject _controllerControls;
    [SerializeField] private GameObject _mnkControls;


    // determine player input scheme
    private PlayerInput _playerInput;

    [SerializeField] private float _closeDelay = 0.1f;
    private bool _controlsOpened;


    private void Start()
    {
        _playerInput = GetComponent<PlayerInput>();

    }

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null && !_controlsCanvas.activeSelf)
        {
            // if keyboard / gamepad input, select the first thing in the list
            if ((Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame) ||
                (Gamepad.current != null && Gamepad.current.allControls.Any(control => control.IsActuated()))
                )
            {
                EventSystem.current.SetSelectedGameObject(_firstBtn);
            }
        }

        if (_controlsCanvas.activeSelf && Input.anyKeyDown && _controlsOpened)
        {
            CloseControls();
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Tutorial");
    }


    public void OpenControls()
    {
        if (_controlsOpened == false)
        {
            StartCoroutine(setControlsOpened(true));
            string _currentControlScheme = _playerInput.currentControlScheme;
            EventSystem.current.SetSelectedGameObject(null);

            // disable all other buttons
            _firstBtn.GetComponent<Button>().interactable = false;
            _controlsBtn.GetComponent<Button>().interactable = false;
            _quitBtn.GetComponent<Button>().interactable = false;

            // setup controls canvas
            _controlsCanvas.SetActive(true);



            // show the correct settings depending on which control scheme
            if (_currentControlScheme == "Gamepad")
            {
                _controllerControls.SetActive(true);
                _mnkControls.SetActive(false);
            }
            else if (_currentControlScheme == "Keyboard&Mouse")
            {
                _mnkControls.SetActive(true);
                _controllerControls.SetActive(false);
            }
        }
    }



    private IEnumerator setControlsOpened(bool b)
    {
        yield return new WaitForSeconds(0.1f);
        _controlsOpened = b;

    }

    public void CloseControls()
    {
        StartCoroutine(setControlsOpened(false));

        EventSystem.current.SetSelectedGameObject(_controlsBtn);

        Invoke("reenableButtons", 0.025f);

        _controlsCanvas.SetActive(false);
    }

    private void reenableButtons()
    {
        // reenable all other buttons
        _firstBtn.GetComponent<Button>().interactable = true;
        _controlsBtn.GetComponent<Button>().interactable = true;
        _quitBtn.GetComponent<Button>().interactable = true;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
