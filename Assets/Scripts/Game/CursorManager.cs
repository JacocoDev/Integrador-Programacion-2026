using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameManager gameManager;

    private void Awake()
    {
        LockCursor();
    }

    private void Update()
    {
        if (gameManager.IsGameActive())
            return;

        UnlockCursor();
        enabled = false;
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}