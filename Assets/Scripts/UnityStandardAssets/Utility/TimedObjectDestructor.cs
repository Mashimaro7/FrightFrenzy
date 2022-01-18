// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets.Utility.TimedObjectDestructor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace UnityStandardAssets.Utility
{
  public class TimedObjectDestructor : MonoBehaviour
  {
    [SerializeField]
    private float m_TimeOut = 1f;
    [SerializeField]
    private bool m_DetachChildren;

    private void Awake() => this.Invoke("DestroyNow", this.m_TimeOut);

    private void DestroyNow()
    {
      if (this.m_DetachChildren)
        this.transform.DetachChildren();
      Object.DestroyObject((Object) this.gameObject);
    }
  }
}
