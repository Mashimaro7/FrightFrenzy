// Decompiled with JetBrains decompiler
// Type: Scorekeeper_SplishSplash
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class Scorekeeper_SplishSplash : Scorekeeper
{
  private GameObject[] fieldElements;
  private double score_red;
  private double score_blue;

  private void Awake()
  {
    GLOBALS.PlayerCount = 4;
    GLOBALS.TIMER_TOTAL = 120;
    GLOBALS.TIMER_AUTO = 0;
    GLOBALS.TIMER_ENDGAME = 0;
  }

  public override void ScorerInit() => this.fieldElements = GameObject.FindGameObjectsWithTag("GameElement");

  public override void ScorerUpdate() => this.CalculateScores(this.timer_elapsed);

  public override void ScorerReset()
  {
    base.ScorerReset();
    this.score_red = 0.0;
    this.score_blue = 0.0;
  }

  public override void OnTimerStart()
  {
    base.OnTimerStart();
    this.ScorerReset();
  }

  private void CalculateScores(TimeSpan elapsed_time)
  {
    foreach (GameObject fieldElement in this.fieldElements)
    {
      if ((double) fieldElement.transform.localPosition.x < 0.0)
      {
        if (fieldElement.GetComponent<gameElement>().type == ElementType.Cube)
          this.score_red += 0.5 * elapsed_time.TotalMilliseconds / 1000.0;
        else if (fieldElement.GetComponent<gameElement>().type == ElementType.Jewel)
          this.score_red += 1.5 * elapsed_time.TotalMilliseconds / 1000.0;
      }
      else if (fieldElement.GetComponent<gameElement>().type == ElementType.Cube)
        this.score_blue += 0.5 * elapsed_time.TotalMilliseconds / 1000.0;
      else if (fieldElement.GetComponent<gameElement>().type == ElementType.Jewel)
        this.score_blue += 1.5 * elapsed_time.TotalMilliseconds / 1000.0;
    }
  }

  public override int GetRedScore() => this.timerstate == Scorekeeper.TimerState.FINISHED ? this.score_redfinal : (int) this.score_red + this.score_redadj;

  public override int GetBlueScore() => this.timerstate == Scorekeeper.TimerState.FINISHED ? this.score_bluefinal : (int) this.score_blue + this.score_blueadj;
}
