using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerVfxManager : MonoBehaviour
{
    [Header("Player VFX")]
    public VisualEffect footStep;
    public ParticleSystem balde01;
    public void Update_FootStep(bool state)
    {
        if (state)
            footStep.Play();
        else
            footStep.Stop();
    }

    public void PlayBlade01()
    {
        balde01.Play();
    }
}
