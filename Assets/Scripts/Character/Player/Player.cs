using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    private void Update()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            FSM.Switch(CharacterState.Run);
        }
        else
        {
            FSM.Switch(CharacterState.Idle);
        }
    }

    public override void InitFSM()
    {
        CharacterParameter param = new CharacterParameter()
        {
            animator = GetComponent<Animator>(),
            character = this,
            defaultStateName = CharacterState.Idle,
            stateClips = new Dictionary<CharacterState, string>() 
            {
                [CharacterState.Idle] = "idle",
                [CharacterState.Idle] = "run",
                [CharacterState.Idle] = "attack",
                [CharacterState.Idle] = "hurt",
                [CharacterState.Idle] = "jump",
                [CharacterState.Idle] = "fall",
                [CharacterState.Idle] = "death",
            },
            animStates = new Dictionary<CharacterState, AnimationTime>()
            {
                [CharacterState.Attack] = new AnimationTime(250, new int[] { 40, 170 }),
            }
        };
        fsm = new FSM(param);
        IReadOnlyDictionary<CharacterState, IFSMState> playerStates = new Dictionary<CharacterState, IFSMState>()
        {
            [CharacterState.Idle] = new PlayerIdle(),
            [CharacterState.Run] = new PlayerRun(),
        };
        fsm.Add(playerStates);
    }
}
