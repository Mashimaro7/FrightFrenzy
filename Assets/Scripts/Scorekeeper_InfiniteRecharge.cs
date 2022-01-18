// Decompiled with JetBrains decompiler
// Type: Scorekeeper_InfiniteRecharge
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scorekeeper_InfiniteRecharge : Scorekeeper
{
  private InfiniteRecharge_Settings ir_settings;
  public IR_scoringBox goal_high_red;
  public IR_scoringBox goal_mid_red;
  public IR_scoringBox goal_low_red;
  public IR_scoringBox goal_high_blue;
  public IR_scoringBox goal_mid_blue;
  public IR_scoringBox goal_low_blue;
  public double penalties_red;
  public int powercells_red;
  public double score_auto_red;
  public double score_teleop_red;
  public double score_endgame_red;
  public double score_red;
  public double penalties_blue;
  public int powercells_blue;
  private int auto_pc_low_r;
  private int auto_pc_mid_r;
  private int auto_pc_high_r;
  private int auto_pc_low_b;
  private int auto_pc_mid_b;
  private int auto_pc_high_b;
  private int tele_pc_low_r;
  private int tele_pc_mid_r;
  private int tele_pc_high_r;
  private int tele_pc_low_b;
  private int tele_pc_mid_b;
  private int tele_pc_high_b;
  private bool cw_rotation_r;
  private bool cw_position_r;
  private bool cw_rotation_b;
  private bool cw_position_b;
  private int bots_parked_r;
  private int bots_parked_b;
  private int bots_hanging_r;
  private int bots_hanging_b;
  private bool switch_level_r;
  private bool switch_level_b;
  public double score_auto_blue;
  public double score_teleop_blue;
  public double score_endgame_blue;
  public double score_cpanel_blue;
  public double score_cpanel_red;
  public double score_blue;
  public Transform ballholdingred;
  public Transform ballholdingblue;
  public List<Transform> ballsheld_red = new List<Transform>();
  public List<Transform> ballsheld_blue = new List<Transform>();
  public FRC_DepotHandler loading_blue;
  public FRC_DepotHandler loading_red;
  public IR_fieldtracker blue_safety;
  public IR_fieldtracker red_safety;
  public IR_fieldtracker blue_wallsafety;
  public IR_fieldtracker red_wallsafety;
  public FRC_DepotHandler floor;
  public FRC_DepotHandler bluepark;
  public FRC_DepotHandler redpark;
  public FRC_DepotHandler bluebalance;
  public FRC_DepotHandler redbalance;
  [Header("IRL FOULS")]
  public GenericFieldTracker BlueTrenchRun;
  public GenericFieldTracker BlueTrenchRun_clear;
  public GenericFieldTracker BlueTargetZone;
  public GenericFieldTracker BlueTargetZone_clear;
  public GenericFieldTracker BlueLoadingZone;
  public GenericFieldTracker BlueLoadingZone_clear;
  public GenericFieldTracker RedTrenchRun;
  public GenericFieldTracker RedTrenchRun_clear;
  public GenericFieldTracker RedTargetZone;
  public GenericFieldTracker RedTargetZone_clear;
  public GenericFieldTracker RedLoadingZone;
  public GenericFieldTracker RedLoadingZone_clear;
  [Header("Control Panel")]
  public HingeJoint controlwheel_red;
  public HingeJoint controlwheel_blue;
  public MeshRenderer wheellight_red;
  public MeshRenderer wheellight_blue;
  public Transform pp_red_lights;
  public Transform pp_blue_lights;
  public List<MeshRenderer> pp_red_array = new List<MeshRenderer>();
  public List<MeshRenderer> pp_blue_array = new List<MeshRenderer>();
  public Material pp_material_off;
  public Material pp_material_yellow;
  public Material pp_material_red;
  public Material pp_material_blue;
  public MeshRenderer[] red_stage;
  public MeshRenderer[] blue_stage;
  public Transform marker_red;
  public Transform marker_blue;
  public GameObject powerup_overlay;
  [Header("PowerUps")]
  public PowerUpScript powerup_center;
  public List<PowerUpScript> powerup_home = new List<PowerUpScript>();
  public Dictionary<string, float> player_opr_endgame = new Dictionary<string, float>();
  private Vector3 old_gravity;
  private GameObject fault_prefab;
  public Scorekeeper_InfiniteRecharge.WheelStruct cw_red_wheel = new Scorekeeper_InfiniteRecharge.WheelStruct();
  public Scorekeeper_InfiniteRecharge.WheelStruct cw_blue_wheel = new Scorekeeper_InfiniteRecharge.WheelStruct();
  private string foul_string = "";
  private List<Scorekeeper_InfiniteRecharge.RobotStates> robotstates = new List<Scorekeeper_InfiniteRecharge.RobotStates>();
  private int animation_played;

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
    this.ir_settings = GameObject.Find("GameSettings").GetComponent<InfiniteRecharge_Settings>();
    this.fault_prefab = UnityEngine.Resources.Load("Prefabs/FaultAnimation") as GameObject;
    Transform transform1 = this.pp_red_lights.Find("a");
    Transform transform2 = this.pp_red_lights.Find("b");
    Transform transform3 = this.pp_blue_lights.Find("a");
    Transform transform4 = this.pp_blue_lights.Find("b");
    this.pp_red_array.Clear();
    this.pp_blue_array.Clear();
    for (int index = 15; index >= 1; --index)
    {
      this.pp_red_array.Add(transform1.Find(index.ToString()).GetComponent<MeshRenderer>());
      this.pp_red_array.Add(transform2.Find(index.ToString()).GetComponent<MeshRenderer>());
      this.pp_blue_array.Add(transform3.Find(index.ToString()).GetComponent<MeshRenderer>());
      this.pp_blue_array.Add(transform4.Find(index.ToString()).GetComponent<MeshRenderer>());
    }
    this.powerup_center.myscorekeeper = (Scorekeeper) this;
    foreach (PowerUpScript powerUpScript in this.powerup_home)
      powerUpScript.myscorekeeper = (Scorekeeper) this;
  }

  public override void ScorerReset()
  {
    base.ScorerReset();
    this.penalties_red = 0.0;
    this.powercells_red = 0;
    this.score_auto_red = 0.0;
    this.score_teleop_red = 0.0;
    this.score_endgame_red = 0.0;
    this.score_cpanel_red = 0.0;
    this.score_red = 0.0;
    this.penalties_blue = 0.0;
    this.powercells_blue = 0;
    this.auto_pc_low_r = 0;
    this.auto_pc_mid_r = 0;
    this.auto_pc_high_r = 0;
    this.auto_pc_low_b = 0;
    this.auto_pc_mid_b = 0;
    this.auto_pc_high_b = 0;
    this.tele_pc_low_r = 0;
    this.tele_pc_mid_r = 0;
    this.tele_pc_high_r = 0;
    this.tele_pc_low_b = 0;
    this.tele_pc_mid_b = 0;
    this.tele_pc_high_b = 0;
    this.score_auto_blue = 0.0;
    this.score_teleop_blue = 0.0;
    this.score_endgame_blue = 0.0;
    this.score_cpanel_blue = 0.0;
    this.score_blue = 0.0;
    this.cw_rotation_r = false;
    this.cw_position_r = false;
    this.cw_rotation_b = false;
    this.cw_position_b = false;
    this.bots_parked_r = 0;
    this.bots_parked_b = 0;
    this.bots_hanging_r = 0;
    this.bots_hanging_b = 0;
    this.switch_level_r = false;
    this.switch_level_b = false;
    this.player_opr_endgame.Clear();
    foreach (string key in this.allrobots_byname.Keys)
      this.player_opr_endgame[key] = 0.0f;
    this.ResetControlPanel();
    this.ResetRobotStates();
    this.ResetPowerUps();
  }

  public override void GetScoreDetails(Dictionary<string, string> data)
  {
    base.GetScoreDetails(data);
    data["PC_R"] = this.powercells_red.ToString();
    data["PC_B"] = this.powercells_blue.ToString();
    data["AutoR"] = ((int) this.score_auto_red).ToString();
    data["AutoB"] = ((int) this.score_auto_blue).ToString();
    data["TeleR"] = ((int) this.score_teleop_red).ToString();
    data["TeleB"] = ((int) this.score_teleop_blue).ToString();
    data["EndR"] = ((int) this.score_endgame_red).ToString();
    data["EndB"] = ((int) this.score_endgame_blue).ToString();
    data["CP_R"] = ((int) this.score_cpanel_red).ToString();
    data["CP_B"] = ((int) this.score_cpanel_blue).ToString();
    data["ScoreR"] = ((int) this.score_red + this.score_redadj).ToString();
    data["ScoreB"] = ((int) this.score_blue + this.score_blueadj).ToString();
    data["PenB"] = ((int) this.penalties_blue).ToString();
    data["PenR"] = ((int) this.penalties_red).ToString();
    data["AutoPC1_R"] = this.auto_pc_low_r.ToString();
    data["AutoPC2_R"] = this.auto_pc_mid_r.ToString();
    data["AutoPC3_R"] = this.auto_pc_high_r.ToString();
    data["AutoPC1_B"] = this.auto_pc_low_b.ToString();
    data["AutoPC2_B"] = this.auto_pc_mid_b.ToString();
    data["AutoPC3_B"] = this.auto_pc_high_b.ToString();
    data["TelePC1_R"] = this.tele_pc_low_r.ToString();
    data["TelePC2_R"] = this.tele_pc_mid_r.ToString();
    data["TelePC3_R"] = this.tele_pc_high_r.ToString();
    data["TelePC1_B"] = this.tele_pc_low_b.ToString();
    data["TelePC2_B"] = this.tele_pc_mid_b.ToString();
    data["TelePC3_B"] = this.tele_pc_high_b.ToString();
    data["CW_rot_R"] = this.cw_rotation_r ? "Y" : "N";
    data["CW_pos_R"] = this.cw_position_r ? "Y" : "N";
    data["CW_rot_B"] = this.cw_rotation_b ? "Y" : "N";
    data["CW_pos_B"] = this.cw_position_b ? "Y" : "N";
    data["End_park_R"] = this.bots_parked_r.ToString();
    Dictionary<string, string> dictionary1 = data;
    int num = this.bots_parked_b;
    string str1 = num.ToString();
    dictionary1["End_park_B"] = str1;
    Dictionary<string, string> dictionary2 = data;
    num = this.bots_hanging_r;
    string str2 = num.ToString();
    dictionary2["End_hang_R"] = str2;
    Dictionary<string, string> dictionary3 = data;
    num = this.bots_hanging_b;
    string str3 = num.ToString();
    dictionary3["End_hang_B"] = str3;
    data["End_level_R"] = this.switch_level_r ? "Y" : "N";
    data["End_level_B"] = this.switch_level_b ? "Y" : "N";
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
    return "<B>AUTO Score   = </B> " + data["Auto" + str] + "\n    # Power-Cells in Low: " + data["AutoPC1_" + str] + "\n    # Power-Cells in Mid: " + data["AutoPC2_" + str] + "\n    # Power-Cells in High: " + data["AutoPC3_" + str] + "\n\n<B>TELEOP Score =</B> " + data["Tele" + str] + "\n    # Power-Cells in Low: " + data["TelePC1_" + str] + "\n    # Power-Cells in Mid: " + data["TelePC2_" + str] + "\n    # Power-Cells in High: " + data["TelePC3_" + str] + "\n    Control Panel Rotation: " + data["CW_rot_" + str] + "\n    Control Panel Position: " + data["CW_pos_" + str] + "\n\n<B>ENDGAME Score=</B> " + data["End" + str] + "\n    # Parked: " + data["End_park_" + str] + "\n    # Hanging: " + data["End_hang_" + str] + "\n    Switch Level: " + data["End_level_" + str];
  }

  public override void Restart()
  {
    base.Restart();
    this.goal_high_red.Reset();
    this.goal_mid_red.Reset();
    this.goal_low_red.Reset();
    this.goal_high_blue.Reset();
    this.goal_mid_blue.Reset();
    this.goal_low_blue.Reset();
    foreach (Component component1 in this.ballsheld_blue)
    {
      Rigidbody component2 = component1.GetComponent<Rigidbody>();
      component2.isKinematic = false;
      component2.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }
    this.ballsheld_blue.Clear();
    foreach (Component component3 in this.ballsheld_red)
    {
      Rigidbody component4 = component3.GetComponent<Rigidbody>();
      component4.isKinematic = false;
      component4.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }
    this.ballsheld_red.Clear();
    this.ResetControlPanel();
  }

  private void ResetControlPanel()
  {
    this.cw_red_wheel.cw_stateMachine = Scorekeeper_InfiniteRecharge.WheelStates.RESET;
    this.cw_red_wheel.cw_ballcount = 0;
    this.cw_red_wheel.cw_lastcolor = 0;
    this.cw_red_wheel.cw_currcolor = 0;
    this.cw_red_wheel.cw_rotation = 0;
    this.cw_red_wheel.cw_waitstart = -1L;
    this.cw_red_wheel.cw_flash_lights = false;
    this.cw_red_wheel.cw_stage3_color = UnityEngine.Random.Range(0, 4);
    this.cw_red_wheel.controlwheel = this.controlwheel_red;
    this.cw_red_wheel.cw_trench_light = false;
    this.cw_blue_wheel.cw_stateMachine = Scorekeeper_InfiniteRecharge.WheelStates.RESET;
    this.cw_blue_wheel.cw_ballcount = 0;
    this.cw_blue_wheel.cw_lastcolor = 0;
    this.cw_blue_wheel.cw_currcolor = 0;
    this.cw_blue_wheel.cw_rotation = 0;
    this.cw_blue_wheel.cw_waitstart = -1L;
    this.cw_blue_wheel.cw_flash_lights = false;
    this.cw_blue_wheel.cw_stage3_color = UnityEngine.Random.Range(0, 4);
    this.cw_blue_wheel.controlwheel = this.controlwheel_blue;
    this.cw_blue_wheel.cw_trench_light = false;
    this.PP_SetLights(this.pp_red_array, this.cw_red_wheel, this.pp_material_red, this.wheellight_red, this.red_stage, this.marker_red);
    this.PP_SetLights(this.pp_blue_array, this.cw_blue_wheel, this.pp_material_blue, this.wheellight_blue, this.blue_stage, this.marker_blue);
  }

  private void UpdateControlPanel()
  {
    if (this.timerstate != Scorekeeper.TimerState.RUNNING)
      return;
    this.PP_SetLights(this.pp_red_array, this.cw_red_wheel, this.pp_material_red, this.wheellight_red, this.red_stage, this.marker_red);
    this.PP_SetLights(this.pp_blue_array, this.cw_blue_wheel, this.pp_material_blue, this.wheellight_blue, this.blue_stage, this.marker_blue);
    if (GLOBALS.CLIENT_MODE)
      return;
    this.ProcessWheelStateMachine(this.cw_red_wheel);
    this.ProcessWheelStateMachine(this.cw_blue_wheel);
  }

  private void ProcessWheelStateMachine(Scorekeeper_InfiniteRecharge.WheelStruct wheel)
  {
    switch (wheel.cw_stateMachine)
    {
      case Scorekeeper_InfiniteRecharge.WheelStates.RESET:
        wheel.cw_flash_lights = false;
        wheel.cw_trench_light = false;
        wheel.cw_stateMachine = Scorekeeper_InfiniteRecharge.WheelStates.READY;
        wheel.cw_ballcount = 0;
        break;
      case Scorekeeper_InfiniteRecharge.WheelStates.READY:
        if (wheel.cw_ballcount < 9 + this.ir_settings.SHIELD_OFFSET)
          break;
        wheel.cw_stateMachine = Scorekeeper_InfiniteRecharge.WheelStates.STAGE1_ARMED;
        wheel.cw_ballcount = 0;
        break;
      case Scorekeeper_InfiniteRecharge.WheelStates.STAGE1_ARMED:
        if (this.firstgamestate != Scorekeeper.FirstGameState.TELEOP && this.firstgamestate != Scorekeeper.FirstGameState.ENDGAME)
          break;
        wheel.cw_stateMachine = Scorekeeper_InfiniteRecharge.WheelStates.STAGE1_COMPLETE;
        wheel.cw_ballcount = 0;
        break;
      case Scorekeeper_InfiniteRecharge.WheelStates.STAGE1_COMPLETE:
        if ((!(this.ir_settings.GAMEVERSION == "2020") || wheel.cw_ballcount < 20 + this.ir_settings.SHIELD_OFFSET) && (!(this.ir_settings.GAMEVERSION == "2021") || wheel.cw_ballcount < 15 + this.ir_settings.SHIELD_OFFSET))
          break;
        wheel.cw_stateMachine = Scorekeeper_InfiniteRecharge.WheelStates.STAGE2_ARMED;
        wheel.cw_waitstart = MyUtils.GetTimeMillis();
        wheel.cw_rotation = 0;
        wheel.cw_trench_light = true;
        break;
      case Scorekeeper_InfiniteRecharge.WheelStates.STAGE2_ARMED:
        if (Math.Abs(wheel.cw_rotation) >= 24)
        {
          wheel.cw_stateMachine = Scorekeeper_InfiniteRecharge.WheelStates.STAGE2_ROTATED;
          wheel.cw_waitstart = MyUtils.GetTimeMillis();
          wheel.cw_flash_lights = true;
          wheel.cw_ballcount = 0;
          break;
        }
        wheel.cw_currcolor = this.GetWheelColor(wheel.controlwheel.angle);
        if (wheel.cw_currcolor == wheel.cw_lastcolor)
          break;
        int num = wheel.cw_currcolor - wheel.cw_lastcolor;
        if (num == 1 || num <= -2)
          ++wheel.cw_rotation;
        else
          --wheel.cw_rotation;
        wheel.cw_lastcolor = wheel.cw_currcolor;
        break;
      case Scorekeeper_InfiniteRecharge.WheelStates.STAGE2_ROTATED:
        if (Math.Abs(wheel.cw_rotation) >= 40)
        {
          wheel.cw_flash_lights = false;
          wheel.cw_rotation = 0;
          wheel.cw_stateMachine = Scorekeeper_InfiniteRecharge.WheelStates.STAGE2_ARMED;
          break;
        }
        wheel.cw_currcolor = this.GetWheelColor(wheel.controlwheel.angle);
        if (wheel.cw_currcolor != wheel.cw_lastcolor)
        {
          wheel.cw_waitstart = MyUtils.GetTimeMillis();
          if (wheel.cw_currcolor == 0)
            ++wheel.cw_rotation;
          else if (wheel.cw_currcolor == 3)
            --wheel.cw_rotation;
          else
            wheel.cw_rotation += wheel.cw_currcolor - wheel.cw_lastcolor;
          wheel.cw_lastcolor = wheel.cw_currcolor;
        }
        if (MyUtils.GetTimeMillis() - wheel.cw_waitstart <= 2000L)
          break;
        wheel.cw_flash_lights = false;
        wheel.cw_trench_light = false;
        wheel.cw_stateMachine = Scorekeeper_InfiniteRecharge.WheelStates.STAGE2_COMPLETE;
        wheel.cw_ballcount = 0;
        break;
      case Scorekeeper_InfiniteRecharge.WheelStates.STAGE2_COMPLETE:
        if ((!(this.ir_settings.GAMEVERSION == "2020") || wheel.cw_ballcount < 20 + this.ir_settings.SHIELD_OFFSET) && (!(this.ir_settings.GAMEVERSION == "2021") || wheel.cw_ballcount < 15 + this.ir_settings.SHIELD_OFFSET))
          break;
        wheel.cw_stateMachine = Scorekeeper_InfiniteRecharge.WheelStates.STAGE3_ARMED;
        wheel.cw_trench_light = true;
        if (wheel.cw_stage3_color != this.GetWheelColor(wheel.controlwheel.angle))
          break;
        ++wheel.cw_stage3_color;
        if (wheel.cw_stage3_color <= 3)
          break;
        wheel.cw_stage3_color = 0;
        break;
      case Scorekeeper_InfiniteRecharge.WheelStates.STAGE3_ARMED:
        if (wheel.cw_stage3_color != this.GetWheelColor(wheel.controlwheel.angle))
          break;
        wheel.cw_stateMachine = Scorekeeper_InfiniteRecharge.WheelStates.STAGE3_ALIGNED;
        wheel.cw_waitstart = MyUtils.GetTimeMillis();
        break;
      case Scorekeeper_InfiniteRecharge.WheelStates.STAGE3_ALIGNED:
        if (wheel.cw_stage3_color != this.GetWheelColor(wheel.controlwheel.angle))
        {
          wheel.cw_stateMachine = Scorekeeper_InfiniteRecharge.WheelStates.STAGE3_ARMED;
          wheel.cw_flash_lights = false;
          break;
        }
        if (MyUtils.GetTimeMillis() - wheel.cw_waitstart >= 2000L)
          wheel.cw_flash_lights = true;
        if (MyUtils.GetTimeMillis() - wheel.cw_waitstart < 5000L)
          break;
        wheel.cw_stateMachine = Scorekeeper_InfiniteRecharge.WheelStates.STAGE3_COMPLETE;
        wheel.cw_flash_lights = false;
        wheel.cw_trench_light = false;
        break;
    }
  }

  private int GetWheelColor(float angle)
  {
    angle += 26f;
    if ((double) angle < 0.0)
      angle += 360f;
    if ((double) angle >= 179.990005493164)
      angle -= 180f;
    return (int) ((double) angle / 45.0);
  }

  private void PP_SetLights(
    List<MeshRenderer> pp_array,
    Scorekeeper_InfiniteRecharge.WheelStruct cw_wheel,
    Material pp_material_team,
    MeshRenderer wheellight,
    MeshRenderer[] stagelights,
    Transform marker)
  {
    if (pp_array.Count < 15)
      return;
    for (int index = 0; index < pp_array.Count; index += 2)
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) pp_array[index].material);
      UnityEngine.Object.Destroy((UnityEngine.Object) pp_array[index + 1].material);
      switch (cw_wheel.cw_stateMachine)
      {
        case Scorekeeper_InfiniteRecharge.WheelStates.RESET:
        case Scorekeeper_InfiniteRecharge.WheelStates.STAGE3_COMPLETE:
          pp_array[index].material = this.pp_material_off;
          pp_array[index + 1].material = this.pp_material_off;
          break;
        case Scorekeeper_InfiniteRecharge.WheelStates.READY:
          if (index < 20 && index < cw_wheel.cw_ballcount * 4 || index >= 20 && index < 20 + (cw_wheel.cw_ballcount - 5) * 2)
          {
            pp_array[index].material = pp_material_team;
            pp_array[index + 1].material = pp_material_team;
            break;
          }
          pp_array[index].material = this.pp_material_off;
          pp_array[index + 1].material = this.pp_material_off;
          break;
        case Scorekeeper_InfiniteRecharge.WheelStates.STAGE1_COMPLETE:
        case Scorekeeper_InfiniteRecharge.WheelStates.STAGE2_COMPLETE:
          if (this.ir_settings.GAMEVERSION == "2020" && (index < 10 && index < cw_wheel.cw_ballcount || index >= 10 && index < (cw_wheel.cw_ballcount - 5) * 2) || this.ir_settings.GAMEVERSION == "2021" && index < cw_wheel.cw_ballcount * 2)
          {
            pp_array[index].material = pp_material_team;
            pp_array[index + 1].material = pp_material_team;
            break;
          }
          pp_array[index].material = this.pp_material_off;
          pp_array[index + 1].material = this.pp_material_off;
          break;
        default:
          if ((MyUtils.GetTimeMillis() - (long) (index * 33)) % 500L > 250L)
          {
            pp_array[index].material = this.pp_material_yellow;
            pp_array[index + 1].material = this.pp_material_yellow;
            break;
          }
          pp_array[index].material = pp_material_team;
          pp_array[index + 1].material = pp_material_team;
          break;
      }
    }
    if (cw_wheel.cw_trench_light)
    {
      if (cw_wheel.cw_flash_lights)
        wheellight.enabled = MyUtils.GetTimeMillis() % 1000L > 500L;
      else
        wheellight.enabled = true;
    }
    else
      wheellight.enabled = false;
    stagelights[0].gameObject.SetActive(cw_wheel.cw_stateMachine >= Scorekeeper_InfiniteRecharge.WheelStates.STAGE1_COMPLETE);
    stagelights[1].gameObject.SetActive(cw_wheel.cw_stateMachine >= Scorekeeper_InfiniteRecharge.WheelStates.STAGE2_COMPLETE);
    stagelights[2].gameObject.SetActive(cw_wheel.cw_stateMachine >= Scorekeeper_InfiniteRecharge.WheelStates.STAGE3_COMPLETE);
    if (cw_wheel.cw_stateMachine == Scorekeeper_InfiniteRecharge.WheelStates.STAGE3_ARMED || cw_wheel.cw_stateMachine == Scorekeeper_InfiniteRecharge.WheelStates.STAGE3_ALIGNED)
    {
      marker.gameObject.SetActive(true);
      Quaternion localRotation = marker.localRotation with
      {
        eulerAngles = new Vector3(0.0f, -45f * (float) cw_wheel.cw_stage3_color, 0.0f)
      };
      marker.localRotation = localRotation;
    }
    else
      marker.gameObject.SetActive(false);
  }

  private void UpdatePowerUps()
  {
    if (GLOBALS.CLIENT_MODE)
      return;
    foreach (Scorekeeper_InfiniteRecharge.RobotStates robotstate in this.robotstates)
    {
      for (int index = robotstate.myoffensive_powerups.Count - 1; index >= 0; --index)
      {
        if (robotstate.myoffensive_powerups[index].GetRemainingTime() < 0L)
          robotstate.myoffensive_powerups.RemoveAt(index);
      }
      for (int index = robotstate.mydeffensive_powerups.Count - 1; index >= 0; --index)
      {
        if (robotstate.mydeffensive_powerups[index].GetRemainingTime() < 0L)
          robotstate.mydeffensive_powerups.RemoveAt(index);
      }
    }
    foreach (PowerUpScript current_pu in this.powerup_home)
      this.ServicePowerUp(current_pu, this.ir_settings.PU_HOME, this.ir_settings.PU_HOME_TYPE);
    this.ServicePowerUp(this.powerup_center, this.ir_settings.PU_CENTER, this.ir_settings.PU_CENTER_TYPE);
    foreach (Scorekeeper_InfiniteRecharge.RobotStates robotstate in this.robotstates)
    {
      robotstate.robot.TweakPerformance(0.0f, 0.0f);
      robotstate.robot.ScaleStickControls(1f);
      robotstate.robot.TurnOnRenderers();
      for (int index = robotstate.myoffensive_powerups.Count - 1; index >= 0; --index)
      {
        Scorekeeper_InfiniteRecharge.PowerUpData myoffensivePowerup = robotstate.myoffensive_powerups[index];
        this.PU_ApplyPowerup(robotstate.robot, myoffensivePowerup.mypowerup);
      }
      for (int index = robotstate.mydeffensive_powerups.Count - 1; index >= 0; --index)
      {
        Scorekeeper_InfiniteRecharge.PowerUpData mydeffensivePowerup = robotstate.mydeffensive_powerups[index];
        this.PU_ApplyPowerup(robotstate.robot, mydeffensivePowerup.mypowerup);
      }
    }
  }

  private void PU_ApplyPowerup(RobotInterface3D robot, PowerUpType powerup)
  {
    this.clean_run = false;
    switch (powerup)
    {
      case PowerUpType.SPEED:
        robot.TweakPerformance((float) (1.0 + (double) this.ir_settings.PU_STRENGTH / 100.0), 1f);
        break;
      case PowerUpType.TORQUE:
        robot.TweakPerformance(1f, (float) (1.0 + 3.0 * ((double) this.ir_settings.PU_STRENGTH / 100.0)));
        break;
      case PowerUpType.INVISIBILITY:
        robot.TurnOffRenderers(true);
        break;
      case PowerUpType.SLOW:
        robot.TweakPerformance((float) (1.0 / (1.0 + (double) this.ir_settings.PU_STRENGTH / 100.0)), 1f);
        break;
      case PowerUpType.WEAK:
        robot.TweakPerformance(1f, (float) (1.0 / (1.0 + (double) this.ir_settings.PU_STRENGTH / 100.0)));
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
    if (current_pu.NeedsServicing() && (bool) (UnityEngine.Object) current_pu.GetOwner())
    {
      current_pu.Serviced();
      Scorekeeper_InfiniteRecharge.RobotStates robotState = this.GetRobotState(current_pu.GetOwner());
      if (robotState == null)
        return;
      Scorekeeper_InfiniteRecharge.PowerUpData powerUpData = new Scorekeeper_InfiniteRecharge.PowerUpData();
      powerUpData.start_time = MyUtils.GetTimeMillisSinceStart();
      powerUpData.mypowerup = current_pu.myPower;
      switch (powerUpData.mypowerup)
      {
        case PowerUpType.SPEED:
        case PowerUpType.TORQUE:
        case PowerUpType.INVISIBILITY:
          if (!this.ir_settings.PU_ONLYONE || robotState.myoffensive_powerups.Count < 1)
          {
            powerUpData.duration = (long) (this.ir_settings.PU_OFFENSIVE_DURATION * 1000);
            bool flag = false;
            foreach (Scorekeeper_InfiniteRecharge.PowerUpData myoffensivePowerup in robotState.myoffensive_powerups)
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
          powerUpData.duration = (long) (this.ir_settings.PU_DEFENSIVE_DURATION * 1000);
          using (List<Scorekeeper_InfiniteRecharge.RobotStates>.Enumerator enumerator = this.robotstates.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              Scorekeeper_InfiniteRecharge.RobotStates current = enumerator.Current;
              if (current.isRed != robotState.isRed)
                current.mydeffensive_powerups.Add(powerUpData);
            }
            break;
          }
      }
    }
    if (this.ir_settings.ENABLE_POWERUPS & isenabled)
    {
      if (!current_pu.IsDisabled() || MyUtils.GetTimeMillisSinceStart() - current_pu.GetTimeStarted() <= (long) (this.ir_settings.PU_RESPAWN * 1000))
        return;
      List<PowerUpType> powerUpTypeList = new List<PowerUpType>();
      if (type_allowed == 0 || type_allowed == 2)
      {
        if (this.ir_settings.PU_SPEED)
          powerUpTypeList.Add(PowerUpType.SPEED);
        if (this.ir_settings.PU_TORQUE)
          powerUpTypeList.Add(PowerUpType.TORQUE);
        if (this.ir_settings.PU_INVISIBLE)
          powerUpTypeList.Add(PowerUpType.INVISIBILITY);
      }
      if (type_allowed == 1 || type_allowed == 2)
      {
        if (this.ir_settings.PU_SLOW)
          powerUpTypeList.Add(PowerUpType.SLOW);
        if (this.ir_settings.PU_WEAK)
          powerUpTypeList.Add(PowerUpType.WEAK);
        if (this.ir_settings.PU_INVERTED)
          powerUpTypeList.Add(PowerUpType.INVERTED);
      }
      if (powerUpTypeList.Count < 1)
        return;
      int index = UnityEngine.Random.Range(0, powerUpTypeList.Count);
      current_pu.PU_Enable(powerUpTypeList[index]);
    }
    else
      current_pu.PU_Disable();
  }

  public override bool PU_CheckIfClearToAssign(PowerUpScript thepu, RobotInterface3D robot)
  {
    Scorekeeper_InfiniteRecharge.RobotStates robotState = this.GetRobotState(robot);
    return robotState != null && (robotState.myoffensive_powerups.Count <= 0 || !this.ir_settings.PU_ONLYONE || thepu.myPower >= PowerUpType.SLOW);
  }

  public override Transform GetOverlays(int id, Transform parent)
  {
    Scorekeeper_InfiniteRecharge.RobotStates robotStateById = this.GetRobotStateById(id);
    if (robotStateById == null)
      return (Transform) null;
    if (robotStateById.mydeffensive_powerups.Count == 0 && robotStateById.myoffensive_powerups.Count == 0)
      return (Transform) null;
    int num = 10;
    foreach (Scorekeeper_InfiniteRecharge.PowerUpData myoffensivePowerup in robotStateById.myoffensive_powerups)
    {
      GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.powerup_overlay, parent.transform);
      gameObject.transform.Find("Time").GetComponent<Text>().text = (myoffensivePowerup.GetRemainingTime() / 1000L).ToString();
      gameObject.transform.Find("Type").GetComponent<Text>().text = myoffensivePowerup.GetPowerChar();
      gameObject.transform.localPosition = gameObject.transform.localPosition with
      {
        y = (float) num
      };
      num += 45;
    }
    foreach (Scorekeeper_InfiniteRecharge.PowerUpData mydeffensivePowerup in robotStateById.mydeffensive_powerups)
    {
      GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.powerup_overlay, parent.transform);
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
    Scorekeeper_InfiniteRecharge.RobotStates robotStateById = this.GetRobotStateById(id);
    if (robotStateById == null || robotStateById.mydeffensive_powerups.Count == 0 && robotStateById.myoffensive_powerups.Count == 0)
      return "";
    string overlaysString = "";
    foreach (Scorekeeper_InfiniteRecharge.PowerUpData myoffensivePowerup in robotStateById.myoffensive_powerups)
      overlaysString = overlaysString + ":" + ((int) myoffensivePowerup.mypowerup).ToString() + ":" + (object) (myoffensivePowerup.GetRemainingTime() / 1000L);
    foreach (Scorekeeper_InfiniteRecharge.PowerUpData mydeffensivePowerup in robotStateById.mydeffensive_powerups)
      overlaysString = overlaysString + ":" + ((int) mydeffensivePowerup.mypowerup).ToString() + ":" + (object) (mydeffensivePowerup.GetRemainingTime() / 1000L);
    return overlaysString;
  }

  public override void ScorerUpdate()
  {
    bool flag = true;
    if (this.timerstate != Scorekeeper.TimerState.PAUSED && this.timerstate != Scorekeeper.TimerState.RUNNING)
      flag = false;
    this.UpdateControlPanel();
    this.UpdatePowerUps();
    this.CalculateScores();
    if (GLOBALS.CLIENT_MODE)
      return;
    if (this.ir_settings.ENABLE_G10_G11 & flag)
      this.Do_IRL_Blocking();
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
      Scorekeeper_InfiniteRecharge.RobotStates robotstate1 = this.robotstates[count3];
      if ((bool) (UnityEngine.Object) robotstate1.robot)
      {
        if (robotstate1.counting_down)
        {
          if ((double) time - (double) robotstate1.start_time > (double) this.ir_settings.BLOCKING_DURATION)
          {
            robotstate1.counting_down = false;
            robotstate1.start_time = -1f;
            robotstate1.robot.SetProgressBar(0.0f);
          }
          else
            robotstate1.robot.SetProgressBar((this.ir_settings.BLOCKING_DURATION - time + robotstate1.start_time) / this.ir_settings.BLOCKING_DURATION);
        }
        if (robotstate1.counting_up)
        {
          if (!this.ir_settings.ENABLE_BLOCKING && !this.ir_settings.ENABLE_WALLBLOCKING)
          {
            robotstate1.counting_up = false;
            robotstate1.counting_down = true;
            robotstate1.start_time = time - (this.ir_settings.BLOCKING_DURATION - (time - robotstate1.start_time));
          }
          else if ((double) time - (double) robotstate1.start_time > (double) this.ir_settings.BLOCKING_DURATION)
          {
            robotstate1.counting_up = false;
            robotstate1.start_time = -1f;
            robotstate1.robot.SetProgressBar(0.0f);
            robotstate1.robot.MarkForReset(this.ir_settings.BLOCKING_RESET_HOLDING_TIME);
            if (flag)
            {
              List<RobotInterface3D> allEnemies = robotstate1.robot.GetAllEnemies();
              foreach (RobotInterface3D robotInterface3D in allEnemies)
              {
                if (this.player_opr.ContainsKey(GLOBALS.client_names[robotInterface3D.myRobotID.id]))
                  this.player_opr[GLOBALS.client_names[robotInterface3D.myRobotID.id]] += (float) (this.ir_settings.PENALTY_BLOCKING / allEnemies.Count);
                else
                  this.player_opr[GLOBALS.client_names[robotInterface3D.myRobotID.id]] = (float) (this.ir_settings.PENALTY_BLOCKING / allEnemies.Count);
              }
              if (robotstate1.isRed)
                this.penalties_blue += (double) this.ir_settings.PENALTY_BLOCKING;
              else
                this.penalties_red += (double) this.ir_settings.PENALTY_BLOCKING;
            }
          }
          else
            robotstate1.robot.SetProgressBar((time - robotstate1.start_time) / this.ir_settings.BLOCKING_DURATION);
        }
        foreach (RobotInterface3D allEnemy in robotstate1.robot.GetAllEnemies())
        {
          int robotStateIndex = this.GetRobotStateIndex(allEnemy);
          if (robotStateIndex >= 0)
          {
            Scorekeeper_InfiniteRecharge.RobotStates robotstate2 = this.robotstates[robotStateIndex];
            if (this.ir_settings.ENABLE_BLOCKING && robotstate1.isRed && this.red_safety.IsFriendInside(robotstate1.robot.transform) && (double) this.red_safety.GetClosestDistance(robotstate1.robot.rb_body.transform.position) + 0.00999999977648258 < (double) this.blue_safety.GetClosestDistance(allEnemy.rb_body.transform.position) || this.ir_settings.ENABLE_BLOCKING && !robotstate1.isRed && this.blue_safety.IsFriendInside(robotstate1.robot.transform) && (double) this.blue_safety.GetClosestDistance(robotstate1.robot.rb_body.transform.position) + 0.00999999977648258 < (double) this.red_safety.GetClosestDistance(allEnemy.rb_body.transform.position) || this.ir_settings.ENABLE_WALLBLOCKING && robotstate1.isRed && this.red_wallsafety.IsFriendInside(robotstate1.robot.transform) && (double) this.red_wallsafety.GetClosestDistance(robotstate1.robot.rb_body.transform.position) + 0.00999999977648258 < (double) this.blue_wallsafety.GetClosestDistance(allEnemy.rb_body.transform.position) || this.ir_settings.ENABLE_WALLBLOCKING && !robotstate1.isRed && this.blue_wallsafety.IsFriendInside(robotstate1.robot.transform) && (double) this.blue_wallsafety.GetClosestDistance(robotstate1.robot.rb_body.transform.position) + 0.00999999977648258 < (double) this.red_wallsafety.GetClosestDistance(allEnemy.rb_body.transform.position))
            {
              robotstate2.isblocking = true;
              if (robotstate2.counting_down)
              {
                robotstate2.counting_down = false;
                robotstate2.counting_up = true;
                robotstate2.start_time = time - (this.ir_settings.BLOCKING_DURATION - (time - robotstate2.start_time));
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
    while (count4 >= 1 && (this.ir_settings.ENABLE_BLOCKING || this.ir_settings.ENABLE_WALLBLOCKING))
    {
      --count4;
      if (!this.robotstates[count4].isblocking && this.robotstates[count4].counting_up)
      {
        this.robotstates[count4].counting_up = false;
        this.robotstates[count4].counting_down = true;
        this.robotstates[count4].start_time = time - (this.ir_settings.BLOCKING_DURATION - (time - this.robotstates[count4].start_time));
      }
    }
    if (this.loading_blue.friends.Count > 0)
    {
      foreach (Transform transform in this.ballsheld_blue)
      {
        if ((bool) (UnityEngine.Object) transform)
        {
          Rigidbody component = transform.GetComponent<Rigidbody>();
          component.isKinematic = false;
          component.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }
      }
      this.ballsheld_blue.Clear();
    }
    if (this.loading_red.friends.Count <= 0)
      return;
    foreach (Transform transform in this.ballsheld_red)
    {
      if ((bool) (UnityEngine.Object) transform)
      {
        Rigidbody component = transform.GetComponent<Rigidbody>();
        component.isKinematic = false;
        component.collisionDetectionMode = CollisionDetectionMode.Continuous;
      }
    }
    this.ballsheld_red.Clear();
  }

  public override void RobotCounterExpired(RobotID bot)
  {
  }

  private void Do_IRL_Blocking()
  {
    if (GLOBALS.CLIENT_MODE)
      return;
    int count = this.robotstates.Count;
    while (count >= 1)
    {
      --count;
      Scorekeeper_InfiniteRecharge.RobotStates robotstate = this.robotstates[count];
      if ((bool) (UnityEngine.Object) robotstate.robot)
      {
        this.G10_Release(robotstate);
        this.G11_Release(robotstate);
        bool flag1 = false;
        RobotID robotId;
        if (robotstate.isRed)
        {
          if ((UnityEngine.Object) (robotId = this.G10_Check(robotstate, this.BlueTrenchRun)) != (UnityEngine.Object) null)
          {
            flag1 = true;
            robotstate.clear_zone = this.BlueTrenchRun_clear;
          }
          else if ((UnityEngine.Object) (robotId = this.G10_Check(robotstate, this.BlueTargetZone)) != (UnityEngine.Object) null)
          {
            flag1 = true;
            robotstate.clear_zone = this.BlueTargetZone_clear;
          }
          else if ((UnityEngine.Object) (robotId = this.G10_Check(robotstate, this.BlueLoadingZone)) != (UnityEngine.Object) null)
          {
            flag1 = true;
            robotstate.clear_zone = this.BlueLoadingZone_clear;
          }
        }
        else if ((UnityEngine.Object) (robotId = this.G10_Check(robotstate, this.RedTrenchRun)) != (UnityEngine.Object) null)
        {
          flag1 = true;
          robotstate.clear_zone = this.RedTrenchRun_clear;
        }
        else if ((UnityEngine.Object) (robotId = this.G10_Check(robotstate, this.RedTargetZone)) != (UnityEngine.Object) null)
        {
          flag1 = true;
          robotstate.clear_zone = this.RedTargetZone_clear;
        }
        else if ((UnityEngine.Object) (robotId = this.G10_Check(robotstate, this.RedLoadingZone)) != (UnityEngine.Object) null)
        {
          flag1 = true;
          robotstate.clear_zone = this.RedLoadingZone_clear;
        }
        if (flag1)
        {
          robotstate.immune_to_G10 = true;
          robotstate.G10_G11_starttime = MyUtils.GetTimeMillis();
          if (robotstate.isRed)
            this.penalties_blue += (double) this.ir_settings.PENALTY_G10_G11;
          else
            this.penalties_red += (double) this.ir_settings.PENALTY_G10_G11;
          this.player_opr[GLOBALS.client_names[robotId.id]] += (float) this.ir_settings.PENALTY_G10_G11;
          this.CreateFoul(robotstate.robotID.id);
        }
        bool flag2 = false;
        Scorekeeper_InfiniteRecharge.RobotStates robotStates;
        if (!robotstate.isRed)
        {
          robotStates = this.G11_Check(robotstate, this.BlueTargetZone) ?? this.G11_Check(robotstate, this.BlueLoadingZone);
          if (robotStates != null)
            flag2 = true;
        }
        else
        {
          robotStates = this.G11_Check(robotstate, this.RedTargetZone) ?? this.G11_Check(robotstate, this.RedLoadingZone);
          if (robotStates != null)
            flag2 = true;
        }
        if (flag2)
        {
          if (robotstate.isRed)
            this.penalties_red += (double) this.ir_settings.PENALTY_G10_G11;
          else
            this.penalties_blue += (double) this.ir_settings.PENALTY_G10_G11;
          this.player_opr[GLOBALS.client_names[robotstate.robotID.id]] += (float) this.ir_settings.PENALTY_G10_G11;
          this.CreateFoul(robotStates.robotID.id);
        }
      }
    }
  }

  private RobotID G10_Check(
    Scorekeeper_InfiniteRecharge.RobotStates currbot,
    GenericFieldTracker zone)
  {
    if (!zone.IsRobotInside(currbot.robot.myRobotID))
      return (RobotID) null;
    if (currbot.immune_to_G10 || currbot.immune_to_G11)
      return (RobotID) null;
    foreach (RobotInterface3D allrobot in this.allrobots)
    {
      if ((bool) (UnityEngine.Object) allrobot && (bool) (UnityEngine.Object) allrobot.rb_body && allrobot.enemies.Contains(currbot.robot))
        return allrobot.myRobotID;
    }
    return (RobotID) null;
  }

  private void G10_Release(Scorekeeper_InfiniteRecharge.RobotStates currbot)
  {
    if ((int) (MyUtils.GetTimeMillis() - currbot.G10_G11_starttime) < this.ir_settings.G10_G11_BLANKING)
      return;
    if ((bool) (UnityEngine.Object) currbot.clear_zone && !currbot.clear_zone.IsRobotInside(currbot.robotID))
    {
      currbot.immune_to_G10 = false;
      currbot.clear_zone = (GenericFieldTracker) null;
      if (currbot.immune_to_G11)
        return;
      currbot.robot.OverrideColor(-1);
    }
    else
    {
      Vector3 position1 = currbot.robot.rb_body.position with
      {
        y = 0.0f
      };
      foreach (RobotInterface3D allrobot in this.allrobots)
      {
        if ((bool) (UnityEngine.Object) allrobot && (bool) (UnityEngine.Object) allrobot.rb_body && currbot.isRed != allrobot.myRobotID.is_red)
        {
          Vector3 position2 = allrobot.rb_body.position with
          {
            y = 0.0f
          };
          if ((double) Vector3.Distance(position1, position2) < (double) this.ir_settings.G10_CLEAR_DISTANCE)
            return;
        }
      }
      currbot.immune_to_G10 = false;
      currbot.clear_zone = (GenericFieldTracker) null;
      if (currbot.immune_to_G11)
        return;
      currbot.robot.OverrideColor(-1);
    }
  }

  private Scorekeeper_InfiniteRecharge.RobotStates G11_Check(
    Scorekeeper_InfiniteRecharge.RobotStates currbot,
    GenericFieldTracker zone)
  {
    if (!zone.IsRobotInside(currbot.robot.myRobotID))
      return (Scorekeeper_InfiniteRecharge.RobotStates) null;
    foreach (RobotInterface3D enemy in currbot.robot.enemies)
    {
      if ((bool) (UnityEngine.Object) enemy && (bool) (UnityEngine.Object) enemy.rb_body)
      {
        Scorekeeper_InfiniteRecharge.RobotStates robotstate = this.robotstates[this.GetRobotStateIndex(enemy)];
        if (!robotstate.immune_to_G10 && !robotstate.immune_to_G11)
        {
          robotstate.immune_to_G11 = true;
          robotstate.G10_G11_starttime = MyUtils.GetTimeMillis();
          return robotstate;
        }
      }
    }
    return (Scorekeeper_InfiniteRecharge.RobotStates) null;
  }

  private void G11_Release(Scorekeeper_InfiniteRecharge.RobotStates currbot)
  {
    if (!currbot.immune_to_G11 || (int) (MyUtils.GetTimeMillis() - currbot.G10_G11_starttime) <= this.ir_settings.G10_G11_BLANKING)
      return;
    currbot.immune_to_G11 = false;
    currbot.G10_G11_starttime = -1L;
    if (currbot.immune_to_G10)
      return;
    currbot.robot.OverrideColor(-1);
  }

  private void CreateFoul(int robotid)
  {
    if (!this.allrobots_byid.ContainsKey(robotid))
      return;
    RobotInterface3D robotInterface3D = this.allrobots_byid[robotid];
    if (!(bool) (UnityEngine.Object) robotInterface3D.rb_body)
      return;
    if (!GLOBALS.HEADLESS_MODE)
      UnityEngine.Object.Instantiate<GameObject>(this.fault_prefab, robotInterface3D.rb_body.transform.position, Quaternion.identity).transform.SetParent(robotInterface3D.rb_body.transform);
    robotInterface3D.OverrideColor(0);
    if (!GLOBALS.SERVER_MODE)
      return;
    if (this.foul_string.Length > 0)
      this.foul_string += ":";
    this.foul_string += (string) (object) robotid;
  }

  public override void FieldChangedTrigger()
  {
    List<Scorekeeper_InfiniteRecharge.RobotStates> robotStatesList = new List<Scorekeeper_InfiniteRecharge.RobotStates>();
    for (int index = 0; index < this.allrobots.Count; ++index)
    {
      int robotStateIndex = this.GetRobotStateIndex(this.allrobots[index]);
      if (robotStateIndex < 0)
      {
        Scorekeeper_InfiniteRecharge.RobotStates robotStates = new Scorekeeper_InfiniteRecharge.RobotStates();
        robotStates.robot = this.allrobots[index];
        if (!((UnityEngine.Object) this.allrobots[index].gameObject == (UnityEngine.Object) null) && !((UnityEngine.Object) this.allrobots[index].gameObject.GetComponent<RobotID>() == (UnityEngine.Object) null))
        {
          robotStates.isRed = this.allrobots[index].gameObject.GetComponent<RobotID>().starting_pos.StartsWith("Red");
          robotStates.robotID = robotStates.robot.myRobotID;
          robotStatesList.Add(robotStates);
        }
      }
      else
        robotStatesList.Add(this.robotstates[robotStateIndex]);
    }
    this.robotstates = robotStatesList;
  }

  private void ResetRobotStates()
  {
    foreach (Scorekeeper_InfiniteRecharge.RobotStates robotstate in this.robotstates)
    {
      robotstate.Reset();
      if ((bool) (UnityEngine.Object) robotstate.robot)
      {
        robotstate.robot.TweakPerformance(-1f, -1f);
        robotstate.robot.SetProgressBar(0.0f);
        robotstate.robot.HoldRobot(false);
      }
    }
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

  private Scorekeeper_InfiniteRecharge.RobotStates GetRobotState(
    RobotInterface3D robot)
  {
    for (int index = 0; index < this.robotstates.Count; ++index)
    {
      if ((UnityEngine.Object) this.robotstates[index].robot == (UnityEngine.Object) robot)
        return this.robotstates[index];
    }
    return (Scorekeeper_InfiniteRecharge.RobotStates) null;
  }

  private Scorekeeper_InfiniteRecharge.RobotStates GetRobotStateById(
    int id)
  {
    return !this.allrobots_byid.ContainsKey(id) ? (Scorekeeper_InfiniteRecharge.RobotStates) null : this.GetRobotState(this.allrobots_byid[id]);
  }

  private void AddToPlayerOPR(Dictionary<int, int> indata, float multiplier, bool is_red)
  {
    foreach (int key in indata.Keys)
    {
      if (!this.allrobots_byid.ContainsKey(key) || (UnityEngine.Object) this.allrobots_byid[key] == (UnityEngine.Object) null || (UnityEngine.Object) this.allrobots_byid[key].myRobotID == (UnityEngine.Object) null)
        this.DoFieldChanged();
      if (this.allrobots_byid.ContainsKey(key) && !((UnityEngine.Object) this.allrobots_byid[key] == (UnityEngine.Object) null) && !((UnityEngine.Object) this.allrobots_byid[key].myRobotID == (UnityEngine.Object) null) && this.allrobots_byid[key].myRobotID.is_red == is_red)
      {
        if (this.player_opr.ContainsKey(GLOBALS.client_names[key]))
          this.player_opr[GLOBALS.client_names[key]] += multiplier * (float) indata[key];
        else
          this.player_opr[GLOBALS.client_names[key]] = multiplier * (float) indata[key];
      }
    }
  }

  private void CalculateScores()
  {
    if (this.timerstate == Scorekeeper.TimerState.RUNNING)
    {
      this.score_endgame_red = 0.0;
      this.score_endgame_blue = 0.0;
      int powercellsRed = this.powercells_red;
      this.powercells_red += this.goal_high_red.number_of_balls + this.goal_mid_red.number_of_balls + this.goal_low_red.number_of_balls;
      this.cw_red_wheel.cw_ballcount += this.powercells_red - powercellsRed;
      int powercellsBlue = this.powercells_blue;
      this.powercells_blue += this.goal_high_blue.number_of_balls + this.goal_mid_blue.number_of_balls + this.goal_low_blue.number_of_balls;
      this.cw_blue_wheel.cw_ballcount += this.powercells_blue - powercellsBlue;
      if (this.ir_settings.GAMEVERSION == "2020")
      {
        this.score_cpanel_red = this.cw_red_wheel.cw_stateMachine >= Scorekeeper_InfiniteRecharge.WheelStates.STAGE2_COMPLETE ? 10.0 : 0.0;
        this.score_cpanel_blue = this.cw_blue_wheel.cw_stateMachine >= Scorekeeper_InfiniteRecharge.WheelStates.STAGE2_COMPLETE ? 10.0 : 0.0;
      }
      else if (this.ir_settings.GAMEVERSION == "2021")
      {
        this.score_cpanel_red = this.cw_red_wheel.cw_stateMachine >= Scorekeeper_InfiniteRecharge.WheelStates.STAGE2_COMPLETE ? 15.0 : 0.0;
        this.score_cpanel_blue = this.cw_blue_wheel.cw_stateMachine >= Scorekeeper_InfiniteRecharge.WheelStates.STAGE2_COMPLETE ? 15.0 : 0.0;
      }
      this.cw_rotation_r = this.cw_red_wheel.cw_stateMachine >= Scorekeeper_InfiniteRecharge.WheelStates.STAGE2_COMPLETE;
      this.cw_position_r = this.cw_red_wheel.cw_stateMachine >= Scorekeeper_InfiniteRecharge.WheelStates.STAGE3_COMPLETE;
      this.cw_rotation_b = this.cw_blue_wheel.cw_stateMachine >= Scorekeeper_InfiniteRecharge.WheelStates.STAGE2_COMPLETE;
      this.cw_position_b = this.cw_blue_wheel.cw_stateMachine >= Scorekeeper_InfiniteRecharge.WheelStates.STAGE3_COMPLETE;
      this.score_cpanel_red += this.cw_red_wheel.cw_stateMachine >= Scorekeeper_InfiniteRecharge.WheelStates.STAGE3_COMPLETE ? 20.0 : 0.0;
      this.score_cpanel_blue += this.cw_blue_wheel.cw_stateMachine >= Scorekeeper_InfiniteRecharge.WheelStates.STAGE3_COMPLETE ? 20.0 : 0.0;
      if (this.time_total.TotalSeconds > 135.0)
      {
        this.auto_pc_low_r += this.goal_low_red.number_of_balls;
        this.auto_pc_mid_r += this.goal_mid_red.number_of_balls;
        this.auto_pc_high_r += this.goal_high_red.number_of_balls;
        this.auto_pc_low_b += this.goal_low_blue.number_of_balls;
        this.auto_pc_mid_b += this.goal_mid_blue.number_of_balls;
        this.auto_pc_high_b += this.goal_high_blue.number_of_balls;
        this.score_auto_red += (double) (6 * this.goal_high_red.number_of_balls + 4 * this.goal_mid_red.number_of_balls + 2 * this.goal_low_red.number_of_balls);
        this.score_auto_blue += (double) (6 * this.goal_high_blue.number_of_balls + 4 * this.goal_mid_blue.number_of_balls + 2 * this.goal_low_blue.number_of_balls);
        this.AddToPlayerOPR(this.goal_high_red.user_ball_count, 6f, true);
        this.AddToPlayerOPR(this.goal_mid_red.user_ball_count, 4f, true);
        this.AddToPlayerOPR(this.goal_low_red.user_ball_count, 2f, true);
        this.AddToPlayerOPR(this.goal_high_blue.user_ball_count, 6f, false);
        this.AddToPlayerOPR(this.goal_mid_blue.user_ball_count, 4f, false);
        this.AddToPlayerOPR(this.goal_low_blue.user_ball_count, 2f, false);
      }
      else
      {
        this.tele_pc_low_r += this.goal_low_red.number_of_balls;
        this.tele_pc_mid_r += this.goal_mid_red.number_of_balls;
        this.tele_pc_high_r += this.goal_high_red.number_of_balls;
        this.tele_pc_low_b += this.goal_low_blue.number_of_balls;
        this.tele_pc_mid_b += this.goal_mid_blue.number_of_balls;
        this.tele_pc_high_b += this.goal_high_blue.number_of_balls;
        this.score_teleop_red += (double) (3 * this.goal_high_red.number_of_balls + 2 * this.goal_mid_red.number_of_balls + this.goal_low_red.number_of_balls);
        this.score_teleop_blue += (double) (3 * this.goal_high_blue.number_of_balls + 2 * this.goal_mid_blue.number_of_balls + this.goal_low_blue.number_of_balls);
        this.AddToPlayerOPR(this.goal_high_red.user_ball_count, 3f, true);
        this.AddToPlayerOPR(this.goal_mid_red.user_ball_count, 2f, true);
        this.AddToPlayerOPR(this.goal_low_red.user_ball_count, 1f, true);
        this.AddToPlayerOPR(this.goal_high_blue.user_ball_count, 3f, false);
        this.AddToPlayerOPR(this.goal_mid_blue.user_ball_count, 2f, false);
        this.AddToPlayerOPR(this.goal_low_blue.user_ball_count, 1f, false);
      }
      this.player_opr_endgame.Clear();
      bool flag1 = false;
      bool flag2 = false;
      if (this.time_total.TotalSeconds < 30.0)
      {
        this.bots_parked_r = 0;
        this.bots_parked_b = 0;
        this.bots_hanging_r = 0;
        this.bots_hanging_b = 0;
        this.switch_level_r = false;
        this.switch_level_b = false;
        double num1 = 0.0;
        int num2 = 0;
        foreach (RobotInterface3D allFriend in this.redbalance.GetAllFriends())
        {
          if (!this.floor.IsFriendInside(allFriend) && !this.floor.IsEnemyInside(allFriend))
          {
            num1 += 25.0;
            this.player_opr_endgame[GLOBALS.client_names[allFriend.myRobotID.id]] = 25f;
            ++num2;
          }
          else if (this.redpark.IsFriendInside(allFriend))
          {
            this.score_endgame_red += 5.0;
            this.player_opr_endgame[GLOBALS.client_names[allFriend.myRobotID.id]] = 5f;
            flag1 = true;
            ++this.bots_parked_r;
          }
          else
            flag1 = true;
        }
        this.score_endgame_red += num1;
        this.bots_hanging_r = num2;
        double num3 = 0.0;
        int num4 = 0;
        foreach (RobotInterface3D allFriend in this.bluebalance.GetAllFriends())
        {
          if (!this.floor.IsFriendInside(allFriend) && !this.floor.IsEnemyInside(allFriend))
          {
            num3 += 25.0;
            this.player_opr_endgame[GLOBALS.client_names[allFriend.myRobotID.id]] = 25f;
            ++num4;
          }
          else if (this.bluepark.IsFriendInside(allFriend))
          {
            this.score_endgame_blue += 5.0;
            this.player_opr_endgame[GLOBALS.client_names[allFriend.myRobotID.id]] = 5f;
            flag2 = true;
            ++this.bots_parked_b;
          }
          else
            flag2 = true;
        }
        this.score_endgame_blue += num3;
        this.bots_hanging_b = num4;
        foreach (RobotInterface3D allFriend in this.redpark.GetAllFriends())
        {
          if (!this.redbalance.IsFriendInside(allFriend))
          {
            this.score_endgame_red += 5.0;
            this.player_opr_endgame[GLOBALS.client_names[allFriend.myRobotID.id]] = 5f;
            ++this.bots_parked_r;
          }
        }
        foreach (RobotInterface3D allFriend in this.bluepark.GetAllFriends())
        {
          if (!this.bluebalance.IsFriendInside(allFriend))
          {
            this.score_endgame_blue += 5.0;
            this.player_opr_endgame[GLOBALS.client_names[allFriend.myRobotID.id]] = 5f;
            ++this.bots_parked_b;
          }
        }
        if (!flag1 && num2 >= 1 && (double) Math.Abs(MyUtils.AngleWrap(this.redbalance.transform.rotation.eulerAngles.x)) < 8.0)
        {
          this.score_endgame_red += 15.0;
          foreach (RobotInterface3D allFriend in this.redbalance.GetAllFriends())
            this.player_opr_endgame[GLOBALS.client_names[allFriend.myRobotID.id]] += (float) (15 / this.redbalance.GetAllFriends().Count);
          this.switch_level_r = true;
        }
        if (!flag2 && num4 >= 1 && (double) Math.Abs(MyUtils.AngleWrap(this.bluebalance.transform.rotation.eulerAngles.x)) < 8.0)
        {
          this.score_endgame_blue += 15.0;
          foreach (RobotInterface3D allFriend in this.bluebalance.GetAllFriends())
            this.player_opr_endgame[GLOBALS.client_names[allFriend.myRobotID.id]] += (float) (15 / this.bluebalance.GetAllFriends().Count);
          this.switch_level_b = true;
        }
      }
    }
    this.ReturnBalls(this.goal_high_blue, this.ballsheld_red, this.ballholdingred);
    this.ReturnBalls(this.goal_mid_blue, this.ballsheld_red, this.ballholdingred);
    this.ReturnBalls(this.goal_low_blue, this.ballsheld_red, this.ballholdingred);
    this.ReturnBalls(this.goal_high_red, this.ballsheld_blue, this.ballholdingblue);
    this.ReturnBalls(this.goal_mid_red, this.ballsheld_blue, this.ballholdingblue);
    this.ReturnBalls(this.goal_low_red, this.ballsheld_blue, this.ballholdingblue);
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
    if (this.cw_red_wheel.cw_stateMachine == Scorekeeper_InfiniteRecharge.WheelStates.STAGE2_ROTATED)
      this.cw_red_wheel.cw_stateMachine = Scorekeeper_InfiniteRecharge.WheelStates.STAGE2_COMPLETE;
    if (this.cw_blue_wheel.cw_stateMachine == Scorekeeper_InfiniteRecharge.WheelStates.STAGE2_ROTATED)
      this.cw_blue_wheel.cw_stateMachine = Scorekeeper_InfiniteRecharge.WheelStates.STAGE2_COMPLETE;
    if (this.cw_red_wheel.cw_stateMachine == Scorekeeper_InfiniteRecharge.WheelStates.STAGE3_ALIGNED)
      this.cw_red_wheel.cw_stateMachine = Scorekeeper_InfiniteRecharge.WheelStates.STAGE3_COMPLETE;
    if (this.cw_blue_wheel.cw_stateMachine == Scorekeeper_InfiniteRecharge.WheelStates.STAGE3_ALIGNED)
      this.cw_blue_wheel.cw_stateMachine = Scorekeeper_InfiniteRecharge.WheelStates.STAGE3_COMPLETE;
    this.score_cpanel_red = this.cw_red_wheel.cw_stateMachine >= Scorekeeper_InfiniteRecharge.WheelStates.STAGE2_COMPLETE ? 10.0 : 0.0;
    this.score_cpanel_blue = this.cw_blue_wheel.cw_stateMachine >= Scorekeeper_InfiniteRecharge.WheelStates.STAGE2_COMPLETE ? 10.0 : 0.0;
    this.score_cpanel_red += this.cw_red_wheel.cw_stateMachine >= Scorekeeper_InfiniteRecharge.WheelStates.STAGE3_COMPLETE ? 20.0 : 0.0;
    this.score_cpanel_blue += this.cw_blue_wheel.cw_stateMachine >= Scorekeeper_InfiniteRecharge.WheelStates.STAGE3_COMPLETE ? 20.0 : 0.0;
    this.PP_SetLights(this.pp_red_array, this.cw_red_wheel, this.pp_material_red, this.wheellight_red, this.red_stage, this.marker_red);
    this.PP_SetLights(this.pp_blue_array, this.cw_blue_wheel, this.pp_material_blue, this.wheellight_blue, this.blue_stage, this.marker_blue);
  }

  public override int GetRedScore()
  {
    if (this.timerstate == Scorekeeper.TimerState.FINISHED)
      return this.score_redfinal;
    this.score_red = this.score_auto_red + this.score_teleop_red + this.score_endgame_red + this.penalties_red + this.score_cpanel_red;
    return (int) this.score_red + this.score_redadj;
  }

  public override int GetBlueScore()
  {
    if (this.timerstate == Scorekeeper.TimerState.FINISHED)
      return this.score_bluefinal;
    this.score_blue = this.score_auto_blue + this.score_teleop_blue + this.score_endgame_blue + this.penalties_blue + this.score_cpanel_blue;
    return (int) this.score_blue + this.score_blueadj;
  }

  private void ReturnBalls(IR_scoringBox goal, List<Transform> ballsheld, Transform ballholding)
  {
    foreach (Transform ball in goal.balls)
    {
      Rigidbody component = ball.GetComponent<Rigidbody>();
      component.velocity = Vector3.zero;
      component.angularVelocity = Vector3.zero;
      if (this.ir_settings.GAMEVERSION == "2020" && ballsheld.Count < 15 || this.ir_settings.GAMEVERSION == "2021" && ballsheld.Count < 14)
      {
        ballsheld.Add(ball);
        ball.position = ballholding.Find("Ball" + (object) ballsheld.Count).position;
        component.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        component.isKinematic = true;
      }
      else
      {
        System.Random random = new System.Random();
        if (this.ir_settings.ENABLE_BALLRETURNTOBAY)
        {
          Vector3 position = ballholding.Find("Ball3").position;
          if ((double) position.z < 0.0)
            position.z += 0.3f;
          else
            position.z -= 0.25f;
          position.y += 3f * (float) random.NextDouble();
          position.x += (float) (0.600000023841858 * random.NextDouble() - 0.300000011920929);
          position.z += (float) (0.200000002980232 * random.NextDouble() - 0.100000001490116);
          ball.position = position;
        }
        else
          ball.position = new Vector3((float) (0.5 * random.NextDouble() - 0.25), (float) (3.0 * random.NextDouble() + 3.0), (float) (0.5 * random.NextDouble() - 0.25));
      }
    }
    goal.Reset();
  }

  public override string CorrectRobotChoice(string requested_robot) => requested_robot == "Bot Royale" ? "FRC shooter" : requested_robot;

  public override void SendServerData(Dictionary<string, string> serverFlags)
  {
    base.SendServerData(serverFlags);
    Dictionary<string, string> dictionary1 = serverFlags;
    int cwStateMachine = (int) this.cw_red_wheel.cw_stateMachine;
    string str1 = cwStateMachine.ToString();
    cwStateMachine = (int) this.cw_blue_wheel.cw_stateMachine;
    string str2 = cwStateMachine.ToString();
    string str3 = str1 + ":" + str2;
    dictionary1["CW_SM"] = str3;
    Dictionary<string, string> dictionary2 = serverFlags;
    int cwBallcount = this.cw_red_wheel.cw_ballcount;
    string str4 = cwBallcount.ToString();
    cwBallcount = this.cw_blue_wheel.cw_ballcount;
    string str5 = cwBallcount.ToString();
    string str6 = str4 + ":" + str5;
    dictionary2["CW_BC"] = str6;
    serverFlags["CW_TL"] = (this.cw_red_wheel.cw_trench_light ? "1" : "0") + ":" + (this.cw_red_wheel.cw_flash_lights ? "1" : "0") + ":" + (this.cw_blue_wheel.cw_trench_light ? "1" : "0") + ":" + (this.cw_blue_wheel.cw_flash_lights ? "1" : "0");
    serverFlags["CW_TC"] = this.cw_red_wheel.cw_stage3_color.ToString() + ":" + this.cw_blue_wheel.cw_stage3_color.ToString();
    serverFlags["FOUL"] = this.foul_string;
    this.foul_string = "";
    string str7 = "";
    foreach (Scorekeeper_InfiniteRecharge.RobotStates robotstate in this.robotstates)
    {
      foreach (Scorekeeper_InfiniteRecharge.PowerUpData mydeffensivePowerup in robotstate.mydeffensive_powerups)
        str7 = str7 + ":" + (object) robotstate.robotID.id + "," + (object) (int) mydeffensivePowerup.mypowerup + "," + mydeffensivePowerup.GetRemainingTime().ToString();
      foreach (Scorekeeper_InfiniteRecharge.PowerUpData myoffensivePowerup in robotstate.myoffensive_powerups)
        str7 = str7 + ":" + (object) robotstate.robotID.id + "," + (object) (int) myoffensivePowerup.mypowerup + "," + myoffensivePowerup.GetRemainingTime().ToString();
    }
    serverFlags["PU"] = str7;
    int power;
    string str8;
    if (!this.powerup_center.IsDisabled())
    {
      power = (int) this.powerup_center.myPower;
      str8 = power.ToString();
    }
    else
      str8 = "0";
    string str9 = str8;
    for (int index = 0; index < this.powerup_home.Count; ++index)
    {
      string str10 = str9;
      string str11;
      if (!this.powerup_home[index].IsDisabled())
      {
        power = (int) this.powerup_home[index].myPower;
        str11 = power.ToString();
      }
      else
        str11 = "0";
      str9 = str10 + ":" + str11;
    }
    serverFlags["PUcore"] = str9;
  }

  private void ResetPowerUps()
  {
    for (int index = 0; index < 5; ++index)
      (index == 0 ? this.powerup_center : this.powerup_home[index - 1]).ClearOwner();
  }

  public override void ReceiveServerData(Dictionary<string, string> serverFlags)
  {
    base.ReceiveServerData(serverFlags);
    if (serverFlags.ContainsKey("CW_SM"))
    {
      string[] strArray = serverFlags["CW_SM"].Split(':');
      if (strArray.Length >= 2)
      {
        this.cw_red_wheel.cw_stateMachine = (Scorekeeper_InfiniteRecharge.WheelStates) Enum.ToObject(typeof (Scorekeeper_InfiniteRecharge.WheelStates), int.Parse(strArray[0]));
        this.cw_blue_wheel.cw_stateMachine = (Scorekeeper_InfiniteRecharge.WheelStates) Enum.ToObject(typeof (Scorekeeper_InfiniteRecharge.WheelStates), int.Parse(strArray[1]));
      }
    }
    if (serverFlags.ContainsKey("CW_BC"))
    {
      string[] strArray = serverFlags["CW_BC"].Split(':');
      if (strArray.Length >= 2)
      {
        this.cw_red_wheel.cw_ballcount = int.Parse(strArray[0]);
        this.cw_blue_wheel.cw_ballcount = int.Parse(strArray[1]);
      }
    }
    if (serverFlags.ContainsKey("CW_TL"))
    {
      string[] strArray = serverFlags["CW_TL"].Split(':');
      if (strArray.Length >= 4)
      {
        this.cw_red_wheel.cw_trench_light = strArray[0][0] == '1';
        this.cw_red_wheel.cw_flash_lights = strArray[1][0] == '1';
        this.cw_blue_wheel.cw_trench_light = strArray[2][0] == '1';
        this.cw_blue_wheel.cw_flash_lights = strArray[3][0] == '1';
      }
    }
    if (serverFlags.ContainsKey("CW_TC"))
    {
      string[] strArray = serverFlags["CW_TC"].Split(':');
      if (strArray.Length >= 2)
      {
        this.cw_red_wheel.cw_stage3_color = int.Parse(strArray[0]);
        this.cw_blue_wheel.cw_stage3_color = int.Parse(strArray[1]);
      }
    }
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
    if (serverFlags.ContainsKey("PU"))
    {
      foreach (Scorekeeper_InfiniteRecharge.RobotStates robotstate in this.robotstates)
      {
        robotstate.mydeffensive_powerups.Clear();
        robotstate.myoffensive_powerups.Clear();
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
            Scorekeeper_InfiniteRecharge.PowerUpData powerUpData = new Scorekeeper_InfiniteRecharge.PowerUpData();
            powerUpData.mypowerup = (PowerUpType) num1;
            powerUpData.duration = (long) num2;
            powerUpData.start_time = MyUtils.GetTimeMillisSinceStart();
            Scorekeeper_InfiniteRecharge.RobotStates robotStateById = this.GetRobotStateById(id);
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
    public bool immune_to_G10;
    public GenericFieldTracker clear_zone;
    public bool immune_to_G11;
    public long G10_G11_starttime = -1;
    public List<Scorekeeper_InfiniteRecharge.PowerUpData> myoffensive_powerups = new List<Scorekeeper_InfiniteRecharge.PowerUpData>();
    public List<Scorekeeper_InfiniteRecharge.PowerUpData> mydeffensive_powerups = new List<Scorekeeper_InfiniteRecharge.PowerUpData>();

    public void Reset()
    {
      this.counting_down = false;
      this.counting_up = false;
      this.start_time = -1f;
      this.isblocking = false;
      this.immune_to_G10 = false;
      this.clear_zone = (GenericFieldTracker) null;
      this.G10_G11_starttime = -1L;
      this.myoffensive_powerups.Clear();
      this.mydeffensive_powerups.Clear();
    }
  }

  public enum WheelStates
  {
    RESET,
    READY,
    STAGE1_ARMED,
    STAGE1_COMPLETE,
    STAGE2_ARMED,
    STAGE2_ROTATED,
    STAGE2_COMPLETE,
    STAGE3_ARMED,
    STAGE3_ALIGNED,
    STAGE3_COMPLETE,
  }

  public class WheelStruct
  {
    public Scorekeeper_InfiniteRecharge.WheelStates cw_stateMachine;
    public int cw_ballcount;
    public int cw_lastcolor;
    public int cw_currcolor;
    public int cw_rotation;
    public long cw_waitstart = -1;
    public bool cw_flash_lights;
    public bool cw_trench_light;
    public int cw_stage3_color;
    public HingeJoint controlwheel;
  }
}
