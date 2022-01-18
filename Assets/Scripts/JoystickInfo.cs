// Decompiled with JetBrains decompiler
// Type: JoystickInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.UI;

public class JoystickInfo : MonoBehaviour
{
  public JoystickRawInfo joydata = new JoystickRawInfo();
  public JoystickAdvancedMenu adv_menu;
  public bool getting_axis;

  private void Start()
  {
    JoystickAdvancedMenu[] objectsOfTypeAll = UnityEngine.Resources.FindObjectsOfTypeAll<JoystickAdvancedMenu>();
    if (objectsOfTypeAll.Length < 1)
      return;
    this.adv_menu = objectsOfTypeAll[0];
  }

  public void Copy(JoystickRawInfo inref) => this.joydata.Copy(inref);

  public void UpdateGUI()
  {
    this.transform.Find("InputField/Label").GetComponent<Text>().text = this.joydata.info;
    this.transform.Find("InputField/Text").GetComponent<Text>().text = this.GetJoystickText(this.joydata);
    if (this.joydata.isInverted())
    {
      Color color = this.transform.Find("InputField").GetComponent<Image>().color with
      {
        r = 0.5f
      };
      this.transform.Find("InputField").GetComponent<Image>().color = color;
    }
    else
    {
      Color color = this.transform.Find("InputField").GetComponent<Image>().color with
      {
        r = 1f
      };
      this.transform.Find("InputField").GetComponent<Image>().color = color;
    }
  }

  public void GUI_Start_Capturing()
  {
    if (GLOBALS.keyboard_inuse)
      return;
    if (this.joydata.isButton)
      this.transform.Find("InputField/Text").GetComponent<Text>().text = "Press Button";
    else
      this.transform.Find("InputField/Text").GetComponent<Text>().text = "Move Up/Left";
    GLOBALS.keyboard_inuse = true;
    this.getting_axis = true;
  }

  private void Update()
  {
    if (!this.getting_axis)
      return;
    if (this.joydata.isButton)
    {
      if (this.GetJoystickButton() >= 0)
        this.getting_axis = false;
    }
    else if (this.GetJoystickAxis() >= 0)
      this.getting_axis = false;
    if (this.getting_axis && !Input.GetKeyDown(KeyCode.Escape))
      return;
    this.UpdateGUI();
    GLOBALS.keyboard_inuse = false;
  }

  public int GetJoystickButton()
  {
    for (int index = 0; index <= 16; ++index)
    {
      for (int joystickButton = 0; joystickButton < 20; ++joystickButton)
      {
        if (Input.GetKey("joystick " + (index > 0 ? index.ToString() + " " : "") + "button " + joystickButton.ToString()))
        {
          this.joydata.joystick_num = index;
          this.joydata.button_lu_num = joystickButton;
          this.joydata.isButton = true;
          return joystickButton;
        }
      }
    }
    return -1;
  }

  public int GetJoystickAxis()
  {
    for (int joystickAxis = 1; joystickAxis <= 15; ++joystickAxis)
    {
      float axis = Input.GetAxis("J0Axis" + joystickAxis.ToString());
      if ((double) Math.Abs(axis) > 0.5)
      {
        this.joydata.axis = joystickAxis;
        if ((double) axis > 0.300000011920929)
        {
          this.joydata.leftup_value = 1f;
          this.joydata.rightdown_value = -1f;
        }
        else
        {
          this.joydata.leftup_value = -1f;
          this.joydata.rightdown_value = 1f;
        }
        return joystickAxis;
      }
    }
    return -1;
  }

  public string GetJoystickText(JoystickRawInfo j)
  {
    string str1 = "";
    if (j.joystick_num > 0)
      str1 = str1 + "joy " + (object) j.joystick_num + " ";
    if (j.isButton)
    {
      string str2 = str1 + "button ";
      return j.axis <= 0 ? str2 + j.button_lu_num.ToString() : str2 + "axis " + (object) j.axis;
    }
    string joystickText;
    if (j.axis > 0)
      joystickText = str1 + "axis " + (object) j.axis;
    else
      joystickText = str1 + "button " + j.button_lu_num.ToString() + " & " + (object) j.button_rd_num;
    return joystickText;
  }

  public void StartAdvMenu()
  {
    if ((UnityEngine.Object) this.adv_menu == (UnityEngine.Object) null)
      return;
    this.adv_menu.Init(this.joydata, this);
    this.adv_menu.gameObject.SetActive(true);
  }

  public void AdvMenuClosed() => this.UpdateGUI();
}
