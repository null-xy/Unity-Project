using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuestButton : MonoBehaviour
{

    // Use this for initialization
    public int questID;
    public Text questTitle;

    public void ShowQuestInfo()
    {
        QuestUI.questUI.ShowSelectedQuest(questID);
    }
}
