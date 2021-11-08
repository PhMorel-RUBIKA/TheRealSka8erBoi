using UnityEngine;

public class SpellHolder : MonoBehaviour
{
    public Spell spell;
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