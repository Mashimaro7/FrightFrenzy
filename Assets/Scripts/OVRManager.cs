// Decompiled with JetBrains decompiler
// Type: OVRManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR;

public class OVRManager : MonoBehaviour
{
  private static OVRProfile _profile;
  private IEnumerable<Camera> disabledCameras;
  private float prevTimeScale;
  private static bool _isHmdPresentCached = false;
  private static bool _isHmdPresent = false;
  private static bool _wasHmdPresent = false;
  private static bool _hasVrFocusCached = false;
  private static bool _hasVrFocus = false;
  private static bool _hadVrFocus = false;
  private static bool _hadInputFocus = true;
  [Header("Performance/Quality")]
  [Tooltip("If true, distortion rendering work is submitted a quarter-frame early to avoid pipeline stalls and increase CPU-GPU parallelism.")]
  public bool queueAhead = true;
  [Tooltip("If true, Unity will use the optimal antialiasing level for quality/performance on the current hardware.")]
  public bool useRecommendedMSAALevel;
  [Tooltip("If true, dynamic resolution will be enabled On PC")]
  public bool enableAdaptiveResolution;
  [Range(0.5f, 2f)]
  [Tooltip("Min RenderScale the app can reach under adaptive resolution mode")]
  public float minRenderScale = 0.7f;
  [Range(0.5f, 2f)]
  [Tooltip("Max RenderScale the app can reach under adaptive resolution mode")]
  public float maxRenderScale = 1f;
  [HideInInspector]
  public bool expandMixedRealityCapturePropertySheet;
  [HideInInspector]
  [Tooltip("If true, Mixed Reality mode will be enabled. It would be always set to false when the game is launching without editor")]
  public bool enableMixedReality;
  [HideInInspector]
  public OVRManager.CompositionMethod compositionMethod;
  [HideInInspector]
  [Tooltip("Extra hidden layers")]
  public LayerMask extraHiddenLayers;
  [HideInInspector]
  [Tooltip("The camera device for direct composition")]
  public OVRManager.CameraDevice capturingCameraDevice;
  [HideInInspector]
  [Tooltip("Flip the camera frame horizontally")]
  public bool flipCameraFrameHorizontally;
  [HideInInspector]
  [Tooltip("Flip the camera frame vertically")]
  public bool flipCameraFrameVertically;
  [HideInInspector]
  [Tooltip("Delay the touch controller pose by a short duration (0 to 0.5 second) to match the physical camera latency")]
  public float handPoseStateLatency;
  [HideInInspector]
  [Tooltip("Delay the foreground / background image in the sandwich composition to match the physical camera latency. The maximum duration is sandwichCompositionBufferedFrames / {Game FPS}")]
  public float sandwichCompositionRenderLatency;
  [HideInInspector]
  [Tooltip("The number of frames are buffered in the SandWich composition. The more buffered frames, the more memory it would consume.")]
  public int sandwichCompositionBufferedFrames = 8;
  [HideInInspector]
  [Tooltip("Chroma Key Color")]
  public Color chromaKeyColor = Color.green;
  [HideInInspector]
  [Tooltip("Chroma Key Similarity")]
  public float chromaKeySimilarity = 0.6f;
  [HideInInspector]
  [Tooltip("Chroma Key Smooth Range")]
  public float chromaKeySmoothRange = 0.03f;
  [HideInInspector]
  [Tooltip("Chroma Key Spill Range")]
  public float chromaKeySpillRange = 0.06f;
  [HideInInspector]
  [Tooltip("Use dynamic lighting (Depth sensor required)")]
  public bool useDynamicLighting;
  [HideInInspector]
  [Tooltip("The quality level of depth image. The lighting could be more smooth and accurate with high quality depth, but it would also be more costly in performance.")]
  public OVRManager.DepthQuality depthQuality = OVRManager.DepthQuality.Medium;
  [HideInInspector]
  [Tooltip("Smooth factor in dynamic lighting. Larger is smoother")]
  public float dynamicLightingSmoothFactor = 8f;
  [HideInInspector]
  [Tooltip("The maximum depth variation across the edges. Make it smaller to smooth the lighting on the edges.")]
  public float dynamicLightingDepthVariationClampingValue = 1f / 1000f;
  [HideInInspector]
  [Tooltip("Type of virutal green screen ")]
  public OVRManager.VirtualGreenScreenType virtualGreenScreenType;
  [HideInInspector]
  [Tooltip("Top Y of virtual green screen")]
  public float virtualGreenScreenTopY = 10f;
  [HideInInspector]
  [Tooltip("Bottom Y of virtual green screen")]
  public float virtualGreenScreenBottomY = -10f;
  [HideInInspector]
  [Tooltip("When using a depth camera (e.g. ZED), whether to use the depth in virtual green screen culling.")]
  public bool virtualGreenScreenApplyDepthCulling;
  [HideInInspector]
  [Tooltip("The tolerance value (in meter) when using the virtual green screen with a depth camera. Make it bigger if the foreground objects got culled incorrectly.")]
  public float virtualGreenScreenDepthTolerance = 0.2f;
  [Header("Tracking")]
  [SerializeField]
  [Tooltip("Defines the current tracking origin type.")]
  private OVRManager.TrackingOrigin _trackingOriginType;
  [Tooltip("If true, head tracking will affect the position of each OVRCameraRig's cameras.")]
  public bool usePositionTracking = true;
  [HideInInspector]
  public bool useRotationTracking = true;
  [Tooltip("If true, the distance between the user's eyes will affect the position of each OVRCameraRig's cameras.")]
  public bool useIPDInPositionTracking = true;
  [Tooltip("If true, each scene load will cause the head pose to reset.")]
  public bool resetTrackerOnLoad;
  [Tooltip("If true, the Reset View in the universal menu will cause the pose to be reset. This should generally be enabled for applications with a stationary position in the virtual world and will allow the View Reset command to place the person back to a predefined location (such as a cockpit seat). Set this to false if you have a locomotion system because resetting the view would effectively teleport the player to potentially invalid locations.")]
  public bool AllowRecenter = true;
  private static bool _isUserPresentCached = false;
  private static bool _isUserPresent = false;
  private static bool _wasUserPresent = false;
  private static bool prevAudioOutIdIsCached = false;
  private static bool prevAudioInIdIsCached = false;
  private static string prevAudioOutId = string.Empty;
  private static string prevAudioInId = string.Empty;
  private static bool wasPositionTracked = false;
  private static bool prevEnableMixedReality = false;
  private bool suppressDisableMixedRealityBecauseOfNoMainCameraWarning;
  private bool multipleMainCameraWarningPresented;

