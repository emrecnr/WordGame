using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UILoginManager : MonoBehaviour
{   
    public static UILoginManager Instance {get; private set;}

    [Header(" ## Login Manager ##")]
    [SerializeField] private LoginManager loginManager;

    #region ELEMENTS
    [Header(" ## Login - Register Panel - Choose Panel - Nickname Panel ##")]
    #region  Login - Register - Choose Panel - Nickname Panel
    [SerializeField] private GameObject windowLogin;
    [SerializeField] private GameObject windowRegister;
    [SerializeField] private GameObject windowChoose;
    [SerializeField] private GameObject windowNickname;
    #endregion

    [Header(" ## Login Panel Elements ##")]
    #region Login Panel Elements
    [SerializeField] private TMP_InputField windowLoginEmailField;
    [SerializeField] private TMP_InputField windowLoginPasswordField;
    [SerializeField] private TMP_InputField nicknameField;

    [SerializeField] private Button windowLoginButtonLogin;
    [SerializeField] private Button windowLoginButtonRegister;
    #endregion

    [Header(" ## Register Panel Elements ##")]
    #region Register Panel Elements
    [SerializeField] private TMP_InputField windowRegisterEmailField;
    [SerializeField] private TMP_InputField windowRegisterPasswordField;
    [SerializeField] private TMP_InputField windowRegisterConfirmPasswordField;

    [SerializeField] private Button windowRegisterButtonRegister;
    [SerializeField] private Button windowRegisterButtonLogin;
    #endregion

    [Header(" ## Error - Warning Panel Elements ##")]
    #region Error - Complete Panel Elements
    [SerializeField] private GameObject windowError;
    [SerializeField] private GameObject windowComplete;
    [SerializeField] private TMP_Text windowErrorCaption;
    [SerializeField] private TMP_Text windowCompleteCaption;
    [SerializeField] private GameObject windowGuestComplete;

    [SerializeField] private Button windowErrorButtonOk;
    [SerializeField] private Button windowCompleteButtonOk;
    #endregion    
    
    [Header(" ## Forgot Password Elements ##")]
    #region Forgot Password Elements
    [SerializeField] GameObject forgotPasswordPanel;
    [SerializeField] TMP_Text forgotPasswordMessage;
    [SerializeField] TMP_InputField forgotEmailField;
    #endregion
    
    [Header(" ## Settings ##")]
    #region Settings
    private bool showPassword;
    #endregion
    #endregion
    
    private void Awake() {
        if(Instance == null) Instance = this;
        else Destroy(this.gameObject);
    }

    private void Start()
    {
        if(!PlayerPrefs.HasKey("Nickname"))
        {
            windowNickname.SetActive(true);            
        }
        else
        {
            windowChoose.SetActive(true);
            windowLogin.SetActive(false);
            windowRegister.SetActive(false);

            windowError.SetActive(false);
            windowComplete.SetActive(false);
        }

        // Button Events :
        windowLoginButtonLogin.onClick.AddListener(WindowLoginButtonLogin);
        windowLoginButtonRegister.onClick.AddListener(WindowLoginButtonRegister);
        windowRegisterButtonRegister.onClick.AddListener(WindowRegisterButtonRegister);
        windowRegisterButtonLogin.onClick.AddListener(WindowRegisterButtonLogin);

        // TODO: oturum kontrol
    }

    public void ChooseButtonCallback(int value)
    {
        if(value == 0)
        {
            // Guest Login Panel
            LoginManager.Instance.GuestLogin(()=>{
                DatabaseManager.Instance.SaveData(new UserData { UserID = LoginManager.Instance.GetUserId(),Nickname = nicknameField.text });
                windowGuestComplete.SetActive(true);                
            });
        }
        else
        {
            // Login Panel
            windowChoose.SetActive(false);
            OpenLoginPanel();
        }
    }

    public void WindowLoginButtonLogin()
    {
        if (string.IsNullOrEmpty(windowLoginEmailField.text))
        {
            Error("Please enter your user name.");
            return;
        }
        if (string.IsNullOrEmpty(windowLoginPasswordField.text))
        {
            Error("Please enter your password.");
            return;
        }
        LoginManager.Instance.Login(windowLoginEmailField.text,windowLoginPasswordField.text,(()=>
        {
            SceneManager.LoadScene(1);
        }));
    }
    
    public void WindowRegisterButtonRegister()
    {
        
        if(string.IsNullOrEmpty(windowRegisterEmailField.text))
        {
            Error("Please enter your user name.");
            return;
        }
        if (string.IsNullOrEmpty(windowRegisterPasswordField.text))
        {
            Error("Please enter your password.");
            return;
        }

        
        if(windowRegisterPasswordField.text.Length < 8)
        {
            Error("Password has to be at least 8 characters long");
            return;
        }

        
        if(windowRegisterPasswordField.text != windowRegisterConfirmPasswordField.text)
        {
            Error("Password and confirm have to be the same");
            return;
        }
        // LoginManager => Register
        LoginManager.Instance.Register(windowRegisterEmailField.text, windowRegisterPasswordField.text,(()=>
        {
            Complete("Successfully");
            DatabaseManager.Instance.SaveData(new UserData{ UserID = LoginManager.Instance.GetUserId(),Nickname =nicknameField.text });
        }));

    }


    public void WindowLoginButtonRegister()
    {
        windowLogin.SetActive(false);
        windowRegister.SetActive(true);
    }

    public void WindowRegisterButtonLogin()
    {
        windowRegister.SetActive(false);
        windowLogin.SetActive(true);
    }

    public void OpenLoginPanel()
    {
        forgotPasswordPanel.SetActive(false);
        windowRegister.SetActive(false);
        windowLogin.SetActive(true);
    }

    public void OpenForgotPasswordPanel()
    {
        windowRegister.SetActive(false);
        windowLogin.SetActive(true);
        forgotPasswordPanel.SetActive(true);
    }

    public void ResetPassword()
    {
        LoginManager.Instance.ResetPassword(forgotEmailField.text,()=>{
            forgotPasswordMessage.gameObject.SetActive(true);
        });
    }

    public void ShowPassword()
    {
        showPassword = !showPassword;
        windowLoginPasswordField.contentType = showPassword ? TMP_InputField.ContentType.Standard : TMP_InputField.ContentType.Password;
        windowLoginPasswordField.ForceLabelUpdate();
    }

    public void Error(string message)
    {
        windowErrorButtonOk.onClick.RemoveAllListeners();
        windowErrorButtonOk.onClick.AddListener(()=> windowError.SetActive(false));
        windowError.transform.SetAsLastSibling();

        windowErrorCaption.text = message;
        windowError.SetActive(true);
    }

    public void Complete(string message)
    {
        windowCompleteButtonOk.onClick.RemoveAllListeners();
        windowCompleteButtonOk.onClick.AddListener(() =>{ 
            windowComplete.SetActive(false);
            windowRegister.SetActive(false);
            windowLogin.SetActive(true);
        });
        windowComplete.transform.SetAsLastSibling();

        windowCompleteCaption.text = message;
        windowComplete.SetActive(true);
    }

    public void NextScene()
    {
        SceneManager.LoadScene(1);
    }
    public void NicknamePanelNextButtonCallback()
    {
        if(string.IsNullOrEmpty(nicknameField.text))
        {
            Debug.Log("Choose Your Nickname");
            return;
        }
        else if (nicknameField.text.Length < 3)
        {
            Debug.Log("Minumum 3 character");
            return;
        } 

        windowNickname.SetActive(false);
        windowChoose.SetActive(true);
        PlayerPrefs.SetString("Nickname",nicknameField.text);
    }
}
