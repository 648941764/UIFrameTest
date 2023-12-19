using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class CharacterManager : SingletonMono<CharacterManager>
{
    public const int PLAYER_ID = 4001;
    public int characterUID;

    private readonly Dictionary<int, Enemy> _characaters = new Dictionary<int, Enemy>();
    private readonly Dictionary<int, CharacterEntity> _enemyEntities = new Dictionary<int, CharacterEntity>();
    private CharacterEntity _playerEntity;

    private Player _player;
    private GameBackpack gameBackpack;

    public GameBackpack GameBackpack => gameBackpack;
    public Player Player => _player;

    public CharacterEntity PlayerEntity => _playerEntity;

    protected override void OnAwake()
    {
        EventManager.Instance.Add(OnCharacterDead);
    }

    private void OnCharacterDead(EventParam eventParam)
    {
        if (eventParam.eventName != EventType.OnEnemyDeath) { return; }

        int uid = eventParam.Get<int>(0);
        if (uid == PLAYER_ID)
        {
            _player.OnPlayerDeath();
            foreach (var key in _characaters.Keys)
            {
                _characaters[key].FSM.Switch(CharacterState.Idle);
            }
            //UIManager.Instance.Close<HealthForm>();
            return;
        }
        _characaters[uid].FSM.Switch(CharacterState.Death);
        _characaters[uid].FSM.OnExit();
    }

    public Enemy GetEnemy(int enemyId)
    {
        _characaters.TryGetValue(enemyId, out Enemy enemy);
        return enemy;
    }

    public CharacterEntity GetEnemyEntity(int uid)
    {
        return _enemyEntities[uid];
    }

    public void AddEntity(CharacterEntity entity)
    {
        _enemyEntities[entity.UID] = entity;
    }

    public void SetPlayer(Player player)
    {
        _player = player;
    }

    public void ClearEnemies()
    {
        foreach (var character in _characaters.Values)
        {
            character.FSM.OnExit();
        }
        _characaters.Clear();
        _enemyEntities.Clear();
    }

    public void CreateEnemiesOnSceneLoaded(List<Character> characters)//处理场景里所有角色类物体
    {
        characterUID = 1;
        for (int i = -1; ++i < characters.Count;)
        {
            Character character = characters[i];
            switch (character)
            {
                case Enemy enemy:
                    {
                        _characaters.Add(characterUID, enemy);
                        CreateEnemyEntity(characterUID);
                        enemy.Init(characterUID);
                        ++characterUID;
                        break;
                    }
                case Player player:
                    {
                        player.Init(PLAYER_ID);
                        _player = player;
                        UIManager.Instance.Open<HealthForm>();
                        break;
                    }
            }
        }
    }

    private void CreateEnemyEntity(int uid)
    {
        CharacterEntity entity = new CharacterEntity();
        List<int> ids = DataManager.Instance.enemyDatas.Keys.ToList();
        int id = ids[UnityEngine.Random.Range(0, ids.Count)];
        EnemyData data = DataManager.Instance.enemyDatas[id];
        entity.SetID(id);
        entity.SetUID(uid);
        entity.SetAttack(data.attack);
        entity.SetDefence(data.defence);
        entity.SetMaxHealth(data.hp);
        entity.SetHealth(data.hp);
        entity.SetGold(data.Gold);
        entity.SetExp(data.Exp);
        var drops = DataManager.Instance.dropDatas.Keys.ToList();
        drops.Add(0);
        entity.SetDropId(drops[UnityEngine.Random.Range(0, drops.Count)]);
        entity.SetAlive();
        _enemyEntities.Add(uid, entity);
    }

    private void CreatePlayerEntity()
    {
        if (_playerEntity == null)
        {
            _playerEntity = new CharacterEntity();
            PlayerData data = DataManager.Instance.playerDatas[PLAYER_ID];
            _playerEntity.SetID(data.id);
            _playerEntity.SetUID(PLAYER_ID);
            _playerEntity.SetDefence(data.defence);
            _playerEntity.SetAttack(data.attack);
            _playerEntity.SetHealth(data.maxHp);
            _playerEntity.SetMaxHealth(data.maxHp);
            _playerEntity.SetLevel(1);
            _playerEntity.SetExp(0);
            _playerEntity.SetAlive();
            _playerEntity.SetGold(1);
        }
        else if (_playerEntity.IsDead())
        {
            _playerEntity.SetHealth(_playerEntity.GetMaxHealth());
            _player.SetAlive();
        }
    }
    
    public List<Enemy> FindInAttackRangeEnemies()
    {
        List<Enemy> list = new List<Enemy>();
        int direction = _player.Orientation ? 1 : -1;
        float attackRange = _player.Position.x + (_player.CharacterInfo.attackRange * direction);
        
        foreach (var uid in _characaters.Keys)
        {
            if (_enemyEntities[uid].IsDead())
            {
                continue;
            }

            Enemy enemy = _characaters[uid];

            if (Mathf.Abs(_player.Position.y - enemy.Position.y) > 1.6f)
            {
                continue;
            }

            if (_player.Orientation && enemy.Position.x > _player.Position.x && enemy.Position.x < attackRange)
            {
                list.Add(enemy);
            }
            else if (!_player.Orientation && enemy.Position.x < _player.Position.x && enemy.Position.x > attackRange)
            {
                list.Add(enemy);
            }
        }
        return list;
    }

    public void LoadPlayerData()
    {
        CreatePlayerEntity();
        gameBackpack = new GameBackpack();
    }
}