using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private float sensitivity = 0.1f;

    private PlayerInput playerInput;
    private InputAction lookAction;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        lookAction = playerInput.actions["Look"];
    }

    private void Update()
    {
        Vector2 lookInput = lookAction.ReadValue<Vector2>();

        float rotationY = lookInput.x * sensitivity;

        transform.Rotate(0f, rotationY, 0f);
    }
}