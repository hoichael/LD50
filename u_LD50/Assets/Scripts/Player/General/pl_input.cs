using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pl_input : MonoBehaviour
{
    public float mouseX, mouseY;

    public float moveX, moveY;

    private void Update()
    {
        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");

        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");
    }
}
