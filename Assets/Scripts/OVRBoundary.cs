// Decompiled with JetBrains decompiler
// Type: OVRBoundary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class OVRBoundary
{
  private static int cachedVector3fSize = Marshal.SizeOf(typeof (OVRPlugin.Vector3f));
  private static OVRNativeBuffer cachedGeometryNativeBuffer = new OVRNativeBuffer(0);
  private static float[] cachedGeometryManagedBuffer = new float[0];

  public bool GetConfigured() => OVRPlugin.GetBoundaryConfigured();

  public OVRBoundary.BoundaryTestResult TestNode(
    OVRBoundary.Node node,
    OVRBoundary.BoundaryType boundaryType)
  {
    OVRPlugin.BoundaryTestResult boundaryTestResult = OVRPlugin.TestBoundaryNode((OVRPlugin.Node) node, (OVRPlugin.BoundaryType) boundaryType);
    return new OVRBoundary.BoundaryTestResult()
    {
      IsTriggering = boundaryTestResult.IsTriggering == OVRPlugin.Bool.True,
      ClosestDistance = boundaryTestResult.ClosestDistance,
      ClosestPoint = boundaryTestResult.ClosestPoint.FromFlippedZVector3f(),
      ClosestPointNormal = boundaryTestResult.ClosestPointNormal.FromFlippedZVector3f()
    };
  }

  public OVRBoundary.BoundaryTestResult TestPoint(
    Vector3 point,
    OVRBoundary.BoundaryType boundaryType)
  {
    OVRPlugin.BoundaryTestResult boundaryTestResult = OVRPlugin.TestBoundaryPoint(point.ToFlippedZVector3f(), (OVRPlugin.BoundaryType) boundaryType);
    return new OVRBoundary.BoundaryTestResult()
    {
      IsTriggering = boundaryTestResult.IsTriggering == OVRPlugin.Bool.True,
      ClosestDistance = boundaryTestResult.ClosestDistance,
      ClosestPoint = boundaryTestResult.ClosestPoint.FromFlippedZVector3f(),
      ClosestPointNormal = boundaryTestResult.ClosestPointNormal.FromFlippedZVector3f()
    };
  }

  public void SetLookAndFeel(OVRBoundary.BoundaryLookAndFeel lookAndFeel) => OVRPlugin.SetBoundaryLookAndFeel(new OVRPlugin.BoundaryLookAndFeel()
  {
    Color = lookAndFeel.Color.ToColorf()
  });

  public void ResetLookAndFeel() => OVRPlugin.ResetBoundaryLookAndFeel();

  public Vector3[] GetGeometry(OVRBoundary.BoundaryType boundaryType)
  {
    int pointsCount = 0;
    if (OVRPlugin.GetBoundaryGeometry2((OVRPlugin.BoundaryType) boundaryType, IntPtr.Zero, ref pointsCount) && pointsCount > 0)
    {
      int numBytes = pointsCount * OVRBoundary.cachedVector3fSize;
      if (OVRBoundary.cachedGeometryNativeBuffer.GetCapacity() < numBytes)
        OVRBoundary.cachedGeometryNativeBuffer.Reset(numBytes);
      int length = pointsCount * 3;
      if (OVRBoundary.cachedGeometryManagedBuffer.Length < length)
        OVRBoundary.cachedGeometryManagedBuffer = new float[length];
      if (OVRPlugin.GetBoundaryGeometry2((OVRPlugin.BoundaryType) boundaryType, OVRBoundary.cachedGeometryNativeBuffer.GetPointer(), ref pointsCount))
      {
        Marshal.Copy(OVRBoundary.cachedGeometryNativeBuffer.GetPointer(), OVRBoundary.cachedGeometryManagedBuffer, 0, length);
        Vector3[] geometry = new Vector3[pointsCount];
        for (int index = 0; index < pointsCount; ++index)
          geometry[index] = new OVRPlugin.Vector3f()
          {
            x = OVRBoundary.cachedGeometryManagedBuffer[3 * index],
            y = OVRBoundary.cachedGeometryManagedBuffer[3 * index + 1],
            z = OVRBoundary.cachedGeometryManagedBuffer[3 * index + 2]
          }.FromFlippedZVector3f();
        return geometry;
      }
    }
    return new Vector3[0];
  }

  public Vector3 GetDimensions(OVRBoundary.BoundaryType boundaryType) => OVRPlugin.GetBoundaryDimensions((OVRPlugin.BoundaryType) boundaryType).FromVector3f();

  public bool GetVisible() => OVRPlugin.GetBoundaryVisible();

  public void SetVisible(bool value) => OVRPlugin.SetBoundaryVisible(value);

  public enum Node
  {
    HandLeft = 3,
    HandRight = 4,
    Head = 9,
  }

  public enum BoundaryType
  {
    OuterBoundary = 1,
    PlayArea = 256, // 0x00000100
  }

  public struct BoundaryTestResult
  {
    public bool IsTriggering;
    public float ClosestDistance;
    public Vector3 ClosestPoint;
    public Vector3 ClosestPointNormal;
  }

  public struct BoundaryLookAndFeel
  {
    public Color Color;
  }
}
