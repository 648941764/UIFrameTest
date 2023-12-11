using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOld : Character
{
    private FSM jumpFSM;
    private CharacterParameter jumpFSMParam;

    private void Update()
    {
        UpdateState();
    }

    public void UpdateJumpState()
    {
        CharacterState jumpState = jumpFSM.GetStateName<CharacterState>();
        switch (jumpState)
        {
            case CharacterState.Jump:
            case CharacterState.Fall:
                {
                    jumpFSMParam.animator.Play(jumpFSMParam.stateClips[jumpState]);
                    break;
                }
        }
    }

    private void UpdateState()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    FSM.Switch(CharacterState.Attack);
        //}

        //if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        //{
        //    FSM.Switch(CharacterState.Run);
        //}

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    if (FSMParameter.stateExchangable)
        //    {
        //        jumpFSM.Switch(CharacterState.Jump);
        //    }
        //}
    }

    public void InitFSM()
    {
        //fsmParameter = new CharacterFSMParameter()
        //{
        //    animator = GetComponent<Animator>(),
        //    character = this,
        //    defaultStateName = CharacterState.Idle,
        //    stateClips = new Dictionary<CharacterState, string>() 
        //    {
        //        [CharacterState.Idle] = "idle",
        //        [CharacterState.Run] = "run",
        //        [CharacterState.Attack] = "attack",
        //        [CharacterState.Death] = "death",
        //    },
        //    animStates = new Dictionary<CharacterState, AnimTime>()
        //    {
        //        [CharacterState.Attack] = new AnimTime(433, new int[] { 40, 170 }, new Action[]
        //        {
        //            () => Debug.Log("AAAAAA 40"),
        //            () => Debug.Log("AAAAAA 170"),
        //        }),
        //    }
        //};

        //fsm = new FSM(fsmParameter);
        //IReadOnlyDictionary<CharacterState, IFSMState> playerStates = new Dictionary<CharacterState, IFSMState>()
        //{
        //    [CharacterState.Idle] = new PlayerIdle(),
        //    [CharacterState.Run] = new PlayerRun(),
        //    [CharacterState.Attack] = new PlayerAttack(),
        //    [CharacterState.Death] = new PlayerDeath(),
        //};
        //fsm.Add(playerStates);

        //    jumpFSMParam = new CharacterFSMParameter()
        //    {
        //        animator = GetComponent<Animator>(),
        //        character = this,
        //        defaultStateName = CharacterState.JumpIdle,
        //        stateClips = new Dictionary<CharacterState, string>()
        //        {
        //            [CharacterState.JumpIdle] = "",
        //            [CharacterState.Jump] = "jump",
        //            [CharacterState.Fall] = "fall",
        //        },
        //    };

        //    IReadOnlyDictionary<CharacterState, IFSMState> playerJumpStates = new Dictionary<CharacterState, IFSMState>()
        //    {
        //        [CharacterState.JumpIdle] = new JumpIdleState(),
        //        [CharacterState.Jump] = new PlayerJump(),
        //        [CharacterState.Fall] = new PlayerFall(),
        //    };
        //    jumpFSM = new FSM(jumpFSMParam);
        //    jumpFSM.Add(playerJumpStates);

        //    jumpFSM.OnStart();
        //    fsm.OnStart();
        //}

        //public override void ExitFSM()
        //{
        //    base.ExitFSM();
        //    jumpFSM.OnExit();
    }

    public override void TakeDamage(int damage)
    {
        throw new NotImplementedException();
    }
}
