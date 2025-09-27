using System;

// Transition descriptor for centralized state switching
public class StateTransition
{
    public string ToStateName { get; }
    public Func<bool> Condition { get; }
    public Func<object> DataProvider { get; }
    public int Priority { get; }
    public Action OnTriggered { get; }

    public StateTransition(string toStateName, Func<bool> condition, Func<object> dataProvider = null, int priority = 0, Action onTriggered = null)
    {
        ToStateName = toStateName;
        Condition = condition;
        DataProvider = dataProvider;
        Priority = priority;
        OnTriggered = onTriggered;
    }
}


