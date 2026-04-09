using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TheWorldManager : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject herdArenaPrefab;
    public GameObject cowPrefab;
    public GameObject[] playerPrefabs;

    [Header("Field Settings")]
    public float fieldHalfWidth = 10f;
    public float fieldHalfHeight = 7f;
    public int cowCount = 8;
    public float wallThickness = 0.5f;
    [Tooltip("Minimum distance from spawn position to the inner movement border.")]
    public float cowSpawnBorderPadding = 1.75f;

    [Header("Camera")]
    [Tooltip("Minimum orthographic size. Cameras may zoom out further to keep borders inside the viewport.")]
    public float cameraOrthoSize = 9f;

    [Header("Settings")]
    [Tooltip("If 0, reads from GameManager.Instance. Set manually for testing.")]
    public int playerCountOverride = 0;
    [Tooltip("Multiplier applied to player and cow prefab scale on spawn.")]
    public float spawnScaleMultiplier = 0.2f;

    [Header("Layers (must match Project Settings)")]
    public string[] playerLayerNames = { "WorldP1", "WorldP2", "WorldP3", "WorldP4" };

    private int playerCount;
    private PlayerHerdInstance[] playerInstances;
    private readonly List<(int playerIndex, float time)> finishOrder = new List<(int, float)>();
    private TMP_Text rankingText;
    private bool allFinished;

    void Start()
    {
        if (playerCountOverride > 0)
        {
            playerCount = playerCountOverride;
        }
        else if (GameManager.Instance != null)
        {
            playerCount = GetPlayerCountFromGameManager();
        }
        else
        {
            playerCount = 2;
        }

        playerCount = Mathf.Clamp(playerCount, 2, 4);

        Camera mainCam = Camera.main;
        if (mainCam != null)
        {
            Destroy(mainCam.gameObject);
        }

        CreateRankingUI();

        playerInstances = new PlayerHerdInstance[playerCount];
        Rect[] viewports = GetViewportRects(playerCount);
        for (int i = 0; i < playerCount; i++)
        {
            SetupPlayer(i, viewports[i]);
        }
    }

    int GetPlayerCountFromGameManager()
    {
        int count = 2;
        if (GameManager.Instance.player3 != null && GameManager.Instance.player3.gameObject.activeSelf)
        {
            count = 3;
        }

        if (GameManager.Instance.player4 != null && GameManager.Instance.player4.gameObject.activeSelf)
        {
            count = 4;
        }

        return count;
    }

    void SetupPlayer(int playerIndex, Rect viewportRect)
    {
        int layer = LayerMask.NameToLayer(playerLayerNames[playerIndex]);
        if (layer == -1)
        {
            Debug.LogError($"Layer '{playerLayerNames[playerIndex]}' not found! Add it in Edit > Project Settings > Tags and Layers.");
            return;
        }

        if (herdArenaPrefab == null)
        {
            Debug.LogError("Herd Arena Prefab is missing on TheWorldManager.");
            return;
        }

        if (cowPrefab == null)
        {
            Debug.LogError("Cow Prefab is missing on TheWorldManager.");
            return;
        }

        if (playerPrefabs == null || playerPrefabs.Length == 0 || playerPrefabs[0] == null)
        {
            Debug.LogError($"Player prefab for P{playerIndex + 1} is missing on TheWorldManager.");
            return;
        }

        GameObject arena = Instantiate(herdArenaPrefab, Vector3.zero, Quaternion.identity);
        arena.name = $"HerdArena_P{playerIndex + 1}";
        SetLayerRecursive(arena, layer);

        GameObject camObj = new GameObject($"Camera_P{playerIndex + 1}");
        Camera cam = camObj.AddComponent<Camera>();
        cam.orthographic = true;
        cam.orthographicSize = CalculateOrthoSizeToFitField(viewportRect);
        cam.transform.position = new Vector3(0f, 0f, -10f);
        cam.rect = viewportRect;
        cam.cullingMask = 1 << layer;
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = new Color(0.3f, 0.5f, 0.2f);
        cam.depth = playerIndex;

        SpawnBoundaryWalls(arena.transform, layer);

        GameObject playerGO = InstantiatePlayerPrefab(playerPrefabs[0], arena.transform, playerIndex + 1);
        if (playerGO == null)
        {
            return;
        }

        PlayerInput playerInput = playerGO.GetComponent<PlayerInput>();
        if (playerInput != null)
        {
            string actionMap = $"Player{playerIndex + 1}";
            playerInput.defaultActionMap = actionMap;
            playerInput.SwitchCurrentActionMap(actionMap);
        }

        ApplySpawnScale(playerGO);
        playerGO.name = $"P{playerIndex + 1}";
        SetLayerRecursive(playerGO, layer);

        CowController[] cows = SpawnCows(arena.transform, playerGO, layer);

        PlayerHerdInstance instance = arena.AddComponent<PlayerHerdInstance>();
        instance.playerIndex = playerIndex;
        instance.myCows = cows;
        instance.onPlayerFinished = OnPlayerFinished;
        playerInstances[playerIndex] = instance;
    }

    void SpawnBoundaryWalls(Transform parent, int layer)
    {
        float t = wallThickness;

        CreateWall("WallTop", parent, layer, new Vector2(0f, fieldHalfHeight + t * 0.5f), new Vector2(fieldHalfWidth * 2f + t * 2f, t));
        CreateWall("WallBottom", parent, layer, new Vector2(0f, -fieldHalfHeight - t * 0.5f), new Vector2(fieldHalfWidth * 2f + t * 2f, t));
        CreateWall("WallLeft", parent, layer, new Vector2(-fieldHalfWidth - t * 0.5f, 0f), new Vector2(t, fieldHalfHeight * 2f));
        CreateWall("WallRight", parent, layer, new Vector2(fieldHalfWidth + t * 0.5f, 0f), new Vector2(t, fieldHalfHeight * 2f));
    }

    void CreateWall(string name, Transform parent, int layer, Vector2 localPosition, Vector2 size)
    {
        GameObject wall = new GameObject(name);
        wall.transform.SetParent(parent, false);
        wall.transform.localPosition = new Vector3(localPosition.x, localPosition.y, 0f);

        BoxCollider2D col = wall.AddComponent<BoxCollider2D>();
        col.size = size;
        SetLayerRecursive(wall, layer);
    }

    CowController[] SpawnCows(Transform parent, GameObject playerGO, int layer)
    {
        int cols = Mathf.CeilToInt(Mathf.Sqrt(cowCount));
        int rows = Mathf.CeilToInt((float)cowCount / cols);
        int actualCowCount = Mathf.Min(cowCount, cols * rows);
        float safeSpawnPaddingX = Mathf.Clamp(cowSpawnBorderPadding, 0f, Mathf.Max(0f, fieldHalfWidth - 0.01f));
        float safeSpawnPaddingY = Mathf.Clamp(cowSpawnBorderPadding, 0f, Mathf.Max(0f, fieldHalfHeight - 0.01f));

        float cellW = (fieldHalfWidth * 2f) / cols;
        float cellH = (fieldHalfHeight * 2f) / rows;

        List<int> cellIndices = new List<int>(cols * rows);
        for (int i = 0; i < cols * rows; i++)
        {
            cellIndices.Add(i);
        }

        for (int i = cellIndices.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (cellIndices[i], cellIndices[j]) = (cellIndices[j], cellIndices[i]);
        }

        CowController[] cows = new CowController[actualCowCount];
        for (int i = 0; i < actualCowCount; i++)
        {
            int cellIndex = cellIndices[i];
            int c = cellIndex % cols;
            int r = cellIndex / cols;

            float x = -fieldHalfWidth + c * cellW + cellW * 0.5f + Random.Range(-cellW * 0.3f, cellW * 0.3f);
            float y = -fieldHalfHeight + r * cellH + cellH * 0.5f + Random.Range(-cellH * 0.3f, cellH * 0.3f);
            x = Mathf.Clamp(x, -fieldHalfWidth + safeSpawnPaddingX, fieldHalfWidth - safeSpawnPaddingX);
            y = Mathf.Clamp(y, -fieldHalfHeight + safeSpawnPaddingY, fieldHalfHeight - safeSpawnPaddingY);

            GameObject cowObj = Instantiate(cowPrefab, new Vector3(x, y, 0f), Quaternion.identity, parent);
            ApplySpawnScale(cowObj);
            SetLayerRecursive(cowObj, layer);

            CowController cow = cowObj.GetComponent<CowController>();
            if (cow == null)
            {
                Debug.LogError("Cow prefab is missing CowController.");
                continue;
            }

            cow.angularOffset = (Mathf.PI * 2f * i) / actualCowCount;
            cow.Init(playerGO.transform);
            cows[i] = cow;
        }

        return cows;
    }

    Rect[] GetViewportRects(int count)
    {
        if (count == 2)
        {
            return new Rect[]
            {
                new Rect(0f, 0f, 0.5f, 1f),
                new Rect(0.5f, 0f, 0.5f, 1f)
            };
        }

        if (count == 3)
        {
            return new Rect[]
            {
                new Rect(0f, 0.5f, 0.5f, 0.5f),
                new Rect(0.5f, 0.5f, 0.5f, 0.5f),
                new Rect(0f, 0f, 1f, 0.5f)
            };
        }

        return new Rect[]
        {
            new Rect(0f, 0.5f, 0.5f, 0.5f),
            new Rect(0.5f, 0.5f, 0.5f, 0.5f),
            new Rect(0f, 0f, 0.5f, 0.5f),
            new Rect(0.5f, 0f, 0.5f, 0.5f)
        };
    }

    float CalculateOrthoSizeToFitField(Rect viewportRect)
    {
        float viewportAspect = GetViewportAspect(viewportRect);
        float safeAspect = Mathf.Max(0.0001f, viewportAspect);
        float targetHalfWidth = fieldHalfWidth + wallThickness;
        float targetHalfHeight = fieldHalfHeight + wallThickness;
        float requiredForWidth = targetHalfWidth / safeAspect;
        return Mathf.Max(cameraOrthoSize, targetHalfHeight, requiredForWidth);
    }

    float GetViewportAspect(Rect viewportRect)
    {
        float viewportPixelWidth = Mathf.Max(1f, Screen.width * viewportRect.width);
        float viewportPixelHeight = Mathf.Max(1f, Screen.height * viewportRect.height);
        return viewportPixelWidth / viewportPixelHeight;
    }

    void OnPlayerFinished(int playerIndex, float time)
    {
        finishOrder.Add((playerIndex, time));

        int place = finishOrder.Count;
        string suffix = place == 1 ? "st" : place == 2 ? "nd" : place == 3 ? "rd" : "th";
        Debug.Log($"P{playerIndex + 1} finished {place}{suffix}! ({time:F2}s)");

        UpdateRankingDisplay();

        if (finishOrder.Count >= playerCount && !allFinished)
        {
            allFinished = true;
            StartCoroutine(EndGameRoutine());
        }
    }

    void CreateRankingUI()
    {
        GameObject overlayCanvas = new GameObject("RankingCanvas");
        Canvas canvas = overlayCanvas.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 100;

        CanvasScaler scaler = overlayCanvas.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);

        GameObject textObj = new GameObject("RankingText");
        textObj.transform.SetParent(overlayCanvas.transform, false);

        rankingText = textObj.AddComponent<TextMeshProUGUI>();
        rankingText.fontSize = 48;
        rankingText.alignment = TextAlignmentOptions.Center;
        rankingText.color = Color.white;
        rankingText.enableWordWrapping = true;
        rankingText.outlineWidth = 0.2f;
        rankingText.outlineColor = Color.black;

        RectTransform rect = textObj.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.2f, 0.3f);
        rect.anchorMax = new Vector2(0.8f, 0.7f);
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        rankingText.text = string.Empty;
    }

    void UpdateRankingDisplay()
    {
        string display = string.Empty;
        for (int i = 0; i < finishOrder.Count; i++)
        {
            int place = i + 1;
            string suffix = place == 1 ? "st" : place == 2 ? "nd" : place == 3 ? "rd" : "th";
            display += $"{place}{suffix}: P{finishOrder[i].playerIndex + 1} - {finishOrder[i].time:F2}s\n";
        }

        if (rankingText != null)
        {
            rankingText.text = display;
        }
    }

    IEnumerator EndGameRoutine()
    {
        UpdateRankingDisplay();
        yield return new WaitForSeconds(3f);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.FinishMinigame();
        }
        else
        {
            Debug.LogError("No GameManager instance found when ending The World minigame.");
        }
    }

    void SetLayerRecursive(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursive(child.gameObject, layer);
        }
    }

    void ApplySpawnScale(GameObject spawnedObject)
    {
        if (spawnedObject == null)
        {
            return;
        }

        float scale = Mathf.Max(0f, spawnScaleMultiplier);
        spawnedObject.transform.localScale *= scale;
    }

    GameObject InstantiatePlayerPrefab(GameObject playerPrefabReference, Transform parent, int playerNumber)
    {
        if (playerPrefabReference == null)
        {
            Debug.LogError($"Player prefab reference for P{playerNumber} is missing on TheWorldManager.");
            return null;
        }

        return Instantiate(playerPrefabReference, Vector3.zero, Quaternion.identity, parent);
    }
}
