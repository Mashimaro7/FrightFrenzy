// Decompiled with JetBrains decompiler
// Type: CodeMonkey.MonoBehaviours.ComponentActions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

namespace CodeMonkey.MonoBehaviours
{
  public class ComponentActions : MonoBehaviour
  {
    public Action OnDestroyFunc;
    public Action OnEnableFunc;
    public Action OnDisableFunc;
    public Action OnUpdate;

    private void OnDestroy()
    {
      if (this.OnDestroyFunc == null)
        return;
      this.OnDestroyFunc();
    }

    private void OnEnable()
    {
      if (this.OnEnableFunc == null)
        return;
      this.OnEnableFunc();
    }

    private void OnDisable()
    {
      if (this.OnDisableFunc == null)
        return;
      this.OnDisableFunc();
    }

    private void Update()
    {
      if (this.OnUpdate == null)
        return;
      this.OnUpdate();
    }

    public static void CreateComponent(
      Action OnDestroyFunc = null,
      Action OnEnableFunc = null,
      Action OnDisableFunc = null,
      Action OnUpdate = null)
    {
      ComponentActions.AddComponent(new GameObject(nameof (ComponentActions)), OnDestroyFunc, OnEnableFunc, OnDisableFunc, OnUpdate);
    }

    public static void AddComponent(
      GameObject gameObject,
      Action OnDestroyFunc = null,
      Action OnEnableFunc = null,
      Action OnDisableFunc = null,
      Action OnUpdate = null)
    {
      ComponentActions componentActions = gameObject.AddComponent<ComponentActions>();
      componentActions.OnDestroyFunc = OnDestroyFunc;
      componentActions.OnEnableFunc = OnEnableFunc;
      componentActions.OnDisableFunc = OnDisableFunc;
      componentActions.OnUpdate = OnUpdate;
    }
  }
}
