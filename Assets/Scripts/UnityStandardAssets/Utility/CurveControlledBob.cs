// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets.Utility.CurveControlledBob
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

namespace UnityStandardAssets.Utility
{
  [Serializable]
  public class CurveControlledBob
  {
    public float HorizontalBobRange = 0.33f;
    public float VerticalBobRange = 0.33f;
    public AnimationCurve Bobcurve = new AnimationCurve(new Keyframe[5]
    {
      new Keyframe(0.0f, 0.0f),
      new Keyframe(0.5f, 1f),
      new Keyframe(1f, 0.0f),
      new Keyframe(1.5f, -1f),
      new Keyframe(2f, 0.0f)
    });
    public float VerticaltoHorizontalRatio = 1f;
    private float m_CyclePositionX;
    private float m_CyclePositionY;
    private float m_BobBaseInterval;
    private Vector3 m_OriginalCameraPosition;
    private float m_Time;

    public void Setup(Camera camera, float bobBaseInterval)
    {
      this.m_BobBaseInterval = bobBaseInterval;
      this.m_OriginalCameraPosition = camera.transform.localPosition;
      this.m_Time = this.Bobcurve[this.Bobcurve.length - 1].time;
    }

    public Vector3 DoHeadBob(float speed)
    {
      double x = (double) this.m_OriginalCameraPosition.x + (double) this.Bobcurve.Evaluate(this.m_CyclePositionX) * (double) this.HorizontalBobRange;
      float num = this.m_OriginalCameraPosition.y + this.Bobcurve.Evaluate(this.m_CyclePositionY) * this.VerticalBobRange;
      this.m_CyclePositionX += speed * Time.deltaTime / this.m_BobBaseInterval;
      this.m_CyclePositionY += speed * Time.deltaTime / this.m_BobBaseInterval * this.VerticaltoHorizontalRatio;
      if ((double) this.m_CyclePositionX > (double) this.m_Time)
        this.m_CyclePositionX -= this.m_Time;
      if ((double) this.m_CyclePositionY > (double) this.m_Time)
        this.m_CyclePositionY -= this.m_Time;
      double y = (double) num;
      return new Vector3((float) x, (float) y, 0.0f);
    }
  }
}
