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

public class PlayerJump : CharacterFSMState
{
    Timer timer;

    public override void OnInit(FSM fsm)
    {
        base.OnInit(fsm);
        TryGetAnimTime(CharacterState.Jump, out AnimTime animTime);

        timer = new Timer()
        {
            time = animTime.time,
            onTick = OnTimerTick,
            onComplete = () => FSM.Switch(CharacterState.Fall)
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
        Vector3 pos = param.character.Position;
        pos.y += param.character.CharacterInfo.jumpSpeed * Time.deltaTime;
        param.character.Position = pos;

        float horizontalInput = Input.GetAxis("Horizontal");
        if (horizontalInput == 0f)
        {
            FSM.Switch(CharacterState.Fall);
            return;
        }
        Vector3 playerPos = param.character.Position;
        pos.x += horizontalInput * param.character.CharacterInfo.moveSpeed * Time.deltaTime;
        param.character.Position = pos;
        CameraController.Instance.FocusPlayer();
        param.character.Orientation = horizontalInput > 0f;
    }

    public override void OnExit()
    {
    }

    private void OnTimerTick(int tick)
    {
        TryGetAnimTime(CharacterState.Jump, out AnimTime animTime);
        if (tick > animTime.keys[animTime.keyCount - 1])//计算当前帧率是否超过事件的最后一个帧率，如果超过，那么就让他可以改变动作
        {
            param.stateExchangable = true;
            return;
        }
        int i = -1;
        while (++i < animTime.keyCount)//遍历所有的关键帧率，找到关键帧然后执行里面的方法
        {
            if (animTime.keys[i] == tick)
            {
                animTime.actions[i]?.Invoke();
                Debug.Log($"key frame:{tick}");
            }
        }
    }
}

public class PlayerFall : CharacterFSMState
{
    public override void OnInit(FSM fsm)
    {
        base.OnInit(fsm);
    }

    public override void OnEnter()
    {
        base.OnEnter();
        param.stateExchangable = false;
    }

    public override void OnExecute()
    {
    }

    public override void OnExit()
    {
    }
}

public class PlayerDeath : CharacterFSMState
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