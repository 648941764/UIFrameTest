using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : SingletonMono<Test> 
{
    public Backpack Backpack { get; private set; }
    public PlayerOld Player { get; private set; }



    protected override void OnAwake()
    {
        base.OnAwake();
        DataManager.Instance.LoadDatas();
    }

    private void Start()
    {
        Backpack = new Backpack();
        //Player = new Player();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            EventManager.Instance.Broadcast(new EventParam() { eventName = EventType.Example });
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            Backpack.AddItem(Random.Range(3001, 3006), Random.Range(1,4));
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            Backpack.UseItem(Backpack.Items[1], 1);
            EventManager.Instance.Broadcast(new EventParam() { eventName = EventType.BackpackItemChange });
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
