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
            player.GetComponent<FirstPersonController>().enabled = false;
            endScreen.SetActive(true);
        }
    }

    void Start()
    {
        endScreen.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player");
    }
}
