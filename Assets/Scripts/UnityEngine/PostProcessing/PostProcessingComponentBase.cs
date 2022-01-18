// Decompiled with JetBrains decompiler
// Type: UnityEngine.PostProcessing.PostProcessingComponentBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

namespace UnityEngine.PostProcessing
{
  public abstract class PostProcessingComponentBase
  {
    public PostProcessingContext context;

    public virtual DepthTextureMode GetCameraFlags() => DepthTextureMode.None;

    public abstract bool active { get; }

    public virtual void OnEnable()
    {
    }

    public virtual void OnDisable()
    {
    }

    public abstract PostProcessingModel GetModel();
  }
}
