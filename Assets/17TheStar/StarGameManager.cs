using UnityEngine;

public class StarGameManager : MonoBehaviour
{
    public Star firstSelectedStar; 
    public LineRenderer linePrefab; 
    public CursorMovement cursorMovement;

    void Start()
    {
        cursorMovement.onAPressed.AddListener(HandleClick);
        cursorMovement.onRTPressed.AddListener(CancelSelection);
    }

    void OnDestroy()
    {
        if (cursorMovement != null)
        {
            cursorMovement.onAPressed.RemoveListener(HandleClick);
            cursorMovement.onRTPressed.RemoveListener(CancelSelection);
        }
    }

    void CancelSelection()
    {
        firstSelectedStar = null;
        Debug.Log("Selection Cancelled");
    }

    void HandleClick()
    {
        Debug.Log("Click registered");
        // Get cursor screen position from its RectTransform (works for Screen Space - Overlay canvas)
        RectTransform cursorRect = cursorMovement.GetComponent<RectTransform>();
        Vector2 screenPos = cursorRect.position;

        // Convert screen position to world point for 2D raycast
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);

        Debug.Log("cursor screenPos: " + screenPos);

        // Raycast to see if we hit a collider (Make sure Stars have CircleCollider2D!)
        RaycastHit2D hit = Physics2D.Raycast(screenPos, Vector2.zero);

        if (hit.collider != null)
        {
            Star clickedStar = hit.collider.GetComponent<Star>();
            if (clickedStar != null)
            {
                OnStarClicked(clickedStar);
            }
        }
    }

    void OnStarClicked(Star clickedStar)
    {
        // CASE 1: No star currently selected. Select this one.
        if (firstSelectedStar == null)
        {
            if (clickedStar.CanConnect())
            {
                firstSelectedStar = clickedStar;
                Debug.Log($"Selected {clickedStar.name}. Select another to connect.");
                // Optional: Add visual highlight here
            }
            else
            {
                Debug.Log("That star is already full!");
            }
            return;
        }

        // CASE 2: We already selected a star. Try to connect to this new one.
        
        // Rule: Cannot connect to itself
        if (clickedStar == firstSelectedStar) return;

        // Rule: Bipartite (Must be different colors)
        if (firstSelectedStar.starColor == clickedStar.starColor)
        {
            Debug.Log("Cannot connect stars of the same color!");
            firstSelectedStar = null; // Reset selection
            return;
        }

        // Rule: Degree Limit 
        if (!clickedStar.CanConnect())
        {
            Debug.Log("Target star is full!");
            firstSelectedStar = null;
            return;
        }

        // Rule: Already connected?
        if (firstSelectedStar.connectedStars.Contains(clickedStar))
        {
            Debug.Log("Already connected!");
            firstSelectedStar = null;
            return;
        }

        // If we passed all rules: Connect
        CreateConnection(firstSelectedStar, clickedStar);
        
        // Reset selection for the next turn
        firstSelectedStar = null;
    }

    void CreateConnection(Star a, Star b)
    {
        // 1. Update Logic
        a.connectedStars.Add(b);
        b.connectedStars.Add(a);

        // 2. Visuals (Draw Line)
        LineRenderer line = Instantiate(linePrefab);
        line.SetPosition(0, a.transform.position);
        line.SetPosition(1, b.transform.position);

        Debug.Log("Connection Successful!");
    }
}