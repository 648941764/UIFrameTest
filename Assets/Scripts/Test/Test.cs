using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : SingletonMono<Test> 
{
    public Backpack Backpack { get; private set; }

    protected override void OnAwake()
    {
        base.OnAwake();
        DataManager.Instance.LoadDatas();
    }

    private void Start()
    {
        Backpack = new Backpack();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            EventManager.Instance.Broadcast(new EventParam() { eventName = EventType.Example });
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            Backpack.AddItem(1, 5);
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
