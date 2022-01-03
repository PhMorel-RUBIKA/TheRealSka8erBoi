using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class ShurikenFaucheuse : Spell
{
    private Vector2 leftJoy;

    public override void Activate(GameObject parent)
    {
        parent.GetComponent<PlayerBehaviour>().shurikenActive = true;
    }
}
