using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Node
{
    public Node parent;
    public float cost;
    public Dictionary<GStateEnum, int> states;
    public GAction action;

    public Node(Node parent, float cost, Dictionary<GStateEnum, int> allStates, GAction action)
    {
        this.parent = parent;
        this.cost = cost;
        states = new Dictionary<GStateEnum, int>(allStates);
        this.action = action;
    }

    public Node(Node parent, float cost, Dictionary<GStateEnum, int> allStates, Dictionary<GStateEnum, int> beliefStates, GAction action)
    {
        this.parent = parent;
        this.cost = cost;
        states = new Dictionary<GStateEnum, int>(allStates);
        foreach(KeyValuePair<GStateEnum, int> s in beliefStates)
        {
            if(!states.ContainsKey(s.Key))
            {
                states.Add(s.Key, s.Value);
            }
        }
        this.action = action;
    }
}

public class GPlanner
{
    public Queue<GAction> Plan(GStateManager worldState, List<GAction> actions, Dictionary<GStateEnum, int> goals, GStateManager beliefState)
    {
        List<GAction> usableActions = new List<GAction>();
        foreach(GAction action in actions)
        {
            if(action.IsAchievable())
            {
                usableActions.Add(action);
            }
        }

        List<Node> leaves = new List<Node>();
        Node start = new Node(null, 0, worldState.GetState(), beliefState.GetState(), null);
        bool success = BuildGraph(start, leaves, usableActions, goals);

        if(!success)
        {
            Debug.Log("No plan!");
            return null;
        }

        Node cheapest = null;
        foreach(Node leaf in leaves)
        {
            if(cheapest == null)
            {
                cheapest = leaf;
            }
            else
            {
                if(leaf.cost < cheapest.cost)
                {
                    cheapest = leaf;
                }
            }
        }
        List<GAction> results = new List<GAction>();
        Node n = cheapest;
        while(n != null)
        {
            if(n.action != null)
            {
                results.Insert(0, n.action);
            }
            n = n.parent;
        }
        Queue<GAction> queue = new Queue<GAction>();
        foreach(GAction g in results)
        {
            queue.Enqueue(g);
        }
        return queue;

    }

    public bool BuildGraph(Node parent, List<Node> leaves, List<GAction> usableActions, Dictionary<GStateEnum, int> goals)
    {
        bool foundPath = false;
        foreach(GAction action in usableActions)
        {
            if(action.IsAchievableGiven(parent.states))
            {
                Dictionary<GStateEnum, int> currentState = new Dictionary<GStateEnum, int>(parent.states);
                foreach (KeyValuePair<GStateEnum, int> eff in action.effects)
                {
                    if(!currentState.ContainsKey(eff.Key))
                    {
                        currentState.Add(eff.Key, eff.Value);
                    }
                }

                Node node = new Node(parent, parent.cost + action.cost, currentState, action);

                if(GoalAchieved(goals, currentState))
                {
                    leaves.Add(node);
                    foundPath = true;
                }
                else
                {
                    List<GAction> subset = ActionSubset(usableActions, action);
                    bool found = BuildGraph(node, leaves, subset, goals);
                    if (found) foundPath = true;
                }
            }
        }
        return foundPath;
    }

    public bool GoalAchieved(Dictionary<GStateEnum, int> goals, Dictionary<GStateEnum, int> states)
    {
        foreach(KeyValuePair<GStateEnum, int> g in goals)
        {
            if(!states.ContainsKey(g.Key))
            {
                return false;
            }
        }
        return true;
    }

    public List<GAction> ActionSubset(List<GAction> subsetActions, GAction actionRemoved)
    {
        List<GAction> result = new List<GAction>();
        foreach(GAction action in subsetActions)
        {
            if(!action.Equals(actionRemoved))
            {
                result.Add(action);
            }
        }
        return result;
    }
}
