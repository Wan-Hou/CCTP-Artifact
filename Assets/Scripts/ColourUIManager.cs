using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class ColourUIManager : MonoBehaviour
{
    public static ColourUIManager instance;

    // Referenced & Current Resolution
    [SerializeField] private Vector2 refRes;
    [SerializeField] private Vector2 curRes;
    [SerializeField] private Vector2 resScale;

    [Header("Indicator References")]
    public GameObject currentColourIndicator;
    public GameObject redIndicator;
    public GameObject greenIndicator;
    public GameObject blueIndicator;
    public Image resultIndicator;

    [Header("Settings")]
    public Vector3 currentColourPosition;
    public Vector3 currentColourOffset;
    public Vector3 redIndicatorPosition;
    public Vector3 greenIndicatorPosition;
    public Vector3 blueIndicatorPosition;
    public Vector3 arrowIndicatorOffset;

    public void UIUpdate()
    {
        curRes = new Vector2(Screen.width, Screen.height);
        resScale = new Vector2(curRes.x / refRes.x, curRes.y / refRes.y);

        currentColourIndicator.transform.position = 
            currentColourPosition + ColourControls.instance.currentColourIndex * resScale.y * currentColourOffset;

        redIndicator.transform.position = 
            redIndicatorPosition + ColourControls.instance.colourIndex[0] * resScale.x * arrowIndicatorOffset;

        greenIndicator.transform.position =
            greenIndicatorPosition + ColourControls.instance.colourIndex[1] * resScale.x * arrowIndicatorOffset;

        blueIndicator.transform.position =
            blueIndicatorPosition + ColourControls.instance.colourIndex[2] * resScale.x * arrowIndicatorOffset;

        resultIndicator.color = new Color(

            (ColourControls.instance.colourIndex[0] == 0 ? 0 : 
            64 * ColourControls.instance.colourIndex[0] - 1) / 255f,

            (ColourControls.instance.colourIndex[1] == 0 ? 0 : 
            64 * ColourControls.instance.colourIndex[1] - 1) / 255f,

            (ColourControls.instance.colourIndex[2] == 0 ? 0 : 
            64 * ColourControls.instance.colourIndex[2] - 1) / 255f,

            1);
    }
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        currentColourPosition = currentColourIndicator.transform.position;
        redIndicatorPosition = redIndicator.transform.position;
        greenIndicatorPosition = greenIndicator.transform.position;
        blueIndicatorPosition = blueIndicator.transform.position;
        refRes = GetComponentInParent<CanvasScaler>().referenceResolution;
        curRes = new Vector2(Screen.width, Screen.height);
        resScale = new Vector2(curRes.x / refRes.x, curRes.y / refRes.y);
    }

}