  public static OVRManager instance { get; private set; }

  public static OVRDisplay display { get; private set; }

  public static OVRTracker tracker { get; private set; }

  public static OVRBoundary boundary { get; private set; }

  public static OVRProfile profile
  {
    get
    {
      if ((UnityEngine.Object) OVRManager._profile == (UnityEngine.Object) null)
        OVRManager._profile = new OVRProfile();
      return OVRManager._profile;
    }
  }

  public static event Action HMDAcquired;

  public static event Action HMDLost;

  public static event Action HMDMounted;

  public static event Action HMDUnmounted;

  public static event Action VrFocusAcquired;

  public static event Action VrFocusLost;

  public static event Action InputFocusAcquired;

  public static event Action InputFocusLost;

  public static event Action AudioOutChanged;

  public static event Action AudioInChanged;

  public static event Action TrackingAcquired;

  public static event Action TrackingLost;

  [Obsolete]
  public static event Action HSWDismissed;

  public static bool isHmdPresent
  {
    get
    {
      if (!OVRManager._isHmdPresentCached)
      {
        OVRManager._isHmdPresentCached = true;
        OVRManager._isHmdPresent = OVRPlugin.hmdPresent;
      }
      return OVRManager._isHmdPresent;
    }
    private set
    {
      OVRManager._isHmdPresentCached = true;
      OVRManager._isHmdPresent = value;
    }
  }

  public static string audioOutId => OVRPlugin.audioOutId;

  public static string audioInId => OVRPlugin.audioInId;

  public static bool hasVrFocus
  {
    get
    {
      if (!OVRManager._hasVrFocusCached)
      {
        OVRManager._hasVrFocusCached = true;
        OVRManager._hasVrFocus = OVRPlugin.hasVrFocus;
      }
      return OVRManager._hasVrFocus;
    }
    private set
    {
      OVRManager._hasVrFocusCached = true;
      OVRManager._hasVrFocus = value;
    }
  }

