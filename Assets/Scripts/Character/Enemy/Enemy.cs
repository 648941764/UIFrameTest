using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : Character
{
    protected FSM fsm;

    protected Vector3 bornPosition;

    public int attackDamageTime;

    public FSM FSM => fsm;

    public Vector3 BornPosition => bornPosition;

    /// <summary>
    /// π÷ŒÔ≤‚ ‘
    /// </summary>
    protected override void Init()
    {
        base.Init();
        Init(0);
    }

    public void Init(int uid)
    {
        bornPosition = Position;
        Orientation = false;
        InitFSMParameter();
        InitFSM();
    }

    public void ExitFSM()
    {
        fsm.OnExit();
    }

    protected virtual void InitFSMParameter()
    {
        parameter = new CharacterParameter()
        {
            animator = GetComponent<Animator>(),
            character = this,
            defaultStateName = CharacterState.Guard,
            stateClips = new Dictionary<CharacterState, string>()
            {
                [CharacterState.Guard] = "idle",
                [CharacterState.Patrol] = "run",
                [CharacterState.Attack] = "attack",
                [CharacterState.Hurt] = "hurt",
                [CharacterState.Death] = "death",
                [CharacterState.Run] = "run",
                [CharacterState.AttackCooling] = "idle",
                [CharacterState.CoolDown] = "idle"
            },
            animStates = new Dictionary<CharacterState, AnimTime>()
            {
                [CharacterState.Attack] = new AnimTime(
                    433,
                    () => parameter.stateExchangable = true
                )
                {
                    keyActions = new Dictionary<int, Action>()
                    {
                        [attackDamageTime] = Attack
                    }
                },
                [CharacterState.Hurt] = new AnimTime(
                    300,
                    () => parameter.stateExchangable = true
                )
            }
        };
    }

    protected virtual void InitFSM()
    {
        fsm = new FSM(parameter);
        IReadOnlyDictionary<CharacterState, IFSMState> enemyStates = new Dictionary<CharacterState, IFSMState>()
        {
            [CharacterState.Guard] = new EnemyGuard(),
            [CharacterState.Patrol] = new EnemyPatrol(),
            [CharacterState.Hurt] = new EnemyHurt(),
            [CharacterState.Attack] = new EnemyAttack(),
            [CharacterState.Death] = new EnemyDeath(),
            [CharacterState.Run] = new EnemyMoveToPlayer(),
            [CharacterState.AttackCooling] = new EnemyAttackCooling(),
            [CharacterState.CoolDown] = new EnemyCoolDown()
        };
        fsm.Add(enemyStates);
        fsm.OnStart();
    }

    public virtual void Attack()
    {
        if (IsPlayerInAttackRange())
        {
            Manager.Player.TakeDamage(CharacterInfo.attackDamage0);
        }
    }

    /// <summary>
    /// ºÏ≤‚Player «∑Ò‘⁄π•ª˜∑∂Œß
    /// </summary>
    public virtual bool IsPlayerInAttackRange()
    {
        Character player = CharacterManager.Instance.Player;
        if (player == null)
        {
            return false;
        }

        if (Orientation)
        {
            return player.Position.x > Position.x
                && player.Position.x < Position.x + CharacterInfo.attackRange;
        }
        else
        {
            return player.Position.x < Position.x
                && player.Position.x > Position.x - CharacterInfo.attackRange;
        }
    }

    private void OnDrawGizmos()
    {
        Color color = Gizmos.color;
        // π•ª˜∑∂Œß
        Gizmos.color = Color.red;
        Vector3 position = Position;
        Vector3 attack = position;
        attack.x += (Orientation ? 1 : -1) * CharacterInfo.attackRange;
        Gizmos.DrawSphere(attack, 0.2f);
        // ø… ”æ‡¿Î
        Gizmos.color = Color.green;
        Vector3 see = position;
        see.x += (Orientation ? 1 : -1) * CharacterInfo.seeRange;
        Gizmos.DrawSphere(see, 0.2f);

        // —≤¬ﬂ∑∂Œß
        Gizmos.color = Color.blue;
        Vector3 l = Application.isPlaying ? BornPosition : Position;
        Vector3 r = Application.isPlaying ? BornPosition : Position;
        l.x -= CharacterInfo.patrolRange;
        r.x += CharacterInfo.patrolRange;
        l.y -= 1.0f;
        r.y -= 1.0f;
        Gizmos.DrawLine(l, r);

        Gizmos.color = color;
    }

    public override void TakeDamage(int damage)
    {
    }
}
