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
    string user_id = null;
    bool init_done = false;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Init(string user_id)
    {
        if (init_done)
            return;

        init_done = true;
        this.user_id = user_id;
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                app = FirebaseApp.DefaultInstance;
                app.Options.DatabaseUrl = new System.Uri("https://dontstopme-aff8f-default-rtdb.asia-southeast1.firebasedatabase.app/");
                reference = FirebaseDatabase.DefaultInstance.RootReference;
                Read();

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

    void Create()
    {
        user = new User(new string[] { "1" });
        Debug.Log(user.game_connection_cnt);
        string json = JsonUtility.ToJson(user);
        reference.Child(user_id).SetRawJsonValueAsync(json);
    }

    void Read()
    {
        var task = reference.Child(user_id).GetValueAsync();
        var dataSnapshot = task.Result;
        if (task.IsCanceled)
            Debug.Log("Error.");
        else if (task.IsFaulted)
            Debug.Log("Error.");
        else if (!dataSnapshot.Exists)
            Create();
        else if (dataSnapshot.Exists)
        {
            int i = 0;
            string[] args = new string[dataSnapshot.ChildrenCount];
            foreach (var data in dataSnapshot.Children)
                args[i++] = data.Value.ToString();
            user = new User(args);

            // 읽기에 성공했으므로 game_connection_cnt++;
            user.game_connection_cnt = (int.Parse(user.game_connection_cnt) + 1).ToString();
            Update_("game_connection_cnt", user.game_connection_cnt);
        }
    }

    public void Update_(string key, string new_value)
    {
        if (!init_done || user == null)
            return;
        reference.Child(user_id).UpdateChildrenAsync(ToDictionary(key, new_value));
    }

    public void Remove()
    {
        if (!init_done || user == null)
            return;
        reference.Child(user_id).RemoveValueAsync();
    }

    Dictionary<string, object> ToDictionary(string key, object value)
    {
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic[key] = value;
        return dic;
    }

    void OnApplicationQuit()
    {
        GPGSBinder.Inst.Logout();
    }
}
