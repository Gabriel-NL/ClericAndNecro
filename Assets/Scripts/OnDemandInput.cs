using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OnDemandInput : MonoBehaviour
{
    private Dictionary<KeyCode, Action> inputActions = new Dictionary<KeyCode, Action>();

    public void AddInput(KeyCode key, Action action)
    {
        if (!inputActions.ContainsKey(key))
        {
            inputActions[key] = action;
        }
        else
        {
            inputActions[key] += action; // Allow multiple actions for the same key
        }
    }

    public void AddInputSingleFunction(KeyCode key, Action action)
    {
        if (!inputActions.ContainsKey(key))
        {
            inputActions[key] = action;
        }
        else
        {
            throw new Exception($"Key {key} is already assigned to an action.");
        }
    }
    public void RemoveInput(KeyCode key, Action action)
    {
        if (inputActions.ContainsKey(key))
        {
            inputActions[key] -= action;
            if (inputActions[key] == null) inputActions.Remove(key); // Remove key if no actions left
        }
    }

    public IEnumerator ProcessInput()
    {
        while (true)
        {
            yield return new WaitUntil(() => Input.anyKeyDown);
            foreach (var keyAction in inputActions)
            {
                if (Input.GetKeyDown(keyAction.Key))
                {
                    keyAction.Value?.Invoke();
                }
            }
            yield return null;
        }

    }

    public void StartListeningInputs()
    {

    }
}
