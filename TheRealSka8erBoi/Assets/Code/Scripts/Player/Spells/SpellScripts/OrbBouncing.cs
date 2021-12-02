using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class OrbBouncing : Spell
{
    public override void Activate(GameObject parent)
    {
        GameObject spawnedProj = PoolObjectManager.Instance.GetBullet("bouncingOrb", parent.transform.position, parent.transform.rotation);
        spawnedProj.GetComponent<BouncingOrbBehaviour>().target = parent.transform;
    }
}
