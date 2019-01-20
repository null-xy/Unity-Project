using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public Slider hpBar;
    private PlayerStats playerStats;
	// Use this for initialization
	void Start () {
        playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
    }
	
	// Update is called once per frame
	void Update () {
        hpBar.maxValue = playerStats.maxHealth;
        hpBar.value = playerStats.currentHealth;
	}
}
