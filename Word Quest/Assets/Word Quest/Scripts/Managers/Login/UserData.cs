
using System.Collections.Generic;
using Firebase.Firestore;


public class UserData 
{
    private string userID;
    private string userNickname;
    private int score;
    private int coin = 500;
    private int wonCount; 
    private int loseCount;
    private int hintKeyboardCount;
    private int hintLetterCount;
    private int removeAds;

    public string UserID {get{return userID;} set{userID = value;}}
    public string Nickname {get{return userNickname; } set{ userNickname = value;}}
    public int Score {get{return score; } set{ score = value;}}
    public int Coin {get{return coin; } set{ coin = value;}}
    public int WonCount {get{return wonCount; } set{ wonCount = value;}}
    public int LoseCount {get{return loseCount; } set{ loseCount = value;}}
    public int HintKeyboardCount { get { return hintKeyboardCount; } set { hintKeyboardCount = value; } }
    public int HintLetterCount { get { return hintLetterCount; } set { hintLetterCount = value; } }
    public int RemoveAds { get { return removeAds; } set { removeAds = value; } }




    public UserData()
    {
        // Default const required for Firestore serialization
    }

    public UserData(DocumentSnapshot snapshot)
    {   
        // Constructor to create UserData object from a DocumentSnapshot

        Dictionary<string, object> data = snapshot.ToDictionary();

        //UserID = (string)data["UserID"];
        Nickname = (string)data["Nickname"];
        Score = int.Parse(data["Score"].ToString());
        Coin = int.Parse(data["Coin"].ToString());
        WonCount = int.Parse(data["WonCount"].ToString());
        LoseCount = int.Parse(data["LoseCount"].ToString());
        HintKeyboardCount = int.Parse(data["HintKeyboardCount"].ToString());
        HintLetterCount = int.Parse(data["HintLetterCount"].ToString());
        RemoveAds = int.Parse(data["RemoveAds"].ToString());
        
    }

    public Dictionary<string, object> ToDictionary()
    {
        // Convert UserData object to a dictionary for Firestore

        Dictionary<string, object> data = new Dictionary<string, object>();

        data["Nickname"] = userNickname;
        data["Score"] = score; 
        data["Coin"] = coin;
        data["WonCount"] = wonCount;
        data["LoseCount"] = loseCount;
        data["HintKeyboardCount"] = hintKeyboardCount;
        data["HintLetterCount"] = hintLetterCount;
        data["RemoveAds"] = removeAds;
        
        return data;
    }
}
