using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Character Health")]
    private Character _cc;
    public int maxHealth;
    public int currentHealth;
    public float currentHealthPercentage
    {
        get
        {
            return (float)currentHealth / maxHealth;
        }
    }
    private void Awake()
    {
        currentHealth = maxHealth;
        _cc = GetComponent<Character>();
    }

    public void ApplyDamage(int damage)
    {
        if (currentHealth <= damage)
            currentHealth -= currentHealth;
        else
            currentHealth -= damage;

        CheckHealth();
    }

    private void CheckHealth()
    {
        // Debug.Log(_cc.charName + ": H[" + currentHealth + " / " + maxHealth + "]");
        if (currentHealth <= 0)
        {
            _cc.SwitchStateTo(Character.CharacterState.Dead);
        }
    }

    public void AddHealth(int addHealth)
    {
        currentHealth += addHealth;

        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
    }
}
