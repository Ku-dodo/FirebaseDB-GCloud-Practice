using Firebase.Storage;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.UI;


public class DownloadStorageBtn : MonoBehaviour
{
    private Button _btn;
    private FirebaseStorage _firebaseStorage;
    private StorageReference _storageRef;
    private StorageReference _imageRef;

    [SerializeField] private RawImage _image;

    private void Awake()
    {
        _firebaseStorage = FirebaseStorage.DefaultInstance;
        _storageRef = _firebaseStorage.GetReferenceFromUrl("gs://ku-test-project.appspot.com/");
        _imageRef = _storageRef.Child("Test.jpg");
    }

    private void Start()
    {
        _btn = GetComponent<Button>();
        _btn.onClick.AddListener(() => Download());
    }

    private void Download()
    {
        _imageRef.GetFileAsync(Application.persistentDataPath + "/Test.jpg").ContinueWithOnMainThread(task =>
       {
           if (!task.IsFaulted && !task.IsCanceled)
           {
               Debug.Log("Download Complet! : " + Application.persistentDataPath);
           }
           else
           {
               Debug.Log(task.Exception);
           }
       });  
    }
}
