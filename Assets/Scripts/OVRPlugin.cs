// Decompiled with JetBrains decompiler
// Type: OVRPlugin
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using System.Runtime.InteropServices;
using UnityEngine;

internal static class OVRPlugin
{
  public const bool isSupportedPlatform = true;
  public static readonly Version wrapperVersion = OVRPlugin.OVRP_1_26_0.version;
  private static Version _version;
  private static Version _nativeSDKVersion;
  private const int OverlayShapeFlagShift = 4;
  public const int AppPerfFrameStatsMaxCount = 5;
  private static OVRPlugin.GUID _nativeAudioOutGuid = new OVRPlugin.GUID();
  private static Guid _cachedAudioOutGuid;
  private static string _cachedAudioOutString;
  private static OVRPlugin.GUID _nativeAudioInGuid = new OVRPlugin.GUID();
  private static Guid _cachedAudioInGuid;
  private static string _cachedAudioInString;
  private static Texture2D cachedCameraFrameTexture = (Texture2D) null;
  private static Texture2D cachedCameraDepthTexture = (Texture2D) null;
  private static Texture2D cachedCameraDepthConfidenceTexture = (Texture2D) null;
  private static OVRNativeBuffer _nativeSystemDisplayFrequenciesAvailable = (OVRNativeBuffer) null;
  private static float[] _cachedSystemDisplayFrequenciesAvailable = (float[]) null;
  private const string pluginName = "OVRPlugin";
  private static Version _versionZero = new Version(0, 0, 0);

  public static Version version
  {
    get
    {
      if (OVRPlugin._version == (Version) null)
      {
        try
        {
          string version = OVRPlugin.OVRP_1_1_0.ovrp_GetVersion();
          if (version != null)
            OVRPlugin._version = new Version(version.Split('-')[0]);
          else
            OVRPlugin._version = OVRPlugin._versionZero;
        }
        catch
        {
          OVRPlugin._version = OVRPlugin._versionZero;
        }
        if (OVRPlugin._version == OVRPlugin.OVRP_0_5_0.version)
          OVRPlugin._version = OVRPlugin.OVRP_0_1_0.version;
        if (OVRPlugin._version > OVRPlugin._versionZero && OVRPlugin._version < OVRPlugin.OVRP_1_3_0.version)
          throw new PlatformNotSupportedException("Oculus Utilities version " + (object) OVRPlugin.wrapperVersion + " is too new for OVRPlugin version " + OVRPlugin._version.ToString() + ". Update to the latest version of Unity.");
      }
      return OVRPlugin._version;
    }
  }

  public static Version nativeSDKVersion
  {
    get
    {
      if (OVRPlugin._nativeSDKVersion == (Version) null)
      {
        try
        {
          string empty = string.Empty;
          string str = !(OVRPlugin.version >= OVRPlugin.OVRP_1_1_0.version) ? OVRPlugin._versionZero.ToString() : OVRPlugin.OVRP_1_1_0.ovrp_GetNativeSDKVersion();
          if (str != null)
            OVRPlugin._nativeSDKVersion = new Version(str.Split('-')[0]);
          else
            OVRPlugin._nativeSDKVersion = OVRPlugin._versionZero;
        }
        catch
        {
          OVRPlugin._nativeSDKVersion = OVRPlugin._versionZero;
        }
      }
      return OVRPlugin._nativeSDKVersion;
    }
  }

  public static bool initialized => OVRPlugin.OVRP_1_1_0.ovrp_GetInitialized() == OVRPlugin.Bool.True;

  public static bool chromatic
  {
    get => !(OVRPlugin.version >= OVRPlugin.OVRP_1_7_0.version) || OVRPlugin.OVRP_1_7_0.ovrp_GetAppChromaticCorrection() == OVRPlugin.Bool.True;
    set
    {
      if (!(OVRPlugin.version >= OVRPlugin.OVRP_1_7_0.version))
        return;
      int num = (int) OVRPlugin.OVRP_1_7_0.ovrp_SetAppChromaticCorrection(OVRPlugin.ToBool(value));
    }
  }

  public static bool monoscopic
  {
    get => OVRPlugin.OVRP_1_1_0.ovrp_GetAppMonoscopic() == OVRPlugin.Bool.True;
    set
    {
      int num = (int) OVRPlugin.OVRP_1_1_0.ovrp_SetAppMonoscopic(OVRPlugin.ToBool(value));
    }
  }

  public static bool rotation
  {
    get => OVRPlugin.OVRP_1_1_0.ovrp_GetTrackingOrientationEnabled() == OVRPlugin.Bool.True;
    set
    {
      int num = (int) OVRPlugin.OVRP_1_1_0.ovrp_SetTrackingOrientationEnabled(OVRPlugin.ToBool(value));
    }
  }

  public static bool position
  {
    get => OVRPlugin.OVRP_1_1_0.ovrp_GetTrackingPositionEnabled() == OVRPlugin.Bool.True;
    set
    {
      int num = (int) OVRPlugin.OVRP_1_1_0.ovrp_SetTrackingPositionEnabled(OVRPlugin.ToBool(value));
    }
  }

  public static bool useIPDInPositionTracking
  {
    get => !(OVRPlugin.version >= OVRPlugin.OVRP_1_6_0.version) || OVRPlugin.OVRP_1_6_0.ovrp_GetTrackingIPDEnabled() == OVRPlugin.Bool.True;
    set
    {
      if (!(OVRPlugin.version >= OVRPlugin.OVRP_1_6_0.version))
        return;
      int num = (int) OVRPlugin.OVRP_1_6_0.ovrp_SetTrackingIPDEnabled(OVRPlugin.ToBool(value));
    }
  }

  public static bool positionSupported => OVRPlugin.OVRP_1_1_0.ovrp_GetTrackingPositionSupported() == OVRPlugin.Bool.True;

  public static bool positionTracked => OVRPlugin.OVRP_1_1_0.ovrp_GetNodePositionTracked(OVRPlugin.Node.EyeCenter) == OVRPlugin.Bool.True;

  public static bool powerSaving => OVRPlugin.OVRP_1_1_0.ovrp_GetSystemPowerSavingMode() == OVRPlugin.Bool.True;

  public static bool hmdPresent => OVRPlugin.OVRP_1_1_0.ovrp_GetNodePresent(OVRPlugin.Node.EyeCenter) == OVRPlugin.Bool.True;

  public static bool userPresent => OVRPlugin.OVRP_1_1_0.ovrp_GetUserPresent() == OVRPlugin.Bool.True;

  public static bool headphonesPresent => OVRPlugin.OVRP_1_3_0.ovrp_GetSystemHeadphonesPresent() == OVRPlugin.Bool.True;

  public static int recommendedMSAALevel => OVRPlugin.version >= OVRPlugin.OVRP_1_6_0.version ? OVRPlugin.OVRP_1_6_0.ovrp_GetSystemRecommendedMSAALevel() : 2;

  public static OVRPlugin.SystemRegion systemRegion => OVRPlugin.version >= OVRPlugin.OVRP_1_5_0.version ? OVRPlugin.OVRP_1_5_0.ovrp_GetSystemRegion() : OVRPlugin.SystemRegion.Unspecified;

  public static string audioOutId
  {
    get
    {
      try
      {
        if (OVRPlugin._nativeAudioOutGuid == null)
          OVRPlugin._nativeAudioOutGuid = new OVRPlugin.GUID();
        IntPtr audioOutId = OVRPlugin.OVRP_1_1_0.ovrp_GetAudioOutId();
        if (audioOutId != IntPtr.Zero)
        {
          Marshal.PtrToStructure<OVRPlugin.GUID>(audioOutId, OVRPlugin._nativeAudioOutGuid);
          Guid guid = new Guid(OVRPlugin._nativeAudioOutGuid.a, OVRPlugin._nativeAudioOutGuid.b, OVRPlugin._nativeAudioOutGuid.c, OVRPlugin._nativeAudioOutGuid.d0, OVRPlugin._nativeAudioOutGuid.d1, OVRPlugin._nativeAudioOutGuid.d2, OVRPlugin._nativeAudioOutGuid.d3, OVRPlugin._nativeAudioOutGuid.d4, OVRPlugin._nativeAudioOutGuid.d5, OVRPlugin._nativeAudioOutGuid.d6, OVRPlugin._nativeAudioOutGuid.d7);
          if (guid != OVRPlugin._cachedAudioOutGuid)
          {
            OVRPlugin._cachedAudioOutGuid = guid;
            OVRPlugin._cachedAudioOutString = OVRPlugin._cachedAudioOutGuid.ToString();
          }
          return OVRPlugin._cachedAudioOutString;
        }
      }
      catch
      {
      }
      return string.Empty;
    }
  }

  public static string audioInId
  {
    get
    {
      try
      {
        if (OVRPlugin._nativeAudioInGuid == null)
          OVRPlugin._nativeAudioInGuid = new OVRPlugin.GUID();
        IntPtr audioInId = OVRPlugin.OVRP_1_1_0.ovrp_GetAudioInId();
        if (audioInId != IntPtr.Zero)
        {
          Marshal.PtrToStructure<OVRPlugin.GUID>(audioInId, OVRPlugin._nativeAudioInGuid);
          Guid guid = new Guid(OVRPlugin._nativeAudioInGuid.a, OVRPlugin._nativeAudioInGuid.b, OVRPlugin._nativeAudioInGuid.c, OVRPlugin._nativeAudioInGuid.d0, OVRPlugin._nativeAudioInGuid.d1, OVRPlugin._nativeAudioInGuid.d2, OVRPlugin._nativeAudioInGuid.d3, OVRPlugin._nativeAudioInGuid.d4, OVRPlugin._nativeAudioInGuid.d5, OVRPlugin._nativeAudioInGuid.d6, OVRPlugin._nativeAudioInGuid.d7);
          if (guid != OVRPlugin._cachedAudioInGuid)
          {
            OVRPlugin._cachedAudioInGuid = guid;
            OVRPlugin._cachedAudioInString = OVRPlugin._cachedAudioInGuid.ToString();
          }
          return OVRPlugin._cachedAudioInString;
        }
      }
      catch
      {
      }
      return string.Empty;
    }
  }

  public static bool hasVrFocus => OVRPlugin.OVRP_1_1_0.ovrp_GetAppHasVrFocus() == OVRPlugin.Bool.True;

  public static bool hasInputFocus
  {
    get
    {
      if (!(OVRPlugin.version >= OVRPlugin.OVRP_1_18_0.version))
        return true;
      OVRPlugin.Bool appHasInputFocus = OVRPlugin.Bool.False;
      return OVRPlugin.OVRP_1_18_0.ovrp_GetAppHasInputFocus(out appHasInputFocus) == OVRPlugin.Result.Success && appHasInputFocus == OVRPlugin.Bool.True;
    }
  }

