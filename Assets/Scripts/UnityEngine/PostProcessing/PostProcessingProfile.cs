// Decompiled with JetBrains decompiler
// Type: UnityEngine.PostProcessing.PostProcessingProfile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

namespace UnityEngine.PostProcessing
{
  public class PostProcessingProfile : ScriptableObject
  {
    public BuiltinDebugViewsModel debugViews = new BuiltinDebugViewsModel();
    public FogModel fog = new FogModel();
    public AntialiasingModel antialiasing = new AntialiasingModel();
    public AmbientOcclusionModel ambientOcclusion = new AmbientOcclusionModel();
    public ScreenSpaceReflectionModel screenSpaceReflection = new ScreenSpaceReflectionModel();
    public DepthOfFieldModel depthOfField = new DepthOfFieldModel();
    public MotionBlurModel motionBlur = new MotionBlurModel();
    public EyeAdaptationModel eyeAdaptation = new EyeAdaptationModel();
    public BloomModel bloom = new BloomModel();
    public ColorGradingModel colorGrading = new ColorGradingModel();
    public UserLutModel userLut = new UserLutModel();
    public ChromaticAberrationModel chromaticAberration = new ChromaticAberrationModel();
    public GrainModel grain = new GrainModel();
    public VignetteModel vignette = new VignetteModel();
    public DitheringModel dithering = new DitheringModel();
  }
}
