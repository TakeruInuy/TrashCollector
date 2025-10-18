using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static PlayerInput playerInput;

    public static Vector3 inputDirection;

    private InputAction _moveAction;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        _moveAction = playerInput.actions["Move"];
    }

    void Update()
    {
        inputDirection.x = _moveAction.ReadValue<Vector2>().x;
        inputDirection.z = _moveAction.ReadValue<Vector2>().y;
    }
}
