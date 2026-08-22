using UnityEngine;
using UnityEngine.InputSystem;

public class Shotgun : MonoBehaviour
{
    public enum ReloadPhase
    {
        None,
        Lowering,
        RotatingDown,
        Loading,
        RotatingUp,
        Raising
    }

    [Header("References")]
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private SectorSystem sectorSystem;
    [SerializeField] private EnemySystem enemySystem;
    [SerializeField] private GameManager gameManager;

    [Header("Ammunition")]
    [SerializeField] private int currentAmmo;
    [SerializeField] private int maxAmmo = 6;

    [Header("Shooting")]
    [SerializeField] private float shotCooldown = 1f;

    [Header("Reloading")]
    [SerializeField] private float loweringDuration = 0.5f;
    [SerializeField] private float rotationDuration = 0.5f;
    [SerializeField] private float loadingDuration = 0.5f;
    [SerializeField] private float raisingDuration = 0.5f;

    private InputAction fireAction;
    private InputAction reloadAction;

    private int shotCount;
    private int errorCount;

    private float shotCooldownTimer;
    private float reloadPhaseProgress;

    private ReloadPhase reloadPhase;

    private bool isReloadReversing;
    private bool hasReloadRequest;

    private void Awake()
    {
        ValidateConfiguration();

        fireAction = playerInput.actions["Fire"];
        reloadAction = playerInput.actions["Reload"];

        currentAmmo = maxAmmo;
    }

    private void Update()
    {
        if (!gameManager.IsGameActive())
            return;

        UpdateTimers();
        HandleShootingInput();
        HandleReloadInput();
        UpdateReload();
    }

    private void UpdateTimers()
    {
        if (shotCooldownTimer <= 0f)
            return;

        shotCooldownTimer -= Time.deltaTime;
    }

    private void HandleShootingInput()
    {
        if (!fireAction.WasPressedThisFrame())
            return;

        if (reloadPhase != ReloadPhase.None)
            return;

        if (currentAmmo <= 0)
        {
            errorCount++;
            return;
        }

        if (shotCooldownTimer > 0f)
            return;

        Shoot();
    }

    private void HandleReloadInput()
    {
        if (!reloadAction.WasPressedThisFrame())
            return;

        if (currentAmmo >= maxAmmo)
        {
            errorCount++;
            return;
        }

        if (shotCooldownTimer > 0f)
            return;

        if (reloadPhase == ReloadPhase.None)
        {
            StartReload();
            return;
        }

        if (reloadPhase == ReloadPhase.RotatingUp || reloadPhase == ReloadPhase.Raising)
        {
            hasReloadRequest = true;
            isReloadReversing = true;
        }
    }

    private void StartReload()
    {
        reloadPhase = ReloadPhase.Lowering;
        reloadPhaseProgress = 0f;
        isReloadReversing = false;
        hasReloadRequest = false;
    }

    private void UpdateReload()
    {
        if (reloadPhase == ReloadPhase.None)
            return;

        switch (reloadPhase)
        {
            case ReloadPhase.Lowering:
                UpdateLowering();
                break;

            case ReloadPhase.RotatingDown:
                UpdateRotatingDown();
                break;

            case ReloadPhase.Loading:
                UpdateLoading();
                break;

            case ReloadPhase.RotatingUp:
                UpdateRotatingUp();
                break;

            case ReloadPhase.Raising:
                UpdateRaising();
                break;
        }
    }

    private void UpdateLowering()
    {
        reloadPhaseProgress += Time.deltaTime / loweringDuration;

        if (reloadPhaseProgress < 1f)
            return;

        reloadPhaseProgress = 0f;
        reloadPhase = ReloadPhase.RotatingDown;
    }

    private void UpdateRotatingDown()
    {
        reloadPhaseProgress += Time.deltaTime / rotationDuration;

        if (reloadPhaseProgress < 1f)
            return;

        reloadPhaseProgress = 0f;
        reloadPhase = ReloadPhase.Loading;
    }

