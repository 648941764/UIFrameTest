using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Awake()
    {
    }

    private void Start()
    {
    }

    private void OnEnable()
    {
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            EventManager.Instance.Broadcast(new EventParam() { eventName = EventType.Example });
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            EventManager.Instance.Broadcast(new EventParam() { eventName = EventType.Example2 });
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (UIManager.Instance.IsOpen<TestForm>())
            {
                UIManager.Instance.Close<TestForm>();
            }
            else
            {
                UIManager.Instance.Open<TestForm>();
            }
        }
    }
}
