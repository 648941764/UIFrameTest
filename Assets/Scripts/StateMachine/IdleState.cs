using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : ICharacterState
{
    public void OnEnter()
    {
        Debug.Log("角色进入待机状态");
    }

    public void OnUpdate()
    {
        Debug.Log("角色正在移动");
    }

    public void OnExit()
    {
        Debug.Log("角色退出移动动画");
    }

}
