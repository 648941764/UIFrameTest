using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTest : MonoBehaviour
{
    public float pressTime;
    public float raiseTime;
    private bool CanJump;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            CanJump = true;
            pressTime = Time.time;
            StartCoroutine(CheckJumpPressHandle());
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            raiseTime = Time.time;
            if (CanJump)
            {
                Log();
            }
        }
    }

    public void Log()
    {
        if (raiseTime - pressTime >=0.5f) 
        {
            Debug.Log("���ĸ���");
        }
        else
        {
            Debug.Log("���ĸ���");
        }
        CanJump = false;
    }

    public IEnumerator CheckJumpPressHandle()
    {

        yield return new WaitForSeconds(0.5f);
        if (CanJump)
        {
            Debug.Log("���ĸ���");
            CanJump = false;
        }
    }
}
