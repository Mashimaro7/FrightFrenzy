// Decompiled with JetBrains decompiler
// Type: OVRMixedReality
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

internal static class OVRMixedReality
{
  public static Color chromaKeyColor = Color.green;
  public static bool useFakeExternalCamera = false;
  public static Vector3 fakeCameraPositon = new Vector3(3f, 0.0f, 3f);
  public static Quaternion fakeCameraRotation = Quaternion.LookRotation((new Vector3(0.0f, 1f, 0.0f) - OVRMixedReality.fakeCameraPositon).normalized, Vector3.up);
  public static float fakeCameraFov = 60f;
  public static float fakeCameraAspect = 1.777778f;
  public static OVRComposition currentComposition = (OVRComposition) null;

  public static void Update(
    GameObject parentObject,
    Camera mainCamera,
    OVRManager.CompositionMethod compositionMethod,
    bool useDynamicLighting,
    OVRManager.CameraDevice cameraDevice,
    OVRManager.DepthQuality depthQuality)
  {
    if (!OVRPlugin.initialized)
    {
      Debug.LogError((object) "OVRPlugin not initialized");
    }
    else
    {
      if (!OVRPlugin.IsMixedRealityInitialized())
        OVRPlugin.InitializeMixedReality();
      if (!OVRPlugin.IsMixedRealityInitialized())
      {
        Debug.LogError((object) "Unable to initialize MixedReality");
      }
      else
      {
        OVRPlugin.UpdateExternalCamera();
        OVRPlugin.UpdateCameraDevices();
        if (OVRMixedReality.currentComposition != null && OVRMixedReality.currentComposition.CompositionMethod() != compositionMethod)
        {
          OVRMixedReality.currentComposition.Cleanup();
          OVRMixedReality.currentComposition = (OVRComposition) null;
        }
        switch (compositionMethod)
        {
          case OVRManager.CompositionMethod.External:
            if (OVRMixedReality.currentComposition == null)
            {
              OVRMixedReality.currentComposition = (OVRComposition) new OVRExternalComposition(parentObject, mainCamera);
              break;
            }
            break;
          case OVRManager.CompositionMethod.Direct:
            if (OVRMixedReality.currentComposition == null)
            {
              OVRMixedReality.currentComposition = (OVRComposition) new OVRDirectComposition(parentObject, mainCamera, cameraDevice, useDynamicLighting, depthQuality);
              break;
            }
            break;
          case OVRManager.CompositionMethod.Sandwich:
            if (OVRMixedReality.currentComposition == null)
            {
              OVRMixedReality.currentComposition = (OVRComposition) new OVRSandwichComposition(parentObject, mainCamera, cameraDevice, useDynamicLighting, depthQuality);
              break;
            }
            break;
          default:
            Debug.LogError((object) ("Unknown CompositionMethod : " + (object) compositionMethod));
            return;
        }
        OVRMixedReality.currentComposition.Update(mainCamera);
      }
    }
  }

  public static void Cleanup()
  {
    if (OVRMixedReality.currentComposition != null)
    {
      OVRMixedReality.currentComposition.Cleanup();
      OVRMixedReality.currentComposition = (OVRComposition) null;
    }
    if (!OVRPlugin.IsMixedRealityInitialized())
      return;
    OVRPlugin.ShutdownMixedReality();
  }

  public static void RecenterPose()
  {
    if (OVRMixedReality.currentComposition == null)
      return;
    OVRMixedReality.currentComposition.RecenterPose();
  }
}
