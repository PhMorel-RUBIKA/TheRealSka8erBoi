using UnityEngine;

public class Spell : ScriptableObject
{
    [Header("Ability Persona")]
    public string name;
    public string description;
    public int id;

    [Header("Ability Attributes")]
    public float cooldownTime;
    public float activeTime;

    public virtual void Activate (GameObject parent) {}
    public virtual void BeginCooldown (GameObject parent) {}
    public virtual void Gizmo (GameObject parent) {}
}