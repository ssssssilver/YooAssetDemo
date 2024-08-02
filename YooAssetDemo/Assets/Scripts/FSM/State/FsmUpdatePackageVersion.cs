using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;
/// <summary>
/// 更新资源版本号
/// </summary>
public class FsmUpdatePackageVersion : IStateNode
{
    private StateMachine stateMachine;
    public void OnCreate(StateMachine machine)
    {
        stateMachine = machine;
    }

    public void OnEnter()
    {
        EventDefine.PatchStateChange.SendEventMessage("更新资源版本号");
        GameManager.Instance.StartCoroutine(UpdatePackageVersion());
    }

    public void OnExit()
    {
    }

    public void OnUpdate()
    {
    }

    private IEnumerator UpdatePackageVersion()
    {
        yield return 0;
        string packageName = stateMachine.GetBlackboardValue("PackageName") as string;
        var package = YooAssets.GetPackage(packageName);
        var operation = package.UpdatePackageVersionAsync();

        yield return operation;
        if (operation.Status == EOperationStatus.Succeed)
        {
            stateMachine.SetBlackboard("PackageVersion", operation.PackageVersion);
            stateMachine.ChangeState<FsmUpdatePackageManifest>();
        }
        else
        {
            Debug.LogError(operation.Error);
            EventDefine.PackageVersionUpdateFailed.SendEventMessage();
        }
    }

}
