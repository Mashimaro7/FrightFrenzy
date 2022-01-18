// Decompiled with JetBrains decompiler
// Type: InfiniteRecharge_Settings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfiniteRecharge_Settings : GameSettings
{
  public bool ENABLE_DEPOT_PENALTIES;
  public float RESET_DURATION_UNDER_SKYBRIDGE = 2f;
  public bool ENABLE_BLOCKING = true;
  public bool ENABLE_WALLBLOCKING;
  public float BLOCKING_DURATION = 5f;
  public float BLOCKING_RESET_HOLDING_TIME = 2f;
  public int PENALTY_BLOCKING = 15;
  public bool ENABLE_BALLRETURNTOBAY = true;
  public GameObject menu;
  public Transform Rule_Depots;
  public Transform Rule_Blocking;
  public Transform Rule_WallBlocking;
  public Transform Penalty_Blocking;
  public Transform Rule_G10G11;
  public Transform Penalty_G10G11;
  public Transform G10_clearance;
  public Transform G11_blanking;
  public Transform BallReturnToBay;
  public Transform ShieldOffset;
  public bool ENABLE_G10_G11 = true;
  public float G10_CLEAR_DISTANCE = 5f;
  public int G10_G11_BLANKING = 5000;
  public int PENALTY_G10_G11 = 25;
  public int SHIELD_OFFSET;
  [Header("Power Ups")]
  public Transform Enable_Powerups;
  public bool ENABLE_POWERUPS;
  public Transform PU_Onlyone;
  public bool PU_ONLYONE = true;
  public Transform PU_Speed;
  public bool PU_SPEED = true;
  public Transform PU_Torque;
  public bool PU_TORQUE = true;
  public Transform PU_Invisible;
  public bool PU_INVISIBLE = true;
  public Transform PU_Slow;
  public bool PU_SLOW = true;
  public Transform PU_Weak;
  public bool PU_WEAK = true;
  public Transform PU_Inverted;
  public bool PU_INVERTED = true;
  public Transform PU_Off_duration;
  public int PU_OFFENSIVE_DURATION = 14;
  public Transform PU_Def_duration;
  public int PU_DEFENSIVE_DURATION = 7;
  public Transform PU_Center;
  public bool PU_CENTER = true;
  public Transform PU_center_type;
  public int PU_CENTER_TYPE = 1;
  public Transform PU_home;
  public bool PU_HOME = true;
  public Transform PU_home_type;
  public int PU_HOME_TYPE;
  public Transform PU_Respawn_time;
  public int PU_RESPAWN = 15;
  public Transform PU_Strength;
  public int PU_STRENGTH = 100;
  [Header("Other")]
  public Transform Game_Version;
  public string GAMEVERSION = "2021";
  private string oldGAMEVERSION = "N/A";
  public TextMeshProUGUI red_details;
  public TextMeshProUGUI blue_details;
  private Scorekeeper_InfiniteRecharge ir_scorer;
  private bool init_done;
  private bool old_toggle_button;
  private float last_time_update;

  private void Awake()
  {
  }

  private void Start()
  {
    this.init_done = false;
    this.ir_scorer = GameObject.Find("Scorekeeper").GetComponent<Scorekeeper_InfiniteRecharge>();
    this.Rule_Depots.GetComponent<Toggle>().isOn = this.ENABLE_DEPOT_PENALTIES;
    this.Rule_Blocking.GetComponent<Toggle>().isOn = this.ENABLE_BLOCKING;
    this.Penalty_Blocking.GetComponent<InputField>().text = this.PENALTY_BLOCKING.ToString();
    this.BallReturnToBay.GetComponent<Toggle>().isOn = this.ENABLE_BALLRETURNTOBAY;
    this.Rule_G10G11.GetComponent<Toggle>().isOn = this.ENABLE_G10_G11;
    this.Penalty_G10G11.GetComponent<InputField>().text = this.PENALTY_G10_G11.ToString();
    this.G10_clearance.GetComponent<InputField>().text = this.G10_CLEAR_DISTANCE.ToString();
    this.G11_blanking.GetComponent<InputField>().text = this.G10_G11_BLANKING.ToString();
    this.ShieldOffset.GetComponent<InputField>().text = this.SHIELD_OFFSET.ToString();
    this.Enable_Powerups.GetComponent<Toggle>().isOn = this.ENABLE_POWERUPS;
    this.PU_Onlyone.GetComponent<Toggle>().isOn = this.PU_ONLYONE;
    this.PU_Speed.GetComponent<Toggle>().isOn = this.PU_SPEED;
    this.PU_Torque.GetComponent<Toggle>().isOn = this.PU_TORQUE;
    this.PU_Invisible.GetComponent<Toggle>().isOn = this.PU_INVISIBLE;
    this.PU_Slow.GetComponent<Toggle>().isOn = this.PU_SLOW;
    this.PU_Weak.GetComponent<Toggle>().isOn = this.PU_WEAK;
    this.PU_Inverted.GetComponent<Toggle>().isOn = this.PU_INVERTED;
    this.PU_Off_duration.GetComponent<InputField>().text = this.PU_OFFENSIVE_DURATION.ToString();
    this.PU_Def_duration.GetComponent<InputField>().text = this.PU_DEFENSIVE_DURATION.ToString();
    this.PU_Center.GetComponent<Toggle>().isOn = this.PU_CENTER;
    this.PU_center_type.GetComponent<Dropdown>().value = this.PU_CENTER_TYPE;
    this.PU_home.GetComponent<Toggle>().isOn = this.PU_HOME;
    this.PU_home_type.GetComponent<Dropdown>().value = this.PU_HOME_TYPE;
    this.PU_Respawn_time.GetComponent<InputField>().text = this.PU_RESPAWN.ToString();
    this.PU_Strength.GetComponent<InputField>().text = this.PU_STRENGTH.ToString();
    this.Game_Version.GetComponent<Dropdown>().value = this.Game_Version.GetComponent<Dropdown>().options.FindIndex((Predicate<Dropdown.OptionData>) (a => a.text == this.GAMEVERSION));
    GLOBALS.game_option = 2;
    this.init_done = true;
    this.Init();
  }

  public override void Init()
  {
    base.Init();
    this.UpdateField(true);
  }

  private void Update()
  {
    if (!GLOBALS.HEADLESS_MODE)
    {
      bool flag = Input.GetKey(KeyCode.Alpha2) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift));
      if (!GLOBALS.keyboard_inuse & flag && !this.old_toggle_button)
        this.info.SetActive(!this.info.activeSelf);
      this.old_toggle_button = flag;
    }
    if ((double) Time.time - (double) this.last_time_update <= 0.5)
      return;
    this.last_time_update = Time.time;
    if (!(bool) (UnityEngine.Object) this.info || !(bool) (UnityEngine.Object) this.red_details || !(bool) (UnityEngine.Object) this.blue_details || !(bool) (UnityEngine.Object) this.ir_scorer)
      return;
    this.red_details.text = this.ir_scorer.GetDetails();
    this.blue_details.text = this.ir_scorer.GetDetails(false);
  }

  public void MenuChanged()
  {
    if ((UnityEngine.Object) this.menu == (UnityEngine.Object) null || !this.init_done)
      return;
    this.ENABLE_DEPOT_PENALTIES = this.Rule_Depots.GetComponent<Toggle>().isOn;
    this.ENABLE_BLOCKING = this.Rule_Blocking.GetComponent<Toggle>().isOn;
    this.ENABLE_WALLBLOCKING = this.Rule_WallBlocking.GetComponent<Toggle>().isOn;
    this.ENABLE_BALLRETURNTOBAY = this.BallReturnToBay.GetComponent<Toggle>().isOn;
    if (!this.ENABLE_BLOCKING && !this.ENABLE_WALLBLOCKING)
      this.Penalty_Blocking.GetComponent<InputField>().interactable = false;
    else
      this.Penalty_Blocking.GetComponent<InputField>().interactable = true;
    int.TryParse(this.Penalty_Blocking.GetComponent<InputField>().text, out this.PENALTY_BLOCKING);
    this.PENALTY_BLOCKING = Mathf.Abs(this.PENALTY_BLOCKING);
    this.ENABLE_G10_G11 = this.Rule_G10G11.GetComponent<Toggle>().isOn;
    int.TryParse(this.Penalty_G10G11.GetComponent<InputField>().text, out this.PENALTY_G10_G11);
    float.TryParse(this.G10_clearance.GetComponent<InputField>().text, out this.G10_CLEAR_DISTANCE);
    if ((double) this.G10_CLEAR_DISTANCE < 2.0)
    {
      this.G10_CLEAR_DISTANCE = 2f;
      this.G10_clearance.GetComponent<InputField>().text = this.G10_CLEAR_DISTANCE.ToString();
    }
    int.TryParse(this.G11_blanking.GetComponent<InputField>().text, out this.G10_G11_BLANKING);
    int.TryParse(this.ShieldOffset.GetComponent<InputField>().text, out this.SHIELD_OFFSET);
    this.ENABLE_POWERUPS = this.Enable_Powerups.GetComponent<Toggle>().isOn;
    this.PU_SPEED = this.PU_Speed.GetComponent<Toggle>().isOn;
    this.PU_ONLYONE = this.PU_Onlyone.GetComponent<Toggle>().isOn;
    this.PU_TORQUE = this.PU_Torque.GetComponent<Toggle>().isOn;
    this.PU_INVISIBLE = this.PU_Invisible.GetComponent<Toggle>().isOn;
    this.PU_SLOW = this.PU_Slow.GetComponent<Toggle>().isOn;
    this.PU_WEAK = this.PU_Weak.GetComponent<Toggle>().isOn;
    this.PU_INVERTED = this.PU_Inverted.GetComponent<Toggle>().isOn;
    int.TryParse(this.PU_Off_duration.GetComponent<InputField>().text, out this.PU_OFFENSIVE_DURATION);
    int.TryParse(this.PU_Def_duration.GetComponent<InputField>().text, out this.PU_DEFENSIVE_DURATION);
    this.PU_CENTER = this.PU_Center.GetComponent<Toggle>().isOn;
    this.PU_CENTER_TYPE = this.PU_center_type.GetComponent<Dropdown>().value;
    this.PU_HOME = this.PU_home.GetComponent<Toggle>().isOn;
    this.PU_HOME_TYPE = this.PU_home_type.GetComponent<Dropdown>().value;
    int.TryParse(this.PU_Respawn_time.GetComponent<InputField>().text, out this.PU_RESPAWN);
    int.TryParse(this.PU_Strength.GetComponent<InputField>().text, out this.PU_STRENGTH);
    this.GAMEVERSION = this.Game_Version.GetComponent<Dropdown>().options[this.Game_Version.GetComponent<Dropdown>().value].text;
    this.UpdateServer();
    this.UpdateField();
    if (!(bool) (UnityEngine.Object) GLOBALS.topsingleplayer || !(bool) (UnityEngine.Object) GLOBALS.topsingleplayer.scorer)
      return;
    GLOBALS.topsingleplayer.scorer.clean_run = false;
  }

  public void OnClose()
  {
    if ((UnityEngine.Object) this.menu == (UnityEngine.Object) null)
      return;
    this.menu.SetActive(false);
  }

  public override string GetString() => (this.ENABLE_DEPOT_PENALTIES ? "1" : "0") + ":" + (this.ENABLE_BLOCKING ? "1" : "0") + ":" + this.PENALTY_BLOCKING.ToString() + ":" + (this.ENABLE_G10_G11 ? "1" : "0") + ":" + this.PENALTY_G10_G11.ToString() + ":" + this.G10_CLEAR_DISTANCE.ToString() + ":" + this.G10_G11_BLANKING.ToString() + ":" + (this.ENABLE_POWERUPS ? "1" : "0") + ":" + (this.PU_SPEED ? "1" : "0") + ":" + (this.PU_ONLYONE ? "1" : "0") + ":" + (this.PU_TORQUE ? "1" : "0") + ":" + (this.PU_INVISIBLE ? "1" : "0") + ":" + (this.PU_SLOW ? "1" : "0") + ":" + (this.PU_WEAK ? "1" : "0") + ":" + (this.PU_INVERTED ? "1" : "0") + ":" + this.PU_OFFENSIVE_DURATION.ToString() + ":" + this.PU_DEFENSIVE_DURATION.ToString() + ":" + (this.PU_CENTER ? "1" : "0") + ":" + this.PU_CENTER_TYPE.ToString() + ":" + (this.PU_HOME ? "1" : "0") + ":" + this.PU_HOME_TYPE.ToString() + ":" + this.PU_RESPAWN.ToString() + ":" + this.PU_STRENGTH.ToString() + ":" + (this.ENABLE_WALLBLOCKING ? "1" : "0") + ":" + (this.ENABLE_BALLRETURNTOBAY ? "1" : "0") + ":" + this.GAMEVERSION.ToString() + ":" + this.SHIELD_OFFSET.ToString();

  public override void SetString(string data)
  {
    if (this.menu.activeSelf)
      return;
    string[] strArray1 = data.Split(':');
    if (strArray1.Length < 3)
    {
      Debug.Log((object) ("Infinite Recharge settings string did not have at least 3 entries. It had " + (object) strArray1.Length));
    }
    else
    {
      int num1 = 0;
      string[] strArray2 = strArray1;
      int index1 = num1;
      int num2 = index1 + 1;
      this.ENABLE_DEPOT_PENALTIES = strArray2[index1] == "1";
      string[] strArray3 = strArray1;
      int index2 = num2;
      int num3 = index2 + 1;
      this.ENABLE_BLOCKING = strArray3[index2] == "1";
      string[] strArray4 = strArray1;
      int index3 = num3;
      int num4 = index3 + 1;
      this.PENALTY_BLOCKING = int.Parse(strArray4[index3]);
      if (strArray1.Length >= 7)
      {
        string[] strArray5 = strArray1;
        int index4 = num4;
        int num5 = index4 + 1;
        this.ENABLE_G10_G11 = strArray5[index4] == "1";
        string[] strArray6 = strArray1;
        int index5 = num5;
        int num6 = index5 + 1;
        this.PENALTY_G10_G11 = int.Parse(strArray6[index5]);
        string[] strArray7 = strArray1;
        int index6 = num6;
        int num7 = index6 + 1;
        this.G10_CLEAR_DISTANCE = float.Parse(strArray7[index6]);
        string[] strArray8 = strArray1;
        int index7 = num7;
        num4 = index7 + 1;
        this.G10_G11_BLANKING = int.Parse(strArray8[index7]);
      }
      if (strArray1.Length >= 22)
      {
        string[] strArray9 = strArray1;
        int index8 = num4;
        int num8 = index8 + 1;
        this.ENABLE_POWERUPS = strArray9[index8] == "1";
        string[] strArray10 = strArray1;
        int index9 = num8;
        int num9 = index9 + 1;
        this.PU_SPEED = strArray10[index9] == "1";
        string[] strArray11 = strArray1;
        int index10 = num9;
        int num10 = index10 + 1;
        this.PU_ONLYONE = strArray11[index10] == "1";
        string[] strArray12 = strArray1;
        int index11 = num10;
        int num11 = index11 + 1;
        this.PU_TORQUE = strArray12[index11] == "1";
        string[] strArray13 = strArray1;
        int index12 = num11;
        int num12 = index12 + 1;
        this.PU_INVISIBLE = strArray13[index12] == "1";
        string[] strArray14 = strArray1;
        int index13 = num12;
        int num13 = index13 + 1;
        this.PU_SLOW = strArray14[index13] == "1";
        string[] strArray15 = strArray1;
        int index14 = num13;
        int num14 = index14 + 1;
        this.PU_WEAK = strArray15[index14] == "1";
        string[] strArray16 = strArray1;
        int index15 = num14;
        int num15 = index15 + 1;
        this.PU_INVERTED = strArray16[index15] == "1";
        string[] strArray17 = strArray1;
        int index16 = num15;
        int num16 = index16 + 1;
        int.TryParse(strArray17[index16], out this.PU_OFFENSIVE_DURATION);
        string[] strArray18 = strArray1;
        int index17 = num16;
        int num17 = index17 + 1;
        int.TryParse(strArray18[index17], out this.PU_DEFENSIVE_DURATION);
        string[] strArray19 = strArray1;
        int index18 = num17;
        int num18 = index18 + 1;
        this.PU_CENTER = strArray19[index18] == "1";
        string[] strArray20 = strArray1;
        int index19 = num18;
        int num19 = index19 + 1;
        this.PU_CENTER_TYPE = int.Parse(strArray20[index19]);
        string[] strArray21 = strArray1;
        int index20 = num19;
        int num20 = index20 + 1;
        this.PU_HOME = strArray21[index20] == "1";
        string[] strArray22 = strArray1;
        int index21 = num20;
        int num21 = index21 + 1;
        this.PU_HOME_TYPE = int.Parse(strArray22[index21]);
        string[] strArray23 = strArray1;
        int index22 = num21;
        num4 = index22 + 1;
        int.TryParse(strArray23[index22], out this.PU_RESPAWN);
      }
      if (strArray1.Length >= num4 + 3)
      {
        string[] strArray24 = strArray1;
        int index23 = num4;
        int num22 = index23 + 1;
        int.TryParse(strArray24[index23], out this.PU_STRENGTH);
        string[] strArray25 = strArray1;
        int index24 = num22;
        int num23 = index24 + 1;
        this.ENABLE_WALLBLOCKING = strArray25[index24] == "1";
        string[] strArray26 = strArray1;
        int index25 = num23;
        num4 = index25 + 1;
        this.ENABLE_BALLRETURNTOBAY = strArray26[index25] == "1";
      }
      if (strArray1.Length >= num4 + 2)
      {
        string[] strArray27 = strArray1;
        int index26 = num4;
        int num24 = index26 + 1;
        this.GAMEVERSION = strArray27[index26];
        string[] strArray28 = strArray1;
        int index27 = num24;
        int num25 = index27 + 1;
        this.SHIELD_OFFSET = int.Parse(strArray28[index27]);
      }
      this.Start();
      this.UpdateServer();
    }
  }

  public void UpdateField(bool force = false)
  {
    if (!force && this.oldGAMEVERSION == this.GAMEVERSION)
      return;
    foreach (MyFlags myFlags in UnityEngine.Resources.FindObjectsOfTypeAll<MyFlags>())
    {
      int index = myFlags.flag.IndexOf("FieldVersion");
      if (index >= 0)
      {
        if (myFlags.value[index] == this.GAMEVERSION)
          myFlags.gameObject.SetActive(true);
        else
          myFlags.gameObject.SetActive(false);
      }
    }
    this.oldGAMEVERSION = this.GAMEVERSION;
    string gameversion = this.GAMEVERSION;
    if (!(gameversion == "2020"))
    {
      if (!(gameversion == "2021"))
        return;
      GLOBALS.game_option = 2;
    }
    else
      GLOBALS.game_option = 1;
  }

  public override string GetCleanString() => this.GetString();
}
