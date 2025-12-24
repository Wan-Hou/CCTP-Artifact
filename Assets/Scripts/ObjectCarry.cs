using UnityEngine;

public class ObjectCarry : MonoBehaviour
{
    [Header("Settings")]
    public float rayDistance = 5f;
    public float carryDistanceMultiplier = 0.9f;
    public Vector3 carryOffset = new Vector3(0f, -0.3f, 0f);

    [Header("State")]
    public bool isCarryingObj = false;

    [Header("Debug")]
    public Color gizmoColor = Color.cyan;

    private Camera cam;
    private GameObject carriedObject;
    [SerializeField] private GameObject carriedObjectPosition;
    private Rigidbody carriedRb;
    private InputSystem_Actions inputsystem_actions;

    void TryPickUp()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance))
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
                carriedObject.transform.localPosition = carriedObjectPosition.transform.localPosition;

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
        }
        else
        {
            Debug.Log("No interactable object found within range.");
        }
    }

    void DropObject()
    {
        if (carriedObject == null) return;

        carriedObject.transform.SetParent(null);

        carriedRb.useGravity = true;
        carriedRb.isKinematic = false;

        carriedObject = null;
        carriedRb = null;
        isCarryingObj = false;
    }

    // ============================
    // Gizmo Drawing
    // ============================
    void OnDrawGizmos()
    {
        Camera gizmoCam = GetComponent<Camera>();
        if (gizmoCam == null) return;

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
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isCarryingObj)
            {
                TryPickUp();
            }
            else
            {
                DropObject();
            }
        }
    }

}
