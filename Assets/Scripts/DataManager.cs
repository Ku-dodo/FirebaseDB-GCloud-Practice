using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class DataManager : MonoBehaviour
{
    [Header("Data Upload")]
    [SerializeField] InputField _userName;
    [SerializeField] InputField _item1;
    [SerializeField] InputField _item2;

    [Header("Data Download")]
    [SerializeField] InputField _searchUserName;
    [SerializeField] Text _itemsText;



    DatabaseReference reference;
    // Start is called before the first frame update
    void Start()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void WriteNewUser()
    {
        User user = new User(_userName.text, _item1.text, _item2.text);
        string userName = JsonUtility.ToJson(user);
        string userInventory = JsonUtility.ToJson(user.inventory);

        reference.Child("users").Child(_userName.text).SetRawJsonValueAsync(userName);
        reference.Child("users").Child(_userName.text).Child("inventory").SetRawJsonValueAsync(userInventory);
        Debug.Log("저장!");
    }

    public async void ReadUser()
    {
        Inventory _inventory = null;

        try
        {
            DataSnapshot snapshot = await reference.Child("users").Child(_searchUserName.text).Child("inventory").GetValueAsync();
            string json = snapshot.GetRawJsonValue();
            _inventory = JsonUtility.FromJson<Inventory>(json);
        }
        catch (Exception e)
        {
            Debug.Log(e);
            return;
        }

        if (_inventory == null)
        {
            Debug.Log("정의되지 않은 데이터");
            return;
        }

        StringBuilder sb = new StringBuilder();
        foreach (var item in _inventory.items)
        {
            sb.AppendLine($"{item}");
        }
        _itemsText.text = sb.ToString();
    }
}

public class User
{
    public string name;
    public Inventory inventory = new Inventory();

    public User(string name, string item1, string item2)
    {
        this.name = name;
        inventory.items.Add(item1);
        inventory.items.Add(item2);
    }
}

public class Inventory
{
    public List<string> items = new List<string>();
}