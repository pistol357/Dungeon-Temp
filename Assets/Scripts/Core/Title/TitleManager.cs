using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private InputActionReference _prevInput;
    [SerializeField] private InputActionReference _nextInput;
    [SerializeField] private InputActionReference _selectInput;
    [SerializeField] private List<Button> _buttons;

    private bool _isPressed;
    private int _index;
    private Button _selectedButton;

    private void OnEnable() => BindInputEvent();
    private void Start() => Init();
    private void OnDisable() => UnbindInputEvent();

    private void BindInputEvent()
    {
        _prevInput.action.Enable();
        _nextInput.action.Enable();
        _selectInput.action.Enable();
        _prevInput.action.performed += OnPrev;
        _nextInput.action.performed += OnNext;
        _selectInput.action.performed += OnSelect;
        _selectInput.action.canceled += OnUnselect;
    }

    private void UnbindInputEvent()
    {
        _prevInput.action.Disable();
        _nextInput.action.Disable();
        _selectInput.action.Disable();
        _prevInput.action.performed -= OnPrev;
        _nextInput.action.performed -= OnNext;
        _selectInput.action.performed -= OnSelect;
        _selectInput.action.canceled -= OnUnselect;
    }

    private void OnPrev(InputAction.CallbackContext context)
    {
        if (_isPressed) return;

        _index = (_index + _buttons.Count - 1) % _buttons.Count;
        _selectedButton = _buttons[_index];
        _selectedButton.Select();
    }

    private void OnNext(InputAction.CallbackContext context)
    {
        if (_isPressed) return;

        _index = (_index + 1) % _buttons.Count;
        _selectedButton = _buttons[_index];
        _selectedButton.Select();
    }

    private void OnSelect(InputAction.CallbackContext context)
    {
        _isPressed = true;
        _selectedButton.image.overrideSprite = _selectedButton.spriteState.pressedSprite;
    }

    private void OnUnselect(InputAction.CallbackContext context)
    {
        _isPressed = false;
        _selectedButton.image.overrideSprite = _selectedButton.spriteState.selectedSprite;
        _selectedButton.onClick?.Invoke();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    private void Init()
    {
        _selectedButton = _buttons[_index];
        _selectedButton.Select();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
