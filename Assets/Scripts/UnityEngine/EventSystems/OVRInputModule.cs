// Decompiled with JetBrains decompiler
// Type: UnityEngine.EventSystems.OVRInputModule
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

namespace UnityEngine.EventSystems
{
  public class OVRInputModule : PointerInputModule
  {
    [Tooltip("Object which points with Z axis. E.g. CentreEyeAnchor from OVRCameraRig")]
    public Transform rayTransform;
    [Tooltip("Gamepad button to act as gaze click")]
    public OVRInput.Button joyPadClickButton = OVRInput.Button.One;
    [Tooltip("Keyboard button to act as gaze click")]
    public KeyCode gazeClickKey = KeyCode.Space;
    [Header("Physics")]
    [Tooltip("Perform an sphere cast to determine correct depth for gaze pointer")]
    public bool performSphereCastForGazepointer;
    [Tooltip("Match the gaze pointer normal to geometry normal for physics colliders")]
    public bool matchNormalOnPhysicsColliders;
    [Header("Gamepad Stick Scroll")]
    [Tooltip("Enable scrolling with the right stick on a gamepad")]
    public bool useRightStickScroll = true;
    [Tooltip("Deadzone for right stick to prevent accidental scrolling")]
    public float rightStickDeadZone = 0.15f;
    [Header("Touchpad Swipe Scroll")]
    [Tooltip("Enable scrolling by swiping the GearVR touchpad")]
    public bool useSwipeScroll = true;
    [Tooltip("Minimum trackpad movement in pixels to start swiping")]
    public float swipeDragThreshold = 2f;
    [Tooltip("Distance scrolled when swipe scroll occurs")]
    public float swipeDragScale = 1f;
    [Tooltip("Invert X axis on touchpad")]
    public bool InvertSwipeXAxis;
    [NonSerialized]
    public OVRRaycaster activeGraphicRaycaster;
    [Header("Dragging")]
    [Tooltip("Minimum pointer movement in degrees to start dragging")]
    public float angleDragThreshold = 1f;
    private float m_NextAction;
    private Vector2 m_LastMousePosition;
    private Vector2 m_MousePosition;
    [Header("Standalone Input Module")]
    [SerializeField]
    private string m_HorizontalAxis = "Horizontal";
    [SerializeField]
    private string m_VerticalAxis = "Vertical";
    [SerializeField]
    private string m_SubmitButton = "Submit";
    [SerializeField]
    private string m_CancelButton = "Cancel";
    [SerializeField]
    private float m_InputActionsPerSecond = 10f;
    [SerializeField]
    private bool m_AllowActivationOnMobileDevice;
    protected Dictionary<int, OVRPointerEventData> m_VRRayPointerData = new Dictionary<int, OVRPointerEventData>();
    private readonly PointerInputModule.MouseState m_MouseState = new PointerInputModule.MouseState();

    protected OVRInputModule()
    {
    }

    [Obsolete("Mode is no longer needed on input module as it handles both mouse and keyboard simultaneously.", false)]
    public OVRInputModule.InputMode inputMode => OVRInputModule.InputMode.Mouse;

    public bool allowActivationOnMobileDevice
    {
      get => this.m_AllowActivationOnMobileDevice;
      set => this.m_AllowActivationOnMobileDevice = value;
    }

    public float inputActionsPerSecond
    {
      get => this.m_InputActionsPerSecond;
      set => this.m_InputActionsPerSecond = value;
    }

    public string horizontalAxis
    {
      get => this.m_HorizontalAxis;
      set => this.m_HorizontalAxis = value;
    }

    public string verticalAxis
    {
      get => this.m_VerticalAxis;
      set => this.m_VerticalAxis = value;
    }

    public string submitButton
    {
      get => this.m_SubmitButton;
      set => this.m_SubmitButton = value;
    }

    public string cancelButton
    {
      get => this.m_CancelButton;
      set => this.m_CancelButton = value;
    }

    public override void UpdateModule()
    {
      this.m_LastMousePosition = this.m_MousePosition;
      this.m_MousePosition = (Vector2) Input.mousePosition;
    }

    public override bool IsModuleSupported() => this.m_AllowActivationOnMobileDevice || Input.mousePresent;

