// Decompiled with JetBrains decompiler
// Type: OVRPose
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

[Serializable]
public struct OVRPose
{
  public Vector3 position;
  public Quaternion orientation;

  public static OVRPose identity => new OVRPose()
  {
    position = Vector3.zero,
    orientation = Quaternion.identity
  };

  public override bool Equals(object obj) => obj is OVRPose ovrPose && this == ovrPose;

  public override int GetHashCode() => this.position.GetHashCode() ^ this.orientation.GetHashCode();

  public static bool operator ==(OVRPose x, OVRPose y) => x.position == y.position && x.orientation == y.orientation;

  public static bool operator !=(OVRPose x, OVRPose y) => !(x == y);

  public static OVRPose operator *(OVRPose lhs, OVRPose rhs) => new OVRPose()
  {
    position = lhs.position + lhs.orientation * rhs.position,
    orientation = lhs.orientation * rhs.orientation
  };

  public OVRPose Inverse()
  {
    OVRPose ovrPose;
    ovrPose.orientation = Quaternion.Inverse(this.orientation);
    ovrPose.position = ovrPose.orientation * -this.position;
    return ovrPose;
  }

  internal OVRPose flipZ()
  {
    OVRPose ovrPose = this;
    ovrPose.position.z = -ovrPose.position.z;
    ovrPose.orientation.z = -ovrPose.orientation.z;
    ovrPose.orientation.w = -ovrPose.orientation.w;
    return ovrPose;
  }

  internal OVRPlugin.Posef ToPosef() => new OVRPlugin.Posef()
  {
    Position = this.position.ToVector3f(),
    Orientation = this.orientation.ToQuatf()
  };
}
