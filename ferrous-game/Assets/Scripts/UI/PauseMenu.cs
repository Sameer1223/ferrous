using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{

    public static bool IsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject gameUI;


    [Header("InputChecks")]
    private bool _pauseMenuInput;

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

        if (Input.GetKeyDown(KeyCode.M) || Input.GetButtonDown("Switch"))
        {
            SceneController.DemoSceneChange();
        }

        //TODO: display different set of controls on the canvas when something is paused
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
    }

    private void InputCheck()
    {
        _pauseMenuInput = InputManager.instance.PauseMenuOpenCloseInput;
    }

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
        Cursor.visible = false; // Hide the cursor

        pauseMenuUI.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);

        Time.timeScale = 1.0f;
        IsPaused = false;

        gameUI.SetActive(true); //re-enable game ui
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);

        // change the selected game object to the pause menu's first item
        EventSystem.current.SetSelectedGameObject(_pauseMenuFirst);

        Time.timeScale = 0;
        IsPaused = true;

        gameUI.SetActive(false); //re-enable game ui
    }

    public void LoadMenu()
    {
        IsPaused = false;
        Time.timeScale = 1.0f;
        SceneController.BackToMain();

        Cursor.lockState = CursorLockMode.None; // Lock the cursor to the center of the screen
        Cursor.visible = true; // Hide the cursor
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
}
