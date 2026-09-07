using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 5;
    public int CurrentHealth { get; private set; }
    private bool isDead = false;

    public event Action<int, int> OnHealthChanged;

    private Animator animator;

    private void Awake()
    {
        CurrentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        CurrentHealth -= amount;
        CurrentHealth = Mathf.Max(CurrentHealth, 0);

        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        if (animator != null)
        {
            animator.ResetTrigger("attack");
            animator.SetTrigger("die");
        }

        MeleeEnemy melee = GetComponent<MeleeEnemy>();
        if (melee != null) melee.enabled = false;

        RangedEnemy ranged = GetComponent<RangedEnemy>();
        if (ranged != null) ranged.enabled = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        }

        Destroy(gameObject, 1f);
    }
}