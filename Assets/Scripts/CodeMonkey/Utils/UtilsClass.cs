// Decompiled with JetBrains decompiler
// Type: CodeMonkey.Utils.UtilsClass
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CodeMonkey.Utils
{
  public static class UtilsClass
  {
    private static readonly Vector3 Vector3zero = Vector3.zero;
    private static readonly Vector3 Vector3one = Vector3.one;
    private static readonly Vector3 Vector3yDown = new Vector3(0.0f, -1f);
    public const int sortingOrderDefault = 5000;
    private static Transform cachedCanvasTransform;

    public static int GetSortingOrder(Vector3 position, int offset, int baseSortingOrder = 5000) => (int) ((double) baseSortingOrder - (double) position.y) + offset;

    public static Transform GetCanvasTransform()
    {
      if ((UnityEngine.Object) UtilsClass.cachedCanvasTransform == (UnityEngine.Object) null)
      {
        Canvas objectOfType = UnityEngine.Object.FindObjectOfType<Canvas>();
        if ((UnityEngine.Object) objectOfType != (UnityEngine.Object) null)
          UtilsClass.cachedCanvasTransform = objectOfType.transform;
      }
      return UtilsClass.cachedCanvasTransform;
    }

    public static Font GetDefaultFont() => UnityEngine.Resources.GetBuiltinResource<Font>("Arial.ttf");

    public static GameObject CreateWorldSprite(
      string name,
      Sprite sprite,
      Vector3 position,
      Vector3 localScale,
      int sortingOrder,
      Color color)
    {
      return UtilsClass.CreateWorldSprite((Transform) null, name, sprite, position, localScale, sortingOrder, color);
    }

    public static GameObject CreateWorldSprite(
      Transform parent,
      string name,
      Sprite sprite,
      Vector3 localPosition,
      Vector3 localScale,
      int sortingOrder,
      Color color)
    {
      GameObject worldSprite = new GameObject(name, new System.Type[1]
      {
        typeof (SpriteRenderer)
      });
      Transform transform = worldSprite.transform;
      transform.SetParent(parent, false);
      transform.localPosition = localPosition;
      transform.localScale = localScale;
      SpriteRenderer component = worldSprite.GetComponent<SpriteRenderer>();
      component.sprite = sprite;
      component.sortingOrder = sortingOrder;
      component.color = color;
      return worldSprite;
    }

    public static Button_Sprite CreateWorldSpriteButton(
      string name,
      Sprite sprite,
      Vector3 localPosition,
      Vector3 localScale,
      int sortingOrder,
      Color color)
    {
      return UtilsClass.CreateWorldSpriteButton((Transform) null, name, sprite, localPosition, localScale, sortingOrder, color);
    }

    public static Button_Sprite CreateWorldSpriteButton(
      Transform parent,
      string name,
      Sprite sprite,
      Vector3 localPosition,
      Vector3 localScale,
      int sortingOrder,
      Color color)
    {
      GameObject worldSprite = UtilsClass.CreateWorldSprite(parent, name, sprite, localPosition, localScale, sortingOrder, color);
      worldSprite.AddComponent<BoxCollider2D>();
      return worldSprite.AddComponent<Button_Sprite>();
    }

    public static FunctionUpdater CreateWorldTextUpdater(
      Func<string> GetTextFunc,
      Vector3 localPosition,
      Transform parent = null)
    {
      TextMesh textMesh = UtilsClass.CreateWorldText(GetTextFunc(), parent, localPosition);
      return FunctionUpdater.Create((Func<bool>) (() =>
      {
        textMesh.text = GetTextFunc();
        return false;
      }), "WorldTextUpdater");
    }

    public static TextMesh CreateWorldText(
      string text,
      Transform parent = null,
      Vector3 localPosition = default (Vector3),
      int fontSize = 40,
      Color? color = null,
      TextAnchor textAnchor = TextAnchor.UpperLeft,
      TextAlignment textAlignment = TextAlignment.Left,
      int sortingOrder = 5000)
    {
      if (!color.HasValue)
        color = new Color?(Color.white);
      return UtilsClass.CreateWorldText(parent, text, localPosition, fontSize, color.Value, textAnchor, textAlignment, sortingOrder);
    }

    public static TextMesh CreateWorldText(
      Transform parent,
      string text,
      Vector3 localPosition,
      int fontSize,
      Color color,
      TextAnchor textAnchor,
      TextAlignment textAlignment,
      int sortingOrder)
    {
      GameObject gameObject = new GameObject("World_Text", new System.Type[1]
      {
        typeof (TextMesh)
      });
      Transform transform = gameObject.transform;
      transform.SetParent(parent, false);
      transform.localPosition = localPosition;
      TextMesh component = gameObject.GetComponent<TextMesh>();
      component.anchor = textAnchor;
      component.alignment = textAlignment;
      component.text = text;
      component.fontSize = fontSize;
      component.color = color;
      component.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
      return component;
    }

    public static void CreateWorldTextPopup(string text, Vector3 localPosition) => UtilsClass.CreateWorldTextPopup((Transform) null, text, localPosition, 40, Color.white, localPosition + new Vector3(0.0f, 20f), 1f);

    public static void CreateWorldTextPopup(
      Transform parent,
      string text,
      Vector3 localPosition,
      int fontSize,
      Color color,
      Vector3 finalPopupPosition,
      float popupTime)
    {
      Transform transform = UtilsClass.CreateWorldText(parent, text, localPosition, fontSize, color, TextAnchor.LowerLeft, TextAlignment.Left, 5000).transform;
      Vector3 moveAmount = (finalPopupPosition - localPosition) / popupTime;
      FunctionUpdater.Create((Func<bool>) (() =>
      {
        transform.position += moveAmount * Time.deltaTime;
        popupTime -= Time.deltaTime;
        if ((double) popupTime > 0.0)
          return false;
        UnityEngine.Object.Destroy((UnityEngine.Object) transform.gameObject);
        return true;
      }), "WorldTextPopup");
    }

    public static FunctionUpdater CreateUITextUpdater(
      Func<string> GetTextFunc,
      Vector2 anchoredPosition)
    {
      Text text = UtilsClass.DrawTextUI(GetTextFunc(), anchoredPosition, 20, UtilsClass.GetDefaultFont());
      return FunctionUpdater.Create((Func<bool>) (() =>
      {
        text.text = GetTextFunc();
        return false;
      }), "UITextUpdater");
    }

    public static RectTransform DrawSprite(
      Color color,
      Transform parent,
      Vector2 pos,
      Vector2 size,
      string name = null)
    {
      return UtilsClass.DrawSprite((Sprite) null, color, parent, pos, size, name);
    }

    public static RectTransform DrawSprite(
      Sprite sprite,
      Transform parent,
      Vector2 pos,
      Vector2 size,
      string name = null)
    {
      return UtilsClass.DrawSprite(sprite, Color.white, parent, pos, size, name);
    }

    public static RectTransform DrawSprite(
      Sprite sprite,
      Color color,
      Transform parent,
      Vector2 pos,
      Vector2 size,
      string name = null)
    {
      switch (name)
      {
        case "":
        case null:
          name = "Sprite";
          break;
      }
      GameObject gameObject = new GameObject(name, new System.Type[2]
      {
        typeof (RectTransform),
        typeof (Image)
      });
      RectTransform component1 = gameObject.GetComponent<RectTransform>();
      component1.SetParent(parent, false);
      component1.sizeDelta = size;
      component1.anchoredPosition = pos;
      Image component2 = gameObject.GetComponent<Image>();
      component2.sprite = sprite;
      component2.color = color;
      return component1;
    }

    public static Text DrawTextUI(
      string textString,
      Vector2 anchoredPosition,
      int fontSize,
      Font font)
    {
      return UtilsClass.DrawTextUI(textString, UtilsClass.GetCanvasTransform(), anchoredPosition, fontSize, font);
    }

    public static Text DrawTextUI(
      string textString,
      Transform parent,
      Vector2 anchoredPosition,
      int fontSize,
      Font font)
    {
      GameObject gameObject = new GameObject("Text", new System.Type[2]
      {
        typeof (RectTransform),
        typeof (Text)
      });
      gameObject.transform.SetParent(parent, false);
      Transform transform = gameObject.transform;
      transform.SetParent(parent, false);
      transform.localPosition = UtilsClass.Vector3zero;
      transform.localScale = UtilsClass.Vector3one;
      RectTransform component1 = gameObject.GetComponent<RectTransform>();
      component1.sizeDelta = new Vector2(0.0f, 0.0f);
      component1.anchoredPosition = anchoredPosition;
      Text component2 = gameObject.GetComponent<Text>();
      component2.text = textString;
      component2.verticalOverflow = VerticalWrapMode.Overflow;
      component2.horizontalOverflow = HorizontalWrapMode.Overflow;
      component2.alignment = TextAnchor.MiddleLeft;
      if ((UnityEngine.Object) font == (UnityEngine.Object) null)
        font = UtilsClass.GetDefaultFont();
      component2.font = font;
      component2.fontSize = fontSize;
      return component2;
    }

    public static float Parse_Float(string txt, float _default)
    {
      float result;
      if (!float.TryParse(txt, out result))
        result = _default;
      return result;
    }

    public static int Parse_Int(string txt, int _default)
    {
      int result;
      if (!int.TryParse(txt, out result))
        result = _default;
      return result;
    }

    public static int Parse_Int(string txt) => UtilsClass.Parse_Int(txt, -1);

    public static Vector3 GetMouseWorldPosition() => UtilsClass.GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main) with
    {
      z = 0.0f
    };

    public static Vector3 GetMouseWorldPositionWithZ() => UtilsClass.GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);

    public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera) => UtilsClass.GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);

    public static Vector3 GetMouseWorldPositionWithZ(
      Vector3 screenPosition,
      Camera worldCamera)
    {
      return worldCamera.ScreenToWorldPoint(screenPosition);
    }

    public static bool IsPointerOverUI()
    {
      if (EventSystem.current.IsPointerOverGameObject())
        return true;
      PointerEventData eventData = new PointerEventData(EventSystem.current);
      eventData.position = (Vector2) Input.mousePosition;
      List<RaycastResult> raycastResults = new List<RaycastResult>();
      EventSystem.current.RaycastAll(eventData, raycastResults);
      return raycastResults.Count > 0;
    }

    public static string Dec_to_Hex(int value) => value.ToString("X2");

    public static int Hex_to_Dec(string hex) => Convert.ToInt32(hex, 16);

    public static string Dec01_to_Hex(float value) => UtilsClass.Dec_to_Hex((int) Mathf.Round(value * (float) byte.MaxValue));

    public static float Hex_to_Dec01(string hex) => (float) UtilsClass.Hex_to_Dec(hex) / (float) byte.MaxValue;

    public static string GetStringFromColor(Color color)
    {
      string hex1 = UtilsClass.Dec01_to_Hex(color.r);
      string hex2 = UtilsClass.Dec01_to_Hex(color.g);
      string hex3 = UtilsClass.Dec01_to_Hex(color.b);
      string str1 = hex2;
      string str2 = hex3;
      return hex1 + str1 + str2;
    }

    public static string GetStringFromColorWithAlpha(Color color)
    {
      string hex = UtilsClass.Dec01_to_Hex(color.a);
      return UtilsClass.GetStringFromColor(color) + hex;
    }

    public static void GetStringFromColor(
      Color color,
      out string red,
      out string green,
      out string blue,
      out string alpha)
    {
      red = UtilsClass.Dec01_to_Hex(color.r);
      green = UtilsClass.Dec01_to_Hex(color.g);
      blue = UtilsClass.Dec01_to_Hex(color.b);
      alpha = UtilsClass.Dec01_to_Hex(color.a);
    }

    public static string GetStringFromColor(float r, float g, float b)
    {
      string hex1 = UtilsClass.Dec01_to_Hex(r);
      string hex2 = UtilsClass.Dec01_to_Hex(g);
      string hex3 = UtilsClass.Dec01_to_Hex(b);
      string str1 = hex2;
      string str2 = hex3;
      return hex1 + str1 + str2;
    }

    public static string GetStringFromColor(float r, float g, float b, float a)
    {
      string hex = UtilsClass.Dec01_to_Hex(a);
      return UtilsClass.GetStringFromColor(r, g, b) + hex;
    }

    public static Color GetColorFromString(string color)
    {
      double dec01_1 = (double) UtilsClass.Hex_to_Dec01(color.Substring(0, 2));
      float dec01_2 = UtilsClass.Hex_to_Dec01(color.Substring(2, 2));
      float dec01_3 = UtilsClass.Hex_to_Dec01(color.Substring(4, 2));
      float num = 1f;
      if (color.Length >= 8)
        num = UtilsClass.Hex_to_Dec01(color.Substring(6, 2));
      double g = (double) dec01_2;
      double b = (double) dec01_3;
      double a = (double) num;
      return new Color((float) dec01_1, (float) g, (float) b, (float) a);
    }

    public static Vector3 GetRandomDir() => new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;

    public static Vector3 GetVectorFromAngle(int angle)
    {
      float f = (float) angle * ((float) Math.PI / 180f);
      return new Vector3(Mathf.Cos(f), Mathf.Sin(f));
    }

    public static float GetAngleFromVectorFloat(Vector3 dir)
    {
      dir = dir.normalized;
      float angleFromVectorFloat = Mathf.Atan2(dir.y, dir.x) * 57.29578f;
      if ((double) angleFromVectorFloat < 0.0)
        angleFromVectorFloat += 360f;
      return angleFromVectorFloat;
    }

    public static int GetAngleFromVector(Vector3 dir)
    {
      dir = dir.normalized;
      float f = Mathf.Atan2(dir.y, dir.x) * 57.29578f;
      if ((double) f < 0.0)
        f += 360f;
      return Mathf.RoundToInt(f);
    }

    public static int GetAngleFromVector180(Vector3 dir)
    {
      dir = dir.normalized;
      return Mathf.RoundToInt(Mathf.Atan2(dir.y, dir.x) * 57.29578f);
    }

    public static Vector3 ApplyRotationToVector(Vector3 vec, Vector3 vecRotation) => UtilsClass.ApplyRotationToVector(vec, UtilsClass.GetAngleFromVectorFloat(vecRotation));

    public static Vector3 ApplyRotationToVector(Vector3 vec, float angle) => Quaternion.Euler(0.0f, 0.0f, angle) * vec;

    public static FunctionUpdater CreateMouseDraggingAction(
      Action<Vector3> onMouseDragging)
    {
      return UtilsClass.CreateMouseDraggingAction(0, onMouseDragging);
    }

    public static FunctionUpdater CreateMouseDraggingAction(
      int mouseButton,
      Action<Vector3> onMouseDragging)
    {
      bool dragging = false;
      return FunctionUpdater.Create((Func<bool>) (() =>
      {
        if (Input.GetMouseButtonDown(mouseButton))
          dragging = true;
        if (Input.GetMouseButtonUp(mouseButton))
          dragging = false;
        if (dragging)
          onMouseDragging(UtilsClass.GetMouseWorldPosition());
        return false;
      }));
    }

    public static FunctionUpdater CreateMouseClickFromToAction(
      Action<Vector3, Vector3> onMouseClickFromTo,
      Action<Vector3, Vector3> onWaitingForToPosition)
    {
      return UtilsClass.CreateMouseClickFromToAction(0, 1, onMouseClickFromTo, onWaitingForToPosition);
    }

    public static FunctionUpdater CreateMouseClickFromToAction(
      int mouseButton,
      int cancelMouseButton,
      Action<Vector3, Vector3> onMouseClickFromTo,
      Action<Vector3, Vector3> onWaitingForToPosition)
    {
      int state = 0;
      Vector3 from = Vector3.zero;
      return FunctionUpdater.Create((Func<bool>) (() =>
      {
        if (state == 1 && onWaitingForToPosition != null)
          onWaitingForToPosition(from, UtilsClass.GetMouseWorldPosition());
        if (state == 1 && Input.GetMouseButtonDown(cancelMouseButton))
          state = 0;
        if (Input.GetMouseButtonDown(mouseButton) && !UtilsClass.IsPointerOverUI())
        {
          if (state == 0)
          {
            state = 1;
            from = UtilsClass.GetMouseWorldPosition();
          }
          else
          {
            state = 0;
            onMouseClickFromTo(from, UtilsClass.GetMouseWorldPosition());
          }
        }
        return false;
      }));
    }

    public static FunctionUpdater CreateMouseClickAction(Action<Vector3> onMouseClick) => UtilsClass.CreateMouseClickAction(0, onMouseClick);

    public static FunctionUpdater CreateMouseClickAction(
      int mouseButton,
      Action<Vector3> onMouseClick)
    {
      return FunctionUpdater.Create((Func<bool>) (() =>
      {
        if (Input.GetMouseButtonDown(mouseButton))
          onMouseClick(UtilsClass.GetWorldPositionFromUI());
        return false;
      }));
    }

    public static FunctionUpdater CreateKeyCodeAction(
      KeyCode keyCode,
      Action onKeyDown)
    {
      return FunctionUpdater.Create((Func<bool>) (() =>
      {
        if (Input.GetKeyDown(keyCode))
          onKeyDown();
        return false;
      }));
    }

    public static Vector2 GetWorldUIPosition(
      Vector3 worldPosition,
      Transform parent,
      Camera uiCamera,
      Camera worldCamera)
    {
      Vector3 screenPoint = worldCamera.WorldToScreenPoint(worldPosition);
      Vector3 worldPoint = uiCamera.ScreenToWorldPoint(screenPoint);
      Vector3 vector3 = parent.InverseTransformPoint(worldPoint);
      return new Vector2(vector3.x, vector3.y);
    }

    public static Vector3 GetWorldPositionFromUIZeroZ() => UtilsClass.GetWorldPositionFromUI(Input.mousePosition, Camera.main) with
    {
      z = 0.0f
    };

    public static Vector3 GetWorldPositionFromUI() => UtilsClass.GetWorldPositionFromUI(Input.mousePosition, Camera.main);

    public static Vector3 GetWorldPositionFromUI(Camera worldCamera) => UtilsClass.GetWorldPositionFromUI(Input.mousePosition, worldCamera);

    public static Vector3 GetWorldPositionFromUI(Vector3 screenPosition, Camera worldCamera) => worldCamera.ScreenToWorldPoint(screenPosition);

    public static Vector3 GetWorldPositionFromUI_Perspective() => UtilsClass.GetWorldPositionFromUI_Perspective(Input.mousePosition, Camera.main);

    public static Vector3 GetWorldPositionFromUI_Perspective(Camera worldCamera) => UtilsClass.GetWorldPositionFromUI_Perspective(Input.mousePosition, worldCamera);

    public static Vector3 GetWorldPositionFromUI_Perspective(
      Vector3 screenPosition,
      Camera worldCamera)
    {
      Ray ray = worldCamera.ScreenPointToRay(screenPosition);
      float enter;
      new Plane(Vector3.forward, new Vector3(0.0f, 0.0f, 0.0f)).Raycast(ray, out enter);
      return ray.GetPoint(enter);
    }

    public static void ShakeCamera(float intensity, float timer)
    {
      Vector3 lastCameraMovement = Vector3.zero;
      FunctionUpdater.Create((Func<bool>) (() =>
      {
        timer -= Time.unscaledDeltaTime;
        Vector3 vector3 = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized * intensity;
        Camera.main.transform.position = Camera.main.transform.position - lastCameraMovement + vector3;
        lastCameraMovement = vector3;
        return (double) timer <= 0.0;
      }), "CAMERA_SHAKE");
    }
  }
}
