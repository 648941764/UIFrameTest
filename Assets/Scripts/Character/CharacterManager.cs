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
    private readonly CharacterEntity _playerEntity = new CharacterEntity();

    private Player _player;
    public Player Player => _player;

    public CharacterEntity PlayerEntity => _playerEntity;


    public event Action CharacterUpdateHandle;
    public event Action<float> TimeCharacterUpdateHandle;

    protected override void OnAwake()
    {
        GameManager.Instance.UpdateHandle += CharacterUpdateHandle;
        GameManager.Instance.TimeUpdateHandle += TimeCharacterUpdateHandle;
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
        _characaters.Clear();
        _enemyEntities.Clear();
    }

    public void GetEnemiesOnSceneLoaded(List<Character> characters)//处理场景里所有角色类物体
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
        _enemyEntities.Add(uid, entity);
    }

    public void CreatePlayerEntity()
    {
        if (_playerEntity == null)
        {
            PlayerData data = DataManager.Instance.playerDatas[PLAYER_ID];
            _playerEntity.SetID(data.id);
            _playerEntity.SetUID(PLAYER_ID);
            _playerEntity.SetDefence(data.defence);
            _playerEntity.SetAttack(data.attack);
            _playerEntity.SetHealth(data.hp);
            _playerEntity.SetMaxHealth(data.maxHp);
            _playerEntity.SetLevel(1);
            _playerEntity.SetExp(0);
        }
    }
    
    public List<Enemy> FindInAttackRangeEnemies()
    {
        List<Enemy> list = new List<Enemy>();
        int direction = _player.Orientation ? 1 : -1;
        float attackRange = _player.Position.x + (_player.CharacterInfo.attackRange * direction);
        
        foreach (var enemy in _characaters.Values)
        {

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
}