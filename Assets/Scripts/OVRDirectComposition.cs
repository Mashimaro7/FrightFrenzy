// Decompiled with JetBrains decompiler
// Type: OVRDirectComposition
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class OVRDirectComposition : OVRCameraComposition
{
  public GameObject directCompositionCameraGameObject;
  public Camera directCompositionCamera;
  public RenderTexture boundaryMeshMaskTexture;

  public override OVRManager.CompositionMethod CompositionMethod() => OVRManager.CompositionMethod.Direct;

  public OVRDirectComposition(
    GameObject parentObject,
    Camera mainCamera,
    OVRManager.CameraDevice cameraDevice,
    bool useDynamicLighting,
    OVRManager.DepthQuality depthQuality)
    : base(cameraDevice, useDynamicLighting, depthQuality)
  {
    this.directCompositionCameraGameObject = new GameObject();
    this.directCompositionCameraGameObject.name = "MRDirectCompositionCamera";
    this.directCompositionCameraGameObject.transform.parent = parentObject.transform;
    this.directCompositionCamera = this.directCompositionCameraGameObject.AddComponent<Camera>();
    this.directCompositionCamera.stereoTargetEye = StereoTargetEyeMask.None;
    this.directCompositionCamera.depth = float.MaxValue;
    this.directCompositionCamera.rect = new Rect(0.0f, 0.0f, 1f, 1f);
    this.directCompositionCamera.clearFlags = mainCamera.clearFlags;
    this.directCompositionCamera.backgroundColor = mainCamera.backgroundColor;
    this.directCompositionCamera.cullingMask = mainCamera.cullingMask & ~(int) OVRManager.instance.extraHiddenLayers;
    this.directCompositionCamera.nearClipPlane = mainCamera.nearClipPlane;
    this.directCompositionCamera.farClipPlane = mainCamera.farClipPlane;
    if (!this.hasCameraDeviceOpened)
    {
      Debug.LogError((object) ("Unable to open camera device " + (object) cameraDevice));
    }
    else
    {
      Debug.Log((object) ("DirectComposition activated : useDynamicLighting " + (useDynamicLighting ? "ON" : "OFF")));
      this.CreateCameraFramePlaneObject(parentObject, this.directCompositionCamera, useDynamicLighting);
    }
  }

  public override void Update(Camera mainCamera)
  {
    if (!this.hasCameraDeviceOpened)
      return;
    if (!OVRPlugin.SetHandNodePoseStateLatency((double) OVRManager.instance.handPoseStateLatency))
      Debug.LogWarning((object) ("HandPoseStateLatency is invalid. Expect a value between 0.0 to 0.5, get " + (object) OVRManager.instance.handPoseStateLatency));
    this.directCompositionCamera.clearFlags = mainCamera.clearFlags;
    this.directCompositionCamera.backgroundColor = mainCamera.backgroundColor;
    this.directCompositionCamera.cullingMask = mainCamera.cullingMask & ~(int) OVRManager.instance.extraHiddenLayers;
    this.directCompositionCamera.nearClipPlane = mainCamera.nearClipPlane;
    this.directCompositionCamera.farClipPlane = mainCamera.farClipPlane;
    if (OVRMixedReality.useFakeExternalCamera || OVRPlugin.GetExternalCameraCount() == 0)
    {
      OVRPose ovrPose = new OVRPose();
      OVRPose worldSpacePose = OVRExtensions.ToWorldSpacePose(new OVRPose()
      {
        position = OVRMixedReality.fakeCameraPositon,
        orientation = OVRMixedReality.fakeCameraRotation
      });
      this.directCompositionCamera.fieldOfView = OVRMixedReality.fakeCameraFov;
      this.directCompositionCamera.aspect = OVRMixedReality.fakeCameraAspect;
      this.directCompositionCamera.transform.FromOVRPose(worldSpacePose);
    }
    else
    {
      OVRPlugin.CameraExtrinsics cameraExtrinsics;
      OVRPlugin.CameraIntrinsics cameraIntrinsics;
      if (OVRPlugin.GetMixedRealityCameraInfo(0, out cameraExtrinsics, out cameraIntrinsics))
      {
        OVRPose cameraWorldSpacePose = this.ComputeCameraWorldSpacePose(cameraExtrinsics);
        float num1 = (float) ((double) Mathf.Atan(cameraIntrinsics.FOVPort.UpTan) * 57.2957801818848 * 2.0);
        float num2 = cameraIntrinsics.FOVPort.LeftTan / cameraIntrinsics.FOVPort.UpTan;
        this.directCompositionCamera.fieldOfView = num1;
        this.directCompositionCamera.aspect = num2;
        this.directCompositionCamera.transform.FromOVRPose(cameraWorldSpacePose);
      }
      else
        Debug.LogWarning((object) "Failed to get external camera information");
    }
    if (!this.hasCameraDeviceOpened)
      return;
    if ((Object) this.boundaryMeshMaskTexture == (Object) null || this.boundaryMeshMaskTexture.width != Screen.width || this.boundaryMeshMaskTexture.height != Screen.height)
    {
      this.boundaryMeshMaskTexture = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.R8);
      this.boundaryMeshMaskTexture.Create();
    }
    this.UpdateCameraFramePlaneObject(mainCamera, this.directCompositionCamera, this.boundaryMeshMaskTexture);
    this.directCompositionCamera.GetComponent<OVRCameraComposition.OVRCameraFrameCompositionManager>().boundaryMeshMaskTexture = this.boundaryMeshMaskTexture;
  }

  public override void Cleanup()
  {
    base.Cleanup();
    OVRCompositionUtil.SafeDestroy(ref this.directCompositionCameraGameObject);
    this.directCompositionCamera = (Camera) null;
    Debug.Log((object) "DirectComposition deactivated");
  }
}
