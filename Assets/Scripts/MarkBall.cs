// Decompiled with JetBrains decompiler
// Type: MarkBall
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class MarkBall : MonoBehaviour
{
  public bool disable_in_client = true;
  public bool clear_flags;
  public string add_key = "";
  public string add_value = "";

  private void OnTriggerEnter(Collider collision)
  {
    if (this.disable_in_client && GLOBALS.CLIENT_MODE)
      return;
    ball_data componentInParent = collision.GetComponentInParent<ball_data>();
    if ((Object) componentInParent == (Object) null)
      return;
    if (this.clear_flags)
      componentInParent.flags.Clear();
    if (this.add_key.Length <= 0)
      return;
    componentInParent.flags[this.add_key] = this.add_value;
  }
}
