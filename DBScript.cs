using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Firebase;
using Firebase.Database;

public class DBScript : MonoBehaviour
{
    public static DBScript Instance = null;

    class User
    {
        public string game_connection_cnt;

        public User(string[] args)
        {
            game_connection_cnt = args[0];
        }
    }

    FirebaseApp app = null;
    DatabaseReference reference = null;
    User user = null;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        Init();
    }

    void Init()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                app = FirebaseApp.DefaultInstance;
                app.Options.DatabaseUrl = new System.Uri("https://dontstopme-aff8f-default-rtdb.asia-southeast1.firebasedatabase.app/");
                reference = FirebaseDatabase.DefaultInstance.RootReference;
                if (!Read(Social.localUser.id))
                    Create(Social.localUser.id);

                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else
            {
                Debug.LogError(string.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }

    void Create(string userID)
    {
        user = new User(new string[] { "1" });
        Debug.Log(user.game_connection_cnt);
        string json = JsonUtility.ToJson(user);
        reference.Child(userID).SetRawJsonValueAsync(json);
    }

    bool Read(string userID)
    {
        bool read_success = false;

        var task = reference.Child(userID).GetValueAsync();
        var dataSnapshot = task.Result;
        if (task.IsCanceled)
            Debug.Log("Error.");
        else if (task.IsFaulted)
            Debug.Log("Error.");
        else if (!dataSnapshot.Exists)
            Debug.Log("나가.");
        else
        {
            int i = 0;
            string[] args = new string[dataSnapshot.ChildrenCount];
            foreach (var data in dataSnapshot.Children)
                args[i++] = data.Value.ToString();
            user = new User(args);
            read_success = true;

            // 읽기에 성공했으므로 game_connection_cnt++;
            user.game_connection_cnt = (int.Parse(user.game_connection_cnt) + 1).ToString();
            Update_(userID, "game_connection_cnt", user.game_connection_cnt);
        }

        return read_success;
    }

    public void Update_(string userID, string key, string new_value)
    {
        if (user == null)
            return;
        reference.Child(userID).UpdateChildrenAsync(ToDictionary(key, new_value));
    }

    public void Remove(string userID)
    {
        if (user == null)
            return;
        reference.Child(userID).RemoveValueAsync();
    }

    Dictionary<string, object> ToDictionary(string key, object value)
    {
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic[key] = value;
        return dic;
    }
}
