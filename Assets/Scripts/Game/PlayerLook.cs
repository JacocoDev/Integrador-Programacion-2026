using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private GameManager gameManager;

    [Header("Look Settings")]
    [SerializeField] private float sensitivity = 0.25f;

    private InputAction lookAction;

    private void Awake()
    {
        lookAction = playerInput.actions["Look"];
    }

    private void Update()
    {
        if (!gameManager.IsGameActive())
            return;

        Vector2 lookInput = lookAction.ReadValue<Vector2>();
        float rotationY = lookInput.x * sensitivity;

        transform.Rotate(0f, rotationY, 0f);
    }
}