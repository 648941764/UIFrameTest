using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Reflection;
using DG.Tweening;

public class ShopItem
{
    public int id;
    public int amount;
    public int price;
}

public class PrepareForm : Form
{
    [Header("Backpack")]
    [SerializeField] private RectTransform _backParent;
    [SerializeField] private BackpackUI _backpackUIPrefab;
    [SerializeField] private Button _btnClose;
    [SerializeField] private Image  _imgDrag;
    [SerializeField] private RectTransform _backpackPanel;
    [SerializeField] private RectTransform[] _itmeSlots;
    
    [Header("RolePanel")]
    [SerializeField] private Text _txtPlayerHealth;
    [SerializeField] private Text _txtPlayerAttack;
    [SerializeField] private Text _txtPlayerDefence;

    [Header("Btns")]
    [SerializeField] private Button _btnBackPack;
    [SerializeField] private Button _btnShop;
    [SerializeField] private Button _btnEnterGame;

    [Header("Shop")]
    [SerializeField] private RectTransform _shopPanel;
    [SerializeField] private RectTransform[] _shopImgs;
    [SerializeField] private Button[] _buttons;
    [SerializeField] private Text _txtGold;
    [SerializeField] private Text _txtTitle;
    [SerializeField] private Text _txtSign;
    [SerializeField] private CanvasGroup _canvasGroupTitle;

    private int _goldCount;
    private Dictionary<int, ShopData> _shopDatas = new Dictionary<int, ShopData>();
    private List<ShopItem> _dataList = new List<ShopItem>();
    private BackpackUI[] _backpackUIs;

    public RectTransform BackpackParent => _backParent;

    public Image ImgDrag => _imgDrag;

    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            CharacterManager.Instance.GameBackpack.Additem(3004, 1);
        }
    }

    #region Form基类
    protected override void InitComponents()
    {
        base.InitComponents();
        _btnEnterGame.onClick.AddListener(OnBtnEnterGameClicked);
        _btnShop.onClick.AddListener(OnBtnShopClicked);
        _btnBackPack.onClick.AddListener(OnBtnBackpackClicked);
        _btnClose.onClick.AddListener(OnBtnCloseClicked);
        _backpackUIs = new BackpackUI[GameBackpack.ITEM_NUM];
        int i = -1;
        while (++i < _buttons.Length)
        {
            int index = i;
            _buttons[i].onClick.AddListener(() => BtnBuyClicked(index));
        }
        //=====================================================================读取数据
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
                _backpackUIs[i] = Instantiate<BackpackUI>(_backpackUIPrefab, _itmeSlots[i]);
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
    }

    protected override void OnClose()
    {
        base.OnClose();
        DelEvent(OnBackpackItemChange);
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
        for (int i = 0; i < _itmeSlots.Length; i++)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(_itmeSlots[i], image.transform.position))
            {
                return i;
            }
        }
        return -1;
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
            if(item.amount == 0)
            {
                _buttons[i].interactable = false;
                _buttons[i].transform.GetChild(0).GetComponent<Text>().text = "售罄";
                return;
            } 
            _buttons[i].transform.GetChild(0).GetComponent<Text>().text = _dataList[i].price.ToString();
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
        player.SetGold(sum);
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

    #endregion
}
