using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AdSource : MonoBehaviour
{
    public static AdSource Instance {get; private set;}
    
    public enum AdProviders { UnityAds, AdMob, TestProvider };
    public AdProviders CurrentProvider = AdProviders.TestProvider;

    private AdProvider _currentProvider = null;

    private void Awake() 
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        switch (CurrentProvider)
        {
            case AdProviders.AdMob:
                _currentProvider = new AdmobProvider();
                break;

            case AdProviders.UnityAds:
                break;

            case AdProviders.TestProvider:
                _currentProvider = GetComponent<TestAdProvider>();
                break;

        }
    }

    public AdProvider GetAdProvider()
    {
        return _currentProvider;
    }
}