  public static bool shouldQuit => OVRPlugin.OVRP_1_1_0.ovrp_GetAppShouldQuit() == OVRPlugin.Bool.True;

  public static bool shouldRecenter => OVRPlugin.OVRP_1_1_0.ovrp_GetAppShouldRecenter() == OVRPlugin.Bool.True;

  public static string productName => OVRPlugin.OVRP_1_1_0.ovrp_GetSystemProductName();

  public static string latency => OVRPlugin.OVRP_1_1_0.ovrp_GetAppLatencyTimings();

  public static float eyeDepth
  {
    get => OVRPlugin.OVRP_1_1_0.ovrp_GetUserEyeDepth();
    set
    {
      int num = (int) OVRPlugin.OVRP_1_1_0.ovrp_SetUserEyeDepth(value);
    }
  }

  public static float eyeHeight
  {
    get => OVRPlugin.OVRP_1_1_0.ovrp_GetUserEyeHeight();
    set
    {
      int num = (int) OVRPlugin.OVRP_1_1_0.ovrp_SetUserEyeHeight(value);
    }
  }

  public static float batteryLevel => OVRPlugin.OVRP_1_1_0.ovrp_GetSystemBatteryLevel();

  public static float batteryTemperature => OVRPlugin.OVRP_1_1_0.ovrp_GetSystemBatteryTemperature();

  public static int cpuLevel
  {
    get => OVRPlugin.OVRP_1_1_0.ovrp_GetSystemCpuLevel();
    set
    {
      int num = (int) OVRPlugin.OVRP_1_1_0.ovrp_SetSystemCpuLevel(value);
    }
  }

  public static int gpuLevel
  {
    get => OVRPlugin.OVRP_1_1_0.ovrp_GetSystemGpuLevel();
    set
    {
      int num = (int) OVRPlugin.OVRP_1_1_0.ovrp_SetSystemGpuLevel(value);
    }
  }

  public static int vsyncCount
  {
    get => OVRPlugin.OVRP_1_1_0.ovrp_GetSystemVSyncCount();
    set
    {
      int num = (int) OVRPlugin.OVRP_1_2_0.ovrp_SetSystemVSyncCount(value);
    }
  }

  public static float systemVolume => OVRPlugin.OVRP_1_1_0.ovrp_GetSystemVolume();

  public static float ipd
  {
    get => OVRPlugin.OVRP_1_1_0.ovrp_GetUserIPD();
    set
    {
      int num = (int) OVRPlugin.OVRP_1_1_0.ovrp_SetUserIPD(value);
    }
  }

  public static bool occlusionMesh
  {
    get => OVRPlugin.OVRP_1_3_0.ovrp_GetEyeOcclusionMeshEnabled() == OVRPlugin.Bool.True;
    set
    {
      int num = (int) OVRPlugin.OVRP_1_3_0.ovrp_SetEyeOcclusionMeshEnabled(OVRPlugin.ToBool(value));
    }
  }

  public static OVRPlugin.BatteryStatus batteryStatus => OVRPlugin.OVRP_1_1_0.ovrp_GetSystemBatteryStatus();

  public static OVRPlugin.Frustumf GetEyeFrustum(OVRPlugin.Eye eyeId) => OVRPlugin.OVRP_1_1_0.ovrp_GetNodeFrustum((OVRPlugin.Node) eyeId);

  public static OVRPlugin.Sizei GetEyeTextureSize(OVRPlugin.Eye eyeId) => OVRPlugin.OVRP_0_1_0.ovrp_GetEyeTextureSize(eyeId);

  public static OVRPlugin.Posef GetTrackerPose(OVRPlugin.Tracker trackerId) => OVRPlugin.GetNodePose((OVRPlugin.Node) (trackerId + 5), OVRPlugin.Step.Render);

  public static OVRPlugin.Frustumf GetTrackerFrustum(OVRPlugin.Tracker trackerId) => OVRPlugin.OVRP_1_1_0.ovrp_GetNodeFrustum((OVRPlugin.Node) (trackerId + 5));

  public static bool ShowUI(OVRPlugin.PlatformUI ui) => OVRPlugin.OVRP_1_1_0.ovrp_ShowSystemUI(ui) == OVRPlugin.Bool.True;

  public static bool EnqueueSubmitLayer(
    bool onTop,
    bool headLocked,
    IntPtr leftTexture,
    IntPtr rightTexture,
    int layerId,
    int frameIndex,
    OVRPlugin.Posef pose,
    OVRPlugin.Vector3f scale,
    int layerIndex = 0,
    OVRPlugin.OverlayShape shape = OVRPlugin.OverlayShape.Quad)
  {
    if (OVRPlugin.version >= OVRPlugin.OVRP_1_6_0.version)
    {
      uint flags = 0;
      if (onTop)
        flags |= 1U;
      if (headLocked)
        flags |= 2U;
      if (shape == OVRPlugin.OverlayShape.Cylinder || shape == OVRPlugin.OverlayShape.Cubemap)
      {
        if (shape == OVRPlugin.OverlayShape.Cubemap && OVRPlugin.version >= OVRPlugin.OVRP_1_10_0.version)
        {
          flags |= (uint) shape << 4;
        }
        else
        {
          if (shape != OVRPlugin.OverlayShape.Cylinder || !(OVRPlugin.version >= OVRPlugin.OVRP_1_16_0.version))
            return false;
          flags |= (uint) shape << 4;
        }
      }
      if (shape == OVRPlugin.OverlayShape.OffcenterCubemap || shape == OVRPlugin.OverlayShape.Equirect)
        return false;
      return OVRPlugin.version >= OVRPlugin.OVRP_1_15_0.version && layerId != -1 ? OVRPlugin.OVRP_1_15_0.ovrp_EnqueueSubmitLayer(flags, leftTexture, rightTexture, layerId, frameIndex, ref pose, ref scale, layerIndex) == OVRPlugin.Result.Success : OVRPlugin.OVRP_1_6_0.ovrp_SetOverlayQuad3(flags, leftTexture, rightTexture, IntPtr.Zero, pose, scale, layerIndex) == OVRPlugin.Bool.True;
    }
    return layerIndex == 0 && OVRPlugin.OVRP_0_1_1.ovrp_SetOverlayQuad2(OVRPlugin.ToBool(onTop), OVRPlugin.ToBool(headLocked), leftTexture, IntPtr.Zero, pose, scale) == OVRPlugin.Bool.True;
  }

  public static OVRPlugin.LayerDesc CalculateLayerDesc(
    OVRPlugin.OverlayShape shape,
    OVRPlugin.LayerLayout layout,
    OVRPlugin.Sizei textureSize,
    int mipLevels,
    int sampleCount,
    OVRPlugin.EyeTextureFormat format,
    int layerFlags)
  {
    OVRPlugin.LayerDesc layerDesc1 = new OVRPlugin.LayerDesc();
    if (OVRPlugin.version >= OVRPlugin.OVRP_1_15_0.version)
    {
      int layerDesc2 = (int) OVRPlugin.OVRP_1_15_0.ovrp_CalculateLayerDesc(shape, layout, ref textureSize, mipLevels, sampleCount, format, layerFlags, ref layerDesc1);
    }
    return layerDesc1;
  }

  public static bool EnqueueSetupLayer(OVRPlugin.LayerDesc desc, IntPtr layerID) => OVRPlugin.version >= OVRPlugin.OVRP_1_15_0.version && OVRPlugin.OVRP_1_15_0.ovrp_EnqueueSetupLayer(ref desc, layerID) == OVRPlugin.Result.Success;

  public static bool EnqueueDestroyLayer(IntPtr layerID) => OVRPlugin.version >= OVRPlugin.OVRP_1_15_0.version && OVRPlugin.OVRP_1_15_0.ovrp_EnqueueDestroyLayer(layerID) == OVRPlugin.Result.Success;

  public static IntPtr GetLayerTexture(int layerId, int stage, OVRPlugin.Eye eyeId)
  {
    IntPtr zero = IntPtr.Zero;
    if (OVRPlugin.version >= OVRPlugin.OVRP_1_15_0.version)
    {
      int layerTexturePtr = (int) OVRPlugin.OVRP_1_15_0.ovrp_GetLayerTexturePtr(layerId, stage, eyeId, ref zero);
    }
    return zero;
  }

  public static int GetLayerTextureStageCount(int layerId)
  {
    int layerTextureStageCount = 1;
    if (OVRPlugin.version >= OVRPlugin.OVRP_1_15_0.version)
    {
      int textureStageCount = (int) OVRPlugin.OVRP_1_15_0.ovrp_GetLayerTextureStageCount(layerId, ref layerTextureStageCount);
    }
    return layerTextureStageCount;
  }

  public static bool UpdateNodePhysicsPoses(int frameIndex, double predictionSeconds) => OVRPlugin.version >= OVRPlugin.OVRP_1_8_0.version && OVRPlugin.OVRP_1_8_0.ovrp_Update2(0, frameIndex, predictionSeconds) == OVRPlugin.Bool.True;

  public static OVRPlugin.Posef GetNodePose(OVRPlugin.Node nodeId, OVRPlugin.Step stepId)
  {
    if (OVRPlugin.version >= OVRPlugin.OVRP_1_12_0.version)
      return OVRPlugin.OVRP_1_12_0.ovrp_GetNodePoseState(stepId, nodeId).Pose;
    return OVRPlugin.version >= OVRPlugin.OVRP_1_8_0.version && stepId == OVRPlugin.Step.Physics ? OVRPlugin.OVRP_1_8_0.ovrp_GetNodePose2(0, nodeId) : OVRPlugin.OVRP_0_1_2.ovrp_GetNodePose(nodeId);
  }

  public static OVRPlugin.Vector3f GetNodeVelocity(
    OVRPlugin.Node nodeId,
    OVRPlugin.Step stepId)
  {
    if (OVRPlugin.version >= OVRPlugin.OVRP_1_12_0.version)
      return OVRPlugin.OVRP_1_12_0.ovrp_GetNodePoseState(stepId, nodeId).Velocity;
    return OVRPlugin.version >= OVRPlugin.OVRP_1_8_0.version && stepId == OVRPlugin.Step.Physics ? OVRPlugin.OVRP_1_8_0.ovrp_GetNodeVelocity2(0, nodeId).Position : OVRPlugin.OVRP_0_1_3.ovrp_GetNodeVelocity(nodeId).Position;
  }

  public static OVRPlugin.Vector3f GetNodeAngularVelocity(
    OVRPlugin.Node nodeId,
    OVRPlugin.Step stepId)
  {
    return OVRPlugin.version >= OVRPlugin.OVRP_1_12_0.version ? OVRPlugin.OVRP_1_12_0.ovrp_GetNodePoseState(stepId, nodeId).AngularVelocity : new OVRPlugin.Vector3f();
  }

