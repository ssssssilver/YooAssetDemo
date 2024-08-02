using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;
/// <summary>
/// 更新资源清单
/// </summary>
public class FsmUpdatePackageManifest : IStateNode
{
    private StateMachine stateMachine;
    public void OnCreate(StateMachine machine)
    {
        stateMachine = machine;
    }

    public void OnEnter()
    {
        EventDefine.PatchStateChange.SendEventMessage("更新资源清单");
        GameManager.Instance.StartCoroutine(UpdateMainifest());
    }

    public void OnExit()
    {
    }

    public void OnUpdate()
    {
    }

    private IEnumerator UpdateMainifest()
    {
        yield return 0;
        string packageName = stateMachine.GetBlackboardValue("PackageName") as string;
        string packageVersion = stateMachine.GetBlackboardValue("PackageVersion") as string;
        var package = YooAssets.GetPackage(packageName);
        bool savePackVersion = true;
        var operation = package.UpdatePackageManifestAsync(packageVersion, savePackVersion);
        yield return operation;

        if (operation.Status == EOperationStatus.Succeed)
        {

            stateMachine.ChangeState<FsmCreatePackageDownloader>();
        }
        else
        {
            Debug.LogError(operation.Error);
            EventDefine.PatchManifestUpdateFailed.SendEventMessage();
        }
    }


}
