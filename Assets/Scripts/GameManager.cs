using System;
using System.Collections.Generic;

public sealed class GameManager : SingletonMono<GameManager>
{
    public event Action UpdateHandle;

    private void Update()
    {
        UpdateHandle?.Invoke();
    }
}