  public static bool hasInputFocus => OVRPlugin.hasInputFocus;

  public bool chromatic
  {
    get => OVRManager.isHmdPresent && OVRPlugin.chromatic;
    set
    {
      if (!OVRManager.isHmdPresent)
        return;
      OVRPlugin.chromatic = value;
    }
  }

  public bool monoscopic
  {
    get => !OVRManager.isHmdPresent || OVRPlugin.monoscopic;
    set
    {
      if (!OVRManager.isHmdPresent)
        return;
      OVRPlugin.monoscopic = value;
    }
  }

  public static bool IsAdaptiveResSupportedByEngine() => Application.unityVersion != "2017.1.0f1";

  public int vsyncCount
  {
    get => !OVRManager.isHmdPresent ? 1 : OVRPlugin.vsyncCount;
    set
    {
      if (!OVRManager.isHmdPresent)
        return;
      OVRPlugin.vsyncCount = value;
    }
  }

  public static float batteryLevel => !OVRManager.isHmdPresent ? 1f : OVRPlugin.batteryLevel;

  public static float batteryTemperature => !OVRManager.isHmdPresent ? 0.0f : OVRPlugin.batteryTemperature;

  public static int batteryStatus => !OVRManager.isHmdPresent ? -1 : (int) OVRPlugin.batteryStatus;

  public static float volumeLevel => !OVRManager.isHmdPresent ? 0.0f : OVRPlugin.systemVolume;

  public static int cpuLevel
  {
    get => !OVRManager.isHmdPresent ? 2 : OVRPlugin.cpuLevel;
    set
    {
      if (!OVRManager.isHmdPresent)
        return;
      OVRPlugin.cpuLevel = value;
    }
  }

  public static int gpuLevel
  {
    get => !OVRManager.isHmdPresent ? 2 : OVRPlugin.gpuLevel;
    set
    {
      if (!OVRManager.isHmdPresent)
        return;
      OVRPlugin.gpuLevel = value;
    }
  }

  public static bool isPowerSavingActive => OVRManager.isHmdPresent && OVRPlugin.powerSaving;

  public static OVRManager.EyeTextureFormat eyeTextureFormat
  {
    get => (OVRManager.EyeTextureFormat) OVRPlugin.GetDesiredEyeTextureFormat();
    set => OVRPlugin.SetDesiredEyeTextureFormat((OVRPlugin.EyeTextureFormat) value);
  }

  public static bool tiledMultiResSupported => OVRPlugin.tiledMultiResSupported;

  public static OVRManager.TiledMultiResLevel tiledMultiResLevel
  {
    get
    {
      if (!OVRPlugin.tiledMultiResSupported)
        Debug.LogWarning((object) "Tiled-based Multi-resolution feature is not supported");
      return (OVRManager.TiledMultiResLevel) OVRPlugin.tiledMultiResLevel;
    }
    set
    {
      if (!OVRPlugin.tiledMultiResSupported)
        Debug.LogWarning((object) "Tiled-based Multi-resolution feature is not supported");
      OVRPlugin.tiledMultiResLevel = (OVRPlugin.TiledMultiResLevel) value;
    }
  }

  public static bool gpuUtilSupported => OVRPlugin.gpuUtilSupported;

  public static float gpuUtilLevel
  {
    get
    {
      if (!OVRPlugin.gpuUtilSupported)
        Debug.LogWarning((object) "GPU Util is not supported");
      return OVRPlugin.gpuUtilLevel;
    }
  }

  public OVRManager.TrackingOrigin trackingOriginType
  {
    get => !OVRManager.isHmdPresent ? this._trackingOriginType : (OVRManager.TrackingOrigin) OVRPlugin.GetTrackingOriginType();
    set
    {
      if (!OVRManager.isHmdPresent || !OVRPlugin.SetTrackingOriginType((OVRPlugin.TrackingOrigin) value))
        return;
      this._trackingOriginType = value;
    }
  }

  public bool isSupportedPlatform { get; private set; }

