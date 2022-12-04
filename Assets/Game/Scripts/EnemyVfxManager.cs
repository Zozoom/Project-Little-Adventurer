using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EnemyVfxManager : MonoBehaviour
{
    [Header("Enemy VFX")]
    public VisualEffect footStep;
    public VisualEffect attackVFX;
    public VisualEffect beingHitSplashVFX;
    public ParticleSystem beingHit;
    public VisualEffect slash;
    public void BurstFootStep()
    {
        footStep.SendEvent("OnPlay");
    }

    public void PlayAttackVFX()
    {
        attackVFX.SendEvent("OnPlay");
    }

    public void PlaySlash(Vector3 pos)
    {
        slash.transform.position = pos;
        slash.Play();
    }

    public void PlayBeingHitVFX(Vector3 attackerPos)
    {
        Vector3 forceForward = transform.position - attackerPos;
        forceForward.Normalize();
        forceForward.y = 0;
        beingHit.transform.rotation = Quaternion.LookRotation(forceForward);
        beingHit.Play();

        // ====================================================================== //
        // TODO: Potential Memory Leak - Because Init and Destroying a lot VFX.
        // Use Object Pool, or DOTS(ECS, Job System, Burst Complier)
        Vector3 splashPos = transform.position;
        splashPos.y = 2f;
        VisualEffect newSplashVFX = Instantiate(beingHitSplashVFX, splashPos, Quaternion.identity);
        newSplashVFX.SendEvent("OnPlay");
        Destroy(newSplashVFX.gameObject, 10f);
        // ====================================================================== //
    }
}
