using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 5;
    public int CurrentHealth { get; private set; }
    public bool IsDead { get; private set; } = false;

    public static PlayerHealth instant;

    public event Action<int, int> OnHealthChanged; // (currentHealth, maxHealth)

    private Animator animator;

    private void Awake()
    {
        instant = this;
        CurrentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);
    }

    public void TakeDamage(int amount)
    {
        if (IsDead) return;

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
        IsDead = true;

        if (animator != null)
        {
            animator.SetTrigger("die");
        }

        PlayerController pc = GetComponent<PlayerController>();
        if (pc != null) pc.enabled = false;
    }
}