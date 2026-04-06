using System.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public enum ObstacleAction
{
    Tangibility,
    Intangibility,
    Translate,
    NoTranslate,
    Rotate,
    NoRotate,
}

public class Obstacle : MonoBehaviour
{
    [SerializeField] private Vector3Int colourKey;
    private Color colorKeyColour;
    public Material colourKeyMaterial;
    public Material currentObstacleMaterial;

    public GameObject effectVFX;

    public ObstacleAction action = ObstacleAction.Tangibility;

    // Tangibility settings
    public bool camouflage = false;

    // Translate settings
    public Vector3 pointA;
    public Vector3 pointB;
    public float translateSpeed = 2f;
    public float translateDelay = 1f;
    public bool isPlatform = false;
    [SerializeField] private bool canTranslate = false;
    [SerializeField] private bool toB = false;

    // Rotation settings
    public Vector3 rotateValue;
    [SerializeField] private bool canRotate = false;

    static float byteToDecimal(int value)
    {
        return value == 0 ? 0 : (64 * value - 1) / 255f;
    }

    public bool CheckObstacleActivation()
    {
        if (ColourControls.instance.colourIndex == colourKey) return true;
        else return false;
    }

    public void ChangeObjectAlpha(float alpha)
    {
        Color currentColour = GetComponent<Renderer>().material.color;
        currentColour.a = alpha / 255f;
        GetComponent<Renderer>().material.color = currentColour;
    }

    public void ActivateObstacle()
    {
        if (TestManager.instance != null && TestManager.instance.use_visual_features)
        {
            GetComponent<Renderer>().material = camouflage ? 
                ColourControls.instance.wallMaterial : 
                ColourControls.instance.activatedObstacleMaterial;
            StartCoroutine(ActivateEffect());
        }
        else
        {
            GetComponent<Renderer>().material = camouflage ?
                ColourControls.instance.wallMaterial :
                currentObstacleMaterial;
        }
        switch (action)
        {
            case (ObstacleAction.Tangibility):
                {
                    if (TryGetComponent<Collider>(out var col)) col.enabled = true;
                    ChangeObjectAlpha(255);
                    break;
                }
            case (ObstacleAction.Intangibility):
                {
                    if (TryGetComponent<Collider>(out var col)) col.enabled = false;
                    ChangeObjectAlpha(30);
                    break;
                }
            case (ObstacleAction.Translate):
                {
                    canTranslate = true;
                    break;
                }
            case (ObstacleAction.NoTranslate):
                {
                    canTranslate = false;
                    break;
                }
            case (ObstacleAction.Rotate):
                {
                    canRotate = true;
                    break;
                }
            case (ObstacleAction.NoRotate):
                {
                    canRotate = false;
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
                if (TryGetComponent<Collider>(out var col)) col.enabled = false;
                ChangeObjectAlpha(30);
                break;
            }
            case (ObstacleAction.Intangibility):
            {
                if (TryGetComponent<Collider>(out var col)) col.enabled = true;
                ChangeObjectAlpha(255);
                break;
            }
            case (ObstacleAction.Translate):
            {
                canTranslate = false;
                break;
            }
            case (ObstacleAction.NoTranslate):
            {
                canTranslate = true;
                break;
            }
            case (ObstacleAction.Rotate):
            {
                canRotate = false;
                break;
            }
            case (ObstacleAction.NoRotate):
            {
                canRotate = true;
                break;
            }
        }
    }

    IEnumerator Translate()
    {         
        while (true)
        {
            if (canTranslate)
            {
                if (toB)
                {
                    transform.position = Vector3.MoveTowards
                        (transform.position, pointB, translateSpeed * Time.deltaTime);
                    if (Vector3.Distance(transform.position, pointB) < 0.1f)
                    {
                        toB = false;
                        yield return new WaitForSeconds(translateDelay);
                    }
                }
                else
                {
                    transform.position = Vector3.MoveTowards
                        (transform.position, pointA, translateSpeed * Time.deltaTime);
                    if (Vector3.Distance(transform.position, pointA) < 0.1f)
                    {
                        toB = true;
                        yield return new WaitForSeconds(translateDelay);
                    }
                }
            }
            yield return null;
        }
    }

    IEnumerator Rotate()
    {
        while (true)
        {
            if (canRotate) transform.Rotate(rotateValue * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator ActivateEffect()
    {
        GameObject newEffect = Instantiate(effectVFX, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(3f);
        Destroy(newEffect);
    }

    private void Start()
    {
        colorKeyColour = new Color(
            byteToDecimal(colourKey.x), 
            byteToDecimal(colourKey.y), 
            byteToDecimal(colourKey.z));
        colourKeyMaterial = GetComponent<MeshRenderer>().material;
        colourKeyMaterial.color = colorKeyColour;
        currentObstacleMaterial = GetComponent<Renderer>().material;

        if (action == ObstacleAction.Tangibility)
        {
            if (TryGetComponent<Collider>(out var col)) col.enabled = false;
            ChangeObjectAlpha(30);
        }

        if (action == ObstacleAction.Translate || 
            action == ObstacleAction.NoTranslate) StartCoroutine(Translate());
        canTranslate = (action == ObstacleAction.NoTranslate);

        if (action == ObstacleAction.Rotate || 
            action == ObstacleAction.NoRotate) StartCoroutine(Rotate());
        canRotate = (action == ObstacleAction.NoRotate);
    }

}
