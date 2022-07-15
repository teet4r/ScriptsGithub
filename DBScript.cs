using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Firebase;
using Firebase.Database;
using System;

public class DBScript : MonoBehaviour
{
    public static DBScript Instance = null;

    protected class User
    {
        public Achievement achievement;

        public User()
        {
            achievement = new Achievement();
        }
    }

    FirebaseApp app = null;
    protected DatabaseReference reference { get; private set; } = null;
    protected DataSnapshot data_snapshot { get; private set; } = null;
    protected User user { get; private set; } = null;
    protected string user_id { get; private set; } = null;
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

    protected virtual void Create()
    {
        user = new User();

        user.achievement.Create();
        //string json = JsonUtility.ToJson(user.achievement.data_int);
        //json = JsonUtility.ToJson(user.achievement.data_bool);
    }

    protected virtual void Read()
    {
        try
        {
            var task = reference.Child(user_id).GetValueAsync();
            data_snapshot = task.Result;
            if (task.IsCanceled)
                Debug.Log("task canceled./DBScript/Read()");
            else if (task.IsFaulted)
                Debug.Log("task faulted./DBScript/Read()");
            else if (!data_snapshot.Exists)
                Create();
            else if (data_snapshot.Exists)
            {
                user = new User();

                user.achievement.Read();

                // 읽기에 성공했으므로 game_connection_cnt++;
                user.achievement.Save(Achievement.Key.TypeInt.game_connected_count, Achievement.Key.TypeInt.type);
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    /// <summary>
    /// for outter class
    /// </summary>
    public void SaveAll()
    {
        _SaveAll();
    }
    protected virtual void _SaveAll()
    {
        if (!init_done || user == null)
        {
            Debug.Log("Save Error./DBScript/_SaveAll()");
            return;
        }

        user.achievement._SaveAll();
    }

    public void Remove()
    {
        if (!init_done || user == null)
        {
            Debug.Log("Remove Error./DBScript/Remove()");
            return;
        }

        reference.Child(user_id).RemoveValueAsync();
    }
    public void Remove(string user_id)
    {
        reference.Child(user_id).RemoveValueAsync();
    }

    void OnApplicationQuit()
    {
        GPGSBinder.Inst.Logout();
    }
}
