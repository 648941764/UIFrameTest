using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Reflection;
using DG.Tweening;
using UnityEngine.EventSystems;

public class ShopItem
{
    public int id;
    public int amount;
    public int price;
}

public class PrepareForm : Form, IPointerClickHandler
{
    [Header("Backpack")]
    [SerializeField] private RectTransform _backParent;
    [SerializeField] private BackpackUI _backpackUIPrefab;
    [SerializeField] private Button _btnClose;
    [SerializeField] private Image  _imgDrag;
    [SerializeField] private RectTransform _backpackPanel;
    [SerializeField] private RectTransform[] _itemSlots;
    //需要添加一个使用物品的逻辑
    //使用完物品过后就减少物品
    
    [Header("RolePanel")]
    [SerializeField] private Text _txtPlayerHealth;
    [SerializeField] private Text _txtPlayerAttack;
    [SerializeField] private Text _txtPlayerDefence;
    [SerializeField] private RectTransform[] _equipmentSlot;
    [SerializeField] private Image _equipmentUIPrefab;
    [SerializeField] private Button[] _btnDischarge;
    private GameItem[] _shopEquipmentItemSlots = new GameItem[2];
    private Image[] _equipmentUIs = new Image[2];

    [Header("Btns")]
    [SerializeField] private Button _btnBackPack;
    [SerializeField] private Button _btnShop;
    [SerializeField] private Button _btnEnterGame;
    [SerializeField] private Button _btnEquipmentEnter;
    [SerializeField] private Button _btnEquipmentReduce;
    [SerializeField] private RectTransform _promptTitle;

    [Header("Shop")]
    [SerializeField] private RectTransform _shopPanel;
    [SerializeField] private RectTransform[] _shopImgs;
    [SerializeField] private Button[] _buttons;
    [SerializeField] private Text _txtGold;
    [SerializeField] private Text _txtTitle;
    [SerializeField] private Text _txtSign;
    [SerializeField] private CanvasGroup _canvasGroupTitle;

    private int _goldCount;
    private GameItem _chooseItem;
    private Dictionary<int, ShopData> _shopDatas = new Dictionary<int, ShopData>();
    private List<ShopItem> _dataList = new List<ShopItem>();
    private BackpackUI[] _backpackUIs;
    private Color _normalColor;
    public RectTransform BackpackParent => _backParent;

    public Image ImgDrag => _imgDrag;

    #region Form基类
    protected override void InitComponents()
    {
        base.InitComponents();
        _btnEnterGame.onClick.AddListener(OnBtnEnterGameClicked);
        _btnShop.onClick.AddListener(OnBtnShopClicked);
        _btnBackPack.onClick.AddListener(OnBtnBackpackClicked);
        _btnClose.onClick.AddListener(OnBtnCloseClicked);
        _btnEquipmentEnter.onClick.AddListener(OnBtnEquipmentEnterClicked);
        _btnEquipmentReduce.onClick.AddListener(OnBtnEquipmentReduceClicked);
        _backpackUIs = new BackpackUI[GameBackpack.ITEM_NUM];
        _normalColor = _itemSlots[0].GetComponent<Image>().color;
        int i = -1;
        while (++i < _buttons.Length)
        {
            int index = i;
            _buttons[i].onClick.AddListener(() => BtnBuyClicked(index));
        }

        int k = -1;
        while (++k < _btnDischarge.Length) 
        {
            int index = k;
            _btnDischarge[k].onClick.AddListener(() => OnBtnDischargeClicked(index));
        }
        //==============================================================================================================读取数据
        _shopDatas = DataManager.Instance.shopDatas;
        foreach (var item in _shopDatas.Values)
        {
            ShopItem temp = new ShopItem();
            temp.id = item.id;
            temp.amount = item.amount;
            temp.price = item.price;
            _dataList.Add(temp);
        }
    }

    protected override void OnRefresh()
    {
        base.OnRefresh();
        RefreshSlotImage();
        GameItem[] items = CharacterManager.Instance.GameBackpack.Items;
        int i = -1;
        while (++i < GameBackpack.ITEM_NUM)
        {
            if (items[i] == null)
            {
                if (_backpackUIs[i] != null)
                {
                    _backpackUIs[i].SetActivate(false);
                }
                continue;
            }

            if (_backpackUIs[i] == null)
            {
                _backpackUIs[i] = Instantiate<BackpackUI>(_backpackUIPrefab, _itemSlots[i]);
                _backpackUIs[i].transform.localPosition = Vector3.zero;
                _backpackUIs[i].SetIndex(i);
            }
            else
            {
                _backpackUIs[i].SetActivate(true);
            }
            _backpackUIs[i].RefreshBackpackUI(items[i]);
        }
    }

