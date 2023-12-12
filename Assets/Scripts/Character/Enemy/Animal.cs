using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : Enemy
{
    protected override void InitFSMParameter()
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
            },
            animStates = new Dictionary<CharacterState, AnimTime>()
            {
                [CharacterState.Attack] = new AnimTime(
                    450,
                    () => parameter.stateExchangable = true
                )
                {
                    keyActions = new Dictionary<int, Action>()
                    {
                        [attackDamageTime] = Attack
                    }
                },
                [CharacterState.Hurt] = new AnimTime(
                    433,
                    () => parameter.stateExchangable = true
                )
            }
        };
    }

    protected override void InitFSM()
    {
        fsm = new FSM(parameter);
        IReadOnlyDictionary<CharacterState, IFSMState> enemyStates = new Dictionary<CharacterState, IFSMState>()
        {
            [CharacterState.Guard] = new AnimalGuard(),
            [CharacterState.Patrol] = new AnimalPartol(),
            [CharacterState.Hurt] = new AnimalHurt(),
            [CharacterState.Attack] = new AnimalHurt(),
            [CharacterState.Death] = new AnimalDeath(),
            [CharacterState.Run] = new AnimalMoveToPlayer(),
            [CharacterState.AttackCooling] = new AnimalAttackCooling(),
        };
        fsm.Add(enemyStates);
        fsm.OnStart();
    }
}
