using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Questable : MonoBehaviour
{
    public Quest quest;

    /// <summary>
    /// ��ȡί������
    /// </summary>
    public void AcquireQuest()
    {
        if(quest.questStatus == Quest.QuestStatus.Waitting)
        {
            //ί��һ������
            CharacterManager.Instance.Player.QuestList.Add(quest);
            quest.questStatus = Quest.QuestStatus.Accepted;
        }
        else
        {
            //�Ѿ����������
            Debug.LogWarning("Already has this quest . You can't double collect");
        }

    }
}
