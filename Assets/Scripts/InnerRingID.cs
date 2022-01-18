// Decompiled with JetBrains decompiler
// Type: InnerRingID
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class InnerRingID : MonoBehaviour
{
  public bool inner_ring = true;
  public int id = -1;

  private void Start()
  {
    gameElement componentInParent = this.transform.GetComponentInParent<gameElement>();
    if (!(bool) (Object) componentInParent)
      return;
    this.id = componentInParent.id;
  }
}
