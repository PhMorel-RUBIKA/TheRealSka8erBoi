using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class ShurikenFaucheuse : Spell
{
    public override void Activate(GameObject parent)
    {
        GameObject spawnedProj =
            PoolObjectManager.Instance.GetBullet("shurikenFaucheuse", parent.transform.position, parent.transform.rotation);
        Debug.Log("shuriken");
        spawnedProj.GetComponent<BulletPoolBehaviour>().force = parent.GetComponent<PlayerBehaviour>().latestDirection.normalized;
    }
}
