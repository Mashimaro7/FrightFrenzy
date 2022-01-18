// Decompiled with JetBrains decompiler
// Type: Robot_Spectator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class Robot_Spectator : RobotInterface3D
{
  public bool disable_motion;
  private double my_z_rotation;
  private double my_y_rotation;

  public void Awake() => this.info = "<Missing Robot Specific Function: TBD>" + this.info;

  public override void Update_Robot()
  {
    if (this.disable_motion)
      return;
    float num1 = this.gamepad1_left_stick_y;
    float num2 = this.gamepad1_left_stick_x;
    if ((double) Math.Abs(num1) < 0.07)
      num1 = 0.0f;
    if ((double) Math.Abs(num2) < 0.07)
      num2 = 0.0f;
    float num3 = this.gamepad1_right_stick_y;
    float num4 = this.gamepad1_right_stick_x;
    if ((double) Math.Abs(num3) < 0.07)
      num3 = 0.0f;
    if ((double) Math.Abs(num4) < 0.07)
      num4 = 0.0f;
    this.rb_body.transform.localRotation = Quaternion.Euler(0.0f, (float) this.my_y_rotation, 0.0f);
    Vector3 translation = new Vector3(0.0f, 0.0f, 0.0f);
    translation.x += (float) (-1.0 * (double) num1 * (double) Time.deltaTime * 5.0);
    translation.z += (float) (-1.0 * (double) num2 * (double) Time.deltaTime * 5.0);
    this.rb_body.transform.Translate(translation, Space.Self);
    if (this.gamepad1_dpad_up || this.gamepad1_dpad_down)
    {
      translation.x = 0.0f;
      translation.z = 0.0f;
      translation.y = Time.deltaTime * 2f;
      if (this.gamepad1_dpad_down)
        translation.y *= -1f;
      this.rb_body.transform.position = this.rb_body.transform.position + translation;
    }
    Vector3 position = this.rb_body.transform.position;
    if ((double) position.y > 10.0)
      position.y = 10f;
    if ((double) position.y < 0.0)
      position.y = 0.0f;
    if ((double) position.x > 10.0)
      position.x = 10f;
    if ((double) position.x < -10.0)
      position.x = -10f;
    if ((double) position.z > 12.0)
      position.z = 12f;
    if ((double) position.z < -12.0)
      position.z = -12f;
    this.rb_body.transform.position = position;
    this.my_z_rotation += (double) num3 * (double) Time.deltaTime * 150.0;
    this.my_y_rotation += (double) num4 * (double) Time.deltaTime * 150.0;
    this.my_y_rotation = MyUtils.AngleWrap(this.my_y_rotation);
    if (this.my_z_rotation < -90.0)
      this.my_z_rotation = -90.0;
    if (this.my_z_rotation > 90.0)
      this.my_z_rotation = 90.0;
    this.rb_body.transform.localRotation = Quaternion.Euler(0.0f, (float) this.my_y_rotation, (float) this.my_z_rotation * -1f);
    KeyCode keyCode = KeyCode.None;
    for (KeyCode key = KeyCode.Alpha0; key <= KeyCode.Alpha9; ++key)
    {
      if (Input.GetKey(key))
      {
        keyCode = key;
        break;
      }
    }
    if (this.gamepad1_a && keyCode != KeyCode.None)
    {
      GLOBALS.GENERIC_DATA["SPEC" + keyCode.ToString()] = this.rb_body.transform.localPosition.x.ToString() + ":" + (object) this.rb_body.transform.localPosition.y + ":" + (object) this.rb_body.transform.localPosition.z + ":" + (object) this.my_z_rotation + ":" + (object) this.my_y_rotation;
      Settings.SavePrefs();
    }
    else
    {
      if (keyCode == KeyCode.None || !GLOBALS.GENERIC_DATA.ContainsKey("SPEC" + keyCode.ToString()))
        return;
      string[] strArray1 = GLOBALS.GENERIC_DATA["SPEC" + keyCode.ToString()].Split(':');
      if (strArray1.Length < 5)
        return;
      int num5 = 0;
      Vector3 vector3;
      ref Vector3 local1 = ref vector3;
      string[] strArray2 = strArray1;
      int index1 = num5;
      int num6 = index1 + 1;
      double num7 = (double) float.Parse(strArray2[index1]);
      local1.x = (float) num7;
      ref Vector3 local2 = ref vector3;
      string[] strArray3 = strArray1;
      int index2 = num6;
      int num8 = index2 + 1;
      double num9 = (double) float.Parse(strArray3[index2]);
      local2.y = (float) num9;
      ref Vector3 local3 = ref vector3;
      string[] strArray4 = strArray1;
      int index3 = num8;
      int num10 = index3 + 1;
      double num11 = (double) float.Parse(strArray4[index3]);
      local3.z = (float) num11;
      string[] strArray5 = strArray1;
      int index4 = num10;
      int num12 = index4 + 1;
      this.my_z_rotation = double.Parse(strArray5[index4]);
      string[] strArray6 = strArray1;
      int index5 = num12;
      int num13 = index5 + 1;
      this.my_y_rotation = double.Parse(strArray6[index5]);
      this.rb_body.transform.localPosition = vector3;
      this.rb_body.transform.localRotation = Quaternion.Euler(0.0f, (float) this.my_y_rotation, (float) this.my_z_rotation * -1f);
    }
  }

  public override void UpdateMovement()
  {
  }

  public override void Start()
  {
    this.isSpectator = true;
    base.Start();
    this.SetKinematic();
  }
}
