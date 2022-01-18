// Decompiled with JetBrains decompiler
// Type: RobotSkin
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class RobotSkin : MonoBehaviour
{
  public RobotInterface3D ri3d;

  public virtual void InitSkin()
  {
    this.ri3d = this.GetComponent<RobotInterface3D>();
    if ((Object) this.ri3d == (Object) null)
      this.ri3d = this.GetComponentInParent<RobotInterface3D>();
    if (!((Object) this.ri3d != (Object) null))
      return;
    this.ri3d.robotskin = this;
  }

  public virtual string GetState() => "";

  public virtual void SetState(string instate)
  {
  }
}
