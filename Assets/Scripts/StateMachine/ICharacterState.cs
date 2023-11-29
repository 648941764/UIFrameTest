

public interface ICharacterState
{

    void OnEnter();

    /// <summary>
    /// /在这个状态需要做什么
    /// </summary>
    void OnUpdate();

    void OnExit();
}
