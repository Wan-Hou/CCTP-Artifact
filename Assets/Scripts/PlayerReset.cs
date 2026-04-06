using StarterAssets;
using UnityEngine;

public class PlayerReset : MonoBehaviour
{
    [Tooltip("World height for respawn")]
    public float DeathPlane = -50f;

    [SerializeField] private Vector3 _respawnPosition;

    [Tooltip("ObjectCarry Script Reference to drop interactable on death")]
    public ObjectCarry objectCarryScript;

    void Start()
    {
        // get respawn position
        _respawnPosition = transform.position + new Vector3(0,1,0);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= DeathPlane)
        {
            if (CompareTag("Player"))
            {
                GetComponent<PlayerController>().enabled = false;
                objectCarryScript.DropObject();
            }
            Debug.Log(name + " fell below death plane. Respawning at" + _respawnPosition);
            transform.localPosition = _respawnPosition;
            if (CompareTag("Player")) GetComponent<PlayerController>().enabled = true;
        }
    }
}
