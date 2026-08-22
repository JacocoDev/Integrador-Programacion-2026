using UnityEngine;

public class SectorSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;

    [Header("Sector Configuration")]
    [SerializeField] private int sectorCount = 4;
    [SerializeField] private float sectorRadius = 12f;

    [Header("Sector Colors")]
    [SerializeField] private Color evenSectorColor = new(0.6f, 0.6f, 0.6f);
    [SerializeField] private Color oddSectorColor = new(0.5f, 0.5f, 0.5f);

    [Header("Feedback")]
    [SerializeField] private Color aimedColor = Color.yellow;
    [SerializeField] private Color shotColor = Color.red;
    [SerializeField] private float shotFlashDuration = 0.15f;

    private GameObject[] sectorObjects;
    private Renderer[] sectorRenderers;
    private Color[] originalColors;

    private int currentSector = -1;
    private int lastShotSector = -1;

    private float shotFlashTimer;
    private float initialPlayerRotationY;

    private void Awake()
    {
        if (sectorCount < 1)
            sectorCount = 1;

        initialPlayerRotationY = player.eulerAngles.y;

        CreateSectors();
    }

    private void Update()
    {
        UpdateCurrentSector();
        UpdateShotFlash();
        UpdateSectorVisuals();
    }

    private void CreateSectors()
    {
        sectorObjects = new GameObject[sectorCount];
        sectorRenderers = new Renderer[sectorCount];
        originalColors = new Color[sectorCount];

        float anglePerSector = 360f / sectorCount;

        for (int i = 0; i < sectorCount; i++)
        {
            GameObject sector = new($"Sector {i}");

            sector.transform.SetParent(transform);
            sector.transform.localPosition = Vector3.zero;

            float sectorStartAngle = -anglePerSector / 2f + i * anglePerSector;
            sector.transform.localRotation = Quaternion.Euler(0f, sectorStartAngle, 0f);

            MeshFilter meshFilter = sector.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = sector.AddComponent<MeshRenderer>();

            meshFilter.mesh = CreateSectorMesh(anglePerSector);

            Material material = new(Shader.Find("Universal Render Pipeline/Lit"));
            Color sectorColor = GetSectorColor(i);

            material.color = sectorColor;
            meshRenderer.material = material;

            sectorObjects[i] = sector;
            sectorRenderers[i] = meshRenderer;
            originalColors[i] = sectorColor;
        }
    }

    private Color GetSectorColor(int sectorIndex)
    {
        if (sectorIndex % 2 == 0)
            return evenSectorColor;

        return oddSectorColor;
    }

    private Mesh CreateSectorMesh(float angle)
    {
        Mesh mesh = new();

        int segments = 20;
        Vector3[] vertices = new Vector3[segments + 2];
        int[] triangles = new int[segments * 3];

        vertices[0] = Vector3.zero;

        for (int i = 0; i <= segments; i++)
        {
            float currentAngle = angle / segments * i;
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
        float relativeAngle = Mathf.DeltaAngle(initialPlayerRotationY, currentPlayerRotationY);
        float sectorAngle = 360f / sectorCount;
        float adjustedAngle = Mathf.Repeat(relativeAngle + sectorAngle / 2f, 360f);

        currentSector = Mathf.FloorToInt(adjustedAngle / sectorAngle);

        if (currentSector >= sectorCount)
            currentSector = sectorCount - 1;
    }

    private void UpdateShotFlash()
    {
        if (shotFlashTimer <= 0f)
            return;

        shotFlashTimer -= Time.deltaTime;

        if (shotFlashTimer > 0f)
            return;

        lastShotSector = -1;
    }

    private void UpdateSectorVisuals()
    {
        for (int i = 0; i < sectorRenderers.Length; i++)
        {
            if (i == lastShotSector)
            {
                sectorRenderers[i].material.color = shotColor;
                continue;
            }

            if (i == currentSector)
            {
                sectorRenderers[i].material.color = aimedColor;
                continue;
            }

            sectorRenderers[i].material.color = originalColors[i];
        }
    }

    public int ShootCurrentSector()
    {
        if (currentSector < 0)
            return -1;

        lastShotSector = currentSector;
        shotFlashTimer = shotFlashDuration;

        return currentSector;
    }

    public int GetCurrentSector()
    {
        return currentSector;
    }

    public int GetSectorCount()
    {
        return sectorCount;
    }

    public Transform GetSectorTransform(int sectorIndex)
    {
        return sectorObjects[sectorIndex].transform;
    }
}