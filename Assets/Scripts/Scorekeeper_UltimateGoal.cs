// Decompiled with JetBrains decompiler
// Type: Scorekeeper_UltimateGoal
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class Scorekeeper_UltimateGoal : Scorekeeper
{
  private UltimateGoal_Settings ug_settings;
  public IR_scoringBox goal_high_red;
  public IR_scoringBox goal_mid_red;
  public IR_scoringBox goal_low_red;
  public IR_scoringBox goal_high_blue;
  public IR_scoringBox goal_mid_blue;
  public IR_scoringBox goal_low_blue;
  public GenericFieldTracker wg_ba;
  public GenericFieldTracker wg_bb;
  public GenericFieldTracker wg_bc;
  public GenericFieldTracker wg_ra;
  public GenericFieldTracker wg_rb;
  public GenericFieldTracker wg_rc;
  public GenericFieldTracker wg_fault_red1;
  public GenericFieldTracker wg_fault_red2;
  public GenericFieldTracker wg_fault_red3;
  public GenericFieldTracker wg_fault_red4;
  public GenericFieldTracker wg_fault_red5;
  public GenericFieldTracker wg_fault_blue1;
  public GenericFieldTracker wg_fault_blue2;
  public GenericFieldTracker wg_fault_blue3;
  public GenericFieldTracker wg_fault_blue4;
  public GenericFieldTracker wg_fault_blue5;
  public GenericFieldTracker wg_outside_box;
  public GenericFieldTracker startline_red1;
  public GenericFieldTracker startline_red2;
  public GenericFieldTracker startline_blue1;
  public GenericFieldTracker startline_blue2;
  public GenericFieldTracker launchline;
  public WobbleGoal wg_red1;
  public WobbleGoal wg_red2;
  public WobbleGoal wg_blue1;
  public WobbleGoal wg_blue2;
  public GenericFieldTracker launch_zone;
  public GenericFieldTracker no_launch_zone;
  public UG_powershots powershot_red1;
  public UG_powershots powershot_red2;
  public UG_powershots powershot_red3;
  public UG_powershots powershot_blue1;
  public UG_powershots powershot_blue2;
  public UG_powershots powershot_blue3;
  public double penalties_red;
  public int rings_red;
  public double score_auto_red;
  public double score_teleop_red;
  public double score_endgame_red;
  public double score_red;
  public double penalties_blue;
  public int rings_blue;
  public double score_auto_blue;
  public double score_teleop_blue;
  public double score_endgame_blue;
  public double score_blue;
  public Transform ringshold;
  public Transform red_return_pos;
  public Transform blue_return_pos;
  public Transform blue_starting_pos;
  public Transform red_starting_pos;
  public GenericFieldTracker red_return_box;
  public GenericFieldTracker blue_return_box;
  public List<Transform> ringsheld_red = new List<Transform>();
  public List<Transform> ringsheld_blue = new List<Transform>();
  public List<Transform> allrings = new List<Transform>();
  private int auto_roll = 1;
  public Dictionary<string, Dictionary<string, float>> player_detailed_opr = new Dictionary<string, Dictionary<string, float>>();
  public Dictionary<string, float> red_detailed_score = new Dictionary<string, float>();
  public Dictionary<string, float> blue_detailed_score = new Dictionary<string, float>();
  public IR_fieldtracker blue_safety;
  public IR_fieldtracker red_safety;
  public SelectiveWall auto_red_blocker;
  public SelectiveWall auto_blue_blocker;
  public GameObject zone_half_field_red;
  public GameObject zone_half_field_blue;
  public GameObject zone_walls_red;
  public GameObject zone_walls_blue;
  private Vector3 old_gravity;
  private bool[] play_goal_animation = new bool[6];
  private List<RobotStates> robotstates = new List<RobotStates>();
  private bool endgame_started;
  private bool wobble_goals_teleop_reset;
  private Scorekeeper.FirstGameState previous_firstgamestate = Scorekeeper.FirstGameState.FINISHED;
  private int animation_played;

  private void Awake()
  {
    GLOBALS.PlayerCount = 4;
    GLOBALS.TIMER_TOTAL = 150;
    GLOBALS.TIMER_AUTO = 30;
    GLOBALS.TIMER_ENDGAME = 30;
    this.old_gravity = Physics.gravity;
    Physics.gravity = new Vector3(0.0f, -19.62f, 0.0f);
    this.ResetDetails();
  }

  private void OnDestroy() => Physics.gravity = this.old_gravity;

  public override void ScorerInit()
  {
    this.ScorerReset();
    this.ug_settings = GameObject.Find("GameSettings").GetComponent<UltimateGoal_Settings>();
    this.allrings.Clear();
    foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("GameElement"))
    {
      gameElement component1 = gameObject.GetComponent<gameElement>();
      if (!((UnityEngine.Object) component1 == (UnityEngine.Object) null) && component1.id > 0 && component1.id <= 30)
      {
        this.allrings.Add(component1.transform);
        Rigidbody component2 = component1.GetComponent<Rigidbody>();
        if ((bool) (UnityEngine.Object) component2)
          component2.isKinematic = true;
      }
    }
  }

  public override void ScorerReset()
  {
    base.ScorerReset();
    this.penalties_red = 0.0;
    this.rings_red = 0;
    this.score_auto_red = 0.0;
    this.score_teleop_red = 0.0;
    this.score_endgame_red = 0.0;
    this.score_red = 0.0;
    this.penalties_blue = 0.0;
    this.rings_blue = 0;
    this.score_auto_blue = 0.0;
    this.score_teleop_blue = 0.0;
    this.score_endgame_blue = 0.0;
    this.score_blue = 0.0;
    this.player_detailed_opr.Clear();
    foreach (string key in this.allrobots_byname.Keys)
    {
      this.player_detailed_opr[key] = new Dictionary<string, float>();
      this.player_detailed_opr[key]["END"] = 0.0f;
      this.player_detailed_opr[key]["AUTO_N"] = 0.0f;
    }
    this.ResetRings();
    this.ResetDetails();
  }

  private void ResetDetails()
  {
    this.red_detailed_score.Clear();
    this.blue_detailed_score.Clear();
    this.red_detailed_score["AUTO"] = 0.0f;
    this.blue_detailed_score["AUTO"] = 0.0f;
    this.red_detailed_score["END"] = 0.0f;
    this.blue_detailed_score["END"] = 0.0f;
    this.red_detailed_score["AUTO_RINGS"] = 0.0f;
    this.blue_detailed_score["AUTO_RINGS"] = 0.0f;
    this.red_detailed_score["TELEOP_RINGS"] = 0.0f;
    this.blue_detailed_score["TELEOP_RINGS"] = 0.0f;
    this.red_detailed_score["AUTO_PS"] = 0.0f;
    this.blue_detailed_score["AUTO_PS"] = 0.0f;
    this.red_detailed_score["AUTO_N"] = 0.0f;
    this.blue_detailed_score["AUTO_N"] = 0.0f;
    this.red_detailed_score["END_PS"] = 0.0f;
    this.blue_detailed_score["END_PS"] = 0.0f;
    this.red_detailed_score["AUTO_WG"] = 0.0f;
    this.blue_detailed_score["AUTO_WG"] = 0.0f;
  }

  public override void GetScoreDetails(Dictionary<string, string> data)
  {
    base.GetScoreDetails(data);
    data["R_Score"] = ((int) this.score_red + this.score_redadj).ToString();
    data["B_Score"] = ((int) this.score_blue + this.score_blueadj).ToString();
    data["B_Pen"] = ((int) this.penalties_blue).ToString();
    data["R_Pen"] = ((int) this.penalties_red).ToString();
    data["R_C_RINGS"] = this.rings_red.ToString();
    data["B_C_RINGS"] = this.rings_blue.ToString();
    data["R_S_Auto"] = ((int) (this.score_auto_red + (double) this.red_detailed_score["AUTO"])).ToString();
    data["B_S_Auto"] = ((int) (this.score_auto_blue + (double) this.blue_detailed_score["AUTO"])).ToString();
    data["R_C_Auto_R"] = ((int) this.red_detailed_score["AUTO_RINGS"]).ToString();
    data["B_C_Auto_R"] = ((int) this.blue_detailed_score["AUTO_RINGS"]).ToString();
    data["R_C_Auto_PS"] = ((int) this.red_detailed_score["AUTO_PS"]).ToString();
    data["B_C_Auto_PS"] = ((int) this.blue_detailed_score["AUTO_PS"]).ToString();
    data["R_C_Auto_WG"] = ((int) this.red_detailed_score["AUTO_WG"]).ToString();
    data["B_C_Auto_WG"] = ((int) this.blue_detailed_score["AUTO_WG"]).ToString();
    data["R_C_Auto_N"] = ((int) this.red_detailed_score["AUTO_N"]).ToString();
    data["B_C_Auto_N"] = ((int) this.blue_detailed_score["AUTO_N"]).ToString();
    data["R_S_Tele"] = ((int) this.score_teleop_red).ToString();
    data["B_S_Tele"] = ((int) this.score_teleop_blue).ToString();
    data["R_C_Tele_R"] = ((int) this.red_detailed_score["TELEOP_RINGS"]).ToString();
    data["B_C_Tele_R"] = ((int) this.blue_detailed_score["TELEOP_RINGS"]).ToString();
    data["R_S_End"] = ((int) (this.score_endgame_red + (double) this.red_detailed_score["END"])).ToString();
    data["B_S_End"] = ((int) (this.score_endgame_blue + (double) this.blue_detailed_score["END"])).ToString();
    data["R_C_End_PS"] = ((int) this.red_detailed_score["END_PS"]).ToString();
    data["B_C_End_PS"] = ((int) this.blue_detailed_score["END_PS"]).ToString();
    data["R_S_End_WG"] = ((int) this.red_detailed_score["END"]).ToString();
    data["B_S_End_WG"] = ((int) this.blue_detailed_score["END"]).ToString();
  }

  private void OPR_clearitem(string item)
  {
    foreach (Dictionary<string, float> dictionary in this.player_detailed_opr.Values)
      dictionary[item] = 0.0f;
  }

  private void ResetRings()
  {
    if (GLOBALS.CLIENT_MODE)
      return;
    foreach (Transform allring in this.allrings)
    {
      Rigidbody component = allring.GetComponent<Rigidbody>();
      component.isKinematic = true;
      component.velocity = Vector3.zero;
      component.angularVelocity = Vector3.zero;
      allring.position = this.ringshold.position;
    }
  }

  public override void Restart()
  {
    base.Restart();
    if (GLOBALS.CLIENT_MODE)
      return;
    this.ResetRings();
    this.goal_high_red.Reset();
    this.goal_mid_red.Reset();
    this.goal_low_red.Reset();
    this.goal_high_blue.Reset();
    this.goal_mid_blue.Reset();
    this.goal_low_blue.Reset();
    this.red_return_box.Reset();
    this.blue_return_box.Reset();
    this.ringsheld_blue.Clear();
    this.ringsheld_red.Clear();
    int num1 = 0;
    while (num1 < this.allrings.Count - 1)
    {
      List<Transform> ringsheldBlue = this.ringsheld_blue;
      List<Transform> allrings1 = this.allrings;
      int index1 = num1;
      int num2 = index1 + 1;
      Transform transform1 = allrings1[index1];
      ringsheldBlue.Add(transform1);
      List<Transform> ringsheldRed = this.ringsheld_red;
      List<Transform> allrings2 = this.allrings;
      int index2 = num2;
      num1 = index2 + 1;
      Transform transform2 = allrings2[index2];
      ringsheldRed.Add(transform2);
    }
    this.ResetPowershots(true);
    this.ResetWobbleGoals();
    this.wg_outside_box.ClearExitedItem();
    if (this.ug_settings.ENABLE_HALFFIELD_ZONES)
    {
      this.zone_half_field_blue.SetActive(true);
      this.zone_half_field_red.SetActive(true);
    }
    else
    {
      this.zone_half_field_blue.SetActive(false);
      this.zone_half_field_red.SetActive(false);
    }
    if (this.ug_settings.ENABLE_WALL_ZONES)
    {
      this.zone_walls_blue.SetActive(true);
      this.zone_walls_red.SetActive(true);
    }
    else
    {
      this.zone_walls_blue.SetActive(false);
      this.zone_walls_red.SetActive(false);
    }
    this.ResetDetails();
  }

  private void ResetWobbleGoals()
  {
    this.wg_red1.GetComponent<Rigidbody>().isKinematic = false;
    this.wg_red2.GetComponent<Rigidbody>().isKinematic = false;
    this.wg_blue1.GetComponent<Rigidbody>().isKinematic = false;
    this.wg_blue2.GetComponent<Rigidbody>().isKinematic = false;
  }

  public override void OnTimerStart()
  {
    this.Restart();
    System.Random random = new System.Random();
    this.auto_roll = GLOBALS.game_option <= 1 ? random.Next(1, 4) : GLOBALS.game_option - 1;
    List<Transform> transformList = new List<Transform>();
    switch (this.auto_roll)
    {
      case 2:
        transformList.Add(this.PopRing(this.ringsheld_blue, this.blue_starting_pos.position, this.blue_starting_pos.rotation));
        transformList.Add(this.PopRing(this.ringsheld_red, this.red_starting_pos.position, this.red_starting_pos.rotation));
        break;
      case 3:
        transformList.Add(this.PopRing(this.ringsheld_blue, this.blue_starting_pos.position + new Vector3(0.0f, 0.12f, 0.0f), this.blue_starting_pos.rotation));
        transformList.Add(this.PopRing(this.ringsheld_red, this.red_starting_pos.position + new Vector3(0.0f, 0.12f, 0.0f), this.red_starting_pos.rotation));
        transformList.Add(this.PopRing(this.ringsheld_blue, this.blue_starting_pos.position + new Vector3(0.0f, 0.08f, 0.0f), this.blue_starting_pos.rotation));
        transformList.Add(this.PopRing(this.ringsheld_red, this.red_starting_pos.position + new Vector3(0.0f, 0.08f, 0.0f), this.red_starting_pos.rotation));
        transformList.Add(this.PopRing(this.ringsheld_blue, this.blue_starting_pos.position + new Vector3(0.0f, 0.04f, 0.0f), this.blue_starting_pos.rotation));
        transformList.Add(this.PopRing(this.ringsheld_red, this.red_starting_pos.position + new Vector3(0.0f, 0.04f, 0.0f), this.red_starting_pos.rotation));
        transformList.Add(this.PopRing(this.ringsheld_blue, this.blue_starting_pos.position, this.blue_starting_pos.rotation));
        transformList.Add(this.PopRing(this.ringsheld_red, this.red_starting_pos.position, this.red_starting_pos.rotation));
        break;
    }
    foreach (RobotInterface3D allrobot in this.allrobots)
    {
      Transform hierarchy = MyUtils.FindHierarchy(allrobot.transform, "RingLoad");
      if ((bool) (UnityEngine.Object) hierarchy)
      {
        RobotID component = allrobot.GetComponent<RobotID>();
        if ((bool) (UnityEngine.Object) component)
        {
          List<Transform> listofrings = component.is_red ? this.ringsheld_red : this.ringsheld_blue;
          this.PopRing(listofrings, hierarchy.position + hierarchy.up * 2f * 0.04f, hierarchy.rotation);
          this.PopRing(listofrings, hierarchy.position + hierarchy.up * 0.04f, hierarchy.rotation);
          this.PopRing(listofrings, hierarchy.position, hierarchy.rotation);
        }
      }
    }
    this.wg_blue1.Reset();
    this.wg_blue2.Reset();
    this.wg_red1.Reset();
    this.wg_red2.Reset();
    if (this.ug_settings.ENABLE_AUTO_WALLS)
    {
      this.auto_blue_blocker.disable = false;
      this.auto_red_blocker.disable = false;
    }
    else
    {
      this.auto_blue_blocker.disable = true;
      this.auto_red_blocker.disable = true;
    }
  }

  private Transform PopRing(
    List<Transform> listofrings,
    Vector3 tolocation,
    Quaternion torotation)
  {
    Transform listofring = listofrings[0];
    listofrings.RemoveAt(0);
    listofring.transform.position = tolocation;
    listofring.transform.rotation = torotation;
    Rigidbody component = listofring.GetComponent<Rigidbody>();
    component.velocity = Vector3.zero;
    component.angularVelocity = Vector3.zero;
    component.isKinematic = false;
    listofring.GetComponent<ball_data>().Clear();
    return listofring;
  }

  private void DeployWaitingRings()
  {
    if (this.time_total.TotalSeconds > (double) (GLOBALS.TIMER_TOTAL - GLOBALS.TIMER_AUTO))
      return;
    System.Random random = new System.Random();
    if (!this.red_return_box.IsAnyGameElementInside() && this.ringsheld_red.Count > 0)
    {
      Vector3 eulerAngles = this.red_return_pos.rotation.eulerAngles;
      eulerAngles.y += (float) (random.NextDouble() * 10.0 - 5.0);
      this.PopRing(this.ringsheld_red, this.red_return_pos.position, Quaternion.Euler(eulerAngles));
    }
    if (this.blue_return_box.IsAnyGameElementInside() || this.ringsheld_blue.Count <= 0)
      return;
    Vector3 eulerAngles1 = this.blue_return_pos.rotation.eulerAngles;
    eulerAngles1.y += (float) (random.NextDouble() * 10.0 - 5.0);
    this.PopRing(this.ringsheld_blue, this.blue_return_pos.position, Quaternion.Euler(eulerAngles1));
  }

  public void ResetPowershots(bool force_reset = false)
  {
    if (force_reset || !this.powershot_blue1.up)
      this.powershot_blue1.Reset();
    if (force_reset || !this.powershot_blue2.up)
      this.powershot_blue2.Reset();
    if (force_reset || !this.powershot_blue3.up)
      this.powershot_blue3.Reset();
    if (force_reset || !this.powershot_red1.up)
      this.powershot_red1.Reset();
    if (force_reset || !this.powershot_red2.up)
      this.powershot_red2.Reset();
    if (!force_reset && this.powershot_red3.up)
      return;
    this.powershot_red3.Reset();
  }

  public override void ScorerUpdate()
  {
    bool flag = true;
    if (this.timerstate != Scorekeeper.TimerState.PAUSED && this.timerstate != Scorekeeper.TimerState.RUNNING)
      flag = false;
    if (GLOBALS.CLIENT_MODE)
      return;
    this.CalculateScores();
    this.DeployWaitingRings();
    if (flag && this.time_total.TotalSeconds > (double) GLOBALS.TIMER_AUTO && this.time_total.TotalSeconds < (double) (GLOBALS.TIMER_AUTO + 1))
      this.ResetPowershots();
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
      if ((bool) (UnityEngine.Object) robotstate1.robot)
      {
        if (robotstate1.counting_down)
        {
          if ((double) time - (double) robotstate1.start_time > (double) this.ug_settings.BLOCKING_DURATION)
          {
            robotstate1.counting_down = false;
            robotstate1.start_time = -1f;
            robotstate1.robot.SetProgressBar(0.0f);
          }
          else
            robotstate1.robot.SetProgressBar((this.ug_settings.BLOCKING_DURATION - time + robotstate1.start_time) / this.ug_settings.BLOCKING_DURATION);
        }
        if (robotstate1.counting_up)
        {
          if (!this.ug_settings.ENABLE_BLOCKING)
          {
            robotstate1.counting_up = false;
            robotstate1.counting_down = true;
            robotstate1.start_time = time - (this.ug_settings.BLOCKING_DURATION - (time - robotstate1.start_time));
          }
          else if ((double) time - (double) robotstate1.start_time > (double) this.ug_settings.BLOCKING_DURATION)
          {
            robotstate1.counting_up = false;
            robotstate1.start_time = -1f;
            robotstate1.robot.SetProgressBar(0.0f);
            robotstate1.robot.MarkForReset(this.ug_settings.BLOCKING_RESET_HOLDING_TIME);
            if (flag)
            {
              List<RobotInterface3D> allEnemies = robotstate1.robot.GetAllEnemies();
              foreach (RobotInterface3D robotInterface3D in allEnemies)
              {
                if (this.player_opr.ContainsKey(GLOBALS.client_names[robotInterface3D.myRobotID.id]))
                  this.player_opr[GLOBALS.client_names[robotInterface3D.myRobotID.id]] += (float) (this.ug_settings.PENALTY_BLOCKING / allEnemies.Count);
                else
                  this.player_opr[GLOBALS.client_names[robotInterface3D.myRobotID.id]] = (float) (this.ug_settings.PENALTY_BLOCKING / allEnemies.Count);
              }
              if (robotstate1.isRed)
                this.penalties_red += (double) this.ug_settings.PENALTY_BLOCKING;
              else
                this.penalties_blue += (double) this.ug_settings.PENALTY_BLOCKING;
            }
          }
          else
            robotstate1.robot.SetProgressBar((time - robotstate1.start_time) / this.ug_settings.BLOCKING_DURATION);
        }
        foreach (RobotInterface3D allEnemy in robotstate1.robot.GetAllEnemies())
        {
          int robotStateIndex = this.GetRobotStateIndex(allEnemy);
          if (robotStateIndex >= 0)
          {
            RobotStates robotstate2 = this.robotstates[robotStateIndex];
            if (this.ug_settings.ENABLE_BLOCKING && robotstate1.isRed && this.red_safety.IsFriendInside(robotstate1.robot.transform) && (double) this.red_safety.GetClosestDistance(robotstate1.robot.rb_body.transform.position) + 0.00999999977648258 < (double) this.blue_safety.GetClosestDistance(allEnemy.rb_body.transform.position) || this.ug_settings.ENABLE_BLOCKING && !robotstate1.isRed && this.blue_safety.IsFriendInside(robotstate1.robot.transform) && (double) this.blue_safety.GetClosestDistance(robotstate1.robot.rb_body.transform.position) + 0.00999999977648258 < (double) this.red_safety.GetClosestDistance(allEnemy.rb_body.transform.position))
            {
              robotstate2.isblocking = true;
              if (robotstate2.counting_down)
              {
                robotstate2.counting_down = false;
                robotstate2.counting_up = true;
                robotstate2.start_time = time - (this.ug_settings.BLOCKING_DURATION - (time - robotstate2.start_time));
              }
              else if (!robotstate2.counting_up)
              {
                robotstate2.counting_up = true;
                robotstate2.start_time = time;
              }
            }
          }
        }
      }
    }
    int count4 = this.robotstates.Count;
    while (count4 >= 1 && this.ug_settings.ENABLE_BLOCKING)
    {
      --count4;
      if (!this.robotstates[count4].isblocking && this.robotstates[count4].counting_up)
      {
        this.robotstates[count4].counting_up = false;
        this.robotstates[count4].counting_down = true;
        this.robotstates[count4].start_time = time - (this.ug_settings.BLOCKING_DURATION - (time - this.robotstates[count4].start_time));
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
        if (!((UnityEngine.Object) this.allrobots[index].gameObject == (UnityEngine.Object) null) && !((UnityEngine.Object) this.allrobots[index].gameObject.GetComponent<RobotID>() == (UnityEngine.Object) null))
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

  private int GetRobotStateIndex(RobotInterface3D robot)
  {
    for (int index = 0; index < this.robotstates.Count; ++index)
    {
      if ((UnityEngine.Object) this.robotstates[index].robot == (UnityEngine.Object) robot)
        return index;
    }
    return -1;
  }

  private void AddToPlayerOPR(Dictionary<int, int> indata, float points, bool is_red)
  {
    foreach (int key in indata.Keys)
    {
      if (!this.allrobots_byid.ContainsKey(key) || (UnityEngine.Object) this.allrobots_byid[key] == (UnityEngine.Object) null || (UnityEngine.Object) this.allrobots_byid[key].myRobotID == (UnityEngine.Object) null)
        this.DoFieldChanged();
      if (this.allrobots_byid.ContainsKey(key) && !((UnityEngine.Object) this.allrobots_byid[key] == (UnityEngine.Object) null) && !((UnityEngine.Object) this.allrobots_byid[key].myRobotID == (UnityEngine.Object) null))
      {
        float num = 1f;
        if (this.allrobots_byid[key].myRobotID.is_red != is_red)
          num = -1f;
        if (this.player_opr.ContainsKey(GLOBALS.client_names[key]))
          this.player_opr[GLOBALS.client_names[key]] += num * points * (float) indata[key];
        else
          this.player_opr[GLOBALS.client_names[key]] = num * points * (float) indata[key];
      }
    }
  }

  private void AddToPlayerOPR(UG_powershots indata, float points, bool is_red)
  {
    if (!indata.scored && !indata.unscored || indata.hit_by_id < 1)
      return;
    if (!this.allrobots_byid.ContainsKey(indata.hit_by_id) || (UnityEngine.Object) this.allrobots_byid[indata.hit_by_id] == (UnityEngine.Object) null || (UnityEngine.Object) this.allrobots_byid[indata.hit_by_id].myRobotID == (UnityEngine.Object) null)
      this.DoFieldChanged();
    if (!this.allrobots_byid.ContainsKey(indata.hit_by_id) || (UnityEngine.Object) this.allrobots_byid[indata.hit_by_id] == (UnityEngine.Object) null || (UnityEngine.Object) this.allrobots_byid[indata.hit_by_id].myRobotID == (UnityEngine.Object) null)
      return;
    float num1 = 1f;
    if (this.allrobots_byid[indata.hit_by_id].myRobotID.is_red != is_red)
      num1 = -1f;
    float num2 = indata.unscored ? -1f : 1f;
    if (this.player_opr.ContainsKey(GLOBALS.client_names[indata.hit_by_id]))
      this.player_opr[GLOBALS.client_names[indata.hit_by_id]] += num1 * num2 * points;
    else
      this.player_opr[GLOBALS.client_names[indata.hit_by_id]] = num1 * num2 * points;
  }

  private void AddToPlayerOPR(RobotID robot, float points)
  {
    if (!this.allrobots_byid.ContainsKey(robot.id))
      this.DoFieldChanged();
    string clientName = GLOBALS.client_names[robot.id];
    if (this.player_opr.ContainsKey(clientName))
      this.player_opr[clientName] += points;
    else
      this.player_opr[clientName] = points;
  }

  private void SubFromPlayerOPR(Dictionary<int, int> indata, float score)
  {
    foreach (int key in indata.Keys)
    {
      if (!this.allrobots_byid.ContainsKey(key) || (UnityEngine.Object) this.allrobots_byid[key] == (UnityEngine.Object) null || (UnityEngine.Object) this.allrobots_byid[key].myRobotID == (UnityEngine.Object) null)
        this.DoFieldChanged();
      if (this.allrobots_byid.ContainsKey(key) && !((UnityEngine.Object) this.allrobots_byid[key] == (UnityEngine.Object) null) && !((UnityEngine.Object) this.allrobots_byid[key].myRobotID == (UnityEngine.Object) null))
      {
        if (this.player_opr.ContainsKey(GLOBALS.client_names[key]))
          this.player_opr[GLOBALS.client_names[key]] -= score * (float) indata[key];
        else
          this.player_opr[GLOBALS.client_names[key]] = -1f * score * (float) indata[key];
      }
    }
  }

  private void SubFromPlayerOPR(UG_powershots indata, float points)
  {
    if (!indata.scored || indata.hit_by_id < 1 || !indata.penalty)
      return;
    if (!this.allrobots_byid.ContainsKey(indata.hit_by_id) || (UnityEngine.Object) this.allrobots_byid[indata.hit_by_id] == (UnityEngine.Object) null || (UnityEngine.Object) this.allrobots_byid[indata.hit_by_id].myRobotID == (UnityEngine.Object) null)
      this.DoFieldChanged();
    if (!this.allrobots_byid.ContainsKey(indata.hit_by_id) || (UnityEngine.Object) this.allrobots_byid[indata.hit_by_id] == (UnityEngine.Object) null || (UnityEngine.Object) this.allrobots_byid[indata.hit_by_id].myRobotID == (UnityEngine.Object) null)
      return;
    if (this.player_opr.ContainsKey(GLOBALS.client_names[indata.hit_by_id]))
      this.player_opr[GLOBALS.client_names[indata.hit_by_id]] -= points;
    else
      this.player_opr[GLOBALS.client_names[indata.hit_by_id]] = -1f * points;
  }

  private void CalculateScores()
  {
    if (this.timerstate == Scorekeeper.TimerState.RUNNING)
    {
      if (this.firstgamestate == Scorekeeper.FirstGameState.AUTO)
      {
        this.red_detailed_score["AUTO"] = 0.0f;
        this.blue_detailed_score["AUTO"] = 0.0f;
        this.endgame_started = false;
        this.wobble_goals_teleop_reset = false;
      }
      if (this.previous_firstgamestate == Scorekeeper.FirstGameState.AUTO && this.firstgamestate != Scorekeeper.FirstGameState.AUTO)
        this.close_temp_opr("AUTO_N");
      if (this.firstgamestate == Scorekeeper.FirstGameState.ENDGAME)
      {
        this.red_detailed_score["END"] = 0.0f;
        this.blue_detailed_score["END"] = 0.0f;
      }
      if (this.previous_firstgamestate == Scorekeeper.FirstGameState.AUTO && this.firstgamestate != Scorekeeper.FirstGameState.AUTO)
        this.close_temp_opr("AUTO_N");
      this.previous_firstgamestate = this.firstgamestate;
      if (this.firstgamestate != Scorekeeper.FirstGameState.AUTO)
      {
        this.auto_blue_blocker.disable = true;
        this.auto_red_blocker.disable = true;
      }
      this.rings_red += this.goal_high_red.number_of_balls + this.goal_mid_red.number_of_balls + this.goal_low_red.number_of_balls;
      this.rings_blue += this.goal_high_blue.number_of_balls + this.goal_mid_blue.number_of_balls + this.goal_low_blue.number_of_balls;
      if (this.firstgamestate == Scorekeeper.FirstGameState.AUTO)
      {
        this.red_detailed_score["AUTO_RINGS"] += (float) (this.goal_high_red.number_of_balls + this.goal_mid_red.number_of_balls + this.goal_low_red.number_of_balls);
        this.blue_detailed_score["AUTO_RINGS"] += (float) (this.goal_high_blue.number_of_balls + this.goal_mid_blue.number_of_balls + this.goal_low_blue.number_of_balls);
        this.score_auto_red += (double) (12 * this.goal_high_red.number_of_balls + 6 * this.goal_mid_red.number_of_balls + 3 * this.goal_low_red.number_of_balls);
        this.score_auto_blue += (double) (12 * this.goal_high_blue.number_of_balls + 6 * this.goal_mid_blue.number_of_balls + 3 * this.goal_low_blue.number_of_balls);
        this.AddToPlayerOPR(this.goal_high_red.user_ball_count, 12f, true);
        this.AddToPlayerOPR(this.goal_mid_red.user_ball_count, 6f, true);
        this.AddToPlayerOPR(this.goal_low_red.user_ball_count, 3f, true);
        this.AddToPlayerOPR(this.goal_high_blue.user_ball_count, 12f, false);
        this.AddToPlayerOPR(this.goal_mid_blue.user_ball_count, 6f, false);
        this.AddToPlayerOPR(this.goal_low_blue.user_ball_count, 3f, false);
        this.penalties_red += (double) (this.ug_settings.PENALTY_MAJOR * (this.goal_high_red.red_bad_elements + this.goal_high_blue.red_bad_elements + this.goal_mid_red.red_bad_elements + this.goal_mid_blue.red_bad_elements));
        this.penalties_blue += (double) (this.ug_settings.PENALTY_MAJOR * (this.goal_high_red.blue_bad_elements + this.goal_high_blue.blue_bad_elements + this.goal_mid_red.blue_bad_elements + this.goal_mid_blue.blue_bad_elements));
      }
      else
      {
        this.red_detailed_score["TELEOP_RINGS"] += (float) (this.goal_high_red.number_of_balls + this.goal_mid_red.number_of_balls + this.goal_low_red.number_of_balls);
        this.blue_detailed_score["TELEOP_RINGS"] += (float) (this.goal_high_blue.number_of_balls + this.goal_mid_blue.number_of_balls + this.goal_low_blue.number_of_balls);
        this.score_teleop_red += (double) (6 * this.goal_high_red.number_of_balls + 4 * this.goal_mid_red.number_of_balls + 2 * this.goal_low_red.number_of_balls);
        this.score_teleop_blue += (double) (6 * this.goal_high_blue.number_of_balls + 4 * this.goal_mid_blue.number_of_balls + 2 * this.goal_low_blue.number_of_balls);
        this.AddToPlayerOPR(this.goal_high_red.user_ball_count, 6f, true);
        this.AddToPlayerOPR(this.goal_mid_red.user_ball_count, 4f, true);
        this.AddToPlayerOPR(this.goal_low_red.user_ball_count, 2f, true);
        this.AddToPlayerOPR(this.goal_high_blue.user_ball_count, 6f, false);
        this.AddToPlayerOPR(this.goal_mid_blue.user_ball_count, 4f, false);
        this.AddToPlayerOPR(this.goal_low_blue.user_ball_count, 2f, false);
        this.penalties_red += (double) (this.ug_settings.PENALTY_MAJOR * (this.goal_high_red.red_bad_elements + this.goal_high_blue.red_bad_elements + this.goal_mid_red.red_bad_elements + this.goal_mid_blue.red_bad_elements));
        this.penalties_blue += (double) (this.ug_settings.PENALTY_MAJOR * (this.goal_high_red.blue_bad_elements + this.goal_high_blue.blue_bad_elements + this.goal_mid_red.blue_bad_elements + this.goal_mid_blue.blue_bad_elements));
      }
      this.SubFromPlayerOPR(this.goal_high_red.bad_ball_count, (float) this.ug_settings.PENALTY_MAJOR);
      this.SubFromPlayerOPR(this.goal_mid_red.bad_ball_count, (float) this.ug_settings.PENALTY_MAJOR);
      this.SubFromPlayerOPR(this.goal_high_blue.bad_ball_count, (float) this.ug_settings.PENALTY_MAJOR);
      this.SubFromPlayerOPR(this.goal_mid_blue.bad_ball_count, (float) this.ug_settings.PENALTY_MAJOR);
      if (this.firstgamestate == Scorekeeper.FirstGameState.AUTO)
      {
        this.blue_detailed_score["AUTO_PS"] += (float) ((this.powershot_blue1.scored ? 1 : 0) + (this.powershot_blue2.scored ? 1 : 0) + (this.powershot_blue3.scored ? 1 : 0));
        this.red_detailed_score["AUTO_PS"] += (float) ((this.powershot_red1.scored ? 1 : 0) + (this.powershot_red2.scored ? 1 : 0) + (this.powershot_red3.scored ? 1 : 0));
        this.score_auto_blue += (double) ((this.powershot_blue1.scored ? 15 : 0) + (this.powershot_blue2.scored ? 15 : 0) + (this.powershot_blue3.scored ? 15 : 0));
        this.score_auto_red += (double) ((this.powershot_red1.scored ? 15 : 0) + (this.powershot_red2.scored ? 15 : 0) + (this.powershot_red3.scored ? 15 : 0));
        this.blue_detailed_score["END_PS"] -= (float) ((this.powershot_blue1.unscored ? 1 : 0) + (this.powershot_blue2.unscored ? 1 : 0) + (this.powershot_blue3.unscored ? 1 : 0));
        this.red_detailed_score["END_PS"] -= (float) ((this.powershot_red1.unscored ? 1 : 0) + (this.powershot_red2.unscored ? 1 : 0) + (this.powershot_red3.unscored ? 1 : 0));
        this.score_auto_blue -= (double) ((this.powershot_blue1.unscored ? 15 : 0) + (this.powershot_blue2.unscored ? 15 : 0) + (this.powershot_blue3.unscored ? 15 : 0));
        this.score_auto_red -= (double) ((this.powershot_red1.unscored ? 15 : 0) + (this.powershot_red2.unscored ? 15 : 0) + (this.powershot_red3.unscored ? 15 : 0));
        this.AddToPlayerOPR(this.powershot_red1, 15f, true);
        this.AddToPlayerOPR(this.powershot_red2, 15f, true);
        this.AddToPlayerOPR(this.powershot_red3, 15f, true);
        this.AddToPlayerOPR(this.powershot_blue1, 15f, false);
        this.AddToPlayerOPR(this.powershot_blue2, 15f, false);
        this.AddToPlayerOPR(this.powershot_blue3, 15f, false);
        this.penalties_blue += (double) (this.ug_settings.PENALTY_MAJOR * ((!this.powershot_blue1.scored || !this.powershot_blue1.penalty ? 0 : 1) + (!this.powershot_blue2.scored || !this.powershot_blue2.penalty ? 0 : 1) + (!this.powershot_blue3.scored || !this.powershot_blue3.penalty ? 0 : 1)));
        this.penalties_red += (double) (this.ug_settings.PENALTY_MAJOR * ((!this.powershot_red1.scored || !this.powershot_red1.penalty ? 0 : 1) + (!this.powershot_red2.scored || !this.powershot_red2.penalty ? 0 : 1) + (!this.powershot_red3.scored || !this.powershot_red3.penalty ? 0 : 1)));
        this.SubFromPlayerOPR(this.powershot_blue1, (float) this.ug_settings.PENALTY_MAJOR);
        this.SubFromPlayerOPR(this.powershot_blue2, (float) this.ug_settings.PENALTY_MAJOR);
        this.SubFromPlayerOPR(this.powershot_blue3, (float) this.ug_settings.PENALTY_MAJOR);
        this.SubFromPlayerOPR(this.powershot_red1, (float) this.ug_settings.PENALTY_MAJOR);
        this.SubFromPlayerOPR(this.powershot_red2, (float) this.ug_settings.PENALTY_MAJOR);
        this.SubFromPlayerOPR(this.powershot_red3, (float) this.ug_settings.PENALTY_MAJOR);
        this.powershot_blue1.ClearScore();
        this.powershot_blue2.ClearScore();
        this.powershot_blue3.ClearScore();
        this.powershot_red1.ClearScore();
        this.powershot_red2.ClearScore();
        this.powershot_red3.ClearScore();
      }
      if (this.firstgamestate == Scorekeeper.FirstGameState.AUTO)
      {
        this.blue_detailed_score["AUTO_N"] = 0.0f;
        this.red_detailed_score["AUTO_N"] = 0.0f;
        foreach (RobotID robot in this.allRobotID.Values)
        {
          if (!this.player_detailed_opr.ContainsKey(GLOBALS.client_names[robot.id]))
            this.player_detailed_opr[GLOBALS.client_names[robot.id]] = new Dictionary<string, float>();
          this.player_detailed_opr[GLOBALS.client_names[robot.id]]["AUTO_N"] = 0.0f;
          if (robot.is_red && this.launchline.IsRobotInside(robot))
          {
            this.red_detailed_score["AUTO"] += 5f;
            ++this.red_detailed_score["AUTO_N"];
            this.player_detailed_opr[GLOBALS.client_names[robot.id]]["AUTO_N"] = 5f;
          }
          if (!robot.is_red && this.launchline.IsRobotInside(robot))
          {
            this.blue_detailed_score["AUTO"] += 5f;
            ++this.blue_detailed_score["AUTO_N"];
            this.player_detailed_opr[GLOBALS.client_names[robot.id]]["AUTO_N"] = 5f;
          }
        }
      }
      if (this.firstgamestate == Scorekeeper.FirstGameState.ENDGAME)
      {
        this.blue_detailed_score["END_PS"] += (float) ((this.powershot_blue1.scored ? 1 : 0) + (this.powershot_blue2.scored ? 1 : 0) + (this.powershot_blue3.scored ? 1 : 0));
        this.red_detailed_score["END_PS"] += (float) ((this.powershot_red1.scored ? 1 : 0) + (this.powershot_red2.scored ? 1 : 0) + (this.powershot_red3.scored ? 1 : 0));
        this.score_endgame_blue += (double) ((this.powershot_blue1.scored ? 15 : 0) + (this.powershot_blue2.scored ? 15 : 0) + (this.powershot_blue3.scored ? 15 : 0));
        this.score_endgame_red += (double) ((this.powershot_red1.scored ? 15 : 0) + (this.powershot_red2.scored ? 15 : 0) + (this.powershot_red3.scored ? 15 : 0));
        this.blue_detailed_score["END_PS"] -= (float) ((this.powershot_blue1.unscored ? 1 : 0) + (this.powershot_blue2.unscored ? 1 : 0) + (this.powershot_blue3.unscored ? 1 : 0));
        this.red_detailed_score["END_PS"] -= (float) ((this.powershot_red1.unscored ? 1 : 0) + (this.powershot_red2.unscored ? 1 : 0) + (this.powershot_red3.unscored ? 1 : 0));
        this.score_endgame_blue -= (double) ((this.powershot_blue1.unscored ? 15 : 0) + (this.powershot_blue2.unscored ? 15 : 0) + (this.powershot_blue3.unscored ? 15 : 0));
        this.score_endgame_red -= (double) ((this.powershot_red1.unscored ? 15 : 0) + (this.powershot_red2.unscored ? 15 : 0) + (this.powershot_red3.unscored ? 15 : 0));
        this.AddToPlayerOPR(this.powershot_red1, 15f, true);
        this.AddToPlayerOPR(this.powershot_red2, 15f, true);
        this.AddToPlayerOPR(this.powershot_red3, 15f, true);
        this.AddToPlayerOPR(this.powershot_blue1, 15f, false);
        this.AddToPlayerOPR(this.powershot_blue2, 15f, false);
        this.AddToPlayerOPR(this.powershot_blue3, 15f, false);
        this.penalties_blue += (double) (this.ug_settings.PENALTY_MAJOR * ((!this.powershot_blue1.scored || !this.powershot_blue1.penalty ? 0 : 1) + (!this.powershot_blue2.scored || !this.powershot_blue2.penalty ? 0 : 1) + (!this.powershot_blue3.scored || !this.powershot_blue3.penalty ? 0 : 1)));
        this.penalties_red += (double) (this.ug_settings.PENALTY_MAJOR * ((!this.powershot_red1.scored || !this.powershot_red1.penalty ? 0 : 1) + (!this.powershot_red2.scored || !this.powershot_red2.penalty ? 0 : 1) + (!this.powershot_red3.scored || !this.powershot_red3.penalty ? 0 : 1)));
        this.SubFromPlayerOPR(this.powershot_blue1, (float) this.ug_settings.PENALTY_MAJOR);
        this.SubFromPlayerOPR(this.powershot_blue2, (float) this.ug_settings.PENALTY_MAJOR);
        this.SubFromPlayerOPR(this.powershot_blue3, (float) this.ug_settings.PENALTY_MAJOR);
        this.SubFromPlayerOPR(this.powershot_red1, (float) this.ug_settings.PENALTY_MAJOR);
        this.SubFromPlayerOPR(this.powershot_red2, (float) this.ug_settings.PENALTY_MAJOR);
        this.SubFromPlayerOPR(this.powershot_red3, (float) this.ug_settings.PENALTY_MAJOR);
        this.powershot_blue1.ClearScore();
        this.powershot_blue2.ClearScore();
        this.powershot_blue3.ClearScore();
        this.powershot_red1.ClearScore();
        this.powershot_red2.ClearScore();
        this.powershot_red3.ClearScore();
      }
      if (!this.endgame_started && this.firstgamestate == Scorekeeper.FirstGameState.ENDGAME)
      {
        this.endgame_started = true;
        this.InitWobbleGoalForEndGame(this.wg_blue1);
        this.InitWobbleGoalForEndGame(this.wg_blue2);
        this.InitWobbleGoalForEndGame(this.wg_red1);
        this.InitWobbleGoalForEndGame(this.wg_red2);
        this.wg_outside_box.ClearExitedItem();
        this.wg_outside_box.Reset();
      }
      if (this.firstgamestate == Scorekeeper.FirstGameState.AUTO)
      {
        GenericFieldTracker genericFieldTracker1;
        GenericFieldTracker genericFieldTracker2;
        if (this.auto_roll == 1)
        {
          genericFieldTracker1 = this.wg_ba;
          genericFieldTracker2 = this.wg_ra;
        }
        else if (this.auto_roll == 2)
        {
          genericFieldTracker1 = this.wg_bb;
          genericFieldTracker2 = this.wg_rb;
        }
        else
        {
          genericFieldTracker1 = this.wg_bc;
          genericFieldTracker2 = this.wg_rc;
        }
        bool scored1 = false;
        bool scored2 = false;
        bool scored3 = false;
        bool scored4 = false;
        if (genericFieldTracker1.IsGameElementInside(this.wg_blue1.transform) && !this.wg_fault_blue1.IsGameElementInside(this.wg_blue1.transform) && !this.wg_fault_blue2.IsGameElementInside(this.wg_blue1.transform) && !this.wg_fault_blue3.IsGameElementInside(this.wg_blue1.transform) && !this.wg_fault_blue4.IsGameElementInside(this.wg_blue1.transform) && !this.wg_fault_blue5.IsGameElementInside(this.wg_blue1.transform))
          scored3 = true;
        if (genericFieldTracker1.IsGameElementInside(this.wg_blue2.transform) && !this.wg_fault_blue1.IsGameElementInside(this.wg_blue2.transform) && !this.wg_fault_blue2.IsGameElementInside(this.wg_blue2.transform) && !this.wg_fault_blue3.IsGameElementInside(this.wg_blue2.transform) && !this.wg_fault_blue4.IsGameElementInside(this.wg_blue2.transform) && !this.wg_fault_blue5.IsGameElementInside(this.wg_blue2.transform))
          scored4 = true;
        if (genericFieldTracker2.IsGameElementInside(this.wg_red1.transform) && !this.wg_fault_red1.IsGameElementInside(this.wg_red1.transform) && !this.wg_fault_red2.IsGameElementInside(this.wg_red1.transform) && !this.wg_fault_red3.IsGameElementInside(this.wg_red1.transform) && !this.wg_fault_red4.IsGameElementInside(this.wg_red1.transform) && !this.wg_fault_red5.IsGameElementInside(this.wg_red1.transform))
          scored1 = true;
        if (genericFieldTracker2.IsGameElementInside(this.wg_red2.transform) && !this.wg_fault_red1.IsGameElementInside(this.wg_red2.transform) && !this.wg_fault_red2.IsGameElementInside(this.wg_red2.transform) && !this.wg_fault_red3.IsGameElementInside(this.wg_red2.transform) && !this.wg_fault_red4.IsGameElementInside(this.wg_red2.transform) && !this.wg_fault_red5.IsGameElementInside(this.wg_red2.transform))
          scored2 = true;
        this.red_detailed_score["AUTO"] += (float) ((scored1 ? 15 : 0) + (scored2 ? 15 : 0));
        this.blue_detailed_score["AUTO"] += (float) ((scored3 ? 15 : 0) + (scored4 ? 15 : 0));
        this.red_detailed_score["AUTO_WG"] = (float) ((scored1 ? 1 : 0) + (scored2 ? 1 : 0));
        this.blue_detailed_score["AUTO_WG"] = (float) ((scored3 ? 1 : 0) + (scored4 ? 1 : 0));
        this.wg_blue1.valid = scored3;
        this.wg_blue1.auto_inplay = true;
        this.wg_blue2.valid = scored4;
        this.wg_blue2.auto_inplay = true;
        this.wg_red1.valid = scored1;
        this.wg_red1.auto_inplay = true;
        this.wg_red2.valid = scored2;
        this.wg_red2.auto_inplay = true;
        this.Score_WG_OPR(scored1, this.wg_red1, 15f);
        this.Score_WG_OPR(scored2, this.wg_red2, 15f);
        this.Score_WG_OPR(scored3, this.wg_blue1, 15f);
        this.Score_WG_OPR(scored4, this.wg_blue2, 15f);
      }
      if (this.firstgamestate == Scorekeeper.FirstGameState.TELEOP && !this.wobble_goals_teleop_reset)
      {
        this.wg_blue1.Reset();
        this.wg_blue2.Reset();
        this.wg_red1.Reset();
        this.wg_red2.Reset();
        this.wobble_goals_teleop_reset = true;
      }
      if (this.endgame_started)
      {
        this.red_detailed_score["END"] += this.Calculate_WG_Endgame(this.wg_red1, this.startline_red1, this.startline_red2);
        this.red_detailed_score["END"] += this.Calculate_WG_Endgame(this.wg_red2, this.startline_red1, this.startline_red2);
        this.blue_detailed_score["END"] += this.Calculate_WG_Endgame(this.wg_blue1, this.startline_blue1, this.startline_blue2);
        this.blue_detailed_score["END"] += this.Calculate_WG_Endgame(this.wg_blue2, this.startline_blue1, this.startline_blue2);
      }
    }
    this.play_goal_animation[0] |= this.ReturnRings(this.goal_high_blue, this.ringsheld_blue);
    this.play_goal_animation[1] |= this.ReturnRings(this.goal_mid_blue, this.ringsheld_red);
    this.play_goal_animation[2] |= this.ReturnRings(this.goal_low_blue, this.ringsheld_blue);
    this.play_goal_animation[3] |= this.ReturnRings(this.goal_high_red, this.ringsheld_red);
    this.play_goal_animation[4] |= this.ReturnRings(this.goal_mid_red, this.ringsheld_blue);
    this.play_goal_animation[5] |= this.ReturnRings(this.goal_low_red, this.ringsheld_red);
  }

  private float Calculate_WG_Endgame(
    WobbleGoal wg,
    GenericFieldTracker startline1,
    GenericFieldTracker startline2)
  {
    if (!wg.valid)
      return 0.0f;
    if (startline1.IsGameElementInside(wg.transform) || startline2.IsGameElementInside(wg.transform))
    {
      this.Score_WG_OPR(true, wg, 5f);
      return 5f;
    }
    this.Score_WG_OPR(false, wg, 5f);
    if (!this.wg_outside_box.IsGameElementInside(wg.transform))
      return 0.0f;
    this.Score_WG_OPR(true, wg, 20f);
    wg.robot_scored = -1;
    wg.last_contact_robot = (RobotID) null;
    return 20f;
  }

  private void Score_WG_OPR(bool scored, WobbleGoal wg, float points)
  {
    if (scored && !wg.valid_old && (bool) (UnityEngine.Object) wg.last_contact_robot)
      this.AddToPlayerOPR(wg.last_contact_robot, points);
    if (!scored && wg.valid_old && (bool) (UnityEngine.Object) wg.last_contact_robot)
      this.AddToPlayerOPR(wg.last_contact_robot, -1f * points);
    wg.valid_old = scored;
  }

  private void close_temp_opr(string key_to_close)
  {
    foreach (string key in this.player_detailed_opr.Keys)
    {
      if (this.player_detailed_opr[key].ContainsKey(key_to_close))
      {
        if (!this.player_opr.ContainsKey(key))
          this.player_opr[key] = this.player_detailed_opr[key][key_to_close];
        else
          this.player_opr[key] += this.player_detailed_opr[key][key_to_close];
        this.player_detailed_opr[key][key_to_close] = 0.0f;
      }
    }
  }

  private void InitWobbleGoalForEndGame(WobbleGoal goal)
  {
    goal.endgame_started = true;
    goal.valid = false;
    goal.valid_old = false;
    if (!this.launch_zone.IsGameElementInside(goal.transform))
      goal.valid = true;
    else if (goal.is_red && (this.wg_ra.IsGameElementInside(goal.transform) || this.wg_rb.IsGameElementInside(goal.transform) || this.wg_rc.IsGameElementInside(goal.transform)))
    {
      goal.valid = true;
    }
    else
    {
      if (!this.wg_ba.IsGameElementInside(goal.transform) && !this.wg_bb.IsGameElementInside(goal.transform) && !this.wg_bc.IsGameElementInside(goal.transform))
        return;
      goal.valid = true;
    }
  }

  public override void DoTimerFinished()
  {
    base.DoTimerFinished();
    foreach (string key in this.player_detailed_opr.Keys)
    {
      if (this.player_opr.ContainsKey(key))
        this.player_opr[key] += this.player_detailed_opr[key]["END"];
      else
        this.player_opr[key] = this.player_detailed_opr[key]["END"];
    }
  }

  public override int GetRedScore()
  {
    if (this.timerstate == Scorekeeper.TimerState.FINISHED)
      return this.score_redfinal;
    this.score_red = this.score_auto_red + (double) this.red_detailed_score["AUTO"] + this.score_teleop_red + this.score_endgame_red + (double) this.red_detailed_score["END"] - this.penalties_red;
    return (int) this.score_red + this.score_redadj;
  }

  public override int GetBlueScore()
  {
    if (this.timerstate == Scorekeeper.TimerState.FINISHED)
      return this.score_bluefinal;
    this.score_blue = this.score_auto_blue + (double) this.blue_detailed_score["AUTO"] + this.score_teleop_blue + this.score_endgame_blue + (double) this.blue_detailed_score["END"] - this.penalties_blue;
    return (int) this.score_blue + this.score_blueadj;
  }

  private bool ReturnRings(IR_scoringBox goal, List<Transform> ringsheld)
  {
    bool flag = false;
    for (Transform nextBall = goal.GetNextBall(); (bool) (UnityEngine.Object) nextBall; nextBall = goal.GetNextBall())
    {
      this.ReturnRing(nextBall, ringsheld);
      flag = true;
    }
    goal.Reset();
    return flag;
  }

  private void ReturnRing(Transform currring, List<Transform> ringsheld)
  {
    Rigidbody component = currring.GetComponent<Rigidbody>();
    component.isKinematic = true;
    component.velocity = Vector3.zero;
    component.angularVelocity = Vector3.zero;
    currring.position = this.ringshold.position;
    ringsheld.Add(currring);
  }

  public override string CorrectRobotChoice(string requested_robot) => requested_robot;

  public override bool CorrectFieldElement(GameObject currobj)
  {
    if (!this.floor_was_found)
    {
      GameObject gameObject = GameObject.Find("3d field/matts");
      if ((UnityEngine.Object) gameObject == (UnityEngine.Object) null)
        gameObject = GameObject.Find("3d field/floor");
      if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
      {
        Bounds bounds = gameObject.GetComponent<Renderer>().bounds;
        this.floor_max = bounds.max;
        bounds = gameObject.GetComponent<Renderer>().bounds;
        this.floor_min = bounds.min;
        this.floor_was_found = true;
      }
    }
    if ((double) currobj.transform.position.y >= (double) this.floor_min.y - 5.0)
      return false;
    Rigidbody component1 = currobj.GetComponent<Rigidbody>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
    {
      component1.velocity = new Vector3(0.0f, 0.0f, 0.0f);
      component1.angularVelocity = new Vector3(0.0f, 0.0f, 0.0f);
    }
    gameElement component2 = currobj.GetComponent<gameElement>();
    if ((bool) (UnityEngine.Object) component2 && component2.type == ElementType.Jewel)
    {
      if (UnityEngine.Random.Range(0, 2) == 0)
        this.ReturnRing(currobj.transform, this.ringsheld_red);
      else
        this.ReturnRing(currobj.transform, this.ringsheld_blue);
    }
    else if ((bool) (UnityEngine.Object) component2 && (component2.type == ElementType.Cube || component2.type == ElementType.CubeDark))
    {
      currobj.transform.position = new Vector3((float) (((double) this.floor_min.x + (double) this.floor_max.x) / 2.0), this.floor_max.y - 20f, (float) (((double) this.floor_min.z + (double) this.floor_max.z) / 2.0));
      currobj.GetComponent<Rigidbody>().isKinematic = true;
    }
    else
      currobj.transform.position = new Vector3(UnityEngine.Random.Range(this.floor_min.x, this.floor_max.x), this.floor_max.y + 20f, UnityEngine.Random.Range(this.floor_min.z, this.floor_max.z));
    return true;
  }

  public override void SendServerData(Dictionary<string, string> serverFlags)
  {
    base.SendServerData(serverFlags);
    serverFlags["ANIM"] = (this.play_goal_animation[0] ? "1" : "0") + (this.play_goal_animation[1] ? "1" : "0") + (this.play_goal_animation[2] ? "1" : "0") + (this.play_goal_animation[3] ? "1" : "0") + (this.play_goal_animation[4] ? "1" : "0") + (this.play_goal_animation[5] ? "1" : "0");
    for (int index = 0; index < 6; ++index)
      this.play_goal_animation[index] = false;
  }

  public override void ReceiveServerData(Dictionary<string, string> serverFlags)
  {
    base.ReceiveServerData(serverFlags);
    if (!serverFlags.ContainsKey("ANIM"))
      return;
    string serverFlag = serverFlags["ANIM"];
    if (serverFlag.Length < 6)
      return;
    if (serverFlag[0] == '1')
      this.goal_high_blue.PlayAnimation();
    if (serverFlag[1] == '1')
      this.goal_mid_blue.PlayAnimation();
    if (serverFlag[2] == '1')
      this.goal_low_blue.PlayAnimation();
    if (serverFlag[3] == '1')
      this.goal_high_red.PlayAnimation();
    if (serverFlag[4] == '1')
      this.goal_mid_red.PlayAnimation();
    if (serverFlag[5] == '1')
      this.goal_low_red.PlayAnimation();
    serverFlags["ANIM"] = "0";
  }
}
