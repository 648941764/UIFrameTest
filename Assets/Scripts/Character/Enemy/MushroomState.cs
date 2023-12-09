using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomState
{
}

public class MushroomIdle : EnemyFSMState
{
    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnExecute()
    {
    }

    public override void OnExit()
    {
    }
}

public class MushroomRun : EnemyFSMState
{
    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnExecute()
    {
    }

    public override void OnExit()
    {
    }
}

public class MushroomAttack : EnemyFSMState
{

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnExecute()
    {
    }

    public override void OnExit()
    {
    }
}

public class MushroomHurt : EnemyFSMState
{

    public override void OnInit(FSM fsm)
    {
        base.OnInit(fsm);
    }

    public override void OnExecute()
    {
        throw new System.NotImplementedException();
    }

    public override void OnExit()
    {
        throw new System.NotImplementedException();
    }
}

public class MushroomDeath : EnemyFSMState
{
    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnExecute()
    {
        throw new System.NotImplementedException();
    }

    public override void OnExit()
    {
        throw new System.NotImplementedException();
    }
}
