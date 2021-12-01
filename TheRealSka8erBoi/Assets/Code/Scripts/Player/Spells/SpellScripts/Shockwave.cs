using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class Shockwave : Spell
{
    public GameObject prefabShockwave;
    public override void Activate(GameObject parent)
    {
        Destroy(Instantiate(prefabShockwave, parent.transform), 0.4f);
    }
}
