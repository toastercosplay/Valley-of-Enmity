using System;
using UnityEngine;

// one of these lives on each player's arena. it owns that player's set of cows,
// times their run, and notifies theworldmanager once every cow is following —
// which is how a player "finishes" the round.
public class PlayerHerdInstance : MonoBehaviour
{
    // 0-based player slot (p1 = 0). used by the manager when reporting placement.
    [HideInInspector] public int playerIndex;
    // the cows this player must round up. set by the spawner before start runs.
    [HideInInspector] public CowController[] myCows;
    // invoked exactly once when this player finishes; manager uses it to track ranking.
    [HideInInspector] public Action<int, float> onPlayerFinished;

    private bool gameWon;
    private float startTime;
    private float completionTime = -1f;

    void Start()
    {
        // race timer starts at scene load — all players start simultaneously.
        startTime = Time.time;
    }

    void Update()
    {
        if (!gameWon)
        {
            CheckWinCondition();
        }
    }

    // the win check is a simple "all cows following" scan. cheap enough to run
    // every frame and avoids needing each cow to push state changes upward.
    void CheckWinCondition()
    {
        if (myCows == null || myCows.Length == 0)
        {
            return;
        }

        for (int i = 0; i < myCows.Length; i++)
        {
            if (myCows[i] == null || !myCows[i].IsFollowing())
            {
                return;
            }
        }

        gameWon = true;
        completionTime = Time.time - startTime;
        onPlayerFinished?.Invoke(playerIndex, completionTime);
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
