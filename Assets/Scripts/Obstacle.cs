using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public enum ObstacleAction
{
    Tangibility,
    Intangibility
}

public class Obstacle : MonoBehaviour
{
    [Header("Action")]
    public ObstacleAction action = ObstacleAction.Tangibility;

    [Header("Settings")]
    [SerializeField] private Vector3Int colourKey;
    public Material currentObstacleMaterial;

    public bool CheckObstacleActivation()
    {
        if (ColourControls.instance.colourIndex == colourKey) return true;
        else return false;
    }

    public void ActivateObstacle()
    {
        GetComponent<Renderer>().material = ColourControls.instance.activatedObstacleMaterial;
        switch (action)
        {
            case (ObstacleAction.Tangibility):
            {
                Collider col = GetComponent<Collider>();
                if (col != null) col.enabled = true;
                Color currentColour = GetComponent<Renderer>().material.color;
                currentColour.a = 1;
                GetComponent<Renderer>().material.color = currentColour;
                break;
            }
            case (ObstacleAction.Intangibility):
            {
                Collider col = GetComponent<Collider>();
                if (col != null) col.enabled = false;
                Color currentColour = GetComponent<Renderer>().material.color;
                currentColour.a = 30 / 255f;
                GetComponent<Renderer>().material.color = currentColour;
                break;
            }
        }
    }

    public void DeactivateObstacle()
    {
        GetComponent<Renderer>().material = currentObstacleMaterial;
        switch (action)
        {
            case (ObstacleAction.Tangibility):
            {
                Collider col = GetComponent<Collider>();
                if (col != null) col.enabled = false;
                Color currentColour = GetComponent<Renderer>().material.color;
                currentColour.a = 30 / 255f;
                GetComponent<Renderer>().material.color = currentColour;
                break;
            }
            case (ObstacleAction.Intangibility):
            {
                Collider col = GetComponent<Collider>();
                if (col != null) col.enabled = true;
                Color currentColour = GetComponent<Renderer>().material.color;
                currentColour.a = 1;
                GetComponent<Renderer>().material.color = currentColour;
                break;
            }
        }
    }

    private void Start()
    {
        currentObstacleMaterial = GetComponent<Renderer>().material;
    }


}
