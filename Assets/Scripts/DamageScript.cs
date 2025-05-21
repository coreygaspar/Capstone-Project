using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageScript : MonoBehaviour
{
    [SerializeField] private int damageBoost = 50;  // Amount of damage boost
    [SerializeField] private float duration = 10f;  // Duration of the power-up in seconds
    [SerializeField] private Attack[] playerAttacks;   // Reference to all Attack scripts on the player

    private bool isPowerUpActive = false;
    private Timer timer;  // Reference to the Timer script

    void Start()
    {
        // Try to find the Timer in the scene
        timer = FindObjectOfType<Timer>();  // This will find the first object of type Timer in the scene
        if (timer == null)
        {
            Debug.LogError("No Timer found in the scene.");
        }
    }

    // Triggered when the player collides with the power-up
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
    {
        ActivatePowerUp();
    }
    }

    // Activate the power-up and start the timer
    private void ActivatePowerUp()
    {
        isPowerUpActive = true;

    // Reapply power-up on all player's attack scripts
    foreach (Attack attack in playerAttacks)
    {
        attack.ActivatePowerUp(damageBoost, duration); // Make sure this method resets the boost timer in Attack.cs
    }

    // Restart timer on UI
    if (timer != null)
    {
        timer.ShowTimer();
        timer.StartTimer(duration); // This should restart the timer UI
    }

    // Reset coroutine to extend the duration
    StopAllCoroutines(); // Important: cancel previous coroutine
    StartCoroutine(DeactivatePowerUp());

    // Optionally: Disable this power-up object (if you don’t want multiple stacks)
    gameObject.SetActive(false);
    }

    // Coroutine to deactivate the power-up after the specified duration
    private IEnumerator DeactivatePowerUp()
    {
        yield return new WaitForSeconds(duration);

        // Re-enable the power-up object so it can be used again (if you want it to respawn later)
        gameObject.SetActive(true);

        isPowerUpActive = false;

        // EnableOtherDamagePickups();
    }

//     private void DisableOtherDamagePickups()
// {
//     GameObject[] pickups = GameObject.FindGameObjectsWithTag("Damage");
//     foreach (GameObject pickup in pickups)
//     {
//         Collider2D col = pickup.GetComponent<Collider2D>();
//         if (pickup != gameObject && col != null)
//         {
//             col.enabled = false;
//         }
//     }
// }

// private void EnableOtherDamagePickups()
// {
//     GameObject[] pickups = GameObject.FindGameObjectsWithTag("Damage");
//     foreach (GameObject pickup in pickups)
//     {
//         Collider2D col = pickup.GetComponent<Collider2D>();
//         if (pickup != gameObject && col != null)
//         {
//             col.enabled = true;
//         }
//     }
// }
}
