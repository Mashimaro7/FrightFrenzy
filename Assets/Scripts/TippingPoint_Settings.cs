// Decompiled with JetBrains decompiler
// Type: TippingPoint_Settings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

public class TippingPoint_Settings : GameSettings
{
  public bool ENABLE_AUTO_PUSHBACK = true;
  public GameObject menu;
  public Transform Rule_AutoPushback;
  private bool init_done;

  private void Start()
  {
    this.init_done = false;
    this.Rule_AutoPushback.GetComponent<Toggle>().isOn = this.ENABLE_AUTO_PUSHBACK;
    this.init_done = true;
  }

  public void MenuChanged()
  {
    if ((Object) this.menu == (Object) null || !this.init_done)
      return;
    this.ENABLE_AUTO_PUSHBACK = this.Rule_AutoPushback.GetComponent<Toggle>().isOn;
    this.UpdateServer();
  }

  public void OnClose()
  {
    if ((Object) this.menu == (Object) null)
      return;
    this.menu.SetActive(false);
  }

  public override string GetString() => !this.ENABLE_AUTO_PUSHBACK ? "0" : "1";

  public override void SetString(string data)
  {
    if (this.menu.activeSelf)
      return;
    string[] strArray = data.Split(':');
    if (strArray.Length != 1)
    {
      Debug.Log((object) ("Infinite Recharge settings string did not have 1 entries. It had " + (object) strArray.Length));
    }
    else
    {
      this.ENABLE_AUTO_PUSHBACK = strArray[0] == "1";
      this.Start();
      this.UpdateServer();
    }
  }
}
