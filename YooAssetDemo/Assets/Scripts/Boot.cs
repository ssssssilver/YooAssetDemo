using System.Collections;
using System.IO;
using UnityEngine;
using YooAsset;

public class Boot : MonoBehaviour
{
    [SerializeField]
    AppSettingScriptableAsset appSettingScriptableAsset;
    [SerializeField]
    private PlayMode playMode = PlayMode.EditorSimulateMode;

    private void Awake()
    {
        Debug.Log("运行模式：" + playMode);
        DontDestroyOnLoad(this.gameObject);
        GameManager.Instance.SetMonoBehaviour(this);
    }
    WaitDone waitDone;
    IEnumerator Start()
    {
        yield return 0;
        //初始化资源系统
        YooAssets.Initialize();
        //加载更新界面
        GameObject patchGO = Resources.Load<GameObject>("PatchUI");
        if (patchGO != null)
        {
            Instantiate(patchGO);
            //开始更新流程
            PatchOperation operation = new PatchOperation("DefaultPackage", "BuiltinBuildPipeline", playMode);
            YooAssets.StartOperation(operation);
            yield return operation;
            Debug.Log("更新流程结束");
            //设置默认包
            var package = YooAssets.GetPackage("DefaultPackage");
            YooAssets.SetDefaultPackage(package);
            YooAssets.LoadSceneAsync("TestLoad");
        }
        else
        {
            Debug.LogError("PatchGameobject is null");
        }
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            waitDone.isDone = false;
        }
    }

    #region 本地资源测试
    // IEnumerator Start()
    // {
    //     YooAssets.Initialize();
    //     var package = YooAssets.CreatePackage("DefaultPackage");
    //     yield return InitializeLocalYooAsset(package);


    //     YooAssets.SetDefaultPackage(package);
    //     Debug.Log(2);
    //     YooAssets.LoadSceneAsync("TestLoad");

    // }
    // /// <summary>
    // /// 本地资源包初始化
    // /// </summary>
    // /// <param name="package"></param>
    // /// <returns></returns>
    // private IEnumerator InitializeLocalYooAsset(ResourcePackage package)
    // {

    //     var initParameters = new EditorSimulateModeParameters();
    //     var simulateManifestFilePath = EditorSimulateModeHelper.SimulateBuild(EDefaultBuildPipeline.BuiltinBuildPipeline, "DefaultPackage");
    //     initParameters.SimulateManifestFilePath = simulateManifestFilePath;
    //     yield return package.InitializeAsync(initParameters);
    // }
    #endregion
}
/// <summary>
/// 运行模式
/// </summary>
public enum PlayMode
{
    /// <summary>
    /// 编辑器下的模拟模式
    /// </summary>
    EditorSimulateMode,

    /// <summary>
    /// 离线运行模式
    /// </summary>
    OfflinePlayMode,

    /// <summary>
    /// 联机运行模式
    /// </summary>
    HostPlayMode,

    /// <summary>
    /// WebGL运行模式
    /// </summary>
    WebPlayMode,
}