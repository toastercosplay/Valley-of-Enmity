using UnityEngine;
using System.Collections.Generic;

public class LoverGame : MonoBehaviour
{
    [SerializeField] List<string> itemNames;

    public string necessary1 = "";
    public string necessary2 = "";
    public string necessary3 = "";
    public string necessary4 = "";

    [SerializeField] GameObject display1;
    [SerializeField] GameObject display2;
    [SerializeField] GameObject display3;
    [SerializeField] GameObject display4;

    // [SerializeField]GameObject player1;
    // [SerializeField]GameObject player2;

    [SerializeField] GameObject loverImage1;
    [SerializeField] GameObject loverImage2;

    [SerializeField] float gravity = 9.8f;

    GameManager gameManager;
    [SerializeField] LoverPlayer player1;
    [SerializeField] LoverPlayer player2;
    [SerializeField] LoverPlayer player3;
    [SerializeField] LoverPlayer player4;
    
    Animator anim;

    void Start()
    {
        gameManager = GameManager.Instance;

        Physics.gravity = new Vector3(0, -gravity, 0);

        for (int i = 0; i < 4; i++)
        {
            int randChoice = Random.Range(0, itemNames.Count-1);
            //Debug.Log("Random Choice: " + randChoice);

            if (necessary1 == "")
            {
                //set necessary 1 then remove from list
                necessary1 = itemNames[randChoice];
                itemNames.RemoveAt(randChoice);
            }
            else if (necessary2 == "")
            {
                
                necessary2 = itemNames[randChoice];
                itemNames.RemoveAt(randChoice);
            }
            else if (necessary3 == "")
            {
                necessary3 = itemNames[randChoice];
                itemNames.RemoveAt(randChoice);
            }
            else if (necessary4 == "")
            {
                necessary4 = itemNames[randChoice];
                itemNames.RemoveAt(randChoice);
            }
        }

        int loverImage = Random.Range(0,2);
        if (loverImage == 0)
        {
            loverImage1.SetActive(true);
            loverImage2.SetActive(false);
        }
        else
        {
            loverImage1.SetActive(false);
            loverImage2.SetActive(true);
        }


        UpdateVisuals();
    }

    void UpdateVisuals()
    {
        display1.SetActive(true);
        display2.SetActive(true);
        display3.SetActive(true);
        display4.SetActive(true);
        
        display1.GetComponent<ItemUIDisplay>().SetItem(necessary1);
        display2.GetComponent<ItemUIDisplay>().SetItem(necessary2);
        display3.GetComponent<ItemUIDisplay>().SetItem(necessary3);
        display4.GetComponent<ItemUIDisplay>().SetItem(necessary4);
    }

    public void EndGame()
    {
        player1.CheckCorrectness();
        player2.CheckCorrectness();
        
        if (player3 != null)
        {
            player3.CheckCorrectness();
        }

        if (player4 != null)
        {
            player4.CheckCorrectness();
        }

        gameManager.FinishMinigame();
    }

}
