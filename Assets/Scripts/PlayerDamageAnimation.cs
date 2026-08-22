using UnityEngine;

public class PlayerDamageAnimation : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Transform cameraTransform;

    [Header("Damage Shake")]
    [SerializeField] private float shakeDistance = 0.05f;
    [SerializeField] private float shakeDuration = 0.15f;
    [SerializeField] private float shakeSpeed = 50f;

    private Vector3 originalLocalPosition;

    private int lastHealth;

    private float shakeTimer;
    private float shakeProgress;

    private void Awake()
    {
        originalLocalPosition = cameraTransform.localPosition;
        lastHealth = playerHealth.GetCurrentHealth();
    }

    private void Update()
    {
        UpdateHealthChange();

        if (shakeTimer <= 0f)
            return;

        UpdateShake();
    }

    private void UpdateHealthChange()
    {
        int currentHealth = playerHealth.GetCurrentHealth();

        if (currentHealth < lastHealth)
        {
            shakeTimer = shakeDuration;
            shakeProgress = 0f;
        }

        lastHealth = currentHealth;
    }

    private void UpdateShake()
    {
        shakeTimer -= Time.deltaTime;
        shakeProgress += Time.deltaTime * shakeSpeed;

        float shakeOffset = Mathf.Sin(shakeProgress) * shakeDistance;

        cameraTransform.localPosition = new Vector3(
            originalLocalPosition.x + shakeOffset,
            originalLocalPosition.y,
            originalLocalPosition.z
        );

        if (shakeTimer > 0f)
            return;

        shakeTimer = 0f;
        cameraTransform.localPosition = originalLocalPosition;
    }
}