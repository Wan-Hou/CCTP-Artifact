using UnityEngine;

public class ObjectCarry : MonoBehaviour
{
    [Header("Settings")]
    public float rayDistance = 5f;
    public float carryDistanceMultiplier = 0.9f;
    public Vector3 carryOffset = new(0f, -0.3f, 0f);

    [Header("UI")]
    public GameObject crosshair;


    [Header("State")]
    public bool isInRange = false;
    public bool ballInRange = false;
    public bool isCarryingObj = false;
    public bool triggered = false;

    [Header("Debug")]
    public Color gizmoColor = Color.cyan;

    private Camera cam;
    public GameObject carriedObject;
    [SerializeField] private GameObject carriedObjectPosition;
    private Rigidbody carriedRb;
    private GameObject standIn;
    private InputSystem_Actions inputsystem_actions;

    void TryPickUp(RaycastHit hit)
    {
        if (hit.collider.CompareTag("Interactable"))
        {
            carriedObject = hit.collider.gameObject;
            carriedRb = carriedObject.GetComponent<Rigidbody>();

            if (carriedRb == null) return;
            //carriedObject.transform.SetParent(cam.transform);

            // Move obj closer to camera
            //float dist = hit.distance * carryDistanceMultiplier;
            //carriedObject.transform.localPosition =
            //    cam.transform.forward * dist + carryOffset;'

            carriedObject.transform.parent = carriedObjectPosition.transform;
            carriedObject.transform.localPosition = Vector3.zero;

            // Physics changes
            carriedRb.useGravity = false;
            carriedRb.linearVelocity = Vector3.zero;
            carriedRb.angularVelocity = Vector3.zero;
            carriedRb.isKinematic = true;

            isCarryingObj = true;

            Debug.Log("Picked up object: " + carriedObject.name);
        }
        else
        {
            Debug.Log("Hit object is not interactable.");
        }

        /*Ray ray = new(cam.transform.position, cam.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
        {
            
        }
        else
        {
            Debug.Log("No interactable object found within range.");
        }*/
    }

    public void DropObject()
    {
        if (carriedObject == null) return;

        carriedObject.transform.SetParent(null);

        carriedRb.useGravity = true;
        carriedRb.isKinematic = false;

        carriedObject = null;
        carriedRb = null;
        isCarryingObj = false;

        Debug.Log("Dropped object.");
    }

    void TryPickUpNoRB(RaycastHit hit)
    {
        if (hit.collider.CompareTag("Interactable"))
        {
            carriedObject = hit.collider.gameObject;

            carriedObject.transform.parent = carriedObjectPosition.transform;
            carriedObject.transform.localPosition = Vector3.zero;

            standIn = new GameObject("StandIn");
            standIn.transform.parent = carriedObjectPosition.transform;
            standIn.transform.localPosition = Vector3.zero;

            standIn.AddComponent<MeshFilter>();
            standIn.AddComponent<MeshRenderer>();
            standIn.GetComponent<MeshFilter>().mesh = carriedObject.GetComponent<MeshFilter>().mesh;
            standIn.GetComponent<MeshRenderer>().material = carriedObject.GetComponent<MeshRenderer>().material;

            carriedObject.SetActive(false);

            isCarryingObj = true;

            Debug.Log("Picked up object: " + carriedObject.name);
        }
        else
        {
            Debug.Log("Hit object is not interactable.");
        }
    }

    public void PickUpNoRB(GameObject obj)
    {
        carriedObject = obj;
        obj.transform.parent = carriedObjectPosition.transform;
        obj.transform.localPosition = Vector3.zero;

        standIn = new GameObject("StandIn");
        standIn.transform.parent = carriedObjectPosition.transform;
        standIn.transform.localPosition = Vector3.zero;

        standIn.AddComponent<MeshFilter>();
        standIn.AddComponent<MeshRenderer>();
        standIn.GetComponent<MeshFilter>().mesh = obj.GetComponent<MeshFilter>().mesh;
        standIn.GetComponent<MeshRenderer>().material = obj.GetComponent<MeshRenderer>().material;

        obj.SetActive(false);

        isCarryingObj = true;
    }

    public void DropObjectNoRB()
    {
        //Debug.Log("Attempting to drop object. Current carriedObject: " + (carriedObject != null ? carriedObject.name : "null"));
        if (carriedObject == null) return;
        
        if (standIn != null)
        {
            Destroy(standIn);
        }
        carriedObject.SetActive(true);

        carriedObject.transform.SetParent(null);

        carriedObject = null;
        isCarryingObj = false;

        Debug.Log("Dropped object.");
    }

    // ============================
    // Gizmo Drawing
    // ============================
    void OnDrawGizmos()
    {
        if (!TryGetComponent<Camera>(out var gizmoCam)) return;

        Gizmos.color = gizmoColor;

        Vector3 origin = gizmoCam.transform.position;
        Vector3 direction = gizmoCam.transform.forward;

        // Draw full ray
        Gizmos.DrawLine(origin, origin + direction * rayDistance);

        // Draw hit point if something is hit
        if (Physics.Raycast(origin, direction, out RaycastHit hit, rayDistance))
        {
            Gizmos.DrawSphere(hit.point, 0.05f);
        }
    }

    void Start()
    {
        cam = GetComponent<Camera>();
        if (cam == null)
        {
            Debug.LogError("ObjectCarry script must be attached to a Camera.");
        }
        inputsystem_actions = new InputSystem_Actions();
    }
    void Update()
    {
        isInRange = Physics.Raycast(cam.transform.position, cam.transform.forward, 
                                    out RaycastHit hit, rayDistance);
        ballInRange = hit.collider != null && hit.collider.CompareTag("Interactable");

        if (InputHandler.instance.player_interact_triggered)
        {
            if (!triggered)
            {
                //Debug.Log("Interact triggered. isCarryingObj: " + isCarryingObj + ", ballInRange: " + ballInRange);
                if (!isCarryingObj && ballInRange) 
                    TryPickUpNoRB(hit);
                else DropObjectNoRB();
                triggered = true;
            }
        }
        else triggered = false;

        if (ballInRange)
        {
            crosshair.transform.localScale = Vector3.one * 1.5f;
            crosshair.GetComponent<UnityEngine.UI.Image>().color = new Color(1, 0, 0, 0.5f);
        }
        else
        {
            crosshair.transform.localScale = Vector3.one;
            crosshair.GetComponent<UnityEngine.UI.Image>().color = new Color(0, 0, 1, 0.5f);
        }
    }

}
