using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;
/// <summary>
/// 创建资源包下载器
/// </summary>
public class FsmCreatePackageDownloader : IStateNode
{
    private StateMachine stateMachine;
    public void OnCreate(StateMachine machine)
    {
        stateMachine = machine;
    }

    public void OnEnter()
    {
        EventDefine.PatchStateChange.SendEventMessage("创建资源包下载器");
        GameManager.Instance.StartCoroutine(CreateDownloader());
    }

    public void OnExit()
    {
    }

    public void OnUpdate()
    {
    }
    IEnumerator CreateDownloader()
    {
        yield return 0;
        string packageName = stateMachine.GetBlackboardValue("PackageName") as string;
        var package = YooAssets.GetPackage(packageName);
        int downloadMaxNum = 10;
        int failTrayAgainTimes = 3;
        var downloader = package.CreateResourceDownloader(downloadMaxNum, failTrayAgainTimes);
        stateMachine.SetBlackboard("Downloader", downloader);
        if (downloader.TotalDownloadCount == 0)
        {
            Debug.Log("资源包已经是最新版本");
            stateMachine.SetBlackboard("NeedUpdate", false);
            stateMachine.ChangeState<FsmUpdaterDone>();

        }
        else
        {
            //发现更新文件 挂起流程
            stateMachine.SetBlackboard("NeedUpdate", true);
            int TotalDownloadCount = downloader.TotalDownloadCount;
            long TotalDownloadCountBytes = downloader.TotalDownloadBytes;
            string version = package.GetPackageVersion();
            EventDefine.FoundUpdateFiles.SendEventMessage(version, TotalDownloadCount, TotalDownloadCountBytes);
            //todo:创建下载器后如何切换下个状态机
        }

    }
}
