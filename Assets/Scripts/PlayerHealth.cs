using System;
using System.Linq;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 5;
    public int CurrentHealth { get; private set; }
    public bool IsDead { get; private set; } = false;

    public static PlayerHealth instant;

    public event Action<int, int> OnHealthChanged;
    public event Action OnPlayerDied; // se déclenche après l'anim de mort

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


        public void Kill()
    {
        if (IsDead) return;
        CurrentHealth = 0;
        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);
        Die();
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

        StartCoroutine(DeathSequence());
    }

    private System.Collections.IEnumerator DeathSequence()
    {
        float delay = GetDeathClipLength();
        yield return new WaitForSeconds(delay);
        OnPlayerDied?.Invoke();
    }

    private float GetDeathClipLength()
    {
        if (animator == null || animator.runtimeAnimatorController == null) return 1f;

        var deathClip = animator.runtimeAnimatorController.animationClips
            .FirstOrDefault(c => c.name.ToLower().Contains("death"));

        return deathClip != null ? deathClip.length : 1f;
    }
}