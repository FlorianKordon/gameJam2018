using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    private int timeScalingDuration = 1;

    public void SetTimeScaleGradually(float targetTimeScale)
    {
        StopAllCoroutines();
        StartCoroutine(ScaleTimeGradually(targetTimeScale));
    }

    public void SetTimeScaleInstant(float targetTimeScale)
    {
        StopAllCoroutines();
        Time.timeScale = targetTimeScale;
    }

    private IEnumerator ScaleTimeGradually(float targetTimeScale)
    {        
        float timeScalingSpeed = Mathf.Abs(Time.timeScale - targetTimeScale) / timeScalingDuration;

        while (!Mathf.Approximately(Time.timeScale, targetTimeScale))
        {
            Time.timeScale = Mathf.MoveTowards(Time.timeScale, targetTimeScale, timeScalingSpeed * 0.01f);
            yield return null;
        }
    }
}
