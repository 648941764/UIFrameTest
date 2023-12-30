using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class CharacterEntity
{
    /// <summary> config id /// </summary>
    private int _id;
    private int _uid;
    private int _health;
    private int _maxHealth;
    private int _level;
    private int _exp;
    private int _attack;
    private int _defence;
    private int _gold;
    private int _dropId;
    private bool _dead;

    public int ID => _id;
    public int UID => _uid;

    public int GetHealth() => _health;
    public int GetMaxHealth() => _maxHealth;
    public int GetLevel() => _level;
    public int GetExp() => _exp;
    public int GetAttack() => _attack;
    public int GetDefence() => _defence;
    public int GetGold() => _gold;
    public int GetDropId() => _dropId;
    public bool IsDead() => _dead;

    public void SetID(int value) => _id = value;
    public void SetUID(int value) => _uid = value;
    public void SetHealth(int value) => _health = value;
    public void SetMaxHealth(int value) => _maxHealth = value;
    public void SetLevel(int value) => _level = value;
    public void SetExp(int value) => _exp = value;
    public void SetAttack(int value) => _attack = value;
    public void SetDefence(int value) => _defence = value;
    public void SetGold(int value) => _gold = value;
    public void SetDropId(int value) => _dropId = value;
    public void SetAlive() => _dead = false;

    public void ChangeHealth(int value)
    {
        _health = Mathf.Clamp(_health + value, 0, _maxHealth);
        if (_health == 0)
        {
            _dead = true;
            EventManager.Instance.Broadcast(EventParam.Get(EventType.OnEnemyDeath, UID));
        }
        EventManager.Instance.Broadcast(EventParam.Get(EventType.OnHealthChange, UID));
    }

    public void ChangeExp(int value)
    {

    }
}