using UnityEngine;
using UnityEngine.Events;

namespace Ferrous.Mechanics
{
    public class OpenDoorTrigger : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField] private AudioSource doorOpenSfx;
        public UnityEvent openDoors;
        private bool doorOpened;

        private void OnTriggerEnter(Collider collision)
        {
            if(collision.gameObject.name == "Player")
            {
                if (!doorOpened && !doorOpenSfx.isPlaying)
                {
                    doorOpenSfx.Play();
                    openDoors.Invoke();
                }
                doorOpened = true;
            }
        }
    }
}
