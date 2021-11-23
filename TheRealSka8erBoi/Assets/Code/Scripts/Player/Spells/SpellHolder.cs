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
    public float cooldownNumber;

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
                if (Input.GetAxisRaw(AxeKey) > 0.95f)
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
                    cooldownNumber = 0;
                }
                break;
            case SpellState.cooldown:
                if (cooldownTime > 0)
                {
                    cooldownTime -= Time.deltaTime;
                    cooldownNumber += 0.5f / cooldownTime * Time.deltaTime;
                    if (cooldownNumber >= 1)
                        cooldownNumber = 1;
                }
                else
                {
                    state = SpellState.ready;
                    cooldownNumber = 1;
                }
                break;
        }
    }
}