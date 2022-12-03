using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EnemyVfxManager : MonoBehaviour
{
    public VisualEffect footStep;
    public void BurstFootStep()
    {
        footStep.SendEvent("OnPlay");
    }
}
