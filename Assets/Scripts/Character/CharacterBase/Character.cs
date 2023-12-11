using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class CharacterInfo
{
    public float moveSpeed;
    public float jumpSpeed;
    public float attackInterval = 1.2f;
    public float attackRange = 0.5f;
    public float patrolRange = 2.0f;
    public float seeRange = 2.0f;
    public float guardTime = 3f;
    public int   attackDamage0 = 3;
    public int   attackDamage1 = 5;
    // 朝X正方向位true
    public bool orientation = true;
}

public abstract class Character : MonoBehaviour
{
    [SerializeField] private CharacterInfo characterInfo;

    [SerializeField] private int _uid;

    protected CharacterParameter parameter;
    public CharacterParameter Parameter => parameter;

    protected static CharacterManager Manager => CharacterManager.Instance;
    public CharacterInfo CharacterInfo => characterInfo;

    public int UID => _uid;

    public Vector3 Position
    {
        get => transform.position;
        set => transform.position = value;
    }

    /// <summary> xy平面上, true 朝向右边， false 朝向左边 /// </summary>
    public bool Orientation
    {
        get => characterInfo.orientation;
        set
        {
            if (characterInfo.orientation != value)
            {
                characterInfo.orientation = value;
                transform.rotation = Quaternion.Euler(0f, value ? 0f : 180f, 0f);
            }
        }
    }

    private void Start()
    {
        Init();
    }

    protected virtual void Init()
    {

    }

    public abstract void TakeDamage(int damage);
}
