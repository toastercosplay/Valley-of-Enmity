using UnityEngine;

public class HermitPlayer : MonoBehaviour
{
    public float fireTemperature = 100f;
    public float tempLossPerSecond = 20f;
    public int minTemp = 900;
    public int maxTemp = 1000;
    
    public int foodPreparedness = 0;
    public int minGoal = 100;
    public int maxGoal = 200;

    public Material ringMaterial;
    public float minRadius = 0.1f;
    public float maxRadius = 0.5f;
    public float volumeConstant = 0.02f;
    
    [SerializeField] string playerName = "";
    PlayerData playerData;
    
    void Start()
    {
        //playerData = GameObject.FindGameObjectWithTag(playerName).GetComponent<PlayerData>();
    }

    void Update()
    {
        fireTemperature -= tempLossPerSecond * Time.deltaTime;
        fireTemperature = Mathf.Clamp(fireTemperature, 0, 1500);

        UpdateCookingLogic();

        UpdateRingVisuals();
    }

    public void FanFlames()
    {
        fireTemperature += 15;
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

    void UpdateRingVisuals()
    {
        if (ringMaterial == null) return;

        // 1. PHYSICAL SCALE (The size on screen)
        // We map 0-1500 temperature to minSize-maxSize scale
        float tScale = Mathf.InverseLerp(0, 1500, fireTemperature);
        float currentScale = Mathf.Lerp(8f, 24f, tScale);
        transform.localScale = new Vector3(currentScale, currentScale, 1f);

        // 2. SHADER RADIUS (Keep this fixed or slightly adjusted)
        // We keep Radius near 0.45 so it never hits the edge of the square
        float shaderRadius = 0.45f; 
        
        // 3. THICKNESS (Thinner as the object gets physically larger)
        // We use currentScale here so it gets thinner as the WHOLE object grows
        float thickness = volumeConstant / currentScale;

        //overall minimum temp: 0, overall maximum temp: 1500
        //want top lerp from #05283f  at 0 to #29a5ff at 900-1000
        //then from #29a5ff at 900-1000 to #ff006a at 1500
        Color finalColor;
        Color coldColor = new Color(0.02f, 0.16f, 0.25f); // #05283f
        Color idealColor = new Color(0.16f, 0.65f, 1f);  // #29a5ff
        Color hotColor = new Color(1f, 0f, 0.42f);       // #ff006a

        if (fireTemperature <= minTemp)
        {
            float t = Mathf.InverseLerp(0, minTemp, fireTemperature);
            finalColor = Color.Lerp(coldColor, idealColor, t);
        }
        else if (fireTemperature >= maxTemp)
        {
            float t = Mathf.InverseLerp(maxTemp, 1500, fireTemperature);
            finalColor = Color.Lerp(idealColor, hotColor, t);
        }
        else
        {
            float t = Mathf.InverseLerp(minTemp, maxTemp, fireTemperature);
            finalColor = idealColor;
        }

        // 5. APPLY
        ringMaterial.SetFloat("_Radius", shaderRadius);
        ringMaterial.SetFloat("_Thickness", thickness);
        ringMaterial.SetColor("_Color", finalColor);
    }

    void UpdateCookingLogic()
    {
        if (fireTemperature > minTemp && fireTemperature < maxTemp)
        {
            foodPreparedness += 1;  
        }
        else if (fireTemperature >= maxTemp)
        {
            foodPreparedness += 2;
        } 
        else if (fireTemperature <= minTemp && foodPreparedness > 0)
        {
            foodPreparedness -= 1;
        }
    }
}
