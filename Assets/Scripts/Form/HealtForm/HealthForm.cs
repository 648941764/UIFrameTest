using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HealthForm : Form
{
    [SerializeField] private Image _imgHeatlthBuffer, _imgHealth;
    private int _uid;
    private CharacterEntity entity;

    protected override void OnOpen()
    {
        base.OnOpen();
        _uid = CharacterManager.Instance.Player.UID;
        entity = CharacterManager.Instance.PlayerEntity;
        _imgHealth.fillAmount = (float)entity.GetHealth() / entity.GetMaxHealth();
        _imgHeatlthBuffer.fillAmount = (float)entity.GetHealth() / entity.GetMaxHealth();
        AddEvent(OnPlayerHealthChange);
    }

    protected override void RegisterEvents()
    {
        base.RegisterEvents();
        AddEvent(OnPlayerHealthChange);
    }

    protected override void OnClose()
    {
        base.OnClose();
        DelEvent(OnPlayerHealthChange);
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
        int health = entity.GetHealth();

        float tempFillAmount = (float)health / entity.GetMaxHealth();
        //DOTween.To(
        //    () => _imgHealth.fillAmount,
        //    _ => _imgHealth.fillAmount = _,
        //    tempFillAmount, 2f);
        DOTween.Sequence()
            .Append(
                DOTween.To(
                    () => _imgHealth.fillAmount,
                    _ => _imgHealth.fillAmount = _,
                    tempFillAmount, 0.25f
                )
            )
            .Append(
                DOTween.To(
                    () => _imgHeatlthBuffer.fillAmount,
                    _ => _imgHeatlthBuffer.fillAmount = _,
                    tempFillAmount, 0.25f
                )
            );
    }
}
