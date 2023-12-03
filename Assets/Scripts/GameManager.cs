using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class GameManager : SingletonMono<GameManager>
{
    public event Action UpdateHandle;
    public event Action<float> TimeUpdateHandle;

    private void Update()
    {
        UpdateHandle?.Invoke();
        TimeUpdateHandle?.Invoke(Time.deltaTime);
    }
}