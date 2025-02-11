using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// https://docs.unity3d.com/ScriptReference/Collider.html
[RequireComponent(typeof(Collider2D))]
public class Trigger2DUnityEvent : MonoBehaviour
{
    // Components
    private Collider2D objectCollider;
    // Configs
    public bool overrideIsTriggerTrue = false;
    // Unity Events
    public UnityEvent onTriggerEnterUnityEvent;
    public UnityEvent<Collider2D> onTriggerEnterUnityEventWithArg;
    public Action onTriggerEnterCallback;

    void Start()
    {
        // Get Collider component
        TryGetComponent<Collider2D>(out objectCollider);
        // Config isTrigger
        if (!objectCollider.isTrigger && overrideIsTriggerTrue)
        {
            objectCollider.isTrigger = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        onTriggerEnterUnityEvent?.Invoke();
        onTriggerEnterUnityEventWithArg?.Invoke(col);
        if (onTriggerEnterCallback != null)
        {
            onTriggerEnterCallback();
        }
        
    }
}
