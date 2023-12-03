using System.Collections.Generic;
using System;
using UnityEngine;

public sealed class FSM
{
    private readonly Dictionary<Enum, IFSMState> _states = new Dictionary<Enum, IFSMState>();
    private readonly FSMParameter _parameter;
    private Enum _currentStateName;
    private IFSMState _currentState;

    private bool Valide => _states.Count > 0;

    public FSM(FSMParameter parameter)
    {
        _parameter = parameter;
    }

    public void Switch(Enum stateName)
    {
        if (!_parameter.running || !_parameter.stateExchangable)
        {
            return;
        }

        if (_currentStateName == stateName)
        {
            return;
        }

        if (!_states.ContainsKey(stateName))
        {
            throw new Exception($"switch fsm state {stateName} is not exist");
        }

        _currentState.OnExit();
        Debug.Log($"FSM state change: <color=#26A0CB>{_currentStateName}</color> -> <color=#CF1919>{stateName}</color>");
        _currentStateName = stateName;
        IFSMState state = _states[stateName];
        state.OnEnter();
        _currentState = state;
    }

    public void Add<T>(IReadOnlyDictionary<T, IFSMState> states) where T : Enum
    {
        if (states == null || states.Count == 0)
        {
            return;
        }
        foreach (var item in states)
        {
            Add(item.Key, item.Value);
        }
    }

    public void Add(Enum stateName, IFSMState state)
    {
        state.OnInit(this);
        if (_states.ContainsKey(stateName))
        {
            return;
        }
        _states.Add(stateName, state);
    }

    public void Del(Enum stateName)
    {
        if (!_states.ContainsKey(stateName) || stateName == _parameter.defaultStateName)
        {
            return;
        }
        IFSMState state = _states[stateName];
        if (state == _currentState)
        {
            _currentState.OnExit();
            Switch(_parameter.defaultStateName);
        }
        _states.Remove(stateName);
    }

    public void Pause(bool pause)
    {
        if (pause)
        {
            OnExit();
        }
        else
        {
            OnStart();
        }
    }

    public void OnStart()
    {
        if (!Valide)
        {
            return;
        }

        _parameter.running = true;
        if (_currentState == null)
        {
            _currentStateName = _parameter.defaultStateName;
            _currentState = _states[_currentStateName];
            return;
        }
        _currentState.OnEnter();
    }

    public void OnExecute()
    {
        if (!Valide || !_parameter.running)
        {
            return;
        }

        _currentState.OnExecute();
    }

    public void OnExit()
    {
        if (!Valide)
        {
            return;
        }

        _parameter.running = false; 
        _currentState.OnExit();
    }

    public void Quit()
    {
        OnExit();
        _states.Clear();
    }

    public T GetParameter<T>() where T : FSMParameter
    {
        return _parameter as T;
    }

    public T GetStateName<T>() where T : Enum
    {
        return (T)_currentStateName;
    }
}