using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pl_input : MonoBehaviour
{
    public float mouseX;
    public float mouseY;

    private void Update()
    {
        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");
    }
}
