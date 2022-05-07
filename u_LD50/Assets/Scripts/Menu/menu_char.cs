using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menu_char : MonoBehaviour
{
    public bool pauseAnim;

    private float animInterval = 0.6f;

    private Vector3 currentTargetPos;

    private Vector3 currentTargetRot;

    // private float moveSpeed = 0.03f;
    // private float rotSpeed = 0.4f;

    private float moveSpeed = 0.14f;
    private float rotSpeed = 1.2f;

    private Vector3 initPos;

    private void Start()
    {
        //   StartCoroutine(GetRandomPos());
        initPos = transform.localPosition;
        UpdateTargetPos();
        UpdateTargetRot();
    }

    private void Update()
    {
        if (pauseAnim) return;
        Anim();
    }

    private void Anim()
    {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, currentTargetPos, moveSpeed * Time.deltaTime);
        if(transform.localPosition == currentTargetPos) UpdateTargetPos();

        transform.localRotation = Quaternion.Euler(Vector3.MoveTowards(transform.localEulerAngles, currentTargetRot, rotSpeed * Time.deltaTime));
        if (transform.localRotation == Quaternion.Euler(currentTargetRot)) UpdateTargetRot();
    }

    private void UpdateTargetPos()
    {
        Vector3 randomOffset = new Vector3(
            Random.Range(-0.1f, 0.1f),
            Random.Range(-0.17f, 0.17f),
            Random.Range(-0.07f, 0.07f)
            );

        currentTargetPos = transform.localPosition + randomOffset + (initPos - transform.localPosition);
    }

    private void UpdateTargetRot()
    {
        Vector3 randomOffset = new Vector3(
            Random.Range(-3.2f, 3.2f),
            Random.Range(-5.6f, 5.6f),
            Random.Range(-0.2f, 0.2f)
            );

        currentTargetRot = transform.localEulerAngles + randomOffset;
    }

    /*
    private IEnumerator GetRandomPos()
    {
        Vector3 randomOffset = new Vector3(
            Random.Range(-0.2f, 0.2f),
            Random.Range(-0.2f, 0.2f),
            Random.Range(-0.1f, 0.1f)
            );

        currentTargetPos = transform.position + randomOffset;

        yield return new WaitForSeconds(animInterval);
        StartCoroutine(GetRandomPos());
    }
    */
}
