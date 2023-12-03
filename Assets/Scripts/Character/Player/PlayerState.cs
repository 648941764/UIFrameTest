using System;

public abstract class CharacterFSMState : IFSMState//具体的动画
{
    protected CharacterParameter param;
    protected FSM fsm;
    public FSM FSM { get => fsm; }

    public void OnInit(FSM fsm)
    {
        this.fsm = fsm;
        param = FSM.GetParameter<CharacterParameter>();
    }

    public abstract void OnEnter();

    public abstract void OnExecute();

    public abstract void OnExit();
}

public class PlayerIdle : CharacterFSMState
{
    public override void OnEnter()
    {
    }

    public override void OnExecute()
    {
    }

    public override void OnExit()
    {
    }
}

public class PlayerRun : CharacterFSMState
{
    public override void OnEnter()
    {
    }

    public override void OnExecute()
    {
    }

    public override void OnExit()
    {
    }
}