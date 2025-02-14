using UnityEngine;

public class FinishPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger entered: " + collision.gameObject.name); // Check if trigger is working

        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player has reached the finish point!"); // Debug log to confirm player collision
            
            // Ensure SceneController instance exists
            if (SceneController.instance != null)
            {
                Debug.Log("SceneController instance found.");
                SceneController.instance.NextLevel();
            }
            else
            {
                Debug.LogError("SceneController instance is not found!");
            }
        }
    }
}
