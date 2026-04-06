using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class ColourUIManager : MonoBehaviour
{
    public static ColourUIManager instance;

    [Header("Indicator References")]
    public GameObject currentColourIndicator;
    public GameObject redIndicator;
    public GameObject greenIndicator;
    public GameObject blueIndicator;
    public Image resultIndicator;

    [Header("Settings")]
    [SerializeField]
    private Vector3 currentColourPosition;
    public Vector3 currentColourOffset;

    [SerializeField]
    private Vector3 redIndicatorPosition;
    [SerializeField]
    private Vector3 greenIndicatorPosition;
    [SerializeField]
    private Vector3 blueIndicatorPosition;
    public Vector3 arrowIndicatorOffset;

    public void UIUpdate()
    {
        currentColourIndicator.transform.localPosition = 
            currentColourPosition + ColourControls.instance.currentColourIndex * currentColourOffset;

        redIndicator.transform.localPosition = 
            redIndicatorPosition + ColourControls.instance.colourIndex[0] * arrowIndicatorOffset;

        greenIndicator.transform.localPosition =
            greenIndicatorPosition + ColourControls.instance.colourIndex[1] * arrowIndicatorOffset;

        blueIndicator.transform.localPosition =
            blueIndicatorPosition + ColourControls.instance.colourIndex[2] * arrowIndicatorOffset;

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
        currentColourPosition = currentColourIndicator.transform.localPosition;
        redIndicatorPosition = redIndicator.transform.localPosition;
        greenIndicatorPosition = greenIndicator.transform.localPosition;
        blueIndicatorPosition = blueIndicator.transform.localPosition;
    }

}
