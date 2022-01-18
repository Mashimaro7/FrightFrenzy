// Decompiled with JetBrains decompiler
// Type: RobotID
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class RobotID : MonoBehaviour
{
  public Transform wheelTL;
  public Transform wheelTR;
  public Transform wheelBL;
  public Transform wheelBR;
  public Vector3 position;
  public string starting_pos;
  public int id;
  public bool is_red;
  public bool is_holding;
  public bool is_counting;
  public float count_start;
  public float count_duration = 5f;
  public Dictionary<string, string> userData = new Dictionary<string, string>();

  public void Copy(RobotID_Data refin)
  {
    this.starting_pos = refin.starting_pos;
    this.id = refin.id;
    this.is_red = refin.is_red;
    this.is_holding = refin.is_holding;
    foreach (string key in refin.userData.Keys)
      this.userData[key] = refin.userData[key];
  }

  public void Copy(RobotID refin)
  {
    this.starting_pos = refin.starting_pos;
    this.id = refin.id;
    this.is_red = refin.is_red;
    this.is_holding = refin.is_holding;
    foreach (string key in refin.userData.Keys)
      this.userData[key] = refin.userData[key];
  }

  private void Update()
  {
    if ((Object) this.wheelTL == (Object) null || (Object) this.wheelTR == (Object) null || (Object) this.wheelBL == (Object) null || (Object) this.wheelBR == (Object) null)
    {
      this.wheelTL = this.FindInHierarchy("wheelTL");
      this.wheelTR = this.FindInHierarchy("wheelTR");
      this.wheelBL = this.FindInHierarchy("wheelBL");
      this.wheelBR = this.FindInHierarchy("wheelBR");
    }
    if ((Object) this.wheelTL == (Object) null || (Object) this.wheelTR == (Object) null || (Object) this.wheelBL == (Object) null || (Object) this.wheelBR == (Object) null)
      return;
    this.position = (this.wheelTL.position + this.wheelTR.position + this.wheelBL.position + this.wheelBR.position) / 4f;
  }

  private Transform FindInHierarchy(string name)
  {
    foreach (Transform componentsInChild in this.gameObject.GetComponentsInChildren<Transform>())
    {
      if (componentsInChild.gameObject.name.CompareTo(name) == 0)
        return componentsInChild;
    }
    return (Transform) null;
  }

  public void SetUserInt(string key, int value) => this.userData[key] = value.ToString();

  public int GetUserInt(string key)
  {
    if (!this.userData.ContainsKey(key))
      this.userData[key] = "0";
    return int.Parse(this.userData[key]);
  }

  public void SetUserFloat(string key, float value) => this.userData[key] = value.ToString();

  public float GetUserFloat(string key)
  {
    if (!this.userData.ContainsKey(key))
      this.userData[key] = "0";
    return float.Parse(this.userData[key]);
  }

  public bool GetUserBool(string key)
  {
    if (!this.userData.ContainsKey(key))
      this.userData[key] = "0";
    return this.userData[key][0] == '1';
  }

  public void SetUserBool(string key, bool value = true) => this.userData[key] = value ? "1" : "0";

  public void RemoveData(string key)
  {
    if (!this.userData.ContainsKey(key))
      return;
    this.userData.Remove(key);
  }
}
