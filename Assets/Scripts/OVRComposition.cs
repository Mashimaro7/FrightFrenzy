// Decompiled with JetBrains decompiler
// Type: OVRComposition
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public abstract class OVRComposition
{
  protected bool usingLastAttachedNodePose;
  protected OVRPose lastAttachedNodePose;

  public abstract OVRManager.CompositionMethod CompositionMethod();

  public abstract void Update(Camera mainCamera);

  public abstract void Cleanup();

  public virtual void RecenterPose()
  {
  }

  internal OVRPose ComputeCameraWorldSpacePose(OVRPlugin.CameraExtrinsics extrinsics)
  {
    OVRPose ovrPose1 = new OVRPose();
    OVRPose trackingSpacePose = extrinsics.RelativePose.ToOVRPose();
    if (extrinsics.AttachedToNode != OVRPlugin.Node.None && OVRPlugin.GetNodePresent(extrinsics.AttachedToNode))
    {
      if (this.usingLastAttachedNodePose)
      {
        Debug.Log((object) "The camera attached node get tracked");
        this.usingLastAttachedNodePose = false;
      }
      OVRPose ovrPose2 = OVRPlugin.GetNodePose(extrinsics.AttachedToNode, OVRPlugin.Step.Render).ToOVRPose();
      this.lastAttachedNodePose = ovrPose2;
      trackingSpacePose = ovrPose2 * trackingSpacePose;
    }
    else if (extrinsics.AttachedToNode != OVRPlugin.Node.None)
    {
      if (!this.usingLastAttachedNodePose)
      {
        Debug.LogWarning((object) "The camera attached node could not be tracked, using the last pose");
        this.usingLastAttachedNodePose = true;
      }
      trackingSpacePose = this.lastAttachedNodePose * trackingSpacePose;
    }
    return OVRExtensions.ToWorldSpacePose(trackingSpacePose);
  }
}
