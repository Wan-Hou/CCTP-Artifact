using StarterAssets;
using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    public GameObject endScreen;
    private GameObject player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            player.GetComponent<PlayerController>().enabled = false;
            Cursor.lockState = CursorLockMode.None;
            endScreen.SetActive(true);
        }
    }

    void Start()
    {
        endScreen.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player");
    }
    
    /*private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            player.GetComponent<PlayerController>().enabled = false;
            Cursor.lockState = CursorLockMode.None;
            endScreen.SetActive(true);
        }
    }*/
}
