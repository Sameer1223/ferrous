using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneController : MonoBehaviour
{

    /// <summary>
    /// An struct that stores all the scene names.
    /// </summary>
    /// 

    public struct Scenes
    {
        public string Menu;
        public string Tutorial;
        public string LevelOne;
        public string LevelTwo;
    }

    public static Scenes gameScenes = new Scenes
    {
        Menu = "GameStart",
        Tutorial = "Main Scene",
        LevelOne = "Varun Level",
    };

    public static void DemoSceneChange()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = (currentSceneIndex + 1) % SceneManager.sceneCountInBuildSettings;

        SceneManager.LoadScene(nextSceneIndex);
    }

    /// <summary>
    /// Restarts the current scene.
    /// </summary>
    public static void Restart()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);

        // un-pause just in case
        PauseMenu.IsPaused = true;
        Time.timeScale = 1.0f;
    }

    public static void BackToMain()
    {
        SceneManager.LoadScene(gameScenes.Menu);
    }
}