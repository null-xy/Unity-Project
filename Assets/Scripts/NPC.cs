using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour {

    // Use this for initialization
    public string npcName;
    //public string[] dialogue;
    public string dialogueMSG;
    //private bool hasInteracted;
    InputManager playerInput;
    //public int questID;
    public List<int> availableQuestIds = new List<int>();
    public List<int> receivableQuestIds = new List<int>();
    void Start () {
        //hasInteracted = false;
        playerInput = GameManager.Instance.InputManager;
    }
	
	// Update is called once per frame
	void Update () {
       // !hasInteracted &&

        if (Input.GetKeyDown(KeyCode.E) && PlayerAim.myState == PlayerAim.aimState.talk)
        {
            playerInput.Talking = true;
            Interact();
           // hasInteracted = true;
        }

    }
    public void Interact()
    {
        // Debug.Log("talk to base npc");
        //DialogueSystem.Instance.AddNewDialogue(dialogue, npcName);
        //Debug.Log("interacting with NPC");
        /*foreach (var msg in dialogueMessage)
        {
            Fungus.Flowchart.BroadcastFungusMessage(msg);
        }*/
        Fungus.Flowchart.BroadcastFungusMessage(dialogueMSG);
    }
    public void EndConversation()
    {
        playerInput.Talking = false;
    }
    public void AcceptQuest()//call it from flowchart
    {
        //QuestManager.questManager.AcceptQuest(questID);
        QuestManager.questManager.QuestRequest(this);
    }
}
