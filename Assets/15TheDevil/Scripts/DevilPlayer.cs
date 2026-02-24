using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DevilPlayer : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] PlayerData playerData;

    [Header("References")]
    [SerializeField] TheDevil gameController;

    bool isMyTurn;

    [SerializeField] Sprite kneelSprite;
    [SerializeField] Sprite bowSprite;
    [SerializeField] Image outOfOrderIndicator; //like a red foreground that flashes when you get it wrong

    private Coroutine FlashCoroutine; // to keep track of the flashing coroutine so we can stop it if its activated a 2nd time

    void Start()
    {
        isMyTurn = false;
    }

    public void OnClick()
    {
        gameController.OnPlayerClick(this);
    }

    public void SetActive(bool active)
    {
        var e = GetComponent<ParticleSystem>().emission;
        e.enabled = active;
        //idrk why you have to do this but you can't just set the particle system to active inactive directly for some reaosn
        isMyTurn = active;
        GetComponent<SpriteRenderer>().sprite = active ? bowSprite : kneelSprite;
    }

    public void OnPenalty()
    {

        if (FlashCoroutine != null)
            StopCoroutine(FlashCoroutine);
        
        FlashCoroutine = StartCoroutine(FlashOutOfOrder());
        
    }

    public void SetResult(int bufferState)
    {
        playerData.SetBufferState(bufferState);
    }

    public bool IsMyTurn()
    {
        return isMyTurn;
    }

    private IEnumerator FlashOutOfOrder()
    {
        Color color = outOfOrderIndicator.color;
        color.a = 1f;
        outOfOrderIndicator.color = color;

        float duration = 0.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, elapsed / duration);
            outOfOrderIndicator.color = color;
            yield return null;
        }

        color.a = 0f;
        outOfOrderIndicator.color = color;
    }
}
