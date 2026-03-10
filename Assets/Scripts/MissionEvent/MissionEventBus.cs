using UnityEngine;
using System;

/// <summary>
/// 정적 클래스
/// </summary>
public static class MissionEventBus
{
    public static event Action OnEnemyKilled;

    public static event Action<string> OnAreaReached;

    public static event Action OnGameFailed;

    public static void PublishEnemyKilled()
    {
        if (OnEnemyKilled != null)
        {
            OnEnemyKilled.Invoke();
        }
    }

    public static void PublishAreaReached(string areaName)
    {
        if (OnAreaReached != null)
        {
            OnAreaReached.Invoke(areaName);
        }
    }

    public static void PublishGameFailed()
    {
        if (OnGameFailed != null)
        {
            OnGameFailed.Invoke();
        }
    }
}
