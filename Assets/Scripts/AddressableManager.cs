using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class AddressableManager : MonoBehaviour
{
    [SerializeField] private RawImage _targetImg;
    private List<AsyncOperationHandle> handles = new List<AsyncOperationHandle>();
    private AudioSource cam;

    private void Start()
    {
        cam = Camera.main.gameObject.GetComponent<AudioSource>();
    }

    public void LoadAssets()
    {
        Addressables.InstantiateAsync("Assets/AddressTest.prefab").Completed += (prefab) =>
        {
            handles.Add(prefab);
            prefab.Result.transform.position = new Vector2(1, -3.5f);
            Debug.Log($"�ڵ� ����Ʈ �߰� : {handles.Count}");
        };

        Addressables.LoadAssetAsync<Texture>("Assets/TestImg.png").Completed += (sprite) =>
        {
            handles.Add(sprite);
            Debug.Log($"�ڵ� ����Ʈ �߰� : {handles.Count}");

            _targetImg.texture = sprite.Result;
        };

        Addressables.LoadAssetAsync<AudioClip>("Assets/sound.wav").Completed += (clip) =>
        {
            handles.Add(clip);
            Debug.Log($"�ڵ� ����Ʈ �߰� : {handles.Count}");

            cam.clip = clip.Result;
            cam.Play();
        };
    }

    public void UnloadAssets()
    {
        if (handles.Count == 0)
        {
            Debug.Log("������ ���� ����");
        }

        foreach (var handle in handles)
        {
            if (handle.Result.GetType() == typeof(GameObject))
            {
                Addressables.ReleaseInstance(handle);
            }
            else
            {
                Addressables.Release(handle);
            }
        }

        handles.Clear();

        Debug.Log($"���� �Ϸ� : {handles.Count}");
    }
}
