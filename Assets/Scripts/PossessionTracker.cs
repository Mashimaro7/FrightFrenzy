// Decompiled with JetBrains decompiler
// Type: PossessionTracker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class PossessionTracker : GenericFieldTracker
{
  public PossessionDetect master;

  public override void OnGameElementTriggerExit(gameElement ge)
  {
    if (!(bool) (Object) this.master)
      return;
    this.master.OnGameElementTriggerExit(ge);
  }

  public override void OnGameElementTriggerEnter(gameElement ge)
  {
    if (!(bool) (Object) this.self_ri)
      return;
    ge.held_by_robot = this.self_ri.id;
  }
}
