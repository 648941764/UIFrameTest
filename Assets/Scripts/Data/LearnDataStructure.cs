using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearnDataStructure : MonoBehaviour
{
    public Queue<float> queue;
    public Stack<int> stack;
    public Dictionary<int, string> dict;

    private void Awake()
    {
        queue = new Queue<float>();
        stack = new Stack<int>();
        dict = new Dictionary<int, string>();
    }

    private void Start()
    {
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Add(1, "Json");
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            Remove(1);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            Check();
        }
    }

    #region ���еĲ���
    //public void Add(float index)
    //{
    //    queue.Enqueue(index);
    //}

    //public void Del()
    //{
    //    queue.Dequeue();
    //    Debug.Log(queue.Count);
    //}

    //public void Check()
    //{
    //    int index = 0;
    //    foreach (var q in queue)
    //    {
    //        index++;
    //        Debug.Log($"��{index}��������{q}");
    //    }
    //}
    #endregion

    #region ջ�Ĳ���

    //public void Add(int index)
    //{
    //    stack.Push(index);
    //    Debug.Log(stack.Count);
    //}

    //public void Del()
    //{
    //    stack.Pop();
    //}

    //public void Check()
    //{
    //    foreach (int item in stack) 
    //    {
    //        Debug.Log(item);
    //    }
    //}

    //public void Find()
    //{
    //    bool include = stack.Contains(2);
    //    Debug.Log(include);
    //}


    #endregion

    #region �ֵ�Ĳ���
    
    public void Add(int id, string name)
    {
        if (!dict.ContainsKey(id))
        {
            dict.Add(id, name);
        }
        else
        {
            Debug.Log("<color=teal>�ֵ����е�ǰid������</color>");
        }
    }

    public void Remove(int id)
    {
        if(dict.ContainsKey(id))
        {
            dict.Remove(id);
        }
        else
        {
            Debug.Log("<color=yellow>�ֵ��Ѿ�û�е�ǰid</color>");
        }
    }

    public void Check()
    {
        if (dict.ContainsKey(1))
        {
            Debug.Log(dict[1]);
        }
    }

    
    #endregion
}
