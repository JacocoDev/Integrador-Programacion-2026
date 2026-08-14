using UnityEngine;
using UnityEngine.InputSystem;

public class SectorSystem : MonoBehaviour
{
    [Header("Sector Configuration")]
    [SerializeField] private int sectorCount = 4;
    [SerializeField] private float sectorRadius = 10f;

    [Header("Sector Colors")]
    [SerializeField] private Color evenSectorColor = new Color(0.6f, 0.6f, 0.6f);
    [SerializeField] private Color oddSectorColor = new Color(0.5f, 0.5f, 0.5f);

    [Header("Feedback")]
    [SerializeField] private Color aimedColor = Color.yellow;
    [SerializeField] private Color shotColor = Color.red;
    [SerializeField] private float shotFlashDuration = 0.15f;

    [Header("References")]
    [SerializeField] private Transform player;

    private GameObject[] sectorObjects;
    private Renderer[] sectorRenderers;
    private Color[] originalColors;

    private int currentSector = -1;
    private int lastShotSector = -1;

    private float shotFlashTimer;

    private PlayerInput playerInput;
    private InputAction fireAction;

    private float initialPlayerRotationY;

    private void Awake()
    {
        ValidateSectorCount();

        if (player == null)
        {
            Debug.LogError("SectorSystem: No se asignó el Player.");
            return;
        }

        playerInput = player.GetComponent<PlayerInput>();

        if (playerInput == null)
        {
            Debug.LogError("SectorSystem: El Player no tiene un componente PlayerInput.");
            return;
        }

        fireAction = playerInput.actions["Fire"];

        if (fireAction == null)
        {
            Debug.LogError("SectorSystem: No se encontró la Action 'Fire'.");
        }

        initialPlayerRotationY = player.eulerAngles.y;

        CreateSectors();
    }

    private void Update()
    {
        if (player == null)
            return;

        UpdateCurrentSector();
        UpdateShotFlash();
        UpdateSectorVisuals();

        if (fireAction != null && fireAction.WasPressedThisFrame())
        {
            Shoot();
        }
    }

    private void ValidateSectorCount()
    {
        if (sectorCount < 1)
        {
            sectorCount = 1;
        }
    }

    private void CreateSectors()
    {
        sectorObjects = new GameObject[sectorCount];
        sectorRenderers = new Renderer[sectorCount];
        originalColors = new Color[sectorCount];

        float anglePerSector = 360f / sectorCount;

        for (int i = 0; i < sectorCount; i++)
        {
            GameObject sector = new GameObject($"Sector {i}");

            sector.transform.SetParent(transform);
            sector.transform.localPosition = Vector3.zero;

            float sectorStartAngle =
                -anglePerSector / 2f + i * anglePerSector;

            sector.transform.localRotation =
                Quaternion.Euler(0f, sectorStartAngle, 0f);

            MeshFilter meshFilter = sector.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = sector.AddComponent<MeshRenderer>();

            meshFilter.mesh = CreateSectorMesh(anglePerSector);

            Material material = new Material(
                Shader.Find("Universal Render Pipeline/Lit")
            );

            Color sectorColor;

            if (i % 2 == 0)
            {
                sectorColor = evenSectorColor;
            }
            else
            {
                sectorColor = oddSectorColor;
            }

            material.color = sectorColor;

            meshRenderer.material = material;

            sectorObjects[i] = sector;
            sectorRenderers[i] = meshRenderer;
            originalColors[i] = sectorColor;
        }
    }

    private Mesh CreateSectorMesh(float angle)
    {
        Mesh mesh = new Mesh();

        int segments = 20;

        Vector3[] vertices = new Vector3[segments + 2];
        int[] triangles = new int[segments * 3];

        vertices[0] = Vector3.zero;

        for (int i = 0; i <= segments; i++)
        {
            float currentAngle = (angle / segments) * i;

            vertices[i + 1] = GetPointOnCircle(currentAngle);
        }

        for (int i = 0; i < segments; i++)
        {
            int triangleIndex = i * 3;

            triangles[triangleIndex] = 0;
            triangles[triangleIndex + 1] = i + 1;
            triangles[triangleIndex + 2] = i + 2;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();

        return mesh;
    }

    private Vector3 GetPointOnCircle(float angle)
    {
        float radians = angle * Mathf.Deg2Rad;

        float x = Mathf.Sin(radians) * sectorRadius;
        float z = Mathf.Cos(radians) * sectorRadius;

        return new Vector3(x, 0f, z);
    }

private void UpdateCurrentSector()
{
    float currentPlayerRotationY = player.eulerAngles.y;

    float relativeAngle =
        Mathf.DeltaAngle(initialPlayerRotationY, currentPlayerRotationY);

    float sectorAngle = 360f / sectorCount;

    float adjustedAngle =
        relativeAngle + sectorAngle / 2f;

    adjustedAngle = Mathf.Repeat(adjustedAngle, 360f);

    currentSector =
        Mathf.FloorToInt(adjustedAngle / sectorAngle);

    if (currentSector >= sectorCount)
    {
        currentSector = sectorCount - 1;
    }
}

    private void Shoot()
    {
        if (currentSector < 0)
            return;

        lastShotSector = currentSector;

        shotFlashTimer = shotFlashDuration;
    }

    private void UpdateShotFlash()
    {
        if (shotFlashTimer <= 0f)
            return;

        shotFlashTimer -= Time.deltaTime;

        if (shotFlashTimer <= 0f)
        {
            lastShotSector = -1;
        }
    }

    private void UpdateSectorVisuals()
    {
        if (sectorRenderers == null)
            return;

        for (int i = 0; i < sectorRenderers.Length; i++)
        {
            if (i == lastShotSector)
            {
                sectorRenderers[i].material.color = shotColor;
            }
            else if (i == currentSector)
            {
                sectorRenderers[i].material.color = aimedColor;
            }
            else
            {
                sectorRenderers[i].material.color = originalColors[i];
            }
        }
    }

    public int GetCurrentSector()
    {
        return currentSector;
    }

    public int GetSectorCount()
    {
        return sectorCount;
    }
}