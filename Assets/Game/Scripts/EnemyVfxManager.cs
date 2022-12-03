using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EnemyVfxManager : MonoBehaviour
{
    public VisualEffect footStep;
    public VisualEffect attackVFX;
    public void BurstFootStep()
    {
        footStep.SendEvent("OnPlay");
    }

    public void PlayAttackVFX()
    {
        attackVFX.SendEvent("OnPlay");
    }
}
