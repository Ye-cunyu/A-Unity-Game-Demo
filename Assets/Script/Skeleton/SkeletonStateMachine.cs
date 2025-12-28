using UnityEngine;

public class SkeletonStateMachine
{
    public SkeletonState currentState { get; private set; }
    public void Initialize(SkeletonState _startState)
    {
        currentState = _startState;
        currentState.Enter();
    }
    public void ChangeState(SkeletonState _newState)
    {
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }
}

