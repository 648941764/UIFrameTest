using UnityEngine;

public class PlayerIdle : CharacterFSMState
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

public class PlayerRun : CharacterFSMState
{
    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnExecute()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        if (horizontalInput == 0f)
        {
            FSM.Switch(CharacterState.Idle);
            return;
        }
        Vector3 pos = param.character.Position;
        pos.x += horizontalInput * param.character.CharacterInfo.moveSpeed * Time.deltaTime;
        param.character.Position = pos;
        CameraController.Instance.FocusPlayer();
        param.character.Orientation = horizontalInput > 0f;
    }

    public override void OnExit()
    {
    }
}

public class PlayerAttack : CharacterFSMState
{
    private Timer timer;

    public override void OnInit(FSM fsm)
    {
        base.OnInit(fsm);

        TryGetAnimTime(CharacterState.Attack, out AnimTime animTime);

        timer = new Timer()
        {
            time = animTime.time,
            onTick = OnTimerTick,
            onComplete = () => FSM.Switch(CharacterState.Idle)
        };
    }

    public override void OnEnter()
    {
        base.OnEnter();
        param.stateExchangable = false;
        timer.Restart();
    }

    public override void OnExecute()
    {
    }

    public override void OnExit()
    {
    }

    private void OnTimerTick(int tick)
    {
        TryGetAnimTime(CharacterState.Attack, out AnimTime animTime);
        if (tick > animTime.keys[animTime.keyCount - 1])
        {
            param.stateExchangable = true;
            return;
        }
        int i = -1;
        while (++i < animTime.keyCount)
        {
            if (animTime.keys[i] == tick)
            {
                animTime.actions[i]?.Invoke();
                Debug.Log($"key frame:{tick}");
            }
        }
    }
}