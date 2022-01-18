// Decompiled with JetBrains decompiler
// Type: interpolation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class interpolation : MonoBehaviour
{
  public bool disable;
  public bool disable2;
  public bool do_not_push;
  public Vector3 lastPosition;
  public Vector3 velocity;
  public Vector3 lastRotation;
  public Vector3 angularVelocity;
  public float time_last_v;
  public float time_last_a;
  public float my_time_last_v;
  public float my_delta_time_last_v;
  public float my_time_last_a;
  public float my_delta_time_last_a;
  public bool reset_delta_time = true;
  public bool pauseUpdates;
  private bool useLocalsv;
  private bool useLocalsa;
  public Vector3 calculated_pos;
  public Vector3 calculated_angle;
  private bool initialized;
  public Vector3 delta_angle_found;

  private void Start()
  {
    this.initialized = true;
    this.pauseUpdates = false;
  }

  private void SetPosInternal(Vector3 newPos, int timestamp = -1)
  {
    float num1 = (float) timestamp / 1000f;
    if (timestamp < 0)
      num1 = Time.time;
    float num2 = num1 - this.time_last_v;
    this.time_last_v = num1;
    this.my_time_last_v = Time.time;
    this.my_delta_time_last_v = 0.0f;
    if ((double) num2 < 0.0)
    {
      this.lastPosition = newPos;
      this.reset_delta_time = true;
    }
    else
    {
      if ((double) num2 < 1.0 / 1000.0)
        num2 = 1f / 1000f;
      this.velocity = (newPos - this.lastPosition) / num2;
      if (!GLOBALS.INTERPOLATE || this.disable || this.disable2)
        this.velocity *= 0.0f;
      this.lastPosition = newPos;
      this.Update();
    }
  }

  private void SetRotInternal(Quaternion newRot, int timestamp = -1) => this.SetRotInternal(newRot.eulerAngles, timestamp);

  private void SetRotInternal(Vector3 newRot, int timestamp = -1)
  {
    float num1 = (float) timestamp / 1000f;
    if (timestamp < 0)
      num1 = Time.time;
    float num2 = num1 - this.time_last_a;
    this.time_last_a = num1;
    this.my_time_last_a = Time.time;
    this.my_delta_time_last_a = 0.0f;
    if ((double) num2 < 0.0)
    {
      this.lastRotation = newRot;
      this.reset_delta_time = true;
    }
    else
    {
      if ((double) num2 < 1.0 / 1000.0)
        num2 = 1f / 1000f;
      this.angularVelocity = newRot - this.lastRotation;
      this.angularVelocity.x = MyUtils.AngleWrap(this.angularVelocity.x);
      this.angularVelocity.y = MyUtils.AngleWrap(this.angularVelocity.y);
      this.angularVelocity.z = MyUtils.AngleWrap(this.angularVelocity.z);
      this.delta_angle_found = this.angularVelocity;
      if ((double) Mathf.Abs(this.angularVelocity.x) > 90.0 || (double) Mathf.Abs(this.angularVelocity.y) > 90.0 || (double) Mathf.Abs(this.angularVelocity.z) > 90.0)
        this.angularVelocity = Vector3.zero;
      this.angularVelocity /= num2;
      if (!GLOBALS.INTERPOLATE || this.disable || this.disable2)
        this.angularVelocity = Vector3.zero;
      this.lastRotation = newRot;
      this.Update();
    }
  }

  public void SetPosition(Vector3 newPos, int timestamp = -1)
  {
    this.useLocalsv = false;
    this.SetPosInternal(newPos, timestamp);
  }

  public void SetLocalPosition(Vector3 newPos, int timestamp = -1)
  {
    this.useLocalsv = true;
    this.SetPosInternal(newPos, timestamp);
  }

  public void SetRotation(Vector3 newRot, int timestamp = -1)
  {
    this.useLocalsa = false;
    this.SetRotInternal(newRot, timestamp);
  }

  public void SetLocalRotation(Vector3 newRot, int timestamp = -1)
  {
    this.useLocalsa = true;
    this.SetRotInternal(newRot, timestamp);
  }

  private void Update()
  {
    if (!this.initialized)
      return;
    this.UpdatePos();
    if (this.do_not_push || this.pauseUpdates)
      return;
    if (!this.useLocalsv)
      this.transform.position = this.calculated_pos;
    else
      this.transform.localPosition = this.calculated_pos;
    if (!this.useLocalsa)
      this.transform.eulerAngles = this.calculated_angle;
    else
      this.transform.localEulerAngles = this.calculated_angle;
  }

  public void DoUpdate() => this.Update();

  public void UpdatePos()
  {
    if (!GLOBALS.now_paused)
    {
      this.my_delta_time_last_v += Time.time - this.my_time_last_v;
      this.my_delta_time_last_a += Time.time - this.my_time_last_a;
    }
    float num = 1f;
    if (GLOBALS.now_playing)
      num = GLOBALS.playback_speed;
    this.my_time_last_v = Time.time;
    this.my_time_last_a = Time.time;
    this.calculated_pos = this.lastPosition + this.velocity * this.my_delta_time_last_v * num;
    this.calculated_angle = this.lastRotation + this.angularVelocity * this.my_delta_time_last_a * num;
  }

  public void StopMovement()
  {
    this.velocity = Vector3.zero;
    this.angularVelocity = Vector3.zero;
    this.time_last_v = 0.0f;
    this.time_last_a = 0.0f;
  }
}
