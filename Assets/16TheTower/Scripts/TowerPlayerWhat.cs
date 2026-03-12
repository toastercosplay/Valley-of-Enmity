using UnityEngine;
using System.Collections;

public class TowerPlayerWhat : MonoBehaviour
{
    
    PlatformerMovement movement;
    PlayerData player;
    [SerializeField] string playerName;
    [SerializeField] float lightningPauseTime = 1f;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movement = GetComponent<PlatformerMovement>();
        //player = GameManager.Instance.GetPlayer(playerName);
    }

    void Update()
    {
        if (transform.position.y < -25)
        {
            //player.SetBufferState(3);
            //this.GameObject.SetActive(false);
        }
        if (transform.position.y > 500)
        {
            //player.SetBufferState(1);
            //this.GameObject.SetActive(false);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Lightning"))
        {
            StartCoroutine(pauseMovement(lightningPauseTime));
            Debug.Log("Hit by lightning");
        }
    }

    IEnumerator pauseMovement(float time)
    {
        movement.enabled = false;
        yield return new WaitForSeconds(time);
        movement.enabled = true;
    }
}
