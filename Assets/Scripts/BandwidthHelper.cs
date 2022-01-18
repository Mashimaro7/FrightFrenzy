// Decompiled with JetBrains decompiler
// Type: BandwidthHelper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class BandwidthHelper : MonoBehaviour
{
  public int priority = 1;
  public bool do_not_update;
  public bool do_not_rotate;
  public bool do_not_move;
  public bool use_global;
  public bool no_kinematic_interpolation;
  private Rigidbody myrb;
  public interpolation interpolation_child;
  public bool pauseUpdates;
  public float pos_threshold = 0.0001f;
  public float angle_threshold = 0.0001f;
  public bool update_pos_x = true;
  public string update_pos_x_format = "0.####";
  public bool update_pos_y = true;
  public string update_pos_y_format = "0.####";
  public bool update_pos_z = true;
  public string update_pos_z_format = "0.####";
  public bool update_angle_x = true;
  public string update_angle_x_format = "0.####";
  public bool update_angle_y = true;
  public string update_angle_y_format = "0.####";
  public bool update_angle_z = true;
  public string update_angle_z_format = "0.####";
  public Vector3 start_pos = new Vector3(0.0f, 0.0f, 0.0f);
  public Quaternion start_angle;
  public Vector3 delta_pos = new Vector3(0.0f, 0.0f, 0.0f);
  public Quaternion delta_angle;
  public Transform refpos;
  public Quaternion ref_start_angle;
  public BHelperData extra_data;
  public Dictionary<int, long> LastUpdateTime = new Dictionary<int, long>();
  public Dictionary<int, Vector3> delta_pos_old = new Dictionary<int, Vector3>();
  public Dictionary<int, Quaternion> delta_angle_old = new Dictionary<int, Quaternion>();
  public Dictionary<int, string> extra_data_old = new Dictionary<int, string>();
  public Dictionary<int, Vector3> old_velocity = new Dictionary<int, Vector3>();
  public Dictionary<int, Vector3> old_angularVelocity = new Dictionary<int, Vector3>();
  private StringBuilder senddata = new StringBuilder();
  private bool initialized;
  public bool do_update = true;
  public int doingit;
  public int notdoingit;
  private Dictionary<int, int> force_update_d = new Dictionary<int, int>();

  private void OnEnable()
  {
    if (GLOBALS.SINGLEPLAYER_MODE || this.initialized)
      return;
    if (!(bool) (UnityEngine.Object) this.refpos)
    {
      this.start_pos = this.use_global ? this.transform.position : this.transform.localPosition;
      this.start_angle = this.use_global ? this.transform.rotation : this.transform.localRotation;
    }
    else
    {
      this.start_pos = (this.use_global ? this.transform.position : this.transform.localPosition) - (this.use_global ? this.refpos.transform.position : this.refpos.transform.localPosition);
      this.start_angle = this.use_global ? this.transform.rotation : this.transform.localRotation;
      if ((bool) (UnityEngine.Object) this.interpolation_child)
        this.interpolation_child.do_not_push = true;
      this.ref_start_angle = this.use_global ? this.refpos.rotation : this.refpos.localRotation;
    }
    this.initialized = true;
  }

  private void Start()
  {
    this.interpolation_child = this.GetComponent<interpolation>();
    this.myrb = this.GetComponentInChildren<Rigidbody>();
    if ((bool) (UnityEngine.Object) this.refpos && (bool) (UnityEngine.Object) this.interpolation_child)
      this.interpolation_child.do_not_push = true;
    if (!((UnityEngine.Object) this.extra_data == (UnityEngine.Object) null))
      return;
    this.extra_data = this.GetComponent<BHelperData>();
  }

  private void Update()
  {
    if (!GLOBALS.CLIENT_MODE)
      return;
    if (this.pauseUpdates)
    {
      if (!(bool) (UnityEngine.Object) this.interpolation_child)
        return;
      this.interpolation_child.pauseUpdates = true;
    }
    else
    {
      if ((bool) (UnityEngine.Object) this.interpolation_child)
        this.interpolation_child.pauseUpdates = false;
      if (!(bool) (UnityEngine.Object) this.refpos)
        return;
      if ((bool) (UnityEngine.Object) this.refpos.GetComponent<interpolation>())
        this.refpos.GetComponent<interpolation>().DoUpdate();
      if ((bool) (UnityEngine.Object) this.interpolation_child)
      {
        this.delta_pos = this.interpolation_child.calculated_pos;
        this.delta_angle.eulerAngles = this.interpolation_child.calculated_angle;
      }
      if (!this.do_not_move)
      {
        if (this.use_global)
          this.transform.position = this.CalculatePosWithRef(this.delta_pos);
        else
          this.transform.localPosition = this.CalculatePosWithRef(this.delta_pos);
      }
      if (this.do_not_rotate)
        return;
      if (GLOBALS.FORCE_OLD_BHELP)
      {
        if (this.use_global)
          this.transform.rotation = Quaternion.Inverse(this.ref_start_angle) * this.refpos.rotation * this.start_angle * this.delta_angle;
        else
          this.transform.localRotation = Quaternion.Inverse(this.ref_start_angle) * this.refpos.localRotation * this.start_angle * this.delta_angle;
      }
      else if (this.use_global)
        this.transform.rotation = this.refpos.rotation * Quaternion.Inverse(this.ref_start_angle) * this.delta_angle * this.start_angle;
      else
        this.transform.localRotation = this.refpos.localRotation * Quaternion.Inverse(this.ref_start_angle) * this.delta_angle * this.start_angle;
    }
  }

  private void UpdateCalcs()
  {
    if ((UnityEngine.Object) this.refpos == (UnityEngine.Object) null)
    {
      this.delta_pos = (this.use_global ? this.transform.position : this.transform.localPosition) - this.start_pos;
      this.delta_angle = (this.use_global ? this.transform.rotation : this.transform.localRotation) * Quaternion.Inverse(this.start_angle);
    }
    else
    {
      Quaternion quaternion = Quaternion.Inverse((this.use_global ? this.refpos.rotation : this.refpos.localRotation) * Quaternion.Inverse(this.ref_start_angle));
      Vector3 vector3 = (this.use_global ? this.transform.position : this.transform.localPosition) - (this.use_global ? this.refpos.transform.position : this.refpos.transform.localPosition);
      this.delta_pos = quaternion * vector3 - this.start_pos;
      this.delta_angle = quaternion * (this.use_global ? this.transform.rotation : this.transform.localRotation) * Quaternion.Inverse(this.start_angle);
    }
  }

  private void ApplyTransformation(Vector3 new_delta_pos, Vector3 new_delta_angle, int timestamp = -1)
  {
    this.delta_pos = new_delta_pos;
    this.delta_angle = Quaternion.Euler(new_delta_angle);
    Vector3 newPos;
    Quaternion quaternion;
    if ((UnityEngine.Object) this.refpos == (UnityEngine.Object) null)
    {
      newPos = new_delta_pos + this.start_pos;
      quaternion = this.delta_angle * this.start_angle;
    }
    else
    {
      if ((bool) (UnityEngine.Object) this.interpolation_child)
      {
        if (!this.do_not_move)
        {
          if (this.use_global)
            this.interpolation_child.SetPosition(new_delta_pos, timestamp);
          else
            this.interpolation_child.SetLocalPosition(new_delta_pos, timestamp);
        }
        if (this.do_not_rotate)
          return;
        if (this.use_global)
        {
          this.interpolation_child.SetRotation(new_delta_angle, timestamp);
          return;
        }
        this.interpolation_child.SetLocalRotation(new_delta_angle, timestamp);
        return;
      }
      newPos = this.CalculatePosWithRef(new_delta_pos);
      quaternion = Quaternion.Inverse(this.ref_start_angle) * (this.use_global ? this.refpos.rotation : this.refpos.localRotation) * this.delta_angle * this.start_angle;
    }
    if ((bool) (UnityEngine.Object) this.interpolation_child)
    {
      if (!this.do_not_move)
      {
        if (this.use_global)
          this.interpolation_child.SetPosition(newPos, timestamp);
        else
          this.interpolation_child.SetLocalPosition(newPos, timestamp);
      }
      if (this.do_not_rotate)
        return;
      if (this.use_global)
        this.interpolation_child.SetRotation(quaternion.eulerAngles, timestamp);
      else
        this.interpolation_child.SetLocalRotation(quaternion.eulerAngles, timestamp);
    }
    else
    {
      if (!this.do_not_move)
      {
        if (this.use_global)
          this.transform.position = newPos;
        else
          this.transform.localPosition = newPos;
      }
      if (this.do_not_rotate)
        return;
      if (this.use_global)
        this.transform.rotation = quaternion;
      else
        this.transform.localRotation = quaternion;
    }
  }

  private Vector3 CalculatePosWithRef(Vector3 new_delta_pos) => (this.use_global ? this.refpos.rotation : this.refpos.localRotation) * Quaternion.Inverse(this.ref_start_angle) * (this.start_pos + new_delta_pos) + (this.use_global ? this.refpos.position : this.refpos.localPosition);

  public string Get(int cache)
  {
    if (this.do_not_update)
      return "";
    this.UpdateCalcs();
    if (!this.LastUpdateTime.ContainsKey(cache))
      this.LastUpdateTime[cache] = 0L;
    if (!this.delta_pos_old.ContainsKey(cache))
      this.delta_pos_old[cache] = new Vector3(0.0f, 0.0f, 0.0f);
    if (!this.delta_angle_old.ContainsKey(cache))
      this.delta_angle_old[cache] = Quaternion.identity;
    if (!this.extra_data_old.ContainsKey(cache))
      this.extra_data_old[cache] = "";
    if (!this.old_velocity.ContainsKey(cache))
      this.old_velocity[cache] = new Vector3(0.0f, 0.0f, 0.0f);
    if (!this.old_angularVelocity.ContainsKey(cache))
      this.old_angularVelocity[cache] = new Vector3(0.0f, 0.0f, 0.0f);
    if (!this.force_update_d.ContainsKey(cache))
      this.force_update_d[cache] = 4;
    int num1 = this.force_update_d[cache];
    Vector3 vector3_1 = this.delta_pos - this.delta_pos_old[cache];
    Quaternion quaternion = this.delta_angle * Quaternion.Inverse(this.delta_angle_old[cache]);
    Vector3 eulerAngles1 = quaternion.eulerAngles;
    long timeMillis = MyUtils.GetTimeMillis();
    float num2 = (float) (timeMillis - this.LastUpdateTime[cache]) / 1000f;
    Vector3 vector3_2 = this.old_velocity[cache] * num2;
    Vector3 vector3_3 = this.old_angularVelocity[cache] * num2;
    this.do_update = false;
    if (timeMillis - this.LastUpdateTime[cache] > GLOBALS.SERVER_MAX_UPDATE_DELAY)
      this.do_update = true;
    else if (this.update_pos_x && (double) Math.Abs(vector3_1.x) > (double) this.pos_threshold || this.update_pos_y && (double) Math.Abs(vector3_1.y) > (double) this.pos_threshold || this.update_pos_z && (double) Math.Abs(vector3_1.z) > (double) this.pos_threshold)
      this.do_update = true;
    else if (this.update_angle_x && (double) Math.Abs(eulerAngles1.x) > (double) this.angle_threshold || this.update_angle_y && (double) Math.Abs(eulerAngles1.y) > (double) this.angle_threshold || this.update_angle_z && (double) Math.Abs(eulerAngles1.z) > (double) this.angle_threshold)
      this.do_update = true;
    else if ((bool) (UnityEngine.Object) this.extra_data && this.extra_data_old[cache] != this.extra_data.GetString())
      this.do_update = true;
    if (this.update_pos_x && (double) Math.Abs(vector3_2.x) > (double) this.pos_threshold || this.update_pos_y && (double) Math.Abs(vector3_2.y) > (double) this.pos_threshold || this.update_pos_z && (double) Math.Abs(vector3_2.z) > (double) this.pos_threshold)
      this.do_update = true;
    else if (this.update_angle_x && (double) Math.Abs(vector3_3.x) > (double) this.angle_threshold || this.update_angle_y && (double) Math.Abs(vector3_3.y) > (double) this.angle_threshold || this.update_angle_z && (double) Math.Abs(vector3_3.z) > (double) this.angle_threshold)
      this.do_update = true;
    if (!this.do_update && (double) num1 <= 0.0)
    {
      ++this.notdoingit;
      return "";
    }
    int num3 = !this.do_update ? num1 - 1 : 2;
    this.force_update_d[cache] = num3;
    ++this.doingit;
    this.senddata.Clear();
    if (this.update_pos_x)
    {
      if ((double) Math.Abs(this.delta_pos.x) > (double) this.pos_threshold)
        this.senddata.Append(this.delta_pos.x.ToString(this.update_pos_x_format));
      else
        this.senddata.Append(this.delta_pos.x.ToString("0"));
    }
    if (this.update_pos_y)
    {
      if (this.senddata.Length > 0)
        this.senddata.Append('\u0012');
      if ((double) Math.Abs(this.delta_pos.y) > (double) this.pos_threshold)
        this.senddata.Append(this.delta_pos.y.ToString(this.update_pos_y_format));
      else
        this.senddata.Append(this.delta_pos.y.ToString("0"));
    }
    if (this.update_pos_z)
    {
      if (this.senddata.Length > 0)
        this.senddata.Append('\u0012');
      if ((double) Math.Abs(this.delta_pos.z) > (double) this.pos_threshold)
        this.senddata.Append(this.delta_pos.z.ToString(this.update_pos_z_format));
      else
        this.senddata.Append(this.delta_pos.z.ToString("0"));
    }
    Vector3 eulerAngles2;
    if (this.update_angle_x)
    {
      if (this.senddata.Length > 0)
        this.senddata.Append('\u0012');
      if ((double) Math.Abs(this.wrap(this.delta_angle.eulerAngles.x)) > (double) this.angle_threshold)
      {
        StringBuilder senddata = this.senddata;
        eulerAngles2 = this.delta_angle.eulerAngles;
        string str = eulerAngles2.x.ToString(this.update_angle_x_format);
        senddata.Append(str);
      }
      else
      {
        StringBuilder senddata = this.senddata;
        eulerAngles2 = this.delta_angle.eulerAngles;
        string str = eulerAngles2.x.ToString("0");
        senddata.Append(str);
      }
    }
    if (this.update_angle_y)
    {
      if (this.senddata.Length > 0)
        this.senddata.Append('\u0012');
      if ((double) Math.Abs(this.wrap(this.delta_angle.eulerAngles.y)) > (double) this.angle_threshold)
      {
        StringBuilder senddata = this.senddata;
        eulerAngles2 = this.delta_angle.eulerAngles;
        string str = eulerAngles2.y.ToString(this.update_angle_y_format);
        senddata.Append(str);
      }
      else
      {
        StringBuilder senddata = this.senddata;
        eulerAngles2 = this.delta_angle.eulerAngles;
        string str = eulerAngles2.y.ToString("0");
        senddata.Append(str);
      }
    }
    if (this.update_angle_z)
    {
      if (this.senddata.Length > 0)
        this.senddata.Append('\u0012');
      if ((double) Math.Abs(this.wrap(this.delta_angle.eulerAngles.z)) > (double) this.angle_threshold)
      {
        StringBuilder senddata = this.senddata;
        eulerAngles2 = this.delta_angle.eulerAngles;
        string str = eulerAngles2.z.ToString(this.update_angle_z_format);
        senddata.Append(str);
      }
      else
      {
        StringBuilder senddata = this.senddata;
        eulerAngles2 = this.delta_angle.eulerAngles;
        string str = eulerAngles2.z.ToString("0");
        senddata.Append(str);
      }
    }
    if (this.no_kinematic_interpolation)
    {
      if (this.senddata.Length > 0)
        this.senddata.Append('\u0012');
      this.senddata.Append(this.myrb.isKinematic ? "1" : "0");
    }
    if ((bool) (UnityEngine.Object) this.extra_data)
    {
      if (this.senddata.Length > 0)
        this.senddata.Append('\u0012');
      this.extra_data_old[cache] = this.extra_data.GetString();
      this.senddata.Append(this.extra_data_old[cache]);
    }
    this.old_velocity[cache] = (this.delta_pos - this.delta_pos_old[cache]) / num2;
    Vector3 eulerAngles3 = this.delta_angle.eulerAngles;
    quaternion = this.delta_angle_old[cache];
    Vector3 eulerAngles4 = quaternion.eulerAngles;
    Vector3 vector3_4 = eulerAngles3 - eulerAngles4;
    vector3_4.x = MyUtils.AngleWrap(vector3_4.x);
    vector3_4.y = MyUtils.AngleWrap(vector3_4.y);
    vector3_4.z = MyUtils.AngleWrap(vector3_4.z);
    Vector3 vector3_5 = vector3_4 / num2;
    this.old_angularVelocity[cache] = vector3_5;
    this.LastUpdateTime[cache] = timeMillis;
    this.delta_pos_old[cache] = this.delta_pos;
    this.delta_angle_old[cache] = this.delta_angle;
    return this.senddata.ToString();
  }

  public void Set(string data, int timestamp = -1)
  {
    if (this.do_not_update)
      return;
    this.Set(data.Split('\u0012'), 0, timestamp);
  }

  public int Set(string[] getdata, int startpos, int timestamp = -1)
  {
    if (this.do_not_update)
      return ++startpos;
    if (getdata.Length - 1 < startpos || getdata[startpos].Length < 1)
      return startpos + 1;
    Vector3 zero1 = Vector3.zero;
    Vector3 zero2 = Vector3.zero;
    try
    {
      if (this.update_pos_x)
        zero1.x = float.Parse(getdata[startpos++]);
      if (this.update_pos_y)
        zero1.y = float.Parse(getdata[startpos++]);
      if (this.update_pos_z)
        zero1.z = float.Parse(getdata[startpos++]);
      if (this.update_angle_x)
        zero2.x = float.Parse(getdata[startpos++]);
      if (this.update_angle_y)
        zero2.y = float.Parse(getdata[startpos++]);
      if (this.update_angle_z)
        zero2.z = float.Parse(getdata[startpos++]);
      if (this.no_kinematic_interpolation)
      {
        if ((bool) (UnityEngine.Object) this.interpolation_child)
          this.interpolation_child.disable2 = getdata[startpos] == "1";
        ++startpos;
      }
      if ((bool) (UnityEngine.Object) this.extra_data)
        this.extra_data.SetString(getdata[startpos++]);
    }
    catch (Exception ex)
    {
      Debug.LogError((object) ("BHelper: " + ex.ToString()));
      return startpos;
    }
    if (!(bool) (UnityEngine.Object) this.interpolation_child)
    {
      this.interpolation_child = this.GetComponent<interpolation>();
      if ((bool) (UnityEngine.Object) this.interpolation_child && (bool) (UnityEngine.Object) this.refpos)
        this.interpolation_child.do_not_push = true;
    }
    this.ApplyTransformation(zero1, zero2, timestamp);
    return startpos;
  }

  private float wrap(float degrees)
  {
    while ((double) degrees > 180.0)
      degrees -= 360f;
    while ((double) degrees < -180.0)
      degrees += 360f;
    return degrees;
  }
}
