using System.Collections;
using System.Data;
using System.Runtime.InteropServices;
using UnityEngine;
using YooAsset;
using static EventDefine;
/// <summary>
/// 初始化资源包
/// </summary>
internal class FsmInitlizePackage : IStateNode
{
    private StateMachine stateMachine;
    public void OnCreate(StateMachine machine)
    {
        stateMachine = machine;
    }

    public void OnEnter()
    {
        PatchStateChange.SendEventMessage("初始化补丁包");
        GameManager.Instance.StartCoroutine(InitPackage());
    }

    public void OnExit()
    {
    }

    public void OnUpdate()
    {
    }

    private IEnumerator InitPackage()
    {
        yield return 0;
        var playMode = (PlayMode)stateMachine.GetBlackboardValue("PlayMode");
        var packageName = (string)stateMachine.GetBlackboardValue("PackageName");
        var bulidPipeline = (string)stateMachine.GetBlackboardValue("BulidPipeline");

        //创建资源包
        var package = YooAssets.TryGetPackage(packageName);
        if (package == null)
        {
            package = YooAssets.CreatePackage(packageName);
        }
        InitializationOperation initializationOperation = null;
        switch (playMode)
        {
            case PlayMode.EditorSimulateMode://编辑器模拟模式
                var createEditorParam = new EditorSimulateModeParameters();
                createEditorParam.SimulateManifestFilePath = EditorSimulateModeHelper.SimulateBuild(bulidPipeline, packageName);
                initializationOperation = package.InitializeAsync(createEditorParam);
                break;
            case PlayMode.OfflinePlayMode://单机离线模式
                var createOfflineParam = new OfflinePlayModeParameters();
                createOfflineParam.DecryptionServices = new FileStreamDecryption();
                initializationOperation = package.InitializeAsync(createOfflineParam);
                break;
            case PlayMode.HostPlayMode:
                string host = GetHostServerURL();
                string fallbackHost = GetHostServerURL();
                var createHostParam = new HostPlayModeParameters();
                createHostParam.DecryptionServices = new FileStreamDecryption();
                createHostParam.BuildinQueryServices = new GameQueryServices();
                createHostParam.RemoteServices = new RemoteServices(host, fallbackHost);
                initializationOperation = package.InitializeAsync(createHostParam);
                break;
            case PlayMode.WebPlayMode:
                string webHost = GetHostServerURL();
                string webfallbackHost = GetHostServerURL();
                var createWebParam = new WebPlayModeParameters();
                createWebParam.DecryptionServices = new FileStreamDecryption();
                createWebParam.BuildinQueryServices = new GameQueryServices();
                createWebParam.RemoteServices = new RemoteServices(webHost, webfallbackHost);
                break;
        }

        yield return initializationOperation;
        if (initializationOperation.Status == EOperationStatus.Succeed)
        {
            var version = initializationOperation.PackageVersion;
            Debug.Log($"初始化补丁包成功,资源版本号:{version}");
            stateMachine.ChangeState<FsmUpdatePackageVersion>();
        }
        else
        {
            Debug.LogError($"初始化补丁包失败,错误信息:{initializationOperation.Error}");
            EventDefine.InitlizeFail.SendEventMessage();
        }
    }

    /// <summary>
    /// 获取资源服务器地址
    /// </summary>
    private string GetHostServerURL()
    {
        AppSettingScriptableAsset appSetting = Resources.Load("AppSetting") as AppSettingScriptableAsset;
        string hostServerIP = appSetting.hostServerIP;
        string appVersion = appSetting.appVersion;

#if UNITY_EDITOR
        if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.Android)
            return $"{hostServerIP}/CDN/Android/{appVersion}";
        else if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.iOS)
            return $"{hostServerIP}/CDN/IPhone/{appVersion}";
        else if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.WebGL)
            return $"{hostServerIP}/CDN/WebGL/{appVersion}";
        else
            return $"{hostServerIP}/CDN/PC/{appVersion}";
#else
        if (Application.platform == RuntimePlatform.Android)
            return $"{hostServerIP}/CDN/Android/{appVersion}";
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
            return $"{hostServerIP}/CDN/IPhone/{appVersion}";
        else if (Application.platform == RuntimePlatform.WebGLPlayer)
            return $"{hostServerIP}/CDN/WebGL/{appVersion}";
        else
            return $"{hostServerIP}/CDN/PC/{appVersion}";
#endif
    }
}