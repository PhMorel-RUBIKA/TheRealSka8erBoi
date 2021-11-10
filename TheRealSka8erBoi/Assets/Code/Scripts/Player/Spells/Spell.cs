using UnityEngine;
using UnityEngine.UI;

public class Spell : ScriptableObject
{
    [Header("Ability Persona")]
    public int id;
    public string name;
    public string description;
    public Sprite spellImage;

    [Header("Ability Attributes")]
    public float cooldownTime;
    public float activeTime;

    public virtual void Activate (GameObject parent) {}
    public virtual void BeginCooldown (GameObject parent) {}
    public virtual void Gizmo (GameObject parent) {}
}