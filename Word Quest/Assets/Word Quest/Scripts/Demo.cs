
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using EasyUI.Progress;

// Json element data
public struct GameData{
    public string Description;
    public string Version;
    public string GameURL;
    public string LogoURL;
}
public class Demo : MonoBehaviour
{
    [Header(" ## UI References :")]
    [SerializeField] private Button updateButton;
    [SerializeField] private Button reconnectButton;
    [SerializeField] private GameObject updatePanel;
    [SerializeField] private GameObject offlinePanel;
    
    [Header(" Settings ")]
    private  bool isAlreadyCheckedForUpdates = false;
    private GameData latestGameData;

    private string _jsonURL = "https://drive.google.com/uc?export=download&id=174L6d8RAECJfBxNwi77-nEdQ0jeVs8Ls";

    private void Start() 
    {
        updatePanel.SetActive(false);
        offlinePanel.SetActive(false);

        if(HasInternetConnection())
        {
            // user has internet connection
            if (!isAlreadyCheckedForUpdates)
            {
                Progress.Show("Please wait...",ProgressColor.Blue,true);
                Progress.SetDetailsText("Loading Datas...");
                StopAllCoroutines();
                StartCoroutine(CheckForUpdates());
                
            }
        }
        else
        { // user offline
            Progress.Hide();
            ShowConnect();
        }

    }   

    public void ShowPopup()
    {
        updateButton.onClick.AddListener(()=>
        { 
            Application.OpenURL(latestGameData.GameURL);
        });
        updatePanel.SetActive(true);
    }

    public void ShowConnect()
    {
        reconnectButton.onClick.AddListener(()=>{
            SceneManager.LoadScene(0);
        });
        offlinePanel.SetActive(true);
    }

    IEnumerator CheckForUpdates()
    {
        UnityWebRequest request = UnityWebRequest.Get(_jsonURL);
        
        request.SendWebRequest();
        
        while (!request.isDone)
        {
            Debug.Log("Request");
            float progress = request.downloadProgress * 100f;
            Progress.SetProgressValue(progress);

            if (request.result == UnityWebRequest.Result.ConnectionError ||
                request.result == UnityWebRequest.Result.DataProcessingError ||
                request.result == UnityWebRequest.Result.ProtocolError)
            {
                // Hata durumunda islemleri gerçekleştir (örnegin, bir hata mesaji göster)
                Debug.LogError("Web request error: " + request.error);
                break;
            }

            yield return null;
        }
        Progress.Hide();
        if (request.result == UnityWebRequest.Result.Success)
        {            
            string error = request.error;
            isAlreadyCheckedForUpdates = true;
            if (string.IsNullOrEmpty(error))
            {
                latestGameData = JsonUtility.FromJson<GameData>(request.downloadHandler.text);
                if (!string.IsNullOrEmpty(latestGameData.Version) && !Application.version.Equals(latestGameData.Version))
                {
                    // NEW UPDATE IS AVAILABLE
                    Progress.Hide();
                    ShowPopup();
                }
                else
                {
                    Debug.Log(" Guncelleme yok ! ");
                    SceneManager.LoadScene(1);
                }
            }
            else
            {
                Debug.Log("Error : " + error);
            }
        }
        request.Dispose();
    }

    private bool HasInternetConnection()
    {
        return Application.internetReachability != NetworkReachability.NotReachable;
    }
    private void OnDestroy() {
        StopAllCoroutines();
    }
}
