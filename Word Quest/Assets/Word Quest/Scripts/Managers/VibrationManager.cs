using System.Collections.Generic;
using UnityEngine;
#if UNITY_ANDROID 
using UnityEngine.Android;
using System.Collections;
#endif

public class VibrationManager : MonoBehaviour
{
    public static VibrationManager Instance{get; private set;}
    [Header(" Settings ")]
    private bool haptics;
    
    private void Awake() {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);
    }


    public void EnableVibration()
    {
        haptics = true;
    }

    public void DisableHaptics()
    {
        haptics = false;
    }
    public bool VibrationEnabled()
    {
        return haptics;
    }

    public static void Vibrate()
    {
        if (Instance.VibrationEnabled())
        {
            Handheld.Vibrate();
        }        
    }

}
