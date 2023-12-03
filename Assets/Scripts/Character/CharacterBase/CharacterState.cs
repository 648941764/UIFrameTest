using System;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterState
{
    Idle, Run, Jump, Fall, Attack, Death
}

public class AnimationTime
{
    /// <summary> �����¼���ʱ���: ms /// </summary>
    public int[] keys;
    /// <summary> ������ʱ��: ms /// </summary>
    public int time;
    public Action[] actions;
    public int keyCount;

    public AnimationTime(int time, int[] keys = default, Action[] actions = default)//����¼���ʱ�䣬������
    {
        this.time = time;
        if (keys != default)
        {
            this.keys = keys;
            keyCount = keys.Length;
            this.actions = actions != null ? actions : new Action[keyCount];
        }
    }
}

public class CharacterParameter : FSMParameter
{
    public Dictionary<CharacterState, string> stateClips;
    public Dictionary<CharacterState, AnimationTime> animStates;
    public Animator animator;
    public Character character;
    public Character target;
}
