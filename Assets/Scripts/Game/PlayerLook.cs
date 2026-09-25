using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private GameManager gameManager;

    [Header("Sensitivity")]
    [SerializeField] private float minimumSensitivity = 0.05f;
    [SerializeField] private float maximumSensitivity = 0.45f;

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

        float sensitivity = GetSensitivity();

        float rotationY =
            lookInput.x *
            sensitivity;

        transform.Rotate(
            0f,
            rotationY,
            0f
        );
    }

    private float GetSensitivity()
    {
        int value = Settings.GetSensitivity();

        float normalizedValue = value / 10f;

        return Mathf.Lerp(
            minimumSensitivity,
            maximumSensitivity,
            normalizedValue
        );
    }
}