// Decompiled with JetBrains decompiler
// Type: Scorekeeper_Skystone
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class Scorekeeper_Skystone : Scorekeeper
{
  private Skystone_Settings ss_settings;
  public SS_scoringBox scorer_red_tl;
  public SS_scoringBox scorer_blue_tl;
  public SS_scoringBox scorer_red_f;
  public SS_scoringBox scorer_blue_f;
  public SS_scoringBox blue_scoring;
  public SS_scoringBox red_scoring;
  public DepotHandler red_depot;
  public DepotHandler blue_depot;
  public GameObject center_marker;
  private double score_red;
  private double penalties_red;
  private double penalties_blue;
  private double score_blue;
  private Dictionary<int, gameElement> found_elements = new Dictionary<int, gameElement>();
  private List<RobotStates> robotstates = new List<RobotStates>();
  private double red_penalties;
  private double red_skybridge;
  private double red_foundation;
  private double red_highest_block;
  private double blue_penalties;
  private double blue_skybridge;
  private double blue_foundation;
  private double blue_highest_block;

  private void Awake()
  {
    GLOBALS.PlayerCount = 4;
    GLOBALS.TIMER_TOTAL = 120;
    GLOBALS.TIMER_AUTO = 0;
    GLOBALS.TIMER_ENDGAME = 30;
  }

  public override void ScorerInit()
  {
    this.ScorerReset();
    this.ss_settings = GameObject.Find("GameSettings").GetComponent<Skystone_Settings>();
  }

  public override void ScorerReset()
  {
    base.ScorerReset();
    this.score_red = 0.0;
    this.score_blue = 0.0;
    this.penalties_red = 0.0;
    this.penalties_blue = 0.0;
    this.scorer_red_tl.Reset();
    this.scorer_blue_tl.Reset();
    this.scorer_red_f.Reset();
    this.scorer_blue_f.Reset();
  }

  public override void Restart()
  {
    base.Restart();
    this.red_depot.Reset();
    this.blue_depot.Reset();
  }

  public override void ScorerUpdate()
  {
    if (GLOBALS.CLIENT_MODE)
      return;
    bool flag = true;
    if (this.timerstate != Scorekeeper.TimerState.PAUSED && this.timerstate != Scorekeeper.TimerState.RUNNING)
      flag = false;
    this.CalculateScores();
    int count1 = this.robotstates.Count;
    while (count1 >= 1)
    {
      --count1;
      this.robotstates[count1].isblocking = false;
    }
    int count2 = this.robotstates.Count;
    float time = Time.time;
    int count3 = this.robotstates.Count;
    while (count3 >= 1)
    {
      --count3;
      RobotStates robotstate1 = this.robotstates[count3];
      if (!((Object) robotstate1.robot == (Object) null))
      {
        if (robotstate1.counting_down)
        {
          if ((double) time - (double) robotstate1.start_time > (double) this.ss_settings.BLOCKING_DURATION)
          {
            robotstate1.counting_down = false;
            robotstate1.start_time = -1f;
            robotstate1.robot.SetProgressBar(0.0f);
          }
          else
            robotstate1.robot.SetProgressBar((this.ss_settings.BLOCKING_DURATION - time + robotstate1.start_time) / this.ss_settings.BLOCKING_DURATION);
        }
        if (robotstate1.counting_up)
        {
          if (!this.ss_settings.ENABLE_BLOCKING)
          {
            robotstate1.counting_up = false;
            robotstate1.counting_down = true;
            robotstate1.start_time = time - (this.ss_settings.BLOCKING_DURATION - (time - robotstate1.start_time));
          }
          else if ((double) time - (double) robotstate1.start_time > (double) this.ss_settings.BLOCKING_DURATION)
          {
            robotstate1.counting_up = false;
            robotstate1.start_time = -1f;
            robotstate1.robot.SetProgressBar(0.0f);
            robotstate1.robot.MarkForReset(this.ss_settings.BLOCKING_RESET_HOLDING_TIME);
            if (flag)
            {
              if (robotstate1.isRed)
                this.penalties_blue += (double) this.ss_settings.PENALTY_BLOCKING;
              else
                this.penalties_red += (double) this.ss_settings.PENALTY_BLOCKING;
            }
          }
          else
            robotstate1.robot.SetProgressBar((time - robotstate1.start_time) / this.ss_settings.BLOCKING_DURATION);
        }
        foreach (RobotInterface3D allEnemy in robotstate1.robot.GetAllEnemies())
        {
          if (this.ss_settings.ENABLE_FOUNDATION_PENALTY && robotstate1.isRed && this.scorer_red_f.IsFriendInside(robotstate1.robot.transform) || this.ss_settings.ENABLE_FOUNDATION_PENALTY && !robotstate1.isRed && this.scorer_blue_f.IsFriendInside(robotstate1.robot.transform))
          {
            if (!allEnemy.GetNeedsReset())
            {
              allEnemy.MarkForReset(this.ss_settings.BLOCKING_RESET_HOLDING_TIME);
              if (flag && this.ss_settings.ENABLE_FOUNDATION_PENALTY)
              {
                if (robotstate1.isRed)
                  this.penalties_red += (double) this.ss_settings.PENALTY_SCORING;
                else
                  this.penalties_blue += (double) this.ss_settings.PENALTY_SCORING;
              }
            }
            else
              continue;
          }
          int robotStateIndex = this.GetRobotStateIndex(allEnemy);
          if (robotStateIndex >= 0)
          {
            RobotStates robotstate2 = this.robotstates[robotStateIndex];
            if (this.ss_settings.ENABLE_BLOCKING && robotstate1.isRed && this.red_scoring.IsFriendInside(robotstate1.robot.transform) || this.ss_settings.ENABLE_BLOCKING && !robotstate1.isRed && this.blue_scoring.IsFriendInside(robotstate1.robot.transform))
            {
              robotstate2.isblocking = true;
              if (robotstate2.counting_down)
              {
                robotstate2.counting_down = false;
                robotstate2.counting_up = true;
                robotstate2.start_time = time - (this.ss_settings.BLOCKING_DURATION - (time - robotstate2.start_time));
              }
              else if (!robotstate2.counting_up)
              {
                robotstate2.counting_up = true;
                robotstate2.start_time = time;
              }
            }
          }
        }
        int num = 0;
        this.found_elements.Clear();
        if (this.ss_settings.ENABLE_POSSESSION_PENALTY)
        {
          foreach (SS_gameidcounter componentsInChild in robotstate1.robot.GetComponentsInChildren<SS_gameidcounter>())
            num += componentsInChild.GetElements(this.found_elements);
          foreach (gameElement gameElement in this.found_elements.Values)
          {
            if (robotstate1.isRed)
            {
              if (this.scorer_red_f.IsGameElementInside(gameElement))
                --num;
            }
            else if (this.scorer_blue_f.IsGameElementInside(gameElement))
              --num;
          }
          if (num > 1)
          {
            if (robotstate1.toomanyblocks_starttime < 0L)
              robotstate1.toomanyblocks_starttime = MyUtils.GetTimeMillis() + (long) this.ss_settings.PENALTY_POSSESSION_GRACE;
            else if (MyUtils.GetTimeMillis() - robotstate1.toomanyblocks_starttime > 0L)
            {
              if (robotstate1.isRed)
                this.penalties_blue += (double) this.ss_settings.PENALTY_POSSESSION;
              else
                this.penalties_red += (double) this.ss_settings.PENALTY_POSSESSION;
              robotstate1.toomanyblocks_starttime = MyUtils.GetTimeMillis() + 5000L;
            }
          }
        }
      }
    }
    int count4 = this.robotstates.Count;
    while (count4 >= 1 && this.ss_settings.ENABLE_BLOCKING)
    {
      --count4;
      if (!this.robotstates[count4].isblocking && this.robotstates[count4].counting_up)
      {
        this.robotstates[count4].counting_up = false;
        this.robotstates[count4].counting_down = true;
        this.robotstates[count4].start_time = time - (this.ss_settings.BLOCKING_DURATION - (time - this.robotstates[count4].start_time));
      }
    }
  }

  public override void FieldChangedTrigger()
  {
    List<RobotStates> robotStatesList = new List<RobotStates>();
    for (int index = 0; index < this.allrobots.Count; ++index)
    {
      int robotStateIndex = this.GetRobotStateIndex(this.allrobots[index]);
      if (robotStateIndex < 0)
      {
        RobotStates robotStates = new RobotStates();
        robotStates.robot = this.allrobots[index];
        if (!((Object) this.allrobots[index].gameObject == (Object) null) && !((Object) this.allrobots[index].gameObject.GetComponent<RobotID>() == (Object) null))
        {
          robotStates.isRed = this.allrobots[index].gameObject.GetComponent<RobotID>().starting_pos.StartsWith("Red");
          robotStatesList.Add(robotStates);
        }
      }
      else
        robotStatesList.Add(this.robotstates[robotStateIndex]);
    }
    this.robotstates = robotStatesList;
  }

  public override void GetScoreDetails(Dictionary<string, string> data)
  {
    base.GetScoreDetails(data);
    data["PenB"] = ((int) this.blue_penalties).ToString();
    data["PenR"] = ((int) this.red_penalties).ToString();
    data["ScoreR"] = ((int) this.score_red + this.score_redadj).ToString();
    data["ScoreB"] = ((int) this.score_blue + this.score_blueadj).ToString();
    data["RFound"] = ((int) this.red_foundation).ToString();
    data["BFound"] = ((int) this.blue_foundation).ToString();
    data["RSky"] = ((int) this.red_skybridge).ToString();
    data["BSky"] = ((int) this.blue_skybridge).ToString();
    data["RLevel"] = ((int) this.red_highest_block).ToString();
    data["BLevel"] = ((int) this.blue_highest_block).ToString();
  }

  private int GetRobotStateIndex(RobotInterface3D robot)
  {
    for (int index = 0; index < this.robotstates.Count; ++index)
    {
      if ((Object) this.robotstates[index].robot == (Object) robot)
        return index;
    }
    return -1;
  }

  private void CalculateScores()
  {
    if (this.timerstate != Scorekeeper.TimerState.RUNNING)
      return;
    this.red_penalties = (double) (this.scorer_red_tl.penalties + this.scorer_red_f.penalties) + this.penalties_red;
    this.red_skybridge = (double) this.scorer_red_tl.score;
    this.red_foundation = (double) this.scorer_red_f.GetElementCount();
    this.red_highest_block = (double) (this.scorer_red_f.GetHighestBlock() * 2);
    this.blue_penalties = (double) (this.scorer_blue_tl.penalties + this.scorer_blue_f.penalties) + this.penalties_blue;
    this.blue_skybridge = (double) this.scorer_blue_tl.score;
    this.blue_foundation = (double) this.scorer_blue_f.GetElementCount();
    this.blue_highest_block = (double) (this.scorer_blue_f.GetHighestBlock() * 2);
    this.score_red = this.red_skybridge + this.red_foundation + this.red_highest_block + this.red_penalties;
    this.score_blue = this.blue_skybridge + this.blue_foundation + this.blue_highest_block + this.blue_penalties;
  }

  public override int GetRedScore() => this.timerstate == Scorekeeper.TimerState.FINISHED ? this.score_redfinal : (int) this.score_red + this.score_redadj;

  public override int GetBlueScore() => this.timerstate == Scorekeeper.TimerState.FINISHED ? this.score_bluefinal : (int) this.score_blue + this.score_blueadj;
}
