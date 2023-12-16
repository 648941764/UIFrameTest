using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public sealed class DropManager : Singleton<DropManager>
{
    private readonly Stack<DropItem> _pool = new Stack<DropItem>();
    private readonly List<DropItem> _dropItems = new List<DropItem>();
    private readonly List<DropItem> _pickedItems = new List<DropItem>();

    public void DropFromEnemy(int enemyId)
    {
        int dropId = CharacterManager.Instance.GetEnemyEntity(enemyId).GetDropId();
        if (dropId == 0) { return; }
        DropData dropData = DataManager.Instance.dropDatas[dropId];
        int index = Random.Range(0, dropData.items.Count);
        int dropItemId = dropData.items[index];
        int dropItemAmount = dropData.amounts[index];
        Enemy enemy = CharacterManager.Instance.GetEnemy(enemyId);
        Vector3 dropPos = enemy.transform.GetChild(1).position;
        Drop(dropItemId, dropItemAmount, dropPos);
        Debug.Log("物品掉落");
    }
    
    public void OnUpdatePick()
    {
        Player player = CharacterManager.Instance.Player;
        for (int i = -1; ++i < _dropItems.Count;)
        {
            DropItem dropItem = _dropItems[i];
            if (Mathf.Abs(player.Position.y - dropItem.transform.position.y) < 0.5f &&
                Mathf.Abs(player.Position.x - dropItem.transform.position.x) < 0.5f)
            {
                PlayerPickUp(dropItem);
                _pool.Push(dropItem);
                _pickedItems.Add(dropItem);
            }
        }

        if (_pickedItems.Count > 0)
        {
            for (int i = -1; ++i < _pickedItems.Count;)
            {
                _dropItems.Remove(_pickedItems[i]);
            }
            _pickedItems.Clear();
        }
    }

    public void Drop(int itemId, int amount, Vector3 postion)
    {
        DropItem dropItem = _pool.Count == 0 ? DropDatas.CreateDropItem() : _pool.Pop();
        dropItem.SetActivate(true);
        dropItem.Init(itemId, amount, postion);
        _dropItems.Add(dropItem);
    }

    public void PlayerPickUp(DropItem dropItem)
    {
        dropItem.SetActivate(false);
        // 将数据添加进背包
    }
}