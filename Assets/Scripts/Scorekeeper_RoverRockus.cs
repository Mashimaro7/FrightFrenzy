// Decompiled with JetBrains decompiler
// Type: Scorekeeper_RoverRockus
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class Scorekeeper_RoverRockus : Scorekeeper
{
  public RR_scoringBox scorer_red_silver;
  public RR_scoringBox scorer_red_gold;
  public RR_scoringBox scorer_red_box;
  public RR_scoringBox scorer_blue_silver;
  public RR_scoringBox scorer_blue_gold;
  public RR_scoringBox scorer_blue_box;
  public GameObject rule1_r1;
  private RR_Rule1 r1_r1;
  public GameObject rule1_r2;
  private RR_Rule1 r1_r2;
  public GameObject rule1_b1;
  private RR_Rule1 r1_b1;
  public GameObject rule1_b2;
  private RR_Rule1 r1_b2;
  public GameObject redcrater;
  public GameObject bluecrater;
  public GameObject center_marker;
  private double score_red;
  private double score_blue;

  private void Awake()
  {
    GLOBALS.PlayerCount = 4;
    GLOBALS.TIMER_TOTAL = 120;
    GLOBALS.TIMER_AUTO = 0;
    GLOBALS.TIMER_ENDGAME = 30;
  }

  public override void ScorerInit()
  {
    this.score_red = 0.0;
    this.score_blue = 0.0;
    this.r1_r1 = this.rule1_r1.GetComponent<RR_Rule1>();
    this.r1_r1.robotid = (RobotID) null;
    this.r1_r1.goal = this.center_marker.transform;
    this.r1_r2 = this.rule1_r2.GetComponent<RR_Rule1>();
    this.r1_r2.robotid = (RobotID) null;
    this.r1_r2.goal = this.center_marker.transform;
    this.r1_b1 = this.rule1_b1.GetComponent<RR_Rule1>();
    this.r1_b1.robotid = (RobotID) null;
    this.r1_b1.goal = this.center_marker.transform;
    this.r1_b2 = this.rule1_b2.GetComponent<RR_Rule1>();
    this.r1_b2.robotid = (RobotID) null;
    this.r1_b2.goal = this.center_marker.transform;
    foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Robot"))
    {
      RobotID component = gameObject.GetComponent<RobotID>();
      if (!((Object) component == (Object) null))
      {
        string startingPos = component.starting_pos;
        if (!(startingPos == "Red Front"))
        {
          if (!(startingPos == "Red Back"))
          {
            if (!(startingPos == "Blue Front"))
            {
              if (startingPos == "Blue Back")
                this.r1_b2.robotid = component;
            }
            else
              this.r1_b1.robotid = component;
          }
          else
            this.r1_r2.robotid = component;
        }
        else
          this.r1_r1.robotid = component;
      }
    }
  }

  public override void ScorerReset()
  {
    base.ScorerReset();
    this.score_red = 0.0;
    this.score_blue = 0.0;
  }

  public override void ScorerUpdate()
  {
    if (this.timerstate != Scorekeeper.TimerState.PAUSED && this.timerstate != Scorekeeper.TimerState.RUNNING)
      this.ScorerReset();
    else
      this.CalculateScores();
  }

  private void CalculateScores()
  {
    this.score_red = (double) this.scorer_red_gold.GetElementCount() * 5.0 + (double) this.scorer_red_silver.GetElementCount() * 5.0 + (double) this.scorer_red_box.GetElementCount() * 2.0;
    this.score_blue = (double) this.scorer_blue_gold.GetElementCount() * 5.0 + (double) this.scorer_blue_silver.GetElementCount() * 5.0 + (double) this.scorer_blue_box.GetElementCount() * 2.0;
  }

  public override int GetRedScore() => this.timerstate == Scorekeeper.TimerState.FINISHED ? this.score_redfinal : (int) this.score_red + this.score_redadj;

  public override int GetBlueScore() => this.timerstate == Scorekeeper.TimerState.FINISHED ? this.score_bluefinal : (int) this.score_blue + this.score_blueadj;
}
