
using System.Diagnostics.Contracts;
using UnityEngine;

public class EnemyGuard : EnemyFSMState
{
    public override void OnEnter()
    {
        base.OnEnter();
        param.timer = 0f;
        //Debug.Log("EnemyFSM： EnemyGuard");
    }

    public override void OnExecute()
    {
        if (IsPlayerInSight())
        {
            fsm.Switch(CharacterState.Run);
            return;
        }

        param.timer += Time.deltaTime;
        if (param.timer > param.character.CharacterInfo.guardTime)
        {
            fsm.Switch(CharacterState.Patrol);
        }
    }

    public override void OnExit()
    {

    }
}

public class EnemyPatrol : EnemyFSMState
{
    public override void OnEnter()
    {
        base.OnEnter();
        param.patrolPosX = BornPosition.x + Random.Range(-CharacterInfo.patrolRange, CharacterInfo.patrolRange);
        //Debug.Log($"巡逻目标点：{param.patrolPosX}");
        param.character.Orientation = param.patrolPosX >= param.character.Position.x;
        //Debug.Log("EnemyFSM： EnemyPatrol");
    }

    public override void OnExecute()
    {
        if (IsPlayerInSight())
        {
            fsm.Switch(CharacterState.Run);
            return;
        }

        Move();

        if (Mathf.Abs(param.character.Position.x - param.patrolPosX) < 0.1f)
        {
            // 到达巡逻点切换状态
            fsm.Switch(CharacterState.Guard);
        }
    }

    public override void OnExit()
    {
    }
}

public class EnemyAttack : EnemyFSMState
{
    AnimTime animTime;
    public override void OnInit(FSM fsm)
    {
        base.OnInit(fsm);
        TryGetAnimTime(CharacterState.Attack, out animTime);
        Debug.Log("EnemyFSM： EnemyAttack");
    }

    public override void OnEnter()
    {
        base.OnEnter();
        animTime.Start();
        Debug.Log("EnemyFSM： EnemyAttack");
    }

    public override void OnExecute()
    {
        if (!animTime.Ticking)
        {
            fsm.Switch(CharacterState.AttackCooling);
        }
    }

    public override void OnExit()
    {
    }
}

public class EnemyHurt : EnemyFSMState
{
    AnimTime animTime;

    public override void OnInit(FSM fsm)
    {
        base.OnInit(fsm);
        TryGetAnimTime(CharacterState.Hurt, out animTime);
        //Debug.Log("EnemyFSM： EnemyHurt");
    }

    public override void OnEnter()
    {
        base.OnEnter();
        param.stateExchangable = false;
        animTime.Start();
        FacePlayer();
    }

    public override void OnExecute()
    {
        if (IsPlayerInPatrolRange())
        {
            FacePlayer();
            fsm.Switch(CharacterState.Run);
        }
    }

    public override void OnExit()
    {
    }
}

public class EnemyMoveToPlayer : EnemyFSMState
{
    public override void OnEnter()
    {
        base.OnEnter();
        //Debug.Log("EnemyFSM： EnemyMoveToPlayer");
    }

    public override void OnExecute()
    {
        FacePlayer();
        if (IsPlayerInPatrolRange())
        {
            Move();
        }
        else
        {
            fsm.Switch(CharacterState.Patrol);
        }
        if ((param.character as Enemy).IsPlayerInAttackRange())
        {
            fsm.Switch(CharacterState.Attack);
        }
    }

    public override void OnExit()
    {
    }
}

public class EnemyDeath : EnemyFSMState
{
    public override void OnEnter()
    {
        base.OnEnter();
        param.animStates[CharacterState.Death].Start();
    }

    public override void OnExecute()
    {
    }

    public override void OnExit()
    {
    }
}

public class EnemyAttackCooling : EnemyFSMState
{

    public override void OnEnter()
    {
        base.OnEnter();
        param.timer = 0;
    }

    public override void OnExecute()
    {
        param.timer += Time.deltaTime;
        if (param.timer > CharacterInfo.attackInterval) 
        {
            fsm.Switch(CharacterState.Run);
        }
    }

    public override void OnExit()
    {

    }
}

public class EnemyCoolDown : EnemyFSMState
{

    public override void OnExecute()
    {
        param.timer = 0f;
    }

    public override void OnExit()
    {
        param.timer += Time.deltaTime;
        if (param.timer > CharacterInfo.attackInterval)
        {
            fsm.Switch(CharacterState.Run);
        }
    }
}
