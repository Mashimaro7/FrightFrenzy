// Decompiled with JetBrains decompiler
// Type: OVRTracker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class OVRTracker
{
  public bool isPresent => OVRManager.isHmdPresent && OVRPlugin.positionSupported;

  public bool isPositionTracked => OVRPlugin.positionTracked;

  public bool isEnabled
  {
    get => OVRManager.isHmdPresent && OVRPlugin.position;
    set
    {
      if (!OVRManager.isHmdPresent)
        return;
      OVRPlugin.position = value;
    }
  }

  public int count
  {
    get
    {
      int count = 0;
      for (int tracker = 0; tracker < 4; ++tracker)
      {
        if (this.GetPresent(tracker))
          ++count;
      }
      return count;
    }
  }

  public OVRTracker.Frustum GetFrustum(int tracker = 0) => !OVRManager.isHmdPresent ? new OVRTracker.Frustum() : OVRPlugin.GetTrackerFrustum((OVRPlugin.Tracker) tracker).ToFrustum();

  public OVRPose GetPose(int tracker = 0)
  {
    if (!OVRManager.isHmdPresent)
      return OVRPose.identity;
    OVRPose ovrPose;
    switch (tracker)
    {
      case 0:
        ovrPose = OVRPlugin.GetNodePose(OVRPlugin.Node.TrackerZero, OVRPlugin.Step.Render).ToOVRPose();
        break;
      case 1:
        ovrPose = OVRPlugin.GetNodePose(OVRPlugin.Node.TrackerOne, OVRPlugin.Step.Render).ToOVRPose();
        break;
      case 2:
        ovrPose = OVRPlugin.GetNodePose(OVRPlugin.Node.TrackerTwo, OVRPlugin.Step.Render).ToOVRPose();
        break;
      case 3:
        ovrPose = OVRPlugin.GetNodePose(OVRPlugin.Node.TrackerThree, OVRPlugin.Step.Render).ToOVRPose();
        break;
      default:
        return OVRPose.identity;
    }
    return new OVRPose()
    {
      position = ovrPose.position,
      orientation = ovrPose.orientation * Quaternion.Euler(0.0f, 180f, 0.0f)
    };
  }

  public bool GetPoseValid(int tracker = 0)
  {
    if (!OVRManager.isHmdPresent)
      return false;
    switch (tracker)
    {
      case 0:
        return OVRPlugin.GetNodePositionTracked(OVRPlugin.Node.TrackerZero);
      case 1:
        return OVRPlugin.GetNodePositionTracked(OVRPlugin.Node.TrackerOne);
      case 2:
        return OVRPlugin.GetNodePositionTracked(OVRPlugin.Node.TrackerTwo);
      case 3:
        return OVRPlugin.GetNodePositionTracked(OVRPlugin.Node.TrackerThree);
      default:
        return false;
    }
  }

  public bool GetPresent(int tracker = 0)
  {
    if (!OVRManager.isHmdPresent)
      return false;
    switch (tracker)
    {
      case 0:
        return OVRPlugin.GetNodePresent(OVRPlugin.Node.TrackerZero);
      case 1:
        return OVRPlugin.GetNodePresent(OVRPlugin.Node.TrackerOne);
      case 2:
        return OVRPlugin.GetNodePresent(OVRPlugin.Node.TrackerTwo);
      case 3:
        return OVRPlugin.GetNodePresent(OVRPlugin.Node.TrackerThree);
      default:
        return false;
    }
  }

  public struct Frustum
  {
    public float nearZ;
    public float farZ;
    public Vector2 fov;
  }
}
