using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class ShurikenFaucheuse : Spell
{
    private Vector2 leftJoy;

    public override void Activate(GameObject parent)
    {
        GameObject spawnedProj =
            PoolObjectManager.Instance.GetBullet("shurikenFaucheuse", parent.transform.position, Quaternion.Euler(Mathf.Atan2(parent.GetComponent<PlayerBehaviour>().latestDirection.y, -parent.GetComponent<PlayerBehaviour>().latestDirection.x) * Mathf.Rad2Deg,90,0));
        Debug.Log("shuriken");
        spawnedProj.GetComponent<BulletPoolBehaviour>().force = parent.GetComponent<PlayerBehaviour>().latestDirection.normalized;
    }
}
