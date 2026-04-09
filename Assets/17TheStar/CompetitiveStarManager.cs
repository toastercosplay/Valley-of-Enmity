using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class CompetitiveStarManager : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject starArenaPrefab;
    public GameObject[] cursorPrefabs; // P1Cursor, P2Cursor, P3Cursor, P4Cursor

    [Header("Background")]
    public Sprite backgroundImage;

    [Header("Settings")]
    [Tooltip("If 0, reads from GameManager.Instance. Set manually for testing.")]
    public int playerCountOverride = 0;

    [Header("Layer Names (must match Project Settings)")]
    public string[] playerLayerNames = { "StarP1", "StarP2", "StarP3", "StarP4" };

    private int playerCount;
    private PlayerStarInstance[] playerInstances;
    private List<(int playerIndex, float time)> finishOrder = new List<(int, float)>();
    private TMP_Text rankingText;
    private bool allFinished;

    void Start()
    {
        // Determine player count
        if (playerCountOverride > 0)
            playerCount = playerCountOverride;
        else if (GameManager.Instance != null)
            playerCount = GetPlayerCountFromGameManager();
        else
            playerCount = 2; // fallback

        playerCount = Mathf.Clamp(playerCount, 2, 4);

        // Destroy existing main camera if present
        Camera mainCam = Camera.main;
        if (mainCam != null)
            Destroy(mainCam.gameObject);

        // Create background camera (renders behind all player cameras)
        // CreateBackgroundCamera();

        // Create ranking UI overlay
        CreateRankingUI();

        // Set up each player
        playerInstances = new PlayerStarInstance[playerCount];
        Rect[] viewports = GetViewportRects(playerCount);

        for (int i = 0; i < playerCount; i++)
        {
            SetupPlayer(i, viewports[i]);
        }
    }

    int GetPlayerCountFromGameManager()
    {
        // Count active players
        int count = 2; // minimum
        if (GameManager.Instance.player3 != null && GameManager.Instance.player3.gameObject.activeSelf)
            count = 3;
        if (GameManager.Instance.player4 != null && GameManager.Instance.player4.gameObject.activeSelf)
            count = 4;
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

        // Instantiate star arena
        GameObject arena = Instantiate(starArenaPrefab, Vector3.zero, Quaternion.identity);
        arena.name = $"StarArena_P{playerIndex + 1}";
        SetLayerRecursive(arena, layer);

        // Create camera
        GameObject camObj = new GameObject($"Camera_P{playerIndex + 1}");
        Camera cam = camObj.AddComponent<Camera>();
        cam.orthographic = true;
        cam.orthographicSize = 100;
        cam.transform.position = new Vector3(0, 0, -10);
        cam.rect = viewportRect;
        cam.cullingMask = 1 << layer; // player layer only
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.depth = playerIndex; // render order

        // Create canvas for cursor (Screen Space - Camera)
        GameObject canvasObj = new GameObject($"Canvas_P{playerIndex + 1}");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = cam;
        canvas.planeDistance = 1;

        CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);

        canvasObj.AddComponent<GraphicRaycaster>();

        // Set canvas to player layer so the player camera can see it
        SetLayerRecursive(canvasObj, layer);

        // Instantiate cursor as child of canvas
        if (playerIndex < cursorPrefabs.Length && cursorPrefabs[playerIndex] != null)
        {
            GameObject cursor = Instantiate(cursorPrefabs[playerIndex], canvasObj.transform);
            SetLayerRecursive(cursor, layer);
            cursor.name = $"P{playerIndex + 1}Cursor";

            CursorMovement cursorMovement = cursor.GetComponent<CursorMovement>();
            cursorMovement.SetMaxSpeed(70f);

            // Collect stars from the arena
            Star[] stars = arena.GetComponentsInChildren<Star>();

            // Add PlayerStarInstance to the arena
            PlayerStarInstance instance = arena.AddComponent<PlayerStarInstance>();
            instance.playerCamera = cam;
            instance.cursorMovement = cursorMovement;
            instance.myStars = stars;
            instance.playerIndex = playerIndex;
            instance.playerLayer = layer;
            instance.onPlayerFinished = OnPlayerFinished;

            playerInstances[playerIndex] = instance;
        }
        else
        {
            Debug.LogError($"Cursor prefab for player {playerIndex + 1} is missing!");
        }
    }

    Rect[] GetViewportRects(int count)
    {
        Rect[] fullGrid = new Rect[]
        {
            new Rect(0f, 0.5f, 0.5f, 0.5f),   // P1: upper-left
            new Rect(0.5f, 0.5f, 0.5f, 0.5f), // P2: upper-right
            new Rect(0f, 0f, 0.5f, 0.5f),     // P3: lower-left
            new Rect(0.5f, 0f, 0.5f, 0.5f)    // P4: lower-right
        };

        Rect[] rects = new Rect[count];
        for (int i = 0; i < count; i++)
        {
            rects[i] = fullGrid[i];
        }
        return rects;
    }

    void OnPlayerFinished(int playerIndex, float time)
    {
        finishOrder.Add((playerIndex, time));

        int place = finishOrder.Count;
        string suffix = place == 1 ? "st" : place == 2 ? "nd" : place == 3 ? "rd" : "th";
        string msg = $"P{playerIndex + 1} finished {place}{suffix}! ({time:F2}s)";
        Debug.Log(msg);

        UpdateRankingDisplay();

        if (finishOrder.Count >= playerCount)
        {
            allFinished = true;
            StartCoroutine(EndGameRoutine());
        }
    }

    void CreateRankingUI()
    {
        // Create overlay canvas for ranking text (renders on top of everything)
        GameObject overlayCanvas = new GameObject("RankingCanvas");
        Canvas canvas = overlayCanvas.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 100;

        CanvasScaler scaler = overlayCanvas.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);

        // Create text object
        GameObject textObj = new GameObject("RankingText");
        textObj.transform.SetParent(overlayCanvas.transform, false);

        rankingText = textObj.AddComponent<TextMeshProUGUI>();
        rankingText.fontSize = 48;
        rankingText.alignment = TextAlignmentOptions.Center;
        rankingText.color = Color.white;
        rankingText.enableWordWrapping = true;

        // Add outline for readability
        rankingText.outlineWidth = 0.2f;
        rankingText.outlineColor = Color.black;

        // Position at center of screen
        RectTransform rect = textObj.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.2f, 0.3f);
        rect.anchorMax = new Vector2(0.8f, 0.7f);
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        rankingText.text = "";
    }

    void UpdateRankingDisplay()
    {
        string display = "";
        for (int i = 0; i < finishOrder.Count; i++)
        {
            int place = i + 1;
            string suffix = place == 1 ? "st" : place == 2 ? "nd" : place == 3 ? "rd" : "th";
            display += $"{place}{suffix}: P{finishOrder[i].playerIndex + 1} - {finishOrder[i].time:F2}s\n";
        }
        rankingText.text = display;
    }

    IEnumerator EndGameRoutine()
    {
        // Show final rankings for 3 seconds
        UpdateRankingDisplay();
        yield return new WaitForSeconds(3f);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.FinishMinigame();
        }
    }

    void CreateBackgroundCamera()
    {
        // Background camera renders first at depth -1, full screen
        GameObject bgCamObj = new GameObject("BackgroundCamera");
        Camera bgCam = bgCamObj.AddComponent<Camera>();
        bgCam.orthographic = true;
        bgCam.orthographicSize = 100;
        bgCam.transform.position = new Vector3(0, 0, -10);
        bgCam.rect = new Rect(0f, 0f, 1f, 1f);
        bgCam.depth = -1;
        bgCam.clearFlags = CameraClearFlags.SolidColor;
        bgCam.backgroundColor = new Color(0.192f, 0.302f, 0.475f);
        bgCam.cullingMask = 1 << 0; // Default layer only

        if (backgroundImage != null)
        {
            // Create sprite on Default layer (player cameras don't see Default)
            GameObject bgObj = new GameObject("BackgroundSprite");
            bgObj.layer = 0;
            SpriteRenderer sr = bgObj.AddComponent<SpriteRenderer>();
            sr.sprite = backgroundImage;
            sr.sortingOrder = -1000;

            // Scale sprite to fill the camera view
            float camHeight = bgCam.orthographicSize * 2f;
            float camWidth = camHeight * bgCam.aspect;
            Vector2 spriteSize = sr.sprite.bounds.size;
            float scaleX = camWidth / spriteSize.x;
            float scaleY = camHeight / spriteSize.y;
            float scale = Mathf.Max(scaleX, scaleY); // cover entire screen
            Vector3 backgroundScale = new Vector3(scale, scale, 1f);
            bgObj.transform.localScale = backgroundScale;

            // Center the rendered sprite, even if the imported sprite pivot is not centered.
            Vector3 cameraCenter = bgCam.transform.position + new Vector3(0f, 0f, 10f);
            Vector3 spriteCenterOffset = Vector3.Scale(sr.sprite.bounds.center, backgroundScale);
            bgObj.transform.position = cameraCenter - spriteCenterOffset;
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
}