    public override bool ShouldActivateModule() => base.ShouldActivateModule() && Input.GetButtonDown(this.m_SubmitButton) | Input.GetButtonDown(this.m_CancelButton) | !Mathf.Approximately(Input.GetAxisRaw(this.m_HorizontalAxis), 0.0f) | !Mathf.Approximately(Input.GetAxisRaw(this.m_VerticalAxis), 0.0f) | (double) (this.m_MousePosition - this.m_LastMousePosition).sqrMagnitude > 0.0 | Input.GetMouseButtonDown(0);

    public override void ActivateModule()
    {
      base.ActivateModule();
      this.m_MousePosition = (Vector2) Input.mousePosition;
      this.m_LastMousePosition = (Vector2) Input.mousePosition;
      GameObject selectedGameObject = this.eventSystem.currentSelectedGameObject;
      if ((UnityEngine.Object) selectedGameObject == (UnityEngine.Object) null)
        selectedGameObject = this.eventSystem.firstSelectedGameObject;
      this.eventSystem.SetSelectedGameObject(selectedGameObject, this.GetBaseEventData());
    }

    public override void DeactivateModule()
    {
      base.DeactivateModule();
      this.ClearSelection();
    }

    private bool SendSubmitEventToSelectedObject()
    {
      if ((UnityEngine.Object) this.eventSystem.currentSelectedGameObject == (UnityEngine.Object) null)
        return false;
      BaseEventData baseEventData = this.GetBaseEventData();
      if (Input.GetButtonDown(this.m_SubmitButton))
        ExecuteEvents.Execute<ISubmitHandler>(this.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.submitHandler);
      if (Input.GetButtonDown(this.m_CancelButton))
        ExecuteEvents.Execute<ICancelHandler>(this.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.cancelHandler);
      return baseEventData.used;
    }

    private bool AllowMoveEventProcessing(float time) => Input.GetButtonDown(this.m_HorizontalAxis) | Input.GetButtonDown(this.m_VerticalAxis) | (double) time > (double) this.m_NextAction;

    private Vector2 GetRawMoveVector()
    {
      Vector2 zero = Vector2.zero with
      {
        x = Input.GetAxisRaw(this.m_HorizontalAxis),
        y = Input.GetAxisRaw(this.m_VerticalAxis)
      };
      if (Input.GetButtonDown(this.m_HorizontalAxis))
      {
        if ((double) zero.x < 0.0)
          zero.x = -1f;
        if ((double) zero.x > 0.0)
          zero.x = 1f;
      }
      if (Input.GetButtonDown(this.m_VerticalAxis))
      {
        if ((double) zero.y < 0.0)
          zero.y = -1f;
        if ((double) zero.y > 0.0)
          zero.y = 1f;
      }
      return zero;
    }

    private bool SendMoveEventToSelectedObject()
    {
      float unscaledTime = Time.unscaledTime;
      if (!this.AllowMoveEventProcessing(unscaledTime))
        return false;
      Vector2 rawMoveVector = this.GetRawMoveVector();
      AxisEventData axisEventData = this.GetAxisEventData(rawMoveVector.x, rawMoveVector.y, 0.6f);
      if (!Mathf.Approximately(axisEventData.moveVector.x, 0.0f) || !Mathf.Approximately(axisEventData.moveVector.y, 0.0f))
        ExecuteEvents.Execute<IMoveHandler>(this.eventSystem.currentSelectedGameObject, (BaseEventData) axisEventData, ExecuteEvents.moveHandler);
      this.m_NextAction = unscaledTime + 1f / this.m_InputActionsPerSecond;
      return axisEventData.used;
    }

    private bool SendUpdateEventToSelectedObject()
    {
      if ((UnityEngine.Object) this.eventSystem.currentSelectedGameObject == (UnityEngine.Object) null)
        return false;
      BaseEventData baseEventData = this.GetBaseEventData();
      ExecuteEvents.Execute<IUpdateSelectedHandler>(this.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.updateSelectedHandler);
      return baseEventData.used;
    }

