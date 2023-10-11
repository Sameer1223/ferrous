using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneController : MonoBehaviour
{
    /// <summary>
    /// An struct that stores all the scene names.
    /// </summary>
    public struct Scenes
    {
        public string Menu;
        public string Tutorial;
    }

    public static Scenes gameScenes = new Scenes
    {
        Menu = "GameStart",
        Tutorial = "Main Scene"
    };

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