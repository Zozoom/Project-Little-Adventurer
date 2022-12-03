using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EnemyVfxManager : MonoBehaviour
{
    [Header("Enemy VFX")]
    public VisualEffect footStep;
    public VisualEffect attackVFX;
    public ParticleSystem beingHit;
    public void BurstFootStep()
    {
        footStep.SendEvent("OnPlay");
    }

    public void PlayAttackVFX()
    {
        attackVFX.SendEvent("OnPlay");
    }

    public void PlayBeingHitVFX(Vector3 attackerPos)
    {
        Vector3 forceForward = transform.position - attackerPos;
        forceForward.Normalize();
        forceForward.y = 0;
        beingHit.transform.rotation = Quaternion.LookRotation(forceForward);
        beingHit.Play();
    }
}
