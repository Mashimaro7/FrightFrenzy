// Decompiled with JetBrains decompiler
// Type: CodeMonkey.Utils.Button_Sprite
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CodeMonkey.Utils
{
  public class Button_Sprite : MonoBehaviour
  {
    private static Func<Camera> GetWorldCamera;
    public Action ClickFunc;
    public Action MouseRightDownOnceFunc;
    public Action MouseRightDownFunc;
    public Action MouseRightUpFunc;
    public Action MouseDownOnceFunc;
    public Action MouseUpOnceFunc;
    public Action MouseOverOnceFunc;
    public Action MouseOutOnceFunc;
    public Action MouseOverOnceTooltipFunc;
    public Action MouseOutOnceTooltipFunc;
    private bool draggingMouseRight;
    private Vector3 mouseRightDragStart;
    public Action<Vector3, Vector3> MouseRightDragFunc;
    public Action<Vector3, Vector3> MouseRightDragUpdateFunc;
    public bool triggerMouseRightDragOnEnter;
    public Button_Sprite.HoverBehaviour hoverBehaviourType;
    private Action hoverBehaviourFunc_Enter;
    private Action hoverBehaviourFunc_Exit;
    public Color hoverBehaviour_Color_Enter = new Color(1f, 1f, 1f, 1f);
    public Color hoverBehaviour_Color_Exit = new Color(1f, 1f, 1f, 1f);
    public SpriteRenderer hoverBehaviour_Image;
    public Sprite hoverBehaviour_Sprite_Exit;
    public Sprite hoverBehaviour_Sprite_Enter;
    public bool hoverBehaviour_Move;
    public Vector2 hoverBehaviour_Move_Amount = Vector2.zero;
    private Vector3 posExit;
    private Vector3 posEnter;
    public bool triggerMouseOutFuncOnClick;
    public bool clickThroughUI;
    private Action internalOnMouseDownFunc;
    private Action internalOnMouseEnterFunc;
    private Action internalOnMouseExitFunc;

    public static void SetGetWorldCamera(Func<Camera> GetWorldCamera) => Button_Sprite.GetWorldCamera = GetWorldCamera;

    public void SetHoverBehaviourChangeColor(Color colorOver, Color colorOut)
    {
      this.hoverBehaviourType = Button_Sprite.HoverBehaviour.Change_Color;
      this.hoverBehaviour_Color_Enter = colorOver;
      this.hoverBehaviour_Color_Exit = colorOut;
      if ((UnityEngine.Object) this.hoverBehaviour_Image == (UnityEngine.Object) null)
        this.hoverBehaviour_Image = this.transform.GetComponent<SpriteRenderer>();
      this.hoverBehaviour_Image.color = this.hoverBehaviour_Color_Exit;
      this.SetupHoverBehaviour();
    }

    private void OnMouseDown()
    {
      if (!this.clickThroughUI && Button_Sprite.IsPointerOverUI())
        return;
      if (this.internalOnMouseDownFunc != null)
        this.internalOnMouseDownFunc();
      if (this.ClickFunc != null)
        this.ClickFunc();
      if (!this.triggerMouseOutFuncOnClick)
        return;
      this.OnMouseExit();
    }

    public void Manual_OnMouseExit() => this.OnMouseExit();

    private void OnMouseUp()
    {
      if (this.MouseUpOnceFunc == null)
        return;
      this.MouseUpOnceFunc();
    }

    private void OnMouseEnter()
    {
      if (!this.clickThroughUI && Button_Sprite.IsPointerOverUI())
        return;
      if (this.internalOnMouseEnterFunc != null)
        this.internalOnMouseEnterFunc();
      if (this.hoverBehaviour_Move)
        this.transform.localPosition = this.posEnter;
      if (this.hoverBehaviourFunc_Enter != null)
        this.hoverBehaviourFunc_Enter();
      if (this.MouseOverOnceFunc != null)
        this.MouseOverOnceFunc();
      if (this.MouseOverOnceTooltipFunc == null)
        return;
      this.MouseOverOnceTooltipFunc();
    }

    private void OnMouseExit()
    {
      if (this.internalOnMouseExitFunc != null)
        this.internalOnMouseExitFunc();
      if (this.hoverBehaviour_Move)
        this.transform.localPosition = this.posExit;
      if (this.hoverBehaviourFunc_Exit != null)
        this.hoverBehaviourFunc_Exit();
      if (this.MouseOutOnceFunc != null)
        this.MouseOutOnceFunc();
      if (this.MouseOutOnceTooltipFunc == null)
        return;
      this.MouseOutOnceTooltipFunc();
    }

    private void OnMouseOver()
    {
      if (!this.clickThroughUI && Button_Sprite.IsPointerOverUI())
        return;
      if (Input.GetMouseButton(1))
      {
        if (this.MouseRightDownFunc != null)
          this.MouseRightDownFunc();
        if (!this.draggingMouseRight && this.triggerMouseRightDragOnEnter)
        {
          this.draggingMouseRight = true;
          this.mouseRightDragStart = Button_Sprite.GetWorldPositionFromUI();
        }
      }
      if (!Input.GetMouseButtonDown(1))
        return;
      this.draggingMouseRight = true;
      this.mouseRightDragStart = Button_Sprite.GetWorldPositionFromUI();
      if (this.MouseRightDownOnceFunc == null)
        return;
      this.MouseRightDownOnceFunc();
    }

    private void Update()
    {
      if (this.draggingMouseRight && this.MouseRightDragUpdateFunc != null)
        this.MouseRightDragUpdateFunc(this.mouseRightDragStart, Button_Sprite.GetWorldPositionFromUI());
      if (!Input.GetMouseButtonUp(1))
        return;
      if (this.draggingMouseRight)
      {
        this.draggingMouseRight = false;
        if (this.MouseRightDragFunc != null)
          this.MouseRightDragFunc(this.mouseRightDragStart, Button_Sprite.GetWorldPositionFromUI());
      }
      if (this.MouseRightUpFunc == null)
        return;
      this.MouseRightUpFunc();
    }

    private void Awake()
    {
      if (Button_Sprite.GetWorldCamera == null)
        Button_Sprite.SetGetWorldCamera((Func<Camera>) (() => Camera.main));
      this.posExit = this.transform.localPosition;
      this.posEnter = this.transform.localPosition + (Vector3) this.hoverBehaviour_Move_Amount;
      this.SetupHoverBehaviour();
    }

    private void SetupHoverBehaviour()
    {
      switch (this.hoverBehaviourType)
      {
        case Button_Sprite.HoverBehaviour.Change_Color:
          this.hoverBehaviourFunc_Enter = (Action) (() => this.hoverBehaviour_Image.color = this.hoverBehaviour_Color_Enter);
          this.hoverBehaviourFunc_Exit = (Action) (() => this.hoverBehaviour_Image.color = this.hoverBehaviour_Color_Exit);
          break;
        case Button_Sprite.HoverBehaviour.Change_Image:
          this.hoverBehaviourFunc_Enter = (Action) (() => this.hoverBehaviour_Image.sprite = this.hoverBehaviour_Sprite_Enter);
          this.hoverBehaviourFunc_Exit = (Action) (() => this.hoverBehaviour_Image.sprite = this.hoverBehaviour_Sprite_Exit);
          break;
        case Button_Sprite.HoverBehaviour.Change_SetActive:
          this.hoverBehaviourFunc_Enter = (Action) (() => this.hoverBehaviour_Image.gameObject.SetActive(true));
          this.hoverBehaviourFunc_Exit = (Action) (() => this.hoverBehaviour_Image.gameObject.SetActive(false));
          break;
      }
    }

    private static Vector3 GetWorldPositionFromUI() => Button_Sprite.GetWorldCamera().ScreenToWorldPoint(Input.mousePosition);

    private static bool IsPointerOverUI()
    {
      if (EventSystem.current.IsPointerOverGameObject())
        return true;
      PointerEventData eventData = new PointerEventData(EventSystem.current);
      eventData.position = (Vector2) Input.mousePosition;
      List<RaycastResult> raycastResults = new List<RaycastResult>();
      EventSystem.current.RaycastAll(eventData, raycastResults);
      return raycastResults.Count > 0;
    }

    public enum HoverBehaviour
    {
      Custom,
      Change_Color,
      Change_Image,
      Change_SetActive,
    }
  }
}