  public bool isUserPresent
  {
    get
    {
      if (!OVRManager._isUserPresentCached)
      {
        OVRManager._isUserPresentCached = true;
        OVRManager._isUserPresent = OVRPlugin.userPresent;
      }
      return OVRManager._isUserPresent;
    }
    private set
    {
      OVRManager._isUserPresentCached = true;
      OVRManager._isUserPresent = value;
    }
  }

  public static Version utilitiesVersion => OVRPlugin.wrapperVersion;

  public static Version pluginVersion => OVRPlugin.version;

  public static Version sdkVersion => OVRPlugin.nativeSDKVersion;

  private static bool MixedRealityEnabledFromCmd()
  {
    foreach (string commandLineArg in Environment.GetCommandLineArgs())
    {
      if (commandLineArg.ToLower() == "-mixedreality")
        return true;
    }
    return false;
  }

  private static bool UseDirectCompositionFromCmd()
  {
    foreach (string commandLineArg in Environment.GetCommandLineArgs())
    {
      if (commandLineArg.ToLower() == "-directcomposition")
        return true;
    }
    return false;
  }

  private static bool UseExternalCompositionFromCmd()
  {
    foreach (string commandLineArg in Environment.GetCommandLineArgs())
    {
      if (commandLineArg.ToLower() == "-externalcomposition")
        return true;
    }
    return false;
  }

  private static bool CreateMixedRealityCaptureConfigurationFileFromCmd()
  {
    foreach (string commandLineArg in Environment.GetCommandLineArgs())
    {
      if (commandLineArg.ToLower() == "-create_mrc_config")
        return true;
    }
    return false;
  }

  private static bool LoadMixedRealityCaptureConfigurationFileFromCmd()
  {
    foreach (string commandLineArg in Environment.GetCommandLineArgs())
    {
      if (commandLineArg.ToLower() == "-load_mrc_config")
        return true;
    }
    return false;
  }

