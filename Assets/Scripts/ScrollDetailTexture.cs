// Decompiled with JetBrains decompiler
// Type: ScrollDetailTexture
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof (Image))]
public class ScrollDetailTexture : MonoBehaviour
{
  public bool uniqueMaterial;
  public Vector2 scrollPerSecond = Vector2.zero;
  private Matrix4x4 m_Matrix;
  private Material mCopy;
  private Material mOriginal;
  private Image mSprite;
  private Material m_Mat;

  private void OnEnable()
  {
    this.mSprite = this.GetComponent<Image>();
    this.mOriginal = this.mSprite.material;
    if (!this.uniqueMaterial || !((Object) this.mSprite.material != (Object) null))
      return;
    this.mCopy = new Material(this.mOriginal);
    this.mCopy.name = "Copy of " + this.mOriginal.name;
    this.mCopy.hideFlags = HideFlags.DontSave;
    this.mSprite.material = this.mCopy;
  }

  private void OnDisable()
  {
    if ((Object) this.mCopy != (Object) null)
    {
      this.mSprite.material = this.mOriginal;
      if (Application.isEditor)
        Object.DestroyImmediate((Object) this.mCopy);
      else
        Object.Destroy((Object) this.mCopy);
      this.mCopy = (Material) null;
    }
    this.mOriginal = (Material) null;
  }

  private void Update()
  {
    Material material = (Object) this.mCopy != (Object) null ? this.mCopy : this.mOriginal;
    if (!((Object) material != (Object) null) || !((Object) material.GetTexture("_DetailTex") != (Object) null))
      return;
    material.SetTextureOffset("_DetailTex", this.scrollPerSecond * Time.time);
  }
}
