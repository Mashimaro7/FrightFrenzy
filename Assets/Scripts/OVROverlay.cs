// Decompiled with JetBrains decompiler
// Type: OVROverlay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.XR;

public class OVROverlay : MonoBehaviour
{
  public OVROverlay.OverlayType currentOverlayType = OVROverlay.OverlayType.Overlay;
  public bool isDynamic;
  public bool isProtectedContent;
  public OVROverlay.OverlayShape currentOverlayShape;
  private OVROverlay.OverlayShape prevOverlayShape;
  public Texture[] textures = new Texture[2];
  protected IntPtr[] texturePtrs = new IntPtr[2]
  {
    IntPtr.Zero,
    IntPtr.Zero
  };
  protected bool isOverridePending;
  internal const int maxInstances = 15;
  internal static OVROverlay[] instances = new OVROverlay[15];
  private static Material tex2DMaterial;
  private static Material cubeMaterial;
  private OVROverlay.LayerTexture[] layerTextures;
  private OVRPlugin.LayerDesc layerDesc;
  private int stageCount = -1;
  private int layerIndex = -1;
  private int layerId;
  private GCHandle layerIdHandle;
  private IntPtr layerIdPtr = IntPtr.Zero;
  private int frameIndex;
  private int prevFrameIndex = -1;
  private Renderer rend;

  public void OverrideOverlayTextureInfo(Texture srcTexture, IntPtr nativePtr, XRNode node)
  {
    int index = node == XRNode.RightEye ? 1 : 0;
    if (this.textures.Length <= index)
      return;
    this.textures[index] = srcTexture;
    this.texturePtrs[index] = nativePtr;
    this.isOverridePending = true;
  }

  private OVRPlugin.LayerLayout layout => OVRPlugin.LayerLayout.Mono;

  private int texturesPerStage => this.layout != OVRPlugin.LayerLayout.Stereo ? 1 : 2;

  private bool CreateLayer(
    int mipLevels,
    int sampleCount,
    OVRPlugin.EyeTextureFormat etFormat,
    int flags,
    OVRPlugin.Sizei size,
    OVRPlugin.OverlayShape shape)
  {
    if (!this.layerIdHandle.IsAllocated || this.layerIdPtr == IntPtr.Zero)
    {
      this.layerIdHandle = GCHandle.Alloc((object) this.layerId, GCHandleType.Pinned);
      this.layerIdPtr = this.layerIdHandle.AddrOfPinnedObject();
    }
    if (this.layerIndex == -1)
    {
      for (int index = 0; index < 15; ++index)
      {
        if ((UnityEngine.Object) OVROverlay.instances[index] == (UnityEngine.Object) null || (UnityEngine.Object) OVROverlay.instances[index] == (UnityEngine.Object) this)
        {
          this.layerIndex = index;
          OVROverlay.instances[index] = this;
          break;
        }
      }
    }
    if ((this.isOverridePending || this.layerDesc.MipLevels != mipLevels || this.layerDesc.SampleCount != sampleCount || this.layerDesc.Format != etFormat || this.layerDesc.Layout != this.layout || this.layerDesc.LayerFlags != flags || !this.layerDesc.TextureSize.Equals((object) size) ? 1 : (this.layerDesc.Shape != shape ? 1 : 0)) == 0)
      return false;
    OVRPlugin.LayerDesc layerDesc = OVRPlugin.CalculateLayerDesc(shape, this.layout, size, mipLevels, sampleCount, etFormat, flags);
    OVRPlugin.EnqueueSetupLayer(layerDesc, this.layerIdPtr);
    this.layerId = (int) this.layerIdHandle.Target;
    if (this.layerId > 0)
    {
      this.layerDesc = layerDesc;
      this.stageCount = OVRPlugin.GetLayerTextureStageCount(this.layerId);
    }
    this.isOverridePending = false;
    return true;
  }

