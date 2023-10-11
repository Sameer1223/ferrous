using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public static bool IsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject gameUI;

    private void Awake()
    {
        IsPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M) || Input.GetButtonDown("Switch"))
        {
            SceneController.DemoSceneChange();
        }

        //TODO: display different set of controls on the canvas when something is paused
        if (Input.GetKeyDown(KeyCode.Escape))
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
        Debug.Log(IsPaused);
    }

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
        Cursor.visible = false; // Hide the cursor

        pauseMenuUI.SetActive(false);
        Time.timeScale = 1.0f;
        IsPaused = false;

        gameUI.SetActive(true); //re-enable game ui
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
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

        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
        Cursor.visible = false; // Hide the cursor
    }
}
