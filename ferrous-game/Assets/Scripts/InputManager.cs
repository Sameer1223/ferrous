 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    public static InputManager instance;

    // public values for input
    public bool JumpInput {  get; private set; }
    public Vector2 MovementInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public bool PushInput { get; private set; }
    public bool PullInput { get; private set; }
    public bool StasisInput { get; private set; }
    public bool PauseMenuOpenCloseInput { get; private set; }
    public bool SelectInput { get; private set; }


    private PlayerInput _playerInput;

    [Header("Actions")]
    private InputAction _jumpAction;
    private InputAction _movementAction;
    private InputAction _lookAction;
    private InputAction _pushAction;
    private InputAction _pullAction;
    private InputAction _stasisAction;
    private InputAction _pauseMenuOpenCloseAction;
    private InputAction _selectAction;



    private void Awake()
    {
        // singleton setup
        if (instance == null)
        {
            instance = this;
        }

        _playerInput = GetComponent<PlayerInput>();
        // getting actions from the input list
        _jumpAction = _playerInput.actions["Jump"];
        _movementAction = _playerInput.actions["Movement"];
        _lookAction = _playerInput.actions["Look"];
        _pushAction = _playerInput.actions["Push"];
        _pullAction = _playerInput.actions["Pull"];
        _stasisAction = _playerInput.actions["Stasis"];
        _pauseMenuOpenCloseAction = _playerInput.actions["PauseMenuOpenClose"];
        _selectAction = _playerInput.actions["Select"];


    }


    // Update is called once per frame
    void Update()
    {
        JumpInput = _jumpAction.IsPressed();
        MovementInput = _movementAction.ReadValue<Vector2>();
        LookInput = _lookAction.ReadValue<Vector2>();
        PushInput = _pushAction.IsPressed();
        PullInput = _pullAction.IsPressed();
        StasisInput = _stasisAction.WasPressedThisFrame();
        PauseMenuOpenCloseInput = _pauseMenuOpenCloseAction.WasPressedThisFrame();
        SelectInput = _selectAction.WasPressedThisFrame();

    }
}
