using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomState
{
}

public class MushroomIdle : CharacterFSMState
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

public class MushroomRun : CharacterFSMState
{
    public override void OnExecute()
    {
        throw new System.NotImplementedException();
    }

    public override void OnExit()
    {
        throw new System.NotImplementedException();
    }
}

public class MushroomAttack : CharacterFSMState
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

public class MushroomHurt : CharacterFSMState
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

public class MushroomDeath : CharacterFSMState
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
