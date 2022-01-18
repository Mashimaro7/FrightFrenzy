// Decompiled with JetBrains decompiler
// Type: CodeMonkey.Utils.Button_UI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CodeMonkey.Utils
{
  public class Button_UI : 
    MonoBehaviour,
    IPointerEnterHandler,
    IEventSystemHandler,
    IPointerExitHandler,
    IPointerClickHandler,
    IPointerDownHandler,
    IPointerUpHandler
  {
    public Action ClickFunc;
    public Action MouseRightClickFunc;
    public Action MouseMiddleClickFunc;
    public Action MouseDownOnceFunc;
    public Action MouseUpFunc;
    public Action MouseOverOnceTooltipFunc;
    public Action MouseOutOnceTooltipFunc;
    public Action MouseOverOnceFunc;
    public Action MouseOutOnceFunc;
    public Action MouseOverFunc;
    public Action MouseOverPerSecFunc;
    public Action MouseUpdate;
    public Action<PointerEventData> OnPointerClickFunc;
    public Button_UI.HoverBehaviour hoverBehaviourType;
    private Action hoverBehaviourFunc_Enter;
    private Action hoverBehaviourFunc_Exit;
    public Color hoverBehaviour_Color_Enter;
    public Color hoverBehaviour_Color_Exit;
    public Image hoverBehaviour_Image;
    public Sprite hoverBehaviour_Sprite_Exit;
    public Sprite hoverBehaviour_Sprite_Enter;
    public bool hoverBehaviour_Move;
    public Vector2 hoverBehaviour_Move_Amount = Vector2.zero;
    private Vector2 posExit;
    private Vector2 posEnter;
    public bool triggerMouseOutFuncOnClick;
    private bool mouseOver;
    private float mouseOverPerSecFuncTimer;
    private Action internalOnPointerEnterFunc;
    private Action internalOnPointerExitFunc;
    private Action internalOnPointerClickFunc;

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
      if (this.internalOnPointerEnterFunc != null)
        this.internalOnPointerEnterFunc();
      if (this.hoverBehaviour_Move)
        this.transform.localPosition = (Vector3) this.posEnter;
      if (this.hoverBehaviourFunc_Enter != null)
        this.hoverBehaviourFunc_Enter();
      if (this.MouseOverOnceFunc != null)
        this.MouseOverOnceFunc();
      if (this.MouseOverOnceTooltipFunc != null)
        this.MouseOverOnceTooltipFunc();
      this.mouseOver = true;
      this.mouseOverPerSecFuncTimer = 0.0f;
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
      if (this.internalOnPointerExitFunc != null)
        this.internalOnPointerExitFunc();
      if (this.hoverBehaviour_Move)
        this.transform.localPosition = (Vector3) this.posExit;
      if (this.hoverBehaviourFunc_Exit != null)
        this.hoverBehaviourFunc_Exit();
      if (this.MouseOutOnceFunc != null)
        this.MouseOutOnceFunc();
      if (this.MouseOutOnceTooltipFunc != null)
        this.MouseOutOnceTooltipFunc();
      this.mouseOver = false;
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
      if (this.internalOnPointerClickFunc != null)
        this.internalOnPointerClickFunc();
      if (this.OnPointerClickFunc != null)
        this.OnPointerClickFunc(eventData);
      if (eventData.button == PointerEventData.InputButton.Left)
      {
        if (this.triggerMouseOutFuncOnClick)
          this.OnPointerExit(eventData);
        if (this.ClickFunc != null)
          this.ClickFunc();
      }
      if (eventData.button == PointerEventData.InputButton.Right && this.MouseRightClickFunc != null)
        this.MouseRightClickFunc();
      if (eventData.button != PointerEventData.InputButton.Middle || this.MouseMiddleClickFunc == null)
        return;
      this.MouseMiddleClickFunc();
    }

    public void Manual_OnPointerExit() => this.OnPointerExit((PointerEventData) null);

    public bool IsMouseOver() => this.mouseOver;

    public void OnPointerDown(PointerEventData eventData)
    {
      if (this.MouseDownOnceFunc == null)
        return;
      this.MouseDownOnceFunc();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
      if (this.MouseUpFunc == null)
        return;
      this.MouseUpFunc();
    }

    private void Update()
    {
      if (this.mouseOver)
      {
        if (this.MouseOverFunc != null)
          this.MouseOverFunc();
        this.mouseOverPerSecFuncTimer -= Time.unscaledDeltaTime;
        if ((double) this.mouseOverPerSecFuncTimer <= 0.0)
        {
          ++this.mouseOverPerSecFuncTimer;
          if (this.MouseOverPerSecFunc != null)
            this.MouseOverPerSecFunc();
        }
      }
      if (this.MouseUpdate == null)
        return;
      this.MouseUpdate();
    }

    private void Awake()
    {
      this.posExit = (Vector2) this.transform.localPosition;
      this.posEnter = (Vector2) this.transform.localPosition + this.hoverBehaviour_Move_Amount;
      this.SetHoverBehaviourType(this.hoverBehaviourType);
    }

    public void SetHoverBehaviourType(Button_UI.HoverBehaviour hoverBehaviourType)
    {
      this.hoverBehaviourType = hoverBehaviourType;
      switch (hoverBehaviourType)
      {
        case Button_UI.HoverBehaviour.Change_Color:
          this.hoverBehaviourFunc_Enter = (Action) (() => this.hoverBehaviour_Image.color = this.hoverBehaviour_Color_Enter);
          this.hoverBehaviourFunc_Exit = (Action) (() => this.hoverBehaviour_Image.color = this.hoverBehaviour_Color_Exit);
          break;
        case Button_UI.HoverBehaviour.Change_Image:
          this.hoverBehaviourFunc_Enter = (Action) (() => this.hoverBehaviour_Image.sprite = this.hoverBehaviour_Sprite_Enter);
          this.hoverBehaviourFunc_Exit = (Action) (() => this.hoverBehaviour_Image.sprite = this.hoverBehaviour_Sprite_Exit);
          break;
        case Button_UI.HoverBehaviour.Change_SetActive:
          this.hoverBehaviourFunc_Enter = (Action) (() => this.hoverBehaviour_Image.gameObject.SetActive(true));
          this.hoverBehaviourFunc_Exit = (Action) (() => this.hoverBehaviour_Image.gameObject.SetActive(false));
          break;
      }
    }

    public Button_UI.InterceptActionHandler InterceptActionClick(
      Func<bool> testPassthroughFunc)
    {
      return this.InterceptAction("ClickFunc", testPassthroughFunc);
    }

    public Button_UI.InterceptActionHandler InterceptAction(
      string fieldName,
      Func<bool> testPassthroughFunc)
    {
      return this.InterceptAction(((object) this).GetType().GetField(fieldName), testPassthroughFunc);
    }

    public Button_UI.InterceptActionHandler InterceptAction(
      FieldInfo fieldInfo,
      Func<bool> testPassthroughFunc)
    {
      Action backFunc = fieldInfo.GetValue((object) this) as Action;
      Button_UI.InterceptActionHandler interceptActionHandler = new Button_UI.InterceptActionHandler((Action) (() => fieldInfo.SetValue((object) this, (object) backFunc)));
      fieldInfo.SetValue((object) this, (object) (Action) (() =>
      {
        if (!testPassthroughFunc())
          return;
        interceptActionHandler.RemoveIntercept();
        backFunc();
      }));
      return interceptActionHandler;
    }

    public enum HoverBehaviour
    {
      Custom,
      Change_Color,
      Change_Image,
      Change_SetActive,
    }

    public class InterceptActionHandler
    {
      private Action removeInterceptFunc;

      public InterceptActionHandler(Action removeInterceptFunc) => this.removeInterceptFunc = removeInterceptFunc;

      public void RemoveIntercept() => this.removeInterceptFunc();
    }
  }
}
