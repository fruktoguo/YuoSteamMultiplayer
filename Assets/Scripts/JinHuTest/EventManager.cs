using System;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    private static EventManager instance;
    private Dictionary<string, Action<object>> eventDictionary;

    public static EventManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<EventManager>();

                if (instance == null)
                {
                    GameObject singleton = new GameObject("EventManager");
                    instance = singleton.AddComponent<EventManager>();
                }
            }

            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            eventDictionary = new Dictionary<string, Action<object>>();
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public static void AddListener(string eventName, Action<object> listener)
    {
        if (Instance.eventDictionary.TryGetValue(eventName, out Action<object> thisEvent))
        {
            thisEvent += listener;
            Instance.eventDictionary[eventName] = thisEvent;
        }
        else
        {
            thisEvent += listener;
            Instance.eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void RemoveListener(string eventName, Action<object> listener)
    {
        if (instance == null) return;
        if (Instance.eventDictionary.TryGetValue(eventName, out Action<object> thisEvent))
        {
            thisEvent -= listener;
            if (thisEvent == null)
            {
                Instance.eventDictionary.Remove(eventName);
            }
            else
            {
                Instance.eventDictionary[eventName] = thisEvent;
            }
        }
    }

    public static void TriggerEvent(string eventName, object eventParam = null)
    {
        if (Instance.eventDictionary.TryGetValue(eventName, out Action<object> thisEvent))
        {
            thisEvent.Invoke(eventParam);
        }
    }
}
