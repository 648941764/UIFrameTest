﻿using UnityEngine;
using System.Collections.Generic;
using System;

public class Player : Character
{
    private readonly Dictionary<CharacterState, Action> _stateHandlers = new Dictionary<CharacterState, Action>();

    private float dt;
    private float _curJumpSpeed, _lastPosY;
    private bool _inAir;
    private int states;

    private Rigidbody2D rigidbody2D;

    private bool isDead => HasState(CharacterState.Death);

    protected override void Init()
    {
        base.Init();
        rigidbody2D = GetComponent<Rigidbody2D>();
        InitParam();
        GameManager.Instance.TimeUpdateHandle += PlayerUpdate;
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
            }
        };

        _stateHandlers.Add(CharacterState.Idle, IdleHandle);
        _stateHandlers.Add(CharacterState.Run, RunHandle);
        _stateHandlers.Add(CharacterState.Jump, JumpHandle);
        _stateHandlers.Add(CharacterState.Attack, AttackHandle);
        _stateHandlers.Add(CharacterState.Fall, FallHandle);
        _stateHandlers.Add(CharacterState.Death, DeathHandle);
        AddState(CharacterState.Idle);
        _lastPosY = Position.y;
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
            if (parameter.stateExchangable && !HasState(CharacterState.Attack))
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
            if (HasState(state))
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

    private bool HasState(CharacterState state)
    {
        return (states & (1 << (int)state)) > 0;
    }

    private void AddState(CharacterState state)
    {
        if (!HasState(state) && !isDead)
        {
            states |= (1 << (int)state);
            OnAddState(state);
        }
    }

    private bool HasAnyState()
    {
        return states != 0;
    }

    private void DelState(CharacterState state)
    {
        if (HasState(state))
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
                PlayStateAnim(state);
                break;
            case CharacterState.Run:
                if (!HasState(CharacterState.Jump) && !HasState(CharacterState.Fall))
                {
                    PlayStateAnim(state);
                }
                break;
            case CharacterState.Jump:
                PlayStateAnim(state);
                break;
            case CharacterState.Fall:
                PlayStateAnim(state);
                break;
            case CharacterState.Attack:
                DelState(CharacterState.Run);
                parameter.stateExchangable = false;
                parameter.animStates[CharacterState.Attack].Start();
                PlayStateAnim(state);
                break;
            case CharacterState.Hurt:
                break;
            case CharacterState.Death:
                PlayStateAnim(state);
                states = (1 << (int)CharacterState.Death);
                break;
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
                }
                break;
            case CharacterState.Hurt:
                break;
            case CharacterState.Death:
                break;
        }
    }

    private void PlayStateAnim(CharacterState state)
    {
        parameter.animator.Play(parameter.stateClips[state]);
    }

    private void Attack1()
    {
        Debug.Log("Attack 1");
    }

    private void Attack2()
    {
        Debug.Log("Attack 2");
    }
}