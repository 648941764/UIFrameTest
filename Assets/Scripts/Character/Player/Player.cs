using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    private void Start()
    {
        InitFSM();
    }

    private void Update()
    {
        UpdateState();
    }

    private void UpdateState()
    {
        if (Input.GetMouseButtonDown(0))
        {
            FSM.Switch(CharacterState.Attack);
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            FSM.Switch(CharacterState.Run);
        }
    }

    public override void InitFSM()
    {
        fsmParameter = new CharacterFSMParameter()
        {
            animator = GetComponent<Animator>(),
            character = this,
            defaultStateName = CharacterState.Idle,
            stateClips = new Dictionary<CharacterState, string>() 
            {
                [CharacterState.Idle] = "idle",
                [CharacterState.Run] = "run",
                [CharacterState.Attack] = "attack",
                [CharacterState.Jump] = "jump",
                [CharacterState.Fall] = "fall",
                [CharacterState.Death] = "death",
            },
            animStates = new Dictionary<CharacterState, AnimTime>()
            {
                [CharacterState.Attack] = new AnimTime(433, new int[] { 40, 170 }, new Action[]
                {
                    () => Debug.Log("AAAAAA 40"),
                    () => Debug.Log("AAAAAA 170"),
                }),
            }
        };

        fsm = new FSM(fsmParameter);
        IReadOnlyDictionary<CharacterState, IFSMState> playerStates = new Dictionary<CharacterState, IFSMState>()
        {
            [CharacterState.Idle] = new PlayerIdle(),
            [CharacterState.Run] = new PlayerRun(),
            [CharacterState.Attack] = new PlayerAttack(),
        };
        fsm.Add(playerStates);
        CharacterManager.Instance.fsmExecute += fsm.OnExecute;
        fsm.OnStart();
    }
}
