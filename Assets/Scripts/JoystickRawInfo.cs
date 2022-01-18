// Decompiled with JetBrains decompiler
// Type: JoystickRawInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class JoystickRawInfo
{
  public int joystick_num = 1;
  public bool isButton;
  public int axis = 1;
  public int button_lu_num;
  public int button_rd_num = 1;
  public float leftup_value = -1f;
  public float rightdown_value = 1f;
  public float dead_zone = 0.05f;
  public string info;

  public JoystickRawInfo(
    string info_in = "",
    int joystick_num_in = 0,
    bool isButton_in = false,
    int axis_in = 1,
    int button_lu_num_in = 0,
    int button_rd_num_in = 1,
    float leftup_value_in = -1f,
    float rightdown_value_in = 1f,
    float dead_zone_in = 0.05f)
  {
    this.info = info_in;
    this.joystick_num = joystick_num_in;
    this.isButton = isButton_in;
    this.axis = axis_in;
    this.button_lu_num = button_lu_num_in;
    this.button_rd_num = button_rd_num_in;
    this.leftup_value = leftup_value_in;
    this.rightdown_value = rightdown_value_in;
    this.dead_zone = dead_zone_in;
  }

  public void Copy(JoystickRawInfo inref)
  {
    this.info = inref.info;
    this.joystick_num = inref.joystick_num;
    this.isButton = inref.isButton;
    this.axis = inref.axis;
    this.button_lu_num = inref.button_lu_num;
    this.button_rd_num = inref.button_rd_num;
    this.leftup_value = inref.leftup_value;
    this.rightdown_value = inref.rightdown_value;
    this.dead_zone = inref.dead_zone;
  }

  public bool GetButton()
  {
    if (this.axis == 0)
    {
      string str = "joystick ";
      if (this.joystick_num > 0)
        str = str + (object) this.joystick_num + " ";
      return Input.GetKey(str + "button " + (object) this.button_lu_num);
    }
    float buttonThreshold = JoystickRawInfo.GetButtonThreshold(this.leftup_value, this.rightdown_value, this.dead_zone);
    float axis = Input.GetAxis("J" + (object) this.joystick_num + "Axis" + (object) this.axis);
    return (double) this.leftup_value < (double) buttonThreshold && (double) axis < (double) buttonThreshold || (double) this.leftup_value > (double) buttonThreshold && (double) axis > (double) buttonThreshold;
  }

  public static float GetButtonThreshold(
    float my_leftup_value,
    float my_rightdown_value,
    float my_dead_zone)
  {
    float num1 = (float) (((double) my_leftup_value + (double) my_rightdown_value) / 2.0);
    float num2 = (double) my_leftup_value >= (double) my_rightdown_value ? num1 + my_dead_zone : num1 - my_dead_zone;
    return (float) (((double) my_leftup_value + (double) num2) / 2.0);
  }

  public float GetAxis()
  {
    if (this.axis == 0)
    {
      string str = "joystick ";
      if (this.joystick_num > 0)
        str = str + (object) this.joystick_num + " ";
      if (Input.GetKey(str + "button " + (object) this.button_lu_num))
        return -1f;
      return Input.GetKey(str + "button " + (object) this.button_rd_num) ? 1f : 0.0f;
    }
    float num1 = (float) (((double) this.rightdown_value + (double) this.leftup_value) / 2.0);
    float num2 = Math.Abs(this.rightdown_value - num1) - this.dead_zone;
    if ((double) num2 == 0.0)
      num2 = 0.0001f;
    float num3 = (double) this.leftup_value > (double) this.rightdown_value ? -1f : 1f;
    float axis1 = Input.GetAxis("J" + (object) this.joystick_num + "Axis" + (object) this.axis);
    if ((double) Math.Abs(axis1 - num1) < (double) this.dead_zone)
      return 0.0f;
    float num4 = axis1 - num1;
    float axis2 = ((double) num4 <= 0.0 ? num4 + this.dead_zone : num4 - this.dead_zone) / (num2 * num3);
    if ((double) axis2 < -1.0)
      axis2 = -1f;
    if ((double) axis2 > 1.0)
      axis2 = 1f;
    return axis2;
  }

  public string GetString() => "v1;" + (object) this.joystick_num + ";" + (this.isButton ? (object) "1" : (object) "0") + ";" + (object) this.axis + ";" + (object) this.button_lu_num + ";" + (object) this.button_rd_num + ";" + (object) this.leftup_value + ";" + (object) this.rightdown_value + ";" + (object) this.dead_zone + ";" + this.info;

  public void FromString(string input)
  {
    string[] strArray1 = input.Split(';');
    if (strArray1.Length < 6)
      return;
    int num1 = 0;
    int num2;
    if (strArray1[0] == "v1")
    {
      int num3 = num1 + 1;
      string[] strArray2 = strArray1;
      int index1 = num3;
      int num4 = index1 + 1;
      this.joystick_num = int.Parse(strArray2[index1]);
      string[] strArray3 = strArray1;
      int index2 = num4;
      int num5 = index2 + 1;
      this.isButton = strArray3[index2] == "1";
      string[] strArray4 = strArray1;
      int index3 = num5;
      int num6 = index3 + 1;
      this.axis = int.Parse(strArray4[index3]);
      string[] strArray5 = strArray1;
      int index4 = num6;
      int num7 = index4 + 1;
      this.button_lu_num = int.Parse(strArray5[index4]);
      string[] strArray6 = strArray1;
      int index5 = num7;
      int num8 = index5 + 1;
      this.button_rd_num = int.Parse(strArray6[index5]);
      string[] strArray7 = strArray1;
      int index6 = num8;
      int num9 = index6 + 1;
      this.leftup_value = float.Parse(strArray7[index6]);
      string[] strArray8 = strArray1;
      int index7 = num9;
      int num10 = index7 + 1;
      this.rightdown_value = float.Parse(strArray8[index7]);
      string[] strArray9 = strArray1;
      int index8 = num10;
      int num11 = index8 + 1;
      this.dead_zone = float.Parse(strArray9[index8]);
      string[] strArray10 = strArray1;
      int index9 = num11;
      num2 = index9 + 1;
      this.info = strArray10[index9];
    }
    else
    {
      string[] strArray11 = strArray1;
      int index10 = num1;
      int num12 = index10 + 1;
      bool flag = int.Parse(strArray11[index10]) > 0;
      string[] strArray12 = strArray1;
      int index11 = num12;
      int num13 = index11 + 1;
      this.joystick_num = int.Parse(strArray12[index11]);
      string[] strArray13 = strArray1;
      int index12 = num13;
      int num14 = index12 + 1;
      this.button_lu_num = int.Parse(strArray13[index12]);
      string[] strArray14 = strArray1;
      int index13 = num14;
      int num15 = index13 + 1;
      this.axis = int.Parse(strArray14[index13]);
      string[] strArray15 = strArray1;
      int index14 = num15;
      int num16 = index14 + 1;
      int num17 = int.Parse(strArray15[index14]) > 0 ? 1 : 0;
      string[] strArray16 = strArray1;
      int index15 = num16;
      num2 = index15 + 1;
      this.info = strArray16[index15];
      if (flag)
        this.axis = 0;
      if (num17 != 0)
      {
        this.leftup_value = 1f;
        this.rightdown_value = -1f;
        this.dead_zone = 0.1f;
      }
      else
      {
        this.leftup_value = -1f;
        this.rightdown_value = 1f;
        this.dead_zone = 0.1f;
      }
    }
  }

  public bool isInverted() => (double) this.leftup_value > (double) this.rightdown_value;
}
