using UnityEngine;
using System.Collections.Generic;
using System;

public class Player : Character
{
    public static int[] ignoreStates = new int[] { (int)CharacterState.AttackCooling };

    private readonly Dictionary<CharacterState, Action> _stateHandlers = new Dictionary<CharacterState, Action>();

    private float dt;
    private float _curJumpSpeed;
    private int states;

    private new Rigidbody2D rigidbody2D;

    private bool isDead => HasState(CharacterState.Death);

    public override void Init(int uid)
    {
        base.Init(uid);
        Manager.SetPlayer(this);
        rigidbody2D = GetComponent<Rigidbody2D>();
        InitParam();
        GameManager.Instance.TimeUpdateHandle += PlayerUpdate;
        CameraController.Instance.player = transform;
        CameraController.Instance.CalculateMoveRange();
        CameraController.Instance.FocusPlayer();
    }

    private void InitParam()
    {
        parameter = new CharacterParameter()
        {
            animator = GetComponent<Animator>(),
            character = this,
            defaultStateName = CharacterState.Idle,
            stateClips = new Dictionary<CharacterState, string>()
            {
                [CharacterState.Idle] = "idle",
                [CharacterState.Run] = "run",
                [CharacterState.Attack] = "attack",
                [CharacterState.Death] = "death",
                [CharacterState.Jump] = "jump",
                [CharacterState.Fall] = "fall",
                [CharacterState.Hurt] = "hurt"
            },
            animStates = new Dictionary<CharacterState, AnimTime>()
            {
                [CharacterState.Attack] = new AnimTime(
                    433, 
                    () => 
                    {
                        parameter.stateExchangable = true;
                        DelState(CharacterState.Attack); 
                    }
                )
                {
                    keyActions = new Dictionary<int, Action>()
                    {
                        [86] = Attack1,
                        [295] = Attack2,
                    }
                },

                [CharacterState.Hurt] = new AnimTime(
                    433,
                    () =>
                    {
                        DelState(CharacterState.Hurt);
                        parameter.stateExchangable = true;
                    }
                )
                {
                    keyActions = new Dictionary<int, Action>()
                    {
                        [86] = () => { Debug.Log("玩家受伤"); }
                    }
                },

                [CharacterState.Death] = new AnimTime(
                    417,
                    () =>
                    {
                        DelState(CharacterState.Hurt);
                        //parameter.stateExchangable = true;
                        this.gameObject.SetActive(false);
                        UIManager.Instance.Close<HealthForm>();
                        CharacterManager.Instance.ClearEnemies();


                        UIManager.Instance.Open<GameOverForm>();
                    }
                )
                {
                    keyActions = new Dictionary<int, Action>()
                    {
                        [417] = () => { Debug.Log("玩家死亡"); }
                    }
                },
            }
        };

        _stateHandlers.Add(CharacterState.Idle, IdleHandle);
        _stateHandlers.Add(CharacterState.Hurt, HurtHandle);
        _stateHandlers.Add(CharacterState.Run, RunHandle);
        _stateHandlers.Add(CharacterState.Jump, JumpHandle);
        _stateHandlers.Add(CharacterState.Attack, AttackHandle);
        _stateHandlers.Add(CharacterState.AttackCooling, AttackCoolingHandle);
        _stateHandlers.Add(CharacterState.Fall, FallHandle);
        _stateHandlers.Add(CharacterState.Death, DeathHandle);
        AddState(CharacterState.Idle);
    }

    private void PlayerUpdate(float dt)
    {
        this.dt = dt;

        // 移动
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            if (parameter.stateExchangable && !HasState(CharacterState.Run))
            {
                AddState(CharacterState.Run);
            }
        }

