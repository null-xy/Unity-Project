using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour {

    Image m_Image;
    public Sprite pickupImage;
    public Sprite aimImage;
    public Sprite talkImage;
    public Sprite normalImage;
    public Text objName;
    InputManager playerInput;
    //Player player;
    /*private PlayerAim m_playerAim;
    public PlayerAim playeraim
    {
        get {
            if (m_playerAim == null)
                m_playerAim = GameObject.Find("Player").GetComponent<PlayerAim>();
            return m_playerAim;
        }
    }*/
    void Awake () {
        //m_Image = GetComponentInChildren<Image>();
        m_Image = transform.Find("crosshair").GetComponent<Image>();
        m_Image.gameObject.SetActive(true);
        playerInput = GameManager.Instance.InputManager;
    }
	void Update () {
        ChangeCrosshair(PlayerAim.myState);
        if(!GameManager.Instance.InputManager.InventoryShow && !playerInput.PauseGame)
        {
            m_Image.gameObject.SetActive(true);
        }else
        {
            m_Image.gameObject.SetActive(false);
        }
    }
    private void ChangeCrosshair(PlayerAim.aimState state)
    {
        switch (state)
        {
            case PlayerAim.aimState.normal:
                m_Image.sprite = normalImage;
                m_Image.color = Color.white;
                objName.text = "";
                break;
            case PlayerAim.aimState.grab:
                m_Image.sprite = pickupImage;
                m_Image.color = Color.white;
                if (PlayerAim.instance.interactedObj != null)
                {
                    objName.text = "[E]" + PlayerAim.instance.interactedObj.name;
                }                
                break;
            case PlayerAim.aimState.aim:
                m_Image.sprite = aimImage;
                m_Image.color = Color.red;
                break;
            case PlayerAim.aimState.talk:
                //m_Image.sprite = talkImage;
                m_Image.sprite = aimImage;
                m_Image.color = Color.red;
                break;
        }
    }
}
