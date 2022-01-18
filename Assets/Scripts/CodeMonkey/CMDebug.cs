// Decompiled with JetBrains decompiler
// Type: CodeMonkey.CMDebug
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using CodeMonkey.Utils;
using System;
using UnityEngine;

namespace CodeMonkey
{
  public static class CMDebug
  {
    public static World_Sprite Button(
      Transform parent,
      Vector3 localPosition,
      string text,
      Action ClickFunc,
      int fontSize = 30,
      float paddingX = 5f,
      float paddingY = 5f)
    {
      return World_Sprite.CreateDebugButton(parent, localPosition, text, ClickFunc, fontSize, paddingX, paddingY);
    }

    public static UI_Sprite ButtonUI(
      Vector2 anchoredPosition,
      string text,
      Action ClickFunc)
    {
      return UI_Sprite.CreateDebugButton(anchoredPosition, text, ClickFunc);
    }

    public static void Text(
      string text,
      Vector3 localPosition = default (Vector3),
      Transform parent = null,
      int fontSize = 40,
      Color? color = null,
      TextAnchor textAnchor = TextAnchor.UpperLeft,
      TextAlignment textAlignment = TextAlignment.Left,
      int sortingOrder = 5000)
    {
      UtilsClass.CreateWorldText(text, parent, localPosition, fontSize, color, textAnchor, textAlignment, sortingOrder);
    }

    public static void TextPopupMouse(string text) => UtilsClass.CreateWorldTextPopup(text, UtilsClass.GetMouseWorldPosition());

    public static void TextPopup(string text, Vector3 position) => UtilsClass.CreateWorldTextPopup(text, position);

    public static FunctionUpdater TextUpdater(
      Func<string> GetTextFunc,
      Vector3 localPosition,
      Transform parent = null)
    {
      return UtilsClass.CreateWorldTextUpdater(GetTextFunc, localPosition, parent);
    }

    public static FunctionUpdater TextUpdaterUI(
      Func<string> GetTextFunc,
      Vector2 anchoredPosition)
    {
      return UtilsClass.CreateUITextUpdater(GetTextFunc, anchoredPosition);
    }

    public static void MouseTextUpdater(Func<string> GetTextFunc, Vector3 positionOffset = default (Vector3))
    {
      GameObject gameObject = new GameObject();
      FunctionUpdater.Create((Func<bool>) (() =>
      {
        gameObject.transform.position = UtilsClass.GetMouseWorldPosition() + positionOffset;
        return false;
      }));
      CMDebug.TextUpdater(GetTextFunc, Vector3.zero, gameObject.transform);
    }

    public static FunctionUpdater KeyCodeAction(KeyCode keyCode, Action onKeyDown) => UtilsClass.CreateKeyCodeAction(keyCode, onKeyDown);

    public static void DebugProjectile(
      Vector3 from,
      Vector3 to,
      float speed,
      float projectileSize)
    {
      Vector3 dir = (to - from).normalized;
      Vector3 pos = from;
      FunctionUpdater.Create((Func<bool>) (() =>
      {
        Debug.DrawLine(pos, pos + dir * projectileSize);
        double num1 = (double) Vector3.Distance(pos, to);
        pos += dir * speed * Time.deltaTime;
        double num2 = (double) Vector3.Distance(pos, to);
        return num1 < num2;
      }));
    }
  }
}
