// Decompiled with JetBrains decompiler
// Type: OVRSandwichComposition
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Rendering;

public class OVRSandwichComposition : OVRCameraComposition
{
  public float frameRealtime;
  public Camera fgCamera;
  public Camera bgCamera;
  public readonly int historyRecordCount = 8;
  public readonly OVRSandwichComposition.HistoryRecord[] historyRecordArray;
  public int historyRecordCursorIndex;
  public GameObject cameraProxyPlane;
  public Camera compositionCamera;
  public OVRSandwichComposition.OVRSandwichCompositionManager compositionManager;
  private int _cameraFramePlaneLayer = -1;

  public int cameraFramePlaneLayer
  {
    get
    {
      if (this._cameraFramePlaneLayer < 0)
      {
        for (int layer = 24; layer <= 29; ++layer)
        {
          switch (LayerMask.LayerToName(layer))
          {
            case "":
            case null:
              this._cameraFramePlaneLayer = layer;
              goto label_6;
            default:
              continue;
          }
        }
label_6:
        if (this._cameraFramePlaneLayer == -1)
        {
          Debug.LogWarning((object) "Unable to find an unnamed layer between 24 and 29.");
          this._cameraFramePlaneLayer = 25;
        }
        Debug.LogFormat("Set the CameraFramePlaneLayer in SandwichComposition to {0}. Please do NOT put any other gameobject in this layer.", (object) this._cameraFramePlaneLayer);
      }
      return this._cameraFramePlaneLayer;
    }
  }

  public override OVRManager.CompositionMethod CompositionMethod() => OVRManager.CompositionMethod.Sandwich;

