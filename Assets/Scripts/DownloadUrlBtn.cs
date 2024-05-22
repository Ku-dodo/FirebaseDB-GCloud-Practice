using System.Collections;
using Firebase.Storage;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class DownloadUrlBtn : MonoBehaviour
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
        _imageRef.GetDownloadUrlAsync().ContinueWithOnMainThread(task =>
       {
           if (!task.IsFaulted && !task.IsCanceled)
           {
               StartCoroutine(LoadImage(task.Result.ToString()));
           }
           else
           {
               Debug.Log(task.Exception);
           }
       });
        
    }
    private IEnumerator LoadImage(string imageName)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageName);
        yield return request.SendWebRequest();
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            _image.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
        }
    }
}
