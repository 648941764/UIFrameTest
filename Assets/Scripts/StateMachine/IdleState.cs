using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : ICharacterState
{
    public void OnEnter()
    {
        Debug.Log("��ɫ�������״̬");
    }

    public void OnUpdate()
    {
        Debug.Log("��ɫ�����ƶ�");
    }

    public void OnExit()
    {
        Debug.Log("��ɫ�˳��ƶ�����");
    }

}
