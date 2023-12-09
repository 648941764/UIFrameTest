using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


/// <summary>
/// 测试用
/// </summary>
public class Enemy : Character
{
    public bool isPatrol;//判断当前怪物是否具备巡逻功能
    private float _rightLimit;
    private float _lefttLimit;

    protected FSM fsm;

    public FSM FSM => fsm;


    private void Awake()
    {
        _rightLimit = transform.position.x + 4f;
        _lefttLimit = transform.position.x - 4f;
    }

    public void Init(int uid)
    {
        InitFSM();
    }

    public void ExitFSM()
    {
        fsm.OnExit();
    }

    public virtual void InitFSM()
    {
        //parameter = new CharacterParameter()
        //{
        //    animator = GetComponent<Animator>(),
        //    character = this,
        //    defaultStateName = CharacterState.Attack,
        //    stateClips = new Dictionary<CharacterState, string>()
        //    {
        //        [CharacterState.Idle] = "idle",
        //        [CharacterState.Run] = "run",
        //        [CharacterState.Attack] = "attack",
        //        [CharacterState.Hurt] = "hurt",
        //        [CharacterState.Death] = "death",
        //    },
        //    animStates = new Dictionary<CharacterState, AnimTime>()
        //    {
        //        [CharacterState.Hurt] = new AnimTime(450, new int[] { 150 }, new Action[]
        //        {
        //            () => Debug.Log($"{150}怪物收到伤害"),//
        //        }),
        //        //[CharacterState.Attack] = new AnimTime(433, new int[] {230}, new Action[]
        //        //{
        //        //    () => Debug.Log("对玩家造成伤害"),
        //        //}) 
        //    }
        //};

        fsm = new FSM(parameter);
        IReadOnlyDictionary<CharacterState, IFSMState> enemyStates = new Dictionary<CharacterState, IFSMState>()
        {
            [CharacterState.Idle] = new MushroomIdle(),
            [CharacterState.Run] = new MushroomRun(),
            [CharacterState.Hurt] = new MushroomHurt(),
            [CharacterState.Attack] = new MushroomAttack(),
            [CharacterState.Death] = new MushroomDeath(),
        };
        fsm.Add(enemyStates);
        fsm.OnStart();
    }

    /// <summary>
    /// 让角色移动的方法
    /// </summary>
    public void Patrol()
    {
        fsm.Switch(CharacterState.Run);

        if (transform.position.x < _lefttLimit)
        {
            Orientation = true;
        }
        else if (transform.position.x > _rightLimit)
        {
            Orientation = false;
        }
        transform.Translate(Vector3.right * CharacterInfo.moveSpeed * Time.deltaTime);
    }
}
