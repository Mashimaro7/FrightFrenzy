// Decompiled with JetBrains decompiler
// Type: OVRCameraComposition
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.Rendering;

public abstract class OVRCameraComposition : OVRComposition
{
  protected GameObject cameraFramePlaneObject;
  protected float cameraFramePlaneDistance;
  protected readonly bool hasCameraDeviceOpened;
  protected readonly bool useDynamicLighting;
  internal readonly OVRPlugin.CameraDevice cameraDevice = OVRPlugin.CameraDevice.WebCamera0;
  private OVRCameraRig cameraRig;
  private Mesh boundaryMesh;
  private float boundaryMeshTopY;
  private float boundaryMeshBottomY;
  private OVRManager.VirtualGreenScreenType boundaryMeshType;
  private bool nullcameraRigWarningDisplayed;

  protected OVRCameraComposition(
    OVRManager.CameraDevice inCameraDevice,
    bool inUseDynamicLighting,
    OVRManager.DepthQuality depthQuality)
  {
    this.cameraDevice = OVRCompositionUtil.ConvertCameraDevice(inCameraDevice);
    this.hasCameraDeviceOpened = false;
    this.useDynamicLighting = inUseDynamicLighting;
    bool flag = OVRPlugin.DoesCameraDeviceSupportDepth(this.cameraDevice);
    if (this.useDynamicLighting && !flag)
      Debug.LogWarning((object) "The camera device doesn't support depth. The result of dynamic lighting might not be correct");
    if (!OVRPlugin.IsCameraDeviceAvailable(this.cameraDevice))
      return;
    OVRPlugin.CameraIntrinsics cameraIntrinsics;
    if (OVRPlugin.GetExternalCameraCount() > 0 && OVRPlugin.GetMixedRealityCameraInfo(0, out OVRPlugin.CameraExtrinsics _, out cameraIntrinsics))
      OVRPlugin.SetCameraDevicePreferredColorFrameSize(this.cameraDevice, cameraIntrinsics.ImageSensorPixelResolution.w, cameraIntrinsics.ImageSensorPixelResolution.h);
    if (this.useDynamicLighting)
    {
      OVRPlugin.SetCameraDeviceDepthSensingMode(this.cameraDevice, OVRPlugin.CameraDeviceDepthSensingMode.Fill);
      OVRPlugin.CameraDeviceDepthQuality depthQuality1 = OVRPlugin.CameraDeviceDepthQuality.Medium;
      switch (depthQuality)
      {
        case OVRManager.DepthQuality.Low:
          depthQuality1 = OVRPlugin.CameraDeviceDepthQuality.Low;
          break;
        case OVRManager.DepthQuality.Medium:
          depthQuality1 = OVRPlugin.CameraDeviceDepthQuality.Medium;
          break;
        case OVRManager.DepthQuality.High:
          depthQuality1 = OVRPlugin.CameraDeviceDepthQuality.High;
          break;
        default:
          Debug.LogWarning((object) "Unknown depth quality");
          break;
      }
      OVRPlugin.SetCameraDevicePreferredDepthQuality(this.cameraDevice, depthQuality1);
    }
    OVRPlugin.OpenCameraDevice(this.cameraDevice);
    if (!OVRPlugin.HasCameraDeviceOpened(this.cameraDevice))
      return;
    this.hasCameraDeviceOpened = true;
  }

  public override void Cleanup()
  {
    OVRCompositionUtil.SafeDestroy(ref this.cameraFramePlaneObject);
    if (!this.hasCameraDeviceOpened)
      return;
    OVRPlugin.CloseCameraDevice(this.cameraDevice);
  }

  public override void RecenterPose() => this.boundaryMesh = (Mesh) null;

  protected void CreateCameraFramePlaneObject(
    GameObject parentObject,
    Camera mixedRealityCamera,
    bool useDynamicLighting)
  {
    this.cameraFramePlaneObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
    this.cameraFramePlaneObject.name = "MRCameraFrame";
    this.cameraFramePlaneObject.transform.parent = parentObject.transform;
    this.cameraFramePlaneObject.GetComponent<Collider>().enabled = false;
    this.cameraFramePlaneObject.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.Off;
    Material material = new Material(Shader.Find(useDynamicLighting ? "Oculus/OVRMRCameraFrameLit" : "Oculus/OVRMRCameraFrame"));
    this.cameraFramePlaneObject.GetComponent<MeshRenderer>().material = material;
    material.SetColor("_Color", Color.white);
    material.SetFloat("_Visible", 0.0f);
    this.cameraFramePlaneObject.transform.localScale = new Vector3(4f, 4f, 4f);
    this.cameraFramePlaneObject.SetActive(true);
    OVRCameraComposition.OVRCameraFrameCompositionManager compositionManager = mixedRealityCamera.gameObject.AddComponent<OVRCameraComposition.OVRCameraFrameCompositionManager>();
    compositionManager.cameraFrameGameObj = this.cameraFramePlaneObject;
    compositionManager.composition = this;
  }

