using StarterAssets;
using UnityEngine;

public class PlayerReset : MonoBehaviour
{
    [Tooltip("World height for respawn")]
    public float DeathPlane = -50f;

    public Vector3 respawnPosition;

    [Tooltip("ObjectCarry Script Reference to drop interactable on death")]
    public ObjectCarry objectCarryScript;

    void Start()
    {
        // get respawn position
        respawnPosition = transform.position + Vector3.up; 
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= DeathPlane)
        {
            if (CompareTag("Player"))
            {
                GetComponent<PlayerController>().enabled = false;
                objectCarryScript.DropObjectNoRB();
            }
            else
            {
                Rigidbody rb = GetComponent<Rigidbody>();
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
            Debug.Log(name + " fell below death plane. Respawning at" + respawnPosition);
            transform.localPosition = respawnPosition;
            if (CompareTag("Player")) GetComponent<PlayerController>().enabled = true;
        }
    }
}
