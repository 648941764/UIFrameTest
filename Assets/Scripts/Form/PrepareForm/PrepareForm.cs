using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrepareForm : Form
{
    [Header("Backpack")]
    [SerializeField] private RectTransform _backParent;
    [SerializeField] private BackpackUI _backpackUIPrefab;
    [SerializeField] private Button _btnClose;
    [SerializeField] private Image  _imgDrag;
    [SerializeField] private RectTransform _backpackPanel;
    [SerializeField] private RectTransform[] _itmeSlots;
    private BackpackUI[] _backpackUIs;
    private bool _isOpen = true;

    public RectTransform BackpackParent => _backParent;

    public Image ImgDrag => _imgDrag;
    
    [Header("RolePanel")]
    [SerializeField] private Text _playerMaxHealth;
    [SerializeField] private Text _playerAttack;
    [SerializeField] private Text _playerDefence;

    [Header("Btns")]
    [SerializeField] private Button _btnBackPack;
    [SerializeField] private Button _btnShop;
    [SerializeField] private Button _btnEnterGame;

    [Header("Shop")]
    [SerializeField] private RectTransform _shopPanel;
    [SerializeField] private RectTransform[] _shopImgs;
    [SerializeField] private Button[] _buttons;
    [SerializeField] private Text _txtGold;
    private int _goldCount;
    private Dictionary<int, ShopData> shopDatas = DataManager.Instance.shopDatas; 


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
        CharacterEntity player = CharacterManager.Instance.PlayerEntity;
        if (player == null) 
        {
            _goldCount = 0;
        }
        else
        {
            _goldCount = player.GetGold();
        }
        _txtGold.text = _goldCount.ToString();
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
        //把原来背包的格子全部删除，重新加载新的格子
    }

    private void OnBtnBackpackClicked()
    {
        _backParent.SetActivate(!_isOpen);
        _isOpen = !_isOpen;
    }

    private void OnBtnCloseClicked()
    {
        _backParent.SetActivate(!_isOpen);
        _isOpen = !_isOpen;
    }

    #endregion

    #region 特有的背包的方法
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

    #region 商店的方法

    public void GetItemByShop()
    {

    }


    #endregion
}
