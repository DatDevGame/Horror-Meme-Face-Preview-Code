using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GStateEnum
{
    Init
}
[System.Serializable]
public class GState
{
    public GStateEnum key;
    public int value;
}

public class GStateManager
{
    public Dictionary<GStateEnum, int> states;

    public GStateManager()
    {
        states = new Dictionary<GStateEnum, int>();
    }

    public bool HasState(GStateEnum key)
    {
        return states.ContainsKey(key);
    }

    void AddState(GStateEnum key, int value)
    {
        states.Add(key, value);
    }

    public void ModifyState(GStateEnum key, int value)
    {
        if(states.ContainsKey(key))
        {
            states[key] += value;
            if (states[key] <= 0)
            {
                RemoveState(key);
            }
        }
        else
        {
            states.Add(key, value);
        }
    }

    public void RemoveState(GStateEnum key)
    {
        if (states.ContainsKey(key))
        {
            states.Remove(key);
        }
    }

    public void SetState(GStateEnum key, int value)
    {
        if(states.ContainsKey(key))
        {
            states[key] = value;
        }
        else
        {
            states.Add(key, value);
        }
    }

    public Dictionary<GStateEnum, int> GetState()
    {
        return states;
    }
}
