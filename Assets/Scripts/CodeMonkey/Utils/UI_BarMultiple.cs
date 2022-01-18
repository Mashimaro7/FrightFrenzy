// Decompiled with JetBrains decompiler
// Type: CodeMonkey.Utils.UI_BarMultiple
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CodeMonkey.Utils
{
  public class UI_BarMultiple
  {
    private GameObject gameObject;
    private RectTransform rectTransform;
    private RectTransform[] barArr;
    private Image[] barImageArr;
    private Vector2 size;

    public UI_BarMultiple(
      Transform parent,
      Vector2 anchoredPosition,
      Vector2 size,
      Color[] barColorArr,
      UI_BarMultiple.Outline outline)
    {
      this.size = size;
      this.SetupParent(parent, anchoredPosition, size);
      if (outline != null)
        this.SetupOutline(outline, size);
      List<RectTransform> rectTransformList = new List<RectTransform>();
      List<Image> imageList = new List<Image>();
      List<float> floatList = new List<float>();
      foreach (Color barColor in barColorArr)
      {
        rectTransformList.Add(this.SetupBar(barColor));
        floatList.Add(1f / (float) barColorArr.Length);
      }
      this.barArr = rectTransformList.ToArray();
      this.barImageArr = imageList.ToArray();
      this.SetSizes(floatList.ToArray());
    }

    private void SetupParent(Transform parent, Vector2 anchoredPosition, Vector2 size)
    {
      this.gameObject = new GameObject(nameof (UI_BarMultiple), new System.Type[1]
      {
        typeof (RectTransform)
      });
      this.rectTransform = this.gameObject.GetComponent<RectTransform>();
      this.rectTransform.SetParent(parent, false);
      this.rectTransform.sizeDelta = size;
      this.rectTransform.anchorMin = new Vector2(0.0f, 0.5f);
      this.rectTransform.anchorMax = new Vector2(0.0f, 0.5f);
      this.rectTransform.pivot = new Vector2(0.0f, 0.5f);
      this.rectTransform.anchoredPosition = anchoredPosition;
    }

    private void SetupOutline(UI_BarMultiple.Outline outline, Vector2 size) => UtilsClass.DrawSprite(outline.color, this.gameObject.transform, Vector2.zero, size + new Vector2(outline.size, outline.size), "Outline");

    private RectTransform SetupBar(Color barColor)
    {
      RectTransform rectTransform = UtilsClass.DrawSprite(barColor, this.gameObject.transform, Vector2.zero, Vector2.zero, "Bar");
      rectTransform.anchorMin = new Vector2(0.0f, 0.0f);
      rectTransform.anchorMax = new Vector2(0.0f, 1f);
      rectTransform.pivot = new Vector2(0.0f, 0.5f);
      return rectTransform;
    }

    public void SetSizes(float[] sizeArr)
    {
      if (sizeArr.Length != this.barArr.Length)
        throw new Exception("Length doesn't match!");
      Vector2 zero = Vector2.zero;
      for (int index = 0; index < sizeArr.Length; ++index)
      {
        float x = sizeArr[index] * this.size.x;
        this.barArr[index].anchoredPosition = zero;
        this.barArr[index].sizeDelta = new Vector2(x, 0.0f);
        zero.x += x;
      }
    }

    public Vector2 GetSize() => this.size;

    public void DestroySelf() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);

    public class Outline
    {
      public float size = 1f;
      public Color color = Color.black;

      public Outline(float size, Color color)
      {
        this.size = size;
        this.color = color;
      }
    }
  }
}