  protected void UpdateCameraFramePlaneObject(
    Camera mainCamera,
    Camera mixedRealityCamera,
    RenderTexture boundaryMeshMaskTexture)
  {
    bool flag = false;
    Material material = this.cameraFramePlaneObject.GetComponent<MeshRenderer>().material;
    Texture2D texture2D1 = Texture2D.blackTexture;
    Texture2D texture2D2 = Texture2D.whiteTexture;
    if (OVRPlugin.IsCameraDeviceColorFrameAvailable(this.cameraDevice))
    {
      texture2D1 = OVRPlugin.GetCameraDeviceColorFrameTexture(this.cameraDevice);
    }
    else
    {
      Debug.LogWarning((object) "Camera: color frame not ready");
      flag = true;
    }
    if (this.useDynamicLighting & OVRPlugin.DoesCameraDeviceSupportDepth(this.cameraDevice))
    {
      if (OVRPlugin.IsCameraDeviceDepthFrameAvailable(this.cameraDevice))
      {
        texture2D2 = OVRPlugin.GetCameraDeviceDepthFrameTexture(this.cameraDevice);
      }
      else
      {
        Debug.LogWarning((object) "Camera: depth frame not ready");
        flag = true;
      }
    }
    if (flag)
      return;
    Vector3 rhs = mainCamera.transform.position - mixedRealityCamera.transform.position;
    float num1 = Vector3.Dot(mixedRealityCamera.transform.forward, rhs);
    this.cameraFramePlaneDistance = num1;
    this.cameraFramePlaneObject.transform.position = mixedRealityCamera.transform.position + mixedRealityCamera.transform.forward * num1;
    this.cameraFramePlaneObject.transform.rotation = mixedRealityCamera.transform.rotation;
    float num2 = Mathf.Tan((float) ((double) mixedRealityCamera.fieldOfView * (Math.PI / 180.0) * 0.5));
    this.cameraFramePlaneObject.transform.localScale = new Vector3((float) ((double) num1 * (double) mixedRealityCamera.aspect * (double) num2 * 2.0), (float) ((double) num1 * (double) num2 * 2.0), 1f);
    float y = (float) ((double) num1 * (double) num2 * 2.0);
    float x = y * mixedRealityCamera.aspect;
    float cullingDistance = float.MaxValue;
    this.cameraRig = (OVRCameraRig) null;
    if (OVRManager.instance.virtualGreenScreenType != OVRManager.VirtualGreenScreenType.Off)
    {
      this.cameraRig = mainCamera.GetComponentInParent<OVRCameraRig>();
      if ((UnityEngine.Object) this.cameraRig != (UnityEngine.Object) null && (UnityEngine.Object) this.cameraRig.centerEyeAnchor == (UnityEngine.Object) null)
        this.cameraRig = (OVRCameraRig) null;
      this.RefreshBoundaryMesh(mixedRealityCamera, out cullingDistance);
    }
    material.mainTexture = (Texture) texture2D1;
    material.SetTexture("_DepthTex", (Texture) texture2D2);
    material.SetVector("_FlipParams", new Vector4(OVRManager.instance.flipCameraFrameHorizontally ? 1f : 0.0f, OVRManager.instance.flipCameraFrameVertically ? 1f : 0.0f, 0.0f, 0.0f));
    material.SetColor("_ChromaKeyColor", OVRManager.instance.chromaKeyColor);
    material.SetFloat("_ChromaKeySimilarity", OVRManager.instance.chromaKeySimilarity);
    material.SetFloat("_ChromaKeySmoothRange", OVRManager.instance.chromaKeySmoothRange);
    material.SetFloat("_ChromaKeySpillRange", OVRManager.instance.chromaKeySpillRange);
    material.SetVector("_TextureDimension", new Vector4((float) texture2D1.width, (float) texture2D1.height, 1f / (float) texture2D1.width, 1f / (float) texture2D1.height));
    material.SetVector("_TextureWorldSize", new Vector4(x, y, 0.0f, 0.0f));
    material.SetFloat("_SmoothFactor", OVRManager.instance.dynamicLightingSmoothFactor);
    material.SetFloat("_DepthVariationClamp", OVRManager.instance.dynamicLightingDepthVariationClampingValue);
    material.SetFloat("_CullingDistance", cullingDistance);
    if (OVRManager.instance.virtualGreenScreenType == OVRManager.VirtualGreenScreenType.Off || (UnityEngine.Object) this.boundaryMesh == (UnityEngine.Object) null || (UnityEngine.Object) boundaryMeshMaskTexture == (UnityEngine.Object) null)
      material.SetTexture("_MaskTex", (Texture) Texture2D.whiteTexture);
    else if ((UnityEngine.Object) this.cameraRig == (UnityEngine.Object) null)
    {
      if (!this.nullcameraRigWarningDisplayed)
      {
        Debug.LogWarning((object) "Could not find the OVRCameraRig/CenterEyeAnchor object. Please check if the OVRCameraRig has been setup properly. The virtual green screen has been temporarily disabled");
        this.nullcameraRigWarningDisplayed = true;
      }
      material.SetTexture("_MaskTex", (Texture) Texture2D.whiteTexture);
    }
    else
    {
      if (this.nullcameraRigWarningDisplayed)
      {
        Debug.Log((object) "OVRCameraRig/CenterEyeAnchor object found. Virtual green screen is activated");
        this.nullcameraRigWarningDisplayed = false;
      }
      material.SetTexture("_MaskTex", (Texture) boundaryMeshMaskTexture);
    }
  }