        // 攻击
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (parameter.stateExchangable && !HasState(CharacterState.Attack) && !HasState(CharacterState.AttackCooling, false))
            {
                AddState(CharacterState.Attack);
            }
        }

        // 跳跃
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (parameter.stateExchangable && !HasState(CharacterState.Jump) && !HasState(CharacterState.Fall))
            {
                _curJumpSpeed = CharacterInfo.jumpSpeed;
                AddState(CharacterState.Jump);
                PlayStateAnim(CharacterState.Jump);
            }
        }

        foreach (CharacterState state in _stateHandlers.Keys)
        {
            if (HasState(state, false))
            {
                _stateHandlers[state]();
            }
        }
    }

    private void IdleHandle()
    {

    }

    private void RunHandle()
    {
        float axisX = Input.GetAxisRaw("Horizontal");
        if (axisX == 0f)
        {
            DelState(CharacterState.Run);
            return;
        }
        Vector3 pos = Position;
        pos.x += CharacterInfo.moveSpeed * axisX * dt;
        Position = pos;
        Orientation = axisX > 0f;
        CameraController.Instance.FocusPlayer();
    }
    
    private void JumpHandle()
    {
        if (_curJumpSpeed > 0.0f)
        {
            Vector3 pos = Position;
            pos.y += _curJumpSpeed * dt;
            Position = pos;
            _curJumpSpeed -= 9.8f * dt;
        }
        else
        {
            DelState(CharacterState.Jump);
            AddState(CharacterState.Fall);
        }
    }

    private void AttackHandle()
    {
        AnimTime animTime = parameter.animStates[CharacterState.Attack];
        bool complete = true;
        foreach (int key in animTime.keyActions.Keys)
        {
            // 第一段攻击结束后可以切换动作
            complete &= animTime.Tick > key;
            break;
        }
        if (complete)
        {
            parameter.stateExchangable = complete;
        }
    }

    private void AttackCoolingHandle()
    {
        parameter.attckCoolTimer += dt;
        if (parameter.attckCoolTimer >= CharacterInfo.attackInterval)
        {
            DelState(CharacterState.AttackCooling, false);
        }
    }

    private void HurtHandle()
    {

    }

    private void FallHandle()
    {
        if (rigidbody2D.velocity.y == 0f)
        {
            DelState(CharacterState.Fall);
        }
    }

    private void DeathHandle()
    {
    }

    private bool HasState(CharacterState state, bool igonre = true)
    {
        int stateCopy = igonre ? StatesCopy() : states;
        return (stateCopy & (1 << (int)state)) > 0;
    }

    private void AddState(CharacterState state, bool igonre = true)
    {
        if (!HasState(state, igonre) && !isDead)
        {
            states |= (1 << (int)state);
            OnAddState(state);
        }
    }

    private int StatesCopy()
    {
        int i = -1;
        int statesCopy = states;
        while (++i < ignoreStates.Length)
        {
            statesCopy &= ~(1 << ignoreStates[i]);
        }
        return statesCopy;
    }

    private bool HasAnyState()
    {
        return StatesCopy() != 0;
    }

    private void DelState(CharacterState state, bool igonre = true)
    {
        if (HasState(state, igonre))
        {
            states &= ~(1 << (int)state);
            OnDelState(state);
        }
    }

    private void OnAddState(CharacterState state)
    {
        if (state != CharacterState.Idle && HasState(CharacterState.Idle))
        {
            DelState(CharacterState.Idle);
        }

        if (state != CharacterState.Attack && HasState(CharacterState.Attack) && parameter.stateExchangable)
        {
            DelState(CharacterState.Attack);
        }

        switch (state)
        {
            case CharacterState.Idle:
                {
                    PlayStateAnim(state);
                    break;
                }
            case CharacterState.Run:
                {
                    if (!HasState(CharacterState.Jump) && !HasState(CharacterState.Fall))
                    {
                        PlayStateAnim(state);
                    }
                    break;
                }
            case CharacterState.Jump:
                {
                    PlayStateAnim(state);
                    break;
                }
            case CharacterState.Fall:
                {
                    PlayStateAnim(state);
                    break;
                }
            case CharacterState.Attack:
                {
                    DelState(CharacterState.Run);
                    parameter.stateExchangable = false;
                    parameter.animStates[CharacterState.Attack].Start();
                    PlayStateAnim(state);
                    break;
                }
            case CharacterState.AttackCooling:
                {
                    parameter.attckCoolTimer = 0f;
                    break;
                }
            case CharacterState.Hurt:
                {
                    parameter.stateExchangable = false;
                    parameter.animStates[CharacterState.Hurt].Start();
                    PlayStateAnim(state);
                    break;
                }
            case CharacterState.Death:
                {
                    parameter.stateExchangable = false;
                    parameter.animStates[CharacterState.Death].Start();
                    PlayStateAnim(state);
                    states = (1 << (int)CharacterState.Death);
                    break;
                }
            }
        }

    private void OnDelState(CharacterState state)
    {
        if (!HasAnyState())
        {
            AddState(CharacterState.Idle);
        }

        switch (state)
        {
            case CharacterState.Idle:
                break;
            case CharacterState.Run:
                break;
            case CharacterState.Jump:
                break;
            case CharacterState.Fall:
                {
                    if (HasState(CharacterState.Run))
                    {
                        PlayStateAnim(CharacterState.Run);
                    }
                    break;
                }
            case CharacterState.Attack:
                {
                    parameter.animStates[CharacterState.Attack].Break();
                    if (rigidbody2D.velocity.y != 0f)
                    {
                        PlayStateAnim(CharacterState.Jump);
                    }
                    AddState(CharacterState.AttackCooling, false);
                }
                break;
            case CharacterState.Hurt:
                {
                    parameter.animStates[CharacterState.Hurt].Break();
                }
                break;
            case CharacterState.Death:
                {
                    parameter.animStates[CharacterState.Hurt].Break();
                }
                break;
        }
    }

    private void PlayStateAnim(CharacterState state)
    {
        parameter.animator.Play(parameter.stateClips[state]);
    }

    private void Attack1()
    {
        AttackApply(CharacterInfo.attackDamage0);
        Debug.Log("Attack 1");
    }

    private void Attack2()
    {
        AttackApply(CharacterInfo.attackDamage1);
        Debug.Log("Attack 2");
    }

    private void OnDrawGizmos()
    {
        Color color = Gizmos.color;
        // 攻击范围
        Gizmos.color = Color.red;
        Vector3 position = Position;
        Vector3 attack = position;
        attack.x += CharacterInfo.attackRange;
        attack.y += 0.6f;
        Gizmos.DrawSphere(attack, 0.2f);

        Gizmos.color = color;
    }

    public override void TakeDamage(int damage)
    {
        CharacterEntity player = CharacterManager.Instance.PlayerEntity;
        int actualDamage = Mathf.Max(damage - player.GetDefence(), 1);
        player.ChangeHealth(-actualDamage);
        if (!HasState(CharacterState.Hurt))
        {
            if (HasState(CharacterState.Run))
            {
                DelState(CharacterState.Run);
            }
            AddState(CharacterState.Hurt);
        }
    }

    public void OnPlayerDeath()
    {
        if (!HasState(CharacterState.Death))
        {
            if (HasState(CharacterState.Run))
            {
                DelState(CharacterState.Run);
            }
            AddState(CharacterState.Death);
        }
    } 

    public void AttackApply(int attackDamage)
    {
        //首先判断角色方向，再找到角色攻击范围内是否
        List<Enemy> enemies = CharacterManager.Instance.FindInAttackRangeEnemies();
        foreach (var enemy in enemies)
        {
            CharacterEntity enemyEntity = CharacterManager.Instance.GetEnemyEntity(enemy.UID);
            int damage = Mathf.Max(attackDamage - enemyEntity.GetDefence(), 1);
            enemyEntity.ChangeHealth(-damage);
            
            enemy.FSM.Switch(CharacterState.Hurt);
        }
    }
}