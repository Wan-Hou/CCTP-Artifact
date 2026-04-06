using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class InteractableObstacle : MonoBehaviour
{
    public Vector3 openPosition;
    public Vector3 closedPosition;
    public bool vertical = true;
    public float move = 2f;
    public float duration = 1f;
    [SerializeField] private bool isMoving = false;

    void Start()
    {
        closedPosition = transform.position;
        if (vertical) openPosition = closedPosition + new Vector3(0, move, 0);
        else openPosition = closedPosition + new Vector3(-move, 0, 0);
        transform.position = openPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            StartCoroutine(InteractableObstacleMove(false));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            StartCoroutine(InteractableObstacleMove(true));
        }
    }

    IEnumerator InteractableObstacleMove(bool isOpening)
    {
        Debug.Log("Starting obstacle move: " + (isOpening ? "Opening" : "Closing"));
        isMoving = true;
        float elapsedTime = 0f;
        if (isOpening)
        {
            while (elapsedTime < duration)
            {
                transform.position = Vector3.Lerp(closedPosition, openPosition, (elapsedTime / duration));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.position = openPosition;
        }
        else
        {
            while (elapsedTime < duration)
            {
                transform.position = Vector3.Lerp(openPosition, closedPosition, (elapsedTime / duration));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.position = closedPosition;
        }
        isMoving = false;
    }
}
