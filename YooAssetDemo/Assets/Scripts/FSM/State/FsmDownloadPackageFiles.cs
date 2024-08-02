using System;
using System.Collections;
using System.Collections.Generic;
using UniFramework.Event;
using Unity.VisualScripting;
using UnityEngine;
using YooAsset;

public class FsmDownloadPackageFiles : IStateNode
{
    private StateMachine stateMachine;
    public void OnCreate(StateMachine machine)
    {
        stateMachine = machine;
    }

    public void OnEnter()
    {
        EventDefine.PatchStateChange.SendEventMessage("下载资源包文件");
        GameManager.Instance.StartCoroutine(BeginDownload());
    }

    public void OnExit()
    {
    }

    public void OnUpdate()
    {
    }

    private IEnumerator BeginDownload()
    {
        var downloader = stateMachine.GetBlackboardValue("Downloader") as ResourceDownloaderOperation;
        downloader.OnDownloadErrorCallback = EventDefine.WebFileDownloadFailed.SendEventMessage;
        downloader.OnDownloadProgressCallback = EventDefine.DownloadProgressUpdate.SendEventMessage;
        downloader.BeginDownload();
        yield return downloader;

        if (downloader.Status == EOperationStatus.Succeed)
        {
            stateMachine.ChangeState<FsmUpdaterDone>();
        }
        else
        {
            Debug.LogError(downloader.Error);
        }
    }
}
