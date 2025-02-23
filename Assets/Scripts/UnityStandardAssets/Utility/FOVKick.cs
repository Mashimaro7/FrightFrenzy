﻿// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets.Utility.FOVKick
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

namespace UnityStandardAssets.Utility
{
  [Serializable]
  public class FOVKick
  {
    public Camera Camera;
    [HideInInspector]
    public float originalFov;
    public float FOVIncrease = 3f;
    public float TimeToIncrease = 1f;
    public float TimeToDecrease = 1f;
    public AnimationCurve IncreaseCurve;

    public void Setup(Camera camera)
    {
      this.CheckStatus(camera);
      this.Camera = camera;
      this.originalFov = camera.fieldOfView;
    }

    private void CheckStatus(Camera camera)
    {
      if ((UnityEngine.Object) camera == (UnityEngine.Object) null)
        throw new Exception("FOVKick camera is null, please supply the camera to the constructor");
      if (this.IncreaseCurve == null)
        throw new Exception("FOVKick Increase curve is null, please define the curve for the field of view kicks");
    }

    public void ChangeCamera(Camera camera) => this.Camera = camera;

    public IEnumerator FOVKickUp()
    {
      float t = Mathf.Abs((this.Camera.fieldOfView - this.originalFov) / this.FOVIncrease);
      while ((double) t < (double) this.TimeToIncrease)
      {
        this.Camera.fieldOfView = this.originalFov + this.IncreaseCurve.Evaluate(t / this.TimeToIncrease) * this.FOVIncrease;
        t += Time.deltaTime;
        yield return (object) new WaitForEndOfFrame();
      }
    }

    public IEnumerator FOVKickDown()
    {
      float t = Mathf.Abs((this.Camera.fieldOfView - this.originalFov) / this.FOVIncrease);
      while ((double) t > 0.0)
      {
        this.Camera.fieldOfView = this.originalFov + this.IncreaseCurve.Evaluate(t / this.TimeToDecrease) * this.FOVIncrease;
        t -= Time.deltaTime;
        yield return (object) new WaitForEndOfFrame();
      }
      this.Camera.fieldOfView = this.originalFov;
    }
  }
}
