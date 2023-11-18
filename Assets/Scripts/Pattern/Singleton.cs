using System;

public class Singleton<T> where T : new()
{
    private static readonly Lazy<T> _lazy = new Lazy<T>(() => new T());

    protected Singleton() { }

    public static T Instance => _lazy.Value;
}