  public static OVRPlugin.Vector3f GetNodeAcceleration(
    OVRPlugin.Node nodeId,
    OVRPlugin.Step stepId)
  {
    if (OVRPlugin.version >= OVRPlugin.OVRP_1_12_0.version)
      return OVRPlugin.OVRP_1_12_0.ovrp_GetNodePoseState(stepId, nodeId).Acceleration;
    return OVRPlugin.version >= OVRPlugin.OVRP_1_8_0.version && stepId == OVRPlugin.Step.Physics ? OVRPlugin.OVRP_1_8_0.ovrp_GetNodeAcceleration2(0, nodeId).Position : OVRPlugin.OVRP_0_1_3.ovrp_GetNodeAcceleration(nodeId).Position;
  }

  public static OVRPlugin.Vector3f GetNodeAngularAcceleration(
    OVRPlugin.Node nodeId,
    OVRPlugin.Step stepId)
  {
    return OVRPlugin.version >= OVRPlugin.OVRP_1_12_0.version ? OVRPlugin.OVRP_1_12_0.ovrp_GetNodePoseState(stepId, nodeId).AngularAcceleration : new OVRPlugin.Vector3f();
  }

  public static bool GetNodePresent(OVRPlugin.Node nodeId) => OVRPlugin.OVRP_1_1_0.ovrp_GetNodePresent(nodeId) == OVRPlugin.Bool.True;

  public static bool GetNodeOrientationTracked(OVRPlugin.Node nodeId) => OVRPlugin.OVRP_1_1_0.ovrp_GetNodeOrientationTracked(nodeId) == OVRPlugin.Bool.True;

  public static bool GetNodePositionTracked(OVRPlugin.Node nodeId) => OVRPlugin.OVRP_1_1_0.ovrp_GetNodePositionTracked(nodeId) == OVRPlugin.Bool.True;

  public static OVRPlugin.ControllerState GetControllerState(uint controllerMask) => OVRPlugin.OVRP_1_1_0.ovrp_GetControllerState(controllerMask);

  public static OVRPlugin.ControllerState2 GetControllerState2(uint controllerMask) => OVRPlugin.version >= OVRPlugin.OVRP_1_12_0.version ? OVRPlugin.OVRP_1_12_0.ovrp_GetControllerState2(controllerMask) : new OVRPlugin.ControllerState2(OVRPlugin.OVRP_1_1_0.ovrp_GetControllerState(controllerMask));

  public static OVRPlugin.ControllerState4 GetControllerState4(uint controllerMask)
  {
    if (!(OVRPlugin.version >= OVRPlugin.OVRP_1_16_0.version))
      return new OVRPlugin.ControllerState4(OVRPlugin.GetControllerState2(controllerMask));
    OVRPlugin.ControllerState4 controllerState = new OVRPlugin.ControllerState4();
    int controllerState4 = (int) OVRPlugin.OVRP_1_16_0.ovrp_GetControllerState4(controllerMask, ref controllerState);
    return controllerState;
  }

  public static bool SetControllerVibration(uint controllerMask, float frequency, float amplitude) => OVRPlugin.OVRP_0_1_2.ovrp_SetControllerVibration(controllerMask, frequency, amplitude) == OVRPlugin.Bool.True;

  public static OVRPlugin.HapticsDesc GetControllerHapticsDesc(uint controllerMask) => OVRPlugin.version >= OVRPlugin.OVRP_1_6_0.version ? OVRPlugin.OVRP_1_6_0.ovrp_GetControllerHapticsDesc(controllerMask) : new OVRPlugin.HapticsDesc();

  public static OVRPlugin.HapticsState GetControllerHapticsState(uint controllerMask) => OVRPlugin.version >= OVRPlugin.OVRP_1_6_0.version ? OVRPlugin.OVRP_1_6_0.ovrp_GetControllerHapticsState(controllerMask) : new OVRPlugin.HapticsState();

  public static bool SetControllerHaptics(
    uint controllerMask,
    OVRPlugin.HapticsBuffer hapticsBuffer)
  {
    return OVRPlugin.version >= OVRPlugin.OVRP_1_6_0.version && OVRPlugin.OVRP_1_6_0.ovrp_SetControllerHaptics(controllerMask, hapticsBuffer) == OVRPlugin.Bool.True;
  }

  public static float GetEyeRecommendedResolutionScale() => OVRPlugin.version >= OVRPlugin.OVRP_1_6_0.version ? OVRPlugin.OVRP_1_6_0.ovrp_GetEyeRecommendedResolutionScale() : 1f;

  public static float GetAppCpuStartToGpuEndTime() => OVRPlugin.version >= OVRPlugin.OVRP_1_6_0.version ? OVRPlugin.OVRP_1_6_0.ovrp_GetAppCpuStartToGpuEndTime() : 0.0f;

  public static bool GetBoundaryConfigured() => OVRPlugin.version >= OVRPlugin.OVRP_1_8_0.version && OVRPlugin.OVRP_1_8_0.ovrp_GetBoundaryConfigured() == OVRPlugin.Bool.True;

  public static OVRPlugin.BoundaryTestResult TestBoundaryNode(
    OVRPlugin.Node nodeId,
    OVRPlugin.BoundaryType boundaryType)
  {
    return OVRPlugin.version >= OVRPlugin.OVRP_1_8_0.version ? OVRPlugin.OVRP_1_8_0.ovrp_TestBoundaryNode(nodeId, boundaryType) : new OVRPlugin.BoundaryTestResult();
  }

  public static OVRPlugin.BoundaryTestResult TestBoundaryPoint(
    OVRPlugin.Vector3f point,
    OVRPlugin.BoundaryType boundaryType)
  {
    return OVRPlugin.version >= OVRPlugin.OVRP_1_8_0.version ? OVRPlugin.OVRP_1_8_0.ovrp_TestBoundaryPoint(point, boundaryType) : new OVRPlugin.BoundaryTestResult();
  }

  public static bool SetBoundaryLookAndFeel(OVRPlugin.BoundaryLookAndFeel lookAndFeel) => OVRPlugin.version >= OVRPlugin.OVRP_1_8_0.version && OVRPlugin.OVRP_1_8_0.ovrp_SetBoundaryLookAndFeel(lookAndFeel) == OVRPlugin.Bool.True;

  public static bool ResetBoundaryLookAndFeel() => OVRPlugin.version >= OVRPlugin.OVRP_1_8_0.version && OVRPlugin.OVRP_1_8_0.ovrp_ResetBoundaryLookAndFeel() == OVRPlugin.Bool.True;

  public static OVRPlugin.BoundaryGeometry GetBoundaryGeometry(
    OVRPlugin.BoundaryType boundaryType)
  {
    return OVRPlugin.version >= OVRPlugin.OVRP_1_8_0.version ? OVRPlugin.OVRP_1_8_0.ovrp_GetBoundaryGeometry(boundaryType) : new OVRPlugin.BoundaryGeometry();
  }

  public static bool GetBoundaryGeometry2(
    OVRPlugin.BoundaryType boundaryType,
    IntPtr points,
    ref int pointsCount)
  {
    if (OVRPlugin.version >= OVRPlugin.OVRP_1_9_0.version)
      return OVRPlugin.OVRP_1_9_0.ovrp_GetBoundaryGeometry2(boundaryType, points, ref pointsCount) == OVRPlugin.Bool.True;
    pointsCount = 0;
    return false;
  }

  public static OVRPlugin.AppPerfStats GetAppPerfStats() => OVRPlugin.version >= OVRPlugin.OVRP_1_9_0.version ? OVRPlugin.OVRP_1_9_0.ovrp_GetAppPerfStats() : new OVRPlugin.AppPerfStats();

  public static bool ResetAppPerfStats() => OVRPlugin.version >= OVRPlugin.OVRP_1_9_0.version && OVRPlugin.OVRP_1_9_0.ovrp_ResetAppPerfStats() == OVRPlugin.Bool.True;

  public static float GetAppFramerate() => OVRPlugin.version >= OVRPlugin.OVRP_1_12_0.version ? OVRPlugin.OVRP_1_12_0.ovrp_GetAppFramerate() : 0.0f;

  public static bool SetHandNodePoseStateLatency(double latencyInSeconds) => OVRPlugin.version >= OVRPlugin.OVRP_1_18_0.version && OVRPlugin.OVRP_1_18_0.ovrp_SetHandNodePoseStateLatency(latencyInSeconds) == OVRPlugin.Result.Success;

  public static double GetHandNodePoseStateLatency()
  {
    if (!(OVRPlugin.version >= OVRPlugin.OVRP_1_18_0.version))
      return 0.0;
    double latencyInSeconds = 0.0;
    return OVRPlugin.OVRP_1_18_0.ovrp_GetHandNodePoseStateLatency(out latencyInSeconds) == OVRPlugin.Result.Success ? latencyInSeconds : 0.0;
  }

  public static OVRPlugin.EyeTextureFormat GetDesiredEyeTextureFormat()
  {
    if (!(OVRPlugin.version >= OVRPlugin.OVRP_1_11_0.version))
      return OVRPlugin.EyeTextureFormat.Default;
    uint eyeTextureFormat = (uint) OVRPlugin.OVRP_1_11_0.ovrp_GetDesiredEyeTextureFormat();
    if (eyeTextureFormat == 1U)
      eyeTextureFormat = 0U;
    return (OVRPlugin.EyeTextureFormat) eyeTextureFormat;
  }

  public static bool SetDesiredEyeTextureFormat(OVRPlugin.EyeTextureFormat value) => OVRPlugin.version >= OVRPlugin.OVRP_1_11_0.version && OVRPlugin.OVRP_1_11_0.ovrp_SetDesiredEyeTextureFormat(value) == OVRPlugin.Bool.True;

  public static bool InitializeMixedReality() => OVRPlugin.version >= OVRPlugin.OVRP_1_15_0.version && OVRPlugin.OVRP_1_15_0.ovrp_InitializeMixedReality() == OVRPlugin.Result.Success;

  public static bool ShutdownMixedReality() => OVRPlugin.version >= OVRPlugin.OVRP_1_15_0.version && OVRPlugin.OVRP_1_15_0.ovrp_ShutdownMixedReality() == OVRPlugin.Result.Success;

  public static bool IsMixedRealityInitialized() => OVRPlugin.version >= OVRPlugin.OVRP_1_15_0.version && OVRPlugin.OVRP_1_15_0.ovrp_GetMixedRealityInitialized() == OVRPlugin.Bool.True;

  public static int GetExternalCameraCount()
  {
    if (!(OVRPlugin.version >= OVRPlugin.OVRP_1_15_0.version))
      return 0;
    int cameraCount = 0;
    return OVRPlugin.OVRP_1_15_0.ovrp_GetExternalCameraCount(out cameraCount) != OVRPlugin.Result.Success ? 0 : cameraCount;
  }

