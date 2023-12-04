using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum CharacterState
{
    Idle, Run, Jump, Fall, Attack ,Hurt ,Death
}

public class AnimTime
{
    /// <summary> 触发事件的时间点: ms /// </summary>
    public int[] keys;
    /// <summary> 动画总时长: ms /// </summary>
    public int time;
    public Action[] actions;
    public int keyCount;

    public AnimTime(int time, int[] keys = default, Action[] actions = default)//添加事件的时间，次数，
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

public class CharacterFSMParameter : FSMParameter
{
    public Dictionary<CharacterState, string> stateClips;
    public Dictionary<CharacterState, AnimTime> animStates;
    public Animator animator;
    public Character character;
    public Character target;
}

public abstract class CharacterFSMState : IFSMState
{
    protected CharacterFSMParameter param;
    protected FSM fsm;
    public FSM FSM { get => fsm; }

    public virtual void OnInit(FSM fsm)
    {
        this.fsm = fsm;
        param = FSM.GetParameter<CharacterFSMParameter>();
    }

    public virtual void OnEnter()
    {
        param.animator.Play(param.stateClips[FSM.GetStateName<CharacterState>()]);
    }

    public abstract void OnExecute();

    public abstract void OnExit();

    protected bool TryGetAnimTime(CharacterState state, out AnimTime animTime)
    {
        animTime = null;
        if (param.animStates == null)
        {
            return false;
        }

        if (param.animStates.ContainsKey(state))
        {
            animTime = param.animStates[state];
            return true;
        }

        return false;
    }
}
