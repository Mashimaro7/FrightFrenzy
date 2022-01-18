// Decompiled with JetBrains decompiler
// Type: UltimateGoal_Settings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

public class UltimateGoal_Settings : GameSettings
{
  public Transform Penalty_Major;
  public int PENALTY_MAJOR = 30;
  public Transform Auto_Walls;
  public bool ENABLE_AUTO_WALLS;
  public Transform Zones_Walls;
  public bool ENABLE_WALL_ZONES;
  public Transform Zones_Field;
  public bool ENABLE_HALFFIELD_ZONES;
  public Transform Penalty_blocking;
  public int PENALTY_BLOCKING = 10;
  public Transform Enable_blocking;
  public bool ENABLE_BLOCKING = true;
  public GameObject menu;
  public float BLOCKING_DURATION = 5f;
  public float BLOCKING_RESET_HOLDING_TIME = 2f;
  private bool init_done;

  private void Awake()
  {
  }

  private void Start()
  {
    this.init_done = false;
    this.Penalty_Major.GetComponent<InputField>().text = this.PENALTY_MAJOR.ToString();
    this.Auto_Walls.GetComponent<Toggle>().isOn = this.ENABLE_AUTO_WALLS;
    this.Zones_Walls.GetComponent<Toggle>().isOn = this.ENABLE_WALL_ZONES;
    this.Zones_Field.GetComponent<Toggle>().isOn = this.ENABLE_HALFFIELD_ZONES;
    this.Penalty_blocking.GetComponent<InputField>().text = this.PENALTY_BLOCKING.ToString();
    this.Enable_blocking.GetComponent<Toggle>().isOn = this.ENABLE_BLOCKING;
    this.init_done = true;
  }

  public void MenuChanged()
  {
    if ((Object) this.menu == (Object) null || !this.init_done)
      return;
    int.TryParse(this.Penalty_Major.GetComponent<InputField>().text, out this.PENALTY_MAJOR);
    this.PENALTY_MAJOR = Mathf.Abs(this.PENALTY_MAJOR);
    this.ENABLE_AUTO_WALLS = this.Auto_Walls.GetComponent<Toggle>().isOn;
    this.ENABLE_WALL_ZONES = this.Zones_Walls.GetComponent<Toggle>().isOn;
    this.ENABLE_HALFFIELD_ZONES = this.Zones_Field.GetComponent<Toggle>().isOn;
    int.TryParse(this.Penalty_blocking.GetComponent<InputField>().text, out this.PENALTY_BLOCKING);
    this.PENALTY_BLOCKING = Mathf.Abs(this.PENALTY_BLOCKING);
    this.ENABLE_BLOCKING = this.Enable_blocking.GetComponent<Toggle>().isOn;
    this.UpdateServer();
  }

  public void OnClose()
  {
    if ((Object) this.menu == (Object) null)
      return;
    this.menu.SetActive(false);
  }

  public override string GetString() => this.PENALTY_MAJOR.ToString() + ":" + (this.ENABLE_AUTO_WALLS ? "1" : "0") + ":" + (this.ENABLE_WALL_ZONES ? "1" : "0") + ":" + (this.ENABLE_HALFFIELD_ZONES ? "1" : "0") + ":" + this.PENALTY_BLOCKING.ToString() + ":" + (this.ENABLE_BLOCKING ? "1" : "0");

  public override void SetString(string data)
  {
    if (this.menu.activeSelf)
      return;
    string[] strArray = data.Split(':');
    if (strArray.Length != 6)
    {
      Debug.Log((object) ("Ultimate Goal settings string did not have 6 entries. It had " + (object) strArray.Length));
    }
    else
    {
      this.PENALTY_MAJOR = int.Parse(strArray[0]);
      this.ENABLE_AUTO_WALLS = strArray[1] == "1";
      this.ENABLE_WALL_ZONES = strArray[2] == "1";
      this.ENABLE_HALFFIELD_ZONES = strArray[3] == "1";
      this.PENALTY_BLOCKING = int.Parse(strArray[4]);
      this.ENABLE_BLOCKING = strArray[5] == "1";
      this.Start();
      this.UpdateServer();
    }
  }
}
