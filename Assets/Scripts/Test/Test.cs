using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : SingletonMono<Test> 
{
    public Backpack Backpack { get; private set; }
    public PlayerOld Player { get; private set; }

    public GameBackpack GameBackpack { get; private set; }


    protected override void OnAwake()
    {
        base.OnAwake();
        DataManager.Instance.LoadDatas();
    }

    private void Start()
    {
        //Backpack = new Backpack();
        //Player = new Player();
        GameBackpack = new GameBackpack();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            EventManager.Instance.Broadcast(EventParam.Get(EventType.Example));
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            //Backpack.AddItem(Random.Range(3001, 3006), Random.Range(1,4));
            if (UIManager.Instance.IsOpen<PrepareForm>())
            {
                UIManager.Instance.Close<PrepareForm>();
            }
            else
            {
                UIManager.Instance.Open<PrepareForm>();
            }

        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            //Backpack.UseItem(Backpack.Items[1], 1);
            //EventManager.Instance.Broadcast(EventParam.Get(EventType.BackpackItemChange));
            GameBackpack.Additem(3001, 4);


        }

        //if (Input.GetKeyDown(KeyCode.C))
        //{
        //    if (UIManager.Instance.IsOpen<BackpackForm>())
        //    {
        //        UIManager.Instance.Close<BackpackForm>();
        //    }
        //    else
        //    {
        //        UIManager.Instance.Open<BackpackForm>();
        //    }
        //}





    }
}
