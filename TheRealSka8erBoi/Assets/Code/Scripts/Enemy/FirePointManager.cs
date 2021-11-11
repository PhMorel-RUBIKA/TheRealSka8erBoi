using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePointManager : MonoBehaviour
{
    public Transform source;
    private GameObject pj;
    public float moveSpeed = 3f;

    private void Start()
    {
        pj = PlayerBehaviour.playerBehaviour.gameObject;
    }

    void Update()
    { 
        Vector2 direction = pj.transform.position - source.transform.position;
        Vector2 distance = (source.transform.position-transform.position);
        transform.localPosition = direction.normalized; //Vector2.MoveTowards(this.transform.position, pj.transform.position, moveSpeed * Time.deltaTime).normalized;

    }
}
