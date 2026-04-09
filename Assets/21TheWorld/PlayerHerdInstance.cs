using System;
using UnityEngine;

public class PlayerHerdInstance : MonoBehaviour
{
    [HideInInspector] public int playerIndex;
    [HideInInspector] public CowController[] myCows;
    [HideInInspector] public Action<int, float> onPlayerFinished;

    private bool gameWon;
    private float startTime;
    private float completionTime = -1f;

    void Start()
    {
        startTime = Time.time;
    }

    void Update()
    {
        if (!gameWon)
        {
            CheckWinCondition();
        }
    }

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