  private bool CreateLayerTextures(bool useMipmaps, OVRPlugin.Sizei size, bool isHdr)
  {
    bool layerTextures = false;
    if (this.stageCount <= 0)
      return false;
    if (this.layerTextures == null)
      this.layerTextures = new OVROverlay.LayerTexture[this.texturesPerStage];
    for (int eyeId = 0; eyeId < this.texturesPerStage; ++eyeId)
    {
      if (this.layerTextures[eyeId].swapChain == null)
        this.layerTextures[eyeId].swapChain = new Texture[this.stageCount];
      if (this.layerTextures[eyeId].swapChainPtr == null)
        this.layerTextures[eyeId].swapChainPtr = new IntPtr[this.stageCount];
      for (int stage = 0; stage < this.stageCount; ++stage)
      {
        Texture texture1 = this.layerTextures[eyeId].swapChain[stage];
        IntPtr layerTexture = this.layerTextures[eyeId].swapChainPtr[stage];
        if (!((UnityEngine.Object) texture1 != (UnityEngine.Object) null) || !(layerTexture != IntPtr.Zero))
        {
          if (layerTexture == IntPtr.Zero)
            layerTexture = OVRPlugin.GetLayerTexture(this.layerId, stage, (OVRPlugin.Eye) eyeId);
          if (!(layerTexture == IntPtr.Zero))
          {
            TextureFormat format = isHdr ? TextureFormat.RGBAHalf : TextureFormat.RGBA32;
            Texture texture2 = this.currentOverlayShape == OVROverlay.OverlayShape.Cubemap || this.currentOverlayShape == OVROverlay.OverlayShape.OffcenterCubemap ? (Texture) Cubemap.CreateExternalTexture(size.w, format, useMipmaps, layerTexture) : (Texture) Texture2D.CreateExternalTexture(size.w, size.h, format, useMipmaps, true, layerTexture);
            this.layerTextures[eyeId].swapChain[stage] = texture2;
            this.layerTextures[eyeId].swapChainPtr[stage] = layerTexture;
            layerTextures = true;
          }
        }
      }
    }
    return layerTextures;
  }

