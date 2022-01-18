// Decompiled with JetBrains decompiler
// Type: OVRProfile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class OVRProfile : UnityEngine.Object
{
  [Obsolete]
  public string id => "000abc123def";

  [Obsolete]
  public string userName => "Oculus User";

  [Obsolete]
  public string locale => "en_US";

  public float ipd => Vector3.Distance(OVRPlugin.GetNodePose(OVRPlugin.Node.EyeLeft, OVRPlugin.Step.Render).ToOVRPose().position, OVRPlugin.GetNodePose(OVRPlugin.Node.EyeRight, OVRPlugin.Step.Render).ToOVRPose().position);

  public float eyeHeight => OVRPlugin.eyeHeight;

  public float eyeDepth => OVRPlugin.eyeDepth;

  public float neckHeight => this.eyeHeight - 0.075f;

  [Obsolete]
  public OVRProfile.State state => OVRProfile.State.READY;

  [Obsolete]
  public enum State
  {
    NOT_TRIGGERED,
    LOADING,
    READY,
    ERROR,
  }
}
