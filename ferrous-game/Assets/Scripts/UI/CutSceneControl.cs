using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

namespace Ferrous.UI
{
    public class CutSceneControl : MonoBehaviour
    {
        [SerializeField] private VideoPlayer _video;
        [SerializeField] private AudioMixerGroup _music;

        void Start()
        {
            _video.GetComponent<AudioSource>().outputAudioMixerGroup = _music;
            Invoke("LoadTutorial", 30);
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyUp(KeyCode.Escape))
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
