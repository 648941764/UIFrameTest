using System;

public interface IFSMState
{
    FSM FSM { get; }
    void OnInit(FSM fsm);
    void OnEnter();
    void OnExecute();
    void OnExit();
}

public abstract class FSMParameter
{
    public Enum defaultStateName;
    public bool running;
}