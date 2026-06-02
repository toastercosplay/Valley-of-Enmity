using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class Scoring : MonoBehaviour
{
    [System.Serializable]
    public class PlayerScoreUI
    {
        public string playerTag;
        public RectTransform uiRoot;
        public TMP_Text totalText;
        public TMP_Text gainedText;

        [HideInInspector] public PlayerData playerData;
        [HideInInspector] public int initialScore;
        [HideInInspector] public int gainedScore;
        [HideInInspector] public int finalScore;
    }

    public List<PlayerScoreUI> allPlayers;


    public float bottomY = -300f;
    public float topY = 300f;
    public float minScale = 0.5f;
    public float maxScale = 1.5f;

    public float revealDelay = 0.8f;
    public float tickSpeed = 0.05f;

    private GameManager gameManager;
    private AudioSource audioSource;
    private List<PlayerScoreUI> activePlayers = new List<PlayerScoreUI>();

    void Start()
    {
        gameManager = GameManager.Instance;
        audioSource = GetComponent<AudioSource>();

        InitializePlayers();
        StartCoroutine(RevealAndScoreSequence());
    }

    void InitializePlayers()
    {
        foreach (var p in allPlayers)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag(p.playerTag);
            if (playerObj != null)
            {
                p.playerData = playerObj.GetComponent<PlayerData>();
                p.uiRoot.gameObject.SetActive(false);

                p.initialScore = p.playerData.GetTotalScore();
                p.gainedScore = CalculateGainedScore(p.playerData);
                p.finalScore = p.initialScore + p.gainedScore;

                p.playerData.SetBufferState(0);
                p.playerData.SetTotalScore(p.finalScore);

                p.totalText.text = p.initialScore.ToString();
                p.gainedText.text = "+" + p.gainedScore.ToString();

                activePlayers.Add(p);
            }
        }

        activePlayers = activePlayers.OrderBy(p => p.finalScore).ToList();
    }

    int CalculateGainedScore(PlayerData data)
    {
        int state = data.GetBufferState();
        if (state == 1) return data.Success();
        if (state == 2) return data.Neutral();
        if (state == 3) return data.Failure();
        return 0;
    }

    IEnumerator RevealAndScoreSequence()
    {
        yield return new WaitForSeconds(0.5f);

        int playerCount = activePlayers.Count;
        int currentRank = 0;
        int previousScore = -9999;

        for (int i = 0; i < activePlayers.Count; i++)
        {
            var p = activePlayers[i];

            if (i == 0 || p.finalScore > previousScore)
            {
                currentRank = i; 
            }
            previousScore = p.finalScore;

            float t = playerCount > 1 ? (float)currentRank / (playerCount - 1) : 1f;

            float targetScale = Mathf.Lerp(minScale, maxScale, t);
            float targetY = Mathf.Lerp(bottomY, topY, t);

            p.uiRoot.anchoredPosition = new Vector2(p.uiRoot.anchoredPosition.x, targetY);

            p.uiRoot.localScale = Vector3.zero;
            p.uiRoot.gameObject.SetActive(true);
            
            if (audioSource != null) audioSource.Play();

            float elapsed = 0f;
            float popDuration = 0.3f;
            while (elapsed < popDuration)
            {
                elapsed += Time.deltaTime;
                float easeOut = Mathf.Sin((elapsed / popDuration) * Mathf.PI * 0.5f);
                p.uiRoot.localScale = Vector3.Lerp(Vector3.zero, Vector3.one * targetScale, easeOut);
                yield return null;
            }
            p.uiRoot.localScale = Vector3.one * targetScale;

            yield return new WaitForSeconds(revealDelay);
        }

        yield return new WaitForSeconds(1.0f);

        bool isTicking = true;
        while (isTicking)
        {
            isTicking = false;
            
            foreach (var p in activePlayers)
            {
                if (p.gainedScore > 0)
                {
                    isTicking = true;
                    
                    p.initialScore++;
                    p.gainedScore--;

                    p.totalText.text = p.initialScore.ToString();
                    
                    if (p.gainedScore > 0)
                        p.gainedText.text = "+" + p.gainedScore.ToString();
                    else
                        p.gainedText.text = "";
                }
            }

            yield return new WaitForSeconds(tickSpeed);
        }

        yield return new WaitForSeconds(1.5f);
        gameManager.BackToTable();
    }
}