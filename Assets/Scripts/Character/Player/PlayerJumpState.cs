using UnityEngine;

public class PlayerJump : EnemyFSMState // 适当增加一个功能，根据按键的时常来进行跳跃高度的改变
{
    private float _lastY;

    public override void OnEnter()
    {
        base.OnEnter();
        _lastY = param.character.Position.y;
    }

    public override void OnExecute()
    {
        Vector3 pos = param.character.Position;
        pos.y += param.character.CharacterInfo.jumpSpeed * Time.deltaTime;
        param.character.Position = pos;

        if (param.character.Position.y < _lastY)
        {
            fsm.Switch(CharacterState.Fall);
        }

        _lastY = param.character.Position.y;
    }

    public override void OnExit()
    {
    }
}

public class PlayerFall : EnemyFSMState
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

public class JumpIdleState : EnemyFSMState
{
    public override void OnExecute()
    {
    }

    public override void OnExit()
    {
    }
}