using UnityEngine;
using System.Collections.Generic;

public class PlayerData : MonoBehaviour
{
    List<int> badCards = new List<int> {1, 2, 3};
    List<int> mediumCards = new List<int> {4, 5, 6, 7};
    List<int> goodCards = new List<int> {8, 9, 10};

    public int totalScore = 0;
    
    int CharacterSelected = 0; // 0 = page, 1  = knight, 2 = queen, 3 = king

    int bufferState = 0; //0  default, 1 = success, 2 = neutral, 3 = failure
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public int Failure()
    {
        //take one random card from badCards, add to totalScore, remove from badCards
        if (badCards.Count > 0)
        {
            int index = Random.Range(0, badCards.Count);
            int card = badCards[index];
            //badCards.RemoveAt(index);
            totalScore += card;
            //Debug.Log("Failure! Drew bad card: " + card);
            return card;
        }
        else
        {
            Debug.Log("No more bad cards to draw.");
            return -1;
        }
    }

    public int Neutral()
    {
        //take one random card from mediumCards, add to totalScore, remove from mediumCards
        if (mediumCards.Count > 0)
        {
            int index = Random.Range(0, mediumCards.Count);
            int card = mediumCards[index];
            //mediumCards.RemoveAt(index);
            totalScore += card;
            //Debug.Log("Neutral! Drew medium card: " + card);
            return card;
        }
        else
        {
            Debug.Log("No more medium cards to draw.");
            return -1;
        }
    }

    public int Success()
    {
        //take one random card from goodCards, add to totalScore, remove from goodCards
        if (goodCards.Count > 0)
        {
            int index = Random.Range(0, goodCards.Count);
            int card = goodCards[index];
            //goodCards.RemoveAt(index);
            totalScore += card;
            //Debug.Log("Success! Drew good card: " + card);
            return card;
        }
        else
        {
            Debug.Log("No more good cards to draw.");
            return -1;
        }
    }

    public void SetCharacter(int characterIndex)
    {
        CharacterSelected = characterIndex;
    }

    public void SetBufferState(int state)
    {
        bufferState = state;
    }

    public int GetBufferState()
    {
        return bufferState;
    }

    public int GetTotalScore()
    {
        return totalScore;
    }

    public void SetTotalScore(int score)
    {
        totalScore = score;
    }
}
