using System;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterState
{
    Idle, Run, Jump, Fall, Attack, Death
}

public class AnimationTime
{
    /// <summary> 触发事件的时间点: ms /// </summary>
    public int[] keys;
    /// <summary> 动画总时长: ms /// </summary>
    public int time;
    public Action[] actions;
    public int keyCount;

    public AnimationTime(int time, int[] keys = default, Action[] actions = default)//添加事件的时间，次数，
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
