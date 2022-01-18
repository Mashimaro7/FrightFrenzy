// Decompiled with JetBrains decompiler
// Type: WobbleGoal
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class WobbleGoal : BHelperData
{
  public bool is_red;
  public bool endgame_started;
  public bool auto_inplay;
  public bool valid;
  public bool valid_old;
  public int robot_scored = -1;
  public RobotID last_contact_robot;
  private Renderer[] renderers;
  private bool valid_color = true;
  private bool last_color;

  private void Start() => this.renderers = this.gameObject.GetComponentsInChildren<Renderer>();

  public void Reset()
  {
    this.endgame_started = false;
    this.auto_inplay = false;
    this.valid = false;
    this.robot_scored = -1;
  }

  private void OnCollisionEnter(Collision collision)
  {
    RobotID componentInParent = collision.gameObject.GetComponentInParent<RobotID>();
    if (!(bool) (Object) componentInParent)
      return;
    this.last_contact_robot = componentInParent;
  }

  private void Update()
  {
    if (!GLOBALS.CLIENT_MODE)
      this.valid_color = this.valid || !this.endgame_started && !this.auto_inplay;
    if (this.valid_color == this.last_color)
      return;
    foreach (Renderer renderer in this.renderers)
    {
      Color color = renderer.material.color;
      if (this.valid_color)
      {
        color.r = (double) color.r > 0.449999988079071 ? 1f : 0.0f;
        color.g = (double) color.g > 0.449999988079071 ? 1f : 0.0f;
        color.b = (double) color.b > 0.449999988079071 ? 1f : 0.0f;
      }
      else
      {
        color.r = (double) color.r > 0.449999988079071 ? 0.5f : 0.2f;
        color.g = (double) color.g > 0.449999988079071 ? 0.5f : 0.2f;
        color.b = (double) color.b > 0.449999988079071 ? 0.5f : 0.2f;
      }
      renderer.material.color = color;
      this.last_color = this.valid_color;
    }
  }

  public override string GetString() => !this.valid_color ? "0" : "1";

  public override void SetString(string indata) => this.valid_color = indata[0] == '1';
}
