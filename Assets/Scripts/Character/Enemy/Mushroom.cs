using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


/// <summary>
/// 测试用
/// </summary>
public class Mushroom : Character
{
    private void Start()
    {
        InitFSM();
    }

    public bool Isfollow()
    {
        return true;
    }
    public bool IsAttackRange()
    {
        return true;
    }

    public override void InitFSM()
    {
        fsmParameter = new CharacterFSMParameter()
        {
            animator = GetComponent<Animator>(),
            character = this,
            defaultStateName = CharacterState.Attack,
            stateClips = new Dictionary<CharacterState, string>() 
            {
                [CharacterState.Idle] = "idle",
                [CharacterState.Run] = "run",
                [CharacterState.Attack] = "attack",
                [CharacterState.Hurt] = "jump",
                [CharacterState.Death] = "death",
            },
            animStates = new Dictionary<CharacterState, AnimTime>()
            {
                [CharacterState.Hurt] = new AnimTime(450, new int[] {150}, new Action[]
                {
                    () => Debug.Log($"{150}怪物收到伤害"),//
                }),
                [CharacterState.Attack] = new AnimTime(433, new int[] {230}, new Action[]
                {
                    () => Debug.Log("对玩家造成伤害"),
                }) 
            }
        };

        fsm = new FSM(fsmParameter);
        IReadOnlyDictionary<CharacterState, IFSMState> enemyStates = new Dictionary<CharacterState, IFSMState>()
        {
            [CharacterState.Idle] = new MushroomIdle(),
            [CharacterState.Run] = new MushroomRun(),
            [CharacterState.Hurt] = new MushroomHurt(),
            [CharacterState.Attack] = new MushroomAttack(),
            [CharacterState.Death] = new MushroomDeath(),
        };
        fsm.Add(enemyStates);
        CharacterManager.Instance.fsmExecute += fsm.OnExecute;
        fsm.OnStart();
    }
}
