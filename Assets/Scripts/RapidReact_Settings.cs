// Decompiled with JetBrains decompiler
// Type: RapidReact_Settings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RapidReact_Settings : GameSettings
{
  public Transform Auto_Pushback;
  public bool ENABLE_AUTO_PUSHBACK;
  public float FOUL_BLANKING = 5f;
  public Toggle enable_lp;
  public bool ENABLE_LP_FOUL = true;
  public InputField lp_foul;
  public int LP_FOUL_POINTS = 4;
  public GameObject menu;
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
  public TextMeshProUGUI red_details;
  public TextMeshProUGUI blue_details;
  private Scorekeeper_RapidReact rr_scorer;
  private bool init_done;
  private bool old_toggle_button;
  private float last_time_update;

  private void Awake()
  {
  }

  private void Start()
  {
    this.init_done = false;
    this.rr_scorer = GameObject.Find("Scorekeeper").GetComponent<Scorekeeper_RapidReact>();
    this.Auto_Pushback.GetComponent<Toggle>().isOn = this.ENABLE_AUTO_PUSHBACK;
    this.enable_lp.isOn = this.ENABLE_LP_FOUL;
    this.lp_foul.text = this.LP_FOUL_POINTS.ToString();
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
    if (!(bool) (Object) this.info || !(bool) (Object) this.red_details || !(bool) (Object) this.blue_details || !(bool) (Object) this.rr_scorer)
      return;
    this.red_details.text = this.rr_scorer.GetDetails();
    this.blue_details.text = this.rr_scorer.GetDetails(false);
  }

  public void MenuChanged()
  {
    if ((Object) this.menu == (Object) null || !this.init_done)
      return;
    this.ENABLE_AUTO_PUSHBACK = this.Auto_Pushback.GetComponent<Toggle>().isOn;
    this.ENABLE_LP_FOUL = this.enable_lp.isOn;
    this.LP_FOUL_POINTS = int.Parse(this.lp_foul.text);
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
    this.UpdateServer();
    this.UpdateField();
    if (!(bool) (Object) GLOBALS.topsingleplayer || !(bool) (Object) GLOBALS.topsingleplayer.scorer)
      return;
    GLOBALS.topsingleplayer.scorer.clean_run = false;
  }

  public void OnClose()
  {
    if ((Object) this.menu == (Object) null)
      return;
    this.menu.SetActive(false);
  }

  public override string GetString() => (this.ENABLE_AUTO_PUSHBACK ? "1" : "0") + ":" + (this.ENABLE_LP_FOUL ? "1" : "0") + ":" + this.LP_FOUL_POINTS.ToString() + ":" + (this.ENABLE_POWERUPS ? "1" : "0") + ":" + (this.PU_SPEED ? "1" : "0") + ":" + (this.PU_ONLYONE ? "1" : "0") + ":" + (this.PU_TORQUE ? "1" : "0") + ":" + (this.PU_INVISIBLE ? "1" : "0") + ":" + (this.PU_SLOW ? "1" : "0") + ":" + (this.PU_WEAK ? "1" : "0") + ":" + (this.PU_INVERTED ? "1" : "0") + ":" + this.PU_OFFENSIVE_DURATION.ToString() + ":" + this.PU_DEFENSIVE_DURATION.ToString() + ":" + (this.PU_CENTER ? "1" : "0") + ":" + this.PU_CENTER_TYPE.ToString() + ":" + (this.PU_HOME ? "1" : "0") + ":" + this.PU_HOME_TYPE.ToString() + ":" + this.PU_RESPAWN.ToString() + ":" + this.PU_STRENGTH.ToString();

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
      this.ENABLE_AUTO_PUSHBACK = strArray2[index1] == "1";
      string[] strArray3 = strArray1;
      int index2 = num2;
      int num3 = index2 + 1;
      this.ENABLE_LP_FOUL = strArray3[index2] == "1";
      string[] strArray4 = strArray1;
      int index3 = num3;
      int num4 = index3 + 1;
      int.TryParse(strArray4[index3], out this.LP_FOUL_POINTS);
      if (strArray1.Length >= num4 + 16)
      {
        string[] strArray5 = strArray1;
        int index4 = num4;
        int num5 = index4 + 1;
        this.ENABLE_POWERUPS = strArray5[index4] == "1";
        string[] strArray6 = strArray1;
        int index5 = num5;
        int num6 = index5 + 1;
        this.PU_SPEED = strArray6[index5] == "1";
        string[] strArray7 = strArray1;
        int index6 = num6;
        int num7 = index6 + 1;
        this.PU_ONLYONE = strArray7[index6] == "1";
        string[] strArray8 = strArray1;
        int index7 = num7;
        int num8 = index7 + 1;
        this.PU_TORQUE = strArray8[index7] == "1";
        string[] strArray9 = strArray1;
        int index8 = num8;
        int num9 = index8 + 1;
        this.PU_INVISIBLE = strArray9[index8] == "1";
        string[] strArray10 = strArray1;
        int index9 = num9;
        int num10 = index9 + 1;
        this.PU_SLOW = strArray10[index9] == "1";
        string[] strArray11 = strArray1;
        int index10 = num10;
        int num11 = index10 + 1;
        this.PU_WEAK = strArray11[index10] == "1";
        string[] strArray12 = strArray1;
        int index11 = num11;
        int num12 = index11 + 1;
        this.PU_INVERTED = strArray12[index11] == "1";
        string[] strArray13 = strArray1;
        int index12 = num12;
        int num13 = index12 + 1;
        int.TryParse(strArray13[index12], out this.PU_OFFENSIVE_DURATION);
        string[] strArray14 = strArray1;
        int index13 = num13;
        int num14 = index13 + 1;
        int.TryParse(strArray14[index13], out this.PU_DEFENSIVE_DURATION);
        string[] strArray15 = strArray1;
        int index14 = num14;
        int num15 = index14 + 1;
        this.PU_CENTER = strArray15[index14] == "1";
        string[] strArray16 = strArray1;
        int index15 = num15;
        int num16 = index15 + 1;
        this.PU_CENTER_TYPE = int.Parse(strArray16[index15]);
        string[] strArray17 = strArray1;
        int index16 = num16;
        int num17 = index16 + 1;
        this.PU_HOME = strArray17[index16] == "1";
        string[] strArray18 = strArray1;
        int index17 = num17;
        int num18 = index17 + 1;
        this.PU_HOME_TYPE = int.Parse(strArray18[index17]);
        string[] strArray19 = strArray1;
        int index18 = num18;
        int num19 = index18 + 1;
        int.TryParse(strArray19[index18], out this.PU_RESPAWN);
        string[] strArray20 = strArray1;
        int index19 = num19;
        int num20 = index19 + 1;
        int.TryParse(strArray20[index19], out this.PU_STRENGTH);
      }
      this.Start();
      this.UpdateServer();
    }
  }

  public void UpdateField(bool force = false)
  {
  }

  public override string GetCleanString() => this.GetString();
}
