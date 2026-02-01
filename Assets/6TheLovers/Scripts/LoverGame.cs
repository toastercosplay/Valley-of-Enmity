using UnityEngine;
using System.Collections.Generic;

public class LoverGame : MonoBehaviour
{
    [SerializeField] List<string> itemNames;

    private string necessary1 = "";
    private string necessary2 = "";
    private string necessary3 = "";
    private string necessary4 = "";

    [SerializeField] GameObject display1;
    [SerializeField] GameObject display2;
    [SerializeField] GameObject display3;
    [SerializeField] GameObject display4;

    [SerializeField]GameObject player1;
    [SerializeField]GameObject player2;

    GameManager gameManager;
    PlayerData player1Data;
    PlayerData player2Data;
    
    Animator anim;

    void Start()
    {
        //gameManager = GameManager.Instance;
        //player1Data = GameObject.FindGameObjectWithTag("Player1Data").GetComponent<PlayerData>();
        //player2Data = GameObject.FindGameObjectWithTag("Player2Data").GetComponent<PlayerData>();

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

}
