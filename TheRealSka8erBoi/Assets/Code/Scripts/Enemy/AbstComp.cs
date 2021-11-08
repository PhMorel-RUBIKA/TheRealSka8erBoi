using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;


public class AbstComp : MonoBehaviour 
{
    public GameObject pj;
    
    public float hp;
    public float lineOfSight;
    protected float distanceFromPlayer;
    [SerializeField] float range = 1;
    

    public bool CheckPlayerInSight()
    {
        distanceFromPlayer = Vector2.Distance(pj.transform.position, this.transform.position);
        return distanceFromPlayer < lineOfSight;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lineOfSight);
    }
        protected bool CheckPlayerInRange()
    {
        return distanceFromPlayer <= range;
    }


}