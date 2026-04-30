using StarterAssets;
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
    public Vector3Int colourKey;
    public Vector3Int keyDifference;
    private Color colorKeyColour;
    public bool colourAssigned = false;
    public bool followParentColour = false;
    public bool continuousFlashing = false;
    public Coroutine confirmation;
    public Material colourKeyMaterial;
    public bool redCorrect = false;
    public bool greenCorrect = false;
    public bool blueCorrect = false;

    public ObstacleAction action = ObstacleAction.Tangibility;

    // Tangibility settings
    public bool camouflage = false;

    // Translate settings
    public Vector3 pointA;
    public Vector3 pointB;
    public float translateSpeed = 2f;
    public float translateDelay = 1f;
    public bool isPlatform = false;
    public BoxCollider platformCollider;
    public PlayerController playerController;
    public Vector3 difference;
    [SerializeField] private bool canTranslate = false;
    [SerializeField] private bool toB = false;

    // Rotation settings
    public Vector3 rotateValue;
    [SerializeField] private bool canRotate = false;

    static float byteToDecimal(int value)
    {
        return value == 0 ? 0 : (64 * value - 1) / 255f;
    }

    private Vector3Int RandomColour()
    {
        return new Vector3Int(Random.Range(0, 5), Random.Range(0, 5), Random.Range(0, 5));
    }

    public void ColourTip(int differnce, ref bool correct, GameObject tipVFX)
    {
        if (differnce == 0 && !correct)
        {
            StartCoroutine(ActivateColorParticleEffect(tipVFX, 1f));
            correct = true;
        }
        else if (differnce != 0) correct = false;
    }

    public bool CheckObstacleActivation()
    {
        keyDifference = new Vector3Int(
            Mathf.Abs(ColourControls.instance.colourIndex[0] - colourKey[0]),
            Mathf.Abs(ColourControls.instance.colourIndex[1] - colourKey[1]),
            Mathf.Abs(ColourControls.instance.colourIndex[2] - colourKey[2]));

        if (ColourControls.instance.colourIndex == colourKey)
        { 
            redCorrect = greenCorrect = blueCorrect = true;
            return true; 
        }

        if (TestManager.instance != null && TestManager.instance.use_visual_features)
        {
            if (!followParentColour)
            {
                ColourTip(keyDifference.x, ref redCorrect, ColourControls.instance.redMatchVFX);
                ColourTip(keyDifference.y, ref greenCorrect, ColourControls.instance.greenMatchVFX);
                ColourTip(keyDifference.z, ref blueCorrect, ColourControls.instance.blueMatchVFX);
            }
        }

        return false;
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
            if (!followParentColour)
                confirmation = StartCoroutine(ActivateColorParticleEffect(ColourControls.instance.activateVFX, 0.5f, continuousFlashing));
        }
        else
        {
            GetComponent<Renderer>().material = camouflage ?
                ColourControls.instance.wallMaterial :
                colourKeyMaterial;
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
                    //if (isPlatform && playerController != null) playerController.externalVelocity = Vector3.zero;
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
        GetComponent<Renderer>().material = colourKeyMaterial;
        if (confirmation != null) StopCoroutine(confirmation);
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
                    //if (isPlatform && playerController != null) playerController.externalVelocity = Vector3.zero;
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

    public void SwapColour(Vector3Int newKey)
    {
        colourKey = newKey;
        Vector3Int indices = ColourControls.instance.colourIndex;
        keyDifference = new Vector3Int(
            Mathf.Abs(indices[0] - colourKey[0]),
            Mathf.Abs(indices[1] - colourKey[1]),
            Mathf.Abs(indices[2] - colourKey[2]));
        if (keyDifference.x == 0)   redCorrect = true;
        if (keyDifference.y == 0) greenCorrect = true;
        if (keyDifference.z == 0)  blueCorrect = true;

        colorKeyColour = new Color(
            byteToDecimal(colourKey.x),
            byteToDecimal(colourKey.y),
            byteToDecimal(colourKey.z));
        colourKeyMaterial = GetComponent<MeshRenderer>().material;
        colourKeyMaterial.color = colorKeyColour;
    }

    IEnumerator Translate()
    {         
        if (isPlatform)
        {
            platformCollider = gameObject.AddComponent<BoxCollider>();
            platformCollider.isTrigger = true;
            platformCollider.center = new Vector3(0, 1.75f, 0);
            platformCollider.size = new Vector3(0.95f, 2.5f, 0.95f);
        }

        while (true)
        {
            if (canTranslate)
            {
                Vector3 old = transform.position;
                transform.position = Vector3.MoveTowards
                    (transform.position, toB ? pointB : pointA, translateSpeed * Time.deltaTime);
                difference = transform.position - old;

                if (isPlatform && playerController != null)
                {
                    playerController.externalVelocityComponent += new Vector3
                        (difference.x > 0 ? translateSpeed : (difference.x < 0 ? -translateSpeed : 0), 0,
                         difference.z > 0 ? translateSpeed : (difference.z < 0 ? -translateSpeed : 0));
                    playerController.externalVelocity += difference;
                }

                if (Vector3.Distance(transform.position, toB ? pointB : pointA) < 0.1f)
                {
                    toB = !toB;
                    //if (isPlatform && playerController != null) playerController.externalVelocity = Vector3.zero;
                    yield return new WaitForSeconds(translateDelay);
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

    IEnumerator ActivateColorParticleEffect(GameObject particleEffectToActivate, float wait)
    {
        GameObject newParticleEffect = Instantiate(particleEffectToActivate, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(wait);
        Destroy(newParticleEffect);
    }

    IEnumerator ActivateColorParticleEffect(GameObject particleEffectToActivate, float wait, bool continuous)
    {
        do {
            GameObject newParticleEffect = Instantiate(particleEffectToActivate, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(wait);
            Destroy(newParticleEffect);
        }
        while (continuous);
    }

    IEnumerator InitObstacle()
    {
        if (followParentColour)
        {
            yield return new WaitUntil(() => transform.parent.GetComponent<Obstacle>().colourAssigned);
            //yield return new WaitForSeconds(0.25f);
        }

        colourKey = followParentColour ? transform.parent.GetComponent<Obstacle>().colourKey : RandomColour();
        colourAssigned = true;
        Vector3Int indices = ColourControls.instance.colourIndex;
        keyDifference = new Vector3Int(
            Mathf.Abs(indices[0] - colourKey[0]),
            Mathf.Abs(indices[1] - colourKey[1]),
            Mathf.Abs(indices[2] - colourKey[2]));
        if (keyDifference.x == 0)   redCorrect = true;
        if (keyDifference.y == 0) greenCorrect = true;
        if (keyDifference.z == 0)  blueCorrect = true;

        colorKeyColour = new Color(
            byteToDecimal(colourKey.x),
            byteToDecimal(colourKey.y),
            byteToDecimal(colourKey.z));
        colourKeyMaterial = GetComponent<MeshRenderer>().material;
        colourKeyMaterial.color = colorKeyColour;

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

    private void Start()
    {
        StartCoroutine(InitObstacle());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isPlatform && other.CompareTag("Player"))
        {
            playerController = other.GetComponent<PlayerController>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isPlatform && other.CompareTag("Player"))
        {
            //playerController.externalVelocity = Vector3.zero;
            playerController = null;
        }
    }

}
