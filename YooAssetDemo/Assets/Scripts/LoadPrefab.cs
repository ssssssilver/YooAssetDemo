using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;

public class LoadPrefab : MonoBehaviour
{
    private AssetHandle _handle;
    private AssetHandle _audioHandle;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        _handle = YooAssets.LoadAssetAsync<GameObject>("Alien_Fighter");
        yield return _handle;
        GameObject prefab = _handle.InstantiateSync(transform.position - Vector3.back, transform.rotation);
        prefab.name = "呵呵";

        var package = YooAssets.GetPackage("DefaultPackage");
        _audioHandle = package.LoadAssetAsync<AudioClip>("HOSN");
        yield return _audioHandle;
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = _audioHandle.AssetObject as AudioClip;
        audioSource.Play();


    }

    // Update is called once per frame
    void Update()
    {

    }
}
