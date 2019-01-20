using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {
    #region Singleton
    public static PlayerManager instance;
    
    void Awake ()
    {
        if (instance != null)
        {
            Debug.LogWarning("more than one instance of Player Manager");
            return;
        }
        instance = this;
    }
    #endregion
    //public static PlayerManager Instance { get; set; }
    public GameObject player;
}
