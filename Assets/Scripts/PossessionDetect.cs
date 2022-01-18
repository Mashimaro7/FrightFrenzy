// Decompiled with JetBrains decompiler
// Type: PossessionDetect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class PossessionDetect : MonoBehaviour
{
  public PossessionTracker[] all_trackers;
  public int limit_num = 1;
  public int curr_count;
  private float time_of_fault;
  private Scorekeeper_FreightFrenzy ff_scorer;
  public RobotID self_ri;
  private Dictionary<gameElement, bool> master_list = new Dictionary<gameElement, bool>();

  private void Start()
  {
    if (!(bool) (Object) this.ff_scorer)
      this.ff_scorer = Object.FindObjectOfType<Scorekeeper_FreightFrenzy>();
    foreach (PossessionTracker allTracker in this.all_trackers)
    {
      if ((bool) (Object) allTracker)
        allTracker.master = this;
    }
    this.self_ri = this.GetComponentInParent<RobotID>();
  }

  private void Update()
  {
    this.curr_count = this.GetGameElementCount();
    if (this.curr_count > this.limit_num)
    {
      if ((double) this.time_of_fault > 0.0)
        return;
      this.time_of_fault = Time.time;
    }
    else
      this.time_of_fault = 0.0f;
  }

  public float GetFaultDuration() => (double) this.time_of_fault <= 0.0 ? 0.0f : Time.time - this.time_of_fault;

  public void ResetFault(float delta_time = 0.0f)
  {
    if (this.curr_count > this.limit_num)
      this.time_of_fault = Time.time + delta_time;
    else
      this.time_of_fault = 0.0f;
  }

  public bool IsFault() => this.curr_count > this.limit_num;

  public int GetGameElementCount()
  {
    int gameElementCount1 = 0;
    int num = 0;
    foreach (GenericFieldTracker allTracker in this.all_trackers)
    {
      if ((bool) (Object) allTracker)
      {
        int gameElementCount2 = allTracker.GetGameElementCount();
        gameElementCount1 += gameElementCount2;
        if (gameElementCount2 > 0)
          ++num;
      }
    }
    if (gameElementCount1 <= 1 || this.all_trackers.Length <= 1 || num <= 1)
      return gameElementCount1;
    this.master_list.Clear();
    foreach (GenericFieldTracker allTracker in this.all_trackers)
    {
      foreach (gameElement gameElement in allTracker.game_elements)
        this.master_list[gameElement] = true;
    }
    return this.master_list.Count;
  }

  public void OnGameElementTriggerExit(gameElement ge)
  {
    foreach (PossessionTracker allTracker in this.all_trackers)
    {
      if ((bool) (Object) allTracker && allTracker.IsGameElementInside(ge))
        return;
    }
    ge.held_by_robot = 0;
    if (!(bool) (Object) ge.tracker || ge.tracker.IsGameElementInside(ge) || !(bool) (Object) this.ff_scorer || !(bool) (Object) this.self_ri)
      return;
    this.ff_scorer.RobotDroppedItem(this.self_ri.id, ge);
  }
}
