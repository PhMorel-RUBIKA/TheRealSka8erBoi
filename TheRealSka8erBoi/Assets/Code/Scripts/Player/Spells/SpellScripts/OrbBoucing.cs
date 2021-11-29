using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class OrbBoucing : Spell
{
    public override void Activate(GameObject parent)
    {
        GameObject spawnedProj = PoolObjectManager.Instance.GetBullet("boucingOrb", parent.transform.position, parent.transform.rotation);
    }
}
