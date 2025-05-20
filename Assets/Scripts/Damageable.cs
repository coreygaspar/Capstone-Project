using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Damageable : MonoBehaviour
{

    public UnityEvent<int, Vector2> damagableHit;
    public UnityEvent damageableDeath;
    public UnityEvent<int, int> healthChanged;
    Animator animator;
    [SerializeField]
    private int _maxHealth = 100;
    public int MaxHealth
    {
        get
        {
            return _maxHealth;
        }
        set
        {
            _maxHealth = value;  
        }
    }
    [SerializeField]
    private int _health = 100;

    public int Health
{
    get
    {
        return _health;
    }
    set
    {
        _health = value;
        healthChanged?.Invoke(_health, MaxHealth);

        // Unified death logic
        if (_health <= 0 && IsAlive)
        {
            IsAlive = false; // this will also set animator bool
            animator.SetTrigger("deathTrigger"); // trigger death animation
            damageableDeath?.Invoke(); // optional: fire UnityEvent

            if (CompareTag("Player"))
            {
                StartCoroutine(LoadGameOverSceneWithDelay(1f));
            }
            else if (CompareTag("Wizard"))
            {
                StartCoroutine(LoadWinSceneWithDelay(1f));
            }
        }
    }
}

    [SerializeField]
    private bool _isAlive = true;
    [SerializeField]
    private bool isInvincible = false;

    private float timeSinceHit = 0;
    public float invincibilityTime = 0.25f;

    public bool IsAlive { 
        get
        {
            return _isAlive;
        } 
        set
        {
            _isAlive = value;
            animator.SetBool(AnimationStrings.isAlive, value);
            Debug.Log(IsAlive);
        } 
    }

     public bool LockVelocity 
    { 
        get
        {
         return animator.GetBool(AnimationStrings.lockVelocity);  
        }
        set
        {
            animator.SetBool(AnimationStrings.lockVelocity, value);
        }
    }

    private void Awake() 
    {
        animator = GetComponent<Animator>();
    }

    public void Update()
    {
        if(isInvincible)
        {
            if(timeSinceHit > invincibilityTime)
            {
               // Remove invincibility
               isInvincible = false;
               timeSinceHit = 0;
            }

            timeSinceHit += Time.deltaTime;
        }
    }

    public bool Hit(int damage, Vector2 knockback)
    {
        if(IsAlive && !isInvincible)
        {
            Health -= damage;
            isInvincible = true;

            animator.SetTrigger(AnimationStrings.hitTrigger);
            LockVelocity = true;
            damagableHit?.Invoke(damage, knockback);
            CharacterEvents.characterDamaged.Invoke(gameObject, damage);

            StartCoroutine(UnlockVelocityAfterDelay(0.2f));

            return true;
        }

        // Unable to be hit
        return false;
    }

    private IEnumerator UnlockVelocityAfterDelay(float delay)
{
    yield return new WaitForSeconds(delay);
    LockVelocity = false;
}

    public bool Heal(int healthRestore)
    {
        if(IsAlive && Health < MaxHealth)
        {
            int maxHeal = Mathf.Max(MaxHealth - Health, 0);
            int actualHeal = Mathf.Min(maxHeal, healthRestore);
            Health += actualHeal;
            CharacterEvents.characterHealed(gameObject, actualHeal);
            return true;
        }
        return false;
    }

    private IEnumerator LoadGameOverSceneWithDelay(float delay)
    {
        // Optionally, add a visual cue here for death (e.g., fade out effect)
        yield return new WaitForSeconds(delay);  // Wait for the specified delay
        SceneManager.LoadScene("GameOver"); // Ensure "GameOver" scene is in the build settings
    }

    private IEnumerator LoadWinSceneWithDelay(float delay)
    {
        // Optionally, add a visual cue here for death (e.g., fade out effect)
        yield return new WaitForSeconds(delay);  // Wait for the specified delay
        SceneManager.LoadScene("Win"); // Ensure "Win" scene is in the build settings
    }
}
