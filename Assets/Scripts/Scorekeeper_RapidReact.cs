// Decompiled with JetBrains decompiler
// Type: Scorekeeper_RapidReact
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scorekeeper_RapidReact : Scorekeeper
{
  private RapidReact_Settings rr_settings;
  public List<TarmacTracker> tarmac_list = new List<TarmacTracker>();
  public List<RR_GoalScorer> top_scorers = new List<RR_GoalScorer>();
  public List<RR_GoalScorer> bot_scorers = new List<RR_GoalScorer>();
  public GameObject pushback;
  public GenericFieldTracker red_low_bar;
  public GenericFieldTracker red_mid_bar;
  public GenericFieldTracker red_high_bar;
  public GenericFieldTracker red_ultrahigh_bar;
  public GenericFieldTracker red_floor;
  public GenericFieldTracker blue_low_bar;
  public GenericFieldTracker blue_mid_bar;
  public GenericFieldTracker blue_high_bar;
  public GenericFieldTracker blue_ultrahigh_bar;
  public GenericFieldTracker blue_floor;
  public GenericFieldTracker lp_red1;
  public GenericFieldTracker lp_red2;
  public GenericFieldTracker lp_blue1;
  public GenericFieldTracker lp_blue2;
  public GenericFieldTracker hanger_red;
  public GenericFieldTracker hanger_blue;
  public Transform preload_redl;
  public Transform preload_redc;
  public Transform preload_redr;
  public Transform preload_bluel;
  public Transform preload_bluec;
  public Transform preload_bluer;
  public double penalties_red;
  public int cargo_red;
  public double score_auto_red;
  public double score_teleop_red;
  public double score_endgame_red;
  public double score_red;
  public double penalties_blue;
  public int cargo_high_blue;
  public int auto_cargo_high_blue;
  public int cargo_low_blue;
  public int auto_cargo_low_blue;
  public int cargo_high_red;
  public int auto_cargo_high_red;
  public int cargo_low_red;
  public int auto_cargo_low_red;
  public int hang_low_red;
  public int hang_mid_red;
  public int hang_high_red;
  public int hang_traverse_red;
  public int hang_low_blue;
  public int hang_mid_blue;
  public int hang_high_blue;
  public int hang_traverse_blue;
  public int taxi_red;
  public int taxi_blue;
  public double score_auto_blue;
  public double score_teleop_blue;
  public double score_endgame_blue;
  public double score_blue;
  public GameObject powerup_overlay;
  [Header("PowerUps")]
  public PowerUpScript powerup_center;
  public List<PowerUpScript> powerup_home = new List<PowerUpScript>();
  public Dictionary<string, float> player_opr_endgame = new Dictionary<string, float>();
  private List<int> hang_awarded_foul = new List<int>();
  private Vector3 old_gravity;
  private GameObject fault_prefab;
  private string mark_string = "";
  private int preload_delay;
  private string foul_string = "";
  private List<Scorekeeper_RapidReact.RobotStates> allrobotstates = new List<Scorekeeper_RapidReact.RobotStates>();
  private Vector3 delta_preload = Vector3.zero;
  private int start_counter;
  private int animation_played;
  private string old_mark_string = "";

  private void Awake()
  {
    GLOBALS.PlayerCount = 6;
    GLOBALS.TIMER_TOTAL = 150;
    GLOBALS.TIMER_AUTO = 15;
    GLOBALS.TIMER_ENDGAME = 30;
    this.old_gravity = Physics.gravity;
    Physics.gravity = new Vector3(0.0f, -9.81f, 0.0f);
  }

  private void OnDestroy() => Physics.gravity = this.old_gravity;

  public override void ScorerInit()
  {
    this.ScorerReset();
    this.rr_settings = GameObject.Find("GameSettings").GetComponent<RapidReact_Settings>();
    this.fault_prefab = UnityEngine.Resources.Load("Prefabs/FaultAnimation") as GameObject;
    this.powerup_center.myscorekeeper = (Scorekeeper) this;
    foreach (PowerUpScript powerUpScript in this.powerup_home)
      powerUpScript.myscorekeeper = (Scorekeeper) this;
  }

  public override void ScorerReset()
  {
    base.ScorerReset();
    this.penalties_red = 0.0;
    this.cargo_red = 0;
    this.score_auto_red = 0.0;
    this.score_teleop_red = 0.0;
    this.score_endgame_red = 0.0;
    this.score_red = 0.0;
    this.taxi_red = 0;
    this.taxi_blue = 0;
    this.penalties_blue = 0.0;
    this.score_auto_blue = 0.0;
    this.score_teleop_blue = 0.0;
    this.score_endgame_blue = 0.0;
    this.score_blue = 0.0;
    this.cargo_high_blue = 0;
    this.auto_cargo_high_blue = 0;
    this.cargo_low_blue = 0;
    this.auto_cargo_low_blue = 0;
    this.cargo_high_red = 0;
    this.auto_cargo_high_red = 0;
    this.cargo_low_red = 0;
    this.auto_cargo_low_red = 0;
    this.hang_awarded_foul.Clear();
    this.mark_string = "";
    this.player_opr_endgame.Clear();
    foreach (string key in this.allrobots_byname.Keys)
      this.player_opr_endgame[key] = 0.0f;
    this.ResetRobotStates();
    this.ResetPowerUps();
    this.ResetBalls();
    foreach (GenericFieldTracker topScorer in this.top_scorers)
      topScorer.Reset();
    foreach (GenericFieldTracker botScorer in this.bot_scorers)
      botScorer.Reset();
  }

  public override void OnTimerStart()
  {
    base.OnTimerStart();
    this.start_counter = 2;
    this.preload_delay = 2;
  }

  public void MyTimerStart()
  {
    foreach (TarmacTracker tarmac in this.tarmac_list)
      tarmac.MarkStartingRobots();
    this.start_counter = -1;
  }

  public override void GetScoreDetails(Dictionary<string, string> data)
  {
    base.GetScoreDetails(data);
    data["AutoR"] = ((int) this.score_auto_red).ToString();
    data["AutoB"] = ((int) this.score_auto_blue).ToString();
    data["TeleR"] = ((int) this.score_teleop_red).ToString();
    data["TeleB"] = ((int) this.score_teleop_blue).ToString();
    data["EndR"] = ((int) this.score_endgame_red).ToString();
    data["EndB"] = ((int) this.score_endgame_blue).ToString();
    data["ScoreR"] = ((int) this.score_red + this.score_redadj).ToString();
    data["ScoreB"] = ((int) this.score_blue + this.score_blueadj).ToString();
    data["PenB"] = ((int) this.penalties_blue).ToString();
    data["PenR"] = ((int) this.penalties_red).ToString();
    data["Auto_Taxi_R"] = this.taxi_red.ToString();
    data["Auto_Taxi_B"] = this.taxi_blue.ToString();
    data["Auto_C_H_R"] = this.auto_cargo_high_red.ToString();
    data["Auto_C_L_R"] = this.auto_cargo_low_red.ToString();
    data["Auto_C_H_B"] = this.auto_cargo_high_blue.ToString();
    data["Auto_C_L_B"] = this.auto_cargo_low_blue.ToString();
    data["C_H_R"] = this.cargo_high_red.ToString();
    data["C_L_R"] = this.cargo_low_red.ToString();
    data["C_H_B"] = this.cargo_high_blue.ToString();
    data["C_L_B"] = this.cargo_low_blue.ToString();
    data["H_L_B"] = this.hang_low_blue.ToString();
    data["H_L_R"] = this.hang_low_red.ToString();
    data["H_M_B"] = this.hang_mid_blue.ToString();
    data["H_M_R"] = this.hang_mid_red.ToString();
    data["H_H_B"] = this.hang_high_blue.ToString();
    data["H_H_R"] = this.hang_high_red.ToString();
    data["H_T_B"] = this.hang_traverse_blue.ToString();
    data["H_T_R"] = this.hang_traverse_red.ToString();
  }

  public string GetDetails(bool red = true)
  {
    Dictionary<string, string> data = new Dictionary<string, string>();
    this.GetScoreDetails(data);
    if (GLOBALS.CLIENT_MODE)
    {
      foreach (string key in MyUtils.score_details.Keys)
        data[key] = MyUtils.score_details[key];
    }
    string str = red ? "R" : "B";
    return "<B>AUTO Score   = </B> " + data["Auto" + str] + "\n    # Taxi: " + data["Auto_Taxi_" + str] + "\n    # Cargo in Low: " + data["Auto_C_L_" + str] + "\n    # Cargo in High: " + data["Auto_C_H_" + str] + "\n\n<B>TELEOP Score =</B> " + data["Tele" + str] + "\n    # Cargo in Low: " + data["C_L_" + str] + "\n    # Cargo in High: " + data["C_H_" + str] + "\n\n<B>ENDGAME Score=</B> " + data["End" + str] + "\n    # Hang Low: " + data["H_L_" + str] + "\n    # Hang Mid: " + data["H_M_" + str] + "\n    # Hang High: " + data["H_H_" + str] + "\n    # Hang Traverse: " + data["H_T_" + str] + "\n";
  }

  public override void Restart() => base.Restart();

  private void UpdatePowerUps()
  {
    if (GLOBALS.CLIENT_MODE)
      return;
    foreach (Scorekeeper_RapidReact.RobotStates allrobotstate in this.allrobotstates)
    {
      for (int index = allrobotstate.myoffensive_powerups.Count - 1; index >= 0; --index)
      {
        if (allrobotstate.myoffensive_powerups[index].GetRemainingTime() < 0L)
          allrobotstate.myoffensive_powerups.RemoveAt(index);
      }
      for (int index = allrobotstate.mydeffensive_powerups.Count - 1; index >= 0; --index)
      {
        if (allrobotstate.mydeffensive_powerups[index].GetRemainingTime() < 0L)
          allrobotstate.mydeffensive_powerups.RemoveAt(index);
      }
    }
    foreach (PowerUpScript current_pu in this.powerup_home)
      this.ServicePowerUp(current_pu, this.rr_settings.PU_HOME, this.rr_settings.PU_HOME_TYPE);
    this.ServicePowerUp(this.powerup_center, this.rr_settings.PU_CENTER, this.rr_settings.PU_CENTER_TYPE);
    foreach (Scorekeeper_RapidReact.RobotStates allrobotstate in this.allrobotstates)
    {
      allrobotstate.robot.TweakPerformance(0.0f, 0.0f);
      allrobotstate.robot.ScaleStickControls(1f);
      allrobotstate.robot.TurnOnRenderers();
      for (int index = allrobotstate.myoffensive_powerups.Count - 1; index >= 0; --index)
      {
        Scorekeeper_RapidReact.PowerUpData myoffensivePowerup = allrobotstate.myoffensive_powerups[index];
        this.PU_ApplyPowerup(allrobotstate.robot, myoffensivePowerup.mypowerup);
      }
      for (int index = allrobotstate.mydeffensive_powerups.Count - 1; index >= 0; --index)
      {
        Scorekeeper_RapidReact.PowerUpData mydeffensivePowerup = allrobotstate.mydeffensive_powerups[index];
        this.PU_ApplyPowerup(allrobotstate.robot, mydeffensivePowerup.mypowerup);
      }
    }
  }

  private void PU_ApplyPowerup(RobotInterface3D robot, PowerUpType powerup)
  {
    this.clean_run = false;
    switch (powerup)
    {
      case PowerUpType.SPEED:
        robot.TweakPerformance((float) (1.0 + (double) this.rr_settings.PU_STRENGTH / 100.0), 1f);
        break;
      case PowerUpType.TORQUE:
        robot.TweakPerformance(1f, (float) (1.0 + 3.0 * ((double) this.rr_settings.PU_STRENGTH / 100.0)));
        break;
      case PowerUpType.INVISIBILITY:
        robot.TurnOffRenderers(true);
        break;
      case PowerUpType.SLOW:
        robot.TweakPerformance((float) (1.0 / (1.0 + (double) this.rr_settings.PU_STRENGTH / 100.0)), 1f);
        break;
      case PowerUpType.WEAK:
        robot.TweakPerformance(1f, (float) (1.0 / (1.0 + (double) this.rr_settings.PU_STRENGTH / 100.0)));
        break;
      case PowerUpType.INVERTED:
        robot.ScaleStickControls(-1f);
        break;
    }
  }

  private void ServicePowerUp(PowerUpScript current_pu, bool isenabled, int type_allowed)
  {
    if (GLOBALS.CLIENT_MODE)
      return;
    if (current_pu.NeedsServicing() && (bool) (Object) current_pu.GetOwner())
    {
      current_pu.Serviced();
      Scorekeeper_RapidReact.RobotStates robotState = this.GetRobotState(current_pu.GetOwner());
      if (robotState == null)
        return;
      Scorekeeper_RapidReact.PowerUpData powerUpData = new Scorekeeper_RapidReact.PowerUpData();
      powerUpData.start_time = MyUtils.GetTimeMillisSinceStart();
      powerUpData.mypowerup = current_pu.myPower;
      switch (powerUpData.mypowerup)
      {
        case PowerUpType.SPEED:
        case PowerUpType.TORQUE:
        case PowerUpType.INVISIBILITY:
          if (!this.rr_settings.PU_ONLYONE || robotState.myoffensive_powerups.Count < 1)
          {
            powerUpData.duration = (long) (this.rr_settings.PU_OFFENSIVE_DURATION * 1000);
            bool flag = false;
            foreach (Scorekeeper_RapidReact.PowerUpData myoffensivePowerup in robotState.myoffensive_powerups)
            {
              if (myoffensivePowerup.mypowerup == powerUpData.mypowerup)
              {
                myoffensivePowerup.SetRemainingTime(powerUpData.GetRemainingTime());
                flag = true;
                break;
              }
            }
            if (!flag)
            {
              robotState.myoffensive_powerups.Add(powerUpData);
              break;
            }
            break;
          }
          break;
        case PowerUpType.SLOW:
        case PowerUpType.WEAK:
        case PowerUpType.INVERTED:
          powerUpData.duration = (long) (this.rr_settings.PU_DEFENSIVE_DURATION * 1000);
          using (List<Scorekeeper_RapidReact.RobotStates>.Enumerator enumerator = this.allrobotstates.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              Scorekeeper_RapidReact.RobotStates current = enumerator.Current;
              if (current.isRed != robotState.isRed)
                current.mydeffensive_powerups.Add(powerUpData);
            }
            break;
          }
      }
    }
    if (this.rr_settings.ENABLE_POWERUPS & isenabled)
    {
      if (!current_pu.IsDisabled() || MyUtils.GetTimeMillisSinceStart() - current_pu.GetTimeStarted() <= (long) (this.rr_settings.PU_RESPAWN * 1000))
        return;
      List<PowerUpType> powerUpTypeList = new List<PowerUpType>();
      if (type_allowed == 0 || type_allowed == 2)
      {
        if (this.rr_settings.PU_SPEED)
          powerUpTypeList.Add(PowerUpType.SPEED);
        if (this.rr_settings.PU_TORQUE)
          powerUpTypeList.Add(PowerUpType.TORQUE);
        if (this.rr_settings.PU_INVISIBLE)
          powerUpTypeList.Add(PowerUpType.INVISIBILITY);
      }
      if (type_allowed == 1 || type_allowed == 2)
      {
        if (this.rr_settings.PU_SLOW)
          powerUpTypeList.Add(PowerUpType.SLOW);
        if (this.rr_settings.PU_WEAK)
          powerUpTypeList.Add(PowerUpType.WEAK);
        if (this.rr_settings.PU_INVERTED)
          powerUpTypeList.Add(PowerUpType.INVERTED);
      }
      if (powerUpTypeList.Count < 1)
        return;
      int index = Random.Range(0, powerUpTypeList.Count);
      current_pu.PU_Enable(powerUpTypeList[index]);
    }
    else
      current_pu.PU_Disable();
  }

  public override bool PU_CheckIfClearToAssign(PowerUpScript thepu, RobotInterface3D robot)
  {
    Scorekeeper_RapidReact.RobotStates robotState = this.GetRobotState(robot);
    return robotState != null && (robotState.myoffensive_powerups.Count <= 0 || !this.rr_settings.PU_ONLYONE || thepu.myPower >= PowerUpType.SLOW);
  }

  public override Transform GetOverlays(int id, Transform parent)
  {
    Scorekeeper_RapidReact.RobotStates robotStateById = this.GetRobotStateById(id);
    if (robotStateById == null)
      return (Transform) null;
    if (robotStateById.mydeffensive_powerups.Count == 0 && robotStateById.myoffensive_powerups.Count == 0)
      return (Transform) null;
    int num = 10;
    foreach (Scorekeeper_RapidReact.PowerUpData myoffensivePowerup in robotStateById.myoffensive_powerups)
    {
      GameObject gameObject = Object.Instantiate<GameObject>(this.powerup_overlay, parent.transform);
      gameObject.transform.Find("Time").GetComponent<Text>().text = (myoffensivePowerup.GetRemainingTime() / 1000L).ToString();
      gameObject.transform.Find("Type").GetComponent<Text>().text = myoffensivePowerup.GetPowerChar();
      gameObject.transform.localPosition = gameObject.transform.localPosition with
      {
        y = (float) num
      };
      num += 45;
    }
    foreach (Scorekeeper_RapidReact.PowerUpData mydeffensivePowerup in robotStateById.mydeffensive_powerups)
    {
      GameObject gameObject = Object.Instantiate<GameObject>(this.powerup_overlay, parent.transform);
      gameObject.transform.Find("Time").GetComponent<Text>().text = (mydeffensivePowerup.GetRemainingTime() / 1000L).ToString();
      gameObject.transform.Find("Type").GetComponent<Text>().text = mydeffensivePowerup.GetPowerChar();
      gameObject.transform.localPosition = gameObject.transform.localPosition with
      {
        y = (float) num
      };
      num += 45;
    }
    return parent.transform;
  }

  public override string GetOverlaysString(int id)
  {
    Scorekeeper_RapidReact.RobotStates robotStateById = this.GetRobotStateById(id);
    if (robotStateById == null || robotStateById.mydeffensive_powerups.Count == 0 && robotStateById.myoffensive_powerups.Count == 0)
      return "";
    string overlaysString = "";
    foreach (Scorekeeper_RapidReact.PowerUpData myoffensivePowerup in robotStateById.myoffensive_powerups)
      overlaysString = overlaysString + ":" + ((int) myoffensivePowerup.mypowerup).ToString() + ":" + (object) (myoffensivePowerup.GetRemainingTime() / 1000L);
    foreach (Scorekeeper_RapidReact.PowerUpData mydeffensivePowerup in robotStateById.mydeffensive_powerups)
      overlaysString = overlaysString + ":" + ((int) mydeffensivePowerup.mypowerup).ToString() + ":" + (object) (mydeffensivePowerup.GetRemainingTime() / 1000L);
    return overlaysString;
  }

  public override void ScorerUpdate()
  {
    bool flag = true;
    if (this.timerstate != Scorekeeper.TimerState.PAUSED && this.timerstate != Scorekeeper.TimerState.RUNNING)
    {
      flag = false;
      this.pushback.SetActive(false);
    }
    else if (this.time_total.TotalSeconds > (double) (GLOBALS.TIMER_TOTAL - GLOBALS.TIMER_AUTO))
      this.pushback.SetActive(this.rr_settings.ENABLE_AUTO_PUSHBACK);
    else
      this.pushback.SetActive(false);
    if (this.preload_delay > 0)
    {
      --this.preload_delay;
      if (this.preload_delay == 0)
        this.PreloadAll();
    }
    if (this.rr_settings.ENABLE_LP_FOUL & flag)
      this.Do_LaunchPads();
    this.Do_HangFoul();
    this.UpdatePowerUps();
    this.CalculateScores();
    if (GLOBALS.CLIENT_MODE)
      return;
    int count = this.allrobotstates.Count;
    while (count >= 1)
    {
      --count;
      this.allrobotstates[count].isblocking = false;
    }
  }

  public override void RobotCounterExpired(RobotID bot)
  {
  }

  private void Do_IRL_Blocking()
  {
    if (GLOBALS.CLIENT_MODE)
      return;
    int count = this.allrobotstates.Count;
    while (count >= 1)
    {
      --count;
      int num = (bool) (Object) this.allrobotstates[count].robot ? 1 : 0;
    }
  }

  private void CreateFoul(int robotid)
  {
    if (!this.allrobots_byid.ContainsKey(robotid))
      return;
    RobotInterface3D robotInterface3D = this.allrobots_byid[robotid];
    if (!(bool) (Object) robotInterface3D.rb_body)
      return;
    if (!GLOBALS.HEADLESS_MODE)
      Object.Instantiate<GameObject>(this.fault_prefab, robotInterface3D.rb_body.transform.position, Quaternion.identity).transform.SetParent(robotInterface3D.rb_body.transform);
    robotInterface3D.OverrideColor(0);
    if (!GLOBALS.SERVER_MODE)
      return;
    if (this.foul_string.Length > 0)
      this.foul_string += ":";
    this.foul_string += (string) (object) robotid;
  }

  private void MarkRobotForHang(int robotid, bool mark)
  {
    if (!this.allrobots_byid.ContainsKey(robotid))
      return;
    RobotInterface3D robotInterface3D = this.allrobots_byid[robotid];
    if (!(bool) (Object) robotInterface3D.rb_body)
      return;
    robotInterface3D.OverrideColor(mark ? 3 : -1);
    if (!GLOBALS.SERVER_MODE)
      return;
    if (this.foul_string.Length > 0)
      this.foul_string += ":";
    this.foul_string += (string) (object) robotid;
  }

  public override void FieldChangedTrigger()
  {
    List<Scorekeeper_RapidReact.RobotStates> robotStatesList = new List<Scorekeeper_RapidReact.RobotStates>();
    for (int index = 0; index < this.allrobots.Count; ++index)
    {
      int robotStateIndex = this.GetRobotStateIndex(this.allrobots[index]);
      if (robotStateIndex < 0)
      {
        Scorekeeper_RapidReact.RobotStates robotStates = new Scorekeeper_RapidReact.RobotStates();
        robotStates.robot = this.allrobots[index];
        if (!((Object) this.allrobots[index].gameObject == (Object) null) && !((Object) this.allrobots[index].gameObject.GetComponent<RobotID>() == (Object) null))
        {
          robotStates.isRed = this.allrobots[index].gameObject.GetComponent<RobotID>().starting_pos.StartsWith("Red");
          robotStates.robotID = robotStates.robot.myRobotID;
          robotStatesList.Add(robotStates);
        }
      }
      else
        robotStatesList.Add(this.allrobotstates[robotStateIndex]);
    }
    this.allrobotstates = robotStatesList;
  }

  private void ResetRobotStates()
  {
    foreach (Scorekeeper_RapidReact.RobotStates allrobotstate in this.allrobotstates)
    {
      allrobotstate.Reset();
      if ((bool) (Object) allrobotstate.robot)
      {
        allrobotstate.robot.TweakPerformance(-1f, -1f);
        allrobotstate.robot.SetProgressBar(0.0f);
        allrobotstate.robot.HoldRobot(false);
      }
    }
  }

  private void ResetBalls()
  {
    foreach (ball_data ballData in Object.FindObjectsOfType<ball_data>())
      ballData.Clear();
  }

  private void PreloadAll()
  {
    foreach (RobotInterface3D allrobot in this.allrobots)
    {
      string startingPos = allrobot.myRobotID.starting_pos;
      if (!(startingPos == "Red Left"))
      {
        if (!(startingPos == "Red Center"))
        {
          if (!(startingPos == "Red Right"))
          {
            if (!(startingPos == "Blue Left"))
            {
              if (!(startingPos == "Blue Center"))
              {
                if (startingPos == "Blue Right")
                  this.PreloadItem(allrobot, this.preload_bluer);
              }
              else
                this.PreloadItem(allrobot, this.preload_bluec);
            }
            else
              this.PreloadItem(allrobot, this.preload_bluel);
          }
          else
            this.PreloadItem(allrobot, this.preload_redr);
        }
        else
          this.PreloadItem(allrobot, this.preload_redc);
      }
      else
        this.PreloadItem(allrobot, this.preload_redl);
    }
  }

  private void PreloadItem(RobotInterface3D bot, Transform preload_items)
  {
    PreloadID componentInChildren = bot.GetComponentInChildren<PreloadID>();
    if (!(bool) (Object) componentInChildren)
      return;
    preload_items.SetPositionAndRotation(componentInChildren.transform.position, componentInChildren.transform.rotation * preload_items.rotation);
  }

  private int GetRobotStateIndex(RobotInterface3D robot)
  {
    for (int index = 0; index < this.allrobotstates.Count; ++index)
    {
      if ((Object) this.allrobotstates[index].robot == (Object) robot)
        return index;
    }
    return -1;
  }

  private Scorekeeper_RapidReact.RobotStates GetRobotState(
    RobotInterface3D robot)
  {
    for (int index = 0; index < this.allrobotstates.Count; ++index)
    {
      if ((Object) this.allrobotstates[index].robot == (Object) robot)
        return this.allrobotstates[index];
    }
    return (Scorekeeper_RapidReact.RobotStates) null;
  }

  private Scorekeeper_RapidReact.RobotStates GetRobotStateById(int id) => !this.allrobots_byid.ContainsKey(id) ? (Scorekeeper_RapidReact.RobotStates) null : this.GetRobotState(this.allrobots_byid[id]);

  private void AddToPlayerOPR(List<ball_data> allballs, float multiplier, bool is_red)
  {
    foreach (ball_data allball in allballs)
    {
      if (!((Object) allball.thrown_robotid == (Object) null) && allball.thrown_robotid.is_red == is_red)
      {
        int thrownById = allball.thrown_by_id;
        if (this.player_opr.ContainsKey(GLOBALS.client_names[thrownById]))
          this.player_opr[GLOBALS.client_names[thrownById]] += multiplier;
        else
          this.player_opr[GLOBALS.client_names[thrownById]] = multiplier;
      }
    }
  }

  private void AddToPlayerOPR(int id, float amount)
  {
    if (this.player_opr.ContainsKey(GLOBALS.client_names[id]))
      this.player_opr[GLOBALS.client_names[id]] += amount;
    else
      this.player_opr[GLOBALS.client_names[id]] = amount;
  }

  private void CalculateScores()
  {
    if (this.timerstate != Scorekeeper.TimerState.RUNNING)
      return;
    if (this.start_counter >= 0)
    {
      --this.start_counter;
      if (this.start_counter == 0)
        this.MyTimerStart();
    }
    if (this.time_total.TotalSeconds > (double) (GLOBALS.TIMER_TOTAL - GLOBALS.TIMER_AUTO))
    {
      foreach (RobotID robotId in this.allRobotID.Values)
      {
        if (robotId.GetUserBool("Taxi") && !robotId.GetUserBool("TaxiCounted"))
        {
          this.AddToPlayerOPR(robotId.id, 2f);
          robotId.SetUserBool("TaxiCounted");
          if (robotId.is_red)
          {
            ++this.taxi_red;
            this.score_auto_red += 2.0;
          }
          else
          {
            ++this.taxi_blue;
            this.score_auto_blue += 2.0;
          }
        }
      }
    }
    if (this.time_total.TotalSeconds > (double) (GLOBALS.TIMER_TOTAL - GLOBALS.TIMER_AUTO) - 5.0)
    {
      foreach (RR_GoalScorer topScorer in this.top_scorers)
      {
        this.auto_cargo_high_blue += topScorer.GetblueBallCount();
        this.score_auto_blue += 4.0 * (double) topScorer.GetblueBallCount();
        this.auto_cargo_high_red += topScorer.GetRedBallCount();
        this.score_auto_red += 4.0 * (double) topScorer.GetRedBallCount();
        this.AddToPlayerOPR(topScorer.red_balls, 4f, true);
        this.AddToPlayerOPR(topScorer.blue_balls, 4f, false);
        topScorer.ClearData();
      }
      foreach (RR_GoalScorer botScorer in this.bot_scorers)
      {
        this.auto_cargo_low_blue += botScorer.GetblueBallCount();
        this.score_auto_blue += 2.0 * (double) botScorer.GetblueBallCount();
        this.auto_cargo_low_red += botScorer.GetRedBallCount();
        this.score_auto_red += 2.0 * (double) botScorer.GetRedBallCount();
        this.AddToPlayerOPR(botScorer.red_balls, 2f, true);
        this.AddToPlayerOPR(botScorer.blue_balls, 2f, false);
        botScorer.ClearData();
      }
    }
    else
    {
      foreach (RR_GoalScorer topScorer in this.top_scorers)
      {
        this.cargo_high_blue += topScorer.GetblueBallCount();
        this.score_teleop_blue += 2.0 * (double) topScorer.GetblueBallCount();
        this.cargo_high_red += topScorer.GetRedBallCount();
        this.score_teleop_red += 2.0 * (double) topScorer.GetRedBallCount();
        this.AddToPlayerOPR(topScorer.red_balls, 2f, true);
        this.AddToPlayerOPR(topScorer.blue_balls, 2f, false);
        topScorer.ClearData();
      }
      foreach (RR_GoalScorer botScorer in this.bot_scorers)
      {
        this.cargo_low_blue += botScorer.GetblueBallCount();
        this.score_teleop_blue += 1.0 * (double) botScorer.GetblueBallCount();
        this.cargo_low_red += botScorer.GetRedBallCount();
        this.score_teleop_red += 1.0 * (double) botScorer.GetRedBallCount();
        this.AddToPlayerOPR(botScorer.red_balls, 1f, true);
        this.AddToPlayerOPR(botScorer.blue_balls, 1f, false);
        botScorer.ClearData();
      }
    }
    this.score_endgame_red = 0.0;
    this.score_endgame_blue = 0.0;
    this.player_opr_endgame.Clear();
    this.hang_low_red = 0;
    this.hang_mid_red = 0;
    this.hang_high_red = 0;
    this.hang_traverse_red = 0;
    this.hang_low_blue = 0;
    this.hang_mid_blue = 0;
    this.hang_high_blue = 0;
    this.hang_traverse_blue = 0;
    foreach (RobotID robot in this.allRobotID.Values)
    {
      if (!((Object) robot == (Object) null))
      {
        if (robot.is_red)
        {
          if (this.GetRobotStateById(robot.id) != null)
          {
            if (this.hang_awarded_foul.Contains(robot.id))
            {
              ++this.hang_traverse_red;
              this.player_opr_endgame[GLOBALS.client_names[robot.id]] = 15f;
            }
            else if (!this.red_floor.IsRobotInside(robot))
            {
              if (this.red_low_bar.IsRobotInside(robot))
              {
                ++this.hang_low_red;
                this.player_opr_endgame[GLOBALS.client_names[robot.id]] = 4f;
              }
              else if (this.red_mid_bar.IsRobotInside(robot))
              {
                ++this.hang_mid_red;
                this.player_opr_endgame[GLOBALS.client_names[robot.id]] = 6f;
              }
              else if (this.red_high_bar.IsRobotInside(robot))
              {
                ++this.hang_high_red;
                this.player_opr_endgame[GLOBALS.client_names[robot.id]] = 10f;
              }
              else if (this.red_ultrahigh_bar.IsRobotInside(robot))
              {
                ++this.hang_traverse_red;
                this.player_opr_endgame[GLOBALS.client_names[robot.id]] = 15f;
              }
            }
          }
        }
        else if (this.GetRobotStateById(robot.id) != null)
        {
          if (this.hang_awarded_foul.Contains(robot.id))
          {
            ++this.hang_traverse_blue;
            this.player_opr_endgame[GLOBALS.client_names[robot.id]] = 15f;
          }
          else if (!this.blue_floor.IsRobotInside(robot))
          {
            if (this.blue_low_bar.IsRobotInside(robot))
            {
              ++this.hang_low_blue;
              this.player_opr_endgame[GLOBALS.client_names[robot.id]] = 4f;
            }
            else if (this.blue_mid_bar.IsRobotInside(robot))
            {
              ++this.hang_mid_blue;
              this.player_opr_endgame[GLOBALS.client_names[robot.id]] = 6f;
            }
            else if (this.blue_high_bar.IsRobotInside(robot))
            {
              ++this.hang_high_blue;
              this.player_opr_endgame[GLOBALS.client_names[robot.id]] = 10f;
            }
            else if (this.blue_ultrahigh_bar.IsRobotInside(robot))
            {
              ++this.hang_traverse_blue;
              this.player_opr_endgame[GLOBALS.client_names[robot.id]] = 15f;
            }
          }
        }
      }
    }
    this.score_endgame_red = (double) (4 * this.hang_low_red + 6 * this.hang_mid_red + 10 * this.hang_high_red + 15 * this.hang_traverse_red);
    this.score_endgame_blue = (double) (4 * this.hang_low_blue + 6 * this.hang_mid_blue + 10 * this.hang_high_blue + 15 * this.hang_traverse_blue);
  }

  private void Do_LaunchPads()
  {
    if (GLOBALS.CLIENT_MODE)
      return;
    int count = this.allrobotstates.Count;
    while (count >= 1)
    {
      --count;
      Scorekeeper_RapidReact.RobotStates allrobotstate = this.allrobotstates[count];
      if ((bool) (Object) allrobotstate.robot)
      {
        this.Foul_Release(allrobotstate);
        if (allrobotstate.isRed && (this.lp_red1.IsRobotInside(allrobotstate.robotID) || this.lp_red2.IsRobotInside(allrobotstate.robotID)) || !allrobotstate.isRed && (this.lp_blue1.IsRobotInside(allrobotstate.robotID) || this.lp_blue2.IsRobotInside(allrobotstate.robotID)))
        {
          foreach (RobotInterface3D enemy in allrobotstate.robot.enemies)
          {
            Scorekeeper_RapidReact.RobotStates robotState = this.GetRobotState(enemy);
            if (!robotState.immune_to_foul)
            {
              robotState.immune_to_foul = true;
              robotState.LP_StartTime = MyUtils.GetTimeMillis();
              if (allrobotstate.isRed)
                this.penalties_red += (double) this.rr_settings.LP_FOUL_POINTS;
              else
                this.penalties_blue += (double) this.rr_settings.LP_FOUL_POINTS;
              this.player_opr[GLOBALS.client_names[allrobotstate.robotID.id]] += (float) this.rr_settings.LP_FOUL_POINTS;
              this.CreateFoul(robotState.robotID.id);
            }
          }
        }
      }
    }
  }

  private void Foul_Release(Scorekeeper_RapidReact.RobotStates currbot)
  {
    if (!currbot.immune_to_foul || (double) (int) (MyUtils.GetTimeMillis() - currbot.LP_StartTime) < 1000.0 * (double) this.rr_settings.FOUL_BLANKING)
      return;
    currbot.immune_to_foul = false;
    currbot.robot.OverrideColor(-1);
  }

  private void Do_HangFoul()
  {
    if (GLOBALS.CLIENT_MODE || this.time_total.TotalSeconds > (double) GLOBALS.TIMER_ENDGAME)
      return;
    int count = this.allrobotstates.Count;
    while (count >= 1)
    {
      --count;
      Scorekeeper_RapidReact.RobotStates allrobotstate = this.allrobotstates[count];
      if ((bool) (Object) allrobotstate.robot)
      {
        if (this.hang_awarded_foul.Contains(allrobotstate.robotID.id))
          break;
        if ((allrobotstate.isRed && this.hanger_red.IsRobotInside(allrobotstate.robotID) || !allrobotstate.isRed && this.hanger_blue.IsRobotInside(allrobotstate.robotID)) && allrobotstate.robot.enemies.Count > 0)
        {
          this.hang_awarded_foul.Add(allrobotstate.robotID.id);
          this.CreateFoul(this.GetRobotState(allrobotstate.robot.enemies[0]).robotID.id);
          allrobotstate.robot.enemies[0].OverrideColor(-1);
          this.MarkRobotForHang(allrobotstate.robotID.id, true);
        }
      }
    }
  }

  public override void DoTimerFinished()
  {
    base.DoTimerFinished();
    foreach (string key in this.player_opr_endgame.Keys)
    {
      if (this.player_opr.ContainsKey(key))
        this.player_opr[key] += this.player_opr_endgame[key];
      else
        this.player_opr[key] = this.player_opr_endgame[key];
    }
  }

  public override int GetRedScore()
  {
    if (this.timerstate == Scorekeeper.TimerState.FINISHED)
      return this.score_redfinal;
    this.score_red = this.score_auto_red + this.score_teleop_red + this.score_endgame_red + this.penalties_red;
    return (int) this.score_red + this.score_redadj;
  }

  public override int GetBlueScore()
  {
    if (this.timerstate == Scorekeeper.TimerState.FINISHED)
      return this.score_bluefinal;
    this.score_blue = this.score_auto_blue + this.score_teleop_blue + this.score_endgame_blue + this.penalties_blue;
    return (int) this.score_blue + this.score_blueadj;
  }

  public override string CorrectRobotChoice(string requested_robot) => requested_robot == "Bot Royale" ? "FRC shooter" : requested_robot;

  public override void SendServerData(Dictionary<string, string> serverFlags)
  {
    base.SendServerData(serverFlags);
    serverFlags["FOUL"] = this.foul_string;
    this.foul_string = "";
    serverFlags["MARK"] = this.mark_string;
    string str1 = "";
    foreach (Scorekeeper_RapidReact.RobotStates allrobotstate in this.allrobotstates)
    {
      foreach (Scorekeeper_RapidReact.PowerUpData mydeffensivePowerup in allrobotstate.mydeffensive_powerups)
        str1 = str1 + ":" + (object) allrobotstate.robotID.id + "," + (object) (int) mydeffensivePowerup.mypowerup + "," + mydeffensivePowerup.GetRemainingTime().ToString();
      foreach (Scorekeeper_RapidReact.PowerUpData myoffensivePowerup in allrobotstate.myoffensive_powerups)
        str1 = str1 + ":" + (object) allrobotstate.robotID.id + "," + (object) (int) myoffensivePowerup.mypowerup + "," + myoffensivePowerup.GetRemainingTime().ToString();
    }
    serverFlags["PU"] = str1;
    int power;
    string str2;
    if (!this.powerup_center.IsDisabled())
    {
      power = (int) this.powerup_center.myPower;
      str2 = power.ToString();
    }
    else
      str2 = "0";
    string str3 = str2;
    for (int index = 0; index < this.powerup_home.Count; ++index)
    {
      string str4 = str3;
      string str5;
      if (!this.powerup_home[index].IsDisabled())
      {
        power = (int) this.powerup_home[index].myPower;
        str5 = power.ToString();
      }
      else
        str5 = "0";
      str3 = str4 + ":" + str5;
    }
    serverFlags["PUcore"] = str3;
  }

  private void ResetPowerUps()
  {
    for (int index = 0; index <= this.powerup_home.Count; ++index)
      (index == 0 ? this.powerup_center : this.powerup_home[index - 1]).ClearOwner();
  }

  public override void ReceiveServerData(Dictionary<string, string> serverFlags)
  {
    base.ReceiveServerData(serverFlags);
    if (serverFlags.ContainsKey("FOUL"))
    {
      string serverFlag = serverFlags["FOUL"];
      char[] chArray = new char[1]{ ':' };
      foreach (string s in serverFlag.Split(chArray))
      {
        if (s.Length > 0)
          this.CreateFoul(int.Parse(s));
      }
    }
    if (serverFlags.ContainsKey("MARK") && this.old_mark_string != serverFlags["MARK"])
    {
      this.old_mark_string = serverFlags["MARK"];
      foreach (int key in this.allrobots_byid.Keys)
        this.MarkRobotForHang(key, false);
      string serverFlag = serverFlags["MARK"];
      char[] chArray = new char[1]{ ':' };
      foreach (string s in serverFlag.Split(chArray))
      {
        if (s.Length > 0)
          this.MarkRobotForHang(int.Parse(s), true);
      }
    }
    if (serverFlags.ContainsKey("PU"))
    {
      foreach (Scorekeeper_RapidReact.RobotStates allrobotstate in this.allrobotstates)
      {
        allrobotstate.mydeffensive_powerups.Clear();
        allrobotstate.myoffensive_powerups.Clear();
      }
      string serverFlag = serverFlags["PU"];
      char[] chArray = new char[1]{ ':' };
      foreach (string str in serverFlag.Split(chArray))
      {
        if (str.Length > 0)
        {
          string[] strArray = str.Split(',');
          if (strArray.Length == 3)
          {
            int id = int.Parse(strArray[0]);
            int num1 = int.Parse(strArray[1]);
            int num2 = int.Parse(strArray[2]);
            Scorekeeper_RapidReact.PowerUpData powerUpData = new Scorekeeper_RapidReact.PowerUpData();
            powerUpData.mypowerup = (PowerUpType) num1;
            powerUpData.duration = (long) num2;
            powerUpData.start_time = MyUtils.GetTimeMillisSinceStart();
            Scorekeeper_RapidReact.RobotStates robotStateById = this.GetRobotStateById(id);
            if (robotStateById != null)
            {
              switch (powerUpData.mypowerup)
              {
                case PowerUpType.SPEED:
                case PowerUpType.TORQUE:
                case PowerUpType.INVISIBILITY:
                  robotStateById.myoffensive_powerups.Add(powerUpData);
                  continue;
                case PowerUpType.SLOW:
                case PowerUpType.WEAK:
                case PowerUpType.INVERTED:
                  robotStateById.mydeffensive_powerups.Add(powerUpData);
                  continue;
                default:
                  continue;
              }
            }
          }
        }
      }
    }
    if (!serverFlags.ContainsKey("PUcore"))
      return;
    string[] strArray1 = serverFlags["PUcore"].Split(':');
    for (int index = 0; index < strArray1.Length && index < 5; ++index)
    {
      bool flag = strArray1[index][0] == '0';
      int myNewPower = int.Parse(strArray1[index]);
      PowerUpScript powerUpScript = index == 0 ? this.powerup_center : this.powerup_home[index - 1];
      if (powerUpScript.IsDisabled() != flag)
      {
        if (!flag)
          powerUpScript.PU_Enable((PowerUpType) myNewPower);
        else
          powerUpScript.PU_DisableWithAnimation();
      }
    }
  }

  public class PowerUpData
  {
    public PowerUpType mypowerup;
    public long duration;
    public long start_time = -1;

    public long GetRemainingTime() => this.duration - (MyUtils.GetTimeMillisSinceStart() - this.start_time);

    public void SetRemainingTime(long timeleft_ms)
    {
      this.start_time = MyUtils.GetTimeMillisSinceStart();
      this.duration = timeleft_ms;
    }

    public string GetPowerChar()
    {
      switch (this.mypowerup)
      {
        case PowerUpType.SPEED:
          return "S";
        case PowerUpType.TORQUE:
          return "T";
        case PowerUpType.INVISIBILITY:
          return "I";
        case PowerUpType.SLOW:
          return "s";
        case PowerUpType.WEAK:
          return "W";
        case PowerUpType.INVERTED:
          return "i";
        default:
          return "?";
      }
    }
  }

  private class RobotStates
  {
    public bool counting_down;
    public bool counting_up;
    public float start_time = -1f;
    public bool isRed;
    public bool isblocking;
    public RobotID robotID;
    public RobotInterface3D robot;
    public long LP_StartTime = -1;
    public bool immune_to_foul;
    public List<Scorekeeper_RapidReact.PowerUpData> myoffensive_powerups = new List<Scorekeeper_RapidReact.PowerUpData>();
    public List<Scorekeeper_RapidReact.PowerUpData> mydeffensive_powerups = new List<Scorekeeper_RapidReact.PowerUpData>();

    public void Reset()
    {
      this.counting_down = false;
      this.counting_up = false;
      this.start_time = -1f;
      this.isblocking = false;
      this.LP_StartTime = -1L;
      this.immune_to_foul = false;
      this.myoffensive_powerups.Clear();
      this.mydeffensive_powerups.Clear();
      if (!(bool) (Object) this.robotID)
        return;
      this.robotID.RemoveData("Taxi");
    }
  }
}
