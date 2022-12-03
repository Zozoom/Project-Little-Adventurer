using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCaster : MonoBehaviour
{
    [Header("Damage Caster")]
    public int damage = 30;
    public string targetTag;
    private Collider _damageCasterCollider;
    private List<Collider> _damagedTargetList;

    private void Awake()
    {
        _damageCasterCollider = GetComponent<Collider>();
        _damageCasterCollider.enabled = false;

        _damagedTargetList = new List<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == targetTag && !_damagedTargetList.Contains(other))
        {
            Character targetCC = other.GetComponent<Character>();

            if (targetCC != null)
                targetCC.ApplyDamage(damage);

            _damagedTargetList.Add(other);
        }
    }

    public void EnableDamageCaster()
    {
        _damagedTargetList.Clear();
        _damageCasterCollider.enabled = true;
    }

    public void DisableDamageCaster()
    {
        _damagedTargetList.Clear();
        _damageCasterCollider.enabled = false;
    }
}
