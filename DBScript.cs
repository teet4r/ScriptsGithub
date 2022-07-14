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
        public Achievement achievement;

        public User()
        {
            achievement = new Achievement();
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

        Init("g13044343168255153369");
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
        user = new User();

        //string json = JsonUtility.ToJson(user.achievement.data_int);
        reference.Child(user_id).Child("achievement").Child("int").SetValueAsync(user.achievement.data_int);
        //json = JsonUtility.ToJson(user.achievement.data_bool);
        reference.Child(user_id).Child("achievement").Child("bool").SetValueAsync(user.achievement.data_bool);
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
            user = new User();

            var achievement_data = dataSnapshot.Child("achievement").Child("int");
            foreach (var d in achievement_data.Children)
                user.achievement.UpdateUser(d.Key, (int)d.Value); // 언박싱하는 과정에서 터짐
            achievement_data = dataSnapshot.Child("achievement").Child("bool");
            foreach (var d in achievement_data.Children)
                user.achievement.UpdateUser(d.Key, (bool)d.Value);

            // 읽기에 성공했으므로 game_connection_cnt++;
            const string key = Achievement.Key.TypeInt.game_connected_count;
            user.achievement.UpdateUser(key, 1);
            UpdateAchievement<int>(key);
        }
    }

    public void UpdateAchievement<T>(string key)
    {
        if (!init_done || user == null)
            return;
        if (typeof(T) == typeof(int) && user.achievement.data_int.ContainsKey(key))
        {
            reference.Child(user_id).Child("achievement").Child("int").UpdateChildrenAsync(ToDictionary(key, user.achievement.data_int[key]));
            return;
        }
        else if (typeof(T) == typeof(bool) && user.achievement.data_bool.ContainsKey(key))
        {
            reference.Child(user_id).Child("achievement").Child("bool").UpdateChildrenAsync(ToDictionary(key, user.achievement.data_bool[key]));
            return;
        }
    }

    public void Remove()
    {
        if (!init_done || user == null)
            return;
        reference.Child(user_id).RemoveValueAsync();
    }
    public void Remove(string user_id)
    {
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
