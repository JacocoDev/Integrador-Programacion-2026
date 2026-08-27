using UnityEngine;

public class ShotgunAnimation : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Shotgun shotgun;

    [Header("Recoil")]
    [SerializeField] private float recoilDistance = 0.25f;
    [SerializeField] private float recoilAngle = -30f;
    [SerializeField] private float recoilDuration = 0.1f;
    [SerializeField] private float recoilReturnDuration = 0.85f;

    [Header("Reload Angles")]
    [SerializeField] private float reloadTiltAngle = 30f;
    [SerializeField] private float reloadRotationAngle = 45f;

    [Header("Error Shake")]
    [SerializeField] private float errorShakeDistance = 0.05f;
    [SerializeField] private float errorShakeDuration = 0.15f;
    [SerializeField] private float errorShakeSpeed = 50f;

    private Vector3 originalLocalPosition;
    private Vector3 originalLocalRotation;

    private int lastShotCount;
    private int lastErrorCount;

    private float recoilTimer;
    private bool isRecoiling;

    private float errorShakeTimer;
    private float errorShakeProgress;

    private void Awake()
    {
        originalLocalPosition = transform.localPosition;
        originalLocalRotation = transform.localEulerAngles;

        lastShotCount = shotgun.GetShotCount();
        lastErrorCount = shotgun.GetErrorCount();
    }

    private void Update()
    {
        UpdateRecoil();
        UpdateReloadAnimation();
        UpdateErrorShake();
    }

    private void UpdateRecoil()
    {
        int currentShotCount = shotgun.GetShotCount();

        if (currentShotCount != lastShotCount)
        {
            lastShotCount = currentShotCount;
            isRecoiling = true;
            recoilTimer = 0f;
            return;
        }

        if (!isRecoiling)
            return;

        recoilTimer += Time.deltaTime;

        if (recoilTimer <= recoilDuration)
        {
            UpdateRecoilMovement();
            return;
        }

        UpdateRecoilReturn();
    }

    private void UpdateRecoilMovement()
    {
        float progress = recoilTimer / recoilDuration;
        float currentZ = Mathf.Lerp(originalLocalPosition.z, originalLocalPosition.z - recoilDistance, progress);
        float currentX = Mathf.Lerp(originalLocalRotation.x, originalLocalRotation.x + recoilAngle, progress);

        SetLocalPositionZ(currentZ);
        SetLocalRotationX(currentX);
    }

    private void UpdateRecoilReturn()
    {
        float returnTimer = recoilTimer - recoilDuration;
        float progress = returnTimer / recoilReturnDuration;

        if (progress >= 1f)
        {
            progress = 1f;
            isRecoiling = false;
        }

        float currentZ = Mathf.Lerp(originalLocalPosition.z - recoilDistance, originalLocalPosition.z, progress);
        float currentX = Mathf.Lerp(originalLocalRotation.x + recoilAngle, originalLocalRotation.x, progress);

        SetLocalPositionZ(currentZ);
        SetLocalRotationX(currentX);
    }

    private void UpdateReloadAnimation()
    {
        Shotgun.ReloadPhase reloadPhase = shotgun.GetReloadPhase();

        if (reloadPhase == Shotgun.ReloadPhase.None)
        {
            if (isRecoiling)
                return;

            ResetRotation();
            return;
        }

        float progress = shotgun.GetReloadPhaseProgress();

        switch (reloadPhase)
        {
            case Shotgun.ReloadPhase.Lowering:
                UpdateLowering(progress);
                break;

            case Shotgun.ReloadPhase.RotatingDown:
                UpdateRotatingDown(progress);
                break;

            case Shotgun.ReloadPhase.Loading:
                UpdateLoading();
                break;

            case Shotgun.ReloadPhase.RotatingUp:
                UpdateRotatingUp(progress);
                break;

            case Shotgun.ReloadPhase.Raising:
                UpdateRaising(progress);
                break;
        }
    }

    private void UpdateLowering(float progress)
    {
        float rotationX = Mathf.Lerp(originalLocalRotation.x, originalLocalRotation.x + reloadTiltAngle, progress);

        SetLocalRotation(rotationX, originalLocalRotation.z);
    }

    private void UpdateRotatingDown(float progress)
    {
        float rotationZ = Mathf.Lerp(originalLocalRotation.z, originalLocalRotation.z + reloadRotationAngle, progress);

        SetLocalRotation(originalLocalRotation.x + reloadTiltAngle, rotationZ);
    }

    private void UpdateLoading()
    {
        float rotationX = originalLocalRotation.x + reloadTiltAngle;
        float rotationZ = originalLocalRotation.z + reloadRotationAngle;

        SetLocalRotation(rotationX, rotationZ);
    }

    private void UpdateRotatingUp(float progress)
    {
        float rotationZ = Mathf.Lerp(originalLocalRotation.z + reloadRotationAngle, originalLocalRotation.z, progress);

        SetLocalRotation(originalLocalRotation.x + reloadTiltAngle, rotationZ);
    }

    private void UpdateRaising(float progress)
    {
        float rotationX = Mathf.Lerp(originalLocalRotation.x + reloadTiltAngle, originalLocalRotation.x, progress);

        SetLocalRotation(rotationX, originalLocalRotation.z);
    }

    private void UpdateErrorShake()
    {
        int currentErrorCount = shotgun.GetErrorCount();

        if (currentErrorCount != lastErrorCount)
        {
            lastErrorCount = currentErrorCount;
            errorShakeTimer = errorShakeDuration;
            errorShakeProgress = 0f;
        }

        if (errorShakeTimer <= 0f)
            return;

        errorShakeTimer -= Time.deltaTime;
        errorShakeProgress += Time.deltaTime * errorShakeSpeed;

        float shakeOffset = Mathf.Sin(errorShakeProgress) * errorShakeDistance;

        SetLocalPositionX(originalLocalPosition.x + shakeOffset);

        if (errorShakeTimer > 0f)
            return;

        errorShakeTimer = 0f;
        SetLocalPositionX(originalLocalPosition.x);
    }

    private void SetLocalPositionX(float x)
    {
        transform.localPosition = new Vector3(x, transform.localPosition.y, transform.localPosition.z);
    }

    private void SetLocalPositionZ(float z)
    {
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, z);
    }

    private void SetLocalRotationX(float x)
    {
        transform.localRotation = Quaternion.Euler(x, originalLocalRotation.y, transform.localEulerAngles.z);
    }

    private void SetLocalRotation(float x, float z)
    {
        transform.localRotation = Quaternion.Euler(x, originalLocalRotation.y, z);
    }

    private void ResetRotation()
    {
        SetLocalRotation(originalLocalRotation.x, originalLocalRotation.z);
    }
}