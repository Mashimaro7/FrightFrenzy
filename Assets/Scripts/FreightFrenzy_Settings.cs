// Decompiled with JetBrains decompiler
// Type: FreightFrenzy_Settings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FreightFrenzy_Settings : GameSettings
{
  public bool ENABLE_AUTO_PUSHBACK = true;
  public Transform Rule_AutoPushback;
  public bool ENABLE_ANCHORS;
  public Toggle anchor_hubs;
  public bool ENABLE_HUB_PUSHBACK;
  public Toggle hub_pushback_toggle;
  public bool ENABLE_POSSESSION_LIMIT = true;
  public Toggle possession_limit;
  public int RESTART_POS_PENALTY = 30;
  public InputField reset_penalty_field;
  public int REF_RESET_PENALTY = 10;
  public InputField ref_restart_field;
  public GameObject menu;
  public bool ENABLE_BLOCKING;
  public float BLOCKING_DURATION = 5f;
  public float BLOCKING_RESET_HOLDING_TIME = 2f;
  public int PENALTIES_MAJOR = 30;
  public int PENALTIES_MINOR = 10;
  public ConfigurableJoint[] hubs;
  public Generic_Robot_Pushback[] hubs_pushbacks;
  public TextMeshProUGUI red_details;
  public TextMeshProUGUI blue_details;
  private float last_time_update;
  private Scorekeeper_FreightFrenzy ff_scorer;
  private bool old_toggle_button;
  private bool init_done;

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
    if (!(bool) (Object) this.info || !(bool) (Object) this.red_details || !(bool) (Object) this.blue_details || !(bool) (Object) this.ff_scorer)
      return;
    this.red_details.text = this.ff_scorer.GetDetails();
    this.blue_details.text = this.ff_scorer.GetDetails(false);
  }

  private void Start()
  {
    this.init_done = false;
    this.Rule_AutoPushback.GetComponent<Toggle>().isOn = this.ENABLE_AUTO_PUSHBACK;
    this.anchor_hubs.isOn = this.ENABLE_ANCHORS;
    this.hub_pushback_toggle.isOn = this.ENABLE_HUB_PUSHBACK;
    this.possession_limit.isOn = this.ENABLE_POSSESSION_LIMIT;
    this.reset_penalty_field.text = this.RESTART_POS_PENALTY.ToString();
    this.ref_restart_field.text = this.REF_RESET_PENALTY.ToString();
    this.ff_scorer = GameObject.Find("Scorekeeper").GetComponent<Scorekeeper_FreightFrenzy>();
    this.UpdateHubs();
    this.init_done = true;
  }

  public void MenuChanged()
  {
    if ((Object) this.menu == (Object) null || !this.init_done)
      return;
    this.ENABLE_AUTO_PUSHBACK = this.Rule_AutoPushback.GetComponent<Toggle>().isOn;
    this.ENABLE_ANCHORS = this.anchor_hubs.isOn;
    this.ENABLE_HUB_PUSHBACK = this.hub_pushback_toggle.isOn;
    this.ENABLE_POSSESSION_LIMIT = this.possession_limit.isOn;
    this.RESTART_POS_PENALTY = int.Parse(this.reset_penalty_field.text);
    this.REF_RESET_PENALTY = int.Parse(this.ref_restart_field.text);
    this.UpdateHubs();
    this.UpdateServer();
  }

  private void UpdateHubs()
  {
    if (GLOBALS.CLIENT_MODE)
      return;
    foreach (ConfigurableJoint hub in this.hubs)
    {
      if ((bool) (Object) hub)
      {
        if (!this.ENABLE_ANCHORS)
        {
          hub.xMotion = ConfigurableJointMotion.Free;
          hub.yMotion = ConfigurableJointMotion.Free;
          hub.zMotion = ConfigurableJointMotion.Free;
        }
        else
        {
          hub.xMotion = ConfigurableJointMotion.Limited;
          hub.yMotion = ConfigurableJointMotion.Locked;
          hub.zMotion = ConfigurableJointMotion.Locked;
        }
      }
    }
    foreach (Generic_Robot_Pushback hubsPushback in this.hubs_pushbacks)
      hubsPushback.enable = this.ENABLE_HUB_PUSHBACK;
  }

  public void OnClose()
  {
    if ((Object) this.menu == (Object) null)
      return;
    this.menu.SetActive(false);
  }

  public override string GetString() => (this.ENABLE_AUTO_PUSHBACK ? "1" : "0") + ":" + (this.ENABLE_ANCHORS ? "1" : "0") + ":" + (this.ENABLE_HUB_PUSHBACK ? "1" : "0") + ":" + (this.ENABLE_POSSESSION_LIMIT ? "1" : "0") + ":" + this.RESTART_POS_PENALTY.ToString() + ":" + this.REF_RESET_PENALTY.ToString();

  public override void SetString(string data)
  {
    if (this.menu.activeSelf)
      return;
    string[] strArray = data.Split(':');
    if (strArray.Length < 3)
    {
      Debug.Log((object) ("Freight Frenzy settings string did not have >=3 entries. It had " + (object) strArray.Length));
    }
    else
    {
      this.ENABLE_AUTO_PUSHBACK = strArray[0] == "1";
      this.ENABLE_ANCHORS = strArray[1] == "1";
      this.ENABLE_HUB_PUSHBACK = strArray[2] == "1";
      if (strArray.Length >= 4)
        this.ENABLE_POSSESSION_LIMIT = strArray[3] == "1";
      if (strArray.Length >= 5)
        int.TryParse(strArray[4], out this.RESTART_POS_PENALTY);
      if (strArray.Length >= 6)
        int.TryParse(strArray[5], out this.REF_RESET_PENALTY);
      this.Start();
      this.UpdateHubs();
      this.UpdateServer();
    }
  }
}
