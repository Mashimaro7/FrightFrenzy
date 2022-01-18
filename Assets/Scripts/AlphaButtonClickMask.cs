// Decompiled with JetBrains decompiler
// Type: AlphaButtonClickMask
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.UI;

public class AlphaButtonClickMask : MonoBehaviour, ICanvasRaycastFilter
{
  protected Image _image;

  public void Start()
  {
    this._image = this.GetComponent<Image>();
    Texture2D texture = this._image.sprite.texture;
    bool flag = false;
    if ((UnityEngine.Object) texture != (UnityEngine.Object) null)
    {
      try
      {
        texture.GetPixels32();
      }
      catch (UnityException ex)
      {
        Debug.LogError((object) ((Exception) ex).Message);
        flag = true;
      }
    }
    else
      flag = true;
    if (!flag)
      return;
    Debug.LogError((object) "This script need an Image with a readbale Texture2D to work.");
  }

  public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
  {
    Vector2 localPoint;
    RectTransformUtility.ScreenPointToLocalPointInRectangle(this._image.rectTransform, sp, eventCamera, out localPoint);
    Vector2 pivot = this._image.rectTransform.pivot;
    Vector2 vector2_1 = new Vector2(pivot.x + localPoint.x / this._image.rectTransform.rect.width, pivot.y + localPoint.y / this._image.rectTransform.rect.height);
    Vector2 vector2_2;
    ref Vector2 local = ref vector2_2;
    Rect rect = this._image.sprite.rect;
    double x1 = (double) rect.x;
    double x2 = (double) vector2_1.x;
    rect = this._image.sprite.rect;
    double width = (double) rect.width;
    double num1 = x2 * width;
    double x3 = x1 + num1;
    rect = this._image.sprite.rect;
    double y1 = (double) rect.y;
    double y2 = (double) vector2_1.y;
    rect = this._image.sprite.rect;
    double height = (double) rect.height;
    double num2 = y2 * height;
    double y3 = y1 + num2;
    local = new Vector2((float) x3, (float) y3);
    vector2_2.x /= (float) this._image.sprite.texture.width;
    vector2_2.y /= (float) this._image.sprite.texture.height;
    return (double) this._image.sprite.texture.GetPixelBilinear(vector2_2.x, vector2_2.y).a > 0.100000001490116;
  }
}
