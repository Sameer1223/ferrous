using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

namespace Ferrous.UI
{
    public class CutSceneControl : MonoBehaviour
    {
        [SerializeField] private VideoPlayer _video;
        [SerializeField] private AudioMixerGroup _music;
        [SerializeField] private PlayerInput _playerInput;


        void Start()
        {
            
            _video.GetComponent<AudioSource>().outputAudioMixerGroup = _music;
            Invoke("LoadTutorial", 30);
        }

        // Update is called once per frame
        void Update()
        {
            string _currentControlScheme = _playerInput.currentControlScheme;
            if (Input.GetKeyUp(KeyCode.B))
            {
                LoadTutorial();
            }
            if (_currentControlScheme == "Gamepad" && Gamepad.current != null && Gamepad.current.buttonEast.isPressed)
            {
                LoadTutorial();
            }
        }

        private void LoadTutorial()
        {
            SceneManager.LoadScene("Tutorial");
        }

    }
}
