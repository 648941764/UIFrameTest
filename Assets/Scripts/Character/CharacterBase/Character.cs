using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[System.Serializable]
public class CharacterInfo
{
    public float moveSpeed;
    public float jumpSpeed;
    public float jumpHeight;
    // 朝X正方向位true
    public bool orientation = true;
}

public abstract class Character : MonoBehaviour
{
    [SerializeField] private CharacterInfo characterInfo;

    protected FSM fsm;

    private int _uid;

    private static CharacterManager Manager => CharacterManager.Instance;
    public CharacterInfo CharacterInfo => characterInfo;
    public int UID => _uid;
    public FSM FSM => fsm;

    public Vector3 Position
    {
        get => transform.position;
        set => transform.position = value;
    }

    public bool Orientation
    {
        set
        {
            if (characterInfo.orientation != value)
            {
                characterInfo.orientation = value;
                transform.rotation = Quaternion.Euler(0f, value ? 0f : 180f, 0f);
            }
        }
    }

    public void Init(int uid)
    {
        InitFSM();
    }

    public abstract void InitFSM();
}
