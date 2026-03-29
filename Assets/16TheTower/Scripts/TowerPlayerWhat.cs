using UnityEngine;
using System.Collections;

public class TowerPlayerWhat : MonoBehaviour
{
    
    PlatformerMovement movement;
    PlayerData player;
    [SerializeField] string playerName;
    [SerializeField] float lightningPauseTime = 1f;

    bool perfect = false;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movement = GetComponent<PlatformerMovement>();
        player = GameObject.FindGameObjectWithTag(playerName).GetComponent<PlayerData>();
    }

    void Update()
    {
        if (transform.position.y < -25)
        {
            player.SetBufferState(3);
            this.gameObject.SetActive(false);
        }
        if (perfect)
        {
            player.SetBufferState(1);
        }
        else
        {
            player.SetBufferState(2);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Lightning"))
        {
            StartCoroutine(pauseMovement(lightningPauseTime));
            Debug.Log("Hit by lightning");
        }
        if (other.CompareTag("Item"))
        {
            perfect = true;
        }
    }

    IEnumerator pauseMovement(float time)
    {
        movement.enabled = false;
        yield return new WaitForSeconds(time);
        movement.enabled = true;
    }
}
