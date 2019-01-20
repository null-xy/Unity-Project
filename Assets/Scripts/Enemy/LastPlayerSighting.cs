using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastPlayerSighting : MonoBehaviour {

    public Vector3 position = new Vector3(1000f, 1000f, 1000f);
    public Vector3 resetPosition = new Vector3(1000f, 1000f, 1000f);
    public float lightHightIntensity = 0.25f;
    public float lightLowIntensity = 0f;
    public float fadeSpeed = 7f;
    public float musicFadeSpeed = 1f;

    //private AlarmLight alarm;
    //private Light mainLight;
    //private AudioSource panicAudio;
    void SwitchAlarms()
    {
        if (position != resetPosition)
        {
        }
    }
}
