using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 下载完毕
/// </summary>
internal class FsmDownloadPackageOver : IStateNode
{
    private StateMachine _machine;

    void IStateNode.OnCreate(StateMachine machine)
    {
        _machine = machine;
    }
    void IStateNode.OnEnter()
    {
        _machine.ChangeState<FsmClearPackageCache>();
    }
    void IStateNode.OnUpdate()
    {
    }
    void IStateNode.OnExit()
    {
    }
}