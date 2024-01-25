using System;
using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Extensions;
using Firebase.Firestore;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager Instance{get; private set;} 

    private FirebaseFirestore firestorDatabase;


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            StartCoroutine(CheckFirestore());
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void SaveData(UserData user)
    {
        StartCoroutine(CreateUser(user));
    }

    public  void LoadData(string userID, Action<UserData> onReaded)
    {
        StartCoroutine(ReadUser(userID, onReaded));
    }

    public IEnumerator CreateUser(UserData user)
    {
        // Get a reference to the "users" collection
        CollectionReference usersRef = firestorDatabase.Collection("users");

        var saveTask = usersRef.Document(user.UserID.ToString()).SetAsync(user.ToDictionary());

        yield return new WaitUntil(()=> saveTask.IsCompleted);

        if(saveTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {saveTask.Exception}");
        }
        else
        {
            Debug.Log("Complete!");
        }
    }

    public IEnumerator ReadUser(string userID, Action<UserData> OnRead)
    {
        // Get a reference to the "users" collection
        CollectionReference usersRef = firestorDatabase.Collection("users");

        // Get the document reference for the specified userId
        DocumentReference userDocumentReference = usersRef.Document(userID.ToString());

        var loadTask = userDocumentReference.GetSnapshotAsync();

        yield return new WaitUntil(() => loadTask.IsCompleted);

        if (loadTask.Exception != null)
        {
            Debug.LogError(loadTask.Exception);
        }
        else 
        {
            DocumentSnapshot snapshot = loadTask.Result;

            if (snapshot.Exists)
            {
                // Convert the document data to a UserData object
                UserData user = new UserData(snapshot);
                OnRead?.Invoke(user);
            }
            else
            {
                Debug.Log("User not found.");
            }
        }       
    }

    IEnumerator CheckFirestore()
    {
        var initializeTask = FirebaseApp.CheckAndFixDependenciesAsync();

        yield return new WaitUntil(()=> initializeTask.IsCompleted);

        if(initializeTask.Exception != null)
        {
            Debug.LogWarning(initializeTask.Exception);
        }
        else 
        {
            if (initializeTask.Result == DependencyStatus.Available)
            {
                firestorDatabase = FirebaseFirestore.DefaultInstance;
                Debug.Log("Firebase Firestore initialized.");
            }
            else
            {
                Debug.LogError("Firebase initialization failed.");
            }
        }      
    }
}
