using System;
using System.Collections.Generic;
using UnityEngine;

public class BossTimeline : MonoBehaviour
{
    [Header("Show Value")]
    [SerializeField] private int inSecond;
    [SerializeField] private int _frameCounter;
    [Space(20)]
    public int refreshTime = 1500;
    public List<AttackSlot> attackSlots = new List<AttackSlot>();

    private void FixedUpdate()
    {
        if (_frameCounter < refreshTime) _frameCounter += 1;
        else _frameCounter = 0;
        inSecond = (int)(_frameCounter / 50);

        for (int i = 0; i < attackSlots.Count; i++)
        {
            if (_frameCounter == attackSlots[i].callingFrame)
            {
                switch (attackSlots[i].attackLibrary)
                {
                    case AttackLibrary.Attack1 : 
                        Attack1(attackSlots[i].attack1);
                        break;
                    case AttackLibrary.Attack2 :
                        Attack2(attackSlots[i].attack2);
                        break;
                    case AttackLibrary.Attack3 :
                        Attack3(attackSlots[i].attack3);
                        break;
                    case AttackLibrary.Attack4 :
                        Attack4(attackSlots[i].attack4);
                        break;
                }
            }
        }
    }

    void Attack1(Attack1 attack1)
    {
        throw new NotImplementedException();
        float speed = attack1.var1;
        int size = attack1.var2;
        bool isActive = attack1.var3;
    }
    
    void Attack2(Attack2 attack2)
    { 
        throw new NotImplementedException();
        float speed = attack2.var1;
        int size = attack2.var2;
        bool isActive = attack2.var3;
    }
    
    void Attack3(Attack3 attack3)
    { 
        throw new NotImplementedException();
        float speed = attack3.var1;
        int size = attack3.var2;
        bool isActive = attack3.var3;
    }
    
    void Attack4(Attack4 attack4)
    { 
        throw new NotImplementedException();
        float speed = attack4.var1;
        int size = attack4.var2;
        bool isActive = attack4.var3;
    }
    
}

public enum AttackLibrary
{
    Attack1, 
    Attack2,
    Attack3,
    Attack4,
}

[Serializable]
public class AttackSlot
{
    public string attackName;
    public int callingFrame;
    
    public AttackLibrary attackLibrary;
    public Attack1 attack1;
    public Attack2 attack2;
    public Attack3 attack3;
    public Attack4 attack4;
}

[Serializable]
public class Attack1
{
    public float var1;
    public int var2;
    public bool var3;
}

[Serializable]
public class Attack2
{
    public float var1;
    public int var2;
    public bool var3;
}

[Serializable]
public class Attack3
{
    public float var1;
    public int var2;
    public bool var3;
}

[Serializable]
public class Attack4
{
    public float var1;
    public int var2;
    public bool var3;
}
