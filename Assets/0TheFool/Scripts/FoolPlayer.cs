using UnityEngine;
using UnityEngine.UI;

public class FoolPlayer : MonoBehaviour
{
    bool hasArrow = true;
    [SerializeField] TheFool theFool;
    [SerializeField] string playerName = "";
    public PlayerData playerData;

    Animator anim;
    Image image;
    FadeAfterDuration fadeAfterDuration;
    private RectTransform imageRectTransform;
    // [SerializeField] float testx;
    // [SerializeField] float testy;

    float maxX = 450f;
    float minX = -450f;
    float maxY = 175f;
    float minY = -250f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerData = GameObject.FindGameObjectWithTag(playerName).GetComponent<PlayerData>();
        anim = GetComponent<Animator>();
        image = GetComponent<Image>();
        fadeAfterDuration = GetComponent<FadeAfterDuration>();

        imageRectTransform = GetComponent<RectTransform>();
        imageRectTransform.anchoredPosition = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
    }

    public void ShootArrow()
    {
        if (hasArrow)
        {
            Vector2 position = transform.position;
            float distance = theFool.submitPosition(position);
            theFool.submitDistance(distance, playerName);
            hasArrow = false;
        }

        fadeAfterDuration.Disable();
        //set to white
        Color c = image.color;
        c.a = 1f;
        image.color = c;
        anim.SetTrigger("Shoot");
    }

    public bool GetHasArrow()
    {
        return hasArrow;
    }

    void Update()
    {
        //for testing
        //imageRectTransform.anchoredPosition = new Vector2(testx, testy);
    }
}
