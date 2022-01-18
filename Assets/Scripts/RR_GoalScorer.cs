// Decompiled with JetBrains decompiler
// Type: RR_GoalScorer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class RR_GoalScorer : GenericFieldTracker
{
  public List<ball_data> red_balls = new List<ball_data>();
  public List<ball_data> blue_balls = new List<ball_data>();

  public override void OnGameElementTriggerEnter(gameElement ge)
  {
    ball_data component = ge.GetComponent<ball_data>();
    if ((Object) component == (Object) null || component.IsFlagSet("Invalid"))
      return;
    component.SetFlag("Invalid");
    if (!component.IsFlagSet("Funnel"))
      return;
    if (ge.type == ElementType.Blue1)
    {
      this.blue_balls.Add(component);
    }
    else
    {
      if (ge.type != ElementType.Red1)
        return;
      this.red_balls.Add(component);
    }
  }

  public override void Reset()
  {
    base.Reset();
    this.ClearData();
  }

  public void ClearData()
  {
    this.red_balls.Clear();
    this.blue_balls.Clear();
  }

  public int GetRedBallCount() => this.red_balls.Count;

  public int GetblueBallCount() => this.blue_balls.Count;
}
