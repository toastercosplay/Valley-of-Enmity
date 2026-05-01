using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PipeIncreaseScore : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var key = collision.GetComponent<KeyMovement>();
        if (key == null) return;

        ScoringText.instance.UpdateScore(key.playerIndex);
    }
}
