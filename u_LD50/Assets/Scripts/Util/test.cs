using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public Transform objLF;
    public Transform objLB;
    public Transform objRF;
    public Transform objRB;

    public float mult = 10;

    public Vector3 offsetLF;
    public Vector3 offsetLB;
    public Vector3 offsetRF;
    public Vector3 offsetRB;

    private float offset;

    private void Update()
    {
        // reset offsets
        offsetLF = offsetLB = offsetRF = offsetRB = Vector3.zero;

        // handle LF
        offset = objLF.position.y - Vector3.zero.y;
        offsetLF = new Vector3(offset * -mult, 0, offset * -mult);

        // handle LB
        offset = objLB.position.y - Vector3.zero.y;
        offsetLB = new Vector3(offset * mult, 0, offset * -mult);

        // handle RF
        offset = objRF.position.y - Vector3.zero.y;
        offsetRF = new Vector3(offset * -mult, 0, offset * mult);

        // handle RB
        offset = objRB.position.y - Vector3.zero.y;
        offsetRB = new Vector3(offset * mult, 0, offset * mult);

        // calc final offset
        Vector3 finalOffset = (offsetLF + offsetLB + offsetRF + offsetRB) / 4;

        // apply final offset
        transform.rotation = Quaternion.Euler(finalOffset);
    }
}
