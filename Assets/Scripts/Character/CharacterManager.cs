using System;
using System.Collections.Generic;

public sealed class CharacterManager : SingletonMono<CharacterManager>
{
    public const int PLAYER_ID = 4001;

    private readonly Dictionary<int, Character> _characaters = new Dictionary<int, Character>();
    private readonly Dictionary<int, CharacterEntity> _enemyEntities = new Dictionary<int, CharacterEntity>();
    private readonly CharacterEntity _playerEntity = new CharacterEntity();

    public CharacterEntity PlayerEntity => _playerEntity;

    public event Action fsmExecute;

    protected override void OnAwake()
    {
        GameManager.Instance.UpdateHandle += Execute;
    }

    public CharacterEntity GetEnemyEntity(int uid)
    {
        return _enemyEntities[uid];
    }

    public void AddEntity(CharacterEntity entity)
    {
        _enemyEntities[entity.UID] = entity;
    }

    public void Execute()
    {
        fsmExecute?.Invoke();
    }

}