  public OVRSandwichComposition(
    GameObject parentObject,
    Camera mainCamera,
    OVRManager.CameraDevice cameraDevice,
    bool useDynamicLighting,
    OVRManager.DepthQuality depthQuality)
    : base(cameraDevice, useDynamicLighting, depthQuality)
  {
    this.frameRealtime = Time.realtimeSinceStartup;
    this.historyRecordCount = OVRManager.instance.sandwichCompositionBufferedFrames;
    if (this.historyRecordCount < 1)
    {
      Debug.LogWarning((object) "Invalid sandwichCompositionBufferedFrames in OVRManager. It should be at least 1");
      this.historyRecordCount = 1;
    }
    if (this.historyRecordCount > 16)
    {
      Debug.LogWarning((object) "The value of sandwichCompositionBufferedFrames in OVRManager is too big. It would consume a lot of memory. It has been override to 16");
      this.historyRecordCount = 16;
    }
    this.historyRecordArray = new OVRSandwichComposition.HistoryRecord[this.historyRecordCount];
    for (int index = 0; index < this.historyRecordCount; ++index)
      this.historyRecordArray[index] = new OVRSandwichComposition.HistoryRecord();
    this.historyRecordCursorIndex = 0;
    this.fgCamera = new GameObject("MRSandwichForegroundCamera")
    {
      transform = {
        parent = parentObject.transform
      }
    }.AddComponent<Camera>();
    this.fgCamera.depth = 200f;
    this.fgCamera.clearFlags = CameraClearFlags.Color;
    this.fgCamera.backgroundColor = Color.clear;
    this.fgCamera.cullingMask = mainCamera.cullingMask & ~(int) OVRManager.instance.extraHiddenLayers;
    this.fgCamera.nearClipPlane = mainCamera.nearClipPlane;
    this.fgCamera.farClipPlane = mainCamera.farClipPlane;
    this.bgCamera = new GameObject("MRSandwichBackgroundCamera")
    {
      transform = {
        parent = parentObject.transform
      }
    }.AddComponent<Camera>();
    this.bgCamera.depth = 100f;
    this.bgCamera.clearFlags = mainCamera.clearFlags;
    this.bgCamera.backgroundColor = mainCamera.backgroundColor;
    this.bgCamera.cullingMask = mainCamera.cullingMask & ~(int) OVRManager.instance.extraHiddenLayers;
    this.bgCamera.nearClipPlane = mainCamera.nearClipPlane;
    this.bgCamera.farClipPlane = mainCamera.farClipPlane;
    this.cameraProxyPlane = GameObject.CreatePrimitive(PrimitiveType.Quad);
    this.cameraProxyPlane.name = "MRProxyClipPlane";
    this.cameraProxyPlane.transform.parent = parentObject.transform;
    this.cameraProxyPlane.GetComponent<Collider>().enabled = false;
    this.cameraProxyPlane.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.Off;
    Material material = new Material(Shader.Find("Oculus/OVRMRClipPlane"));
    this.cameraProxyPlane.GetComponent<MeshRenderer>().material = material;
    material.SetColor("_Color", Color.clear);
    material.SetFloat("_Visible", 0.0f);
    this.cameraProxyPlane.transform.localScale = new Vector3(1000f, 1000f, 1000f);
    this.cameraProxyPlane.SetActive(true);
    this.fgCamera.gameObject.AddComponent<OVRMRForegroundCameraManager>().clipPlaneGameObj = this.cameraProxyPlane;
    this.compositionCamera = new GameObject("MRSandwichCaptureCamera")
    {
      transform = {
        parent = parentObject.transform
      }
    }.AddComponent<Camera>();
    this.compositionCamera.stereoTargetEye = StereoTargetEyeMask.None;
    this.compositionCamera.depth = float.MaxValue;
    this.compositionCamera.rect = new Rect(0.0f, 0.0f, 1f, 1f);
    this.compositionCamera.clearFlags = CameraClearFlags.Depth;
    this.compositionCamera.backgroundColor = mainCamera.backgroundColor;
    this.compositionCamera.cullingMask = 1 << this.cameraFramePlaneLayer;
    this.compositionCamera.nearClipPlane = mainCamera.nearClipPlane;
    this.compositionCamera.farClipPlane = mainCamera.farClipPlane;
    if (!this.hasCameraDeviceOpened)
    {
      Debug.LogError((object) ("Unable to open camera device " + (object) cameraDevice));
    }
    else
    {
      Debug.Log((object) ("SandwichComposition activated : useDynamicLighting " + (useDynamicLighting ? "ON" : "OFF")));
      this.CreateCameraFramePlaneObject(parentObject, this.compositionCamera, useDynamicLighting);
      this.cameraFramePlaneObject.layer = this.cameraFramePlaneLayer;
      this.RefreshRenderTextures(mainCamera);
      this.compositionManager = this.compositionCamera.gameObject.AddComponent<OVRSandwichComposition.OVRSandwichCompositionManager>();
      this.compositionManager.fgTexture = this.historyRecordArray[this.historyRecordCursorIndex].fgRenderTexture;
      this.compositionManager.bgTexture = this.historyRecordArray[this.historyRecordCursorIndex].bgRenderTexture;
    }
  }

