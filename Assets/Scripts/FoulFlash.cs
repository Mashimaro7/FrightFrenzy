// Decompiled with JetBrains decompiler
// Type: FoulFlash
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class FoulFlash : MonoBehaviour
{
  public long start_time;
  public float rot_speed = 720f;
  public long duration = 500;
  public float y_add = 2f;
  public float y_initial;

  private void Start()
  {
    this.start_time = MyUtils.GetTimeMillis();
    this.y_initial = this.transform.position.y;
  }

  private void Update()
  {
    long num1 = MyUtils.GetTimeMillis() - this.start_time;
    if (num1 < this.duration)
    {
      float f = (float) (2.0 * (double) num1 / (double) this.duration - 1.0);
      float num2 = 1f - Mathf.Abs(f);
      this.transform.position = this.transform.position with
      {
        y = this.y_initial + (float) (0.5 * ((double) f + 1.0)) * this.y_add
      };
      this.transform.localScale = this.transform.localScale with
      {
        y = num2
      };
      this.transform.localRotation = Quaternion.Euler(this.transform.localRotation.eulerAngles with
      {
        y = (float) ((double) this.rot_speed * (double) num1 / 1000.0)
      });
    }
    else
    {
      this.gameObject.SetActive(false);
      Object.Destroy((Object) this.gameObject);
    }
  }
}
