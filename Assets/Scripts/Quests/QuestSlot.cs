using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using System;

public class QuestSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{

    public int questID;
    public Text questTitle;
    public Button questBoxImage;
    public Image questFollowImage;

    public event Action<QuestSlot> OnRightClickEvent;
    public event Action<QuestSlot> OnPointerEnterEvent;
    public event Action<QuestSlot> OnPointerExitEvent;

    public void AddQuest(int newQuestID)
    {
        questTitle.enabled = true;
        questBoxImage.interactable = true;
        questBoxImage.image.color = Color.clear;
    }

    public void ClearSlot()
    {
        questID = 0;
        questTitle.text = null;
        questTitle.enabled = false;
        questBoxImage.interactable = false;
        questFollowImage.enabled = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData != null && eventData.button == PointerEventData.InputButton.Right|| eventData.button == PointerEventData.InputButton.Left)
        {
            if (OnRightClickEvent != null)
                OnRightClickEvent(this);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (OnPointerEnterEvent != null)
            OnPointerEnterEvent(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (OnPointerExitEvent != null)
            OnPointerExitEvent(this);
    }
}