  public static bool UpdateExternalCamera() => OVRPlugin.version >= OVRPlugin.OVRP_1_15_0.version && OVRPlugin.OVRP_1_15_0.ovrp_UpdateExternalCamera() == OVRPlugin.Result.Success;

  public static bool GetMixedRealityCameraInfo(
    int cameraId,
    out OVRPlugin.CameraExtrinsics cameraExtrinsics,
    out OVRPlugin.CameraIntrinsics cameraIntrinsics)
  {
    cameraExtrinsics = new OVRPlugin.CameraExtrinsics();
    cameraIntrinsics = new OVRPlugin.CameraIntrinsics();
    if (!(OVRPlugin.version >= OVRPlugin.OVRP_1_15_0.version))
      return false;
    bool realityCameraInfo = true;
    if (OVRPlugin.OVRP_1_15_0.ovrp_GetExternalCameraExtrinsics(cameraId, out cameraExtrinsics) != OVRPlugin.Result.Success)
      realityCameraInfo = false;
    if (OVRPlugin.OVRP_1_15_0.ovrp_GetExternalCameraIntrinsics(cameraId, out cameraIntrinsics) != OVRPlugin.Result.Success)
      realityCameraInfo = false;
    return realityCameraInfo;
  }

  public static OVRPlugin.Vector3f GetBoundaryDimensions(
    OVRPlugin.BoundaryType boundaryType)
  {
    return OVRPlugin.version >= OVRPlugin.OVRP_1_8_0.version ? OVRPlugin.OVRP_1_8_0.ovrp_GetBoundaryDimensions(boundaryType) : new OVRPlugin.Vector3f();
  }

  public static bool GetBoundaryVisible() => OVRPlugin.version >= OVRPlugin.OVRP_1_8_0.version && OVRPlugin.OVRP_1_8_0.ovrp_GetBoundaryVisible() == OVRPlugin.Bool.True;

  public static bool SetBoundaryVisible(bool value) => OVRPlugin.version >= OVRPlugin.OVRP_1_8_0.version && OVRPlugin.OVRP_1_8_0.ovrp_SetBoundaryVisible(OVRPlugin.ToBool(value)) == OVRPlugin.Bool.True;

  public static OVRPlugin.SystemHeadset GetSystemHeadsetType() => OVRPlugin.version >= OVRPlugin.OVRP_1_9_0.version ? OVRPlugin.OVRP_1_9_0.ovrp_GetSystemHeadsetType() : OVRPlugin.SystemHeadset.None;

  public static OVRPlugin.Controller GetActiveController() => OVRPlugin.version >= OVRPlugin.OVRP_1_9_0.version ? OVRPlugin.OVRP_1_9_0.ovrp_GetActiveController() : OVRPlugin.Controller.None;

  public static OVRPlugin.Controller GetConnectedControllers() => OVRPlugin.version >= OVRPlugin.OVRP_1_9_0.version ? OVRPlugin.OVRP_1_9_0.ovrp_GetConnectedControllers() : OVRPlugin.Controller.None;

  private static OVRPlugin.Bool ToBool(bool b) => !b ? OVRPlugin.Bool.False : OVRPlugin.Bool.True;

  public static OVRPlugin.TrackingOrigin GetTrackingOriginType() => OVRPlugin.OVRP_1_0_0.ovrp_GetTrackingOriginType();

  public static bool SetTrackingOriginType(OVRPlugin.TrackingOrigin originType) => OVRPlugin.OVRP_1_0_0.ovrp_SetTrackingOriginType(originType) == OVRPlugin.Bool.True;

  public static OVRPlugin.Posef GetTrackingCalibratedOrigin() => OVRPlugin.OVRP_1_0_0.ovrp_GetTrackingCalibratedOrigin();

  public static bool SetTrackingCalibratedOrigin() => OVRPlugin.OVRP_1_2_0.ovrpi_SetTrackingCalibratedOrigin() == OVRPlugin.Bool.True;

  public static bool RecenterTrackingOrigin(OVRPlugin.RecenterFlags flags) => OVRPlugin.OVRP_1_0_0.ovrp_RecenterTrackingOrigin((uint) flags) == OVRPlugin.Bool.True;

  public static bool UpdateCameraDevices() => OVRPlugin.version >= OVRPlugin.OVRP_1_16_0.version && OVRPlugin.OVRP_1_16_0.ovrp_UpdateCameraDevices() == OVRPlugin.Result.Success;

  public static bool IsCameraDeviceAvailable(OVRPlugin.CameraDevice cameraDevice) => OVRPlugin.version >= OVRPlugin.OVRP_1_16_0.version && OVRPlugin.OVRP_1_16_0.ovrp_IsCameraDeviceAvailable(cameraDevice) == OVRPlugin.Bool.True;

  public static bool SetCameraDevicePreferredColorFrameSize(
    OVRPlugin.CameraDevice cameraDevice,
    int width,
    int height)
  {
    if (!(OVRPlugin.version >= OVRPlugin.OVRP_1_16_0.version))
      return false;
    return OVRPlugin.OVRP_1_16_0.ovrp_SetCameraDevicePreferredColorFrameSize(cameraDevice, new OVRPlugin.Sizei()
    {
      w = width,
      h = height
    }) == OVRPlugin.Result.Success;
  }

  public static bool OpenCameraDevice(OVRPlugin.CameraDevice cameraDevice) => OVRPlugin.version >= OVRPlugin.OVRP_1_16_0.version && OVRPlugin.OVRP_1_16_0.ovrp_OpenCameraDevice(cameraDevice) == OVRPlugin.Result.Success;

  public static bool CloseCameraDevice(OVRPlugin.CameraDevice cameraDevice) => OVRPlugin.version >= OVRPlugin.OVRP_1_16_0.version && OVRPlugin.OVRP_1_16_0.ovrp_CloseCameraDevice(cameraDevice) == OVRPlugin.Result.Success;

  public static bool HasCameraDeviceOpened(OVRPlugin.CameraDevice cameraDevice) => OVRPlugin.version >= OVRPlugin.OVRP_1_16_0.version && OVRPlugin.OVRP_1_16_0.ovrp_HasCameraDeviceOpened(cameraDevice) == OVRPlugin.Bool.True;

  public static bool IsCameraDeviceColorFrameAvailable(OVRPlugin.CameraDevice cameraDevice) => OVRPlugin.version >= OVRPlugin.OVRP_1_16_0.version && OVRPlugin.OVRP_1_16_0.ovrp_IsCameraDeviceColorFrameAvailable(cameraDevice) == OVRPlugin.Bool.True;

  public static Texture2D GetCameraDeviceColorFrameTexture(
    OVRPlugin.CameraDevice cameraDevice)
  {
    if (!(OVRPlugin.version >= OVRPlugin.OVRP_1_16_0.version))
      return (Texture2D) null;
    OVRPlugin.Sizei colorFrameSize = new OVRPlugin.Sizei();
    if (OVRPlugin.OVRP_1_16_0.ovrp_GetCameraDeviceColorFrameSize(cameraDevice, out colorFrameSize) != OVRPlugin.Result.Success)
      return (Texture2D) null;
    IntPtr colorFrameBgraPixels;
    int colorFrameRowPitch;
    if (OVRPlugin.OVRP_1_16_0.ovrp_GetCameraDeviceColorFrameBgraPixels(cameraDevice, out colorFrameBgraPixels, out colorFrameRowPitch) != OVRPlugin.Result.Success)
      return (Texture2D) null;
    if (colorFrameRowPitch != colorFrameSize.w * 4)
      return (Texture2D) null;
    if (!(bool) (UnityEngine.Object) OVRPlugin.cachedCameraFrameTexture || OVRPlugin.cachedCameraFrameTexture.width != colorFrameSize.w || OVRPlugin.cachedCameraFrameTexture.height != colorFrameSize.h)
      OVRPlugin.cachedCameraFrameTexture = new Texture2D(colorFrameSize.w, colorFrameSize.h, TextureFormat.BGRA32, false);
    OVRPlugin.cachedCameraFrameTexture.LoadRawTextureData(colorFrameBgraPixels, colorFrameRowPitch * colorFrameSize.h);
    OVRPlugin.cachedCameraFrameTexture.Apply();
    return OVRPlugin.cachedCameraFrameTexture;
  }

  public static bool DoesCameraDeviceSupportDepth(OVRPlugin.CameraDevice cameraDevice)
  {
    OVRPlugin.Bool supportDepth;
    return OVRPlugin.version >= OVRPlugin.OVRP_1_17_0.version && OVRPlugin.OVRP_1_17_0.ovrp_DoesCameraDeviceSupportDepth(cameraDevice, out supportDepth) == OVRPlugin.Result.Success && supportDepth == OVRPlugin.Bool.True;
  }

  public static bool SetCameraDeviceDepthSensingMode(
    OVRPlugin.CameraDevice camera,
    OVRPlugin.CameraDeviceDepthSensingMode depthSensoringMode)
  {
    return OVRPlugin.version >= OVRPlugin.OVRP_1_17_0.version && OVRPlugin.OVRP_1_17_0.ovrp_SetCameraDeviceDepthSensingMode(camera, depthSensoringMode) == OVRPlugin.Result.Success;
  }

  public static bool SetCameraDevicePreferredDepthQuality(
    OVRPlugin.CameraDevice camera,
    OVRPlugin.CameraDeviceDepthQuality depthQuality)
  {
    return OVRPlugin.version >= OVRPlugin.OVRP_1_17_0.version && OVRPlugin.OVRP_1_17_0.ovrp_SetCameraDevicePreferredDepthQuality(camera, depthQuality) == OVRPlugin.Result.Success;
  }

  public static bool IsCameraDeviceDepthFrameAvailable(OVRPlugin.CameraDevice cameraDevice)
  {
    OVRPlugin.Bool available;
    return OVRPlugin.version >= OVRPlugin.OVRP_1_17_0.version && OVRPlugin.OVRP_1_17_0.ovrp_IsCameraDeviceDepthFrameAvailable(cameraDevice, out available) == OVRPlugin.Result.Success && available == OVRPlugin.Bool.True;
  }

