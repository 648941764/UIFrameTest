
using UnityEngine;

public class AnimalPartol : EnemyFSMState
{

    public override void OnEnter()
    {
        base.OnEnter();
        param.patrolPosX = BornPosition.x + Random.Range(-CharacterInfo.patrolRange, CharacterInfo.patrolRange);
        param.character.Orientation = param.patrolPosX >= param.character.Position.x;
        Debug.Log($"<color=green>角色目标点{param.patrolPosX}</color>");
    }

    public override void OnExecute()
    {
        Move();

        if (Mathf.Abs(param.character.Position.x - param.patrolPosX) < 0.1f)
        {
            fsm.Switch(CharacterState.Guard);
        }
    }

    public override void OnExit()
    {
    }
}

public class AnimalAttack : EnemyFSMState
{
    AnimTime animTime;

    public override void OnInit(FSM fsm)
    {
        base.OnInit(fsm);
        TryGetAnimTime(CharacterState.Attack, out animTime);
    }

    public override void OnEnter()
    {
        base.OnEnter();
        animTime.Start();
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

public class AnimalMoveToPlayer : EnemyFSMState
{
    public override void OnEnter()
    {
        base.OnEnter(); 
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
        throw new System.NotImplementedException();
    }
}

public class AnimalHurt : EnemyFSMState
{
    AnimTime animTime;
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

public class AnimalGuard : EnemyFSMState
{
    public override void OnEnter()
    {
        base.OnEnter();
        param.timer = 0f;
    }

    public override void OnExecute()
    {
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

public class AnimalAttackCooling : EnemyFSMState
{
    public override void OnEnter()
    {
        base.OnEnter();
        param.timer = 0f;
    }

    public override void OnExecute()
    {
        if (param.timer >= CharacterInfo.attackRange)
        {
            fsm.Switch(CharacterState.Run);
        }
    }

    public override void OnExit()
    {
        throw new System.NotImplementedException();
    }
}

public class AnimalDeath : EnemyFSMState
{
    public override void OnExecute()
    {
    }

    public override void OnExit()
    {
    }
}

