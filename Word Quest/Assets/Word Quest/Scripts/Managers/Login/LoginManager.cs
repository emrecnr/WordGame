using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Firebase;
using Firebase.Auth;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    public static LoginManager Instance { get; private set; }

    [Header("Firebase")]
    public DependencyStatus _dependencyStatus;

    private FirebaseAuth _fbAuth;
    public FirebaseUser user;
    public static string userId;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        StartCoroutine(CheckAndInitialize());
    }


    private void InitializeFirebase()
    {
        _fbAuth = FirebaseAuth.DefaultInstance;

        _fbAuth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    public void Login(string email, string password, Action onLogin)
    {
        StartCoroutine(LoginAsync(email, password, onLogin));
    }

    public void Register(string email, string password, Action onRegister)
    {
        StartCoroutine(RegisterAsync(email, password, onRegister));
    }
   
    private void AutoLogin()
    {
        if (user != null)
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            UILoginManager.Instance.OpenLoginPanel();
        }
    }

    public void Logout()
    {
        if (_fbAuth != null && user != null)
        {
            PlayerPrefs.DeleteKey("Nickname");
            _fbAuth.SignOut();
        }
    }

    public void ResetPassword(string email,Action onReset)
    {
        StartCoroutine(ResetPasswordAsync(email,onReset));
    }

    public void GuestLogin(Action onGuestLogin)
    {
        StartCoroutine(GuestLoginAsync(onGuestLogin));
    }

    private IEnumerator GuestLoginAsync(Action onGuestLogin)
    {
        var guestTask = _fbAuth.SignInAnonymouslyAsync();

        yield return new WaitUntil(()=> guestTask.IsCompleted);

        AuthResult result = guestTask.Result;
        user = result.User;
        userId = user.UserId;
        PlayerPrefs.SetString("UserId",userId);
        onGuestLogin?.Invoke();
    }

    private IEnumerator CheckForAutoLogin()
    {
        if (user != null)
        {
            var reloadUserTask = user.ReloadAsync();

            yield return new WaitUntil(() => reloadUserTask.IsCompleted);

            userId = user.UserId;
            if(userId == null) userId = PlayerPrefs.GetString("UserId");
            AutoLogin();
        }
        else
        {
            UILoginManager.Instance.OpenLoginPanel();
        }
    }

    private IEnumerator CheckAndInitialize()
    {
        var dependencyStatus = FirebaseApp.CheckAndFixDependenciesAsync();

        yield return new WaitUntil(() => dependencyStatus.IsCompleted);

        _dependencyStatus = dependencyStatus.Result;

        if (_dependencyStatus == DependencyStatus.Available)
        {
            InitializeFirebase();
            yield return new WaitForEndOfFrame();
            StartCoroutine(CheckForAutoLogin());
        }
        else
        {
            Debug.LogError("Could not resolve all firebase dependencies: " + _dependencyStatus);
        }
    }

    private IEnumerator LoginAsync(string email, string password, Action onLogin)
    {
        var loginTask = _fbAuth.SignInWithEmailAndPasswordAsync(email, password);

        yield return new WaitUntil(() => loginTask.IsCompleted);

        if (loginTask.Exception != null)
        {
            Debug.LogError(loginTask.Exception);

            FirebaseException firebaseException = loginTask.Exception.GetBaseException() as FirebaseException;
            AuthError authError = (AuthError)firebaseException.ErrorCode;

            string failedMessage = "Login Failed! Becase ";
            switch (authError)
            {
                case AuthError.InvalidEmail:
                    failedMessage += "Email is invalid";
                    break;
                case AuthError.WrongPassword:
                    failedMessage += "Wrong Password";
                    break;
                case AuthError.MissingEmail:
                    failedMessage += "Email is missing";
                    break;
                case AuthError.MissingPassword:
                    failedMessage += "Password is missing";
                    break;
                default:
                    failedMessage = "Login Failed";
                    break;
            }
            Debug.Log(failedMessage);
        }
        else
        {
            AuthResult result = loginTask.Result;
            if (result.User != null)
            {
                FirebaseUser user = result.User;
                userId = user.UserId;
                onLogin?.Invoke();
            }
        }
    }

    private IEnumerator RegisterAsync(string email, string password, Action onRegister)
    {
        var registerTask = _fbAuth.CreateUserWithEmailAndPasswordAsync(email, password);

        yield return new WaitUntil(() => registerTask.IsCompleted);

        if (registerTask.Exception != null)
        {
            Debug.LogError(registerTask.Exception);

            FirebaseException firebaseException = registerTask.Exception.GetBaseException() as FirebaseException;
            AuthError authError = (AuthError)firebaseException.ErrorCode;

            string failedMessage = "Registration Failed! Because ";
            switch (authError)
            {
                case AuthError.InvalidEmail:
                    failedMessage += "Email is invalid.";
                    break;
                case AuthError.WrongPassword:
                    failedMessage += "Wrong Password.";
                    break;
                case AuthError.MissingEmail:
                    failedMessage += "Email is missing.";
                    break;
                case AuthError.MissingPassword:
                    failedMessage += "Password is missing.";
                    break;

                default:
                    failedMessage += "Registration Failed.";
                    break;
            }
            Debug.Log(failedMessage);
        }
        else
        {
            AuthResult result = registerTask.Result;
            user = result.User;
            userId = user.UserId;
            Debug.Log("Registration Successful Welcome " + user.Email);
            onRegister?.Invoke();

        }
    }

   
    IEnumerator ResetPasswordAsync(string email,Action onReset)
    {
        var resetTask = _fbAuth.SendPasswordResetEmailAsync(email);

        yield return new WaitUntil(()=> resetTask.IsCompleted);

        if(resetTask.Exception != null)
        {
            Debug.LogError(resetTask.Exception);
        }
        else
        {
            // Notification
            onReset?.Invoke();
        }        
    }

    public string GetUserId()
    {
        return userId;
    }

    private void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (_fbAuth.CurrentUser != user)
        {
            bool signedIn = user != _fbAuth.CurrentUser && _fbAuth.CurrentUser != null;

            if (!signedIn && user != null)
            {
                Debug.Log("Signed out " + user.UserId);
            }

            user = _fbAuth.CurrentUser;

            if (signedIn)
            {
                Debug.Log("Signed in " + user.UserId);
            }
        }
    }

}
