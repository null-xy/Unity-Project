using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {
    //public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    InputManager playerInput;
   // InventoryUI inventoryUI;
    void Awake()
    {
        playerInput = GameManager.Instance.InputManager;
        //inventoryUI = GetComponent<InventoryUI>();
        pauseMenuUI.SetActive(false);
    }
    void Update()
    {
        
            if (!playerInput.PauseGame)
            {
                Resume();
            }
            else if(playerInput.PauseGame)
            {
                Pause();
            }
        
        
    }
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }
	void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void LoadMenu()
    {
        Debug.Log("Loading menu.....");
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
    public void QuitGame()
    {
        Debug.Log("Quiting Game");
        Application.Quit();
    }
}
