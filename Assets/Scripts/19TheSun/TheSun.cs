using UnityEngine;

public class TheSun : MonoBehaviour
{
    [SerializeField] int shootDuration = 12;
    int countDown;
    int shootingState; //0 = before, 1 = during, 2 = after

    Animator anim;
    AudioSource audioSource;

    [SerializeField] SunPlayer playerOne;
    [SerializeField] SunPlayer playerTwo;

    GameManager gameManager;

    void Start()
    {
        countDown = Random.Range(300, 600);
        shootingState = 0;
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        gameManager = GameManager.Instance;
    }

    void FixedUpdate()
    {
        if ( countDown > 0)
        {
            countDown--;
            return;
        }

        if (shootingState == 0)
        {
            shootingState = 1;
            anim.SetTrigger("Shoot");
            countDown = shootDuration;
            return;
        }

        if (shootingState == 1)
        {
            shootingState = 2;
            countDown = 100;
            //anim.SetTrigger("over");

            if (playerOne.HasBullets())
            {
                playerOne.Shoot();
            }
            if (playerTwo.HasBullets())
            {
                playerTwo.Shoot();
            }

            return;
        }

        if (shootingState == 2)
        {
            gameManager.FinishMinigame();
            return;
        }
    }

    public bool Evaluate()
    {
        return (shootingState == 1);
    }

    public void PlaySound()
    {
        audioSource.Play();
    }


}