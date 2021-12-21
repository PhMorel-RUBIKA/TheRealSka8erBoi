using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ShadowDash : Spell
{
    public override void Activate(GameObject parent)
    {
        if (parent.GetComponent<PlayerBehaviour>().dashSpellActive == true)
        {
            parent.GetComponent<PlayerBehaviour>().dashSpellactivation = 0;  
            parent.GetComponent<PlayerBehaviour>().DashSpell();
        }

        parent.GetComponent<PlayerBehaviour>().dashNodeList = new List<GameObject>(); 
        parent.GetComponent<PlayerBehaviour>().dashSpellActive = true;
        parent.GetComponent<PlayerBehaviour>().dashSpellactivation = 4;
    }
}
