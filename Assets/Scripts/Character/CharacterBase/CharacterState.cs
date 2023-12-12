using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEngine.EventSystems.EventTrigger;

public enum CharacterState
{
    None = 0, Idle, Run, Jump, Fall, Attack, AttackCooling, coolDown, Hurt, Death, Guard, Patrol
}

public class AnimTime
{
    /// <summary> ������ʱ��: ms /// </summary>
    public Dictionary<int, Action> keyActions;
    private Timer _timer;

    public int Tick => _timer.TickTime;

    public bool Ticking => _timer.Ticking;

    public AnimTime(int time, Action onComplete) //����¼���ʱ�䣬������
    {
        _timer = new Timer()
        {
            time = time,
            onTick = OnTick,
            onComplete = onComplete,
        };
    }

    public void Start()
    {
        _timer.Start();
    }

    public void Break()
    {
        _timer.BreakOff();
    }

    private void OnTick(int tick)
    {
        if (keyActions == null) { return; }
        foreach (int key in keyActions.Keys)
        {
            if (key == tick)
            {
                keyActions[key]();
            }
        }
    }
}

public class CharacterParameter : FSMParameter
{
    public Dictionary<CharacterState, string> stateClips;
    public Dictionary<CharacterState, AnimTime> animStates;
    public Animator animator;
    public Character character;
    public Character target;
    public float patrolPosX;
    public float timer;
    public float attckCoolTimer;
}

public abstract class EnemyFSMState : IFSMState
{
    protected CharacterParameter param;
    protected FSM fsm;
    public FSM FSM { get => fsm; }

    protected Vector3 BornPosition => (param.character as Enemy).BornPosition;

    protected CharacterInfo CharacterInfo => param.character.CharacterInfo;

    public virtual void OnInit(FSM fsm)
    {
        this.fsm = fsm;
        param = FSM.GetParameter<CharacterParameter>();//��ȡ��ɫstates����Ķ�������
    }

    public virtual void OnEnter()
    {
        string animName = param.stateClips[FSM.GetStateName<CharacterState>()];
        if (!string.IsNullOrEmpty(animName))
        {
            param.animator.Play(animName);  
        }
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

    /// <summary>
    /// ����Player
    /// </summary>
    protected virtual void FacePlayer()
    {
        Character player = CharacterManager.Instance.Player;
        if (player == null)
        {
            return;
        }

        param.character.Orientation = !(param.character.Position.x > player.Position.x);
    }

    /// <summary>
    /// ���Player�Ƿ���Ѳ�߷�Χ,�ý�ɫ����Player
    /// </summary>
    protected virtual bool IsPlayerInSight()
    {
        Character player = CharacterManager.Instance.Player;
        if (player == null)
        {
            return false;
        }

        // ������Ҫ�ж��Ƿ���һ��y����
        if (Mathf.Abs(player.Position.y - param.character.Position.y) >= 2.0f)
        {
            return false;
        }

        if (!IsPlayerInPatrolRange())
        {
            // ����Ѳ�߷�Χ�ڲ����
            return false;
        }

        // �����Ұ��Χ
        if (param.character.Orientation)
        {
            // ����
            return player.Position.x > param.character.Position.x
                && player.Position.x <= param.character.Position.x + CharacterInfo.seeRange;
        }
        else
        {
            // ����
            return player.Position.x < param.character.Position.x
                && player.Position.x >= param.character.Position.x - CharacterInfo.seeRange;
        }
    }

    /// <summary>
    /// ���Player�Ƿ���Ѳ�߷�Χ
    /// </summary>
    protected virtual bool IsPlayerInPatrolRange()
    {
        Character player = CharacterManager.Instance.Player;
        if (player == null)
        {
            return false;
        }
        return player.Position.x >= BornPosition.x - CharacterInfo.patrolRange 
            && player.Position.x <= BornPosition.x + CharacterInfo.patrolRange;
    }

    /// <summary>
    /// �ƶ�
    /// </summary>
    protected virtual void Move()
    {
        int sign = param.character.Orientation ? 1 : -1;
        Vector3 pos = param.character.Position;
        pos.x += sign * param.character.CharacterInfo.moveSpeed * Time.deltaTime;
        if (pos.x < BornPosition.x - CharacterInfo.patrolRange || pos.x > BornPosition.x + CharacterInfo.patrolRange)
        {
            return;
        }
        param.character.Position = pos;
    }
}

public class DeathState : EnemyFSMState
{
    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnExecute()
    {
    }

    public override void OnExit()
    {
    }
}
