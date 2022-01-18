// Decompiled with JetBrains decompiler
// Type: CodeMonkey.Utils.UI_Sprite
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.UI;

namespace CodeMonkey.Utils
{
  public class UI_Sprite
  {
    public GameObject gameObject;
    public Image image;
    public RectTransform rectTransform;

    private static Transform GetCanvasTransform() => UtilsClass.GetCanvasTransform();

    public static UI_Sprite CreateDebugButton(
      Vector2 anchoredPosition,
      Vector2 size,
      Action ClickFunc)
    {
      return UI_Sprite.CreateDebugButton(anchoredPosition, size, ClickFunc, Color.green);
    }

    public static UI_Sprite CreateDebugButton(
      Vector2 anchoredPosition,
      Vector2 size,
      Action ClickFunc,
      Color color)
    {
      UI_Sprite debugButton = new UI_Sprite(UI_Sprite.GetCanvasTransform(), Assets.i.s_White, anchoredPosition, size, color);
      debugButton.AddButton(ClickFunc, (Action) null, (Action) null);
      return debugButton;
    }

    public static UI_Sprite CreateDebugButton(
      Vector2 anchoredPosition,
      string text,
      Action ClickFunc)
    {
      return UI_Sprite.CreateDebugButton(anchoredPosition, text, ClickFunc, Color.green);
    }

    public static UI_Sprite CreateDebugButton(
      Vector2 anchoredPosition,
      string text,
      Action ClickFunc,
      Color color)
    {
      return UI_Sprite.CreateDebugButton(anchoredPosition, text, ClickFunc, color, new Vector2(30f, 20f));
    }

    public static UI_Sprite CreateDebugButton(
      Vector2 anchoredPosition,
      string text,
      Action ClickFunc,
      Color color,
      Vector2 padding)
    {
      UI_TextComplex uiTextComplex;
      UI_Sprite debugButton = UI_Sprite.CreateDebugButton(anchoredPosition, Vector2.zero, ClickFunc, color, text, out uiTextComplex);
      debugButton.SetSize(new Vector2(uiTextComplex.GetTotalWidth(), uiTextComplex.GetTotalHeight()) + padding);
      return debugButton;
    }

    public static UI_Sprite CreateDebugButton(
      Vector2 anchoredPosition,
      Vector2 size,
      Action ClickFunc,
      Color color,
      string text)
    {
      return UI_Sprite.CreateDebugButton(anchoredPosition, size, ClickFunc, color, text, out UI_TextComplex _);
    }

    public static UI_Sprite CreateDebugButton(
      Vector2 anchoredPosition,
      Vector2 size,
      Action ClickFunc,
      Color color,
      string text,
      out UI_TextComplex uiTextComplex)
    {
      if ((double) color.r >= 1.0)
        color.r = 0.9f;
      if ((double) color.g >= 1.0)
        color.g = 0.9f;
      if ((double) color.b >= 1.0)
        color.b = 0.9f;
      Color colorOver = color * 1.1f;
      UI_Sprite uiSprite = new UI_Sprite(UI_Sprite.GetCanvasTransform(), Assets.i.s_White, anchoredPosition, size, color);
      uiSprite.AddButton(ClickFunc, (Action) (() => uiSprite.SetColor(colorOver)), (Action) (() => uiSprite.SetColor(color)));
      uiTextComplex = new UI_TextComplex(uiSprite.gameObject.transform, Vector2.zero, 12, '#', text, (UI_TextComplex.Icon[]) null, (Font) null);
      uiTextComplex.SetTextColor(Color.black);
      uiTextComplex.SetAnchorMiddle();
      uiTextComplex.CenterOnPosition(Vector2.zero);
      return uiSprite;
    }

    public UI_Sprite(
      Transform parent,
      Sprite sprite,
      Vector2 anchoredPosition,
      Vector2 size,
      Color color)
    {
      this.rectTransform = UtilsClass.DrawSprite(sprite, parent, anchoredPosition, size, nameof (UI_Sprite));
      this.gameObject = this.rectTransform.gameObject;
      this.image = this.gameObject.GetComponent<Image>();
      this.image.color = color;
    }

    public void SetColor(Color color) => this.image.color = color;

    public void SetSprite(Sprite sprite) => this.image.sprite = sprite;

    public void SetSize(Vector2 size) => this.rectTransform.sizeDelta = size;

    public void SetAnchoredPosition(Vector2 anchoredPosition) => this.rectTransform.anchoredPosition = anchoredPosition;

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

    public void DestroySelf() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }
}
