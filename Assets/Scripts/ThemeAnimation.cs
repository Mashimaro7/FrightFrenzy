// Decompiled with JetBrains decompiler
// Type: ThemeAnimation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class ThemeAnimation : MonoBehaviour
{
  public bool animation_started;
  public bool animation_finished;
  public float multiplier = 1f;

  private void Start()
  {
  }

  private void Update()
  {
  }

  private void LateUpdate()
  {
    if ((double) this.transform.localPosition.y != 0.0)
    {
      Vector3 localPosition = this.transform.localPosition;
      localPosition.y *= this.multiplier;
      this.transform.localPosition = localPosition;
    }
    if (!this.animation_finished || !this.animation_started)
      return;
    this.animation_started = false;
    this.animation_finished = false;
    this.transform.parent.localPosition += this.transform.localPosition;
    this.transform.localPosition = Vector3.zero;
    this.multiplier = 1f;
  }

  public void MoveUp(float mymultiplier = 1f)
  {
    this.multiplier = mymultiplier;
    this.animation_started = true;
    this.animation_finished = false;
    this.GetComponent<Animation>().Play("themeUp");
  }

  public void MoveDown(float mymultiplier = 1f)
  {
    this.multiplier = mymultiplier;
    this.animation_started = true;
    this.animation_finished = false;
    this.GetComponent<Animation>().Play("themeDown");
  }
}
