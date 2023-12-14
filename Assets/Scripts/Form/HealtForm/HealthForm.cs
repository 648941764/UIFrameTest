using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthForm : Form
{
    [SerializeField] private Image _imaHeatlthBuffer, _imaHealth;
    private int _uid;
    private bool _isHealthChange = true;
    private CharacterEntity entity;

    private void OnEnable()
    {
        _uid = CharacterManager.Instance.Player.UID;
        entity = CharacterManager.Instance.PlayerEntity;
        EventManager.Instance.Add(OnPlayerHealthChange);
        GameManager.Instance.UpdateHandle += UpdateHeathBar;
    }

    private void OnDisable()
    {
        EventManager.Instance.Del(OnPlayerHealthChange);
        GameManager.Instance.UpdateHandle -= UpdateHeathBar;
    }

    private void OnPlayerHealthChange(EventParam eventParam)
    {
        if (eventParam.eventName != EventType.OnHealthChange)
        {
            return;
        }
        int uid = eventParam.Get<int>(0);
        if (uid != _uid)
        {
            return;
        }
        _isHealthChange = true;
    }

    private void UpdateHeathBar()
    {
        if (!_isHealthChange)
        {
            return;
        }
        
        int health = entity.GetHealth();
        float tempFillAmount = (float)health / entity.GetMaxHealth();
        _imaHealth.fillAmount = tempFillAmount;

        if(_imaHeatlthBuffer.fillAmount > _imaHealth.fillAmount)
        {
            _imaHeatlthBuffer.fillAmount -= Time.deltaTime;
        }
        else
        {
            _imaHeatlthBuffer.fillAmount = _imaHealth.fillAmount;
            _isHealthChange = false;
        }

    }
}
