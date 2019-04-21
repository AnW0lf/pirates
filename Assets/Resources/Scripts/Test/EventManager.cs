﻿using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System;
using Facebook.Unity;

public class EventManager : MonoBehaviour
{

    [Serializable]
    public class GameEvent : UnityEvent<object[]> { };

    private static EventManager _instance;
    private Dictionary<string, GameEvent> _eventDictionary;

    void Awake()
    {
        if (_instance != null)
            return;
        _instance = GetComponent<EventManager>();
        if (_eventDictionary == null)
            _eventDictionary = new Dictionary<string, GameEvent>();

        // FACEBOOK SDK
        FB.Init();
        if (FB.IsInitialized)
        {
            FB.ActivateApp();
        }
        else
        {
            FB.Init(() => { FB.ActivateApp(); });
        }
    }

    /// <summary>Добавляем "слушателя" для события</summary>
    /// <param name="eventName">Название события</param>
    /// <param name="listener">Метод-обработчик события</param>
    public static void Subscribe(string eventName, UnityAction<object[]> listener)
    {
        if (_instance._eventDictionary.TryGetValue(eventName, out GameEvent thisEvent))
            thisEvent.AddListener(listener);
        else
        {
            thisEvent = new GameEvent();
            thisEvent.AddListener(listener);
            _instance._eventDictionary.Add(eventName, thisEvent);
        }
    }

    /// <summary>Удаляем "слушателя" из списка</summary>
    /// <param name="eventName">Название события</param>
    /// <param name="listener">Метод-обработчик события</param>
    public static void Unsubscribe(string eventName, UnityAction<object[]> listener)
    {
        if (_instance == null)
            return;
        if (_instance._eventDictionary.TryGetValue(eventName, out GameEvent thisEvent))
            thisEvent.RemoveListener(listener);
    }

    /// <summary>Отправить событие</summary>
    /// <param name="eventName">Название события</param>
    /// <param name="parameters">Параметр массив</param>
    public static void SendEvent(string eventName, params object[] parameters)
    {
        if (_instance._eventDictionary.TryGetValue(eventName, out GameEvent thisEvent))
        {
            thisEvent.Invoke(parameters);
        }
    }
}