  protected void RefreshBoundaryMesh(Camera camera, out float cullingDistance)
  {
    float num = OVRManager.instance.virtualGreenScreenApplyDepthCulling ? OVRManager.instance.virtualGreenScreenDepthTolerance : float.PositiveInfinity;
    cullingDistance = OVRCompositionUtil.GetMaximumBoundaryDistance(camera, OVRCompositionUtil.ToBoundaryType(OVRManager.instance.virtualGreenScreenType)) + num;
    if (!((UnityEngine.Object) this.boundaryMesh == (UnityEngine.Object) null) && this.boundaryMeshType == OVRManager.instance.virtualGreenScreenType && (double) this.boundaryMeshTopY == (double) OVRManager.instance.virtualGreenScreenTopY && (double) this.boundaryMeshBottomY == (double) OVRManager.instance.virtualGreenScreenBottomY)
      return;
    this.boundaryMeshTopY = OVRManager.instance.virtualGreenScreenTopY;
    this.boundaryMeshBottomY = OVRManager.instance.virtualGreenScreenBottomY;
    this.boundaryMesh = OVRCompositionUtil.BuildBoundaryMesh(OVRCompositionUtil.ToBoundaryType(OVRManager.instance.virtualGreenScreenType), this.boundaryMeshTopY, this.boundaryMeshBottomY);
    this.boundaryMeshType = OVRManager.instance.virtualGreenScreenType;
  }

  public class OVRCameraFrameCompositionManager : MonoBehaviour
  {
    public GameObject cameraFrameGameObj;
    public OVRCameraComposition composition;
    public RenderTexture boundaryMeshMaskTexture;
    private Material cameraFrameMaterial;
    private Material whiteMaterial;

    private void Start()
    {
      Shader shader = Shader.Find("Oculus/Unlit");
      if (!(bool) (UnityEngine.Object) shader)
      {
        Debug.LogError((object) "Oculus/Unlit shader does not exist");
      }
      else
      {
        this.whiteMaterial = new Material(shader);
        this.whiteMaterial.color = Color.white;
      }
    }

    private void OnPreRender()
    {
      if (OVRManager.instance.virtualGreenScreenType != OVRManager.VirtualGreenScreenType.Off && (UnityEngine.Object) this.boundaryMeshMaskTexture != (UnityEngine.Object) null && (UnityEngine.Object) this.composition.boundaryMesh != (UnityEngine.Object) null)
      {
        RenderTexture active = RenderTexture.active;
        RenderTexture.active = this.boundaryMeshMaskTexture;
        GL.PushMatrix();
        GL.LoadProjectionMatrix(this.GetComponent<Camera>().projectionMatrix);
        GL.Clear(false, true, Color.black);
        for (int pass = 0; pass < this.whiteMaterial.passCount; ++pass)
        {
          if (this.whiteMaterial.SetPass(pass))
            Graphics.DrawMeshNow(this.composition.boundaryMesh, this.composition.cameraRig.ComputeTrackReferenceMatrix());
        }
        GL.PopMatrix();
        RenderTexture.active = active;
      }
      if (!(bool) (UnityEngine.Object) this.cameraFrameGameObj)
        return;
      if ((UnityEngine.Object) this.cameraFrameMaterial == (UnityEngine.Object) null)
        this.cameraFrameMaterial = this.cameraFrameGameObj.GetComponent<MeshRenderer>().material;
      this.cameraFrameMaterial.SetFloat("_Visible", 1f);
    }

    private void OnPostRender()
    {
      if (!(bool) (UnityEngine.Object) this.cameraFrameGameObj)
        return;
      this.cameraFrameMaterial.SetFloat("_Visible", 0.0f);
    }
  }
}
