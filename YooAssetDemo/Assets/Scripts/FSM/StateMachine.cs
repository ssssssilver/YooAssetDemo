using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Object = System.Object;

public class StateMachine
{
    private readonly Dictionary<string, Object> _blackboard = new Dictionary<string, Object>(100);
    private readonly Dictionary<string, IStateNode> _nodes = new Dictionary<string, IStateNode>(100);

    private IStateNode _currentState;
    private IStateNode _preState;

    public Object Owner { get; private set; }
    public string CurrentState
    {
        get
        {

            return _currentState == null ? string.Empty : _currentState.GetType().Name;
        }
    }
    public string PreviousState
    {
        get
        {
            return _preState == null ? string.Empty : _preState.GetType().Name;
        }
    }
    #region  状态机
    public StateMachine(Object owner)
    {
        Owner = owner;
    }

    public void Update()
    {
        if (_currentState != null)
        {
            _currentState.OnUpdate();
        }
    }

    public void Run<TNode>() where TNode : IStateNode
    {
        var nodeType = typeof(TNode);
        string nodeName = nodeType.FullName;
        Run(nodeName);
    }
    public void Run(Type node)
    {
        string nodeName = node.FullName;
        Run(nodeName);
    }
    public void Run(string nodeName)
    {
        _preState = _currentState;
        _currentState = TryGetNode(nodeName);

        if (_currentState != null)
        {
            _currentState.OnEnter();
        }
        else
        {
            throw new ArgumentException($"The node: {nodeName} does not exist.");
        }
    }

    public void AddNode<TNode>() where TNode : IStateNode
    {
        var nodeType = typeof(TNode);
        var stateNode = Activator.CreateInstance(nodeType) as IStateNode;
        AddNode(stateNode);
    }

    public void AddNode(IStateNode node)
    {
        if (node == null) throw new ArgumentNullException("node");
        var nodeName = node.GetType().FullName;
        if (_nodes.ContainsKey(nodeName))
        {
            throw new ArgumentException("The node has been added.");
        }
        else
        {
            _nodes.Add(nodeName, node);
            node.OnCreate(this);
        }
    }

    private IStateNode TryGetNode(string nodeName)
    {
        _nodes.TryGetValue(nodeName, out IStateNode node);
        return node;
    }

    public void ChangeState<TNode>() where TNode : IStateNode
    {
        var nodeType = typeof(TNode);
        string nodeName = nodeType.FullName;
        ChangeState(nodeName);
    }
    public void ChangeState(Type node)
    {
        string nodeName = node.FullName;
        ChangeState(nodeName);
    }
    public void ChangeState(string nodeName)
    {
        if (string.IsNullOrEmpty(nodeName))
        {
            throw new ArgumentNullException("nodeName");
        }
        IStateNode node = TryGetNode(nodeName);
        if (node == null)
        {
            throw new ArgumentException($"The node: {nodeName} does not exist.");
        }
        else
        {
            if (_currentState != null)
            {
                _preState = _currentState;
                _currentState.OnExit();
            }
            _currentState = node;
            _currentState.OnEnter();
        }
    }

    #endregion


    #region  黑板
    public void SetBlackboard(string key, Object value)
    {
        if (_blackboard.ContainsKey(key))
        {
            _blackboard[key] = value;
        }
        else
        {
            _blackboard.Add(key, value);
        }
    }
    public Object GetBlackboardValue(string key)
    {
        if (_blackboard.ContainsKey(key))
        {
            return _blackboard[key];
        }
        return null;
    }
    #endregion
}
