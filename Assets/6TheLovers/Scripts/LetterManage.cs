using UnityEngine;
using System.Collections.Generic;

public class LetterManage : MonoBehaviour
{
    [SerializeField]LoverGame loverGame;

    private string necessary1 = "";
    private string necessary2 = "";
    private string necessary3 = "";
    private string necessary4 = "";

    [SerializeField] List<GameObject> objects;
    [SerializeField] GameObject[] negativeObjects;
    List<bool> needings = new List<bool>(12);

    void Start()
    {
        necessary1 = loverGame.necessary1;
        necessary2 = loverGame.necessary2;
        necessary3 = loverGame.necessary3;
        necessary4 = loverGame.necessary4;

        if (necessary1 == "Rose" || necessary2 == "Rose" || necessary3 == "Rose" || necessary4 == "Rose")
        {
            needings[0] = true;
        }
        if (necessary1 == "Clover" || necessary2 == "Clover" || necessary3 == "Clover" || necessary4 == "Clover")
        {
            needings[1] = true;
        }
        if (necessary1 == "Sunflower" || necessary2 == "Sunflower" || necessary3 == "Sunflower" || necessary4 == "Sunflower")
        {
            needings[2] = true;
        }
        if (necessary1 == "Unicorn" || necessary2 == "Unicorn" || necessary3 == "Unicorn" || necessary4 == "Unicorn")
        {
            needings[3] = true;
        }
        if (necessary1 == "Dragon" || necessary2 == "Dragon" || necessary3 == "Dragon" || necessary4 == "Dragon")
        {
            needings[4] = true;
        }
        if (necessary1 == "Cat" || necessary2 == "Cat" || necessary3 == "Cat" || necessary4 == "Cat")
        {
            needings[5] = true;
        }
        if (necessary1 == "Hat" || necessary2 == "Hat" || necessary3 == "Hat" || necessary4 == "Hat")
        {
            needings[6] = true;
        }
        if (necessary1 == "Boots" || necessary2 == "Boots" || necessary3 == "Boots" || necessary4 == "Boots")
        {
            needings[7] = true;
        }
        if (necessary1 == "Mustache" || necessary2 == "Mustache" || necessary3 == "Mustache" || necessary4 == "Mustache")
        {
            needings[8] = true;
        }
        if (necessary1 == "Gun" || necessary2 == "Gun" || necessary3 == "Gun" || necessary4 == "Gun")
        {
            needings[9] = true;
        }
        if (necessary1 == "Whip" || necessary2 == "Whip" || necessary3 == "Whip" || necessary4 == "Whip")
        {
            needings[10] = true;
        }
        if (necessary1 == "Wand" || necessary2 == "Wand" || necessary3 == "Wand" || necessary4 == "Wand")
        {
            needings[11] = true;
        }

        

        for (int i = 0; i < objects.Count; i++)
        {
            print(i);
            if (needings[i])
            {
                objects[i].SetActive(true);
                objects.RemoveAt(i);
                //needings.RemoveAt(i);
            }
        }

        Debug.Log("Good");

        //choose random 4 other non-necessary items to fill the rest of the letter
        for (int i = 0; i < 4; i++)
        {
            int randChoice = Random.Range(0, objects.Count-1);
            objects[randChoice].SetActive(true);
            objects.RemoveAt(randChoice);
        }

        //finally, choose a random negative item
        int randNegative = Random.Range(0, negativeObjects.Length-1);
        negativeObjects[randNegative].SetActive(true);
    }
}
