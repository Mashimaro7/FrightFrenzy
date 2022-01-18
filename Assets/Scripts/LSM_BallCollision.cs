// Decompiled with JetBrains decompiler
// Type: LSM_BallCollision
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class LSM_BallCollision : BHelperData
{
  public ball_data balldata;
  public long highlite_speed = 175;
  private Material my_material;
  private LastManStanding_Settings lms_settings;
  private bool old_ball_highlite;
  public int highlite_color = 3;

  private void Start()
  {
    this.balldata = this.GetComponent<ball_data>();
    this.my_material = this.GetComponent<MeshRenderer>().material;
    this.highlite_speed = (long) ((double) this.highlite_speed * ((double) Random.value * 0.200000002980232 - 0.100000001490116 + 1.0));
    GameObject gameObject = GameObject.Find("GameSettings");
    if (!(bool) (Object) gameObject)
      return;
    this.lms_settings = gameObject.GetComponent<LastManStanding_Settings>();
  }

  public override string GetString() => this.highlite_color.ToString();

  public override void SetString(string indata) => int.TryParse(indata, out this.highlite_color);

  private void Update()
  {
    if (!(bool) (Object) this.my_material)
      return;
    if (!GLOBALS.CLIENT_MODE)
    {
      bool flag = this.balldata.thrown_by_id > 0;
      if (flag && !this.old_ball_highlite)
      {
        if (!(bool) (Object) this.lms_settings)
          this.lms_settings = GameObject.Find("GameSettings").GetComponent<LastManStanding_Settings>();
        this.highlite_color = !this.lms_settings.FREE_FOR_ALL ? (!this.balldata.thrown_robotid.is_red ? 2 : 1) : 3;
      }
      else if (!flag)
        this.highlite_color = 0;
      this.old_ball_highlite = flag;
    }
    float num = (float) (MyUtils.GetTimeMillis() % this.highlite_speed) / (float) this.highlite_speed;
    if (this.highlite_color == 1)
      this.my_material.color = new Color(2f, (float) (0.5 * (double) num + 0.5), (float) (0.5 * (double) num + 0.5), 1f);
    else if (this.highlite_color == 2)
      this.my_material.color = new Color((float) (0.5 * (double) num + 0.5), (float) (0.5 * (double) num + 0.5), 2f, 1f);
    else if (this.highlite_color == 3)
      this.my_material.color = new Color(num + 0.5f, num + 0.5f, num + 0.5f, 1f);
    else
      this.my_material.color = Color.yellow;
  }

  private void OnCollisionEnter(Collision collision)
  {
    if (this.balldata.thrown_by_id < 1)
      return;
    RobotID componentInParent = collision.transform.GetComponentInParent<RobotID>();
    if (!(bool) (Object) componentInParent)
    {
      this.balldata.thrown_by_id = -1;
      this.balldata.thrown_robotid = (RobotID) null;
    }
    else
    {
      if (this.balldata.thrown_by_id == componentInParent.id)
        return;
      if (this.lms_settings.FREE_FOR_ALL || this.balldata.thrown_robotid.is_red != componentInParent.is_red)
      {
        componentInParent.SetUserInt("HITS", componentInParent.GetUserInt("HITS") + 1);
        componentInParent.SetUserInt("LAST_HIT_ID", this.balldata.thrown_robotid.id);
        this.balldata.thrown_robotid.SetUserFloat("KILLS", this.balldata.thrown_robotid.GetUserFloat("KILLS") + 1f / (float) this.lms_settings.HIT_COUNT);
        this.GetComponent<AudioManager>().Play("HitOpponent", volume: 1f);
      }
      else
        this.GetComponent<AudioManager>().Play("ballhit1", volume: 1f);
      this.balldata.thrown_by_id = -1;
      this.balldata.thrown_robotid = (RobotID) null;
    }
  }

  public void DeactiveBall()
  {
    this.balldata.thrown_by_id = -1;
    this.balldata.thrown_robotid = (RobotID) null;
  }
}
