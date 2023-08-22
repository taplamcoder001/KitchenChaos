using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    private const string PLAYER_PREFS_BINDINGS_KEY = "InputBindings";

    public static GameInput Instance {get; private set;}

    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractActionAlternate;
    public event EventHandler OnPauseAction;
    public event EventHandler OnBindingRebind;

    [SerializeField] private PlayerInputAction playerInputAction;
    
    public enum Binding{
        MoveUp,
        MoveLeft,
        MoveDown,
        MoveRight,
        Interact,
        InteractAlt,
        Pause,
    }

    private void Awake() {
        Instance = this;

        playerInputAction = new PlayerInputAction();

        if(PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS_KEY))
        {
            playerInputAction.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS_KEY));
        }
        
        playerInputAction.Enable();

        playerInputAction.Player.Interact.performed += Interact_performed;
        playerInputAction.Player.InteractAlternate.performed += InteractAlternate_performed;
        playerInputAction.Player.Pause.performed += Pause_performed;
    }

    private void OnDestroy() {
        playerInputAction.Player.Interact.performed -= Interact_performed;
        playerInputAction.Player.InteractAlternate.performed -= InteractAlternate_performed;
        playerInputAction.Player.Pause.performed -= Pause_performed;

        playerInputAction.Dispose();
    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj){
        OnPauseAction?.Invoke(this,EventArgs.Empty);
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnInteractAction?.Invoke(this,EventArgs.Empty);
    }

    private void InteractAlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractActionAlternate?.Invoke(this,EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = playerInputAction.Player.Move.ReadValue<Vector2>();
        inputVector.Normalize();
        return inputVector;
    }

    public string GetBindingText(Binding binding)
    {
        switch(binding){
            default:
            case Binding.Interact:
                return playerInputAction.Player.Interact.bindings[0].ToDisplayString();
            case Binding.InteractAlt:
                return playerInputAction.Player.InteractAlternate.bindings[0].ToDisplayString();
            case Binding.Pause:
                return playerInputAction.Player.Pause.bindings[0].ToDisplayString();
            case Binding.MoveUp:
                return playerInputAction.Player.Move.bindings[1].ToDisplayString();
            case Binding.MoveDown:
                return playerInputAction.Player.Move.bindings[2].ToDisplayString();
            case Binding.MoveLeft:
                return playerInputAction.Player.Move.bindings[3].ToDisplayString();
            case Binding.MoveRight:
                return playerInputAction.Player.Move.bindings[4].ToDisplayString();
        }
    }

    public void RebindBinding(Binding binding, Action onActionReBound)
    {
        playerInputAction.Player.Disable();
        InputAction inputAction;
        int bindingIndex;
        switch(binding)
        {
            default:
            case Binding.MoveUp:
                inputAction = playerInputAction.Player.Move;
                bindingIndex = 1;
                break;
            case Binding.MoveDown:
                inputAction = playerInputAction.Player.Move;
                bindingIndex = 2;
                break;
            case Binding.MoveLeft:
                inputAction = playerInputAction.Player.Move;
                bindingIndex = 3;
                break;
            case Binding.MoveRight:
                inputAction = playerInputAction.Player.Move;
                bindingIndex = 4;
                break;
            case Binding.Interact:
                inputAction = playerInputAction.Player.Interact;
                bindingIndex = 0;
                break;
            case Binding.InteractAlt:
                inputAction = playerInputAction.Player.InteractAlternate;
                bindingIndex = 0;
                break;
            case Binding.Pause:
                inputAction = playerInputAction.Player.Pause;
                bindingIndex = 0;
                break;
        }

        inputAction.PerformInteractiveRebinding(bindingIndex)
            .OnComplete(callback => {
                callback.Dispose();
                playerInputAction.Player.Enable();
                onActionReBound();

                PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS_KEY,playerInputAction.SaveBindingOverridesAsJson());
                PlayerPrefs.Save();

                OnBindingRebind?.Invoke(this,EventArgs.Empty);

            })
            .Start();
    }
}