  public override void Update(Camera mainCamera)
  {
    if (!this.hasCameraDeviceOpened)
      return;
    this.frameRealtime = Time.realtimeSinceStartup;
    ++this.historyRecordCursorIndex;
    if (this.historyRecordCursorIndex >= this.historyRecordCount)
      this.historyRecordCursorIndex = 0;
    if (!OVRPlugin.SetHandNodePoseStateLatency((double) OVRManager.instance.handPoseStateLatency))
      Debug.LogWarning((object) ("HandPoseStateLatency is invalid. Expect a value between 0.0 to 0.5, get " + (object) OVRManager.instance.handPoseStateLatency));
    this.RefreshRenderTextures(mainCamera);
    this.bgCamera.clearFlags = mainCamera.clearFlags;
    this.bgCamera.backgroundColor = mainCamera.backgroundColor;
    this.bgCamera.cullingMask = mainCamera.cullingMask & ~(int) OVRManager.instance.extraHiddenLayers;
    this.fgCamera.cullingMask = mainCamera.cullingMask & ~(int) OVRManager.instance.extraHiddenLayers;
    if (OVRMixedReality.useFakeExternalCamera || OVRPlugin.GetExternalCameraCount() == 0)
    {
      OVRPose ovrPose = new OVRPose();
      OVRPose worldSpacePose = OVRExtensions.ToWorldSpacePose(new OVRPose()
      {
        position = OVRMixedReality.fakeCameraPositon,
        orientation = OVRMixedReality.fakeCameraRotation
      });
      this.RefreshCameraPoses(OVRMixedReality.fakeCameraFov, OVRMixedReality.fakeCameraAspect, worldSpacePose);
    }
    else
    {
      OVRPlugin.CameraExtrinsics cameraExtrinsics;
      OVRPlugin.CameraIntrinsics cameraIntrinsics;
      if (OVRPlugin.GetMixedRealityCameraInfo(0, out cameraExtrinsics, out cameraIntrinsics))
      {
        OVRPose cameraWorldSpacePose = this.ComputeCameraWorldSpacePose(cameraExtrinsics);
        this.RefreshCameraPoses((float) ((double) Mathf.Atan(cameraIntrinsics.FOVPort.UpTan) * 57.2957801818848 * 2.0), cameraIntrinsics.FOVPort.LeftTan / cameraIntrinsics.FOVPort.UpTan, cameraWorldSpacePose);
      }
      else
        Debug.LogWarning((object) "Failed to get external camera information");
    }
    this.compositionCamera.GetComponent<OVRCameraComposition.OVRCameraFrameCompositionManager>().boundaryMeshMaskTexture = this.historyRecordArray[this.historyRecordCursorIndex].boundaryMeshMaskTexture;
    OVRSandwichComposition.HistoryRecord recordForComposition = this.GetHistoryRecordForComposition();
    this.UpdateCameraFramePlaneObject(mainCamera, this.compositionCamera, recordForComposition.boundaryMeshMaskTexture);
    OVRSandwichComposition.OVRSandwichCompositionManager component = this.compositionCamera.gameObject.GetComponent<OVRSandwichComposition.OVRSandwichCompositionManager>();
    component.fgTexture = recordForComposition.fgRenderTexture;
    component.bgTexture = recordForComposition.bgRenderTexture;
    this.cameraProxyPlane.transform.position = this.fgCamera.transform.position + this.fgCamera.transform.forward * this.cameraFramePlaneDistance;
    this.cameraProxyPlane.transform.LookAt(this.cameraProxyPlane.transform.position + this.fgCamera.transform.forward);
  }

  public override void Cleanup()
  {
    base.Cleanup();
    Camera[] cameraArray = new Camera[3]
    {
      this.fgCamera,
      this.bgCamera,
      this.compositionCamera
    };
    foreach (Component component in cameraArray)
      OVRCompositionUtil.SafeDestroy(component.gameObject);
    this.fgCamera = (Camera) null;
    this.bgCamera = (Camera) null;
    this.compositionCamera = (Camera) null;
    Debug.Log((object) "SandwichComposition deactivated");
  }

  private RenderTextureFormat DesiredRenderTextureFormat(
    RenderTextureFormat originalFormat)
  {
    if (originalFormat == RenderTextureFormat.RGB565)
      return RenderTextureFormat.ARGB1555;
    return originalFormat == RenderTextureFormat.RGB111110Float ? RenderTextureFormat.ARGBHalf : originalFormat;
  }

