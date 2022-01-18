// Decompiled with JetBrains decompiler
// Type: PassCollisionsUp
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class PassCollisionsUp : MonoBehaviour
{
  public RobotInterface3D owner;

  private void OnTriggerEnter(Collider other) => this.owner.OnTriggerEnter(other);

  private void OnTriggerExit(Collider other) => this.owner.OnTriggerExit(other);
}
