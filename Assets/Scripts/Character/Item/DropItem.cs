using UnityEngine;

public class DropItem : MonoBehaviour
{
    [SerializeField] SpriteRenderer _sprite;

    private int _itemId, _amount;

    public int ItemID => _itemId;
    public int Amount => _amount;

    private void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
    }

    public void Init(int itemId, int amount, Vector3 postion)
    {
        _itemId = itemId;
        _amount = amount;
        ItemData data = DataManager.Instance.itemDatas[itemId];
        _sprite.sprite = data.sprite;
        _sprite.transform.position = postion;
    }
}