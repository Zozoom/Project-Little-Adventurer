using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float horizontalInput, verticalInput;
    public bool mouseButtonDown;
    public bool spaceKeyDown;

    private void Update()
    {
        // Time.timeScale != 0 this means the game not paused
        if (!mouseButtonDown && Time.timeScale != 0)
        {
            mouseButtonDown = Input.GetMouseButtonDown(0);
        }

        if (!spaceKeyDown && Time.timeScale != 0)
        {
            spaceKeyDown = Input.GetKeyDown(KeyCode.Space);
        }

        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void OnDisable()
    {
        ClearChace();
    }

    public void ClearChace()
    {
        mouseButtonDown = false;
        spaceKeyDown = false;
        horizontalInput = 0;
        verticalInput = 0;
    }
}
