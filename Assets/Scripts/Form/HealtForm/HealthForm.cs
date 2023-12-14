using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthForm : Form
{
    [SerializeField] private Image _imgHeatlthBuffer, _imgHealth;
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
        _imgHealth.fillAmount = tempFillAmount;

        if(_imgHeatlthBuffer.fillAmount > _imgHealth.fillAmount)
        {
            _imgHeatlthBuffer.fillAmount -= Time.deltaTime;
        }
        else
        {
            _imgHeatlthBuffer.fillAmount = _imgHealth.fillAmount;
            _isHealthChange = false;
        }
    }
}
