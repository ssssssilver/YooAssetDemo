using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStateNode
{
    void OnCreate(StateMachine machine);
    void OnEnter();
    void OnUpdate();
    void OnExit();

}
