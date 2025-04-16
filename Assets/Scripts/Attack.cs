using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
  Collider2D attackCollider;
    public int attackDmg = 10;  // Base damage
    public Vector2 knockback = Vector2.zero;

    private int boostedAttackDmg; // Store boosted damage
    private bool powerUpActive = false; // Whether power-up is active
    private Coroutine powerUpCoroutine;

    void Awake()
    {
        attackCollider = GetComponent<Collider2D>();
        boostedAttackDmg = attackDmg; // Set initial boosted damage to base damage
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // You can handle other updates here if needed
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // See if it can be hit
        Damageable damageable = collision.GetComponent<Damageable>();

        if (damageable != null)
        {
            // Deliver knockback based on character's facing direction
            Vector2 deliveredKnockback = transform.parent.localScale.x > 0 ? knockback : new Vector2(-knockback.x, knockback.y);
            
            // Hit the target with the appropriate attack damage (including boosted damage if applicable)
            bool gotHit = damageable.Hit(boostedAttackDmg, deliveredKnockback);

            if (gotHit)
            {
                Debug.Log(collision.name + " hit for " + boostedAttackDmg);
            }
        }
    }

    // Method to activate the power-up (called by the DamagePowerUp script)
    public void ActivatePowerUp(int damageBoost, float duration)
    {
        if (!powerUpActive)
        {
            powerUpActive = true;
            boostedAttackDmg = attackDmg + damageBoost; // Increase damage by power-up value
            Debug.Log("Power-Up Activated! New damage: " + boostedAttackDmg);

            // Start the power-up timer
            if (powerUpCoroutine != null)
            {
                StopCoroutine(powerUpCoroutine); // Stop any previous timers
            }
            powerUpCoroutine = StartCoroutine(DeactivatePowerUpAfterDuration(duration));
        }
    }

    // Coroutine to reset damage after power-up duration ends
    private IEnumerator DeactivatePowerUpAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);

        // Reset the attack damage to the original value
        boostedAttackDmg = attackDmg;
        powerUpActive = false;
        Debug.Log("Power-Up Ended! Damage reset to: " + boostedAttackDmg);
    }
}
