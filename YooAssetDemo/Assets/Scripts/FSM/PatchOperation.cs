using System.Collections;
using System.Collections.Generic;
using UniFramework.Event;
using UnityEngine;
using YooAsset;
using static EventDefine;

public class PatchOperation : GameAsyncOperation
{
    public enum State
    {
        None,
        Update,
        Done
    }
    private readonly StateMachine stateMachine;
    private State currentState = State.None;

    public PatchOperation(string packageName, string bulidPipeline, PlayMode playMode)
    {
        this.AddListener<UserInitlize>(OnUserInitlize);
        this.AddListener<BeginDownload>(OnBeginDownload);
        this.AddListener<UpdatePackageVersion>(OnUpdatePackageVersion);
        this.AddListener<UpdatePatchMainifest>(OnUpdatePatchMainifest);
        this.AddListener<DownloadFile>(OnDownloadFile);
        this.AddListener<PatchProcessExit>(OnPatchProcessExit);

        stateMachine = new StateMachine(this);
        stateMachine.AddNode<FsmInitlizePackage>();
        stateMachine.AddNode<FsmUpdatePackageVersion>();
        stateMachine.AddNode<FsmUpdatePackageManifest>();
        stateMachine.AddNode<FsmCreatePackageDownloader>();
        stateMachine.AddNode<FsmDownloadPackageFiles>();
        stateMachine.AddNode<FsmDownloadPackageOver>();
        stateMachine.AddNode<FsmUpdaterDone>();
        stateMachine.AddNode<FsmClearPackageCache>();

        stateMachine.SetBlackboard("PackageName", packageName);
        stateMachine.SetBlackboard("BulidPipeline", bulidPipeline);
        stateMachine.SetBlackboard("PlayMode", playMode);


    }
    protected override void OnAbort()
    {
    }

    protected override void OnStart()
    {
        currentState = State.Update;
        stateMachine.Run<FsmInitlizePackage>();
    }


    protected override void OnUpdate()
    {
        if (currentState != State.Update)
            return;
        stateMachine.Update();
    }

    private void OnUserInitlize(UserInitlize userInitlize)
    {
        stateMachine.ChangeState<FsmInitlizePackage>();
    }
    private void OnBeginDownload(BeginDownload beginDownload)
    {
        stateMachine.ChangeState<FsmDownloadPackageFiles>();
    }
    private void OnUpdatePackageVersion(UpdatePackageVersion updatePackageVersion)
    {
        stateMachine.ChangeState<FsmUpdatePackageVersion>();
    }
    private void OnUpdatePatchMainifest(UpdatePatchMainifest updatePatchMainifest)
    {
        stateMachine.ChangeState<FsmUpdatePackageManifest>();
    }
    private void OnDownloadFile(DownloadFile downloadFile)
    {
        stateMachine.ChangeState<FsmDownloadPackageFiles>();
    }
    private void OnPatchProcessExit(PatchProcessExit patchProcessExit)
    {
        //状态机结束
        Debug.Log("PatchProcessExit");

        currentState = State.Done;
        Status = EOperationStatus.Succeed;
    }

}
