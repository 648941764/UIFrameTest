using UnityEngine;

public class EnemyGuard : EnemyFSMState
{
    public override void OnEnter()
    {
        base.OnEnter();
        param.timer = 0f;
    }

    public override void OnExecute()
    {
        if (SeePlayer())
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
        Enemy enemy = param.character as Enemy;
        float range = 0f;
        while (Mathf.Abs(range) < 1.5f)
        {
            range = Random.Range(-enemy.CharacterInfo.patrolRange, enemy.CharacterInfo.patrolRange);
        }
        param.patrolPosX = enemy.BornPosition.x + range;
        Debug.Log($"巡逻目标点：{param.patrolPosX}");
        param.character.Orientation = param.patrolPosX >= param.character.Position.x;
    }

    public override void OnExecute()
    {
        if (SeePlayer())
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
    public override void OnExecute()
    {
        fsm.Switch(CharacterState.Guard);    
    }

    public override void OnExit()
    {
        
    }
}

public class EnemyHurt : EnemyFSMState
{
    public override void OnEnter()
    {
        base.OnEnter();
        param.stateExchangable = false;
        FacePlayer();
    }

    public override void OnExecute()
    {
        if (SeePlayer())
        {
            fsm.Switch(CharacterState.Run);
        }
        else
        {
            fsm.Switch(CharacterState.Guard);
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
        param.character.Orientation = CharacterManager.Instance.Player.Position.x > param.character.Position.x;
    }

    public override void OnExecute()
    {
        Move();
        if (Mathf.Abs(param.character.Position.x - CharacterManager.Instance.Player.Position.x) < param.character.CharacterInfo.attackRange)
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
    public override void OnExecute()
    {
    }

    public override void OnExit()
    {
    }
}
