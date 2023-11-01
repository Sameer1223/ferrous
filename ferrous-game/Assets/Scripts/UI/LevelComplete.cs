using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class LevelComplete : MonoBehaviour
{

    [Header("Possible First Btns")]
    [SerializeField] private GameObject _nextLevelBtn;
    [SerializeField] private GameObject _restartBtn;
    private GameObject _firstBtn;


    // Start is called before the first frame update
    void Start()
    {
        if (SceneController.IsLastScene())
        {
            _firstBtn = _restartBtn;
            // hide the next level button
            _nextLevelBtn.SetActive(false);
        } else
        {
            _firstBtn = _nextLevelBtn;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            // if keyboard / gamepad input, select the first thing in the list
            if ((Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame) ||
                (Gamepad.current != null && Gamepad.current.allControls.Any(control => control.IsActuated()))
                )
            {
                EventSystem.current.SetSelectedGameObject(_firstBtn);
            }
        }
    }
}
