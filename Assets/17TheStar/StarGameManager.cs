using UnityEngine;

public class StarGameManager : MonoBehaviour
{
    public CursorMovement cursorMovement;

    [Header("Stars")]
    public Star[] allStars;

    private bool gameWon;

    void Start()
    {
        cursorMovement.onAPressed.AddListener(HandleClick);
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
        Vector2 screenPos = cursorRect.position;

        RaycastHit2D hit = Physics2D.Raycast(screenPos, Vector2.zero);

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
        if (allStars == null || allStars.Length == 0) return;

        // Check if all set A stars share one color and all set B stars share the other
        StarColor? setAColor = null;
        StarColor? setBColor = null;

        foreach (Star s in allStars)
        {
            if (s.bipartiteSet == BipartiteSet.A)
            {
                if (setAColor == null)
                    setAColor = s.starColor;
                else if (s.starColor != setAColor)
                    return; // Not all A stars match
            }
            else
            {
                if (setBColor == null)
                    setBColor = s.starColor;
                else if (s.starColor != setBColor)
                    return; // Not all B stars match
            }
        }

        // Both sets must exist and have different colors
        if (setAColor != null && setBColor != null && setAColor != setBColor)
        {
            gameWon = true;
            Debug.Log("You win!");
        }
    }
}