  public static Texture2D GetCameraDeviceDepthFrameTexture(
    OVRPlugin.CameraDevice cameraDevice)
  {
    if (!(OVRPlugin.version >= OVRPlugin.OVRP_1_17_0.version))
      return (Texture2D) null;
    OVRPlugin.Sizei depthFrameSize = new OVRPlugin.Sizei();
    if (OVRPlugin.OVRP_1_17_0.ovrp_GetCameraDeviceDepthFrameSize(cameraDevice, out depthFrameSize) != OVRPlugin.Result.Success)
      return (Texture2D) null;
    IntPtr depthFramePixels;
    int depthFrameRowPitch;
    if (OVRPlugin.OVRP_1_17_0.ovrp_GetCameraDeviceDepthFramePixels(cameraDevice, out depthFramePixels, out depthFrameRowPitch) != OVRPlugin.Result.Success)
      return (Texture2D) null;
    if (depthFrameRowPitch != depthFrameSize.w * 4)
      return (Texture2D) null;
    if (!(bool) (UnityEngine.Object) OVRPlugin.cachedCameraDepthTexture || OVRPlugin.cachedCameraDepthTexture.width != depthFrameSize.w || OVRPlugin.cachedCameraDepthTexture.height != depthFrameSize.h)
    {
      OVRPlugin.cachedCameraDepthTexture = new Texture2D(depthFrameSize.w, depthFrameSize.h, TextureFormat.RFloat, false);
      OVRPlugin.cachedCameraDepthTexture.filterMode = FilterMode.Point;
    }
    OVRPlugin.cachedCameraDepthTexture.LoadRawTextureData(depthFramePixels, depthFrameRowPitch * depthFrameSize.h);
    OVRPlugin.cachedCameraDepthTexture.Apply();
    return OVRPlugin.cachedCameraDepthTexture;
  }

  public static Texture2D GetCameraDeviceDepthConfidenceTexture(
    OVRPlugin.CameraDevice cameraDevice)
  {
    if (!(OVRPlugin.version >= OVRPlugin.OVRP_1_17_0.version))
      return (Texture2D) null;
    OVRPlugin.Sizei depthFrameSize = new OVRPlugin.Sizei();
    if (OVRPlugin.OVRP_1_17_0.ovrp_GetCameraDeviceDepthFrameSize(cameraDevice, out depthFrameSize) != OVRPlugin.Result.Success)
      return (Texture2D) null;
    IntPtr depthConfidencePixels;
    int depthConfidenceRowPitch;
    if (OVRPlugin.OVRP_1_17_0.ovrp_GetCameraDeviceDepthConfidencePixels(cameraDevice, out depthConfidencePixels, out depthConfidenceRowPitch) != OVRPlugin.Result.Success)
      return (Texture2D) null;
    if (depthConfidenceRowPitch != depthFrameSize.w * 4)
      return (Texture2D) null;
    if (!(bool) (UnityEngine.Object) OVRPlugin.cachedCameraDepthConfidenceTexture || OVRPlugin.cachedCameraDepthConfidenceTexture.width != depthFrameSize.w || OVRPlugin.cachedCameraDepthConfidenceTexture.height != depthFrameSize.h)
      OVRPlugin.cachedCameraDepthConfidenceTexture = new Texture2D(depthFrameSize.w, depthFrameSize.h, TextureFormat.RFloat, false);
    OVRPlugin.cachedCameraDepthConfidenceTexture.LoadRawTextureData(depthConfidencePixels, depthConfidenceRowPitch * depthFrameSize.h);
    OVRPlugin.cachedCameraDepthConfidenceTexture.Apply();
    return OVRPlugin.cachedCameraDepthConfidenceTexture;
  }

  public static bool tiledMultiResSupported
  {
    get
    {
      OVRPlugin.Bool foveationSupported;
      return OVRPlugin.version >= OVRPlugin.OVRP_1_21_0.version && OVRPlugin.OVRP_1_21_0.ovrp_GetTiledMultiResSupported(out foveationSupported) == OVRPlugin.Result.Success && foveationSupported == OVRPlugin.Bool.True;
    }
  }

  public static OVRPlugin.TiledMultiResLevel tiledMultiResLevel
  {
    get
    {
      if (!(OVRPlugin.version >= OVRPlugin.OVRP_1_21_0.version) || !OVRPlugin.tiledMultiResSupported)
        return OVRPlugin.TiledMultiResLevel.Off;
      OVRPlugin.TiledMultiResLevel level;
      int tiledMultiResLevel = (int) OVRPlugin.OVRP_1_21_0.ovrp_GetTiledMultiResLevel(out level);
      return level;
    }
    set
    {
      if (!(OVRPlugin.version >= OVRPlugin.OVRP_1_21_0.version) || !OVRPlugin.tiledMultiResSupported)
        return;
      int num = (int) OVRPlugin.OVRP_1_21_0.ovrp_SetTiledMultiResLevel(value);
    }
  }

  public static bool gpuUtilSupported
  {
    get
    {
      OVRPlugin.Bool gpuUtilSupported;
      return OVRPlugin.version >= OVRPlugin.OVRP_1_21_0.version && OVRPlugin.OVRP_1_21_0.ovrp_GetGPUUtilSupported(out gpuUtilSupported) == OVRPlugin.Result.Success && gpuUtilSupported == OVRPlugin.Bool.True;
    }
  }

  public static float gpuUtilLevel
  {
    get
    {
      float gpuUtil;
      return OVRPlugin.version >= OVRPlugin.OVRP_1_21_0.version && OVRPlugin.gpuUtilSupported && OVRPlugin.OVRP_1_21_0.ovrp_GetGPUUtilLevel(out gpuUtil) == OVRPlugin.Result.Success ? gpuUtil : 0.0f;
    }
  }

  public static float[] systemDisplayFrequenciesAvailable
  {
    get
    {
      if (OVRPlugin._cachedSystemDisplayFrequenciesAvailable == null)
      {
        OVRPlugin._cachedSystemDisplayFrequenciesAvailable = new float[0];
        if (OVRPlugin.version >= OVRPlugin.OVRP_1_21_0.version)
        {
          int numFrequencies = 0;
          if (OVRPlugin.OVRP_1_21_0.ovrp_GetSystemDisplayAvailableFrequencies(IntPtr.Zero, out numFrequencies) == OVRPlugin.Result.Success && numFrequencies > 0)
          {
            int num = numFrequencies;
            OVRPlugin._nativeSystemDisplayFrequenciesAvailable = new OVRNativeBuffer(4 * num);
            if (OVRPlugin.OVRP_1_21_0.ovrp_GetSystemDisplayAvailableFrequencies(OVRPlugin._nativeSystemDisplayFrequenciesAvailable.GetPointer(), out numFrequencies) == OVRPlugin.Result.Success)
            {
              int length = numFrequencies <= num ? numFrequencies : num;
              if (length > 0)
              {
                OVRPlugin._cachedSystemDisplayFrequenciesAvailable = new float[length];
                Marshal.Copy(OVRPlugin._nativeSystemDisplayFrequenciesAvailable.GetPointer(), OVRPlugin._cachedSystemDisplayFrequenciesAvailable, 0, length);
              }
            }
          }
        }
      }
      return OVRPlugin._cachedSystemDisplayFrequenciesAvailable;
    }
  }

  public static float systemDisplayFrequency
  {
    get
    {
      float systemDisplayFrequency;
      return OVRPlugin.version >= OVRPlugin.OVRP_1_21_0.version ? (OVRPlugin.OVRP_1_21_0.ovrp_GetSystemDisplayFrequency2(out systemDisplayFrequency) == OVRPlugin.Result.Success ? systemDisplayFrequency : 0.0f) : (OVRPlugin.version >= OVRPlugin.OVRP_1_1_0.version ? OVRPlugin.OVRP_1_1_0.ovrp_GetSystemDisplayFrequency() : 0.0f);
    }
    set
    {
      if (!(OVRPlugin.version >= OVRPlugin.OVRP_1_21_0.version))
        return;
      int num = (int) OVRPlugin.OVRP_1_21_0.ovrp_SetSystemDisplayFrequency(value);
    }
  }

  [StructLayout(LayoutKind.Sequential)]
  private class GUID
  {
    public int a;
    public short b;
    public short c;
    public byte d0;
    public byte d1;
    public byte d2;
    public byte d3;
    public byte d4;
    public byte d5;
    public byte d6;
    public byte d7;
  }

  public enum Bool
  {
    False,
    True,
  }

  public enum Result
  {
    Failure_InsufficientSize = -1007, // 0xFFFFFC11
    Failure_OperationFailed = -1006, // 0xFFFFFC12
    Failure_NotYetImplemented = -1005, // 0xFFFFFC13
    Failure_Unsupported = -1004, // 0xFFFFFC14
    Failure_InvalidOperation = -1003, // 0xFFFFFC15
    Failure_NotInitialized = -1002, // 0xFFFFFC16
    Failure_InvalidParameter = -1001, // 0xFFFFFC17
    Failure = -1000, // 0xFFFFFC18
    Success = 0,
  }

  public enum CameraStatus
  {
    CameraStatus_None = 0,
    CameraStatus_Connected = 1,
    CameraStatus_Calibrating = 2,
    CameraStatus_CalibrationFailed = 3,
    CameraStatus_Calibrated = 4,
    CameraStatus_EnumSize = 2147483647, // 0x7FFFFFFF
  }

  public enum Eye
  {
    None = -1, // 0xFFFFFFFF
    Left = 0,
    Right = 1,
    Count = 2,
  }

  public enum Tracker
  {
    None = -1, // 0xFFFFFFFF
    Zero = 0,
    One = 1,
    Two = 2,
    Three = 3,
    Count = 4,
  }

  public enum Node
  {
    None = -1, // 0xFFFFFFFF
    EyeLeft = 0,
    EyeRight = 1,
    EyeCenter = 2,
    HandLeft = 3,
    HandRight = 4,
    TrackerZero = 5,
    TrackerOne = 6,
    TrackerTwo = 7,
    TrackerThree = 8,
    Head = 9,
    DeviceObjectZero = 10, // 0x0000000A
    Count = 11, // 0x0000000B
  }

  public enum Controller
  {
    Active = -2147483648, // 0x80000000
    All = -1, // 0xFFFFFFFF
    None = 0,
    LTouch = 1,
    RTouch = 2,
    Touch = 3,
    Remote = 4,
    Gamepad = 16, // 0x00000010
    LTrackedRemote = 16777216, // 0x01000000
    RTrackedRemote = 33554432, // 0x02000000
    Touchpad = 134217728, // 0x08000000
  }

  public enum TrackingOrigin
  {
    EyeLevel,
    FloorLevel,
    Count,
  }

  public enum RecenterFlags
  {
    IgnoreAll = -2147483648, // 0x80000000
    Count = -2147483647, // 0x80000001
    Default = 0,
    Controllers = 1073741824, // 0x40000000
  }

  public enum BatteryStatus
  {
    Charging,
    Discharging,
    Full,
    NotCharging,
    Unknown,
  }

  public enum EyeTextureFormat
  {
    Default = 0,
    R8G8B8A8_sRGB = 0,
    R8G8B8A8 = 1,
    R16G16B16A16_FP = 2,
    R11G11B10_FP = 3,
    B8G8R8A8_sRGB = 4,
    B8G8R8A8 = 5,
    R5G6B5 = 11, // 0x0000000B
    EnumSize = 2147483647, // 0x7FFFFFFF
  }

