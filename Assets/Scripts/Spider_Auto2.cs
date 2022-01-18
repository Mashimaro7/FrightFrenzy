// Decompiled with JetBrains decompiler
// Type: Spider_Auto2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class Spider_Auto2 : MonoBehaviour
{
  private Animation theanimator;
  private float rand_wait;
  private System.Random random = new System.Random();
  private long exit_time;
  private bool waiting;

  private void Start() => this.theanimator = this.GetComponent<Animation>();

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
      switch (this.random.Next(1, 12))
      {
        case 1:
          this.theanimator.Play("run");
          break;
        case 2:
          this.theanimator.Play("attack1");
          break;
        case 3:
          this.theanimator.Play("attack2");
          break;
        case 4:
          this.theanimator.Play("taunt");
          break;
        case 5:
          this.theanimator.Play("hit1");
          break;
        case 6:
          this.theanimator.Play("hit2");
          break;
        case 7:
          this.theanimator.Play("jump");
          break;
        case 8:
          this.theanimator.Play("death1");
          break;
        case 9:
          this.theanimator.Play("death2");
          break;
        default:
          this.theanimator.Play("walk");
          this.exit_time = timeMillis + 4000L;
          break;
      }
    }
  }
}
