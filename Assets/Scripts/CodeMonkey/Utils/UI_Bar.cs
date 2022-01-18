// Decompiled with JetBrains decompiler
// Type: CodeMonkey.Utils.UI_Bar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.UI;

namespace CodeMonkey.Utils
{
  public class UI_Bar
  {
    public GameObject gameObject;
    private RectTransform rectTransform;
    private RectTransform background;
    private RectTransform bar;
    private Vector2 size;

    public UI_Bar(
      Transform parent,
      Vector2 anchoredPosition,
      Vector2 size,
      Color barColor,
      float sizeRatio)
    {
      this.SetupParent(parent, anchoredPosition, size);
      this.SetupBar(barColor);
      this.SetSize(sizeRatio);
    }

    public UI_Bar(
      Transform parent,
      Vector2 anchoredPosition,
      Vector2 size,
      Color barColor,
      float sizeRatio,
      UI_Bar.Outline outline)
    {
      this.SetupParent(parent, anchoredPosition, size);
      if (outline != null)
        this.SetupOutline(outline, size);
      this.SetupBar(barColor);
      this.SetSize(sizeRatio);
    }

    public UI_Bar(
      Transform parent,
      Vector2 anchoredPosition,
      Vector2 size,
      Color backgroundColor,
      Color barColor,
      float sizeRatio)
    {
      this.SetupParent(parent, anchoredPosition, size);
      this.SetupBackground(backgroundColor);
      this.SetupBar(barColor);
      this.SetSize(sizeRatio);
    }

    public UI_Bar(
      Transform parent,
      Vector2 anchoredPosition,
      Vector2 size,
      Color backgroundColor,
      Color barColor,
      float sizeRatio,
      UI_Bar.Outline outline)
    {
      this.SetupParent(parent, anchoredPosition, size);
      if (outline != null)
        this.SetupOutline(outline, size);
      this.SetupBackground(backgroundColor);
      this.SetupBar(barColor);
      this.SetSize(sizeRatio);
    }

    private void SetupParent(Transform parent, Vector2 anchoredPosition, Vector2 size)
    {
      this.size = size;
      this.gameObject = new GameObject(nameof (UI_Bar), new System.Type[1]
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

    private RectTransform SetupOutline(UI_Bar.Outline outline, Vector2 size) => UtilsClass.DrawSprite(outline.color, this.gameObject.transform, Vector2.zero, size + new Vector2(outline.size, outline.size), "Outline");

    private void SetupBackground(Color backgroundColor)
    {
      this.background = UtilsClass.DrawSprite(backgroundColor, this.gameObject.transform, Vector2.zero, Vector2.zero, "Background");
      this.background.anchorMin = new Vector2(0.0f, 0.0f);
      this.background.anchorMax = new Vector2(1f, 1f);
    }

    private void SetupBar(Color barColor)
    {
      this.bar = UtilsClass.DrawSprite(barColor, this.gameObject.transform, Vector2.zero, Vector2.zero, "Bar");
      this.bar.anchorMin = new Vector2(0.0f, 0.0f);
      this.bar.anchorMax = new Vector2(0.0f, 1f);
      this.bar.pivot = new Vector2(0.0f, 0.5f);
    }

    public void SetSize(float sizeRatio) => this.bar.sizeDelta = new Vector2(sizeRatio * this.size.x, 0.0f);

    public void SetColor(Color color) => this.bar.GetComponent<Image>().color = color;

    public void SetActive(bool active) => this.gameObject.SetActive(active);

    public void AddOutline(UI_Bar.Outline outline) => this.SetupOutline(outline, this.size).transform.SetAsFirstSibling();

    public void SetRaycastTarget(bool set)
    {
      foreach (Transform transform in this.gameObject.transform)
      {
        if ((UnityEngine.Object) transform.GetComponent<Image>() != (UnityEngine.Object) null)
          transform.GetComponent<Image>().raycastTarget = set;
      }
    }

    public void DestroySelf() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);

    public Button_UI AddButton() => this.AddButton((Action) null, (Action) null, (Action) null);

    public Button_UI AddButton(
      Action ClickFunc,
      Action MouseOverOnceFunc,
      Action MouseOutOnceFunc)
    {
      Button_UI buttonUi = this.gameObject.AddComponent<Button_UI>();
      if (ClickFunc != null)
        buttonUi.ClickFunc = ClickFunc;
      if (MouseOverOnceFunc != null)
        buttonUi.MouseOverOnceFunc = MouseOverOnceFunc;
      if (MouseOutOnceFunc != null)
        buttonUi.MouseOutOnceFunc = MouseOutOnceFunc;
      return buttonUi;
    }

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
