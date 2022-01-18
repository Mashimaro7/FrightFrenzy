// Decompiled with JetBrains decompiler
// Type: CodeMonkey.Utils.FunctionTimer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace CodeMonkey.Utils
{
  public class FunctionTimer
  {
    private static List<FunctionTimer> timerList;
    private static GameObject initGameObject;
    private GameObject gameObject;
    private float timer;
    private string functionName;
    private bool active;
    private bool useUnscaledDeltaTime;
    private Action action;

    private static void InitIfNeeded()
    {
      if (!((UnityEngine.Object) FunctionTimer.initGameObject == (UnityEngine.Object) null))
        return;
      FunctionTimer.initGameObject = new GameObject("FunctionTimer_Global");
      FunctionTimer.timerList = new List<FunctionTimer>();
    }

    public static FunctionTimer Create(Action action, float timer) => FunctionTimer.Create(action, timer, "", false, false);

    public static FunctionTimer Create(
      Action action,
      float timer,
      string functionName)
    {
      return FunctionTimer.Create(action, timer, functionName, false, false);
    }

    public static FunctionTimer Create(
      Action action,
      float timer,
      string functionName,
      bool useUnscaledDeltaTime)
    {
      return FunctionTimer.Create(action, timer, functionName, useUnscaledDeltaTime, false);
    }

    public static FunctionTimer Create(
      Action action,
      float timer,
      string functionName,
      bool useUnscaledDeltaTime,
      bool stopAllWithSameName)
    {
      FunctionTimer.InitIfNeeded();
      if (stopAllWithSameName)
        FunctionTimer.StopAllTimersWithName(functionName);
      GameObject gameObject = new GameObject("FunctionTimer Object " + functionName, new System.Type[1]
      {
        typeof (FunctionTimer.MonoBehaviourHook)
      });
      FunctionTimer functionTimer = new FunctionTimer(gameObject, action, timer, functionName, useUnscaledDeltaTime);
      gameObject.GetComponent<FunctionTimer.MonoBehaviourHook>().OnUpdate = new Action(functionTimer.Update);
      FunctionTimer.timerList.Add(functionTimer);
      return functionTimer;
    }

    public static void RemoveTimer(FunctionTimer funcTimer)
    {
      FunctionTimer.InitIfNeeded();
      FunctionTimer.timerList.Remove(funcTimer);
    }

    public static void StopAllTimersWithName(string functionName)
    {
      FunctionTimer.InitIfNeeded();
      for (int index = 0; index < FunctionTimer.timerList.Count; ++index)
      {
        if (FunctionTimer.timerList[index].functionName == functionName)
        {
          FunctionTimer.timerList[index].DestroySelf();
          --index;
        }
      }
    }

    public static void StopFirstTimerWithName(string functionName)
    {
      FunctionTimer.InitIfNeeded();
      for (int index = 0; index < FunctionTimer.timerList.Count; ++index)
      {
        if (FunctionTimer.timerList[index].functionName == functionName)
        {
          FunctionTimer.timerList[index].DestroySelf();
          break;
        }
      }
    }

    public FunctionTimer(
      GameObject gameObject,
      Action action,
      float timer,
      string functionName,
      bool useUnscaledDeltaTime)
    {
      this.gameObject = gameObject;
      this.action = action;
      this.timer = timer;
      this.functionName = functionName;
      this.useUnscaledDeltaTime = useUnscaledDeltaTime;
    }

    private void Update()
    {
      if (this.useUnscaledDeltaTime)
        this.timer -= Time.unscaledDeltaTime;
      else
        this.timer -= Time.deltaTime;
      if ((double) this.timer > 0.0)
        return;
      this.action();
      this.DestroySelf();
    }

    private void DestroySelf()
    {
      FunctionTimer.RemoveTimer(this);
      if (!((UnityEngine.Object) this.gameObject != (UnityEngine.Object) null))
        return;
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    }

    public static FunctionTimer.FunctionTimerObject CreateObject(
      Action callback,
      float timer)
    {
      return new FunctionTimer.FunctionTimerObject(callback, timer);
    }

    private class MonoBehaviourHook : MonoBehaviour
    {
      public Action OnUpdate;

      private void Update()
      {
        if (this.OnUpdate == null)
          return;
        this.OnUpdate();
      }
    }

    public class FunctionTimerObject
    {
      private float timer;
      private Action callback;

      public FunctionTimerObject(Action callback, float timer)
      {
        this.callback = callback;
        this.timer = timer;
      }

      public void Update() => this.Update(Time.deltaTime);

      public void Update(float deltaTime)
      {
        this.timer -= deltaTime;
        if ((double) this.timer > 0.0)
          return;
        this.callback();
      }
    }
  }
}
