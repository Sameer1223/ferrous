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
    };

    public static void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = (currentSceneIndex + 1) % SceneManager.sceneCountInBuildSettings;

        SceneManager.LoadScene(nextSceneIndex);
    }

    public static bool IsLastScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int totalScenes = SceneManager.sceneCountInBuildSettings;

        return currentSceneIndex == totalScenes - 1;
    }

    /// <summary>
    /// Restarts the current scene.
    /// </summary>
    public static void Restart()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
        Time.timeScale = 1.0f;
    }

    public static void BackToMain()
    {
        SceneManager.LoadScene(gameScenes.Menu);
    }
}