// Decompiled with JetBrains decompiler
// Type: JoystickAdvancedMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.UI;

public class JoystickAdvancedMenu : MonoBehaviour
{
  public Dropdown joystic_select;
  public Dropdown axis_select;
  public InputField axis_leftup;
  public InputField axis_rightdown;
  public InputField axis_deadzone;
  public Text axis_reading;
  public Dropdown button_leftup;
  public Dropdown button_rightdown;
  public Text joystick_data_out;
  public GameObject buttons_settings;
  public GameObject axis_settings;
  public Text button_note;
  public Text threshold;
  public GameObject threshold_go;
  public bool isButton;
  public JoystickRawInfo joydata;
  public JoystickInfo callback;
  private bool blank_menuchanged;

  private void Update()
  {
    if (!this.gameObject.activeSelf)
      return;
    this.joystick_data_out.text = "";
    for (int index1 = 1; index1 <= 6; ++index1)
    {
      for (int index2 = 1; index2 <= 15; ++index2)
      {
        if ((double) Math.Abs(Input.GetAxis("J" + (object) index1 + "Axis" + (object) index2)) > 0.0500000007450581)
        {
          Text joystickDataOut = this.joystick_data_out;
          joystickDataOut.text = joystickDataOut.text + "Joy " + (object) index1 + " Axis " + (object) index2 + "=" + Input.GetAxis("J" + (object) index1 + "Axis" + (object) index2).ToString("n2") + "\n";
        }
      }
      for (int index3 = 0; index3 <= 19; ++index3)
      {
        if (Input.GetKey("joystick " + (object) index1 + " button " + (object) index3))
        {
          Text joystickDataOut = this.joystick_data_out;
          joystickDataOut.text = joystickDataOut.text + "Joy " + (object) index1 + " Button " + (object) index3 + "\n";
        }
      }
    }
    int num1 = this.joystic_select.value;
    int num2 = this.axis_select.value;
    float num3 = 0.0f;
    if (num2 > 0)
    {
      num3 = Input.GetAxis("J" + (object) num1 + "Axis" + (object) num2);
      this.axis_reading.text = num3.ToString("n2");
    }
    string str = "joystick ";
    if (num1 > 0)
      str = str + (object) num1 + " ";
    if (Input.GetKey(str + "button " + (object) this.button_leftup.value))
      this.button_leftup.colors = this.button_leftup.colors with
      {
        normalColor = Color.green
      };
    else
      this.button_leftup.colors = this.button_leftup.colors with
      {
        normalColor = Color.white
      };
    if (Input.GetKey(str + "button " + (object) this.button_rightdown.value))
      this.button_rightdown.colors = this.button_rightdown.colors with
      {
        normalColor = Color.green
      };
    else
      this.button_rightdown.colors = this.button_rightdown.colors with
      {
        normalColor = Color.white
      };
    if (this.isButton && num2 > 0)
    {
      float result1 = 0.0f;
      float result2 = 0.0f;
      float result3 = 0.0f;
      float.TryParse(this.axis_leftup.text, out result1);
      float.TryParse(this.axis_rightdown.text, out result2);
      float.TryParse(this.axis_deadzone.text, out result3);
      float buttonThreshold = JoystickRawInfo.GetButtonThreshold(result1, result2, result3);
      this.threshold.text = buttonThreshold.ToString("N2");
      if ((double) result1 < (double) buttonThreshold && (double) num3 < (double) buttonThreshold || (double) result1 > (double) buttonThreshold && (double) num3 > (double) buttonThreshold)
        this.axis_reading.color = Color.green;
      else
        this.axis_reading.color = Color.white;
    }
    else
      this.axis_reading.color = Color.white;
  }

  private void Start() => this.MenuChanged();

  public void MenuChanged()
  {
    if (this.blank_menuchanged)
      return;
    if (this.axis_select.value == 0)
    {
      this.axis_settings.SetActive(false);
      this.buttons_settings.SetActive(true);
    }
    else
    {
      this.axis_settings.SetActive(true);
      this.buttons_settings.SetActive(false);
    }
    if (this.isButton)
    {
      this.button_rightdown.gameObject.SetActive(false);
      this.button_note.gameObject.SetActive(true);
      this.threshold_go.SetActive(true);
    }
    else
    {
      this.button_rightdown.gameObject.SetActive(true);
      this.button_note.gameObject.SetActive(false);
      this.threshold_go.SetActive(false);
    }
    if (this.joydata == null)
      return;
    this.joydata.joystick_num = this.joystic_select.value;
    this.joydata.axis = this.axis_select.value;
    float result1 = 0.0f;
    float result2 = 0.0f;
    float result3 = 0.0f;
    if (float.TryParse(this.axis_leftup.text, out result1))
      this.joydata.leftup_value = result1;
    if (float.TryParse(this.axis_rightdown.text, out result2))
      this.joydata.rightdown_value = result2;
    if (float.TryParse(this.axis_deadzone.text, out result3))
      this.joydata.dead_zone = result3;
    this.joydata.button_lu_num = this.button_leftup.value;
    this.joydata.button_rd_num = this.button_rightdown.value;
  }

  public void Close()
  {
    this.joydata = (JoystickRawInfo) null;
    this.gameObject.SetActive(false);
    if (!((UnityEngine.Object) this.callback != (UnityEngine.Object) null))
      return;
    this.callback.AdvMenuClosed();
    this.callback = (JoystickInfo) null;
  }

  public void Init(JoystickRawInfo joydata_in, JoystickInfo callback_in)
  {
    this.blank_menuchanged = true;
    this.joydata = joydata_in;
    this.callback = callback_in;
    this.isButton = this.joydata.isButton;
    this.joystic_select.value = this.joydata.joystick_num;
    this.axis_select.value = this.joydata.axis;
    this.axis_leftup.text = this.joydata.leftup_value.ToString("N2");
    this.axis_rightdown.text = this.joydata.rightdown_value.ToString("N2");
    this.axis_deadzone.text = this.joydata.dead_zone.ToString("N2");
    this.button_leftup.value = this.joydata.button_lu_num;
    this.button_rightdown.value = this.joydata.button_rd_num;
    this.blank_menuchanged = false;
    this.MenuChanged();
  }
}
