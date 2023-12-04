using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary> ms timer counter /// </summary>
public sealed partial class Timer
{
    private float 
        timer;
    private int
        tick,       // 真正计时的事件(ms)
        elapsed;    // 当前过度的事件
    public int
        time,       // 总时间
        interval = 1,   // 间隔(ms)
        delay = 0,      // 推迟
        repeate = 1;    // 重复次数

    private bool ticking;
    public bool returnToPool;
    public Action onStart, onComplete;
    public Action<int> onTick;         // 当前计时的时间
    public Action<float> onTickTime;   // 剩余时间

    public void Start()
    {
        ticking = true;
        onStart?.Invoke();
        GameManager.Instance.TimeUpdateHandle += Tick;
    }

    public void Tick(float dt)
    {
        if (!ticking) { return; }

        timer += dt * 1000f;
        while (timer >= 1)
        {
            --timer;
            if (++elapsed <= delay) { continue; }
            ++tick;
            if (tick % interval == 0)
            {
                onTick?.Invoke(tick);
                onTickTime?.Invoke(time - tick);
            }
            if (tick >= time)
            {
                if (--repeate <= 0)
                {
                    Complete(true);
                }
                else
                {
                    tick = 0;
                }
            }
        }
    }

    public void Complete(bool callComplete = false)
    {
        ticking = false;
        if (callComplete)
        {
            onComplete?.Invoke();
        }
        GameManager.Instance.TimeUpdateHandle -= Tick;
        if (returnToPool)
        {
            pool.Release(this);
        }
    }

    public void Restart()
    {
        timer = 0f;
        elapsed = 0;
        tick = 0;
        Start();
    }

    private void Reset()
    {
        timer = 0f;
        time = 0;
        tick = 0;
        elapsed = 0;
        interval = 0;
        delay = 0;
        repeate = 0;
        ticking = false;
        onStart = null;
        onComplete = null;
        onTick = null;
        onTickTime = null;
    }
}

public partial class Timer
{
    private static TimerPool pool = new TimerPool();
    public static void Tick(
        int time,
        Action<int> onTick = default, Action<float> onTickTime = default,
        Action onStart = default, Action onComplete = default,
        int interval = 1, int delay = 0, int repeat = 1)
    {
        if (repeat <= 0) { return; }
        Timer timer = pool.Get();
        timer.Reset();
        timer.time = time;
        timer.interval = interval;
        timer.delay = delay;
        timer.repeate = repeat;
        timer.onTick = onTick;
        timer.onTickTime = onTickTime;
        timer.onStart = onStart;
        timer.onComplete = onComplete;
        timer.Start();
    }

    public static Timer GetTimer()
    {
        return pool.Get();
    }
}

public partial class Timer
{
    private class TimerPool
    {
        public readonly Stack<Timer> _pool = new Stack<Timer>();

        public Timer Get()
        {
            if (_pool.Count == 0)
            {
                return new Timer();
            }
            return _pool.Pop();
        }

        public void Release(Timer timer)
        {
            _pool.Push(timer);
        }
    }
}