using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageManager
{
    static MessageManager mInstance;

    public static MessageManager Instatnce
    {
        get
        {
            return mInstance ?? (mInstance = new MessageManager());
        }
    }

    Dictionary<string, Action<object[]>> mMessageDict = new Dictionary<string, Action<object[]>>(32);

    Dictionary<string, object[]> mDispatchCacheDict = new Dictionary<string, object[]>(16);

    private MessageManager() { }

    public void Subscribe(string message, Action<object[]> action)
    {
        Action<object[]> value = null;

        if (mMessageDict.TryGetValue(message, out value))
        {
            value += action;
            mMessageDict[message] = value;
        }
        else
        {
            mMessageDict.Add(message, action);
        }
    }

    public void Unsubscribe(string message)
    {
        mMessageDict.Remove(message);
    }

    public void Dispatch(string message, object[] args = null, bool addToCache = false)
    {
        if (addToCache)
        {
            mDispatchCacheDict[message] = args;
        }
        else
        {
            Action<object[]> value = null;
            if (mMessageDict.TryGetValue(message, out value))
            {
                value.Invoke(args);
            }
        }
    }

    public void ProcessDispatchCache(string message)
    {
        object[] value = null;
        if (mDispatchCacheDict.TryGetValue(message, out value))
        {
            Dispatch(message, value);
            mDispatchCacheDict.Remove(message);
        }
    }
}
