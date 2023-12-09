using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[System.Serializable]
public class CharacterInfo
{
    public float moveSpeed;
    public float jumpSpeed;
    // ��X������λtrue
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

    /// <summary> xyƽ����, true �����ұߣ� false ������� /// </summary>
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

    private void Start()
    {
        Init();
    }

    protected virtual void Init()
    {

    }
}
