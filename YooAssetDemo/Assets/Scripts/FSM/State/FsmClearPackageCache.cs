using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;
/// <summary>
/// 清理缓存文件
/// </summary>
public class FsmClearPackageCache : IStateNode
{
    private StateMachine stateMachine;
    public void OnCreate(StateMachine machine)
    {
        stateMachine = machine;
    }

    public void OnEnter()
    {
        EventDefine.PatchStateChange.SendEventMessage("清理缓存文件");
        string packageName = stateMachine.GetBlackboardValue("PackageName") as string;
        var package = YooAssets.GetPackage(packageName);
        var operation = package.ClearUnusedCacheFilesAsync();
        operation.Completed += OnComplete;
    }

    public void OnExit()
    {
    }

    public void OnUpdate()
    {
    }

    private void OnComplete(AsyncOperationBase asyncOperationBase)
    {
        stateMachine.ChangeState<FsmUpdaterDone>();
    }
}
