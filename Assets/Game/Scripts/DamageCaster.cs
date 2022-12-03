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
            {
                PlayerVfxManager playerVfxManager = transform.parent.GetComponent<PlayerVfxManager>();
                if (playerVfxManager != null)
                {
                    RaycastHit hit;

                    Vector3 originalPos = transform.position + (-_damageCasterCollider.bounds.extents.z) * transform.forward;

                    bool istHit = Physics.BoxCast(originalPos, _damageCasterCollider.bounds.extents / 2, transform.forward, out hit, transform.rotation, _damageCasterCollider.bounds.extents.z, 1 << 6);

                    if (istHit)
                    {
                        playerVfxManager.PlaySlash(hit.point + new Vector3(0f, 0.5f, 0f));
                        targetCC.ApplyDamage(damage, transform.parent.position);
                    }
                }

            }

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

    private void OnDrawGizmos()
    {
        if (_damageCasterCollider == null)
            _damageCasterCollider = GetComponent<Collider>();

        RaycastHit hit;

        Vector3 originalPos = transform.position + (-_damageCasterCollider.bounds.extents.z) * transform.forward;

        bool istHit = Physics.BoxCast(originalPos, _damageCasterCollider.bounds.extents / 2, transform.forward, out hit, transform.rotation, _damageCasterCollider.bounds.extents.z, 1 << 6);

        if (istHit)
        {
            Debug.Log("Gizmoo...");
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(hit.point, 0.3f);
        }
    }
}
