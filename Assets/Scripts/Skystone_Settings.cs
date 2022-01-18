// Decompiled with JetBrains decompiler
// Type: Skystone_Settings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

public class Skystone_Settings : GameSettings
{
  public bool ENABLE_DEPOT_PENALTIES = true;
  public bool ENABLE_SKYBRIDGE_PENALTIES = true;
  public float RESET_DURATION_UNDER_SKYBRIDGE = 2f;
  public bool ENABLE_FOUNDATION_PENALTY = true;
  public bool ENABLE_POSSESSION_PENALTY = true;
  public bool ENABLE_BLOCKING = true;
  public float BLOCKING_DURATION = 5f;
  public float BLOCKING_RESET_HOLDING_TIME = 2f;
  public int PENALTY_SKYBRIDGE = 5;
  public int PENALTY_BLOCKING = 5;
  public int PENALTY_SCORING = 20;
  public int PENALTY_POSSESSION = 5;
  public int PENALTY_POSSESSION_GRACE = 500;
  public GameObject menu;
  public Transform Rule_Depots;
  public Transform Rule_Skybridge;
  public Transform Rule_Foundation;
  public Transform Rule_Blocking;
  public Transform Penalty_Skybridge;
  public Transform Penalty_Foundation;
  public Transform Penalty_Blocking;
  public Transform Rule_possession;
  public Transform Penalty_possession;
  public Transform Penalty_possession_grace;
  private bool init_done;

  private void Start()
  {
    this.init_done = false;
    this.Rule_Depots.GetComponent<Toggle>().isOn = this.ENABLE_DEPOT_PENALTIES;
    this.Rule_Skybridge.GetComponent<Toggle>().isOn = this.ENABLE_SKYBRIDGE_PENALTIES;
    this.Rule_Foundation.GetComponent<Toggle>().isOn = this.ENABLE_FOUNDATION_PENALTY;
    this.Rule_Blocking.GetComponent<Toggle>().isOn = this.ENABLE_BLOCKING;
    this.Rule_possession.GetComponent<Toggle>().isOn = this.ENABLE_POSSESSION_PENALTY;
    this.Penalty_Skybridge.GetComponent<InputField>().text = this.PENALTY_SKYBRIDGE.ToString();
    this.Penalty_Foundation.GetComponent<InputField>().text = this.PENALTY_SCORING.ToString();
    this.Penalty_Blocking.GetComponent<InputField>().text = this.PENALTY_BLOCKING.ToString();
    this.Penalty_possession.GetComponent<InputField>().text = this.PENALTY_POSSESSION.ToString();
    this.Penalty_possession_grace.GetComponent<InputField>().text = this.PENALTY_POSSESSION_GRACE.ToString();
    this.init_done = true;
  }

  public void MenuChanged()
  {
    if ((Object) this.menu == (Object) null || !this.init_done)
      return;
    this.ENABLE_DEPOT_PENALTIES = this.Rule_Depots.GetComponent<Toggle>().isOn;
    this.ENABLE_SKYBRIDGE_PENALTIES = this.Rule_Skybridge.GetComponent<Toggle>().isOn;
    this.ENABLE_FOUNDATION_PENALTY = this.Rule_Foundation.GetComponent<Toggle>().isOn;
    this.ENABLE_BLOCKING = this.Rule_Blocking.GetComponent<Toggle>().isOn;
    this.ENABLE_POSSESSION_PENALTY = this.Rule_possession.GetComponent<Toggle>().isOn;
    int.TryParse(this.Penalty_Skybridge.GetComponent<InputField>().text, out this.PENALTY_SKYBRIDGE);
    int.TryParse(this.Penalty_Foundation.GetComponent<InputField>().text, out this.PENALTY_SCORING);
    int.TryParse(this.Penalty_Blocking.GetComponent<InputField>().text, out this.PENALTY_BLOCKING);
    int.TryParse(this.Penalty_possession.GetComponent<InputField>().text, out this.PENALTY_POSSESSION);
    int.TryParse(this.Penalty_possession_grace.GetComponent<InputField>().text, out this.PENALTY_POSSESSION_GRACE);
    this.PENALTY_SKYBRIDGE = Mathf.Abs(this.PENALTY_SKYBRIDGE);
    this.PENALTY_SCORING = Mathf.Abs(this.PENALTY_SCORING);
    this.PENALTY_BLOCKING = Mathf.Abs(this.PENALTY_BLOCKING);
    this.PENALTY_POSSESSION = Mathf.Abs(this.PENALTY_POSSESSION);
    this.PENALTY_POSSESSION_GRACE = Mathf.Abs(this.PENALTY_POSSESSION_GRACE);
    this.UpdateServer();
  }

  public void OnClose()
  {
    if ((Object) this.menu == (Object) null)
      return;
    this.menu.SetActive(false);
  }

  public override string GetString() => (this.ENABLE_DEPOT_PENALTIES ? (object) "1" : (object) "0").ToString() + ":" + (this.ENABLE_SKYBRIDGE_PENALTIES ? (object) "1" : (object) "0") + ":" + (this.ENABLE_FOUNDATION_PENALTY ? (object) "1" : (object) "0") + ":" + (this.ENABLE_BLOCKING ? (object) "1" : (object) "0") + ":" + (this.ENABLE_POSSESSION_PENALTY ? (object) "1" : (object) "0") + ":" + (object) this.PENALTY_SKYBRIDGE + ":" + (object) this.PENALTY_SCORING + ":" + (object) this.PENALTY_BLOCKING + ":" + (object) this.PENALTY_POSSESSION + ":" + (object) this.PENALTY_POSSESSION_GRACE;

  public override void SetString(string data)
  {
    if (this.menu.activeSelf)
      return;
    string[] strArray = data.Split(':');
    if (strArray.Length != 10)
    {
      Debug.Log((object) ("Infinite Recharge settings string did not have 10 entries. It had " + (object) strArray.Length));
    }
    else
    {
      this.ENABLE_DEPOT_PENALTIES = strArray[0] == "1";
      this.ENABLE_SKYBRIDGE_PENALTIES = strArray[1] == "1";
      this.ENABLE_FOUNDATION_PENALTY = strArray[2] == "1";
      this.ENABLE_BLOCKING = strArray[3] == "1";
      this.ENABLE_POSSESSION_PENALTY = strArray[4] == "1";
      this.PENALTY_SKYBRIDGE = int.Parse(strArray[5]);
      this.PENALTY_SCORING = int.Parse(strArray[6]);
      this.PENALTY_BLOCKING = int.Parse(strArray[7]);
      this.PENALTY_POSSESSION = int.Parse(strArray[8]);
      this.PENALTY_POSSESSION_GRACE = int.Parse(strArray[9]);
      this.Start();
      this.UpdateServer();
    }
  }
}
