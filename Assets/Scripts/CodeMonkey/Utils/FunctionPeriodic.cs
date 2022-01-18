// Decompiled with JetBrains decompiler
// Type: CodeMonkey.Utils.FunctionPeriodic
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace CodeMonkey.Utils
{
  public class FunctionPeriodic
  {
    private static List<FunctionPeriodic> funcList;
    private static GameObject initGameObject;
    private GameObject gameObject;
    private float timer;
    private float baseTimer;
    private bool useUnscaledDeltaTime;
    private string functionName;
    public Action action;
    public Func<bool> testDestroy;

    private static void InitIfNeeded()
    {
      if (!((UnityEngine.Object) FunctionPeriodic.initGameObject == (UnityEngine.Object) null))
        return;
      FunctionPeriodic.initGameObject = new GameObject("FunctionPeriodic_Global");
      FunctionPeriodic.funcList = new List<FunctionPeriodic>();
    }

    public static FunctionPeriodic Create_Global(
      Action action,
      Func<bool> testDestroy,
      float timer)
    {
      FunctionPeriodic global = FunctionPeriodic.Create(action, testDestroy, timer, "", false, false, false);
      UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) global.gameObject);
      return global;
    }

    public static FunctionPeriodic Create(
      Action action,
      Func<bool> testDestroy,
      float timer)
    {
      return FunctionPeriodic.Create(action, testDestroy, timer, "", false);
    }

    public static FunctionPeriodic Create(Action action, float timer) => FunctionPeriodic.Create(action, (Func<bool>) null, timer, "", false, false, false);

    public static FunctionPeriodic Create(
      Action action,
      float timer,
      string functionName)
    {
      return FunctionPeriodic.Create(action, (Func<bool>) null, timer, functionName, false, false, false);
    }

    public static FunctionPeriodic Create(
      Action callback,
      Func<bool> testDestroy,
      float timer,
      string functionName,
      bool stopAllWithSameName)
    {
      return FunctionPeriodic.Create(callback, testDestroy, timer, functionName, false, false, stopAllWithSameName);
    }

    public static FunctionPeriodic Create(
      Action action,
      Func<bool> testDestroy,
      float timer,
      string functionName,
      bool useUnscaledDeltaTime,
      bool triggerImmediately,
      bool stopAllWithSameName)
    {
      FunctionPeriodic.InitIfNeeded();
      if (stopAllWithSameName)
        FunctionPeriodic.StopAllFunc(functionName);
      GameObject gameObject = new GameObject("FunctionPeriodic Object " + functionName, new System.Type[1]
      {
        typeof (FunctionPeriodic.MonoBehaviourHook)
      });
      FunctionPeriodic functionPeriodic = new FunctionPeriodic(gameObject, action, timer, testDestroy, functionName, useUnscaledDeltaTime);
      gameObject.GetComponent<FunctionPeriodic.MonoBehaviourHook>().OnUpdate = new Action(functionPeriodic.Update);
      FunctionPeriodic.funcList.Add(functionPeriodic);
      if (triggerImmediately)
        action();
      return functionPeriodic;
    }

    public static void RemoveTimer(FunctionPeriodic funcTimer)
    {
      FunctionPeriodic.InitIfNeeded();
      FunctionPeriodic.funcList.Remove(funcTimer);
    }

    public static void StopTimer(string _name)
    {
      FunctionPeriodic.InitIfNeeded();
      for (int index = 0; index < FunctionPeriodic.funcList.Count; ++index)
      {
        if (FunctionPeriodic.funcList[index].functionName == _name)
        {
          FunctionPeriodic.funcList[index].DestroySelf();
          break;
        }
      }
    }

    public static void StopAllFunc(string _name)
    {
      FunctionPeriodic.InitIfNeeded();
      for (int index = 0; index < FunctionPeriodic.funcList.Count; ++index)
      {
        if (FunctionPeriodic.funcList[index].functionName == _name)
        {
          FunctionPeriodic.funcList[index].DestroySelf();
          --index;
        }
      }
    }

    public static bool IsFuncActive(string name)
    {
      FunctionPeriodic.InitIfNeeded();
      for (int index = 0; index < FunctionPeriodic.funcList.Count; ++index)
      {
        if (FunctionPeriodic.funcList[index].functionName == name)
          return true;
      }
      return false;
    }

    private FunctionPeriodic(
      GameObject gameObject,
      Action action,
      float timer,
      Func<bool> testDestroy,
      string functionName,
      bool useUnscaledDeltaTime)
    {
      this.gameObject = gameObject;
      this.action = action;
      this.timer = timer;
      this.testDestroy = testDestroy;
      this.functionName = functionName;
      this.useUnscaledDeltaTime = useUnscaledDeltaTime;
      this.baseTimer = timer;
    }

    public void SkipTimerTo(float timer) => this.timer = timer;

    private void Update()
    {
      if (this.useUnscaledDeltaTime)
        this.timer -= Time.unscaledDeltaTime;
      else
        this.timer -= Time.deltaTime;
      if ((double) this.timer > 0.0)
        return;
      this.action();
      if (this.testDestroy != null && this.testDestroy())
        this.DestroySelf();
      else
        this.timer += this.baseTimer;
    }

    public void DestroySelf()
    {
      FunctionPeriodic.RemoveTimer(this);
      if (!((UnityEngine.Object) this.gameObject != (UnityEngine.Object) null))
        return;
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
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
  }
}