  protected void RefreshRenderTextures(Camera mainCamera)
  {
    int width = Screen.width;
    int height = Screen.height;
    RenderTextureFormat format = (bool) (Object) mainCamera.targetTexture ? this.DesiredRenderTextureFormat(mainCamera.targetTexture.format) : RenderTextureFormat.ARGB32;
    int depth = (bool) (Object) mainCamera.targetTexture ? mainCamera.targetTexture.depth : 24;
    OVRSandwichComposition.HistoryRecord historyRecord = this.historyRecordArray[this.historyRecordCursorIndex];
    historyRecord.timestamp = this.frameRealtime;
    if ((Object) historyRecord.fgRenderTexture == (Object) null || historyRecord.fgRenderTexture.width != width || historyRecord.fgRenderTexture.height != height || historyRecord.fgRenderTexture.format != format || historyRecord.fgRenderTexture.depth != depth)
    {
      historyRecord.fgRenderTexture = new RenderTexture(width, height, depth, format);
      historyRecord.fgRenderTexture.name = "Sandwich FG " + this.historyRecordCursorIndex.ToString();
    }
    this.fgCamera.targetTexture = historyRecord.fgRenderTexture;
    if ((Object) historyRecord.bgRenderTexture == (Object) null || historyRecord.bgRenderTexture.width != width || historyRecord.bgRenderTexture.height != height || historyRecord.bgRenderTexture.format != format || historyRecord.bgRenderTexture.depth != depth)
    {
      historyRecord.bgRenderTexture = new RenderTexture(width, height, depth, format);
      historyRecord.bgRenderTexture.name = "Sandwich BG " + this.historyRecordCursorIndex.ToString();
    }
    this.bgCamera.targetTexture = historyRecord.bgRenderTexture;
    if (OVRManager.instance.virtualGreenScreenType != OVRManager.VirtualGreenScreenType.Off)
    {
      if (!((Object) historyRecord.boundaryMeshMaskTexture == (Object) null) && historyRecord.boundaryMeshMaskTexture.width == width && historyRecord.boundaryMeshMaskTexture.height == height)
        return;
      historyRecord.boundaryMeshMaskTexture = new RenderTexture(width, height, 0, RenderTextureFormat.R8);
      historyRecord.boundaryMeshMaskTexture.name = "Boundary Mask " + this.historyRecordCursorIndex.ToString();
      historyRecord.boundaryMeshMaskTexture.Create();
    }
    else
      historyRecord.boundaryMeshMaskTexture = (RenderTexture) null;
  }

  protected OVRSandwichComposition.HistoryRecord GetHistoryRecordForComposition()
  {
    float num = this.frameRealtime - OVRManager.instance.sandwichCompositionRenderLatency;
    int index1 = this.historyRecordCursorIndex;
    int index2 = index1 - 1;
    if (index2 < 0)
      index2 = this.historyRecordCount - 1;
    while (index2 != this.historyRecordCursorIndex)
    {
      if ((double) this.historyRecordArray[index2].timestamp <= (double) num)
        return (double) this.historyRecordArray[index1].timestamp - (double) num > (double) (num - this.historyRecordArray[index2].timestamp) ? this.historyRecordArray[index2] : this.historyRecordArray[index1];
      index1 = index2--;
      if (index2 < 0)
        index2 = this.historyRecordCount - 1;
    }
    return this.historyRecordArray[index1];
  }

  protected void RefreshCameraPoses(float fovY, float aspect, OVRPose pose)
  {
    Camera[] cameraArray = new Camera[3]
    {
      this.fgCamera,
      this.bgCamera,
      this.compositionCamera
    };
    foreach (Camera camera in cameraArray)
    {
      camera.fieldOfView = fovY;
      camera.aspect = aspect;
      camera.transform.FromOVRPose(pose);
    }
  }

  public class HistoryRecord
  {
    public float timestamp = float.MinValue;
    public RenderTexture fgRenderTexture;
    public RenderTexture bgRenderTexture;
    public RenderTexture boundaryMeshMaskTexture;
  }

  public class OVRSandwichCompositionManager : MonoBehaviour
  {
    public RenderTexture fgTexture;
    public RenderTexture bgTexture;
    public Material alphaBlendMaterial;

    private void Start()
    {
      Shader shader = Shader.Find("Oculus/UnlitTransparent");
      if ((Object) shader == (Object) null)
        Debug.LogError((object) "Unable to create transparent shader");
      else
        this.alphaBlendMaterial = new Material(shader);
    }

    private void OnPreRender()
    {
      if ((Object) this.fgTexture == (Object) null || (Object) this.bgTexture == (Object) null || (Object) this.alphaBlendMaterial == (Object) null)
        Debug.LogError((object) "OVRSandwichCompositionManager has not setup properly");
      else
        Graphics.Blit((Texture) this.bgTexture, RenderTexture.active);
    }

    private void OnPostRender()
    {
      if ((Object) this.fgTexture == (Object) null || (Object) this.bgTexture == (Object) null || (Object) this.alphaBlendMaterial == (Object) null)
        Debug.LogError((object) "OVRSandwichCompositionManager has not setup properly");
      else
        Graphics.Blit((Texture) this.fgTexture, RenderTexture.active, this.alphaBlendMaterial);
    }
  }
}
