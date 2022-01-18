// Decompiled with JetBrains decompiler
// Type: Scorekeeper_FreightFrenzy
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class Scorekeeper_FreightFrenzy : Scorekeeper
{
  private FreightFrenzy_Settings ff_settings;
  public Transform ducks_red;
  public Transform ducks_blue;
  public Transform duck_red_pos;
  private int red_ducks_left;
  public Transform duck_blue_pos;
  private int blue_ducks_left;
  public Rigidbody platform_red;
  public Rigidbody platform_blue;
  public GameElementCounter carousel_red_detect;
  public GameElementCounter carousel_blue_detect;
  public DuckChecker duck_scorer_red;
  public DuckChecker duck_scorer_blue;
  public GenericFieldTracker red_storage;
  public GenericFieldTracker red_partial_storage;
  public GenericFieldTracker blue_storage;
  public GenericFieldTracker blue_partial_storage;
  public GenericFieldTracker red_warehouse;
  public GenericFieldTracker red_partial_warehouse;
  public GenericFieldTracker blue_warehouse;
  public GenericFieldTracker blue_partial_warehouse;
  public GenericFieldTracker red_hub_low;
  public GenericFieldTracker red_hub_mid;
  public GenericFieldTracker red_hub_high;
  public GenericFieldTracker red_hub_common;
  public GenericFieldTracker blue_hub_low;
  public GenericFieldTracker blue_hub_mid;
  public GenericFieldTracker blue_hub_high;
  public GenericFieldTracker blue_hub_common;
  public GenericFieldTracker red_hub_balance;
  public RobotCollision red_hub_robotcollision;
  public GenericFieldTracker blue_hub_balance;
  public RobotCollision blue_hub_robotcollision;
  public GenericFieldTracker red_shared_balance;
  public GenericFieldTracker blue_shared_balance;
  public RobotCollision shared_robotcollision;
  public MeshRenderer shared_hub_pole;
  public MeshRenderer red_hub_pole;
  public MeshRenderer blue_hub_pole;
  public Material red_glow;
  public Material blue_glow;
  public Material no_glow;
  public Material red_no_glow;
  public Material blue_no_glow;
  public Transform barcode_red;
  public Transform barcode_blue;
  public GameObject pushback;
  public Transform preload_redl;
  public Transform preload_redr;
  public Transform preload_bluel;
  public Transform preload_bluer;
  private double score_red;
  private int penalties_red;
  private int penalties_blue;
  private double score_blue;
  private Vector3 old_gravity;
  private GameObject fault_prefab;
  private Dictionary<int, gameElement> found_elements = new Dictionary<int, gameElement>();
  private bool game_running;
  private Dictionary<string, string> details = new Dictionary<string, string>();
  private int red_resets;
  private int blue_resets;
  private int auto_red_score;
  private int auto_blue_score;
  private int teleop_red_score;
  private int teleop_blue_score;
  private int endgame_red_score;
  private int endgame_blue_score;
  private bool auto_red_delivered;
  private bool auto_blue_delivered;
  private int red_ducks_delivered;
  private int blue_ducks_delivered;
  private int red_fully_sparked;
  private int blue_fully_sparked;
  private int red_part_sparked;
  private int blue_part_sparked;
  private int red_fully_wparked;
  private int blue_fully_wparked;
  private int red_part_wparked;
  private int blue_part_wparked;
  private int red_auto_storage;
  private int blue_auto_storage;
  private int red_auto_hub;
  private int blue_auto_hub;
  private int red_auto_bonus;
  private int blue_auto_bonus;
  private int red_tele_storage;
  private int blue_tele_storage;
  private int red_hub_1;
  private int blue_hub_1;
  private int red_hub_2;
  private int blue_hub_2;
  private int red_hub_3;
  private int blue_hub_3;
  private int red_shared;
  private int blue_shared;
  private bool red_balance;
  private bool blue_balance;
  private int red_end_fully_wparked;
  private int blue_end_fully_wparked;
  private int red_end_part_wparked;
  private int blue_end_part_wparked;
  private bool red_sbalance;
  private bool blue_sbalance;
  private int red_duck_lockout;
  private int blue_duck_lockout;
  private int preload_delay;
  private Vector3 delta_preload = new Vector3(0.0f, 0.0f, 0.0f);
  private int auto_roll = 1;
  private string foul_string = "";

  private void Awake()
  {
    GLOBALS.PlayerCount = 4;
    GLOBALS.TIMER_TOTAL = 150;
    GLOBALS.TIMER_AUTO = 30;
    GLOBALS.TIMER_ENDGAME = 30;
    this.old_gravity = Physics.gravity;
    Physics.gravity = new Vector3(0.0f, -19.62f, 0.0f);
  }

  private void OnDestroy() => Physics.gravity = this.old_gravity;

  public override void ScorerInit()
  {
    this.ScorerReset();
    this.ff_settings = GameObject.Find("GameSettings").GetComponent<FreightFrenzy_Settings>();
    this.fault_prefab = Resources.Load("Prefabs/FaultAnimation") as GameObject;
  }

  public override void ScorerReset()
  {
    base.ScorerReset();
    this.score_red = 0.0;
    this.score_blue = 0.0;
    this.penalties_red = 0;
    this.penalties_blue = 0;
    this.red_resets = 0;
    this.blue_resets = 0;
    this.auto_red_score = 0;
    this.auto_blue_score = 0;
    this.red_ducks_delivered = 0;
    this.blue_ducks_delivered = 0;
    this.auto_red_delivered = false;
    this.auto_blue_delivered = false;
    this.teleop_red_score = 0;
    this.teleop_blue_score = 0;
    this.endgame_red_score = 0;
    this.endgame_blue_score = 0;
    this.red_fully_sparked = 0;
    this.blue_fully_sparked = 0;
    this.red_part_sparked = 0;
    this.blue_part_sparked = 0;
    this.red_fully_wparked = 0;
    this.blue_fully_wparked = 0;
    this.red_part_wparked = 0;
    this.blue_part_wparked = 0;
    this.red_auto_storage = 0;
    this.blue_auto_storage = 0;
    this.red_auto_hub = 0;
    this.blue_auto_hub = 0;
    this.red_auto_bonus = 0;
    this.blue_auto_bonus = 0;
    this.red_tele_storage = 0;
    this.blue_tele_storage = 0;
    this.red_hub_1 = 0;
    this.blue_hub_1 = 0;
    this.red_hub_2 = 0;
    this.blue_hub_2 = 0;
    this.red_hub_3 = 0;
    this.blue_hub_3 = 0;
    this.red_shared = 0;
    this.blue_shared = 0;
    this.red_balance = false;
    this.blue_balance = false;
    this.red_end_fully_wparked = 0;
    this.blue_end_fully_wparked = 0;
    this.red_end_part_wparked = 0;
    this.blue_end_part_wparked = 0;
    this.red_sbalance = false;
    this.blue_sbalance = false;
    this.red_ducks_left = this.ducks_red.childCount;
    this.blue_ducks_left = this.ducks_blue.childCount;
    foreach (DuckStates duckStates in UnityEngine.Object.FindObjectsOfType<DuckStates>())
    {
      if (duckStates.mystate != DuckStates.DuckPositions.NonCarousel)
        duckStates.mystate = DuckStates.DuckPositions.Placed;
    }
    this.duck_scorer_blue.Reset();
    this.duck_scorer_red.Reset();
    foreach (gameElement gameElement in UnityEngine.Object.FindObjectsOfType<gameElement>())
    {
      if (gameElement.note2 == "out")
        gameElement.note2 = "in";
    }
  }

  public override void Restart() => base.Restart();

  public override void PlayerReset(int id, bool referee = false)
  {
    base.PlayerReset(id);
    if (!this.game_running || !this.allRobotID.ContainsKey(id))
      return;
    if (!referee)
    {
      if (this.allRobotID[id].is_red)
        ++this.red_resets;
      else
        ++this.blue_resets;
    }
    else if (this.allRobotID[id].is_red)
      this.penalties_red += this.ff_settings.REF_RESET_PENALTY;
    else
      this.penalties_blue += this.ff_settings.REF_RESET_PENALTY;
  }

  public override void ScorerUpdate()
  {
    this.DoAnimations();
    if (GLOBALS.CLIENT_MODE)
      return;
    this.game_running = true;
    if (this.timerstate != Scorekeeper.TimerState.PAUSED && this.timerstate != Scorekeeper.TimerState.RUNNING)
    {
      this.game_running = false;
      this.pushback.SetActive(false);
    }
    else if (this.time_total.TotalSeconds > (double) (GLOBALS.TIMER_TOTAL - GLOBALS.TIMER_AUTO))
      this.pushback.SetActive(true);
    else
      this.pushback.SetActive(false);
    if (this.preload_delay > 0)
    {
      --this.preload_delay;
      if (this.preload_delay == 0)
        this.PreloadAll();
    }
    this.CalculateScores();
  }

  public override void FieldChangedTrigger()
  {
  }

  public string GetDetails(bool red = true)
  {
    this.GetScoreDetails(this.details);
    if (GLOBALS.CLIENT_MODE)
    {
      foreach (string key in MyUtils.score_details.Keys)
        this.details[key] = MyUtils.score_details[key];
    }
    string str = red ? "_R" : "_B";
    return "<B>AUTO Score   = </B> " + this.details["Auto" + str] + "\n    Delivered Duck: " + this.details["AutoDel" + str] + "\n    # Bots In Storage: " + this.details["AutoBotStore" + str] + "\n    # Bots Completely In Storage: " + this.details["AutoBotCStore" + str] + "\n    # Bots In Warehouse: " + this.details["AutoBotWare" + str] + "\n    # Bots Completely In Warehouse: " + this.details["AutoBotCWare" + str] + "\n    # Freight In Storage: " + this.details["AutoFStore" + str] + "\n    # Freight On Team Hub: " + this.details["AutoFHub" + str] + "\n    # Auto Bonus: " + this.details["AutoBonus" + str] + "\n\n<B>TELEOP Score =</B> " + this.details["Tele" + str] + "\n    # in Storage: " + this.details["TeleFStore" + str] + "\n    # on Level 1: " + this.details["TeleHub1" + str] + "\n    # on Level 2: " + this.details["TeleHub2" + str] + "\n    # on Level 3: " + this.details["TeleHub3" + str] + "\n    # on Shared:  " + this.details["TeleHubS" + str] + "\n\n<B>ENDGAME Score=</B> " + this.details["End" + str] + "\n    # Ducks Delivered: " + this.details["EndDel" + str] + "\n    Team Hub Balanced: " + this.details["EndBal" + str] + "\n    Shared Hub Unbalanced: " + this.details["EndSBal" + str] + "\n    # Bots In Warehouse: " + this.details["EndBotWare" + str] + "\n    # Bots Completely In Warehouse: " + this.details["EndBotCWare" + str] + "\n\n<B>Resets = </B> " + this.details["Resets" + str] + "\n<B>PENALTIES = </B> " + this.details["Pen" + str];
  }

  public override void GetScoreDetails(Dictionary<string, string> data)
  {
    base.GetScoreDetails(data);
    data["Pen_B"] = this.penalties_blue.ToString();
    data["Pen_R"] = this.penalties_red.ToString();
    data["Score_R"] = ((int) this.score_red + this.score_redadj).ToString();
    data["Score_B"] = ((int) this.score_blue + this.score_blueadj).ToString();
    data["Resets_B"] = this.red_resets.ToString();
    data["Resets_R"] = this.blue_resets.ToString();
    data["Auto_R"] = this.auto_red_score.ToString();
    data["Auto_B"] = this.auto_blue_score.ToString();
    data["AutoDel_R"] = this.auto_red_delivered ? "Y" : "N";
    data["AutoDel_B"] = this.auto_blue_delivered ? "Y" : "N";
    data["AutoBotStore_R"] = this.red_part_sparked.ToString();
    data["AutoBotCStore_R"] = this.red_fully_sparked.ToString();
    data["AutoBotStore_B"] = this.blue_part_sparked.ToString();
    Dictionary<string, string> dictionary1 = data;
    int num = this.blue_fully_sparked;
    string str1 = num.ToString();
    dictionary1["AutoBotCStore_B"] = str1;
    Dictionary<string, string> dictionary2 = data;
    num = this.red_part_wparked;
    string str2 = num.ToString();
    dictionary2["AutoBotWare_R"] = str2;
    Dictionary<string, string> dictionary3 = data;
    num = this.red_fully_wparked;
    string str3 = num.ToString();
    dictionary3["AutoBotCWare_R"] = str3;
    Dictionary<string, string> dictionary4 = data;
    num = this.blue_part_wparked;
    string str4 = num.ToString();
    dictionary4["AutoBotWare_B"] = str4;
    Dictionary<string, string> dictionary5 = data;
    num = this.blue_fully_wparked;
    string str5 = num.ToString();
    dictionary5["AutoBotCWare_B"] = str5;
    Dictionary<string, string> dictionary6 = data;
    num = this.red_auto_storage;
    string str6 = num.ToString();
    dictionary6["AutoFStore_R"] = str6;
    Dictionary<string, string> dictionary7 = data;
    num = this.blue_auto_storage;
    string str7 = num.ToString();
    dictionary7["AutoFStore_B"] = str7;
    Dictionary<string, string> dictionary8 = data;
    num = this.red_auto_hub;
    string str8 = num.ToString();
    dictionary8["AutoFHub_R"] = str8;
    Dictionary<string, string> dictionary9 = data;
    num = this.blue_auto_hub;
    string str9 = num.ToString();
    dictionary9["AutoFHub_B"] = str9;
    Dictionary<string, string> dictionary10 = data;
    num = this.red_auto_bonus;
    string str10 = num.ToString();
    dictionary10["AutoBonus_R"] = str10;
    Dictionary<string, string> dictionary11 = data;
    num = this.blue_auto_bonus;
    string str11 = num.ToString();
    dictionary11["AutoBonus_B"] = str11;
    Dictionary<string, string> dictionary12 = data;
    num = this.teleop_red_score;
    string str12 = num.ToString();
    dictionary12["Tele_R"] = str12;
    Dictionary<string, string> dictionary13 = data;
    num = this.teleop_blue_score;
    string str13 = num.ToString();
    dictionary13["Tele_B"] = str13;
    Dictionary<string, string> dictionary14 = data;
    num = this.red_tele_storage;
    string str14 = num.ToString();
    dictionary14["TeleFStore_R"] = str14;
    Dictionary<string, string> dictionary15 = data;
    num = this.blue_tele_storage;
    string str15 = num.ToString();
    dictionary15["TeleFStore_B"] = str15;
    Dictionary<string, string> dictionary16 = data;
    num = this.red_hub_1;
    string str16 = num.ToString();
    dictionary16["TeleHub1_R"] = str16;
    Dictionary<string, string> dictionary17 = data;
    num = this.blue_hub_1;
    string str17 = num.ToString();
    dictionary17["TeleHub1_B"] = str17;
    Dictionary<string, string> dictionary18 = data;
    num = this.red_hub_2;
    string str18 = num.ToString();
    dictionary18["TeleHub2_R"] = str18;
    Dictionary<string, string> dictionary19 = data;
    num = this.blue_hub_2;
    string str19 = num.ToString();
    dictionary19["TeleHub2_B"] = str19;
    Dictionary<string, string> dictionary20 = data;
    num = this.red_hub_3;
    string str20 = num.ToString();
    dictionary20["TeleHub3_R"] = str20;
    Dictionary<string, string> dictionary21 = data;
    num = this.blue_hub_3;
    string str21 = num.ToString();
    dictionary21["TeleHub3_B"] = str21;
    Dictionary<string, string> dictionary22 = data;
    num = this.red_shared;
    string str22 = num.ToString();
    dictionary22["TeleHubS_R"] = str22;
    Dictionary<string, string> dictionary23 = data;
    num = this.blue_shared;
    string str23 = num.ToString();
    dictionary23["TeleHubS_B"] = str23;
    Dictionary<string, string> dictionary24 = data;
    num = this.endgame_red_score;
    string str24 = num.ToString();
    dictionary24["End_R"] = str24;
    Dictionary<string, string> dictionary25 = data;
    num = this.endgame_blue_score;
    string str25 = num.ToString();
    dictionary25["End_B"] = str25;
    data["EndBal_R"] = this.red_balance ? "Y" : "N";
    data["EndBal_B"] = this.blue_balance ? "Y" : "N";
    Dictionary<string, string> dictionary26 = data;
    num = this.red_end_part_wparked;
    string str26 = num.ToString();
    dictionary26["EndBotWare_R"] = str26;
    Dictionary<string, string> dictionary27 = data;
    num = this.red_end_fully_wparked;
    string str27 = num.ToString();
    dictionary27["EndBotCWare_R"] = str27;
    Dictionary<string, string> dictionary28 = data;
    num = this.blue_end_part_wparked;
    string str28 = num.ToString();
    dictionary28["EndBotWare_B"] = str28;
    Dictionary<string, string> dictionary29 = data;
    num = this.blue_end_fully_wparked;
    string str29 = num.ToString();
    dictionary29["EndBotCWare_B"] = str29;
    data["EndSBal_R"] = this.red_sbalance ? "Y" : "N";
    data["EndSBal_B"] = this.blue_sbalance ? "Y" : "N";
    Dictionary<string, string> dictionary30 = data;
    num = this.red_ducks_delivered;
    string str30 = num.ToString();
    dictionary30["EndDel_R"] = str30;
    Dictionary<string, string> dictionary31 = data;
    num = this.blue_ducks_delivered;
    string str31 = num.ToString();
    dictionary31["EndDel_B"] = str31;
  }

  private void CalculateScores()
  {
    if (this.timerstate != Scorekeeper.TimerState.RUNNING)
      return;
    if (this.time_total.TotalSeconds > (double) (GLOBALS.TIMER_TOTAL - GLOBALS.TIMER_AUTO))
    {
      if (this.duck_scorer_red.processed_counter > 0)
        this.auto_red_delivered = true;
      if (this.duck_scorer_blue.processed_counter > 0)
        this.auto_blue_delivered = true;
      this.red_fully_sparked = 0;
      this.blue_fully_sparked = 0;
      this.red_part_sparked = 0;
      this.blue_part_sparked = 0;
      foreach (RobotID robot in this.allRobotID.Values)
      {
        if (robot.is_red)
        {
          if (this.red_storage.IsAnyRobotInside() && this.red_storage.IsRobotInside(robot))
          {
            if (this.red_partial_storage.IsRobotInside(robot))
              ++this.red_part_sparked;
            else
              ++this.red_fully_sparked;
          }
        }
        else if (this.blue_storage.IsAnyRobotInside() && this.blue_storage.IsRobotInside(robot))
        {
          if (this.blue_partial_storage.IsRobotInside(robot))
            ++this.blue_part_sparked;
          else
            ++this.blue_fully_sparked;
        }
      }
      this.red_fully_wparked = 0;
      this.blue_fully_wparked = 0;
      this.red_part_wparked = 0;
      this.blue_part_wparked = 0;
      foreach (RobotID robot in this.allRobotID.Values)
      {
        if (robot.is_red)
        {
          if (this.red_warehouse.IsAnyRobotInside() && this.red_warehouse.IsRobotInside(robot))
          {
            if (this.red_partial_warehouse.IsRobotInside(robot))
              ++this.red_part_wparked;
            else
              ++this.red_fully_wparked;
          }
        }
        else if (this.blue_warehouse.IsAnyRobotInside() && this.blue_warehouse.IsRobotInside(robot))
        {
          if (this.blue_partial_warehouse.IsRobotInside(robot))
            ++this.blue_part_wparked;
          else
            ++this.blue_fully_wparked;
        }
      }
      this.red_auto_storage = this.GetValidFreight(this.red_storage, this.red_partial_storage);
      this.blue_auto_storage = this.GetValidFreight(this.blue_storage, this.blue_partial_storage);
      this.red_auto_hub = this.GetValidFreight(this.red_hub_low) + this.GetValidFreight(this.red_hub_mid) + this.GetValidFreight(this.red_hub_high);
      this.blue_auto_hub = this.GetValidFreight(this.blue_hub_low) + this.GetValidFreight(this.blue_hub_mid) + this.GetValidFreight(this.blue_hub_high);
      this.red_auto_bonus = 0;
      this.blue_auto_bonus = 0;
      switch (this.auto_roll)
      {
        case 1:
          this.red_auto_bonus = this.GetPreloadBonus(this.red_hub_low, true);
          this.blue_auto_bonus = this.GetPreloadBonus(this.blue_hub_low, false);
          break;
        case 2:
          this.red_auto_bonus = this.GetPreloadBonus(this.red_hub_mid, true);
          this.blue_auto_bonus = this.GetPreloadBonus(this.blue_hub_mid, false);
          break;
        case 3:
          this.red_auto_bonus = this.GetPreloadBonus(this.red_hub_high, true);
          this.blue_auto_bonus = this.GetPreloadBonus(this.blue_hub_high, false);
          break;
      }
      this.auto_red_score = (this.auto_red_delivered ? 10 : 0) + this.red_fully_sparked * 6 + this.red_part_sparked * 3 + this.red_fully_wparked * 10 + this.red_part_wparked * 5 + this.red_auto_storage * 2 + this.red_auto_hub * 6 + this.red_auto_bonus * 10;
      this.auto_blue_score = (this.auto_blue_delivered ? 10 : 0) + this.blue_fully_sparked * 6 + this.blue_part_sparked * 3 + this.blue_fully_wparked * 10 + this.blue_part_wparked * 5 + this.blue_auto_storage * 2 + this.blue_auto_hub * 6 + this.blue_auto_bonus * 10;
    }
    if (this.time_total.TotalSeconds <= (double) (GLOBALS.TIMER_TOTAL - GLOBALS.TIMER_AUTO))
    {
      this.red_tele_storage = this.GetValidFreight(this.red_storage, this.red_partial_storage);
      this.blue_tele_storage = this.GetValidFreight(this.blue_storage, this.blue_partial_storage);
      this.red_hub_1 = this.GetValidFreight(this.red_hub_low);
      this.red_hub_2 = this.GetValidFreight(this.red_hub_mid);
      this.red_hub_3 = this.GetValidFreight(this.red_hub_high);
      this.red_shared = this.GetValidFreight(this.red_hub_common);
      this.blue_hub_1 = this.GetValidFreight(this.blue_hub_low);
      this.blue_hub_2 = this.GetValidFreight(this.blue_hub_mid);
      this.blue_hub_3 = this.GetValidFreight(this.blue_hub_high);
      this.blue_shared = this.GetValidFreight(this.blue_hub_common);
      this.teleop_red_score = this.red_tele_storage + 2 * this.red_hub_1 + 4 * this.red_hub_2 + 6 * this.red_hub_3 + 4 * this.red_shared;
      this.teleop_blue_score = this.blue_tele_storage + 2 * this.blue_hub_1 + 4 * this.blue_hub_2 + 6 * this.blue_hub_3 + 4 * this.blue_shared;
    }
    if (this.time_total.TotalSeconds > (double) GLOBALS.TIMER_ENDGAME)
    {
      if (this.duck_scorer_red.processed_counter > 0)
        this.duck_scorer_red.Reset();
      if (this.duck_scorer_blue.processed_counter > 0)
        this.duck_scorer_blue.Reset();
    }
    this.red_balance = !this.red_hub_balance.IsAnyGameElementInside() && !this.red_hub_balance.IsAnyRobotInside() && this.red_hub_robotcollision.GetRobotCount() <= 0;
    this.blue_balance = !this.blue_hub_balance.IsAnyGameElementInside() && !this.blue_hub_balance.IsAnyRobotInside() && this.blue_hub_robotcollision.GetRobotCount() <= 0;
    foreach (RobotID robot in this.allRobotID.Values)
    {
      if (robot.is_red)
      {
        if (this.blue_hub_balance.IsAnyRobotInside() && this.blue_hub_balance.IsRobotInside(robot) || this.blue_hub_robotcollision.GetRobotCount() > 0 && this.blue_hub_robotcollision.IsRobotInside(robot))
          this.blue_balance = true;
      }
      else if (this.red_hub_balance.IsAnyRobotInside() && this.red_hub_balance.IsRobotInside(robot) || this.red_hub_robotcollision.GetRobotCount() > 0 && this.red_hub_robotcollision.IsRobotInside(robot))
        this.red_balance = true;
    }
    this.red_sbalance = false;
    this.blue_sbalance = false;
    if (!this.red_shared_balance.IsAnyRobotInside() && this.shared_robotcollision.GetRobotCount() <= 0 && this.red_shared_balance.GetGameElementCount() == 1 && this.red_shared_balance.game_elements[0].note == "FLOOR")
      this.red_sbalance = true;
    if (!this.blue_shared_balance.IsAnyRobotInside() && this.shared_robotcollision.GetRobotCount() <= 0 && this.blue_shared_balance.GetGameElementCount() == 1 && this.blue_shared_balance.game_elements[0].note == "FLOOR")
      this.blue_sbalance = true;
    if (this.red_sbalance && this.blue_sbalance)
    {
      this.red_sbalance = false;
      this.blue_sbalance = false;
    }
    bool flag1 = false;
    bool flag2 = false;
    foreach (RobotID robot in this.allRobotID.Values)
    {
      bool flag3 = false;
      if (this.shared_robotcollision.GetRobotCount() > 0 && this.shared_robotcollision.IsRobotInside(robot))
        flag3 = true;
      else if (this.red_shared_balance.IsAnyRobotInside() && this.red_shared_balance.IsRobotInside(robot))
        flag3 = true;
      else if (this.blue_shared_balance.IsAnyRobotInside() && this.blue_shared_balance.IsRobotInside(robot))
        flag3 = true;
      if (flag3 && robot.is_red)
        flag2 = true;
      if (flag3 && !robot.is_red)
        flag1 = true;
    }
    if (flag1 && !flag2)
      this.red_sbalance = true;
    if (!flag1 & flag2)
      this.blue_sbalance = true;
    this.endgame_red_score = 0;
    this.endgame_blue_score = 0;
    if (this.time_total.TotalSeconds <= (double) GLOBALS.TIMER_ENDGAME)
    {
      this.AddDucks();
      this.red_ducks_delivered = this.duck_scorer_red.processed_counter;
      this.blue_ducks_delivered = this.duck_scorer_blue.processed_counter;
      this.red_end_fully_wparked = 0;
      this.blue_end_fully_wparked = 0;
      this.red_end_part_wparked = 0;
      this.blue_end_part_wparked = 0;
      foreach (RobotID robot in this.allRobotID.Values)
      {
        bool flag4 = false;
        bool flag5 = false;
        if (this.red_warehouse.IsAnyRobotInside() && this.red_warehouse.IsRobotInside(robot))
        {
          if (this.red_partial_warehouse.IsRobotInside(robot))
            flag4 = true;
          else
            flag5 = true;
        }
        if (this.blue_warehouse.IsAnyRobotInside() && this.blue_warehouse.IsRobotInside(robot))
        {
          if (this.blue_partial_warehouse.IsRobotInside(robot))
            flag4 = true;
          else
            flag5 = true;
        }
        if (robot.is_red)
        {
          this.red_end_part_wparked += flag4 ? 1 : 0;
          this.red_end_fully_wparked += flag5 ? 1 : 0;
        }
        else
        {
          this.blue_end_part_wparked += flag4 ? 1 : 0;
          this.blue_end_fully_wparked += flag5 ? 1 : 0;
        }
      }
      this.endgame_red_score = this.red_ducks_delivered * 6 + (this.red_balance ? 10 : 0) + this.red_end_part_wparked * 3 + this.red_end_fully_wparked * 6 + (this.red_sbalance ? 20 : 0);
      this.endgame_blue_score = this.blue_ducks_delivered * 6 + (this.blue_balance ? 10 : 0) + this.blue_end_part_wparked * 3 + this.blue_end_fully_wparked * 6 + (this.blue_sbalance ? 20 : 0);
    }
    if (this.ff_settings.ENABLE_POSSESSION_LIMIT)
    {
      foreach (RobotID robotId in this.allRobotID.Values)
      {
        if ((bool) (UnityEngine.Object) robotId)
        {
          PossessionDetect component = robotId.GetComponent<PossessionDetect>();
          if ((bool) (UnityEngine.Object) component && (double) component.GetFaultDuration() > 1.0)
          {
            if (robotId.is_red)
              this.penalties_red += this.ff_settings.PENALTIES_MINOR;
            else
              this.penalties_blue += this.ff_settings.PENALTIES_MINOR;
            this.CreateFoul(robotId.id);
            component.ResetFault(4f);
          }
        }
      }
    }
    this.score_red = (double) (this.auto_red_score + this.teleop_red_score + this.endgame_red_score - this.penalties_red - this.red_resets * this.ff_settings.RESTART_POS_PENALTY);
    this.score_blue = (double) (this.auto_blue_score + this.teleop_blue_score + this.endgame_blue_score - this.penalties_blue - this.blue_resets * this.ff_settings.RESTART_POS_PENALTY);
  }

  public void RobotDroppedItem(int robotid, gameElement item)
  {
    if (!this.game_running || item.note2 != "in")
      return;
    if (this.red_warehouse.IsRobotInside(this.allRobotID[robotid]) || this.blue_warehouse.IsRobotInside(this.allRobotID[robotid]))
      this.GameElementLeftWarehouse(item);
    else
      item.note2 = "out";
  }

  public void GameElementLeftWarehouse(gameElement item, GenericFieldTracker warehouse = null)
  {
    if (!this.game_running)
      return;
    if ((UnityEngine.Object) warehouse == (UnityEngine.Object) null)
    {
      warehouse = item.tracker;
      if ((UnityEngine.Object) warehouse == (UnityEngine.Object) null)
        return;
    }
    Vector3 position = warehouse.transform.position with
    {
      y = UnityEngine.Random.Range(2.5f, 4f)
    };
    position.x += UnityEngine.Random.Range(-0.3f, 0.3f);
    position.z += UnityEngine.Random.Range(-0.5f, 0.3f);
    item.transform.position = position;
  }

  private void AddDucks()
  {
    if (this.red_ducks_left > 0 && this.carousel_red_detect.GetElementCount() == 0 && (double) this.platform_red.angularVelocity.magnitude < 0.00999999977648258 && this.red_duck_lockout <= 0)
    {
      --this.red_ducks_left;
      Transform child = this.ducks_red.GetChild(this.red_ducks_left);
      child.SetPositionAndRotation(this.duck_red_pos.position, this.duck_red_pos.rotation);
      child.GetComponent<DuckStates>().mystate = DuckStates.DuckPositions.Placed;
      this.red_duck_lockout = 10;
    }
    if (this.blue_ducks_left > 0 && this.carousel_blue_detect.GetElementCount() == 0 && (double) this.platform_blue.angularVelocity.magnitude < 0.00999999977648258 && this.blue_duck_lockout <= 0)
    {
      --this.blue_ducks_left;
      Transform child = this.ducks_blue.GetChild(this.blue_ducks_left);
      child.SetPositionAndRotation(this.duck_blue_pos.position, this.duck_blue_pos.rotation);
      child.GetComponent<DuckStates>().mystate = DuckStates.DuckPositions.Placed;
      this.blue_duck_lockout = 10;
    }
    if (this.red_duck_lockout > 0)
      --this.red_duck_lockout;
    if (this.blue_duck_lockout <= 0)
      return;
    --this.blue_duck_lockout;
  }

  public override int GetRedScore() => this.timerstate == Scorekeeper.TimerState.FINISHED ? this.score_redfinal : (int) this.score_red + this.score_redadj;

  public override int GetBlueScore() => this.timerstate == Scorekeeper.TimerState.FINISHED ? this.score_bluefinal : (int) this.score_blue + this.score_blueadj;

  private int GetValidFreight(GenericFieldTracker hub, GenericFieldTracker keepout = null)
  {
    int validFreight = 0;
    foreach (gameElement gameElement in hub.game_elements)
    {
      if ((!(bool) (UnityEngine.Object) keepout || !keepout.IsGameElementInside(gameElement)) && !(gameElement.note != "score"))
      {
        RobotCollision component = gameElement.GetComponent<RobotCollision>();
        if ((bool) (UnityEngine.Object) component && component.GetRobotCount() <= 0)
          ++validFreight;
      }
    }
    return validFreight;
  }

  private int GetPreloadBonus(GenericFieldTracker level, bool isred)
  {
    int preloadBonus = 0;
    foreach (gameElement gameElement in level.game_elements)
    {
      if (!(gameElement.note != "score"))
      {
        RobotCollision component1 = gameElement.GetComponent<RobotCollision>();
        if ((bool) (UnityEngine.Object) component1 && component1.GetRobotCount() <= 0)
        {
          PreloadMarker component2 = gameElement.GetComponent<PreloadMarker>();
          if ((bool) (UnityEngine.Object) component2 && (component2.red & isred || component2.blue && !isred))
            ++preloadBonus;
        }
      }
    }
    return preloadBonus;
  }

  private void PreloadAll()
  {
    foreach (RobotInterface3D allrobot in this.allrobots)
    {
      string startingPos = allrobot.myRobotID.starting_pos;
      if (!(startingPos == "Red Left"))
      {
        if (!(startingPos == "Red Right"))
        {
          if (!(startingPos == "Blue Left"))
          {
            if (startingPos == "Blue Right")
              this.PreloadItem(allrobot, this.preload_bluer);
          }
          else
            this.PreloadItem(allrobot, this.preload_bluel);
        }
        else
          this.PreloadItem(allrobot, this.preload_redr);
      }
      else
        this.PreloadItem(allrobot, this.preload_redl);
    }
  }

  private void PreloadItem(RobotInterface3D bot, Transform preload_items)
  {
    PreloadID componentInChildren = bot.GetComponentInChildren<PreloadID>();
    if (!(bool) (UnityEngine.Object) componentInChildren)
      return;
    float num = 0.0f;
    foreach (Transform preloadItem in preload_items)
    {
      preloadItem.SetPositionAndRotation(componentInChildren.transform.position + num * this.delta_preload.magnitude * componentInChildren.transform.up, componentInChildren.transform.rotation * preloadItem.rotation);
      ++num;
    }
  }

  public override void OnTimerStart()
  {
    this.preload_delay = 2;
    base.OnTimerStart();
    this.Restart();
    System.Random random = new System.Random();
    this.auto_roll = GLOBALS.game_option <= 1 ? random.Next(1, 4) : GLOBALS.game_option - 1;
    if (this.auto_roll > 3)
      this.auto_roll = 3;
    Vector3 position = this.barcode_red.position with
    {
      x = (float) (-1.0 * (double) (this.auto_roll - 1) * 0.41499999165535)
    };
    this.barcode_red.position = position;
    position = this.barcode_blue.position with
    {
      x = (float) (this.auto_roll - 1) * 0.415f
    };
    this.barcode_blue.position = position;
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
    if (!GLOBALS.SERVER_MODE)
      return;
    if (this.foul_string.Length > 0)
      this.foul_string += ":";
    this.foul_string += (string) (object) robotid;
  }

  public override void SendServerData(Dictionary<string, string> serverFlags)
  {
    base.SendServerData(serverFlags);
    serverFlags["FOUL"] = this.foul_string;
    this.foul_string = "";
    serverFlags["SHUB"] = "0";
    if (this.red_sbalance)
      serverFlags["SHUB"] = "1";
    if (this.blue_sbalance)
      serverFlags["SHUB"] = "2";
    serverFlags["RHUB"] = this.red_balance ? "1" : "0";
    serverFlags["BHUB"] = this.blue_balance ? "1" : "0";
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
    if (serverFlags.ContainsKey("SHUB"))
    {
      this.red_sbalance = false;
      this.blue_sbalance = false;
      if (serverFlags["SHUB"] == "1")
        this.red_sbalance = true;
      if (serverFlags["SHUB"] == "2")
        this.blue_sbalance = true;
    }
    if (serverFlags.ContainsKey("RHUB"))
      this.red_balance = serverFlags["RHUB"] == "1";
    if (!serverFlags.ContainsKey("BHUB"))
      return;
    this.blue_balance = serverFlags["BHUB"] == "1";
  }

  private void DoAnimations()
  {
    if (this.red_sbalance == this.blue_sbalance)
    {
      if (!this.shared_hub_pole.material.name.StartsWith(this.no_glow.name))
      {
        UnityEngine.Object.Destroy((UnityEngine.Object) this.shared_hub_pole.material);
        this.shared_hub_pole.material = this.no_glow;
      }
    }
    else if (this.red_sbalance)
    {
      if (!this.shared_hub_pole.material.name.StartsWith(this.red_glow.name))
      {
        UnityEngine.Object.Destroy((UnityEngine.Object) this.shared_hub_pole.material);
        this.shared_hub_pole.material = this.red_glow;
      }
    }
    else if (!this.shared_hub_pole.material.name.StartsWith(this.blue_glow.name))
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) this.shared_hub_pole.material);
      this.shared_hub_pole.material = this.blue_glow;
    }
    if (this.red_balance)
    {
      if (!this.red_hub_pole.material.name.StartsWith(this.red_glow.name))
      {
        UnityEngine.Object.Destroy((UnityEngine.Object) this.red_hub_pole.material);
        this.red_hub_pole.material = this.red_glow;
      }
    }
    else if (!this.red_hub_pole.material.name.StartsWith(this.red_no_glow.name))
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) this.red_hub_pole.material);
      this.red_hub_pole.material = this.red_no_glow;
    }
    if (this.blue_balance)
    {
      if (this.blue_hub_pole.material.name.StartsWith(this.blue_glow.name))
        return;
      UnityEngine.Object.Destroy((UnityEngine.Object) this.blue_hub_pole.material);
      this.blue_hub_pole.material = this.blue_glow;
    }
    else
    {
      if (this.blue_hub_pole.material.name.StartsWith(this.blue_no_glow.name))
        return;
      UnityEngine.Object.Destroy((UnityEngine.Object) this.blue_hub_pole.material);
      this.blue_hub_pole.material = this.blue_no_glow;
    }
  }
}
