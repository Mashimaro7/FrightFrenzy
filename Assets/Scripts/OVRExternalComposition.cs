// Decompiled with JetBrains decompiler
// Type: OVRExternalComposition
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Rendering;

public class OVRExternalComposition : OVRComposition
{
  private GameObject foregroundCameraGameObject;
  private Camera foregroundCamera;
  private GameObject backgroundCameraGameObject;
  private Camera backgroundCamera;
  private GameObject cameraProxyPlane;

  public override OVRManager.CompositionMethod CompositionMethod() => OVRManager.CompositionMethod.External;

  public OVRExternalComposition(GameObject parentObject, Camera mainCamera)
  {
    this.backgroundCameraGameObject = new GameObject();
    this.backgroundCameraGameObject.name = "MRBackgroundCamera";
    this.backgroundCameraGameObject.transform.parent = parentObject.transform;
    this.backgroundCamera = this.backgroundCameraGameObject.AddComponent<Camera>();
    this.backgroundCamera.stereoTargetEye = StereoTargetEyeMask.None;
    this.backgroundCamera.depth = float.MaxValue;
    this.backgroundCamera.rect = new Rect(0.0f, 0.0f, 0.5f, 1f);
    this.backgroundCamera.clearFlags = mainCamera.clearFlags;
    this.backgroundCamera.backgroundColor = mainCamera.backgroundColor;
    this.backgroundCamera.cullingMask = mainCamera.cullingMask & ~(int) OVRManager.instance.extraHiddenLayers;
    this.backgroundCamera.nearClipPlane = mainCamera.nearClipPlane;
    this.backgroundCamera.farClipPlane = mainCamera.farClipPlane;
    this.foregroundCameraGameObject = new GameObject();
    this.foregroundCameraGameObject.name = "MRForgroundCamera";
    this.foregroundCameraGameObject.transform.parent = parentObject.transform;
    this.foregroundCamera = this.foregroundCameraGameObject.AddComponent<Camera>();
    this.foregroundCamera.stereoTargetEye = StereoTargetEyeMask.None;
    this.foregroundCamera.depth = float.MaxValue;
    this.foregroundCamera.rect = new Rect(0.5f, 0.0f, 0.5f, 1f);
    this.foregroundCamera.clearFlags = CameraClearFlags.Color;
    this.foregroundCamera.backgroundColor = OVRMixedReality.chromaKeyColor;
    this.foregroundCamera.cullingMask = mainCamera.cullingMask & ~(int) OVRManager.instance.extraHiddenLayers;
    this.foregroundCamera.nearClipPlane = mainCamera.nearClipPlane;
    this.foregroundCamera.farClipPlane = mainCamera.farClipPlane;
    this.cameraProxyPlane = GameObject.CreatePrimitive(PrimitiveType.Quad);
    this.cameraProxyPlane.name = "MRProxyClipPlane";
    this.cameraProxyPlane.transform.parent = parentObject.transform;
    this.cameraProxyPlane.GetComponent<Collider>().enabled = false;
    this.cameraProxyPlane.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.Off;
    Material material = new Material(Shader.Find("Oculus/OVRMRClipPlane"));
    this.cameraProxyPlane.GetComponent<MeshRenderer>().material = material;
    material.SetColor("_Color", OVRMixedReality.chromaKeyColor);
    material.SetFloat("_Visible", 0.0f);
    this.cameraProxyPlane.transform.localScale = new Vector3(1000f, 1000f, 1000f);
    this.cameraProxyPlane.SetActive(true);
    this.foregroundCameraGameObject.AddComponent<OVRMRForegroundCameraManager>().clipPlaneGameObj = this.cameraProxyPlane;
  }

  public override void Update(Camera mainCamera)
  {
    OVRPlugin.SetHandNodePoseStateLatency(0.0);
    this.backgroundCamera.clearFlags = mainCamera.clearFlags;
    this.backgroundCamera.backgroundColor = mainCamera.backgroundColor;
    this.backgroundCamera.cullingMask = mainCamera.cullingMask & ~(int) OVRManager.instance.extraHiddenLayers;
    this.backgroundCamera.nearClipPlane = mainCamera.nearClipPlane;
    this.backgroundCamera.farClipPlane = mainCamera.farClipPlane;
    this.foregroundCamera.cullingMask = mainCamera.cullingMask & ~(int) OVRManager.instance.extraHiddenLayers;
    this.foregroundCamera.nearClipPlane = mainCamera.nearClipPlane;
    this.foregroundCamera.farClipPlane = mainCamera.farClipPlane;
    if (OVRMixedReality.useFakeExternalCamera || OVRPlugin.GetExternalCameraCount() == 0)
    {
      OVRPose ovrPose = new OVRPose();
      OVRPose worldSpacePose = OVRExtensions.ToWorldSpacePose(new OVRPose()
      {
        position = OVRMixedReality.fakeCameraPositon,
        orientation = OVRMixedReality.fakeCameraRotation
      });
      this.backgroundCamera.fieldOfView = OVRMixedReality.fakeCameraFov;
      this.backgroundCamera.aspect = OVRMixedReality.fakeCameraAspect;
      this.backgroundCamera.transform.FromOVRPose(worldSpacePose);
      this.foregroundCamera.fieldOfView = OVRMixedReality.fakeCameraFov;
      this.foregroundCamera.aspect = OVRMixedReality.fakeCameraAspect;
      this.foregroundCamera.transform.FromOVRPose(worldSpacePose);
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
        this.backgroundCamera.fieldOfView = num1;
        this.backgroundCamera.aspect = num2;
        this.backgroundCamera.transform.FromOVRPose(cameraWorldSpacePose);
        this.foregroundCamera.fieldOfView = num1;
        this.foregroundCamera.aspect = cameraIntrinsics.FOVPort.LeftTan / cameraIntrinsics.FOVPort.UpTan;
        this.foregroundCamera.transform.FromOVRPose(cameraWorldSpacePose);
      }
      else
      {
        Debug.LogError((object) "Failed to get external camera information");
        return;
      }
    }
    Vector3 vector3 = (mainCamera.transform.position - this.foregroundCamera.transform.position) with
    {
      y = 0.0f
    };
    this.cameraProxyPlane.transform.position = mainCamera.transform.position;
    this.cameraProxyPlane.transform.LookAt(this.cameraProxyPlane.transform.position + vector3);
  }

  public override void Cleanup()
  {
    OVRCompositionUtil.SafeDestroy(ref this.backgroundCameraGameObject);
    this.backgroundCamera = (Camera) null;
    OVRCompositionUtil.SafeDestroy(ref this.foregroundCameraGameObject);
    this.foregroundCamera = (Camera) null;
    OVRCompositionUtil.SafeDestroy(ref this.cameraProxyPlane);
    Debug.Log((object) "ExternalComposition deactivated");
  }
}
