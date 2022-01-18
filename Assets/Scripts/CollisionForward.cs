// Decompiled with JetBrains decompiler
// Type: CollisionForward
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class CollisionForward : MonoBehaviour
{
  private void OnCollisionEnter(Collision collision) => this.transform.parent.GetComponent<Sound_Walls>().OnCollisionEnter(collision);
}
