// Decompiled with JetBrains decompiler
// Type: OVRCompositionUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

internal class OVRCompositionUtil
{
  public static void SafeDestroy(GameObject obj)
  {
    if (Application.isPlaying)
      Object.Destroy((Object) obj);
    else
      Object.DestroyImmediate((Object) obj);
  }

  public static void SafeDestroy(ref GameObject obj)
  {
    OVRCompositionUtil.SafeDestroy(obj);
    obj = (GameObject) null;
  }

  public static OVRPlugin.CameraDevice ConvertCameraDevice(
    OVRManager.CameraDevice cameraDevice)
  {
    switch (cameraDevice)
    {
      case OVRManager.CameraDevice.WebCamera0:
        return OVRPlugin.CameraDevice.WebCamera0;
      case OVRManager.CameraDevice.WebCamera1:
        return OVRPlugin.CameraDevice.WebCamera1;
      case OVRManager.CameraDevice.ZEDCamera:
        return OVRPlugin.CameraDevice.ZEDCamera;
      default:
        return OVRPlugin.CameraDevice.None;
    }
  }

  public static OVRBoundary.BoundaryType ToBoundaryType(
    OVRManager.VirtualGreenScreenType type)
  {
    if (type == OVRManager.VirtualGreenScreenType.OuterBoundary)
      return OVRBoundary.BoundaryType.OuterBoundary;
    if (type == OVRManager.VirtualGreenScreenType.PlayArea)
      return OVRBoundary.BoundaryType.PlayArea;
    Debug.LogWarning((object) "Unmatched VirtualGreenScreenType");
    return OVRBoundary.BoundaryType.OuterBoundary;
  }

  public static Vector3 GetWorldPosition(Vector3 trackingSpacePosition)
  {
    OVRPose trackingSpacePose;
    trackingSpacePose.position = trackingSpacePosition;
    trackingSpacePose.orientation = Quaternion.identity;
    return OVRExtensions.ToWorldSpacePose(trackingSpacePose).position;
  }

  public static float GetMaximumBoundaryDistance(
    Camera camera,
    OVRBoundary.BoundaryType boundaryType)
  {
    if (!OVRManager.boundary.GetConfigured())
      return float.MaxValue;
    Vector3[] geometry = OVRManager.boundary.GetGeometry(boundaryType);
    if (geometry.Length == 0)
      return float.MaxValue;
    float boundaryDistance = float.MinValue;
    foreach (Vector3 trackingSpacePosition in geometry)
    {
      Vector3 worldPosition = OVRCompositionUtil.GetWorldPosition(trackingSpacePosition);
      float num = Vector3.Dot(camera.transform.forward, worldPosition);
      if ((double) boundaryDistance < (double) num)
        boundaryDistance = num;
    }
    return boundaryDistance;
  }

  public static Mesh BuildBoundaryMesh(
    OVRBoundary.BoundaryType boundaryType,
    float topY,
    float bottomY)
  {
    if (!OVRManager.boundary.GetConfigured())
      return (Mesh) null;
    List<Vector3> vector3List = new List<Vector3>((IEnumerable<Vector3>) OVRManager.boundary.GetGeometry(boundaryType));
    if (vector3List.Count == 0)
      return (Mesh) null;
    vector3List.Add(vector3List[0]);
    int count = vector3List.Count;
    Vector3[] vector3Array = new Vector3[count * 2];
    Vector2[] vector2Array = new Vector2[count * 2];
    for (int index = 0; index < count; ++index)
    {
      Vector3 vector3 = vector3List[index];
      vector3Array[index] = new Vector3(vector3.x, bottomY, vector3.z);
      vector3Array[index + count] = new Vector3(vector3.x, topY, vector3.z);
      vector2Array[index] = new Vector2((float) index / (float) (count - 1), 0.0f);
      vector2Array[index + count] = new Vector2(vector2Array[index].x, 1f);
    }
    int[] numArray = new int[(count - 1) * 2 * 3];
    for (int index = 0; index < count - 1; ++index)
    {
      numArray[index * 6] = index;
      numArray[index * 6 + 1] = index + count;
      numArray[index * 6 + 2] = index + 1 + count;
      numArray[index * 6 + 3] = index;
      numArray[index * 6 + 4] = index + 1 + count;
      numArray[index * 6 + 5] = index + 1;
    }
    return new Mesh()
    {
      vertices = vector3Array,
      uv = vector2Array,
      triangles = numArray
    };
  }
}
