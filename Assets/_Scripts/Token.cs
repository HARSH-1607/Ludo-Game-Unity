using UnityEngine;
using TMPro;
using System.Collections;

public class Token : MonoBehaviour
{
    // --- Public References & State ---
    public Transform[] pathPoints;

    // --- State Variables ---
    public int currentPathIndex = -1;
    public bool isMoving = false;
    public bool isInBase = true;
    public bool isHome = false;

    // --- Private Variables ---
    private Vector3 basePosition;

    void Start()
    {
        basePosition = transform.position;
    }

    // This is for debugging, to see the index on the token itself.
    void Update()
    {
        TextMeshProUGUI statusText = GetComponentInChildren<TextMeshProUGUI>();
        if (statusText == null) return;

        if (isHome)
        {
            statusText.text = "HOME";
        }
        else if (isInBase)
        {
            statusText.text = "BASE";
        }
        else
        {
            statusText.text = currentPathIndex.ToString();
        }
    }

    public IEnumerator MoveCoroutine(int steps)
    {
        isMoving = true;

        for (int i = 0; i < steps; i++)
        {
            currentPathIndex++;
            if (currentPathIndex >= pathPoints.Length)
            {
                isHome = true;
                break;
            }
            
            Vector3 targetPosition = pathPoints[currentPathIndex].position;
            while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, 10f * Time.deltaTime);
                yield return null;
            }
            transform.position = targetPosition;
        }
        isMoving = false;
    }

    public void MoveToStart()
    {
        isInBase = false;
        currentPathIndex = 0;
        transform.position = pathPoints[0].position;
    }

    public void ReturnToBase()
    {
        isInBase = true;
        currentPathIndex = -1;
        transform.position = basePosition;
    }

    public void MoveToFinalHome(Vector3 homePosition)
    {
        transform.position = homePosition;
    }
}