// Decompiled with JetBrains decompiler
// Type: Spider_Auto
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class Spider_Auto : MonoBehaviour
{
  private Animator theanimator;
  private float rand_wait;
  private System.Random random = new System.Random();
  private long exit_time;
  private bool waiting;

  private void Start() => this.theanimator = this.GetComponent<Animator>();

  private void Update()
  {
    long timeMillis = MyUtils.GetTimeMillis();
    if (this.exit_time - timeMillis >= 0L)
      return;
    if (!this.waiting)
    {
      this.waiting = true;
      this.exit_time = timeMillis + (long) (this.random.Next(1, 8) * 1000);
      this.theanimator.Play("idle");
    }
    else
    {
      this.waiting = false;
      this.exit_time = timeMillis + 2000L;
      switch (this.random.Next(1, 4))
      {
        case 1:
          this.theanimator.Play("jump");
          break;
        case 2:
          this.theanimator.Play("attack");
          break;
        case 3:
          this.theanimator.Play("die");
          break;
        default:
          this.theanimator.Play("walk");
          this.exit_time = timeMillis + 4000L;
          break;
      }
    }
  }
}
