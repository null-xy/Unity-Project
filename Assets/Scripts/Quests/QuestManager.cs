using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour {
    public static QuestManager questManager;
    public List <Quest> questList = new List<Quest>();
    public List <Quest> currentQuestList = new List<Quest>();

    void Awake()
    {
        if (questManager == null)
        {
            questManager = this;
        }else if (questManager != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void QuestRequest(NPC NpcQuestObject)
    {
        //avaible quest
        if (NpcQuestObject.availableQuestIds.Count > 0)
        {
            for (int i = 0; i < questList.Count; i++)
            {
                for (int j = 0; j < NpcQuestObject.availableQuestIds.Count; j++)
                {
                    if (questList[i].id== NpcQuestObject.availableQuestIds[j] && questList[i].progress == Quest.QuestProgress.available)
                    {
                        AcceptQuest(NpcQuestObject.availableQuestIds[j]);
                    }
                }
                    
            }
        }
        //active quest
        for(int i = 0; i < currentQuestList.Count; i++)
        {
            for(int j = 0; j < NpcQuestObject.receivableQuestIds.Count; j++)
            {
                if(currentQuestList[i].id==NpcQuestObject.receivableQuestIds[j] && currentQuestList[i].progress==Quest.QuestProgress.accepted || currentQuestList[i].progress == Quest.QuestProgress.complete)
                {
                   // CompeleteQuest(NpcQuestObject.receivableQuestIds[j]);
                }
            }
        }
    }

    public void AcceptQuest(int questID)
    {
        for(int i = 0; i < questList.Count; i++)
        {
            if(questList[i].id== questID && questList[i].progress == Quest.QuestProgress.available)
            {
                currentQuestList.Add(questList[i]);
                //ui
                
                //QuestUI.questUI.activeQuests.Add(questList[i]);
                questList[i].progress = Quest.QuestProgress.accepted;
            }
        }
    }
    public void GiveUpQuest(int questID)
    {
        for(int i = 0; i < currentQuestList.Count; i++)
        {
            if(currentQuestList[i].id==questID && currentQuestList[i].progress == Quest.QuestProgress.accepted)
            {
                currentQuestList[i].progress = Quest.QuestProgress.available;
                currentQuestList[i].questObjectiveCount = 0;
                currentQuestList.Remove(currentQuestList[i]);
            }
        }
    }
    public void CompeleteQuest(int questID)
    {
        for(int i = 0; i < currentQuestList.Count; i++)
        {
            if(currentQuestList[i].id==questID && currentQuestList[i].progress == Quest.QuestProgress.complete)
            {
                currentQuestList[i].progress = Quest.QuestProgress.done;
                currentQuestList.Remove(currentQuestList[i]);
            }
        }
        //check for chain quests
        CheckChainQuest(questID);
    }
    void CheckChainQuest(int questID)
    {
        int num = 0;
        for(int i = 0; i < questList.Count; i++)
        {
            if(questList[i].id==questID && questList[i].nextQuest > 0)
            {
                num = questList[i].nextQuest;
            }
        }
        if (num > 0)
        {
            for(int i = 0; i < questList.Count; i++)
            {
                if(questList[i].id==num && questList[i].progress == Quest.QuestProgress.notAvailable)
                {
                    questList[i].progress = Quest.QuestProgress.available;
                }
            }
        }
    }
    public void AddQuestItem(Item questObjective, int itemAmount)
    {
        for(int i=0; i < currentQuestList.Count; i++)
        {
            if(currentQuestList[i].questObjective== questObjective && currentQuestList[i].progress== Quest.QuestProgress.accepted)
            {
                currentQuestList[i].questObjectiveCount += itemAmount;
            }
            if(currentQuestList[i].questObjectiveCount>=currentQuestList[i].questObjectiveRequirement && currentQuestList[i].progress == Quest.QuestProgress.accepted)
            {
                currentQuestList[i].progress = Quest.QuestProgress.complete;
            }
        }
    }
    public bool RequestAvailableQuest(int questID)
    {
        for(int i = 0; i < questList.Count; i++)
        {
            if (questList[i].id == questID && questList[i].progress == Quest.QuestProgress.available)
            {
                return true;
            }
        }return false;
    }
    public bool RequestAcceptedQuest(int questID)
    {
        for (int i = 0; i < questList.Count; i++)
        {
            if (questList[i].id == questID && questList[i].progress == Quest.QuestProgress.accepted)
            {
                return true;
            }
        }
        return false;
    }
    public bool RequestCompleteQuest(int questID)
    {
        for (int i = 0; i < questList.Count; i++)
        {
            if (questList[i].id == questID && questList[i].progress == Quest.QuestProgress.complete)
            {
                return true;
            }
        }
        return false;
    }
    public bool CheckAvailableQuests(NPC npcQuestObject)
    {
        for(int i = 0; i < questList.Count; i++)
        {
            for(int j = 0; j < npcQuestObject.availableQuestIds.Count; j++)
            {
                if(questList[i].id==npcQuestObject.availableQuestIds[j] && questList[i].progress == Quest.QuestProgress.available)
                {
                    return true;
                }
            }
        }return false;
    }

    public bool CheckAcceptedQuests(NPC npcQuestObject)
    {
        for (int i = 0; i < questList.Count; i++)
        {
            for (int j = 0; j < npcQuestObject.receivableQuestIds.Count; j++)
            {
                if (questList[i].id == npcQuestObject.receivableQuestIds[j] && questList[i].progress == Quest.QuestProgress.available)
                {
                    return true;
                }
            }
        }
        return false;
    }
    public bool CheckCompletedQuests(NPC npcQuestObject)
    {
        for (int i = 0; i < questList.Count; i++)
        {
            for (int j = 0; j < npcQuestObject.receivableQuestIds.Count; j++)
            {
                if (questList[i].id == npcQuestObject.receivableQuestIds[j] && questList[i].progress == Quest.QuestProgress.available)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