    protected override void RegisterEvents()
    {
        base.RegisterEvents();
        AddEvent(OnBackpackItemChange);
    }

    protected override void OnOpen()
    {
        base.OnOpen();
        AddEvent(OnBackpackItemChange);
        RefreshShopUI();
        RefreshRolePanel();
        GameManager.Instance.UpdateHandle += PrepareFormUpdate;
    }

    protected override void OnClose()
    {
        base.OnClose();
        DelEvent(OnBackpackItemChange);
        GameManager.Instance.UpdateHandle -= PrepareFormUpdate;
    }

    private void OnBackpackItemChange(EventParam eventParam)
    {
        if (eventParam.eventName == EventType.BackpackItemChange)
        {
            OnRefresh();
        }
    }

    private void RefreshShopUI()
    {
        CharacterEntity player = CharacterManager.Instance.PlayerEntity;
        {
            if (player == null)
            {
                _goldCount = 0;
            }
            else
            {
                _goldCount = player.GetGold();
            }
            _txtGold.text = _goldCount.ToString();
            RefreshShopImgAndBtn();
        }
    }
    #endregion

    #region 按钮逻辑

    private void OnBtnEnterGameClicked()
    {
        GameManager.Instance.StartGame();
        UIManager.Instance.Close<PrepareForm>();
        //把选择好的物品放到快捷栏里
        //进入游戏
        //把携带好的物品加入到游戏的背包里面
        //关闭当前场景
    }

    private void OnBtnShopClicked()
    {
        _shopPanel.SetActivate(true);
        _backpackPanel.SetActivate(false);
        _txtTitle.text = "Shop";

    }

    private void OnBtnBackpackClicked()
    {
        _shopPanel.SetActivate(false);
        _backpackPanel.SetActivate(true);
        _txtTitle.text = "Backpack";
    }

    private void OnBtnCloseClicked()
    {
    }

    #endregion

