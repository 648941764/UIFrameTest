using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    public int attack;
    public int hp;
    public int defence;
    public int Exp;
    public float speed;
    

    public virtual void Move() { }
    public virtual void Attack(CharacterBase target)
    {
        int damage = Mathf.Clamp(1, this.attack - target.defence, this.attack);
        target.hp -= damage;
        if (target.hp < 0)
        {
            target.Die();
            this.Exp += target.Exp;
        }
    }
    public virtual void Skill() { }
    public virtual void Die() { }

    public virtual void MoveTo(CharacterBase target) { }

}
