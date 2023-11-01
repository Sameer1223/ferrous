using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{

    public static bool IsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject gameUI;

    [Header("control schemes")]
    [SerializeField] public GameObject _controllerControls;
    [SerializeField] public GameObject _keyboardControls;


    [Header("InputChecks")]
    private bool _pauseMenuInput;
    private string _currCtrlScheme;

    [Header("First Menu Option")]
    [SerializeField] private GameObject _pauseMenuFirst;

    
    private void Awake()
    {
        IsPaused = false;
    }


    // Update is called once per frame
    void Update()
    {
        InputCheck();
        /* Cheat code to switch levels
        if (Input.GetKeyDown(KeyCode.M) || Input.GetButtonDown("Switch"))
        {
            SceneController.DemoSceneChange();
        }
        */

        if (_pauseMenuInput)
        {
            if (IsPaused)
            {
                Resume();
            } else
            {
                Cursor.lockState = CursorLockMode.None; // unlock the cursor
                Cursor.visible = true; // unhide the cursor
                Pause();
            }
        }

        // setting the correct selected option for menus
        if (EventSystem.current.currentSelectedGameObject == null && pauseMenuUI.activeSelf)
        {
            // if keyboard / gamepad input, select the first thing in the list
            if ((Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame) ||
                (Gamepad.current != null && Gamepad.current.allControls.Any(control => control.IsActuated()))
                )
            {
                EventSystem.current.SetSelectedGameObject(_pauseMenuFirst);
            }
        }
    }

    private void InputCheck()
    {
        _pauseMenuInput = InputManager.instance.PauseMenuOpenCloseInput;
        _currCtrlScheme = InputManager.instance.CurrControlScheme;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);

        Time.timeScale = 0;
        IsPaused = true;

        // show the correct controls
        if (_currCtrlScheme == "Keyboard&Mouse")
        {
            _keyboardControls.SetActive(true);
            _controllerControls.SetActive(false);
        } else if (_currCtrlScheme == "Gamepad")
        {
            _controllerControls.SetActive(true);
            _keyboardControls.SetActive(false);
        }

        gameUI.SetActive(false); //disable game ui
    }

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
        Cursor.visible = false; // Hide the cursor

        pauseMenuUI.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);

        Time.timeScale = 1.0f;
        StartCoroutine("setIsPaused", false);


        gameUI.SetActive(true); //re-enable game ui
    }

    public void RestartLevel()
    {
        SceneController.Restart();
        Time.timeScale = 1.0f;
        IsPaused = false;
        EventSystem.current.SetSelectedGameObject(null);


        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
        Cursor.visible = false; // Hide the cursor
    }

    public void LoadMenu()
    {
        IsPaused = false;
        Time.timeScale = 1.0f;
        SceneController.BackToMain();

        Cursor.lockState = CursorLockMode.None; // Lock the cursor to the center of the screen
        Cursor.visible = true; // Hide the cursor
    }

    private IEnumerator setIsPaused(bool b)
    {
        yield return new WaitForSeconds(0.2f);
        IsPaused = b;
    }
}
