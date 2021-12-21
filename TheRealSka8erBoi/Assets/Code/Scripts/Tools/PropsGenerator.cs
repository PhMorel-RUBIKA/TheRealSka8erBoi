using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PropsGenerator : MonoBehaviour
{
    public enum GizmoColor
    {
        Rouge,
        Vert,
        Bleu, 
        Rose,
        Jaune,
        BleuClair,
        Blanc,
    }

    [Header("Generator Attributs")] 
    public bool simpleIsland;
    public float generatorSize;
    public GizmoColor gizmoColor;
    
    [Header("Object Parameters")]
    public int minObjectToSpawn;
    public int maxObjectToSpawn;
    public float gapBetweenObjects;
    public GameObject[] objectToSpawn;
    
    private Collider[] flowerColliders;
    private RaycastHit2D[] hit;
    private bool possible;
    
    private void OnDrawGizmos()
    {
        switch (gizmoColor)
        {
            case GizmoColor.Rouge: Gizmos.color = Color.red;
                break;
            case GizmoColor.Vert: Gizmos.color = Color.green;
                break;
            case GizmoColor.Bleu: Gizmos.color = Color.blue;
                break;
            case GizmoColor.Rose: Gizmos.color = Color.magenta;
                break;
            case GizmoColor.Jaune: Gizmos.color = Color.yellow;
                break;
            case GizmoColor.BleuClair: Gizmos.color = Color.cyan;
                break;
            case GizmoColor.Blanc: Gizmos.color = Color.white;
                break;
        }
        
        Gizmos.DrawWireSphere(transform.position, generatorSize);
    }

    private void Start()
    {
        GameObject propsParent = new GameObject();

        for (int i = 0; i < Random.Range(minObjectToSpawn, maxObjectToSpawn); i++)
        {
            possible = true;
            var position = this.transform.position;
            Vector3 pos = position + Random.insideUnitSphere * generatorSize;
            pos.z = 0;
            
            CheckDistance(pos);

            if (possible)
            {
                GameObject selected = objectToSpawn[Random.Range(0, objectToSpawn.Length)];
                GameObject obj = Instantiate(selected, pos, selected.transform.rotation, propsParent.transform);
                propsParent.name = obj + "Parent";
                propsParent.transform.parent = this.transform.parent;
            }
        }
    }
    
    void CheckDistance(Vector3 objectPosition)
    {
        flowerColliders = Physics.OverlapSphere(objectPosition, gapBetweenObjects);
        
        Vector2 direction = objectPosition - transform.position;
        hit = Physics2D.RaycastAll(transform.position, direction, Vector2.Distance(transform.position, objectPosition), LayerMask.GetMask("IslandBorder"));

        if (simpleIsland)
        {
            if (hit.Length == 1 || hit.Length == 3 || hit.Length == 5 || hit.Length == 7 || hit.Length == 9 || hit.Length == 11)
            {
                possible = false;
            }
        }
        else if (!simpleIsland)
        {
            if (hit.Length == 0 || hit.Length == 2 || hit.Length == 4 || hit.Length == 6 || hit.Length == 8 || hit.Length == 10)
            {
                possible = false;
            }
        }


        foreach (Collider flowerCollider in flowerColliders)
        {
            possible = false;
        }
    }
}
