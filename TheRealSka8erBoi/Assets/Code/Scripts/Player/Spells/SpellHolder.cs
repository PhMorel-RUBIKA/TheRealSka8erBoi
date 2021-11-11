using System;
using UnityEngine;

public class SpellHolder : MonoBehaviour
{
    public Inventory slot1;
    [Range(0,1)] public int spellNumber;
    [HideInInspector] public Spell spell;
    private float cooldownTime;
    private float activeTime;

    enum SpellState
    {
        ready,
        active,
        cooldown, 
    }

    private SpellState state = SpellState.ready;

    public KeyCode key;

    private void Start()
    {
        spell = slot1.slots[spellNumber].GetComponent<Item>().TheItems[0].SpellItem.spellScriptable;
    }

    private void Update()
    {
        switch (state)
        {
            case SpellState.ready:
                if (Input.GetKeyDown(key))
                {
                    spell.Activate(this.gameObject);
                    state = SpellState.active;
                    activeTime = spell.activeTime;
                }
                break;
            case SpellState.active:
                if (activeTime > 0)
                    activeTime -= Time.deltaTime;
                else
                {
                    spell.BeginCooldown(this.gameObject); 
                    state = SpellState.cooldown;
                    cooldownTime = spell.cooldownTime;
                }
                break;
            case SpellState.cooldown:
                if (cooldownTime > 0)
                    cooldownTime -= Time.deltaTime;
                else
                    state = SpellState.ready;
                break;
        }
    }

    private void OnDrawGizmos()
    {
        spell.Gizmo(this.gameObject);
    }
}