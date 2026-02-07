using UnityEngine;

public class HermitPlayer : MonoBehaviour
{
    int fireTemperature = 100;
    int foodPreparedness = 0;
    public int minTemp = 900;
    public int maxTemp = 1000;
    public int minGoal = 100;
    public int maxGoal = 200;

    public float maxSize = 1.5f;
    public float minSize = 0.5f;

    SpriteRenderer spriteRenderer;

    //Animator anim;
    [SerializeField] string playerName = "";
    PlayerData playerData;
    
    void Start()
    {
        //anim = GetComponent<Animator>();
        //playerData = GameObject.FindGameObjectWithTag(playerName).GetComponent<PlayerData>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        //Debug.Log(spriteRenderer);
    }

    void Update()
    {
        fireTemperature -= 1;

        if (fireTemperature > minTemp && fireTemperature < maxTemp)
        {
            foodPreparedness += 1;
        }
        else if (fireTemperature >= maxTemp)
        {
            foodPreparedness += 2;
        }
        else if (fireTemperature <= minTemp)
        {
            foodPreparedness -= 1;
            if (foodPreparedness < 0)
            {
                foodPreparedness = 0;
            }
        }

        //anim.SetInteger("Temperature", fireTemperature);
        //update visuals
        //overall minimum temp: 0, overall maximum temp: 1500
        //want top lerp from #05283f  at 0 to #29a5ff at 900-1000
        //then from #29a5ff at 900-1000 to #ff006a at 1500

        if(spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer is null!");
            return;
        }

        if (fireTemperature <= minTemp)
        {
            spriteRenderer.color = Color.Lerp(new Color(0.02f, 0.16f, 0.25f), new Color(0.16f, 0.65f, 1f), fireTemperature / (float)minTemp);
        }
        else if (fireTemperature >= maxTemp)
        {
            spriteRenderer.color = Color.Lerp(new Color(1f, 0f, 0.42f), new Color(0.5f, 0f, 0.21f), (fireTemperature - maxTemp) / (float)(1500 - maxTemp));
        }
        else
        {
            spriteRenderer.color = Color.Lerp(new Color(0.16f, 0.65f, 1f), new Color(1f, 0f, 0.42f), (fireTemperature - minTemp) / (float)(maxTemp - minTemp));
        }        

        //also lerp from minSize to maxSize based on fireTemperature
        float sizeLerp = Mathf.InverseLerp(0, 1500, fireTemperature);
        float newSize = Mathf.Lerp(minSize, maxSize, sizeLerp);
        transform.localScale = new Vector3(newSize, newSize, newSize);

        Debug.Log("Fire Temperature: " + fireTemperature);
    }

    public void FanFlames()
    {
        fireTemperature += 20;
    }

    public void PrepareFood()
    {
        if (foodPreparedness < minGoal)
        {
            playerData.SetBufferState(3); //uncooked food
        }
        else if (foodPreparedness >= minGoal && foodPreparedness <= maxGoal)
        {
            playerData.SetBufferState(1); //perfectly cooked food
        }
        else if (foodPreparedness > maxGoal)
        {
            playerData.SetBufferState(2); //burnt food
        }
    }
}