    private void UpdateLoading()
    {
        reloadPhaseProgress += Time.deltaTime / loadingDuration;

        if (reloadPhaseProgress < 1f)
            return;

        reloadPhaseProgress = 1f;
        currentAmmo++;

        if (currentAmmo > maxAmmo)
            currentAmmo = maxAmmo;

        if (currentAmmo >= maxAmmo)
        {
            hasReloadRequest = false;
            isReloadReversing = false;
            StartRotatingUp();
            return;
        }

        if (reloadAction.IsPressed())
        {
            reloadPhaseProgress = 0f;
            hasReloadRequest = false;
            return;
        }

        if (hasReloadRequest)
        {
            reloadPhaseProgress = 0f;
            hasReloadRequest = false;
            return;
        }

        StartRotatingUp();
    }

    private void UpdateRotatingUp()
    {
        if (isReloadReversing)
        {
            reloadPhaseProgress -= Time.deltaTime / rotationDuration;

            if (reloadPhaseProgress > 0f)
                return;

            reloadPhaseProgress = 0f;
            reloadPhase = ReloadPhase.Loading;
            isReloadReversing = false;
            hasReloadRequest = false;

            return;
        }

        reloadPhaseProgress += Time.deltaTime / rotationDuration;

        if (reloadPhaseProgress < 1f)
            return;

        reloadPhaseProgress = 1f;

        if (currentAmmo >= maxAmmo)
        {
            hasReloadRequest = false;
            isReloadReversing = false;
            StartRaising();
            return;
        }

        if (hasReloadRequest || reloadAction.IsPressed())
        {
            isReloadReversing = true;
            return;
        }

        StartRaising();
    }

    private void UpdateRaising()
    {
        if (isReloadReversing)
        {
            reloadPhaseProgress -= Time.deltaTime / raisingDuration;

            if (reloadPhaseProgress > 0f)
                return;

            reloadPhaseProgress = 0f;
            reloadPhase = ReloadPhase.RotatingUp;
            reloadPhaseProgress = 1f;
            isReloadReversing = true;

            return;
        }

        reloadPhaseProgress += Time.deltaTime / raisingDuration;

        if (reloadPhaseProgress < 1f)
            return;

        reloadPhase = ReloadPhase.None;
        reloadPhaseProgress = 0f;
        isReloadReversing = false;
        hasReloadRequest = false;
    }

    private void StartRotatingUp()
    {
        reloadPhase = ReloadPhase.RotatingUp;
        reloadPhaseProgress = 0f;
        isReloadReversing = false;
    }

    private void StartRaising()
    {
        reloadPhase = ReloadPhase.Raising;
        reloadPhaseProgress = 0f;
        isReloadReversing = false;
    }

    private void Shoot()
    {
        currentAmmo--;
        shotCooldownTimer = shotCooldown;
        shotCount++;

        int sectorIndex = sectorSystem.ShootCurrentSector();

        if (sectorIndex < 0)
            return;

        enemySystem.ShootSector(sectorIndex);
    }

    private void ValidateConfiguration()
    {
        if (maxAmmo < 1)
            maxAmmo = 1;

        if (shotCooldown < 0f)
            shotCooldown = 0f;

        if (loweringDuration <= 0f)
            loweringDuration = 0.01f;

        if (rotationDuration <= 0f)
            rotationDuration = 0.01f;

        if (loadingDuration <= 0f)
            loadingDuration = 0.01f;

        if (raisingDuration <= 0f)
            raisingDuration = 0.01f;
    }

    public ReloadPhase GetReloadPhase()
    {
        return reloadPhase;
    }

    public float GetReloadPhaseProgress()
    {
        return Mathf.Clamp01(reloadPhaseProgress);
    }

    public bool IsReloadReversing()
    {
        return isReloadReversing;
    }

    public int GetShotCount()
    {
        return shotCount;
    }

    public int GetErrorCount()
    {
        return errorCount;
    }

    public int GetCurrentAmmo()
    {
        return currentAmmo;
    }

    public int GetMaxAmmo()
    {
        return maxAmmo;
    }
}