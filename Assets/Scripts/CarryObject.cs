using UnityEngine;

public class CarryObject : MonoBehaviour
{
    [Header("Pickup Settings")]
    [SerializeField] Transform holdArea;
    private GameObject heldObject;
    private Rigidbody heldObjectRb;

    [Header("Physics Parameters")]
    [SerializeField] float pickupRange = 5.0f;
    //[SerializeField] float pickupForce = 150.0f;

    private float pickupBufferTime = 0.5f;
    private float pickupBufferTimer = 0f;

    void CheckPickUpRange()
    {
        if (heldObject == null)
        {
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, pickupRange))
            {
                Debug.Log("Object in range: " + hit.transform.gameObject.name);
            }
        }
    }

    void TryPickUp()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, pickupRange))
        {
            PickupObject(hit.transform.gameObject);
        }
    }

    public void PickupObject(GameObject pickUp)
    {
        if (pickUp.GetComponent<Rigidbody>())
        {
            heldObjectRb = pickUp.GetComponent<Rigidbody>();
            heldObjectRb.useGravity = false;
            heldObjectRb.linearDamping = 10;
            heldObjectRb.angularDamping = 10;
            heldObjectRb.constraints = RigidbodyConstraints.FreezeRotation;

            heldObjectRb.transform.SetParent(holdArea);
            heldObject = pickUp;
        }
    }

    public void DropObject()
    {
        heldObjectRb.useGravity = true;
        heldObjectRb.linearDamping = 1;
        heldObjectRb.angularDamping = 1;
        heldObjectRb.constraints = RigidbodyConstraints.None;

        heldObjectRb.transform.SetParent(null);
        heldObject = null;
    }

    private void Update()
    {
        CheckPickUpRange();

        if (InputHandler.instance.player_interact_triggered)
        {
            if (pickupBufferTimer <= 0)
            {
                if (heldObject == null) TryPickUp();
                else DropObject();

                pickupBufferTimer = pickupBufferTime;
            }
            else pickupBufferTimer -= Time.deltaTime;
        }
        
    }
}
