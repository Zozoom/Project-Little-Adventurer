using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Character Health")]
    public int maxHealth;
    public int currentHealth;
    private Character _cc;

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
        Debug.Log(_cc.charName + ": H[" + currentHealth + " / " + maxHealth + "]");
        if (currentHealth <= 0)
        {
            _cc.SwitchStateTo(Character.CharacterState.Dead);
        }
    }
}