    private void ProcessMousePress(PointerInputModule.MouseButtonEventData data)
    {
      PointerEventData buttonData = data.buttonData;
      GameObject gameObject1 = buttonData.pointerCurrentRaycast.gameObject;
      if (data.PressedThisFrame())
      {
        buttonData.eligibleForClick = true;
        buttonData.delta = Vector2.zero;
        buttonData.dragging = false;
        buttonData.useDragThreshold = true;
        buttonData.pressPosition = buttonData.position;
        if (buttonData.IsVRPointer())
          buttonData.SetSwipeStart((Vector2) Input.mousePosition);
        buttonData.pointerPressRaycast = buttonData.pointerCurrentRaycast;
        this.DeselectIfSelectionChanged(gameObject1, (BaseEventData) buttonData);
        GameObject gameObject2 = ExecuteEvents.ExecuteHierarchy<IPointerDownHandler>(gameObject1, (BaseEventData) buttonData, ExecuteEvents.pointerDownHandler);
        if ((UnityEngine.Object) gameObject2 == (UnityEngine.Object) null)
          gameObject2 = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject1);
        float unscaledTime = Time.unscaledTime;
        if ((UnityEngine.Object) gameObject2 == (UnityEngine.Object) buttonData.lastPress)
        {
          if ((double) unscaledTime - (double) buttonData.clickTime < 0.300000011920929)
            ++buttonData.clickCount;
          else
            buttonData.clickCount = 1;
          buttonData.clickTime = unscaledTime;
        }
        else
          buttonData.clickCount = 1;
        buttonData.pointerPress = gameObject2;
        buttonData.rawPointerPress = gameObject1;
        buttonData.clickTime = unscaledTime;
        buttonData.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(gameObject1);
        if ((UnityEngine.Object) buttonData.pointerDrag != (UnityEngine.Object) null)
          ExecuteEvents.Execute<IInitializePotentialDragHandler>(buttonData.pointerDrag, (BaseEventData) buttonData, ExecuteEvents.initializePotentialDrag);
      }
      if (!data.ReleasedThisFrame())
        return;
      ExecuteEvents.Execute<IPointerUpHandler>(buttonData.pointerPress, (BaseEventData) buttonData, ExecuteEvents.pointerUpHandler);
      GameObject eventHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject1);
      if ((UnityEngine.Object) buttonData.pointerPress == (UnityEngine.Object) eventHandler && buttonData.eligibleForClick)
        ExecuteEvents.Execute<IPointerClickHandler>(buttonData.pointerPress, (BaseEventData) buttonData, ExecuteEvents.pointerClickHandler);
      else if ((UnityEngine.Object) buttonData.pointerDrag != (UnityEngine.Object) null)
        ExecuteEvents.ExecuteHierarchy<IDropHandler>(gameObject1, (BaseEventData) buttonData, ExecuteEvents.dropHandler);
      buttonData.eligibleForClick = false;
      buttonData.pointerPress = (GameObject) null;
      buttonData.rawPointerPress = (GameObject) null;
      if ((UnityEngine.Object) buttonData.pointerDrag != (UnityEngine.Object) null && buttonData.dragging)
        ExecuteEvents.Execute<IEndDragHandler>(buttonData.pointerDrag, (BaseEventData) buttonData, ExecuteEvents.endDragHandler);
      buttonData.dragging = false;
      buttonData.pointerDrag = (GameObject) null;
      if (!((UnityEngine.Object) gameObject1 != (UnityEngine.Object) buttonData.pointerEnter))
        return;
      this.HandlePointerExitAndEnter(buttonData, (GameObject) null);
      this.HandlePointerExitAndEnter(buttonData, gameObject1);
    }

    private void ProcessMouseEvent(PointerInputModule.MouseState mouseData)
    {
      int num1 = mouseData.AnyPressesThisFrame() ? 1 : 0;
      bool flag = mouseData.AnyReleasesThisFrame();
      PointerInputModule.MouseButtonEventData eventData = mouseData.GetButtonState(PointerEventData.InputButton.Left).eventData;
      int num2 = flag ? 1 : 0;
      PointerEventData buttonData = eventData.buttonData;
      if (!OVRInputModule.UseMouse(num1 != 0, num2 != 0, buttonData))
        return;
      this.ProcessMousePress(eventData);
      this.ProcessMove(eventData.buttonData);
      this.ProcessDrag(eventData.buttonData);
      this.ProcessMousePress(mouseData.GetButtonState(PointerEventData.InputButton.Right).eventData);
      this.ProcessDrag(mouseData.GetButtonState(PointerEventData.InputButton.Right).eventData.buttonData);
      this.ProcessMousePress(mouseData.GetButtonState(PointerEventData.InputButton.Middle).eventData);
      this.ProcessDrag(mouseData.GetButtonState(PointerEventData.InputButton.Middle).eventData.buttonData);
      if (Mathf.Approximately(eventData.buttonData.scrollDelta.sqrMagnitude, 0.0f))
        return;
      ExecuteEvents.ExecuteHierarchy<IScrollHandler>(ExecuteEvents.GetEventHandler<IScrollHandler>(eventData.buttonData.pointerCurrentRaycast.gameObject), (BaseEventData) eventData.buttonData, ExecuteEvents.scrollHandler);
    }

    public override void Process()
    {
      bool selectedObject = this.SendUpdateEventToSelectedObject();
      if (this.eventSystem.sendNavigationEvents)
      {
        if (!selectedObject)
          selectedObject |= this.SendMoveEventToSelectedObject();
        if (!selectedObject)
          this.SendSubmitEventToSelectedObject();
      }
      this.ProcessMouseEvent(this.GetGazePointerData());
      this.ProcessMouseEvent(this.GetCanvasPointerData());
    }

    private static bool UseMouse(bool pressed, bool released, PointerEventData pointerData) => pressed | released || OVRInputModule.IsPointerMoving(pointerData) || pointerData.IsScrolling();

    protected void CopyFromTo(OVRPointerEventData from, OVRPointerEventData to)
    {
      to.position = from.position;
      to.delta = from.delta;
      to.scrollDelta = from.scrollDelta;
      to.pointerCurrentRaycast = from.pointerCurrentRaycast;
      to.pointerEnter = from.pointerEnter;
      to.worldSpaceRay = from.worldSpaceRay;
    }

    protected new void CopyFromTo(PointerEventData from, PointerEventData to)
    {
      to.position = from.position;
      to.delta = from.delta;
      to.scrollDelta = from.scrollDelta;
      to.pointerCurrentRaycast = from.pointerCurrentRaycast;
      to.pointerEnter = from.pointerEnter;
    }

    protected bool GetPointerData(int id, out OVRPointerEventData data, bool create)
    {
      if (!(!this.m_VRRayPointerData.TryGetValue(id, out data) & create))
        return false;
      ref OVRPointerEventData local = ref data;
      OVRPointerEventData pointerEventData = new OVRPointerEventData(this.eventSystem);
      pointerEventData.pointerId = id;
      local = pointerEventData;
      this.m_VRRayPointerData.Add(id, data);
      return true;
    }

    protected new void ClearSelection()
    {
      BaseEventData baseEventData = this.GetBaseEventData();
      foreach (PointerEventData currentPointerData in this.m_PointerData.Values)
        this.HandlePointerExitAndEnter(currentPointerData, (GameObject) null);
      foreach (PointerEventData currentPointerData in this.m_VRRayPointerData.Values)
        this.HandlePointerExitAndEnter(currentPointerData, (GameObject) null);
      this.m_PointerData.Clear();
      this.eventSystem.SetSelectedGameObject((GameObject) null, baseEventData);
    }

    private static Vector3 GetRectTransformNormal(RectTransform rectTransform)
    {
      Vector3[] fourCornersArray = new Vector3[4];
      rectTransform.GetWorldCorners(fourCornersArray);
      Vector3 lhs = fourCornersArray[3] - fourCornersArray[0];
      Vector3 vector3 = fourCornersArray[1] - fourCornersArray[0];
      rectTransform.GetWorldCorners(fourCornersArray);
      Vector3 rhs = vector3;
      return Vector3.Cross(lhs, rhs).normalized;
    }

    protected virtual PointerInputModule.MouseState GetGazePointerData()
    {
      OVRPointerEventData data1;
      this.GetPointerData(-1, out data1, true);
      data1.Reset();
      data1.worldSpaceRay = new Ray(this.rayTransform.position, this.rayTransform.forward);
      data1.scrollDelta = this.GetExtraScrollDelta();
      data1.button = PointerEventData.InputButton.Left;
      data1.useDragThreshold = true;
      this.eventSystem.RaycastAll((PointerEventData) data1, this.m_RaycastResultCache);
      RaycastResult firstRaycast = BaseInputModule.FindFirstRaycast(this.m_RaycastResultCache);
      data1.pointerCurrentRaycast = firstRaycast;
      this.m_RaycastResultCache.Clear();
      OVRRaycaster module1 = firstRaycast.module as OVRRaycaster;
      if ((bool) (UnityEngine.Object) module1)
      {
        data1.position = module1.GetScreenPosition(firstRaycast);
        RectTransform component = firstRaycast.gameObject.GetComponent<RectTransform>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        {
          OVRGazePointer.instance.SetPosition(firstRaycast.worldPosition, OVRInputModule.GetRectTransformNormal(component));
          OVRGazePointer.instance.RequestShow();
        }
      }
      OVRPhysicsRaycaster module2 = firstRaycast.module as OVRPhysicsRaycaster;
      if ((bool) (UnityEngine.Object) module2)
      {
        Vector3 worldPosition = firstRaycast.worldPosition;
        if (this.performSphereCastForGazepointer)
        {
          List<RaycastResult> resultAppendList = new List<RaycastResult>();
          module2.Spherecast((PointerEventData) data1, resultAppendList, OVRGazePointer.instance.GetCurrentRadius());
          if (resultAppendList.Count > 0 && (double) resultAppendList[0].distance < (double) firstRaycast.distance)
            worldPosition = resultAppendList[0].worldPosition;
        }
        data1.position = module2.GetScreenPos(firstRaycast.worldPosition);
        OVRGazePointer.instance.RequestShow();
        if (this.matchNormalOnPhysicsColliders)
          OVRGazePointer.instance.SetPosition(worldPosition, firstRaycast.worldNormal);
        else
          OVRGazePointer.instance.SetPosition(worldPosition);
      }
      OVRPointerEventData data2;
      this.GetPointerData(-2, out data2, true);
      this.CopyFromTo(data1, data2);
      data2.button = PointerEventData.InputButton.Right;
      OVRPointerEventData data3;
      this.GetPointerData(-3, out data3, true);
      this.CopyFromTo(data1, data3);
      data3.button = PointerEventData.InputButton.Middle;
      this.m_MouseState.SetButtonState(PointerEventData.InputButton.Left, this.GetGazeButtonState(), (PointerEventData) data1);
      this.m_MouseState.SetButtonState(PointerEventData.InputButton.Right, PointerEventData.FramePressState.NotChanged, (PointerEventData) data2);
      this.m_MouseState.SetButtonState(PointerEventData.InputButton.Middle, PointerEventData.FramePressState.NotChanged, (PointerEventData) data3);
      return this.m_MouseState;
    }

    protected PointerInputModule.MouseState GetCanvasPointerData()
    {
      PointerEventData data1;
      this.GetPointerData(-1, out data1, true);
      data1.Reset();
      data1.position = Vector2.zero;
      data1.scrollDelta = Input.mouseScrollDelta;
      data1.button = PointerEventData.InputButton.Left;
      if ((bool) (UnityEngine.Object) this.activeGraphicRaycaster)
      {
        this.activeGraphicRaycaster.RaycastPointer(data1, this.m_RaycastResultCache);
        RaycastResult firstRaycast = BaseInputModule.FindFirstRaycast(this.m_RaycastResultCache);
        data1.pointerCurrentRaycast = firstRaycast;
        this.m_RaycastResultCache.Clear();
        OVRRaycaster module = firstRaycast.module as OVRRaycaster;
        if ((bool) (UnityEngine.Object) module)
        {
          Vector2 screenPosition = module.GetScreenPosition(firstRaycast);
          data1.delta = screenPosition - data1.position;
          data1.position = screenPosition;
        }
      }
      PointerEventData data2;
      this.GetPointerData(-2, out data2, true);
      this.CopyFromTo(data1, data2);
      data2.button = PointerEventData.InputButton.Right;
      PointerEventData data3;
      this.GetPointerData(-3, out data3, true);
      this.CopyFromTo(data1, data3);
      data3.button = PointerEventData.InputButton.Middle;
      this.m_MouseState.SetButtonState(PointerEventData.InputButton.Left, this.StateForMouseButton(0), data1);
      this.m_MouseState.SetButtonState(PointerEventData.InputButton.Right, this.StateForMouseButton(1), data2);
      this.m_MouseState.SetButtonState(PointerEventData.InputButton.Middle, this.StateForMouseButton(2), data3);
      return this.m_MouseState;
    }

    private bool ShouldStartDrag(PointerEventData pointerEvent)
    {
      if (!pointerEvent.useDragThreshold)
        return true;
      if (!pointerEvent.IsVRPointer())
        return (double) (pointerEvent.pressPosition - pointerEvent.position).sqrMagnitude >= (double) (this.eventSystem.pixelDragThreshold * this.eventSystem.pixelDragThreshold);
      Vector3 position = pointerEvent.pressEventCamera.transform.position;
      return (double) Vector3.Dot((pointerEvent.pointerPressRaycast.worldPosition - position).normalized, (pointerEvent.pointerCurrentRaycast.worldPosition - position).normalized) < (double) Mathf.Cos((float) Math.PI / 180f * this.angleDragThreshold);
    }

    private static bool IsPointerMoving(PointerEventData pointerEvent) => pointerEvent.IsVRPointer() || pointerEvent.IsPointerMoving();

    protected Vector2 SwipeAdjustedPosition(
      Vector2 originalPosition,
      PointerEventData pointerEvent)
    {
      return originalPosition;
    }

    protected override void ProcessDrag(PointerEventData pointerEvent)
    {
      Vector2 position = pointerEvent.position;
      bool flag = OVRInputModule.IsPointerMoving(pointerEvent);
      if (flag && (UnityEngine.Object) pointerEvent.pointerDrag != (UnityEngine.Object) null && !pointerEvent.dragging && this.ShouldStartDrag(pointerEvent))
      {
        if (pointerEvent.IsVRPointer())
          pointerEvent.position = this.SwipeAdjustedPosition(position, pointerEvent);
        ExecuteEvents.Execute<IBeginDragHandler>(pointerEvent.pointerDrag, (BaseEventData) pointerEvent, ExecuteEvents.beginDragHandler);
        pointerEvent.dragging = true;
      }
      if (!(pointerEvent.dragging & flag) || !((UnityEngine.Object) pointerEvent.pointerDrag != (UnityEngine.Object) null))
        return;
      if (pointerEvent.IsVRPointer())
        pointerEvent.position = this.SwipeAdjustedPosition(position, pointerEvent);
      if ((UnityEngine.Object) pointerEvent.pointerPress != (UnityEngine.Object) pointerEvent.pointerDrag)
      {
        ExecuteEvents.Execute<IPointerUpHandler>(pointerEvent.pointerPress, (BaseEventData) pointerEvent, ExecuteEvents.pointerUpHandler);
        pointerEvent.eligibleForClick = false;
        pointerEvent.pointerPress = (GameObject) null;
        pointerEvent.rawPointerPress = (GameObject) null;
      }
      ExecuteEvents.Execute<IDragHandler>(pointerEvent.pointerDrag, (BaseEventData) pointerEvent, ExecuteEvents.dragHandler);
    }

    protected virtual PointerEventData.FramePressState GetGazeButtonState()
    {
      bool flag1 = Input.GetKeyDown(this.gazeClickKey) || OVRInput.GetDown(this.joyPadClickButton);
      bool flag2 = Input.GetKeyUp(this.gazeClickKey) || OVRInput.GetUp(this.joyPadClickButton);
      if (flag1 & flag2)
        return PointerEventData.FramePressState.PressedAndReleased;
      if (flag1)
        return PointerEventData.FramePressState.Pressed;
      return flag2 ? PointerEventData.FramePressState.Released : PointerEventData.FramePressState.NotChanged;
    }

    protected Vector2 GetExtraScrollDelta()
    {
      Vector2 extraScrollDelta = new Vector2();
      if (this.useRightStickScroll)
      {
        Vector2 vector2 = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
        if ((double) Mathf.Abs(vector2.x) < (double) this.rightStickDeadZone)
          vector2.x = 0.0f;
        if ((double) Mathf.Abs(vector2.y) < (double) this.rightStickDeadZone)
          vector2.y = 0.0f;
        extraScrollDelta = vector2;
      }
      return extraScrollDelta;
    }

    [Obsolete("Mode is no longer needed on input module as it handles both mouse and keyboard simultaneously.", false)]
    public enum InputMode
    {
      Mouse,
      Buttons,
    }
  }
}
