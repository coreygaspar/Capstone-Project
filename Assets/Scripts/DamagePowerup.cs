using UnityEngine;

public class DamagePowerUp : MonoBehaviour
{
    public int damageIncrease = 50;  // Amount to increase damage by
    public float duration = 10f;     // Duration the power-up lasts

    private void OnTriggerEnter2D(Collider2D collision)
{
    if (collision.CompareTag("Player"))
    {
        Attack playerAttack = collision.GetComponent<Attack>();
        if (playerAttack != null)
        {
            playerAttack.ActivatePowerUp(damageIncrease, duration);
        }

        Destroy(gameObject);
    }
}
}
