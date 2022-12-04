using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropWeapons : MonoBehaviour
{
    [Header("Weapons Handle")]
    public List<GameObject> weapons;

    public void DropSwords()
    {
        foreach (GameObject item in weapons)
        {
            item.AddComponent<Rigidbody>();
            item.AddComponent<BoxCollider>();
            item.transform.parent = null;
        }
    }
}