  public enum PlatformUI
  {
    None = -1, // 0xFFFFFFFF
    ConfirmQuit = 1,
    GlobalMenuTutorial = 2,
  }

  public enum SystemRegion
  {
    Unspecified,
    Japan,
    China,
  }

  public enum SystemHeadset
  {
    None = 0,
    GearVR_R320 = 1,
    GearVR_R321 = 2,
    GearVR_R322 = 3,
    GearVR_R323 = 4,
    GearVR_R324 = 5,
    GearVR_R325 = 6,
    Oculus_Go = 7,
    Rift_DK1 = 4096, // 0x00001000
    Rift_DK2 = 4097, // 0x00001001
    Rift_CV1 = 4098, // 0x00001002
  }

  public enum OverlayShape
  {
    Quad = 0,
    Cylinder = 1,
    Cubemap = 2,
    OffcenterCubemap = 4,
    Equirect = 5,
  }

  public enum Step
  {
    Render = -1, // 0xFFFFFFFF
    Physics = 0,
  }

  public enum CameraDevice
  {
    None = 0,
    WebCamera0 = 100, // 0x00000064
    WebCamera1 = 101, // 0x00000065
    ZEDCamera = 300, // 0x0000012C
  }

  public enum CameraDeviceDepthSensingMode
  {
    Standard,
    Fill,
  }

  public enum CameraDeviceDepthQuality
  {
    Low,
    Medium,
    High,
  }

  public enum TiledMultiResLevel
  {
    Off = 0,
    LMSLow = 1,
    LMSMedium = 2,
    LMSHigh = 3,
    EnumSize = 2147483647, // 0x7FFFFFFF
  }

  public struct CameraDeviceIntrinsicsParameters
  {
    private float fx;
    private float fy;
    private float cx;
    private float cy;
    private double disto0;
    private double disto1;
    private double disto2;
    private double disto3;
    private double disto4;
    private float v_fov;
    private float h_fov;
    private float d_fov;
    private int w;
    private int h;
  }

  private enum OverlayFlag
  {
    None = 0,
    ShapeFlag_Quad = 0,
    OnTop = 1,
    HeadLocked = 2,
    ShapeFlag_Cylinder = 16, // 0x00000010
    ShapeFlag_Cubemap = 32, // 0x00000020
    ShapeFlag_OffcenterCubemap = 64, // 0x00000040
    ShapeFlagRangeMask = 240, // 0x000000F0
  }

  public struct Vector2f
  {
    public float x;
    public float y;
  }

  public struct Vector3f
  {
    public float x;
    public float y;
    public float z;
    public static readonly OVRPlugin.Vector3f zero = new OVRPlugin.Vector3f()
    {
      x = 0.0f,
      y = 0.0f,
      z = 0.0f
    };

    public override string ToString() => string.Format("{0}, {1}, {2}", (object) this.x, (object) this.y, (object) this.z);
  }

  public struct Quatf
  {
    public float x;
    public float y;
    public float z;
    public float w;
    public static readonly OVRPlugin.Quatf identity = new OVRPlugin.Quatf()
    {
      x = 0.0f,
      y = 0.0f,
      z = 0.0f,
      w = 1f
    };

    public override string ToString() => string.Format("{0}, {1}, {2}, {3}", (object) this.x, (object) this.y, (object) this.z, (object) this.w);
  }

  public struct Posef
  {
    public OVRPlugin.Quatf Orientation;
    public OVRPlugin.Vector3f Position;
    public static readonly OVRPlugin.Posef identity = new OVRPlugin.Posef()
    {
      Orientation = OVRPlugin.Quatf.identity,
      Position = OVRPlugin.Vector3f.zero
    };

    public override string ToString() => string.Format("Position ({0}), Orientation({1})", (object) this.Position, (object) this.Orientation);
  }

  public struct PoseStatef
  {
    public OVRPlugin.Posef Pose;
    public OVRPlugin.Vector3f Velocity;
    public OVRPlugin.Vector3f Acceleration;
    public OVRPlugin.Vector3f AngularVelocity;
    public OVRPlugin.Vector3f AngularAcceleration;
    private double Time;
  }

  public struct ControllerState4
  {
    public uint ConnectedControllers;
    public uint Buttons;
    public uint Touches;
    public uint NearTouches;
    public float LIndexTrigger;
    public float RIndexTrigger;
    public float LHandTrigger;
    public float RHandTrigger;
    public OVRPlugin.Vector2f LThumbstick;
    public OVRPlugin.Vector2f RThumbstick;
    public OVRPlugin.Vector2f LTouchpad;
    public OVRPlugin.Vector2f RTouchpad;
    public byte LBatteryPercentRemaining;
    public byte RBatteryPercentRemaining;
    public byte LRecenterCount;
    public byte RRecenterCount;
    public byte Reserved_27;
    public byte Reserved_26;
    public byte Reserved_25;
    public byte Reserved_24;
    public byte Reserved_23;
    public byte Reserved_22;
    public byte Reserved_21;
    public byte Reserved_20;
    public byte Reserved_19;
    public byte Reserved_18;
    public byte Reserved_17;
    public byte Reserved_16;
    public byte Reserved_15;
    public byte Reserved_14;
    public byte Reserved_13;
    public byte Reserved_12;
    public byte Reserved_11;
    public byte Reserved_10;
    public byte Reserved_09;
    public byte Reserved_08;
    public byte Reserved_07;
    public byte Reserved_06;
    public byte Reserved_05;
    public byte Reserved_04;
    public byte Reserved_03;
    public byte Reserved_02;
    public byte Reserved_01;
    public byte Reserved_00;

    public ControllerState4(OVRPlugin.ControllerState2 cs)
    {
      this.ConnectedControllers = cs.ConnectedControllers;
      this.Buttons = cs.Buttons;
      this.Touches = cs.Touches;
      this.NearTouches = cs.NearTouches;
      this.LIndexTrigger = cs.LIndexTrigger;
      this.RIndexTrigger = cs.RIndexTrigger;
      this.LHandTrigger = cs.LHandTrigger;
      this.RHandTrigger = cs.RHandTrigger;
      this.LThumbstick = cs.LThumbstick;
      this.RThumbstick = cs.RThumbstick;
      this.LTouchpad = cs.LTouchpad;
      this.RTouchpad = cs.RTouchpad;
      this.LBatteryPercentRemaining = (byte) 0;
      this.RBatteryPercentRemaining = (byte) 0;
      this.LRecenterCount = (byte) 0;
      this.RRecenterCount = (byte) 0;
      this.Reserved_27 = (byte) 0;
      this.Reserved_26 = (byte) 0;
      this.Reserved_25 = (byte) 0;
      this.Reserved_24 = (byte) 0;
      this.Reserved_23 = (byte) 0;
      this.Reserved_22 = (byte) 0;
      this.Reserved_21 = (byte) 0;
      this.Reserved_20 = (byte) 0;
      this.Reserved_19 = (byte) 0;
      this.Reserved_18 = (byte) 0;
      this.Reserved_17 = (byte) 0;
      this.Reserved_16 = (byte) 0;
      this.Reserved_15 = (byte) 0;
      this.Reserved_14 = (byte) 0;
      this.Reserved_13 = (byte) 0;
      this.Reserved_12 = (byte) 0;
      this.Reserved_11 = (byte) 0;
      this.Reserved_10 = (byte) 0;
      this.Reserved_09 = (byte) 0;
      this.Reserved_08 = (byte) 0;
      this.Reserved_07 = (byte) 0;
      this.Reserved_06 = (byte) 0;
      this.Reserved_05 = (byte) 0;
      this.Reserved_04 = (byte) 0;
      this.Reserved_03 = (byte) 0;
      this.Reserved_02 = (byte) 0;
      this.Reserved_01 = (byte) 0;
      this.Reserved_00 = (byte) 0;
    }
  }

  public struct ControllerState2
  {
    public uint ConnectedControllers;
    public uint Buttons;
    public uint Touches;
    public uint NearTouches;
    public float LIndexTrigger;
    public float RIndexTrigger;
    public float LHandTrigger;
    public float RHandTrigger;
    public OVRPlugin.Vector2f LThumbstick;
    public OVRPlugin.Vector2f RThumbstick;
    public OVRPlugin.Vector2f LTouchpad;
    public OVRPlugin.Vector2f RTouchpad;

    public ControllerState2(OVRPlugin.ControllerState cs)
    {
      this.ConnectedControllers = cs.ConnectedControllers;
      this.Buttons = cs.Buttons;
      this.Touches = cs.Touches;
      this.NearTouches = cs.NearTouches;
      this.LIndexTrigger = cs.LIndexTrigger;
      this.RIndexTrigger = cs.RIndexTrigger;
      this.LHandTrigger = cs.LHandTrigger;
      this.RHandTrigger = cs.RHandTrigger;
      this.LThumbstick = cs.LThumbstick;
      this.RThumbstick = cs.RThumbstick;
      this.LTouchpad = new OVRPlugin.Vector2f()
      {
        x = 0.0f,
        y = 0.0f
      };
      this.RTouchpad = new OVRPlugin.Vector2f()
      {
        x = 0.0f,
        y = 0.0f
      };
    }
  }

  public struct ControllerState
  {
    public uint ConnectedControllers;
    public uint Buttons;
    public uint Touches;
    public uint NearTouches;
    public float LIndexTrigger;
    public float RIndexTrigger;
    public float LHandTrigger;
    public float RHandTrigger;
    public OVRPlugin.Vector2f LThumbstick;
    public OVRPlugin.Vector2f RThumbstick;
  }

  public struct HapticsBuffer
  {
    public IntPtr Samples;
    public int SamplesCount;
  }

  public struct HapticsState
  {
    public int SamplesAvailable;
    public int SamplesQueued;
  }

  public struct HapticsDesc
  {
    public int SampleRateHz;
    public int SampleSizeInBytes;
    public int MinimumSafeSamplesQueued;
    public int MinimumBufferSamplesCount;
    public int OptimalBufferSamplesCount;
    public int MaximumBufferSamplesCount;
  }

  public struct AppPerfFrameStats
  {
    public int HmdVsyncIndex;
    public int AppFrameIndex;
    public int AppDroppedFrameCount;
    public float AppMotionToPhotonLatency;
    public float AppQueueAheadTime;
    public float AppCpuElapsedTime;
    public float AppGpuElapsedTime;
    public int CompositorFrameIndex;
    public int CompositorDroppedFrameCount;
    public float CompositorLatency;
    public float CompositorCpuElapsedTime;
    public float CompositorGpuElapsedTime;
    public float CompositorCpuStartToGpuEndElapsedTime;
    public float CompositorGpuEndToVsyncElapsedTime;
  }

  public struct AppPerfStats
  {
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
    public OVRPlugin.AppPerfFrameStats[] FrameStats;
    public int FrameStatsCount;
    public OVRPlugin.Bool AnyFrameStatsDropped;
    public float AdaptiveGpuPerformanceScale;
  }

