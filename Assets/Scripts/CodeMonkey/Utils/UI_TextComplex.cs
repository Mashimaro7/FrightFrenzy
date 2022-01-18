// Decompiled with JetBrains decompiler
// Type: CodeMonkey.Utils.UI_TextComplex
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

namespace CodeMonkey.Utils
{
  public class UI_TextComplex
  {
    public GameObject gameObject;
    private Transform transform;
    private RectTransform rectTransform;

    private static Transform GetCanvasTransform() => UtilsClass.GetCanvasTransform();

    public UI_TextComplex(
      Transform parent,
      Vector2 anchoredPosition,
      int fontSize,
      char iconChar,
      string text,
      UI_TextComplex.Icon[] iconArr,
      Font font)
    {
      this.SetupParent(parent, anchoredPosition);
      string textString1 = text;
      float x = 0.0f;
      while (textString1.IndexOf(iconChar) != -1)
      {
        string textString2 = textString1.Substring(0, textString1.IndexOf(iconChar));
        string txt = textString1.Substring(textString1.IndexOf(iconChar) + 1);
        int length = txt.IndexOf(" ");
        if (length != -1)
          txt = txt.Substring(0, length);
        textString1 = textString1.Substring(textString1.IndexOf(iconChar.ToString() + txt) + (iconChar.ToString() + txt).Length);
        if (textString2.Trim() != "")
        {
          Text text1 = UtilsClass.DrawTextUI(textString2, this.transform, new Vector2(x, 0.0f), fontSize, font);
          x += text1.preferredWidth;
        }
        int index = UtilsClass.Parse_Int(txt, 0);
        UI_TextComplex.Icon icon = iconArr[index];
        UtilsClass.DrawSprite(icon.sprite, this.transform, new Vector2(x + icon.size.x / 2f, 0.0f), icon.size);
        x += icon.size.x;
      }
      if (!(textString1.Trim() != ""))
        return;
      UtilsClass.DrawTextUI(textString1, this.transform, new Vector2(x, 0.0f), fontSize, font);
    }

    private void SetupParent(Transform parent, Vector2 anchoredPosition)
    {
      this.gameObject = new GameObject(nameof (UI_TextComplex), new System.Type[1]
      {
        typeof (RectTransform)
      });
      this.transform = this.gameObject.transform;
      this.rectTransform = this.gameObject.GetComponent<RectTransform>();
      this.rectTransform.SetParent(parent, false);
      this.rectTransform.sizeDelta = new Vector2(0.0f, 0.0f);
      this.rectTransform.anchorMin = new Vector2(0.0f, 0.5f);
      this.rectTransform.anchorMax = new Vector2(0.0f, 0.5f);
      this.rectTransform.pivot = new Vector2(0.0f, 0.5f);
      this.rectTransform.anchoredPosition = anchoredPosition;
    }

    public void SetTextColor(Color color)
    {
      foreach (Component component1 in this.transform)
      {
        Text component2 = component1.GetComponent<Text>();
        if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
          component2.color = color;
      }
    }

    public float GetTotalWidth()
    {
      float totalWidth = 0.0f;
      foreach (Transform transform in this.transform)
      {
        Text component1 = transform.GetComponent<Text>();
        if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
          totalWidth += component1.preferredWidth;
        Image component2 = transform.GetComponent<Image>();
        if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
          totalWidth += component2.GetComponent<RectTransform>().sizeDelta.x;
      }
      return totalWidth;
    }

    public float GetTotalHeight()
    {
      foreach (Component component1 in this.transform)
      {
        Text component2 = component1.GetComponent<Text>();
        if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
          return component2.preferredHeight;
      }
      return 0.0f;
    }

    public void AddTextOutline(Color color, float size)
    {
      foreach (Transform transform in this.transform)
      {
        if ((UnityEngine.Object) transform.GetComponent<Text>() != (UnityEngine.Object) null)
        {
          UnityEngine.UI.Outline outline = transform.gameObject.AddComponent<UnityEngine.UI.Outline>();
          outline.effectColor = color;
          outline.effectDistance = new Vector2(size, size);
        }
      }
    }

    public void SetAnchorMiddle()
    {
      this.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
      this.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
    }

    public void CenterOnPosition(Vector2 position) => this.rectTransform.anchoredPosition = position + new Vector2((float) (-(double) this.GetTotalWidth() / 2.0), 0.0f);

    public void DestroySelf() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);

    public struct Icon
    {
      public Sprite sprite;
      public Vector2 size;
      public Color color;

      public Icon(Sprite sprite, Vector2 size, Color? color = null)
      {
        this.sprite = sprite;
        this.size = size;
        if (!color.HasValue)
          this.color = Color.white;
        else
          this.color = color.Value;
      }
    }
  }
}
