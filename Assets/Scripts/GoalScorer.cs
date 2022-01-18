// Decompiled with JetBrains decompiler
// Type: GoalScorer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class GoalScorer : MonoBehaviour
{
  public List<RingCounter> low_counter = new List<RingCounter>();
  public List<RingCounter> mid_counter = new List<RingCounter>();
  public List<RingCounter> high_counter = new List<RingCounter>();
  public float time_last_updated;
  private int low_count;
  private int mid_count;
  private int high_count;
  public bool isRed;
  public bool isBlue;
  public bool isYellow;

  public void UpdateCounts()
  {
    if ((double) this.time_last_updated == (double) Time.time)
      return;
    this.time_last_updated = Time.time;
    this.low_count = 0;
    this.mid_count = 0;
    this.high_count = 0;
    List<gameElement> gameElementList = new List<gameElement>();
    foreach (RingCounter ringCounter in this.high_counter)
    {
      for (int index = ringCounter.collisions_ir_count.Count - 1; index >= 0; --index)
      {
        if (ringCounter.collisions_ir_count[index] >= 2)
        {
          gameElementList.Add(ringCounter.collisions_ir[index]);
          RobotCollision component = ringCounter.collisions_ir[index].GetComponent<RobotCollision>();
          if (!(bool) (Object) component || component.GetRobotCount() <= 0)
            ++this.high_count;
        }
      }
    }
    foreach (RingCounter ringCounter in this.mid_counter)
    {
      for (int index = ringCounter.collisions_ir_count.Count - 1; index >= 0; --index)
      {
        if (ringCounter.collisions_ir_count[index] >= 2)
        {
          gameElementList.Add(ringCounter.collisions_ir[index]);
          RobotCollision component = ringCounter.collisions_ir[index].GetComponent<RobotCollision>();
          if (!(bool) (Object) component || component.GetRobotCount() <= 0)
            ++this.mid_count;
        }
      }
    }
    foreach (RingCounter ringCounter in this.low_counter)
    {
      foreach (gameElement collision in ringCounter.collisions)
      {
        if (!gameElementList.Contains(collision))
        {
          gameElementList.Add(collision);
          RobotCollision component = collision.GetComponent<RobotCollision>();
          if (!(bool) (Object) component || component.GetRobotCount() <= 0)
            ++this.low_count;
        }
      }
    }
  }

  public int GetLowCount()
  {
    this.UpdateCounts();
    return this.low_count;
  }

  public int GetMidCount()
  {
    this.UpdateCounts();
    return this.mid_count;
  }

  public int GetHighCount()
  {
    this.UpdateCounts();
    return this.high_count;
  }
}
