using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Image imgHp;

    private int _uid;

    private void OnEnable()
    {
        EventManager.Instance.Add(OnHealthChangeHandle);
    }

    private void OnDisable()
    {
        EventManager.Instance.Del(OnHealthChangeHandle);
    }

    public void Init(int uid)
    {
        _uid = uid;
        UpdateBar();
    }

    private void OnHealthChangeHandle(EventParam eventParam)
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
        UpdateBar();
    }

    private void UpdateBar()
    {
        CharacterEntity entity = CharacterManager.Instance.GetEnemyEntity(_uid);
        int health = entity.GetHealth();
        float currentFillAmount = (float)health / entity.GetMaxHealth();
        DOVirtual
            .Float(imgHp.fillAmount, currentFillAmount, 1f, _ => { imgHp.fillAmount = _; })
            .OnComplete(() => gameObject.SetActivate(health > 0));
        
    }
}
