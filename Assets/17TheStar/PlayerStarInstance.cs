using UnityEngine;
using System;

public class PlayerStarInstance : MonoBehaviour
{
    public Camera playerCamera;
    public CursorMovement cursorMovement;
    public Star[] myStars;
    public int playerIndex;
    public int playerLayer;

    private bool gameWon;
    private float startTime;
    private float completionTime = -1f;

    public Action<int, float> onPlayerFinished;

    void Start()
    {
        startTime = Time.time;
        if (cursorMovement != null)
        {
            cursorMovement.onAPressed.AddListener(HandleClick);
        }
    }

    void OnDestroy()
    {
        if (cursorMovement != null)
        {
            cursorMovement.onAPressed.RemoveListener(HandleClick);
        }
    }

    void HandleClick()
    {
        if (gameWon) return;

        RectTransform cursorRect = cursorMovement.GetComponent<RectTransform>();
        Vector2 worldPos = cursorRect.position;

        // Raycast only on this player's layer
        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero, Mathf.Infinity, 1 << playerLayer);

        if (hit.collider != null)
        {
            Star clickedStar = hit.collider.GetComponent<Star>();
            if (clickedStar != null)
            {
                clickedStar.ToggleColor();
                CheckWinCondition();
            }
        }
    }

    void CheckWinCondition()
    {
        if (myStars == null || myStars.Length == 0) return;

        StarColor? setAColor = null;
        StarColor? setBColor = null;

        foreach (Star s in myStars)
        {
            if (s.bipartiteSet == BipartiteSet.A)
            {
                if (setAColor == null)
                    setAColor = s.starColor;
                else if (s.starColor != setAColor)
                    return;
            }
            else
            {
                if (setBColor == null)
                    setBColor = s.starColor;
                else if (s.starColor != setBColor)
                    return;
            }
        }

        if (setAColor != null && setBColor != null && setAColor != setBColor)
        {
            gameWon = true;
            completionTime = Time.time - startTime;
            Debug.Log($"Player {playerIndex + 1} finished in {completionTime:F2}s!");
            onPlayerFinished?.Invoke(playerIndex, completionTime);
        }
    }

    public bool HasFinished()
    {
        return gameWon;
    }

    public float GetCompletionTime()
    {
        return completionTime;
    }
}
