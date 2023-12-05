using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


/// <summary>
/// ������
/// </summary>
public class Mushroom : Character
{
    public bool isPatrol;//�жϵ�ǰ�����Ƿ�߱�Ѳ�߹���

    private void Start()
    {
        InitFSM();
    }

    /// <summary>
    /// Ѳ�ߵķ������ڵ��˵����߷ֱ�����һ�����õ��������ƶ�
    /// </summary>
    public void PatrolMethod()
    {
        if (!isPatrol)
        {
            return;
        }
        
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
                    () => Debug.Log($"{150}�����յ��˺�"),//
                }),
                [CharacterState.Attack] = new AnimTime(433, new int[] {230}, new Action[]
                {
                    () => Debug.Log("���������˺�"),
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
