using System;
using UnityEngine;

public class SpellHolder : MonoBehaviour
{
    public Inventory inventory;
    [Range(0,1)] public int spellNumber;
    [HideInInspector] public Spell spell;
    public Spell neutral;
    private float cooldownTime;
    private float activeTime;

    enum SpellState
    {
        ready,
        active,
        cooldown, 
    }

    SpellState state = SpellState.ready;

    public KeyCode key;
    public string AxeKey;

    private void Update()
    {
        if (inventory.slots[spellNumber].item != null)
        {
            spell = inventory.slots[spellNumber].item.GetComponent<Item>().TheItem.SpellItem.spellScriptable;
        }
        else
        {
            spell = neutral;
        }
        
        
        switch (state)
        {
            case SpellState.ready:
                if (Input.GetAxisRaw(AxeKey) > 0.90f)
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
                }
                break;
            case SpellState.cooldown:
                state = SpellState.ready;
                break;
        }
    }
}