    #region 背包的逻辑
    public int FindNearestBackpackItems(Image image)
    {
        for (int i = 0; i < _itemSlots.Length; i++)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(_itemSlots[i], image.transform.position))
            {
                return i;
            }
        }
        return -1;
    }

    /// <summary>
    /// 把物品放入装备栏的方法
    /// </summary>
    public void OnBtnEquipmentEnterClicked()
    {
        if (_chooseItem == null)
        {
            Debug.LogError("不存在选择的物品");
            return;
        }
        ItemData itemData = DataManager.Instance.itemDatas[_chooseItem.id];

        if (itemData.itemType != ItemEnum.Equipment)
        {
            _promptTitle.SetActivate(false);
            RefreshSlotImage();
            _chooseItem = null;
            return;
        }
        //把chooseItem的数据放到格子里面
        //for (int i = 0; i < _shopEquipmentItemSlots.Length; i++)
        //{
        //    if (_shopEquipmentItemSlots[i] == null)//数据逻辑，后面放到CharacterManager
        //    {
        //        CharacterEntity player = CharacterManager.Instance.PlayerEntity;
        //        player.SetAttack(player.GetAttack() + itemData.attack);
        //        player.SetDefence(player.GetDefence() + itemData.defence);
        //        _shopEquipmentItemSlots[i] = _chooseItem;
        //        CharacterManager.Instance.GameBackpack.RemoveItme(_chooseItem);
        //        _chooseItem = null;
        //        _promptTitle.SetActivate(false);
        //        break;
        //    }
        //}
        CharacterManager.Instance.ChangePlayerDataForEquipment(_shopEquipmentItemSlots, _chooseItem);//把上面代码改写成方法
        _promptTitle.SetActivate(false);
        RefreshRolePanel();
        RefreshEquipmentslot();
    }

    public void PrepareFormUpdate()//数据逻辑，后面放到CharacterManager
    {
        if (Input.GetKeyDown(KeyCode.K) && _chooseItem != null)
        {
            CharacterManager.Instance.UseItemForFood(_chooseItem);
            RefreshRolePanel();
            _promptTitle.SetActivate(false);
            RefreshSlotImage();
        }
    }

    public void OnBtnEquipmentReduceClicked() 
    {

        _promptTitle.SetActivate(false);
        RefreshSlotImage();
        _chooseItem = null;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button != PointerEventData.InputButton.Right)
        {
            return;
        }
        Vector2 mouPos = eventData.position;
        int i = -1;
        while (++i < _itemSlots.Length)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(_itemSlots[i], mouPos))
            {
                GameItem currentItem = CharacterManager.Instance.GameBackpack.Items[i];
                if (currentItem == _chooseItem)
                {
                    _chooseItem = null;
                    _itemSlots[i].GetComponent<Image>().color = _normalColor;
                }
                else if (currentItem != null)
                {
                    RefreshSlotImage();
                    _chooseItem = currentItem;
                    _itemSlots[i].GetComponent<Image>().color = Color.gray;
                    _promptTitle.SetActivate(true);
                }
                break;
            }
        }
    }

    public void RefreshSlotImage()
    {
        int k = -1;
        while (++k < GameBackpack.ITEM_NUM)
        {
            _itemSlots[k].GetComponent<Image>().color = _normalColor;
        }
    }
    #endregion

    #region 商店的逻辑

    private void RefreshShopImgAndBtn()
    {
        int i = -1;
        while (++i < _shopImgs.Length)
        {
            ShopData shopData = DataManager.Instance.shopDatas[_dataList[i].id];
            ShopItem item = _dataList[i];
            _shopImgs[i].transform.GetComponent<Image>().sprite = DataManager.Instance.itemDatas[shopData.id].sprite;
            _buttons[i].transform.GetChild(0).GetComponent<Text>().text = _dataList[i].price.ToString();
            if (item.amount <= 0)
            {
                _buttons[i].interactable = false;
                _buttons[i].transform.GetChild(0).GetComponent<Text>().text = "售罄";
            } 
        }
    }

    private void BtnBuyClicked(int index)
    {
        CharacterEntity player = CharacterManager.Instance.PlayerEntity;
        ShopData shopData = DataManager.Instance.shopDatas[_dataList[index].id];
        ShopItem item = _dataList[index];
        int sum = player.GetGold() - shopData.price;
        if (sum < 0)
        {
            _txtSign.SetActivate(true);
            DOVirtual.Float(1f, 0f, 3f, _ => _canvasGroupTitle.alpha = _).OnComplete(() => _txtSign.SetActivate(false));
            return;
        }
        player.SetGold(sum);//数据逻辑，后面放到CharacterManager
        CharacterManager.Instance.GameBackpack.Additem(shopData.id, 1);
        item.amount -= 1;
        RefreshShopUI();
    }

    #endregion

    #region 角色面板的逻辑

    private void RefreshRolePanel()
    { 
        CharacterEntity player = CharacterManager.Instance.PlayerEntity;
        _txtPlayerAttack.text = player.GetAttack().ToString();
        _txtPlayerDefence.text = player.GetDefence().ToString();
        _txtPlayerHealth.text = player.GetHealth().ToString();
        
    }

    /// <summary>
    /// 刷新装备栏图片的物品
    /// </summary>
    public void RefreshEquipmentslot()
    {
        int i = -1;
        while (++i < _shopEquipmentItemSlots.Length)
        {
            if (_shopEquipmentItemSlots[i] == null)
            {
                if (_equipmentUIs[i] != null)
                {
                    _equipmentUIs[i].SetActivate(false);
                }
                continue;
            }

            if (_equipmentUIs[i] == null)
            {
                _equipmentUIs[i] = Instantiate<Image>(_equipmentUIPrefab, _equipmentSlot[i]);
            }
            else
            {
                _equipmentUIs[i].SetActivate(true);
            }
            _equipmentUIs[i].sprite = DataManager.Instance.itemDatas[_shopEquipmentItemSlots[i].id].sprite;

        }
    }

    public void OnBtnDischargeClicked(int index)//卸下装备，刷新角色面板数据 //数据逻辑，后面放到CharacterManager
    {
        GameItem item = _shopEquipmentItemSlots[index];
        if (item != null)
        {
            ItemData data = DataManager.Instance.itemDatas[item.id];
            CharacterEntity player = CharacterManager.Instance.PlayerEntity;
            int attack = player.GetAttack() - data.attack;
            int defence = player.GetDefence() - data.defence;
            player.SetAttack(attack);
            player.SetDefence(defence);
            _txtPlayerAttack.text = player.GetAttack().ToString();
            _txtPlayerDefence.text = player.GetDefence().ToString();
            _shopEquipmentItemSlots[index] = null;
            CharacterManager.Instance.GameBackpack.Additem(data.id,1);
            RefreshEquipmentslot();
        }
    }


    #endregion
}
