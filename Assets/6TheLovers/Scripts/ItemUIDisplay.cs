using UnityEngine;
using UnityEngine.UI;

public class ItemUIDisplay : MonoBehaviour
{
    private string itemName = ""; 

    [SerializeField] public Sprite[] itemSprites;
    
    public void SetItem(string item)
    {
        itemName = item;

       if (itemName == "Rose")
       {
            GetComponent<Image>().sprite = itemSprites[0];
       }
       else if (itemName == "Clover")
       {
            GetComponent<Image>().sprite = itemSprites[1];
       }
       else if (itemName == "Sunflower")
       {
            GetComponent<Image>().sprite = itemSprites[2];
       }
       else if (itemName == "Unicorn")
       {
            GetComponent<Image>().sprite = itemSprites[3];
       }
       else if (itemName == "Dragon")
       {
            GetComponent<Image>().sprite = itemSprites[4];
       }
       else if (itemName == "Cat")
       {
            GetComponent<Image>().sprite = itemSprites[5];
       }
       else if (itemName == "Hat")
       {
            GetComponent<Image>().sprite = itemSprites[6];
       }
       else if (itemName == "Boots")
       {
            GetComponent<Image>().sprite = itemSprites[7];
       }
       else if (itemName == "Mustache")
       {
            GetComponent<Image>().sprite = itemSprites[8];
       }
       else if (itemName == "Gun")
       {
            GetComponent<Image>().sprite = itemSprites[9];
       }
       else if (itemName == "Whip")
       {
            GetComponent<Image>().sprite = itemSprites[10];
       }
       else if (itemName == "Wand")
       {
            GetComponent<Image>().sprite = itemSprites[11];
       }
    }

}
