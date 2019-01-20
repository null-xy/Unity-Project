using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour {
    public static DialogueSystem Instance { get; set; }
    public GameObject dialoguePanel;
    public List<string> dialogueLines = new List<string>();
    public string npcName;

    public bool isTalking=false;

    Button nextButton;
    Text dialogueText, nameText;
    int dialogueIndex;

	// Use this for initialization
	void Awake () {
        nextButton = dialoguePanel.transform.Find("Button").GetComponent<Button>();
        dialogueText = dialoguePanel.transform.Find("Text").GetComponent<Text>();
        nextButton.onClick.AddListener(delegate { ContinueDialogue(); });
        dialoguePanel.SetActive(false);
		if(Instance!=null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
	}
	
	// Update is called once per frame
    public void AddNewDialogue(string[] lines,string npcName)
    {
        dialogueIndex = 0;
        dialogueLines = new List<string>();
        foreach(string line in lines)
        {
            dialogueLines.Add(line);
        }
        this.npcName = npcName;
        Debug.Log(dialogueLines.Count);
        CreatDialogue();
    }
    public void CreatDialogue()
    {
        // dialogueText.text = dialogueLines[dialogueIndex];
        //name.text = NPCname;
        dialogueText.text = npcName +":  "+ dialogueLines[0];
        dialoguePanel.SetActive(true);
        isTalking = true;
    }
    public void ContinueDialogue()
    {
        if (dialogueIndex < dialogueLines.Count-1)
        {
            dialogueIndex++;
            dialogueText.text = npcName + ":  " + dialogueLines[dialogueIndex];
        }
        else
        {
            dialoguePanel.SetActive(false);
            isTalking = false;
        }
    }
	void Update () {
		
	}
}
