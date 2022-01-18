// Decompiled with JetBrains decompiler
// Type: OVRProgressIndicator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class OVRProgressIndicator : MonoBehaviour
{
  public MeshRenderer progressImage;
  [Range(0.0f, 1f)]
  public float currentProgress = 0.7f;

  private void Awake() => this.progressImage.sortingOrder = 150;

  private void Update() => this.progressImage.sharedMaterial.SetFloat("_AlphaCutoff", 1f - this.currentProgress);
}
