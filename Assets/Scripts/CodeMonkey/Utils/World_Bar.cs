// Decompiled with JetBrains decompiler
// Type: CodeMonkey.Utils.World_Bar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

namespace CodeMonkey.Utils
{
  public class World_Bar
  {
    private GameObject gameObject;
    private Transform transform;
    private Transform background;
    private Transform bar;

    public static int GetSortingOrder(Vector3 position, int offset, int baseSortingOrder = 5000) => (int) ((double) baseSortingOrder - (double) position.y) + offset;

    public World_Bar(
      Transform parent,
      Vector3 localPosition,
      Vector3 localScale,
      Color? backgroundColor,
      Color barColor,
      float sizeRatio,
      int sortingOrder,
      World_Bar.Outline outline = null)
    {
      this.SetupParent(parent, localPosition);
      if (outline != null)
        this.SetupOutline(outline, localScale, sortingOrder - 1);
      if (backgroundColor.HasValue)
        this.SetupBackground(backgroundColor.Value, localScale, sortingOrder);
      this.SetupBar(barColor, localScale, sortingOrder + 1);
      this.SetSize(sizeRatio);
    }

    private void SetupParent(Transform parent, Vector3 localPosition)
    {
      this.gameObject = new GameObject(nameof (World_Bar));
      this.transform = this.gameObject.transform;
      this.transform.SetParent(parent);
      this.transform.localPosition = localPosition;
    }

    private void SetupOutline(World_Bar.Outline outline, Vector3 localScale, int sortingOrder) => UtilsClass.CreateWorldSprite(this.transform, "Outline", Assets.i.s_White, new Vector3(0.0f, 0.0f), localScale + new Vector3(outline.size, outline.size), sortingOrder, outline.color);

    private void SetupBackground(Color backgroundColor, Vector3 localScale, int sortingOrder) => this.background = UtilsClass.CreateWorldSprite(this.transform, "Background", Assets.i.s_White, new Vector3(0.0f, 0.0f), localScale, sortingOrder, backgroundColor).transform;

    private void SetupBar(Color barColor, Vector3 localScale, int sortingOrder)
    {
      this.bar = new GameObject("Bar").transform;
      this.bar.SetParent(this.transform);
      this.bar.localPosition = new Vector3((float) (-(double) localScale.x / 2.0), 0.0f, 0.0f);
      this.bar.localScale = new Vector3(1f, 1f, 1f);
      Transform transform = UtilsClass.CreateWorldSprite(this.bar, "BarIn", Assets.i.s_White, new Vector3(localScale.x / 2f, 0.0f), localScale, sortingOrder, barColor).transform;
    }

    public void SetRotation(float rotation) => this.transform.localEulerAngles = new Vector3(0.0f, 0.0f, rotation);

    public void SetSize(float sizeRatio) => this.bar.localScale = new Vector3(sizeRatio, 1f, 1f);

    public void SetColor(Color color) => this.bar.Find("BarIn").GetComponent<SpriteRenderer>().color = color;

    public void Show() => this.gameObject.SetActive(true);

    public void Hide() => this.gameObject.SetActive(false);

    public Button_Sprite AddButton(
      Action ClickFunc,
      Action MouseOverOnceFunc,
      Action MouseOutOnceFunc)
    {
      Button_Sprite buttonSprite = this.gameObject.AddComponent<Button_Sprite>();
      if (ClickFunc != null)
        buttonSprite.ClickFunc = ClickFunc;
      if (MouseOverOnceFunc != null)
        buttonSprite.MouseOverOnceFunc = MouseOverOnceFunc;
      if (MouseOutOnceFunc != null)
        buttonSprite.MouseOutOnceFunc = MouseOutOnceFunc;
      return buttonSprite;
    }

    public void DestroySelf()
    {
      if (!((UnityEngine.Object) this.gameObject != (UnityEngine.Object) null))
        return;
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    }

    public class Outline
    {
      public float size = 1f;
      public Color color = Color.black;
    }
  }
}
