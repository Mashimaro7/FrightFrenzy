// Decompiled with JetBrains decompiler
// Type: CodeMonkey.Utils.FunctionUpdater
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace CodeMonkey.Utils
{
  public class FunctionUpdater
  {
    private static List<FunctionUpdater> updaterList;
    private static GameObject initGameObject;
    private GameObject gameObject;
    private string functionName;
    private bool active;
    private Func<bool> updateFunc;

    private static void InitIfNeeded()
    {
      if (!((UnityEngine.Object) FunctionUpdater.initGameObject == (UnityEngine.Object) null))
        return;
      FunctionUpdater.initGameObject = new GameObject("FunctionUpdater_Global");
      FunctionUpdater.updaterList = new List<FunctionUpdater>();
    }

    public static FunctionUpdater Create(Action updateFunc) => FunctionUpdater.Create((Func<bool>) (() =>
    {
      updateFunc();
      return false;
    }), "", true, false);

    public static FunctionUpdater Create(Func<bool> updateFunc) => FunctionUpdater.Create(updateFunc, "", true, false);

    public static FunctionUpdater Create(Func<bool> updateFunc, string functionName) => FunctionUpdater.Create(updateFunc, functionName, true, false);

    public static FunctionUpdater Create(
      Func<bool> updateFunc,
      string functionName,
      bool active)
    {
      return FunctionUpdater.Create(updateFunc, functionName, active, false);
    }

    public static FunctionUpdater Create(
      Func<bool> updateFunc,
      string functionName,
      bool active,
      bool stopAllWithSameName)
    {
      FunctionUpdater.InitIfNeeded();
      if (stopAllWithSameName)
        FunctionUpdater.StopAllUpdatersWithName(functionName);
      GameObject gameObject = new GameObject("FunctionUpdater Object " + functionName, new System.Type[1]
      {
        typeof (FunctionUpdater.MonoBehaviourHook)
      });
      FunctionUpdater functionUpdater = new FunctionUpdater(gameObject, updateFunc, functionName, active);
      gameObject.GetComponent<FunctionUpdater.MonoBehaviourHook>().OnUpdate = new Action(functionUpdater.Update);
      FunctionUpdater.updaterList.Add(functionUpdater);
      return functionUpdater;
    }

    private static void RemoveUpdater(FunctionUpdater funcUpdater)
    {
      FunctionUpdater.InitIfNeeded();
      FunctionUpdater.updaterList.Remove(funcUpdater);
    }

    public static void DestroyUpdater(FunctionUpdater funcUpdater)
    {
      FunctionUpdater.InitIfNeeded();
      funcUpdater?.DestroySelf();
    }

    public static void StopUpdaterWithName(string functionName)
    {
      FunctionUpdater.InitIfNeeded();
      for (int index = 0; index < FunctionUpdater.updaterList.Count; ++index)
      {
        if (FunctionUpdater.updaterList[index].functionName == functionName)
        {
          FunctionUpdater.updaterList[index].DestroySelf();
          break;
        }
      }
    }

    public static void StopAllUpdatersWithName(string functionName)
    {
      FunctionUpdater.InitIfNeeded();
      for (int index = 0; index < FunctionUpdater.updaterList.Count; ++index)
      {
        if (FunctionUpdater.updaterList[index].functionName == functionName)
        {
          FunctionUpdater.updaterList[index].DestroySelf();
          --index;
        }
      }
    }

    public FunctionUpdater(
      GameObject gameObject,
      Func<bool> updateFunc,
      string functionName,
      bool active)
    {
      this.gameObject = gameObject;
      this.updateFunc = updateFunc;
      this.functionName = functionName;
      this.active = active;
    }

    public void Pause() => this.active = false;

    public void Resume() => this.active = true;

    private void Update()
    {
      if (!this.active || !this.updateFunc())
        return;
      this.DestroySelf();
    }

    public void DestroySelf()
    {
      FunctionUpdater.RemoveUpdater(this);
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
