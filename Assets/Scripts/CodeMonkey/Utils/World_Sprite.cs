// Decompiled with JetBrains decompiler
// Type: CodeMonkey.Utils.World_Sprite
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

namespace CodeMonkey.Utils
{
  public class World_Sprite
  {
    private const int sortingOrderDefault = 5000;
    public GameObject gameObject;
    public Transform transform;
    private SpriteRenderer spriteRenderer;

    public static World_Sprite CreateDebugButton(Vector3 position, Action ClickFunc)
    {
      World_Sprite debugButton = new World_Sprite((Transform) null, position, new Vector3(10f, 10f), Assets.i.s_White, Color.green, 5000);
      debugButton.AddButton(ClickFunc, (Action) null, (Action) null);
      return debugButton;
    }

    public static World_Sprite CreateDebugButton(
      Transform parent,
      Vector3 localPosition,
      Action ClickFunc)
    {
      World_Sprite debugButton = new World_Sprite(parent, localPosition, new Vector3(10f, 10f), Assets.i.s_White, Color.green, 5000);
      debugButton.AddButton(ClickFunc, (Action) null, (Action) null);
      return debugButton;
    }

    public static World_Sprite CreateDebugButton(
      Transform parent,
      Vector3 localPosition,
      string text,
      Action ClickFunc,
      int fontSize = 30,
      float paddingX = 5f,
      float paddingY = 5f)
    {
      GameObject gameObject = new GameObject("DebugButton");
      gameObject.transform.parent = parent;
      gameObject.transform.localPosition = localPosition;
      Bounds bounds = UtilsClass.CreateWorldText(text, gameObject.transform, Vector3.zero, fontSize, new Color?(Color.white), TextAnchor.MiddleCenter, TextAlignment.Center, 20000).GetComponent<MeshRenderer>().bounds;
      Color color = UtilsClass.GetColorFromString("00BA00FF");
      if ((double) color.r >= 1.0)
        color.r = 0.9f;
      if ((double) color.g >= 1.0)
        color.g = 0.9f;
      if ((double) color.b >= 1.0)
        color.b = 0.9f;
      Color colorOver = color * 1.1f;
      World_Sprite worldSprite = new World_Sprite(gameObject.transform, Vector3.zero, bounds.size + new Vector3(paddingX, paddingY), Assets.i.s_White, color, 5000);
      worldSprite.AddButton(ClickFunc, (Action) (() => worldSprite.SetColor(colorOver)), (Action) (() => worldSprite.SetColor(color)));
      return worldSprite;
    }

    public static World_Sprite Create(
      Transform parent,
      Vector3 localPosition,
      Vector3 localScale,
      Sprite sprite,
      Color color,
      int sortingOrderOffset)
    {
      return new World_Sprite(parent, localPosition, localScale, sprite, color, sortingOrderOffset);
    }

    public static World_Sprite Create(Vector3 worldPosition, Sprite sprite) => new World_Sprite((Transform) null, worldPosition, new Vector3(1f, 1f, 1f), sprite, Color.white, 0);

    public static World_Sprite Create(
      Vector3 worldPosition,
      Vector3 localScale,
      Sprite sprite,
      Color color,
      int sortingOrderOffset)
    {
      return new World_Sprite((Transform) null, worldPosition, localScale, sprite, color, sortingOrderOffset);
    }

    public static World_Sprite Create(
      Vector3 worldPosition,
      Vector3 localScale,
      Sprite sprite,
      Color color)
    {
      return new World_Sprite((Transform) null, worldPosition, localScale, sprite, color, 0);
    }

    public static World_Sprite Create(
      Vector3 worldPosition,
      Vector3 localScale,
      Color color)
    {
      return new World_Sprite((Transform) null, worldPosition, localScale, Assets.i.s_White, color, 0);
    }

    public static World_Sprite Create(Vector3 worldPosition, Vector3 localScale) => new World_Sprite((Transform) null, worldPosition, localScale, Assets.i.s_White, Color.white, 0);

    public static World_Sprite Create(
      Vector3 worldPosition,
      Vector3 localScale,
      int sortingOrderOffset)
    {
      return new World_Sprite((Transform) null, worldPosition, localScale, Assets.i.s_White, Color.white, sortingOrderOffset);
    }

    public static int GetSortingOrder(Vector3 position, int offset, int baseSortingOrder = 5000) => (int) ((double) baseSortingOrder - (double) position.y) + offset;

    public World_Sprite(
      Transform parent,
      Vector3 localPosition,
      Vector3 localScale,
      Sprite sprite,
      Color color,
      int sortingOrderOffset)
    {
      int sortingOrder = World_Sprite.GetSortingOrder(localPosition, sortingOrderOffset);
      this.gameObject = UtilsClass.CreateWorldSprite(parent, "Sprite", sprite, localPosition, localScale, sortingOrder, color);
      this.transform = this.gameObject.transform;
      this.spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
    }

    public void SetSortingOrderOffset(int sortingOrderOffset) => this.SetSortingOrder(World_Sprite.GetSortingOrder(this.gameObject.transform.position, sortingOrderOffset));

    public void SetSortingOrder(int sortingOrder) => this.gameObject.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;

    public void SetLocalScale(Vector3 localScale) => this.transform.localScale = localScale;

    public void SetPosition(Vector3 localPosition) => this.transform.localPosition = localPosition;

    public void SetColor(Color color) => this.spriteRenderer.color = color;

    public void SetSprite(Sprite sprite) => this.spriteRenderer.sprite = sprite;

    public void Show() => this.gameObject.SetActive(true);

    public void Hide() => this.gameObject.SetActive(false);

    public Button_Sprite AddButton(
      Action ClickFunc,
      Action MouseOverOnceFunc,
      Action MouseOutOnceFunc)
    {
      this.gameObject.AddComponent<BoxCollider2D>();
      Button_Sprite buttonSprite = this.gameObject.AddComponent<Button_Sprite>();
      if (ClickFunc != null)
        buttonSprite.ClickFunc = ClickFunc;
      if (MouseOverOnceFunc != null)
        buttonSprite.MouseOverOnceFunc = MouseOverOnceFunc;
      if (MouseOutOnceFunc != null)
        buttonSprite.MouseOutOnceFunc = MouseOutOnceFunc;
      return buttonSprite;
    }

    public void DestroySelf() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }
}
