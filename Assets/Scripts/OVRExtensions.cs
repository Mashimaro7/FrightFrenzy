// Decompiled with JetBrains decompiler
// Type: OVRExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.XR;

public static class OVRExtensions
{
  public static OVRPose ToTrackingSpacePose(this Transform transform, Camera camera)
  {
    OVRPose ovrPose;
    ovrPose.position = InputTracking.GetLocalPosition(XRNode.Head);
    ovrPose.orientation = InputTracking.GetLocalRotation(XRNode.Head);
    return ovrPose * transform.ToHeadSpacePose(camera);
  }

  public static OVRPose ToWorldSpacePose(OVRPose trackingSpacePose)
  {
    OVRPose ovrPose1;
    ovrPose1.position = InputTracking.GetLocalPosition(XRNode.Head);
    ovrPose1.orientation = InputTracking.GetLocalRotation(XRNode.Head);
    OVRPose ovrPose2 = ovrPose1.Inverse() * trackingSpacePose;
    return Camera.main.transform.ToOVRPose() * ovrPose2;
  }

  public static OVRPose ToHeadSpacePose(this Transform transform, Camera camera) => camera.transform.ToOVRPose().Inverse() * transform.ToOVRPose();

  internal static OVRPose ToOVRPose(this Transform t, bool isLocal = false)
  {
    OVRPose ovrPose;
    ovrPose.orientation = isLocal ? t.localRotation : t.rotation;
    ovrPose.position = isLocal ? t.localPosition : t.position;
    return ovrPose;
  }

  internal static void FromOVRPose(this Transform t, OVRPose pose, bool isLocal = false)
  {
    if (isLocal)
    {
      t.localRotation = pose.orientation;
      t.localPosition = pose.position;
    }
    else
    {
      t.rotation = pose.orientation;
      t.position = pose.position;
    }
  }

  internal static OVRPose ToOVRPose(this OVRPlugin.Posef p) => new OVRPose()
  {
    position = new Vector3(p.Position.x, p.Position.y, -p.Position.z),
    orientation = new Quaternion(-p.Orientation.x, -p.Orientation.y, p.Orientation.z, p.Orientation.w)
  };

  internal static OVRTracker.Frustum ToFrustum(this OVRPlugin.Frustumf f) => new OVRTracker.Frustum()
  {
    nearZ = f.zNear,
    farZ = f.zFar,
    fov = new Vector2()
    {
      x = 57.29578f * f.fovX,
      y = 57.29578f * f.fovY
    }
  };

  internal static Color FromColorf(this OVRPlugin.Colorf c) => new Color()
  {
    r = c.r,
    g = c.g,
    b = c.b,
    a = c.a
  };

  internal static OVRPlugin.Colorf ToColorf(this Color c) => new OVRPlugin.Colorf()
  {
    r = c.r,
    g = c.g,
    b = c.b,
    a = c.a
  };

  internal static Vector3 FromVector3f(this OVRPlugin.Vector3f v) => new Vector3()
  {
    x = v.x,
    y = v.y,
    z = v.z
  };

  internal static Vector3 FromFlippedZVector3f(this OVRPlugin.Vector3f v) => new Vector3()
  {
    x = v.x,
    y = v.y,
    z = -v.z
  };

  internal static OVRPlugin.Vector3f ToVector3f(this Vector3 v) => new OVRPlugin.Vector3f()
  {
    x = v.x,
    y = v.y,
    z = v.z
  };

  internal static OVRPlugin.Vector3f ToFlippedZVector3f(this Vector3 v) => new OVRPlugin.Vector3f()
  {
    x = v.x,
    y = v.y,
    z = -v.z
  };

  internal static Quaternion FromQuatf(this OVRPlugin.Quatf q) => new Quaternion()
  {
    x = q.x,
    y = q.y,
    z = q.z,
    w = q.w
  };

  internal static Quaternion FromFlippedZQuatf(this OVRPlugin.Quatf q) => new Quaternion()
  {
    x = -q.x,
    y = -q.y,
    z = q.z,
    w = q.w
  };

  internal static OVRPlugin.Quatf ToQuatf(this Quaternion q) => new OVRPlugin.Quatf()
  {
    x = q.x,
    y = q.y,
    z = q.z,
    w = q.w
  };

  internal static OVRPlugin.Quatf ToFlippedZQuatf(this Quaternion q) => new OVRPlugin.Quatf()
  {
    x = -q.x,
    y = -q.y,
    z = q.z,
    w = q.w
  };
}
