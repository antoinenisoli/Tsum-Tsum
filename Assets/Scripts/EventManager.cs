using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;

    public class FloatEvent : UnityEvent<float> { };
    public FloatEvent onNewPet = new FloatEvent();
    public FloatEvent onAddScore = new FloatEvent();
    public UnityEvent onNewGame = new UnityEvent();

    public class BoolEvent : UnityEvent<bool> { };
    public BoolEvent onEndGame = new BoolEvent();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
}
