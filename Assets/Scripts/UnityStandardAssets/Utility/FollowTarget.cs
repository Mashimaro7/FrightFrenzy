// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets.Utility.FollowTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace UnityStandardAssets.Utility
{
  public class FollowTarget : MonoBehaviour
  {
    public Transform target;
    public Vector3 offset = new Vector3(0.0f, 7.5f, 0.0f);

    private void LateUpdate() => this.transform.position = this.target.position + this.offset;
  }
}
