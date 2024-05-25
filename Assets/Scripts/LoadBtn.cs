using Firebase.Database;
using Firebase.Extensions;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class LoadBtn : MonoBehaviour
{
    private Button _btn;
    DatabaseReference reference;
    Inventory inventory;
    [SerializeField] InputField _userName;
    [SerializeField] Text _items;

    private void Start()
    {
        _btn = GetComponent<Button>();
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        _btn.onClick.AddListener(ReadUser);
    }

    private async void ReadUser()
    {
        await reference.Child("users").Child(_userName.text).Child("inventory").GetValueAsync().ContinueWithOnMainThread(task =>
       {
           if (task.IsFaulted)
           {
               Debug.Log($"실패! :{task.Result}");
           }
           else if (task.IsCompleted)
           {
               DataSnapshot snapshot = task.Result;
               string json = snapshot.GetRawJsonValue();
               inventory = JsonUtility.FromJson<Inventory>(json);
           }
       });

        if (inventory == null)
        {
            Debug.Log("정의되지 않은 유저 이름과 인벤토리");
            return;
        }

        StringBuilder sb = new StringBuilder();
        foreach (var item in inventory.items)
        {
            sb.AppendLine($"{item}");
        }
        _items.text = sb.ToString();
    }
}
