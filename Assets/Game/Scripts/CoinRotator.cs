using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinRotator : MonoBehaviour
{
    public float speed = 80f;

    private void Update()
    {
        transform.Rotate(new Vector3(0f, speed * Time.deltaTime, 0f), Space.World);
    }
}
