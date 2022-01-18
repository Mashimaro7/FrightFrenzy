// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets.Utility.TimedObjectActivator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityStandardAssets.Utility
{
  public class TimedObjectActivator : MonoBehaviour
  {
    public TimedObjectActivator.Entries entries = new TimedObjectActivator.Entries();

    private void Awake()
    {
      foreach (TimedObjectActivator.Entry entry in this.entries.entries)
      {
        switch (entry.action)
        {
          case TimedObjectActivator.Action.Activate:
            this.StartCoroutine(this.Activate(entry));
            break;
          case TimedObjectActivator.Action.Deactivate:
            this.StartCoroutine(this.Deactivate(entry));
            break;
          case TimedObjectActivator.Action.Destroy:
            UnityEngine.Object.Destroy((UnityEngine.Object) entry.target, entry.delay);
            break;
          case TimedObjectActivator.Action.ReloadLevel:
            this.StartCoroutine(this.ReloadLevel(entry));
            break;
        }
      }
    }

    private IEnumerator Activate(TimedObjectActivator.Entry entry)
    {
      yield return (object) new WaitForSeconds(entry.delay);
      entry.target.SetActive(true);
    }

    private IEnumerator Deactivate(TimedObjectActivator.Entry entry)
    {
      yield return (object) new WaitForSeconds(entry.delay);
      entry.target.SetActive(false);
    }

    private IEnumerator ReloadLevel(TimedObjectActivator.Entry entry)
    {
      yield return (object) new WaitForSeconds(entry.delay);
      SceneManager.LoadScene(SceneManager.GetSceneAt(0).name);
    }

    public enum Action
    {
      Activate,
      Deactivate,
      Destroy,
      ReloadLevel,
      Call,
    }

    [Serializable]
    public class Entry
    {
      public GameObject target;
      public TimedObjectActivator.Action action;
      public float delay;
    }

    [Serializable]
    public class Entries
    {
      public TimedObjectActivator.Entry[] entries;
    }
  }
}
