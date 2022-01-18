// Decompiled with JetBrains decompiler
// Type: ChangeUp_Settings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

public class ChangeUp_Settings : GameSettings
{
  public bool ENABLE_LINES = true;
  public bool ENABLE_LINES_SPEC = true;
  public bool ENABLE_LIGHTS = true;
  public bool ENABLE_AUTO_PUSHBACK = true;
  public GameObject menu;
  public Transform Rule_Lines;
  public Transform Rule_Lines_Spec;
  public Transform Rule_Lights;
  public Transform Rule_AutoPushback;
  private bool init_done;

  private void Start()
  {
    this.init_done = false;
    this.Rule_Lines.GetComponent<Toggle>().isOn = this.ENABLE_LINES;
    this.Rule_Lines_Spec.GetComponent<Toggle>().isOn = this.ENABLE_LINES_SPEC;
    this.Rule_Lights.GetComponent<Toggle>().isOn = this.ENABLE_LIGHTS;
    this.Rule_AutoPushback.GetComponent<Toggle>().isOn = this.ENABLE_AUTO_PUSHBACK;
    this.init_done = true;
  }

  public void MenuChanged()
  {
    if ((Object) this.menu == (Object) null || !this.init_done)
      return;
    this.ENABLE_LINES = this.Rule_Lines.GetComponent<Toggle>().isOn;
    this.ENABLE_LINES_SPEC = this.Rule_Lines_Spec.GetComponent<Toggle>().isOn;
    this.ENABLE_LIGHTS = this.Rule_Lights.GetComponent<Toggle>().isOn;
    this.ENABLE_AUTO_PUSHBACK = this.Rule_AutoPushback.GetComponent<Toggle>().isOn;
    this.UpdateServer();
  }

  public void OnClose()
  {
    if ((Object) this.menu == (Object) null)
      return;
    this.menu.SetActive(false);
  }

  public override string GetString() => (this.ENABLE_LINES ? "1" : "0") + ":" + (this.ENABLE_LINES_SPEC ? "1" : "0") + ":" + (this.ENABLE_LIGHTS ? "1" : "0") + ":" + (this.ENABLE_AUTO_PUSHBACK ? "1" : "0");

  public override void SetString(string data)
  {
    if (this.menu.activeSelf)
      return;
    string[] strArray = data.Split(':');
    if (strArray.Length != 4)
    {
      Debug.Log((object) ("Infinite Recharge settings string did not have 4 entries. It had " + (object) strArray.Length));
    }
    else
    {
      this.ENABLE_LINES = strArray[0] == "1";
      this.ENABLE_LINES_SPEC = strArray[1] == "1";
      this.ENABLE_LIGHTS = strArray[2] == "1";
      this.ENABLE_AUTO_PUSHBACK = strArray[3] == "1";
      this.Start();
      this.UpdateServer();
    }
  }
}
