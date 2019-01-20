using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemTooltip : MonoBehaviour {

    // Use this for initialization
    //[SerializeField]
    //GameObject itemTooltip;
    [SerializeField]    Text itemDescrip;
    [SerializeField]
    GameObject itemTooltip;

    public void ShowTooltip(Item item)
    {
        itemDescrip.text = item.name;
        itemTooltip.SetActive(true);
    }
    public void HideTooltip()
    {
        itemTooltip.SetActive(false);
    }
}
