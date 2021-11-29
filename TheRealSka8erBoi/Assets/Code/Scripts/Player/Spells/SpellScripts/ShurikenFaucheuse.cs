using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class ShurikenFaucheuse : Spell
{
    private Vector2 leftJoy;

    public override void Activate(GameObject parent)
    {
        leftJoy.x = Input.GetAxisRaw("Horizontal");
        leftJoy.y = Input.GetAxisRaw("Vertical");
        GameObject spawnedProj =
            PoolObjectManager.Instance.GetBullet("shurikenFaucheuse", parent.transform.position, Quaternion.Euler(0f,0f,Mathf.Atan2(leftJoy.y, leftJoy.x) * Mathf.Rad2Deg));
            
        Debug.Log("shuriken");
        spawnedProj.GetComponent<BulletPoolBehaviour>().force = parent.GetComponent<PlayerBehaviour>().latestDirection.normalized;
    }
}
