// Decompiled with JetBrains decompiler
// Type: OVRDisplay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.XR;

public class OVRDisplay
{
  private bool needsConfigureTexture;
  private OVRDisplay.EyeRenderDesc[] eyeDescs = new OVRDisplay.EyeRenderDesc[2];
  private bool recenterRequested;
  private int recenterRequestedFrameCount = int.MaxValue;

  public OVRDisplay() => this.UpdateTextures();

  public void Update()
  {
    this.UpdateTextures();
    if (!this.recenterRequested || Time.frameCount <= this.recenterRequestedFrameCount)
      return;
    if (this.RecenteredPose != null)
      this.RecenteredPose();
    this.recenterRequested = false;
    this.recenterRequestedFrameCount = int.MaxValue;
  }

  public event Action RecenteredPose;

  public void RecenterPose()
  {
    InputTracking.Recenter();
    this.recenterRequested = true;
    this.recenterRequestedFrameCount = Time.frameCount;
    OVRMixedReality.RecenterPose();
  }

  public Vector3 acceleration => !OVRManager.isHmdPresent ? Vector3.zero : OVRPlugin.GetNodeAcceleration(OVRPlugin.Node.Head, OVRPlugin.Step.Render).FromFlippedZVector3f();

  public Vector3 angularAcceleration => !OVRManager.isHmdPresent ? Vector3.zero : OVRPlugin.GetNodeAngularAcceleration(OVRPlugin.Node.Head, OVRPlugin.Step.Render).FromFlippedZVector3f() * 57.29578f;

  public Vector3 velocity => !OVRManager.isHmdPresent ? Vector3.zero : OVRPlugin.GetNodeVelocity(OVRPlugin.Node.Head, OVRPlugin.Step.Render).FromFlippedZVector3f();

  public Vector3 angularVelocity => !OVRManager.isHmdPresent ? Vector3.zero : OVRPlugin.GetNodeAngularVelocity(OVRPlugin.Node.Head, OVRPlugin.Step.Render).FromFlippedZVector3f() * 57.29578f;

  public OVRDisplay.EyeRenderDesc GetEyeRenderDesc(XRNode eye) => this.eyeDescs[(int) eye];

  public OVRDisplay.LatencyData latency
  {
    get
    {
      if (!OVRManager.isHmdPresent)
        return new OVRDisplay.LatencyData();
      string latency1 = OVRPlugin.latency;
      Regex regex = new Regex("Render: ([0-9]+[.][0-9]+)ms, TimeWarp: ([0-9]+[.][0-9]+)ms, PostPresent: ([0-9]+[.][0-9]+)ms", RegexOptions.None);
      OVRDisplay.LatencyData latency2 = new OVRDisplay.LatencyData();
      string input = latency1;
      Match match = regex.Match(input);
      if (match.Success)
      {
        latency2.render = float.Parse(match.Groups[1].Value);
        latency2.timeWarp = float.Parse(match.Groups[2].Value);
        latency2.postPresent = float.Parse(match.Groups[3].Value);
      }
      return latency2;
    }
  }

  public float appFramerate => !OVRManager.isHmdPresent ? 0.0f : OVRPlugin.GetAppFramerate();

  public int recommendedMSAALevel
  {
    get
    {
      int recommendedMsaaLevel = OVRPlugin.recommendedMSAALevel;
      if (recommendedMsaaLevel == 1)
        recommendedMsaaLevel = 0;
      return recommendedMsaaLevel;
    }
  }

  public float[] displayFrequenciesAvailable => OVRPlugin.systemDisplayFrequenciesAvailable;

  public float displayFrequency
  {
    get => OVRPlugin.systemDisplayFrequency;
    set => OVRPlugin.systemDisplayFrequency = value;
  }

  private void UpdateTextures()
  {
    this.ConfigureEyeDesc(XRNode.LeftEye);
    this.ConfigureEyeDesc(XRNode.RightEye);
  }

  private void ConfigureEyeDesc(XRNode eye)
  {
    if (!OVRManager.isHmdPresent)
      return;
    OVRPlugin.Sizei eyeTextureSize = OVRPlugin.GetEyeTextureSize((OVRPlugin.Eye) eye);
    OVRPlugin.Frustumf eyeFrustum = OVRPlugin.GetEyeFrustum((OVRPlugin.Eye) eye);
    this.eyeDescs[(int) eye] = new OVRDisplay.EyeRenderDesc()
    {
      resolution = new Vector2((float) eyeTextureSize.w, (float) eyeTextureSize.h),
      fov = 57.29578f * new Vector2(eyeFrustum.fovX, eyeFrustum.fovY)
    };
  }

  public struct EyeRenderDesc
  {
    public Vector2 resolution;
    public Vector2 fov;
  }

  public struct LatencyData
  {
    public float render;
    public float timeWarp;
    public float postPresent;
    public float renderError;
    public float timeWarpError;
  }
}
