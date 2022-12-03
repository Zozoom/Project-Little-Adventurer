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
        if (damage > currentHealth && (currentHealth <= 0))
            Debug.Log(_cc.charName + " is Dead!");
        else
            currentHealth -= damage;

        Debug.Log(_cc.charName + ": D[" + damage + "] / H[" + currentHealth + "]");
    }
}