  private void DestroyLayerTextures()
  {
    for (int index1 = 0; this.layerTextures != null && index1 < this.texturesPerStage; ++index1)
    {
      if (this.layerTextures[index1].swapChain != null)
      {
        for (int index2 = 0; index2 < this.stageCount; ++index2)
          UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.layerTextures[index1].swapChain[index2]);
      }
    }
    this.layerTextures = (OVROverlay.LayerTexture[]) null;
  }

  private void DestroyLayer()
  {
    if (this.layerIndex != -1)
    {
      OVRPlugin.EnqueueSubmitLayer(true, false, IntPtr.Zero, IntPtr.Zero, -1, 0, OVRPose.identity.ToPosef(), Vector3.one.ToVector3f(), this.layerIndex, (OVRPlugin.OverlayShape) this.prevOverlayShape);
      OVROverlay.instances[this.layerIndex] = (OVROverlay) null;
      this.layerIndex = -1;
    }
    if (this.layerIdPtr != IntPtr.Zero)
    {
      OVRPlugin.EnqueueDestroyLayer(this.layerIdPtr);
      this.layerIdPtr = IntPtr.Zero;
      this.layerIdHandle.Free();
      this.layerId = 0;
    }
    this.layerDesc = new OVRPlugin.LayerDesc();
  }

  private bool LatchLayerTextures()
  {
    for (int index = 0; index < this.texturesPerStage; ++index)
    {
      if (((UnityEngine.Object) this.textures[index] != (UnityEngine.Object) this.layerTextures[index].appTexture || this.layerTextures[index].appTexturePtr == IntPtr.Zero) && (UnityEngine.Object) this.textures[index] != (UnityEngine.Object) null)
      {
        RenderTexture texture = this.textures[index] as RenderTexture;
        if ((bool) (UnityEngine.Object) texture && !texture.IsCreated())
          texture.Create();
        this.layerTextures[index].appTexturePtr = this.texturePtrs[index] != IntPtr.Zero ? this.texturePtrs[index] : this.textures[index].GetNativeTexturePtr();
        if (this.layerTextures[index].appTexturePtr != IntPtr.Zero)
          this.layerTextures[index].appTexture = this.textures[index];
      }
      if (this.currentOverlayShape == OVROverlay.OverlayShape.Cubemap && (UnityEngine.Object) (this.textures[index] as Cubemap) == (UnityEngine.Object) null)
      {
        Debug.LogError((object) "Need Cubemap texture for cube map overlay");
        return false;
      }
    }
    if (this.currentOverlayShape == OVROverlay.OverlayShape.OffcenterCubemap)
    {
      Debug.LogWarning((object) ("Overlay shape " + (object) this.currentOverlayShape + " is not supported on current platform"));
      return false;
    }
    return !((UnityEngine.Object) this.layerTextures[0].appTexture == (UnityEngine.Object) null) && !(this.layerTextures[0].appTexturePtr == IntPtr.Zero);
  }

  private OVRPlugin.LayerDesc GetCurrentLayerDesc()
  {
    OVRPlugin.LayerDesc currentLayerDesc = new OVRPlugin.LayerDesc()
    {
      Format = OVRPlugin.EyeTextureFormat.Default,
      LayerFlags = 8,
      Layout = this.layout,
      MipLevels = 1,
      SampleCount = 1,
      Shape = (OVRPlugin.OverlayShape) this.currentOverlayShape,
      TextureSize = new OVRPlugin.Sizei()
      {
        w = this.textures[0].width,
        h = this.textures[0].height
      }
    };
    Texture2D texture1 = this.textures[0] as Texture2D;
    if ((UnityEngine.Object) texture1 != (UnityEngine.Object) null)
    {
      if (texture1.format == TextureFormat.RGBAHalf || texture1.format == TextureFormat.RGBAFloat)
        currentLayerDesc.Format = OVRPlugin.EyeTextureFormat.R16G16B16A16_FP;
      currentLayerDesc.MipLevels = texture1.mipmapCount;
    }
    Cubemap texture2 = this.textures[0] as Cubemap;
    if ((UnityEngine.Object) texture2 != (UnityEngine.Object) null)
    {
      if (texture2.format == TextureFormat.RGBAHalf || texture2.format == TextureFormat.RGBAFloat)
        currentLayerDesc.Format = OVRPlugin.EyeTextureFormat.R16G16B16A16_FP;
      currentLayerDesc.MipLevels = texture2.mipmapCount;
    }
    RenderTexture texture3 = this.textures[0] as RenderTexture;
    if ((UnityEngine.Object) texture3 != (UnityEngine.Object) null)
    {
      currentLayerDesc.SampleCount = texture3.antiAliasing;
      if (texture3.format == RenderTextureFormat.ARGBHalf || texture3.format == RenderTextureFormat.ARGBFloat || texture3.format == RenderTextureFormat.RGB111110Float)
        currentLayerDesc.Format = OVRPlugin.EyeTextureFormat.R16G16B16A16_FP;
    }
    if (this.isProtectedContent)
      currentLayerDesc.LayerFlags |= 64;
    return currentLayerDesc;
  }

  private bool PopulateLayer(
    int mipLevels,
    bool isHdr,
    OVRPlugin.Sizei size,
    int sampleCount,
    int stage)
  {
    bool flag1 = false;
    RenderTextureFormat colorFormat = isHdr ? RenderTextureFormat.ARGBHalf : RenderTextureFormat.ARGB32;
    for (int index = 0; index < this.texturesPerStage; ++index)
    {
      Texture dst = this.layerTextures[index].swapChain[stage];
      if (!((UnityEngine.Object) dst == (UnityEngine.Object) null))
      {
        for (int dstMip = 0; dstMip < mipLevels; ++dstMip)
        {
          int width = size.w >> dstMip;
          if (width < 1)
            width = 1;
          int height = size.h >> dstMip;
          if (height < 1)
            height = 1;
          RenderTexture temporary = RenderTexture.GetTemporary(new RenderTextureDescriptor(width, height, colorFormat, 0)
          {
            msaaSamples = sampleCount,
            useMipMap = true,
            autoGenerateMips = false,
            sRGB = false
          });
          if (!temporary.IsCreated())
            temporary.Create();
          temporary.DiscardContents();
          Texture texture = this.textures[index];
          bool flag2 = isHdr || QualitySettings.activeColorSpace == ColorSpace.Linear;
          if (this.currentOverlayShape != OVROverlay.OverlayShape.Cubemap && this.currentOverlayShape != OVROverlay.OverlayShape.OffcenterCubemap)
          {
            OVROverlay.tex2DMaterial.SetInt("_linearToSrgb", !isHdr & flag2 ? 1 : 0);
            OVROverlay.tex2DMaterial.SetInt("_premultiply", 1);
            Graphics.Blit(this.textures[index], temporary, OVROverlay.tex2DMaterial);
            Graphics.CopyTexture((Texture) temporary, 0, 0, dst, 0, dstMip);
          }
          else
          {
            for (int dstElement = 0; dstElement < 6; ++dstElement)
            {
              OVROverlay.cubeMaterial.SetInt("_linearToSrgb", !isHdr & flag2 ? 1 : 0);
              OVROverlay.cubeMaterial.SetInt("_premultiply", 1);
              OVROverlay.cubeMaterial.SetInt("_face", dstElement);
              Graphics.Blit(this.textures[index], temporary, OVROverlay.cubeMaterial);
              Graphics.CopyTexture((Texture) temporary, 0, 0, dst, dstElement, dstMip);
            }
          }
          RenderTexture.ReleaseTemporary(temporary);
          flag1 = true;
        }
      }
    }
    return flag1;
  }

  private bool SubmitLayer(
    bool overlay,
    bool headLocked,
    OVRPose pose,
    Vector3 scale,
    int frameIndex)
  {
    int index = this.texturesPerStage >= 2 ? 1 : 0;
    int num = OVRPlugin.EnqueueSubmitLayer(overlay, headLocked, this.layerTextures[0].appTexturePtr, this.layerTextures[index].appTexturePtr, this.layerId, frameIndex, pose.flipZ().ToPosef(), scale.ToVector3f(), this.layerIndex, (OVRPlugin.OverlayShape) this.currentOverlayShape) ? 1 : 0;
    this.prevOverlayShape = this.currentOverlayShape;
    return num != 0;
  }

  private void Awake()
  {
    Debug.Log((object) "Overlay Awake");
    if ((UnityEngine.Object) OVROverlay.tex2DMaterial == (UnityEngine.Object) null)
      OVROverlay.tex2DMaterial = new Material(Shader.Find("Oculus/Texture2D Blit"));
    if ((UnityEngine.Object) OVROverlay.cubeMaterial == (UnityEngine.Object) null)
      OVROverlay.cubeMaterial = new Material(Shader.Find("Oculus/Cubemap Blit"));
    this.rend = this.GetComponent<Renderer>();
    if (this.textures.Length == 0)
      this.textures = new Texture[1];
    if (!((UnityEngine.Object) this.rend != (UnityEngine.Object) null) || !((UnityEngine.Object) this.textures[0] == (UnityEngine.Object) null))
      return;
    this.textures[0] = this.rend.material.mainTexture;
  }

  private void OnEnable()
  {
    if (OVRManager.isHmdPresent)
      return;
    this.enabled = false;
  }

  private void OnDisable()
  {
    if ((this.gameObject.hideFlags & HideFlags.DontSaveInBuild) != HideFlags.None)
      return;
    this.DestroyLayerTextures();
    this.DestroyLayer();
  }

  private void OnDestroy()
  {
    this.DestroyLayerTextures();
    this.DestroyLayer();
  }

  private bool ComputeSubmit(
    ref OVRPose pose,
    ref Vector3 scale,
    ref bool overlay,
    ref bool headLocked)
  {
    Camera main = Camera.main;
    overlay = this.currentOverlayType == OVROverlay.OverlayType.Overlay;
    headLocked = false;
    for (Transform transform = this.transform; (UnityEngine.Object) transform != (UnityEngine.Object) null && !headLocked; transform = transform.parent)
      headLocked |= (UnityEngine.Object) transform == (UnityEngine.Object) main.transform;
    pose = headLocked ? this.transform.ToHeadSpacePose(main) : this.transform.ToTrackingSpacePose(main);
    scale = this.transform.lossyScale;
    for (int index = 0; index < 3; ++index)
      scale[index] /= main.transform.lossyScale[index];
    if (this.currentOverlayShape == OVROverlay.OverlayShape.Cubemap)
      pose.position = main.transform.position;
    if (this.currentOverlayShape == OVROverlay.OverlayShape.OffcenterCubemap)
    {
      pose.position = this.transform.position;
      if ((double) pose.position.magnitude > 1.0)
      {
        Debug.LogWarning((object) "Your cube map center offset's magnitude is greater than 1, which will cause some cube map pixel always invisible .");
        return false;
      }
    }
    if (this.currentOverlayShape == OVROverlay.OverlayShape.Cylinder)
    {
      float num = (float) ((double) scale.x / (double) scale.z / 3.14159274101257 * 180.0);
      if ((double) num > 180.0)
      {
        Debug.LogWarning((object) ("Cylinder overlay's arc angle has to be below 180 degree, current arc angle is " + (object) num + " degree."));
        return false;
      }
    }
    return true;
  }

  private void LateUpdate()
  {
    if (this.currentOverlayType == OVROverlay.OverlayType.None || this.textures.Length < this.texturesPerStage || (UnityEngine.Object) this.textures[0] == (UnityEngine.Object) null)
      return;
    OVRPose identity = OVRPose.identity;
    Vector3 one = Vector3.one;
    bool overlay = false;
    bool headLocked = false;
    if (!this.ComputeSubmit(ref identity, ref one, ref overlay, ref headLocked))
      return;
    OVRPlugin.LayerDesc currentLayerDesc = this.GetCurrentLayerDesc();
    bool isHdr = currentLayerDesc.Format == OVRPlugin.EyeTextureFormat.R16G16B16A16_FP;
    bool layer = this.CreateLayer(currentLayerDesc.MipLevels, currentLayerDesc.SampleCount, currentLayerDesc.Format, currentLayerDesc.LayerFlags, currentLayerDesc.TextureSize, currentLayerDesc.Shape);
    if (this.layerIndex == -1 || this.layerId <= 0)
      return;
    bool useMipmaps = currentLayerDesc.MipLevels > 1;
    bool flag1 = layer | this.CreateLayerTextures(useMipmaps, currentLayerDesc.TextureSize, isHdr);
    if ((UnityEngine.Object) (this.layerTextures[0].appTexture as RenderTexture) != (UnityEngine.Object) null)
      this.isDynamic = true;
    if (!this.LatchLayerTextures())
      return;
    if (this.frameIndex > this.prevFrameIndex)
    {
      int stage = this.frameIndex % this.stageCount;
      if (!this.PopulateLayer(currentLayerDesc.MipLevels, isHdr, currentLayerDesc.TextureSize, currentLayerDesc.SampleCount, stage))
        return;
    }
    bool flag2 = this.SubmitLayer(overlay, headLocked, identity, one, this.frameIndex);
    this.prevFrameIndex = this.frameIndex;
    if (this.isDynamic)
      ++this.frameIndex;
    if (!(bool) (UnityEngine.Object) this.rend)
      return;
    this.rend.enabled = !flag2;
  }

  public enum OverlayShape
  {
    Quad = 0,
    Cylinder = 1,
    Cubemap = 2,
    OffcenterCubemap = 4,
    Equirect = 5,
  }

  public enum OverlayType
  {
    None,
    Underlay,
    Overlay,
  }

  private struct LayerTexture
  {
    public Texture appTexture;
    public IntPtr appTexturePtr;
    public Texture[] swapChain;
    public IntPtr[] swapChainPtr;
  }
}
