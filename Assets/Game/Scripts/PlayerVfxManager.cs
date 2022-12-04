using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerVfxManager : MonoBehaviour
{
    [Header("Player VFX")]
    public VisualEffect footStep;
    public VisualEffect slash;
    public VisualEffect heal;
    public ParticleSystem balde01;
    public ParticleSystem balde02;
    public ParticleSystem balde03;

    public void PlayHealVFX()
    {
        heal.Play();
    }

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
    public void PlayBlade02()
    {
        balde02.Play();
    }
    public void PlayBlade03()
    {
        balde03.Play();
    }

    public void StopBlade()
    {
        balde01.Simulate(0);
        balde01.Stop();
        balde02.Simulate(0);
        balde02.Stop();
        balde03.Simulate(0);
        balde03.Stop();
    }

    public void PlaySlash(Vector3 pos)
    {
        slash.transform.position = pos;
        slash.Play();
    }
}
