// Decompiled with JetBrains decompiler
// Type: Ball_Visuals
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class Ball_Visuals : BHelperData
{
  public ball_data balldata;
  private MeshRenderer[] my_renderers;
  private Color starting_color;
  public int old_ball_highlite;
  public int brightness_setting = 100;

  private void Start()
  {
    this.balldata = this.GetComponent<ball_data>();
    this.my_renderers = this.GetComponentsInChildren<MeshRenderer>();
    this.starting_color = this.my_renderers[0].material.color;
  }

  public override string GetString() => this.brightness_setting.ToString();

  public override void SetString(string indata) => int.TryParse(indata, out this.brightness_setting);

  private void Update()
  {
    if (this.my_renderers.Length == 0)
      return;
    if (!GLOBALS.CLIENT_MODE)
      this.brightness_setting = !this.balldata.IsFlagSet("Invalid") ? 100 : 30;
    if (this.old_ball_highlite == this.brightness_setting)
      return;
    this.old_ball_highlite = this.brightness_setting;
    Color color = this.starting_color * (float) this.brightness_setting / 100f;
    foreach (MeshRenderer renderer in this.my_renderers)
    {
      if ((bool) (Object) renderer.material)
        renderer.material.color = color;
    }
  }
}
