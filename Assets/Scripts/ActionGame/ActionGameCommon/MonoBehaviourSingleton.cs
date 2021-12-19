using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoBehaviourSingleton : MonoBehaviour
{
    static bool mIsDestroying;

    static MonoBehaviourSingleton mInstance;

    public static MonoBehaviourSingleton Instance
    {
        get
        {
            if (mIsDestroying)
            {
                return null;
            }

            if (mInstance == null)
            {
                mInstance = new GameObject("[MonoBehaviourSingleton]").AddComponent<MonoBehaviourSingleton>();
                DontDestroyOnLoad(mInstance.gameObject);
            }

            return mInstance;
        }
    }

    private void OnDestroy()
    {
        mIsDestroying = true;
    }
}
