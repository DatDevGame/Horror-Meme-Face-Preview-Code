using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GAction : MonoBehaviour
{
    public string actionName = "Action";
    public float cost = 1.0f;
    public GState[] preConditions;
    public GState[] afterEffects;

    public Dictionary<GStateEnum, int> preconditions;
    public Dictionary<GStateEnum, int> effects;

    public GStateManager agentBelief;
    public bool isRunning = false;

    public GAction()
    {
        preconditions = new Dictionary<GStateEnum, int>();
        effects = new Dictionary<GStateEnum, int>();
    }

    public void Awake()
    {
        if(preConditions != null)
        {
            foreach (GState p in preConditions)
            {
                preconditions.Add(p.key, p.value);
            }
        }

        if (afterEffects != null)
        {
            foreach (GState p in afterEffects)
            {
                effects.Add(p.key, p.value);
            }
        }
        agentBelief = this.GetComponent<GAgent>().belief;
    }

    public bool IsAchievable()
    {
        return true;
    }

    public bool IsAchievableGiven(Dictionary<GStateEnum, int> conditions)
    {
        foreach(KeyValuePair<GStateEnum, int> p in preconditions)
        {
            if(!conditions.ContainsKey(p.Key))
            {
                return false;
            }
        }
        return true;
    }


    public abstract bool PrePerform();
}
