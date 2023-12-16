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
    private BackpackUI[] _backpackUIs;
    private bool _isOpen = true;

    public RectTransform BackpackParent => _backParent;

    public Image Image => _imgDrag;
    

    [Header("RolePanel")]
    [SerializeField] private Text _playerMaxHealth;
    [SerializeField] private Text _playerAttack;
    [SerializeField] private Text _playerDefence;

    [Header("Btns")]
    [SerializeField] private Button _btnBackPack;
    [SerializeField] private Button _btnShop;
    [SerializeField] private Button _btnEnterGame;


    #region Form����
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
        GameItem[] items = Test.Instance.GameBackpack.items;
        int i = -1;
        while (++i < GameBackpack.ITEM_NUM)
        {
            if (items[i] == null && _backpackUIs[i] != null)
            {
                _backpackUIs[i].SetActivate(false);
                continue;
            }

            if (_backpackUIs[i] == null)
            {
                _backpackUIs[i] = Instantiate<BackpackUI>(_backpackUIPrefab, _backParent);
                //����һ������ֵ�ķ���
            }

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

    #region ��ť�߼�

    private void OnBtnEnterGameClicked()
    {
        //��ѡ��õ���Ʒ�ŵ��������
        //������Ϸ
        //��Я���õ���Ʒ���뵽��Ϸ�ı�������
        //�رյ�ǰ����
    }

    private void OnBtnShopClicked()
    {
        //���̵��Form;
        //�رյ�ǰ����
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

    #region ���еı����ķ���

    #endregion
}
