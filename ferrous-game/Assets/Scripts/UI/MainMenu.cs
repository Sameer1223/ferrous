using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Ferrous.UI
{
    public class MainMenu : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private GameObject _firstBtn;
        [SerializeField] private GameObject _controlsBtn;
        [SerializeField] private GameObject _soundBtn;
        [SerializeField] private GameObject _quitBtn;


        [Header("Options")]
        [SerializeField] private GameObject _controlsCanvas;
        [SerializeField] private GameObject _controllerControls;
        [SerializeField] private GameObject _mnkControls;
        [SerializeField] private GameObject _soundSetCanvas;


        // determine player input scheme
        private PlayerInput _playerInput;

        [SerializeField] private float _closeDelay = 0.1f;
        private bool _controlsOpened;
        private bool _soundSetOpened;


        private void Start()
        {
            _playerInput = GetComponent<PlayerInput>();
        
        }

        private void Update()
        {
            if (EventSystem.current.currentSelectedGameObject == null && !_controlsCanvas.activeSelf && !_soundSetCanvas.activeSelf)
            {
                // if keyboard / gamepad input, select the first thing in the list
                if ((Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame) ||
                    (Gamepad.current != null && Gamepad.current.allControls.Any(control => control.IsActuated()))
                   )
                {
                    EventSystem.current.SetSelectedGameObject(_firstBtn);
                }
            }


            if (_controlsCanvas.activeSelf && Input.anyKeyDown && _controlsOpened)
            {
                CloseControls();
            }

            if(Gamepad.current == null)
            {
                if (_soundSetCanvas.activeSelf && Input.GetKeyDown(KeyCode.Escape) && _soundSetOpened)
                {
                    CloseSetSound();
                }
            }
            else
            {
                if (_soundSetCanvas.activeSelf && (Input.GetKeyDown(KeyCode.Escape) || Gamepad.current.buttonEast.isPressed)&& _soundSetOpened)
                {
                    CloseSetSound();
                }
            }
            
        }

        public void StartGame()
        {
            SceneManager.LoadScene("Tutorial");
            Time.timeScale = 1.0f;
        }


        public void OpenControls()
        {
            if (_controlsOpened == false)
            {
                StartCoroutine(setControlsOpened(true));
                string _currentControlScheme = _playerInput.currentControlScheme;
                EventSystem.current.SetSelectedGameObject(null);

                // disable all other buttons
                _firstBtn.GetComponent<Button>().interactable = false;
                _controlsBtn.GetComponent<Button>().interactable = false;
                _soundBtn.GetComponent<Button>().interactable= false;
                _quitBtn.GetComponent<Button>().interactable = false;

                // setup controls canvas
                _controlsCanvas.SetActive(true);



                // show the correct settings depending on which control scheme
                if (_currentControlScheme == "Gamepad")
                {
                    _controllerControls.SetActive(true);
                    _mnkControls.SetActive(false);
                }
                else if (_currentControlScheme == "Keyboard&Mouse")
                {
                    _mnkControls.SetActive(true);
                    _controllerControls.SetActive(false);
                }
            }
        }

        

        private IEnumerator setControlsOpened(bool b)
        {
            yield return new WaitForSeconds(0.1f);
            _controlsOpened = b;

        }

        public void CloseControls()
        {
            StartCoroutine(setControlsOpened(false));

            EventSystem.current.SetSelectedGameObject(_controlsBtn);

            Invoke("reenableButtons", 0.025f);

            _controlsCanvas.SetActive(false);
        }

        public void OpenSoundSet()
        {
            if (_soundSetOpened == false)
            {
                StartCoroutine(setSoundSetOpened(true));

                // disable all other buttons
                _firstBtn.GetComponent<Button>().interactable = false;
                _controlsBtn.GetComponent<Button>().interactable = false;
                _soundBtn.GetComponent<Button>().interactable = false;
                _quitBtn.GetComponent<Button>().interactable = false;

                // setup controls canvas
                _soundSetCanvas.SetActive(true);
            }
        }

        private IEnumerator setSoundSetOpened(bool b)
        {
            yield return new WaitForSeconds(0.1f);
            _soundSetOpened = b;

        }

        public void CloseSetSound()
        {
            StartCoroutine(setSoundSetOpened(false));

            EventSystem.current.SetSelectedGameObject(_soundBtn);

            Invoke("reenableButtons", 0.025f);

            _soundSetCanvas.SetActive(false);
        }


        private void reenableButtons()
        {
            // reenable all other buttons
            _firstBtn.GetComponent<Button>().interactable = true;
            _controlsBtn.GetComponent<Button>().interactable = true;
            _soundBtn.GetComponent<Button>().interactable = true;
            _quitBtn.GetComponent<Button>().interactable = true;
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}
