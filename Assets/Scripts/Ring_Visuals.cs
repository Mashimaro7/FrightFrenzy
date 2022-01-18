// Decompiled with JetBrains decompiler
// Type: Ring_Visuals
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class Ring_Visuals : BHelperData
{
  public ball_data balldata;
  public long highlite_speed = 175;
  private MeshRenderer[] my_renderers;
  public int old_ball_highlite;
  public int highlite_color;

  private void Start()
  {
    this.balldata = this.GetComponent<ball_data>();
    this.my_renderers = this.GetComponentsInChildren<MeshRenderer>();
    this.highlite_speed = (long) ((double) this.highlite_speed * ((double) Random.value * 0.200000002980232 - 0.100000001490116 + 1.0));
  }

  public override string GetString() => this.highlite_color.ToString();

  public override void SetString(string indata) => int.TryParse(indata, out this.highlite_color);

  private void Update()
  {
    if (this.my_renderers.Length == 0)
      return;
    if (!GLOBALS.CLIENT_MODE)
      this.highlite_color = this.balldata.thrown_by_id <= 0 || !this.balldata.flags.ContainsKey("Bad") ? 0 : 1;
    if (this.old_ball_highlite == 0 && this.highlite_color == 0)
      return;
    this.old_ball_highlite = this.highlite_color;
    float num = (float) (MyUtils.GetTimeMillis() % this.highlite_speed) / (float) this.highlite_speed;
    Color color = new Color(1f, 0.254f, 0.0f);
    if (this.highlite_color == 1)
    {
      color.r = num + 0.5f;
      color.g = num + 0.5f;
      color.b = num + 0.5f;
    }
    foreach (MeshRenderer renderer in this.my_renderers)
    {
      if ((bool) (Object) renderer.material)
        renderer.material.color = color;
    }
  }

  private void OnCollisionEnter(Collision collision)
  {
    if (this.balldata.thrown_by_id < 1 || collision.transform.tag == "FieldStructure")
      return;
    RobotID componentInParent = collision.transform.GetComponentInParent<RobotID>();
    if ((Object) componentInParent != (Object) null && this.balldata.thrown_by_id == componentInParent.id)
      return;
    this.balldata.thrown_by_id = -1;
    this.balldata.thrown_robotid = (RobotID) null;
    this.balldata.flags.Clear();
    this.GetComponent<AudioManager>().Play("ballhit1", volume: 1f);
  }

  public void DeactiveBall()
  {
    this.balldata.thrown_by_id = -1;
    this.balldata.thrown_robotid = (RobotID) null;
    this.balldata.flags.Clear();
  }
}
