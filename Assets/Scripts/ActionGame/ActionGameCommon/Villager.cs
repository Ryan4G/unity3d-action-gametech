using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Villager : MonoBehaviour
{
    // tied max val
    const float FATIGUE_DEFAULT_VALUE = 8f;
    // food max val
    const float SATIATION_DEFAULT_VALUE = 5f;
    // tied min val
    const float FATIGUE_MIN_VALUE = .2f;
    // food min va
    const float SATIATION_MIN_VALUE = .2f;

    float mSatiation;
    
    float mFatigue;

    Coroutine mActionCoroutine;

    private void OnEnable()
    {
        mSatiation = SATIATION_DEFAULT_VALUE;
        mFatigue = FATIGUE_DEFAULT_VALUE;

        // loop update two figues
        StartCoroutine(Tick());
    }

    private IEnumerator Tick()
    {
        while (true)
        {
            mSatiation = Mathf.Max(0, mSatiation - Time.deltaTime);
            mFatigue = Mathf.Max(0, mFatigue - Time.deltaTime);

            if (mSatiation <= SATIATION_MIN_VALUE && mActionCoroutine == null)
            {
                mActionCoroutine = StartCoroutine(EatFood());
            }

            if (mFatigue <= FATIGUE_MIN_VALUE)
            {
                mActionCoroutine = StartCoroutine(GoSleep());
            }

            yield return null;
        }
    }

    private IEnumerator GoSleep()
    {
        StopCoroutine(mActionCoroutine);

        Debug.Log($"It's time to sleep... {mSatiation} {mFatigue}");

        mFatigue = FATIGUE_DEFAULT_VALUE;

        mActionCoroutine = null;

        yield return mActionCoroutine;
    }

    private IEnumerator EatFood()
    {
        Debug.Log($"It's time to eat food... {mSatiation} {mFatigue}");

        mSatiation = SATIATION_DEFAULT_VALUE;

        mActionCoroutine = null;

        yield return mActionCoroutine;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
