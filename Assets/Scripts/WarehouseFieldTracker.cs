// Decompiled with JetBrains decompiler
// Type: WarehouseFieldTracker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class WarehouseFieldTracker : GenericFieldTracker
{
  private Scorekeeper_FreightFrenzy ff_scorer;

  protected override void Start()
  {
    base.Start();
    this.ff_scorer = GameObject.Find("Scorekeeper").GetComponent<Scorekeeper_FreightFrenzy>();
  }

  public override void OnGameElementTriggerExit(gameElement ge)
  {
    if (ge.note2 != "in" || this.IsGameElementInside(ge) || !(bool) (Object) this.ff_scorer || ge.held_by_robot != 0)
      return;
    this.ff_scorer.GameElementLeftWarehouse(ge, (GenericFieldTracker) this);
  }

  public override void OnGameElementTriggerEnter(gameElement ge)
  {
    if (!(ge.note2 == "in"))
      return;
    ge.tracker = (GenericFieldTracker) this;
  }
}
