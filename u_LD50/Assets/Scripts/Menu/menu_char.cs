using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menu_char : MonoBehaviour
{
    public bool pauseAnim;

    private float animInterval = 0.6f;

    private Vector3 currentTargetPos;

    private float moveSpeed = 0.03f;

    private Vector3 initPos;

    private void Start()
    {
        //   StartCoroutine(GetRandomPos());
        initPos = transform.localPosition;
        UpdateTargetPos();
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
    }

    private void UpdateTargetPos()
    {
        Vector3 randomOffset = new Vector3(
            Random.Range(-0.1f, 0.1f),
            Random.Range(-0.1f, 0.1f),
            Random.Range(-0.07f, 0.07f)
            );

        currentTargetPos = transform.localPosition + randomOffset + (initPos - transform.localPosition);
    }

    private void UpdateTargetRot()
    {
        Vector3 randomOffset = new Vector3(
            Random.Range(-0.1f, 0.1f),
            Random.Range(-0.1f, 0.1f),
            Random.Range(-0.07f, 0.07f)
    );
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
