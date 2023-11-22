using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ferrous
{
    public class PlayAudioClip : MonoBehaviour
    {
        [SerializeField] private AudioSource sfxToPlay;
        private bool soundPlayed = false;

        public void PlaySfx()
        {
            if (!soundPlayed)
            {
                sfxToPlay.Play();
                soundPlayed = true;
            }
        }
    }
}
