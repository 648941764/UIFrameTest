using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerJump;

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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            FSM.Switch(CharacterState.Jump);
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
                [CharacterState.Jump] = new AnimTime(433, new int[] { 433 }, new Action[]
                {
                    () => Debug.Log("ÏÂÂä"),
                }),
                [CharacterState.Fall] = new AnimTime(433, new int[] {432})
            }
        };

        fsm = new FSM(fsmParameter);
        IReadOnlyDictionary<CharacterState, IFSMState> playerStates = new Dictionary<CharacterState, IFSMState>()
        {
            [CharacterState.Idle] = new PlayerIdle(),
            [CharacterState.Run] = new PlayerRun(),
            [CharacterState.Attack] = new PlayerAttack(),
            [CharacterState.Jump] = new PlayerJump(),
            [CharacterState.Fall] = new PlayerFall(),
        };
        fsm.Add(playerStates);
        CharacterManager.Instance.fsmExecute += fsm.OnExecute;
        fsm.OnStart();
    }
}
