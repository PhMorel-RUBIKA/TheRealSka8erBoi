using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NaveMeshGenerator : MonoBehaviour
{
    private void Start()
    {
        NavMeshSurface2d[] allChildren = GetComponentsInChildren<NavMeshSurface2d>();

        for (int i = 0; i < allChildren.Length; i++)
            allChildren[i].BuildNavMesh();
    }
}