  public struct Sizei
  {
    public int w;
    public int h;
  }

  public struct Sizef
  {
    public float w;
    public float h;
  }

  public struct Vector2i
  {
    public int x;
    public int y;
  }

  public struct Recti
  {
    private OVRPlugin.Vector2i Pos;
    private OVRPlugin.Sizei Size;
  }

  public struct Rectf
  {
    private OVRPlugin.Vector2f Pos;
    private OVRPlugin.Sizef Size;
  }

  public struct Frustumf
  {
    public float zNear;
    public float zFar;
    public float fovX;
    public float fovY;
  }

  public enum BoundaryType
  {
    OuterBoundary = 1,
    PlayArea = 256, // 0x00000100
  }

  public struct BoundaryTestResult
  {
    public OVRPlugin.Bool IsTriggering;
    public float ClosestDistance;
    public OVRPlugin.Vector3f ClosestPoint;
    public OVRPlugin.Vector3f ClosestPointNormal;
  }

  public struct BoundaryLookAndFeel
  {
    public OVRPlugin.Colorf Color;
  }

  public struct BoundaryGeometry
  {
    public OVRPlugin.BoundaryType BoundaryType;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
    public OVRPlugin.Vector3f[] Points;
    public int PointsCount;
  }

  public struct Colorf
  {
    public float r;
    public float g;
    public float b;
    public float a;
  }

  public struct Fovf
  {
    public float UpTan;
    public float DownTan;
    public float LeftTan;
    public float RightTan;
  }

  public struct CameraIntrinsics
  {
    public bool IsValid;
    public double LastChangedTimeSeconds;
    public OVRPlugin.Fovf FOVPort;
    public float VirtualNearPlaneDistanceMeters;
    public float VirtualFarPlaneDistanceMeters;
    public OVRPlugin.Sizei ImageSensorPixelResolution;
  }

  public struct CameraExtrinsics
  {
    public bool IsValid;
    public double LastChangedTimeSeconds;
    public OVRPlugin.CameraStatus CameraStatusData;
    public OVRPlugin.Node AttachedToNode;
    public OVRPlugin.Posef RelativePose;
  }

  public enum LayerLayout
  {
    Stereo = 0,
    Mono = 1,
    DoubleWide = 2,
    Array = 3,
    EnumSize = 15, // 0x0000000F
  }

  public enum LayerFlags
  {
    Static = 1,
    LoadingScreen = 2,
    SymmetricFov = 4,
    TextureOriginAtBottomLeft = 8,
    ChromaticAberrationCorrection = 16, // 0x00000010
    NoAllocation = 32, // 0x00000020
    ProtectedContent = 64, // 0x00000040
  }

  public struct LayerDesc
  {
    public OVRPlugin.OverlayShape Shape;
    public OVRPlugin.LayerLayout Layout;
    public OVRPlugin.Sizei TextureSize;
    public int MipLevels;
    public int SampleCount;
    public OVRPlugin.EyeTextureFormat Format;
    public int LayerFlags;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
    public OVRPlugin.Fovf[] Fov;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
    public OVRPlugin.Rectf[] VisibleRect;
    public OVRPlugin.Sizei MaxViewportSize;
    private OVRPlugin.EyeTextureFormat DepthFormat;

    public override string ToString()
    {
      string str = ", ";
      return this.Shape.ToString() + str + this.Layout.ToString() + str + this.TextureSize.w.ToString() + "x" + this.TextureSize.h.ToString() + str + this.MipLevels.ToString() + str + this.SampleCount.ToString() + str + this.Format.ToString() + str + this.LayerFlags.ToString();
    }
  }

  public struct LayerSubmit
  {
    private int LayerId;
    private int TextureStage;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
    private OVRPlugin.Recti[] ViewportRect;
    private OVRPlugin.Posef Pose;
    private int LayerSubmitFlags;
  }

  private static class OVRP_0_1_0
  {
    public static readonly Version version = new Version(0, 1, 0);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Sizei ovrp_GetEyeTextureSize(OVRPlugin.Eye eyeId);
  }

  private static class OVRP_0_1_1
  {
    public static readonly Version version = new Version(0, 1, 1);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Bool ovrp_SetOverlayQuad2(
      OVRPlugin.Bool onTop,
      OVRPlugin.Bool headLocked,
      IntPtr texture,
      IntPtr device,
      OVRPlugin.Posef pose,
      OVRPlugin.Vector3f scale);
  }

  private static class OVRP_0_1_2
  {
    public static readonly Version version = new Version(0, 1, 2);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Posef ovrp_GetNodePose(OVRPlugin.Node nodeId);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Bool ovrp_SetControllerVibration(
      uint controllerMask,
      float frequency,
      float amplitude);
  }

  private static class OVRP_0_1_3
  {
    public static readonly Version version = new Version(0, 1, 3);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Posef ovrp_GetNodeVelocity(OVRPlugin.Node nodeId);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Posef ovrp_GetNodeAcceleration(OVRPlugin.Node nodeId);
  }

  private static class OVRP_0_5_0
  {
    public static readonly Version version = new Version(0, 5, 0);
  }

  private static class OVRP_1_0_0
  {
    public static readonly Version version = new Version(1, 0, 0);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.TrackingOrigin ovrp_GetTrackingOriginType();

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Bool ovrp_SetTrackingOriginType(
      OVRPlugin.TrackingOrigin originType);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Posef ovrp_GetTrackingCalibratedOrigin();

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Bool ovrp_RecenterTrackingOrigin(uint flags);
  }

  private static class OVRP_1_1_0
  {
    public static readonly Version version = new Version(1, 1, 0);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Bool ovrp_GetInitialized();

