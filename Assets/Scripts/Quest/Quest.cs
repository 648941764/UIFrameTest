using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class Quest
{
    public enum QuestType { Gathering, Talk, Reach };
    public enum QuestStatus { Waitting, Accepted, Completed };

    public string questName;
    public QuestType questType;
    public QuestStatus questStatus;

    public int expRewards;
    public int golddRewards;
}
