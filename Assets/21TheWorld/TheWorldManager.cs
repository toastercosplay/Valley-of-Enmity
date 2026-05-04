using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// top-level controller for "the world" minigame. each player gets their own
// instanced arena, camera viewport, and cow herd. arenas live at the same
// world-space coordinates but on different layers, so a per-camera culling
// mask makes each player only see their own arena. this lets us run n
// independent races on one scene without spatial separation.
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
    // these must exist in project settings → tags and layers. each player's
    // arena and camera get assigned the matching layer to isolate visuals/physics.
    public string[] playerLayerNames = { "WorldP1", "WorldP2", "WorldP3", "WorldP4" };

    private int playerCount;
    private PlayerHerdInstance[] playerInstances;
    // Append-only finish log used to compute placement (1st/2nd/...) by index.
    private readonly List<(int playerIndex, float time)> finishOrder = new List<(int, float)>();
    private TMP_Text rankingText;
    private bool allFinished;

    void Start()
    {
        // player count resolution priority: inspector override (test path) → live gamemanager → safe default.
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

        // the scene's default main camera would render every layer at full screen
        // and overwrite the split-screen viewports, so we destroy it before spawning per-player cameras.
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

    // gamemanager exposes player slots as nullable references whose activeself
    // tells us how many joined in the lobby. p1/p2 are always assumed present.
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

    // builds the per-player slice of the scene: arena prefab, walls, isolated
    // camera, the player avatar, and a fresh herd of cows tied to that player.
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

        // all arenas are spawned at the origin; layer-based culling keeps them visually separate.
        GameObject arena = Instantiate(herdArenaPrefab, Vector3.zero, Quaternion.identity);
        arena.name = $"HerdArena_P{playerIndex + 1}";
        SetLayerRecursive(arena, layer);

        GameObject camObj = new GameObject($"Camera_P{playerIndex + 1}");
        Camera cam = camObj.AddComponent<Camera>();
        cam.orthographic = true;
        cam.orthographicSize = CalculateOrthoSizeToFitField(viewportRect);
        cam.transform.position = new Vector3(0f, 0f, -10f);
        cam.rect = viewportRect;
        // bitmask with only this player's layer set — camera renders nothing else.
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

        // each player binds to a dedicated action map (player1..player4) so input devices don't cross over.
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

        // playerherdinstance is attached at runtime so all per-player state lives on the arena root.
        PlayerHerdInstance instance = arena.AddComponent<PlayerHerdInstance>();
        instance.playerIndex = playerIndex;
        instance.myCows = cows;
        instance.onPlayerFinished = OnPlayerFinished;
        playerInstances[playerIndex] = instance;
    }

    // builds the four invisible boundary colliders that keep player and cows
    // inside the arena. walls overlap at corners so there's no diagonal gap.
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

    // distributes cowcount cows roughly evenly across the field using a
    // shuffled grid (jittered cells) so spawns are non-clumpy but reproducibly
    // spread out. each cow also receives a unique angularoffset so they fan
    // out into a ring once recruited.
    CowController[] SpawnCows(Transform parent, GameObject playerGO, int layer)
    {
        // square-ish grid that's at least large enough to hold cowcount cells.
        int cols = Mathf.CeilToInt(Mathf.Sqrt(cowCount));
        int rows = Mathf.CeilToInt((float)cowCount / cols);
        int actualCowCount = Mathf.Min(cowCount, cols * rows);
        // clamp padding so a misconfigured value can't push spawn bounds inside-out.
        float safeSpawnPaddingX = Mathf.Clamp(cowSpawnBorderPadding, 0f, Mathf.Max(0f, fieldHalfWidth - 0.01f));
        float safeSpawnPaddingY = Mathf.Clamp(cowSpawnBorderPadding, 0f, Mathf.Max(0f, fieldHalfHeight - 0.01f));

        float cellW = (fieldHalfWidth * 2f) / cols;
        float cellH = (fieldHalfHeight * 2f) / rows;

        List<int> cellIndices = new List<int>(cols * rows);
        for (int i = 0; i < cols * rows; i++)
        {
            cellIndices.Add(i);
        }

        // fisher-yates shuffle so when actualcowcount < cell count, the
        // unused cells are random rather than always the last few in row-major order.
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

            // place at cell center plus jitter (±30% of cell size), then clamp to the safe spawn rectangle.
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

            // evenly distribute angles around the player so the herd forms a full ring.
            cow.angularOffset = (Mathf.PI * 2f * i) / actualCowCount;
            cow.Init(playerGO.transform);
            cows[i] = cow;
        }

        return cows;
    }

    // returns normalized (0..1) viewport rectangles for split-screen layouts.
    // 2 players: vertical split. 3 players: top half split in two, bottom full-width. 4 players: 2x2 grid.
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

    // picks an ortho size big enough that the entire field (plus walls) is
    // visible in the given viewport regardless of its aspect ratio. narrow
    // viewports (e.g. half-screen) need a larger ortho size to fit width.
    float CalculateOrthoSizeToFitField(Rect viewportRect)
    {
        float viewportAspect = GetViewportAspect(viewportRect);
        float safeAspect = Mathf.Max(0.0001f, viewportAspect);
        float targetHalfWidth = fieldHalfWidth + wallThickness;
        float targetHalfHeight = fieldHalfHeight + wallThickness;
        // ortho size is half-height; convert width into the equivalent half-height via aspect.
        float requiredForWidth = targetHalfWidth / safeAspect;
        return Mathf.Max(cameraOrthoSize, targetHalfHeight, requiredForWidth);
    }

    float GetViewportAspect(Rect viewportRect)
    {
        // mathf.max guards against zero-pixel viewports causing divide-by-zero downstream.
        float viewportPixelWidth = Mathf.Max(1f, Screen.width * viewportRect.width);
        float viewportPixelHeight = Mathf.Max(1f, Screen.height * viewportRect.height);
        return viewportPixelWidth / viewportPixelHeight;
    }

    // callback registered on each playerherdinstance. order of arrival
    // dictates placement, and once everyone has finished we kick off the end-of-round flow.
    void OnPlayerFinished(int playerIndex, float time)
    {
        finishOrder.Add((playerIndex, time));

        int place = finishOrder.Count;
        string suffix = place == 1 ? "st" : place == 2 ? "nd" : place == 3 ? "rd" : "th";
        Debug.Log($"P{playerIndex + 1} finished {place}{suffix}! ({time:F2}s)");

        UpdateRankingDisplay();

        // guard against the unlikely double-fire while the end coroutine is queued.
        if (finishOrder.Count >= playerCount && !allFinished)
        {
            allFinished = true;
            StartCoroutine(EndGameRoutine());
        }
    }

    // builds a single screen-space-overlay canvas that floats above all
    // player viewports and shows finishing places as players complete.
    void CreateRankingUI()
    {
        GameObject overlayCanvas = new GameObject("RankingCanvas");
        Canvas canvas = overlayCanvas.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        // high sorting order keeps the leaderboard above any per-player ui.
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

    // brief pause after the last finisher so players can see the final ranking before the scene transitions.
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

    // walks the hierarchy and assigns the same layer to every child. necessary
    // because per-player culling masks rely on the whole arena/player tree being on one layer.
    void SetLayerRecursive(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursive(child.gameObject, layer);
        }
    }

    // multiplies the prefab's authored scale rather than overwriting it, so any
    // non-uniform scaling baked into the prefab (e.g. flipped sprites) is preserved.
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
