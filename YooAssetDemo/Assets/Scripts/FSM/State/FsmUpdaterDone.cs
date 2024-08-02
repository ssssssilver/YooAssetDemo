using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EventDefine;

public class FsmUpdaterDone : IStateNode
{
    private StateMachine stateMachine;
    public void OnCreate(StateMachine machine)
    {
        stateMachine = machine;
    }

    public void OnEnter()
    {
        bool needUpdate = (bool)stateMachine.GetBlackboardValue("NeedUpdate");
        PatchProcessDone.SendEventMessage(needUpdate);
    }

    public void OnExit()
    {
    }

    public void OnUpdate()
    {
    }
}
