using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Questable : MonoBehaviour
{
    public Quest quest;

    /// <summary>
    /// 获取委派任务
    /// </summary>
    public void AcquireQuest()
    {
        if(quest.questStatus == Quest.QuestStatus.Waitting)
        {
            //委派一个任务
            CharacterManager.Instance.Player.QuestList.Add(quest);
            quest.questStatus = Quest.QuestStatus.Accepted;
        }
        else
        {
            //已经有这个任务
            Debug.LogWarning("Already has this quest . You can't double collect");
        }

    }
}
