// Decompiled with JetBrains decompiler
// Type: DriveTrainCalcs
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DriveTrainCalcs : MonoBehaviour
{
  public Transform T_Drivetrain;

  private void Start() => this.UpdateAllCalcs();

  public void UpdateAllCalcs()
  {
    if ((Object) this.T_Drivetrain.Find("MotorType/Dropdown") == (Object) null)
      return;
    int num1 = this.T_Drivetrain.Find("MotorType/Dropdown").GetComponent<Dropdown>().value;
    string text = this.T_Drivetrain.Find("MotorType/Dropdown").GetComponent<Dropdown>().options[num1].text;
    float result1 = 20f;
    if (!float.TryParse(this.T_Drivetrain.Find("GearRatio").GetComponent<InputField>().text, out result1))
      result1 = -1f;
    float result2 = 4f;
    if (!float.TryParse(this.T_Drivetrain.Find("wheelDiameter").GetComponent<InputField>().text, out result2))
      result2 = -1f;
    float result3 = 42f;
    if (!float.TryParse(this.T_Drivetrain.Find("Weight").GetComponent<InputField>().text, out result3))
      result3 = -1f;
    if ((double) result1 < 5.0)
    {
      this.T_Drivetrain.Find("GearRatio").GetComponent<InputField>().text = "5";
      result1 = 5f;
    }
    if ((double) result1 > 999.0)
    {
      this.T_Drivetrain.Find("GearRatio").GetComponent<InputField>().text = "999";
      result1 = 999f;
    }
    if ((double) result2 < 1.0)
    {
      this.T_Drivetrain.Find("wheelDiameter").GetComponent<InputField>().text = "1";
      result2 = 15f;
    }
    if ((double) result2 > 9.0)
    {
      this.T_Drivetrain.Find("wheelDiameter").GetComponent<InputField>().text = "9";
      result2 = 9f;
    }
    if ((double) result3 < 15.0)
    {
      this.T_Drivetrain.Find("Weight").GetComponent<InputField>().text = "15";
      result3 = 15f;
    }
    if ((double) result3 > 42.0)
    {
      this.T_Drivetrain.Find("Weight").GetComponent<InputField>().text = "42";
      result3 = 42f;
    }
    float speed;
    float acc;
    DriveTrainCalcs.CalcDriveTrain(result2, result1, num1, result3, out speed, out acc);
    GLOBALS.speed = speed;
    GLOBALS.acceleration = acc;
    float num2 = speed * (acc / (acc + GLOBALS.friction));
    this.T_Drivetrain.Find("Speed").GetComponent<InputField>().text = num2.ToString("0.#");
    this.T_Drivetrain.Find("Acceleration").GetComponent<InputField>().text = acc.ToString("0.#");
    List<float> xvalueList = new List<float>();
    List<float> yvalueList = new List<float>();
    xvalueList.Add(0.0f);
    yvalueList.Add(0.0f);
    for (float num3 = 0.0f; (double) num3 <= 2.09999990463257; num3 += 0.05f)
    {
      xvalueList.Add(num3);
      float num4 = num2 * ((Mathf.Exp(-acc * num3 / num2) - 1f) * num2 / acc + num3);
      yvalueList.Add(num4);
    }
    this.T_Drivetrain.Find("Graph").GetComponent<Graph>().UpdateGraph(xvalueList, yvalueList, 1.8f, 6f);
  }

  public static void CalcDriveTrain(
    float wheel_diameter,
    float gear_ratio,
    int motortypeindex,
    float weight,
    out float speed,
    out float acc)
  {
    float num1;
    float num2;
    switch (motortypeindex)
    {
      case 1:
        num1 = 6000f;
        num2 = 0.1125f;
        break;
      case 2:
        num1 = 6021f;
        num2 = 0.1407f;
        break;
      case 3:
        num1 = 6000f;
        num2 = 0.105f;
        break;
      case 4:
        num1 = 4800f;
        num2 = 0.178f;
        break;
      default:
        num1 = 7200f;
        num2 = 0.208f;
        break;
    }
    speed = (float) ((double) wheel_diameter / 12.0 * 3.14159274101257 * (double) num1 / 60.0) / gear_ratio;
    float num3 = (float) ((double) num2 * (double) gear_ratio / ((double) wheel_diameter * 3.14159274101257 / 39.3700981140137)) * GLOBALS.motor_count;
    acc = (float) ((double) num3 / (double) weight * 2.20461988449097 * 3.28082990646362);
  }
}
