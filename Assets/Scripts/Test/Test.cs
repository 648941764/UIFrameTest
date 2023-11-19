using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Awake()
    {
        DataManager.Instance.LoadDatas();
        foreach (var item in DataManager.Instance.TestDatas.Values)
        {
            Debug.Log(item.id + ", " + item.pos);
        }
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
            EventManager.Instance.Broadcast(new EventParam() { eventName = EventType.BackPackAddItem });
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (UIManager.Instance.IsOpen<BackpackForm>())
            {
                UIManager.Instance.Close<BackpackForm>();
            }
            else
            {
                UIManager.Instance.Open<BackpackForm>();
            }
        }
    }
}
