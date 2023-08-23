using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Assets._SDK.GOAP;

public class SubGoal
{
    public Dictionary<GStateEnum, int> subGoals;
    public bool remove;

    public SubGoal(GStateEnum s, int v, bool r)
    {
        subGoals = new Dictionary<GStateEnum, int>();
        subGoals.Add(s, v);
        remove = r;
    }
}

public class GAgent : MonoBehaviour
{
    public List<GAction> actions = new List<GAction>();
    public Dictionary<SubGoal, int> goals = new Dictionary<SubGoal, int>();
    public GStateManager belief = new GStateManager();
    public GStateManager world;

    GPlanner planner;
    protected Queue<GAction> actionQueue;
    public GAction currentAction;

    SubGoal currentGoal;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        GAction[] acts = this.GetComponents<GAction>();
        world = GWorld.Instance.stateManager;

        foreach(GAction a in acts)
        {
            actions.Add(a);
        }
    }

    // Update is called once per frame
    protected virtual void LateUpdate()
    {
        if(currentAction != null && currentAction.isRunning)
        {
            UpdateRunningAction();
            return;
        }

        if (planner == null || actionQueue == null)
        {
            planner = new GPlanner();
            var sortedGoals = from entry in goals orderby entry.Value descending select entry;
            foreach(KeyValuePair<SubGoal, int> sg in sortedGoals)
            {
                actionQueue = planner.Plan(world, actions, sg.Key.subGoals, belief);
                if(actionQueue != null)
                {
                    currentGoal = sg.Key;
                    break;
                }
            }
            return;
        }

        if(actionQueue != null && actionQueue.Count == 0)
        {
            if(currentGoal.remove)
            {
                goals.Remove(currentGoal);
            }
            planner = null;
            return;
        }

        if (actionQueue != null && actionQueue.Count > 0)
        { 
            currentAction = actionQueue.Dequeue();
            if(currentAction.PrePerform())
            {
                currentAction.isRunning = true;
            }
            else
            {
                actionQueue = null;
            }
            return;
        }
    }

    public virtual void UpdateRunningAction()
    {

    }
}
