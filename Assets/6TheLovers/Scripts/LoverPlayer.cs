using UnityEngine;

public class LoverPlayer : MonoBehaviour
{
    public LoverItem currentlyHolding = null;
    public LoverItem[] inLetter = new LoverItem[4];
    
    [SerializeField] float interactionRadius = 1.0f;
    
    [SerializeField] Transform holdPoint; 

    [SerializeField] LoverGame myGame;
    PlayerData myData; 
    [SerializeField] string myName; 

    void Start()
    {
        myData = GameObject.FindGameObjectWithTag(myName).GetComponent<PlayerData>();
    }

    public bool isHoldingItem => currentlyHolding != null;

    public void dropOrPickUp()
    {
        //Debug.Log("done");
        
        if (!isHoldingItem)
        {
           DepositBox box = GetClosestBox();
            if (box != null && !box.IsEmpty)
            {
                currentlyHolding = box.TakeItem();
                currentlyHolding.AssignToParent(holdPoint != null ? holdPoint : this.transform);
                return;
            }

            LoverItem itemOnGround = GetClosestItemOnGround();
            if (itemOnGround != null)
            {
                Debug.Log("done");
                currentlyHolding = itemOnGround;
                currentlyHolding.AssignToParent(holdPoint != null ? holdPoint : this.transform);
            }
        }
        else
        {
            DepositBox box = GetClosestBox();
            
            if (box != null && box.IsEmpty)
            {
                box.PlaceItem(currentlyHolding);
                currentlyHolding = null;
            }
            else
            {
                currentlyHolding.Detach();
                currentlyHolding.transform.position = transform.position + new Vector3(0, -0.5f, 0); 
                currentlyHolding = null;
            }
        }
    }

    private DepositBox GetClosestBox()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, interactionRadius);
        DepositBox closest = null;
        float minDist = float.MaxValue;

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<DepositBox>(out var box))
            {
                float dist = Vector3.Distance(transform.position, box.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    closest = box;
                }
            }
        }
        return closest;
    }

    private LoverItem GetClosestItemOnGround()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, interactionRadius);
        LoverItem closest = null;
        float minDist = float.MaxValue;

        foreach (var hit in hits)
        {
            Debug.Log(hit.gameObject.name);
            
            if (hit.TryGetComponent<LoverItem>(out var item))// && item.transform.parent == null)
            {
                float dist = Vector3.Distance(transform.position, item.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    closest = item;
                }
            }
        }
        return closest;
    }

    public void CheckCorrectness()
    {
        int points = 0;
        
        //check inLetter[].itemName...
        //all items are unique, so no duplicates.
        //match to necessary (regardless of order) is +1 point
        //empty = 0 points 
        //if inLetter[].itemName == "Bad", -1 point
        //if all matched: myData.SetBufferState(1)
        //if points >0 and <4, myData.SetBufferState(2)
        //if points 0 or less, myData.SetBufferState(3)

        for (int i = 0; i <= 3; i++)
        {
            if (inLetter[i].itemName == "Bad")
            {
                points -= 1; 
            }
            else if (inLetter[i].itemName == myGame.necessary1)
            {
                points +=1; 
            }
            else if (inLetter[i].itemName == myGame.necessary2)
            {
                points +=1; 
            }
            else if (inLetter[i].itemName == myGame.necessary3)
            {
                points +=1; 
            }
            else if (inLetter[i].itemName == myGame.necessary4)
            {
                points +=1; 
            }
        }

        if (points <= 0)
        {
            myData.SetBufferState(3);
        }
        else if (points > 3)
        {
            myData.SetBufferState(1);
        }
        else
        {
            myData.SetBufferState(2);
        }
    }
}
