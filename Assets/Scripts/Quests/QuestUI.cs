using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestUI : MonoBehaviour {
    private InputManager playerInput;
    public static QuestUI questUI;
    public GameObject questPanel;
    public List<Quest> activeQuests = new List<Quest>();
    
    private List<GameObject> qButtons=new List<GameObject>();
    public GameObject qButton;
    public Transform qButtonSpacer;
    
    //quest info
    public Text questTitle;
    public Text questDescription;

    /*protected virtual void OnValidate()
    {
        GetComponentsInChildren<QuestButton>(includeInactive: true, result: qButtons);
    }*/
    protected void RenewButton()
    {
        for(int i = 0; i < qButtons.Count; i++)
        {
            //qButtons[i]
        }

    }
    void Awake()
    {
        if (questUI == null)
        {
            questUI = this;
        }else if (questUI != this)
        {
            Destroy(gameObject);
        }
    }
	void Start () {
        playerInput = GameManager.Instance.InputManager;
        activeQuests = QuestManager.questManager.currentQuestList;
    }
	
	void Update () {
        if (playerInput.questPanelShow && playerInput.OpenQuestPanel)
        {
            ShowQuestPanel();
        }else if (!playerInput.questPanelShow && playerInput.OpenQuestPanel)
        {
            HideQuestPanel();
        }
	}
    public void ShowQuestPanel()
    {
        FillQuestButton();
        questPanel.SetActive(true);
        
    }
    public void HideQuestPanel()
    {
        questTitle.text = "";
        questDescription.text = "";
        questPanel.SetActive(false);
        //activeQuests.Clear();
        /*for (int i = 0; i < qButtons.Count; i++)
        {
            Destroy(qButtons[i]);
        }*/
        if(qButtonSpacer.childCount > 0)
        {
            foreach (Transform t in qButtonSpacer)
            {
                Destroy(t.gameObject);
            }
        }
        qButtons.Clear();
    }
    void FillQuestButton()
    {
        foreach(Quest activeQuest in activeQuests)
        {
            GameObject qusetButton = Instantiate(qButton);
            QuestButton qButtonScript = qusetButton.GetComponent<QuestButton>();
            qButtonScript.questID = activeQuest.id;
            qButtonScript.questTitle.text = activeQuest.title;
            qusetButton.transform.SetParent(qButtonSpacer, false);
            qButtons.Add(qusetButton);
        }
    }
    public void ShowSelectedQuest(int questID)
    {
        for(int i=0; i < activeQuests.Count; i++)
        {
            if (activeQuests[i].id == questID && activeQuests[i].progress == Quest.QuestProgress.accepted)
            {
                //questTitle.text = activeQuests[i].title;
                questDescription.text = activeQuests[i].description;
            }
        }
    }
}
