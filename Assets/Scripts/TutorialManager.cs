using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;

    public List<GameObject> tutorialPanels;
    public int currentPanelIndex = 0;

    private InputHandler input;
    private bool pauseNavInput = false;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        instance.tutorialPanels[0].SetActive(true);
        input = GameObject.FindGameObjectWithTag("Manager").GetComponent<InputHandler>();
        if (input == null)
        {
            Debug.Log("InputHandler component not found on the manager object. Please add an InputHandler component.");
            input = InputHandler.instance; // Fallback to the singleton instance if not found on the player object
        }
        input.EnableUIInput();
    }

    void Update()
    {
        if (input.ui_navigation_input.x < 0 && !pauseNavInput)
        {
            if (currentPanelIndex > 0)
            {
                tutorialPanels[currentPanelIndex].SetActive(false);
                currentPanelIndex--;
                tutorialPanels[currentPanelIndex].SetActive(true);
                pauseNavInput = true;
            }
        }
        if (input.ui_navigation_input.x > 0 && !pauseNavInput)
        {
            if (currentPanelIndex < tutorialPanels.Count - 1)
            {
                tutorialPanels[currentPanelIndex].SetActive(false);
                currentPanelIndex++;
                tutorialPanels[currentPanelIndex].SetActive(true);
                pauseNavInput = true;
            }
        }
        if (input.ui_navigation_input.x == 0)
        {
            pauseNavInput = false;
        }
        if (input.ui_cancel_triggered || input.ui_exit_triggered)
        {
            tutorialPanels[currentPanelIndex].SetActive(false);
            input.EnablePlayerInput();
        }
    }

}