  private void Awake()
  {
    if (Application.isBatchMode)
      return;
    if ((UnityEngine.Object) OVRManager.instance != (UnityEngine.Object) null)
    {
      this.enabled = false;
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this);
    }
    else
    {
      OVRManager.instance = this;
      Debug.Log((object) ("Unity v" + Application.unityVersion + ", Oculus Utilities v" + (object) OVRPlugin.wrapperVersion + ", OVRPlugin v" + (object) OVRPlugin.version + ", SDK v" + (object) OVRPlugin.nativeSDKVersion + "."));
      GraphicsDeviceType graphicsDeviceType1 = GraphicsDeviceType.Direct3D11;
      string str1 = graphicsDeviceType1.ToString();
      graphicsDeviceType1 = GraphicsDeviceType.Direct3D12;
      string str2 = graphicsDeviceType1.ToString();
      string str3 = str1 + ", " + str2;
      string str4 = str3;
      GraphicsDeviceType graphicsDeviceType2 = SystemInfo.graphicsDeviceType;
      string str5 = graphicsDeviceType2.ToString();
      if (!str4.Contains(str5))
      {
        string str6 = str3;
        graphicsDeviceType2 = SystemInfo.graphicsDeviceType;
        string str7 = graphicsDeviceType2.ToString();
        Debug.LogWarning((object) ("VR rendering requires one of the following device types: (" + str6 + "). Your graphics device: " + str7));
      }
      RuntimePlatform platform = Application.platform;
      this.isSupportedPlatform |= platform == RuntimePlatform.Android;
      this.isSupportedPlatform |= platform == RuntimePlatform.OSXEditor;
      this.isSupportedPlatform |= platform == RuntimePlatform.OSXPlayer;
      this.isSupportedPlatform |= platform == RuntimePlatform.WindowsEditor;
      this.isSupportedPlatform |= platform == RuntimePlatform.WindowsPlayer;
      if (!this.isSupportedPlatform)
      {
        Debug.LogWarning((object) "This platform is unsupported");
      }
      else
      {
        this.enableMixedReality = false;
        bool flag = OVRManager.LoadMixedRealityCaptureConfigurationFileFromCmd();
        bool configurationFileFromCmd = OVRManager.CreateMixedRealityCaptureConfigurationFileFromCmd();
        if (flag | configurationFileFromCmd)
        {
          OVRMixedRealityCaptureSettings instance = ScriptableObject.CreateInstance<OVRMixedRealityCaptureSettings>();
          instance.ReadFrom(this);
          if (flag)
          {
            instance.CombineWithConfigurationFile();
            instance.ApplyTo(this);
          }
          if (configurationFileFromCmd)
            instance.WriteToConfigurationFile();
          UnityEngine.Object.Destroy((UnityEngine.Object) instance);
        }
        if (OVRManager.MixedRealityEnabledFromCmd())
          this.enableMixedReality = true;
        if (this.enableMixedReality)
        {
          Debug.Log((object) "OVR: Mixed Reality mode enabled");
          if (OVRManager.UseDirectCompositionFromCmd())
            this.compositionMethod = OVRManager.CompositionMethod.Direct;
          if (OVRManager.UseExternalCompositionFromCmd())
            this.compositionMethod = OVRManager.CompositionMethod.External;
          Debug.Log((object) ("OVR: CompositionMethod : " + (object) this.compositionMethod));
        }
        if (this.enableAdaptiveResolution && !OVRManager.IsAdaptiveResSupportedByEngine())
        {
          this.enableAdaptiveResolution = false;
          Debug.LogError((object) ("Your current Unity Engine " + Application.unityVersion + " might have issues to support adaptive resolution, please disable it under OVRManager"));
        }
        this.Initialize();
        if (this.resetTrackerOnLoad)
          OVRManager.display.RecenterPose();
        OVRPlugin.occlusionMesh = true;
      }
    }
  }

  private void Initialize()
  {
    if (OVRManager.display == null)
      OVRManager.display = new OVRDisplay();
    if (OVRManager.tracker == null)
      OVRManager.tracker = new OVRTracker();
    if (OVRManager.boundary != null)
      return;
    OVRManager.boundary = new OVRBoundary();
  }

  private void Update()
  {
    if (Application.isBatchMode)
      return;
    if (OVRPlugin.shouldQuit)
      Application.Quit();
    if (this.AllowRecenter && OVRPlugin.shouldRecenter)
      OVRManager.display.RecenterPose();
    if (this.trackingOriginType != this._trackingOriginType)
      this.trackingOriginType = this._trackingOriginType;
    OVRManager.tracker.isEnabled = this.usePositionTracking;
    OVRPlugin.rotation = this.useRotationTracking;
    OVRPlugin.useIPDInPositionTracking = this.useIPDInPositionTracking;
    OVRManager.isHmdPresent = OVRPlugin.hmdPresent;
    if (this.useRecommendedMSAALevel && QualitySettings.antiAliasing != OVRManager.display.recommendedMSAALevel)
    {
      Debug.Log((object) ("The current MSAA level is " + (object) QualitySettings.antiAliasing + ", but the recommended MSAA level is " + (object) OVRManager.display.recommendedMSAALevel + ". Switching to the recommended level."));
      QualitySettings.antiAliasing = OVRManager.display.recommendedMSAALevel;
    }
    if (OVRManager._wasHmdPresent)
    {
      if (!OVRManager.isHmdPresent)
      {
        try
        {
          if (OVRManager.HMDLost != null)
            OVRManager.HMDLost();
        }
        catch (Exception ex)
        {
          Debug.LogError((object) ("Caught Exception: " + (object) ex));
        }
      }
    }
    if (!OVRManager._wasHmdPresent)
    {
      if (OVRManager.isHmdPresent)
      {
        try
        {
          if (OVRManager.HMDAcquired != null)
            OVRManager.HMDAcquired();
        }
        catch (Exception ex)
        {
          Debug.LogError((object) ("Caught Exception: " + (object) ex));
        }
      }
    }
    OVRManager._wasHmdPresent = OVRManager.isHmdPresent;
    this.isUserPresent = OVRPlugin.userPresent;
    if (OVRManager._wasUserPresent)
    {
      if (!this.isUserPresent)
      {
        try
        {
          if (OVRManager.HMDUnmounted != null)
            OVRManager.HMDUnmounted();
        }
        catch (Exception ex)
        {
          Debug.LogError((object) ("Caught Exception: " + (object) ex));
        }
      }
    }
    if (!OVRManager._wasUserPresent)
    {
      if (this.isUserPresent)
      {
        try
        {
          if (OVRManager.HMDMounted != null)
            OVRManager.HMDMounted();
        }
        catch (Exception ex)
        {
          Debug.LogError((object) ("Caught Exception: " + (object) ex));
        }
      }
    }
    OVRManager._wasUserPresent = this.isUserPresent;
    OVRManager.hasVrFocus = OVRPlugin.hasVrFocus;
    if (OVRManager._hadVrFocus)
    {
      if (!OVRManager.hasVrFocus)
      {
        try
        {
          if (OVRManager.VrFocusLost != null)
            OVRManager.VrFocusLost();
        }
        catch (Exception ex)
        {
          Debug.LogError((object) ("Caught Exception: " + (object) ex));
        }
      }
    }
    if (!OVRManager._hadVrFocus)
    {
      if (OVRManager.hasVrFocus)
      {
        try
        {
          if (OVRManager.VrFocusAcquired != null)
            OVRManager.VrFocusAcquired();
        }
        catch (Exception ex)
        {
          Debug.LogError((object) ("Caught Exception: " + (object) ex));
        }
      }
    }
    OVRManager._hadVrFocus = OVRManager.hasVrFocus;
    bool hasInputFocus = OVRPlugin.hasInputFocus;
    if (OVRManager._hadInputFocus)
    {
      if (!hasInputFocus)
      {
        try
        {
          if (OVRManager.InputFocusLost != null)
            OVRManager.InputFocusLost();
        }
        catch (Exception ex)
        {
          Debug.LogError((object) ("Caught Exception: " + (object) ex));
        }
      }
    }
    if (!OVRManager._hadInputFocus & hasInputFocus)
    {
      try
      {
        if (OVRManager.InputFocusAcquired != null)
          OVRManager.InputFocusAcquired();
      }
      catch (Exception ex)
      {
        Debug.LogError((object) ("Caught Exception: " + (object) ex));
      }
    }
    OVRManager._hadInputFocus = hasInputFocus;
    if (this.enableAdaptiveResolution)
    {
      if ((double) XRSettings.eyeTextureResolutionScale < (double) this.maxRenderScale)
        XRSettings.eyeTextureResolutionScale = this.maxRenderScale;
      else
        this.maxRenderScale = Mathf.Max(this.maxRenderScale, XRSettings.eyeTextureResolutionScale);
      this.minRenderScale = Mathf.Min(this.minRenderScale, this.maxRenderScale);
      float min = this.minRenderScale / XRSettings.eyeTextureResolutionScale;
      XRSettings.renderViewportScale = Mathf.Clamp(OVRPlugin.GetEyeRecommendedResolutionScale() / XRSettings.eyeTextureResolutionScale, min, 1f);
    }
    string audioOutId = OVRPlugin.audioOutId;
    if (!OVRManager.prevAudioOutIdIsCached)
    {
      OVRManager.prevAudioOutId = audioOutId;
      OVRManager.prevAudioOutIdIsCached = true;
    }
    else if (audioOutId != OVRManager.prevAudioOutId)
    {
      try
      {
        if (OVRManager.AudioOutChanged != null)
          OVRManager.AudioOutChanged();
      }
      catch (Exception ex)
      {
        Debug.LogError((object) ("Caught Exception: " + (object) ex));
      }
      OVRManager.prevAudioOutId = audioOutId;
    }
    string audioInId = OVRPlugin.audioInId;
    if (!OVRManager.prevAudioInIdIsCached)
    {
      OVRManager.prevAudioInId = audioInId;
      OVRManager.prevAudioInIdIsCached = true;
    }
    else if (audioInId != OVRManager.prevAudioInId)
    {
      try
      {
        if (OVRManager.AudioInChanged != null)
          OVRManager.AudioInChanged();
      }
      catch (Exception ex)
      {
        Debug.LogError((object) ("Caught Exception: " + (object) ex));
      }
      OVRManager.prevAudioInId = audioInId;
    }
    if (OVRManager.wasPositionTracked)
    {
      if (!OVRManager.tracker.isPositionTracked)
      {
        try
        {
          if (OVRManager.TrackingLost != null)
            OVRManager.TrackingLost();
        }
        catch (Exception ex)
        {
          Debug.LogError((object) ("Caught Exception: " + (object) ex));
        }
      }
    }
    if (!OVRManager.wasPositionTracked)
    {
      if (OVRManager.tracker.isPositionTracked)
      {
        try
        {
          if (OVRManager.TrackingAcquired != null)
            OVRManager.TrackingAcquired();
        }
        catch (Exception ex)
        {
          Debug.LogError((object) ("Caught Exception: " + (object) ex));
        }
      }
    }
    OVRManager.wasPositionTracked = OVRManager.tracker.isPositionTracked;
    OVRManager.display.Update();
    OVRInput.Update();
    if (!this.enableMixedReality && !OVRManager.prevEnableMixedReality)
      return;
    Camera mainCamera = this.FindMainCamera();
    if ((UnityEngine.Object) Camera.main != (UnityEngine.Object) null)
    {
      this.suppressDisableMixedRealityBecauseOfNoMainCameraWarning = false;
      if (this.enableMixedReality)
        OVRMixedReality.Update(this.gameObject, mainCamera, this.compositionMethod, this.useDynamicLighting, this.capturingCameraDevice, this.depthQuality);
      if (OVRManager.prevEnableMixedReality && !this.enableMixedReality)
        OVRMixedReality.Cleanup();
      OVRManager.prevEnableMixedReality = this.enableMixedReality;
    }
    else
    {
      if (this.suppressDisableMixedRealityBecauseOfNoMainCameraWarning)
        return;
      Debug.LogWarning((object) "Main Camera is not set, Mixed Reality disabled");
      this.suppressDisableMixedRealityBecauseOfNoMainCameraWarning = true;
    }
  }

  private Camera FindMainCamera()
  {
    GameObject[] gameObjectsWithTag = GameObject.FindGameObjectsWithTag("MainCamera");
    List<Camera> cameraList = new List<Camera>(4);
    foreach (GameObject gameObject in gameObjectsWithTag)
    {
      Camera component = gameObject.GetComponent<Camera>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.enabled)
      {
        OVRCameraRig componentInParent = component.GetComponentInParent<OVRCameraRig>();
        if ((UnityEngine.Object) componentInParent != (UnityEngine.Object) null && (UnityEngine.Object) componentInParent.trackingSpace != (UnityEngine.Object) null)
          cameraList.Add(component);
      }
    }
    if (cameraList.Count == 0)
      return Camera.main;
    if (cameraList.Count == 1)
      return cameraList[0];
    if (!this.multipleMainCameraWarningPresented)
    {
      Debug.LogWarning((object) "Multiple MainCamera found. Assume the real MainCamera is the camera with the least depth");
      this.multipleMainCameraWarningPresented = true;
    }
    cameraList.Sort((Comparison<Camera>) ((c0, c1) =>
    {
      if ((double) c0.depth < (double) c1.depth)
        return -1;
      return (double) c0.depth <= (double) c1.depth ? 0 : 1;
    }));
    return cameraList[0];
  }

  private void OnDisable() => OVRMixedReality.Cleanup();

  private void LateUpdate() => OVRHaptics.Process();

  private void FixedUpdate() => OVRInput.FixedUpdate();

  public void ReturnToLauncher() => OVRManager.PlatformUIConfirmQuit();

  public static void PlatformUIConfirmQuit()
  {
    if (!OVRManager.isHmdPresent)
      return;
    OVRPlugin.ShowUI(OVRPlugin.PlatformUI.ConfirmQuit);
  }

  public enum TrackingOrigin
  {
    EyeLevel,
    FloorLevel,
  }

  public enum EyeTextureFormat
  {
    Default = 0,
    R16G16B16A16_FP = 2,
    R11G11B10_FP = 3,
  }

  public enum TiledMultiResLevel
  {
    Off,
    LMSLow,
    LMSMedium,
    LMSHigh,
  }

  public enum CompositionMethod
  {
    External,
    Direct,
    Sandwich,
  }

  public enum CameraDevice
  {
    WebCamera0,
    WebCamera1,
    ZEDCamera,
  }

  public enum DepthQuality
  {
    Low,
    Medium,
    High,
  }

  public enum VirtualGreenScreenType
  {
    Off,
    OuterBoundary,
    PlayArea,
  }
}