    [DllImport("OVRPlugin", EntryPoint = "ovrp_GetVersion", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr _ovrp_GetVersion();

    public static string ovrp_GetVersion() => Marshal.PtrToStringAnsi(OVRPlugin.OVRP_1_1_0._ovrp_GetVersion());

    [DllImport("OVRPlugin", EntryPoint = "ovrp_GetNativeSDKVersion", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr _ovrp_GetNativeSDKVersion();

    public static string ovrp_GetNativeSDKVersion() => Marshal.PtrToStringAnsi(OVRPlugin.OVRP_1_1_0._ovrp_GetNativeSDKVersion());

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ovrp_GetAudioOutId();

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ovrp_GetAudioInId();

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern float ovrp_GetEyeTextureScale();

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Bool ovrp_SetEyeTextureScale(float value);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Bool ovrp_GetTrackingOrientationSupported();

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Bool ovrp_GetTrackingOrientationEnabled();

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Bool ovrp_SetTrackingOrientationEnabled(
      OVRPlugin.Bool value);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Bool ovrp_GetTrackingPositionSupported();

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Bool ovrp_GetTrackingPositionEnabled();

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Bool ovrp_SetTrackingPositionEnabled(
      OVRPlugin.Bool value);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Bool ovrp_GetNodePresent(OVRPlugin.Node nodeId);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Bool ovrp_GetNodeOrientationTracked(
      OVRPlugin.Node nodeId);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Bool ovrp_GetNodePositionTracked(OVRPlugin.Node nodeId);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Frustumf ovrp_GetNodeFrustum(OVRPlugin.Node nodeId);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.ControllerState ovrp_GetControllerState(
      uint controllerMask);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern int ovrp_GetSystemCpuLevel();

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Bool ovrp_SetSystemCpuLevel(int value);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern int ovrp_GetSystemGpuLevel();

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Bool ovrp_SetSystemGpuLevel(int value);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Bool ovrp_GetSystemPowerSavingMode();

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern float ovrp_GetSystemDisplayFrequency();

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern int ovrp_GetSystemVSyncCount();

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern float ovrp_GetSystemVolume();

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.BatteryStatus ovrp_GetSystemBatteryStatus();

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern float ovrp_GetSystemBatteryLevel();

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern float ovrp_GetSystemBatteryTemperature();

    [DllImport("OVRPlugin", EntryPoint = "ovrp_GetSystemProductName", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr _ovrp_GetSystemProductName();

    public static string ovrp_GetSystemProductName() => Marshal.PtrToStringAnsi(OVRPlugin.OVRP_1_1_0._ovrp_GetSystemProductName());

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Bool ovrp_ShowSystemUI(OVRPlugin.PlatformUI ui);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Bool ovrp_GetAppMonoscopic();

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Bool ovrp_SetAppMonoscopic(OVRPlugin.Bool value);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Bool ovrp_GetAppHasVrFocus();

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Bool ovrp_GetAppShouldQuit();

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Bool ovrp_GetAppShouldRecenter();

    [DllImport("OVRPlugin", EntryPoint = "ovrp_GetAppLatencyTimings", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr _ovrp_GetAppLatencyTimings();

    public static string ovrp_GetAppLatencyTimings() => Marshal.PtrToStringAnsi(OVRPlugin.OVRP_1_1_0._ovrp_GetAppLatencyTimings());

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Bool ovrp_GetUserPresent();

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern float ovrp_GetUserIPD();

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Bool ovrp_SetUserIPD(float value);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern float ovrp_GetUserEyeDepth();

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Bool ovrp_SetUserEyeDepth(float value);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern float ovrp_GetUserEyeHeight();

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Bool ovrp_SetUserEyeHeight(float value);
  }

  private static class OVRP_1_2_0
  {
    public static readonly Version version = new Version(1, 2, 0);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Bool ovrp_SetSystemVSyncCount(int vsyncCount);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Bool ovrpi_SetTrackingCalibratedOrigin();
  }

  private static class OVRP_1_3_0
  {
    public static readonly Version version = new Version(1, 3, 0);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Bool ovrp_GetEyeOcclusionMeshEnabled();

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Bool ovrp_SetEyeOcclusionMeshEnabled(
      OVRPlugin.Bool value);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Bool ovrp_GetSystemHeadphonesPresent();
  }

  private static class OVRP_1_5_0
  {
    public static readonly Version version = new Version(1, 5, 0);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.SystemRegion ovrp_GetSystemRegion();
  }

  private static class OVRP_1_6_0
  {
    public static readonly Version version = new Version(1, 6, 0);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Bool ovrp_GetTrackingIPDEnabled();

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Bool ovrp_SetTrackingIPDEnabled(OVRPlugin.Bool value);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.HapticsDesc ovrp_GetControllerHapticsDesc(
      uint controllerMask);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.HapticsState ovrp_GetControllerHapticsState(
      uint controllerMask);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Bool ovrp_SetControllerHaptics(
      uint controllerMask,
      OVRPlugin.HapticsBuffer hapticsBuffer);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Bool ovrp_SetOverlayQuad3(
      uint flags,
      IntPtr textureLeft,
      IntPtr textureRight,
      IntPtr device,
      OVRPlugin.Posef pose,
      OVRPlugin.Vector3f scale,
      int layerIndex);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern float ovrp_GetEyeRecommendedResolutionScale();

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern float ovrp_GetAppCpuStartToGpuEndTime();

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern int ovrp_GetSystemRecommendedMSAALevel();
  }

  private static class OVRP_1_7_0
  {
    public static readonly Version version = new Version(1, 7, 0);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Bool ovrp_GetAppChromaticCorrection();

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Bool ovrp_SetAppChromaticCorrection(OVRPlugin.Bool value);
  }

  private static class OVRP_1_8_0
  {
    public static readonly Version version = new Version(1, 8, 0);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Bool ovrp_GetBoundaryConfigured();

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.BoundaryTestResult ovrp_TestBoundaryNode(
      OVRPlugin.Node nodeId,
      OVRPlugin.BoundaryType boundaryType);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.BoundaryTestResult ovrp_TestBoundaryPoint(
      OVRPlugin.Vector3f point,
      OVRPlugin.BoundaryType boundaryType);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Bool ovrp_SetBoundaryLookAndFeel(
      OVRPlugin.BoundaryLookAndFeel lookAndFeel);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Bool ovrp_ResetBoundaryLookAndFeel();

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.BoundaryGeometry ovrp_GetBoundaryGeometry(
      OVRPlugin.BoundaryType boundaryType);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Vector3f ovrp_GetBoundaryDimensions(
      OVRPlugin.BoundaryType boundaryType);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Bool ovrp_GetBoundaryVisible();

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Bool ovrp_SetBoundaryVisible(OVRPlugin.Bool value);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Bool ovrp_Update2(
      int stateId,
      int frameIndex,
      double predictionSeconds);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Posef ovrp_GetNodePose2(
      int stateId,
      OVRPlugin.Node nodeId);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Posef ovrp_GetNodeVelocity2(
      int stateId,
      OVRPlugin.Node nodeId);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Posef ovrp_GetNodeAcceleration2(
      int stateId,
      OVRPlugin.Node nodeId);
  }

  private static class OVRP_1_9_0
  {
    public static readonly Version version = new Version(1, 9, 0);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.SystemHeadset ovrp_GetSystemHeadsetType();

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Controller ovrp_GetActiveController();

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Controller ovrp_GetConnectedControllers();

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Bool ovrp_GetBoundaryGeometry2(
      OVRPlugin.BoundaryType boundaryType,
      IntPtr points,
      ref int pointsCount);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.AppPerfStats ovrp_GetAppPerfStats();

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Bool ovrp_ResetAppPerfStats();
  }

  private static class OVRP_1_10_0
  {
    public static readonly Version version = new Version(1, 10, 0);
  }

  private static class OVRP_1_11_0
  {
    public static readonly Version version = new Version(1, 11, 0);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Bool ovrp_SetDesiredEyeTextureFormat(
      OVRPlugin.EyeTextureFormat value);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.EyeTextureFormat ovrp_GetDesiredEyeTextureFormat();
  }

  private static class OVRP_1_12_0
  {
    public static readonly Version version = new Version(1, 12, 0);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern float ovrp_GetAppFramerate();

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.PoseStatef ovrp_GetNodePoseState(
      OVRPlugin.Step stepId,
      OVRPlugin.Node nodeId);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.ControllerState2 ovrp_GetControllerState2(
      uint controllerMask);
  }

  private static class OVRP_1_15_0
  {
    public const int OVRP_EXTERNAL_CAMERA_NAME_SIZE = 32;
    public static readonly Version version = new Version(1, 15, 0);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Result ovrp_InitializeMixedReality();

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Result ovrp_ShutdownMixedReality();

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Bool ovrp_GetMixedRealityInitialized();

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Result ovrp_UpdateExternalCamera();

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Result ovrp_GetExternalCameraCount(out int cameraCount);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Result ovrp_GetExternalCameraName(
      int cameraId,
      [MarshalAs(UnmanagedType.LPArray, SizeConst = 32)] char[] cameraName);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Result ovrp_GetExternalCameraIntrinsics(
      int cameraId,
      out OVRPlugin.CameraIntrinsics cameraIntrinsics);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Result ovrp_GetExternalCameraExtrinsics(
      int cameraId,
      out OVRPlugin.CameraExtrinsics cameraExtrinsics);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Result ovrp_CalculateLayerDesc(
      OVRPlugin.OverlayShape shape,
      OVRPlugin.LayerLayout layout,
      ref OVRPlugin.Sizei textureSize,
      int mipLevels,
      int sampleCount,
      OVRPlugin.EyeTextureFormat format,
      int layerFlags,
      ref OVRPlugin.LayerDesc layerDesc);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Result ovrp_EnqueueSetupLayer(
      ref OVRPlugin.LayerDesc desc,
      IntPtr layerId);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Result ovrp_EnqueueDestroyLayer(IntPtr layerId);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Result ovrp_GetLayerTextureStageCount(
      int layerId,
      ref int layerTextureStageCount);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Result ovrp_GetLayerTexturePtr(
      int layerId,
      int stage,
      OVRPlugin.Eye eyeId,
      ref IntPtr textureHandle);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Result ovrp_EnqueueSubmitLayer(
      uint flags,
      IntPtr textureLeft,
      IntPtr textureRight,
      int layerId,
      int frameIndex,
      ref OVRPlugin.Posef pose,
      ref OVRPlugin.Vector3f scale,
      int layerIndex);
  }

  private static class OVRP_1_16_0
  {
    public static readonly Version version = new Version(1, 16, 0);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Result ovrp_UpdateCameraDevices();

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Bool ovrp_IsCameraDeviceAvailable(
      OVRPlugin.CameraDevice cameraDevice);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Result ovrp_SetCameraDevicePreferredColorFrameSize(
      OVRPlugin.CameraDevice cameraDevice,
      OVRPlugin.Sizei preferredColorFrameSize);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Result ovrp_OpenCameraDevice(
      OVRPlugin.CameraDevice cameraDevice);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Result ovrp_CloseCameraDevice(
      OVRPlugin.CameraDevice cameraDevice);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Bool ovrp_HasCameraDeviceOpened(
      OVRPlugin.CameraDevice cameraDevice);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Bool ovrp_IsCameraDeviceColorFrameAvailable(
      OVRPlugin.CameraDevice cameraDevice);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Result ovrp_GetCameraDeviceColorFrameSize(
      OVRPlugin.CameraDevice cameraDevice,
      out OVRPlugin.Sizei colorFrameSize);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Result ovrp_GetCameraDeviceColorFrameBgraPixels(
      OVRPlugin.CameraDevice cameraDevice,
      out IntPtr colorFrameBgraPixels,
      out int colorFrameRowPitch);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Result ovrp_GetControllerState4(
      uint controllerMask,
      ref OVRPlugin.ControllerState4 controllerState);
  }

  private static class OVRP_1_17_0
  {
    public static readonly Version version = new Version(1, 17, 0);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Result ovrp_GetExternalCameraPose(
      OVRPlugin.CameraDevice camera,
      out OVRPlugin.Posef cameraPose);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Result ovrp_ConvertPoseToCameraSpace(
      OVRPlugin.CameraDevice camera,
      ref OVRPlugin.Posef trackingSpacePose,
      out OVRPlugin.Posef cameraSpacePose);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Result ovrp_GetCameraDeviceIntrinsicsParameters(
      OVRPlugin.CameraDevice camera,
      out OVRPlugin.Bool supportIntrinsics,
      out OVRPlugin.CameraDeviceIntrinsicsParameters intrinsicsParameters);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Result ovrp_DoesCameraDeviceSupportDepth(
      OVRPlugin.CameraDevice camera,
      out OVRPlugin.Bool supportDepth);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Result ovrp_GetCameraDeviceDepthSensingMode(
      OVRPlugin.CameraDevice camera,
      out OVRPlugin.CameraDeviceDepthSensingMode depthSensoringMode);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Result ovrp_SetCameraDeviceDepthSensingMode(
      OVRPlugin.CameraDevice camera,
      OVRPlugin.CameraDeviceDepthSensingMode depthSensoringMode);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Result ovrp_GetCameraDevicePreferredDepthQuality(
      OVRPlugin.CameraDevice camera,
      out OVRPlugin.CameraDeviceDepthQuality depthQuality);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Result ovrp_SetCameraDevicePreferredDepthQuality(
      OVRPlugin.CameraDevice camera,
      OVRPlugin.CameraDeviceDepthQuality depthQuality);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Result ovrp_IsCameraDeviceDepthFrameAvailable(
      OVRPlugin.CameraDevice camera,
      out OVRPlugin.Bool available);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Result ovrp_GetCameraDeviceDepthFrameSize(
      OVRPlugin.CameraDevice camera,
      out OVRPlugin.Sizei depthFrameSize);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Result ovrp_GetCameraDeviceDepthFramePixels(
      OVRPlugin.CameraDevice cameraDevice,
      out IntPtr depthFramePixels,
      out int depthFrameRowPitch);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Result ovrp_GetCameraDeviceDepthConfidencePixels(
      OVRPlugin.CameraDevice cameraDevice,
      out IntPtr depthConfidencePixels,
      out int depthConfidenceRowPitch);
  }

  private static class OVRP_1_18_0
  {
    public static readonly Version version = new Version(1, 18, 0);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Result ovrp_SetHandNodePoseStateLatency(
      double latencyInSeconds);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Result ovrp_GetHandNodePoseStateLatency(
      out double latencyInSeconds);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Result ovrp_GetAppHasInputFocus(
      out OVRPlugin.Bool appHasInputFocus);
  }

  private static class OVRP_1_19_0
  {
    public static readonly Version version = new Version(1, 19, 0);
  }

  private static class OVRP_1_21_0
  {
    public static readonly Version version = new Version(1, 21, 0);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Result ovrp_GetTiledMultiResSupported(
      out OVRPlugin.Bool foveationSupported);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Result ovrp_GetTiledMultiResLevel(
      out OVRPlugin.TiledMultiResLevel level);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Result ovrp_SetTiledMultiResLevel(
      OVRPlugin.TiledMultiResLevel level);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Result ovrp_GetGPUUtilSupported(
      out OVRPlugin.Bool gpuUtilSupported);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Result ovrp_GetGPUUtilLevel(out float gpuUtil);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Result ovrp_GetSystemDisplayFrequency2(
      out float systemDisplayFrequency);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Result ovrp_GetSystemDisplayAvailableFrequencies(
      IntPtr systemDisplayAvailableFrequencies,
      out int numFrequencies);

    [DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
    public static extern OVRPlugin.Result ovrp_SetSystemDisplayFrequency(
      float requestedFrequency);
  }

  private static class OVRP_1_26_0
  {
    public static readonly Version version = new Version(1, 26, 0);
  }
}
