// Decompiled with JetBrains decompiler
// Type: OVRInput
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public static class OVRInput
{
  private static readonly float AXIS_AS_BUTTON_THRESHOLD = 0.5f;
  private static readonly float AXIS_DEADZONE_THRESHOLD = 0.2f;
  private static List<OVRInput.OVRControllerBase> controllers;
  private static OVRInput.Controller activeControllerType = OVRInput.Controller.None;
  private static OVRInput.Controller connectedControllerTypes = OVRInput.Controller.None;
  private static OVRPlugin.Step stepType = OVRPlugin.Step.Render;
  private static int fixedUpdateCount = 0;
  private static bool _pluginSupportsActiveController = false;
  private static bool _pluginSupportsActiveControllerCached = false;
  private static Version _pluginSupportsActiveControllerMinVersion = new Version(1, 9, 0);

  private static bool pluginSupportsActiveController
  {
    get
    {
      if (!OVRInput._pluginSupportsActiveControllerCached)
      {
        OVRInput._pluginSupportsActiveController = OVRPlugin.version >= OVRInput._pluginSupportsActiveControllerMinVersion;
        OVRInput._pluginSupportsActiveControllerCached = true;
      }
      return OVRInput._pluginSupportsActiveController;
    }
  }

  static OVRInput() => OVRInput.controllers = new List<OVRInput.OVRControllerBase>()
  {
    (OVRInput.OVRControllerBase) new OVRInput.OVRControllerGamepadPC(),
    (OVRInput.OVRControllerBase) new OVRInput.OVRControllerTouch(),
    (OVRInput.OVRControllerBase) new OVRInput.OVRControllerLTouch(),
    (OVRInput.OVRControllerBase) new OVRInput.OVRControllerRTouch(),
    (OVRInput.OVRControllerBase) new OVRInput.OVRControllerRemote()
  };

  public static void Update()
  {
    OVRInput.connectedControllerTypes = OVRInput.Controller.None;
    OVRInput.stepType = OVRPlugin.Step.Render;
    OVRInput.fixedUpdateCount = 0;
    for (int index = 0; index < OVRInput.controllers.Count; ++index)
    {
      OVRInput.OVRControllerBase controller = OVRInput.controllers[index];
      OVRInput.connectedControllerTypes |= controller.Update();
      if ((OVRInput.connectedControllerTypes & controller.controllerType) != OVRInput.Controller.None)
      {
        OVRInput.RawTouch rawMask = OVRInput.RawTouch.Any;
        if (OVRInput.Get(OVRInput.RawButton.Any, controller.controllerType) || OVRInput.Get(rawMask, controller.controllerType))
          OVRInput.activeControllerType = controller.controllerType;
      }
    }
    if (OVRInput.activeControllerType == OVRInput.Controller.LTouch || OVRInput.activeControllerType == OVRInput.Controller.RTouch)
      OVRInput.activeControllerType = OVRInput.Controller.Touch;
    if ((OVRInput.connectedControllerTypes & OVRInput.activeControllerType) == OVRInput.Controller.None)
      OVRInput.activeControllerType = OVRInput.Controller.None;
    if (OVRInput.activeControllerType == OVRInput.Controller.None)
    {
      if ((OVRInput.connectedControllerTypes & OVRInput.Controller.RTrackedRemote) != OVRInput.Controller.None)
        OVRInput.activeControllerType = OVRInput.Controller.RTrackedRemote;
      else if ((OVRInput.connectedControllerTypes & OVRInput.Controller.LTrackedRemote) != OVRInput.Controller.None)
        OVRInput.activeControllerType = OVRInput.Controller.LTrackedRemote;
    }
    if (!OVRInput.pluginSupportsActiveController)
      return;
    OVRInput.connectedControllerTypes = (OVRInput.Controller) OVRPlugin.GetConnectedControllers();
    OVRInput.activeControllerType = (OVRInput.Controller) OVRPlugin.GetActiveController();
  }

  public static void FixedUpdate()
  {
    OVRInput.stepType = OVRPlugin.Step.Physics;
    double predictionSeconds = (double) OVRInput.fixedUpdateCount * (double) Time.fixedDeltaTime / (double) Mathf.Max(Time.timeScale, 1E-06f);
    ++OVRInput.fixedUpdateCount;
    OVRPlugin.UpdateNodePhysicsPoses(0, predictionSeconds);
  }

  public static bool GetControllerOrientationTracked(OVRInput.Controller controllerType)
  {
    switch (controllerType)
    {
      case OVRInput.Controller.LTouch:
      case OVRInput.Controller.LTrackedRemote:
        return OVRPlugin.GetNodeOrientationTracked(OVRPlugin.Node.HandLeft);
      case OVRInput.Controller.RTouch:
      case OVRInput.Controller.RTrackedRemote:
        return OVRPlugin.GetNodeOrientationTracked(OVRPlugin.Node.HandRight);
      default:
        return false;
    }
  }

  public static bool GetControllerPositionTracked(OVRInput.Controller controllerType)
  {
    switch (controllerType)
    {
      case OVRInput.Controller.LTouch:
      case OVRInput.Controller.LTrackedRemote:
        return OVRPlugin.GetNodePositionTracked(OVRPlugin.Node.HandLeft);
      case OVRInput.Controller.RTouch:
      case OVRInput.Controller.RTrackedRemote:
        return OVRPlugin.GetNodePositionTracked(OVRPlugin.Node.HandRight);
      default:
        return false;
    }
  }

  public static Vector3 GetLocalControllerPosition(OVRInput.Controller controllerType)
  {
    switch (controllerType)
    {
      case OVRInput.Controller.LTouch:
      case OVRInput.Controller.LTrackedRemote:
        return OVRPlugin.GetNodePose(OVRPlugin.Node.HandLeft, OVRInput.stepType).ToOVRPose().position;
      case OVRInput.Controller.RTouch:
      case OVRInput.Controller.RTrackedRemote:
        return OVRPlugin.GetNodePose(OVRPlugin.Node.HandRight, OVRInput.stepType).ToOVRPose().position;
      default:
        return Vector3.zero;
    }
  }

  public static Vector3 GetLocalControllerVelocity(OVRInput.Controller controllerType)
  {
    switch (controllerType)
    {
      case OVRInput.Controller.LTouch:
      case OVRInput.Controller.LTrackedRemote:
        return OVRPlugin.GetNodeVelocity(OVRPlugin.Node.HandLeft, OVRInput.stepType).FromFlippedZVector3f();
      case OVRInput.Controller.RTouch:
      case OVRInput.Controller.RTrackedRemote:
        return OVRPlugin.GetNodeVelocity(OVRPlugin.Node.HandRight, OVRInput.stepType).FromFlippedZVector3f();
      default:
        return Vector3.zero;
    }
  }

  public static Vector3 GetLocalControllerAcceleration(OVRInput.Controller controllerType)
  {
    switch (controllerType)
    {
      case OVRInput.Controller.LTouch:
      case OVRInput.Controller.LTrackedRemote:
        return OVRPlugin.GetNodeAcceleration(OVRPlugin.Node.HandLeft, OVRInput.stepType).FromFlippedZVector3f();
      case OVRInput.Controller.RTouch:
      case OVRInput.Controller.RTrackedRemote:
        return OVRPlugin.GetNodeAcceleration(OVRPlugin.Node.HandRight, OVRInput.stepType).FromFlippedZVector3f();
      default:
        return Vector3.zero;
    }
  }

  public static Quaternion GetLocalControllerRotation(OVRInput.Controller controllerType)
  {
    switch (controllerType)
    {
      case OVRInput.Controller.LTouch:
      case OVRInput.Controller.LTrackedRemote:
        return OVRPlugin.GetNodePose(OVRPlugin.Node.HandLeft, OVRInput.stepType).ToOVRPose().orientation;
      case OVRInput.Controller.RTouch:
      case OVRInput.Controller.RTrackedRemote:
        return OVRPlugin.GetNodePose(OVRPlugin.Node.HandRight, OVRInput.stepType).ToOVRPose().orientation;
      default:
        return Quaternion.identity;
    }
  }

  public static Vector3 GetLocalControllerAngularVelocity(OVRInput.Controller controllerType)
  {
    switch (controllerType)
    {
      case OVRInput.Controller.LTouch:
      case OVRInput.Controller.LTrackedRemote:
        return OVRPlugin.GetNodeAngularVelocity(OVRPlugin.Node.HandLeft, OVRInput.stepType).FromFlippedZVector3f();
      case OVRInput.Controller.RTouch:
      case OVRInput.Controller.RTrackedRemote:
        return OVRPlugin.GetNodeAngularVelocity(OVRPlugin.Node.HandRight, OVRInput.stepType).FromFlippedZVector3f();
      default:
        return Vector3.zero;
    }
  }

  public static Vector3 GetLocalControllerAngularAcceleration(
    OVRInput.Controller controllerType)
  {
    switch (controllerType)
    {
      case OVRInput.Controller.LTouch:
      case OVRInput.Controller.LTrackedRemote:
        return OVRPlugin.GetNodeAngularAcceleration(OVRPlugin.Node.HandLeft, OVRInput.stepType).FromFlippedZVector3f();
      case OVRInput.Controller.RTouch:
      case OVRInput.Controller.RTrackedRemote:
        return OVRPlugin.GetNodeAngularAcceleration(OVRPlugin.Node.HandRight, OVRInput.stepType).FromFlippedZVector3f();
      default:
        return Vector3.zero;
    }
  }

  public static bool Get(OVRInput.Button virtualMask, OVRInput.Controller controllerMask = OVRInput.Controller.Active) => OVRInput.GetResolvedButton(virtualMask, OVRInput.RawButton.None, controllerMask);

  public static bool Get(OVRInput.RawButton rawMask, OVRInput.Controller controllerMask = OVRInput.Controller.Active) => OVRInput.GetResolvedButton(OVRInput.Button.None, rawMask, controllerMask);

  private static bool GetResolvedButton(
    OVRInput.Button virtualMask,
    OVRInput.RawButton rawMask,
    OVRInput.Controller controllerMask)
  {
    if ((controllerMask & OVRInput.Controller.Active) != OVRInput.Controller.None)
      controllerMask |= OVRInput.activeControllerType;
    for (int index = 0; index < OVRInput.controllers.Count; ++index)
    {
      OVRInput.OVRControllerBase controller = OVRInput.controllers[index];
      if (OVRInput.ShouldResolveController(controller.controllerType, controllerMask))
      {
        OVRInput.RawButton rawButton = rawMask | controller.ResolveToRawMask(virtualMask);
        if (((OVRInput.RawButton) controller.currentState.Buttons & rawButton) != OVRInput.RawButton.None)
          return true;
      }
    }
    return false;
  }

  public static bool GetDown(OVRInput.Button virtualMask, OVRInput.Controller controllerMask = OVRInput.Controller.Active) => OVRInput.GetResolvedButtonDown(virtualMask, OVRInput.RawButton.None, controllerMask);

  public static bool GetDown(OVRInput.RawButton rawMask, OVRInput.Controller controllerMask = OVRInput.Controller.Active) => OVRInput.GetResolvedButtonDown(OVRInput.Button.None, rawMask, controllerMask);

  private static bool GetResolvedButtonDown(
    OVRInput.Button virtualMask,
    OVRInput.RawButton rawMask,
    OVRInput.Controller controllerMask)
  {
    bool resolvedButtonDown = false;
    if ((controllerMask & OVRInput.Controller.Active) != OVRInput.Controller.None)
      controllerMask |= OVRInput.activeControllerType;
    for (int index = 0; index < OVRInput.controllers.Count; ++index)
    {
      OVRInput.OVRControllerBase controller = OVRInput.controllers[index];
      if (OVRInput.ShouldResolveController(controller.controllerType, controllerMask))
      {
        OVRInput.RawButton rawButton = rawMask | controller.ResolveToRawMask(virtualMask);
        if (((OVRInput.RawButton) controller.previousState.Buttons & rawButton) != OVRInput.RawButton.None)
          return false;
        if (((OVRInput.RawButton) controller.currentState.Buttons & rawButton) != OVRInput.RawButton.None && ((OVRInput.RawButton) controller.previousState.Buttons & rawButton) == OVRInput.RawButton.None)
          resolvedButtonDown = true;
      }
    }
    return resolvedButtonDown;
  }

  public static bool GetUp(OVRInput.Button virtualMask, OVRInput.Controller controllerMask = OVRInput.Controller.Active) => OVRInput.GetResolvedButtonUp(virtualMask, OVRInput.RawButton.None, controllerMask);

  public static bool GetUp(OVRInput.RawButton rawMask, OVRInput.Controller controllerMask = OVRInput.Controller.Active) => OVRInput.GetResolvedButtonUp(OVRInput.Button.None, rawMask, controllerMask);

  private static bool GetResolvedButtonUp(
    OVRInput.Button virtualMask,
    OVRInput.RawButton rawMask,
    OVRInput.Controller controllerMask)
  {
    bool resolvedButtonUp = false;
    if ((controllerMask & OVRInput.Controller.Active) != OVRInput.Controller.None)
      controllerMask |= OVRInput.activeControllerType;
    for (int index = 0; index < OVRInput.controllers.Count; ++index)
    {
      OVRInput.OVRControllerBase controller = OVRInput.controllers[index];
      if (OVRInput.ShouldResolveController(controller.controllerType, controllerMask))
      {
        OVRInput.RawButton rawButton = rawMask | controller.ResolveToRawMask(virtualMask);
        if (((OVRInput.RawButton) controller.currentState.Buttons & rawButton) != OVRInput.RawButton.None)
          return false;
        if (((OVRInput.RawButton) controller.currentState.Buttons & rawButton) == OVRInput.RawButton.None && ((OVRInput.RawButton) controller.previousState.Buttons & rawButton) != OVRInput.RawButton.None)
          resolvedButtonUp = true;
      }
    }
    return resolvedButtonUp;
  }

  public static bool Get(OVRInput.Touch virtualMask, OVRInput.Controller controllerMask = OVRInput.Controller.Active) => OVRInput.GetResolvedTouch(virtualMask, OVRInput.RawTouch.None, controllerMask);

  public static bool Get(OVRInput.RawTouch rawMask, OVRInput.Controller controllerMask = OVRInput.Controller.Active) => OVRInput.GetResolvedTouch(OVRInput.Touch.None, rawMask, controllerMask);

  private static bool GetResolvedTouch(
    OVRInput.Touch virtualMask,
    OVRInput.RawTouch rawMask,
    OVRInput.Controller controllerMask)
  {
    if ((controllerMask & OVRInput.Controller.Active) != OVRInput.Controller.None)
      controllerMask |= OVRInput.activeControllerType;
    for (int index = 0; index < OVRInput.controllers.Count; ++index)
    {
      OVRInput.OVRControllerBase controller = OVRInput.controllers[index];
      if (OVRInput.ShouldResolveController(controller.controllerType, controllerMask))
      {
        OVRInput.RawTouch rawTouch = rawMask | controller.ResolveToRawMask(virtualMask);
        if (((OVRInput.RawTouch) controller.currentState.Touches & rawTouch) != OVRInput.RawTouch.None)
          return true;
      }
    }
    return false;
  }

  public static bool GetDown(OVRInput.Touch virtualMask, OVRInput.Controller controllerMask = OVRInput.Controller.Active) => OVRInput.GetResolvedTouchDown(virtualMask, OVRInput.RawTouch.None, controllerMask);

  public static bool GetDown(OVRInput.RawTouch rawMask, OVRInput.Controller controllerMask = OVRInput.Controller.Active) => OVRInput.GetResolvedTouchDown(OVRInput.Touch.None, rawMask, controllerMask);

  private static bool GetResolvedTouchDown(
    OVRInput.Touch virtualMask,
    OVRInput.RawTouch rawMask,
    OVRInput.Controller controllerMask)
  {
    bool resolvedTouchDown = false;
    if ((controllerMask & OVRInput.Controller.Active) != OVRInput.Controller.None)
      controllerMask |= OVRInput.activeControllerType;
    for (int index = 0; index < OVRInput.controllers.Count; ++index)
    {
      OVRInput.OVRControllerBase controller = OVRInput.controllers[index];
      if (OVRInput.ShouldResolveController(controller.controllerType, controllerMask))
      {
        OVRInput.RawTouch rawTouch = rawMask | controller.ResolveToRawMask(virtualMask);
        if (((OVRInput.RawTouch) controller.previousState.Touches & rawTouch) != OVRInput.RawTouch.None)
          return false;
        if (((OVRInput.RawTouch) controller.currentState.Touches & rawTouch) != OVRInput.RawTouch.None && ((OVRInput.RawTouch) controller.previousState.Touches & rawTouch) == OVRInput.RawTouch.None)
          resolvedTouchDown = true;
      }
    }
    return resolvedTouchDown;
  }

  public static bool GetUp(OVRInput.Touch virtualMask, OVRInput.Controller controllerMask = OVRInput.Controller.Active) => OVRInput.GetResolvedTouchUp(virtualMask, OVRInput.RawTouch.None, controllerMask);

  public static bool GetUp(OVRInput.RawTouch rawMask, OVRInput.Controller controllerMask = OVRInput.Controller.Active) => OVRInput.GetResolvedTouchUp(OVRInput.Touch.None, rawMask, controllerMask);

  private static bool GetResolvedTouchUp(
    OVRInput.Touch virtualMask,
    OVRInput.RawTouch rawMask,
    OVRInput.Controller controllerMask)
  {
    bool resolvedTouchUp = false;
    if ((controllerMask & OVRInput.Controller.Active) != OVRInput.Controller.None)
      controllerMask |= OVRInput.activeControllerType;
    for (int index = 0; index < OVRInput.controllers.Count; ++index)
    {
      OVRInput.OVRControllerBase controller = OVRInput.controllers[index];
      if (OVRInput.ShouldResolveController(controller.controllerType, controllerMask))
      {
        OVRInput.RawTouch rawTouch = rawMask | controller.ResolveToRawMask(virtualMask);
        if (((OVRInput.RawTouch) controller.currentState.Touches & rawTouch) != OVRInput.RawTouch.None)
          return false;
        if (((OVRInput.RawTouch) controller.currentState.Touches & rawTouch) == OVRInput.RawTouch.None && ((OVRInput.RawTouch) controller.previousState.Touches & rawTouch) != OVRInput.RawTouch.None)
          resolvedTouchUp = true;
      }
    }
    return resolvedTouchUp;
  }

  public static bool Get(OVRInput.NearTouch virtualMask, OVRInput.Controller controllerMask = OVRInput.Controller.Active) => OVRInput.GetResolvedNearTouch(virtualMask, OVRInput.RawNearTouch.None, controllerMask);

  public static bool Get(OVRInput.RawNearTouch rawMask, OVRInput.Controller controllerMask = OVRInput.Controller.Active) => OVRInput.GetResolvedNearTouch(OVRInput.NearTouch.None, rawMask, controllerMask);

  private static bool GetResolvedNearTouch(
    OVRInput.NearTouch virtualMask,
    OVRInput.RawNearTouch rawMask,
    OVRInput.Controller controllerMask)
  {
    if ((controllerMask & OVRInput.Controller.Active) != OVRInput.Controller.None)
      controllerMask |= OVRInput.activeControllerType;
    for (int index = 0; index < OVRInput.controllers.Count; ++index)
    {
      OVRInput.OVRControllerBase controller = OVRInput.controllers[index];
      if (OVRInput.ShouldResolveController(controller.controllerType, controllerMask))
      {
        OVRInput.RawNearTouch rawNearTouch = rawMask | controller.ResolveToRawMask(virtualMask);
        if (((OVRInput.RawNearTouch) controller.currentState.NearTouches & rawNearTouch) != OVRInput.RawNearTouch.None)
          return true;
      }
    }
    return false;
  }

  public static bool GetDown(OVRInput.NearTouch virtualMask, OVRInput.Controller controllerMask = OVRInput.Controller.Active) => OVRInput.GetResolvedNearTouchDown(virtualMask, OVRInput.RawNearTouch.None, controllerMask);

  public static bool GetDown(OVRInput.RawNearTouch rawMask, OVRInput.Controller controllerMask = OVRInput.Controller.Active) => OVRInput.GetResolvedNearTouchDown(OVRInput.NearTouch.None, rawMask, controllerMask);

  private static bool GetResolvedNearTouchDown(
    OVRInput.NearTouch virtualMask,
    OVRInput.RawNearTouch rawMask,
    OVRInput.Controller controllerMask)
  {
    bool resolvedNearTouchDown = false;
    if ((controllerMask & OVRInput.Controller.Active) != OVRInput.Controller.None)
      controllerMask |= OVRInput.activeControllerType;
    for (int index = 0; index < OVRInput.controllers.Count; ++index)
    {
      OVRInput.OVRControllerBase controller = OVRInput.controllers[index];
      if (OVRInput.ShouldResolveController(controller.controllerType, controllerMask))
      {
        OVRInput.RawNearTouch rawNearTouch = rawMask | controller.ResolveToRawMask(virtualMask);
        if (((OVRInput.RawNearTouch) controller.previousState.NearTouches & rawNearTouch) != OVRInput.RawNearTouch.None)
          return false;
        if (((OVRInput.RawNearTouch) controller.currentState.NearTouches & rawNearTouch) != OVRInput.RawNearTouch.None && ((OVRInput.RawNearTouch) controller.previousState.NearTouches & rawNearTouch) == OVRInput.RawNearTouch.None)
          resolvedNearTouchDown = true;
      }
    }
    return resolvedNearTouchDown;
  }

  public static bool GetUp(OVRInput.NearTouch virtualMask, OVRInput.Controller controllerMask = OVRInput.Controller.Active) => OVRInput.GetResolvedNearTouchUp(virtualMask, OVRInput.RawNearTouch.None, controllerMask);

  public static bool GetUp(OVRInput.RawNearTouch rawMask, OVRInput.Controller controllerMask = OVRInput.Controller.Active) => OVRInput.GetResolvedNearTouchUp(OVRInput.NearTouch.None, rawMask, controllerMask);

  private static bool GetResolvedNearTouchUp(
    OVRInput.NearTouch virtualMask,
    OVRInput.RawNearTouch rawMask,
    OVRInput.Controller controllerMask)
  {
    bool resolvedNearTouchUp = false;
    if ((controllerMask & OVRInput.Controller.Active) != OVRInput.Controller.None)
      controllerMask |= OVRInput.activeControllerType;
    for (int index = 0; index < OVRInput.controllers.Count; ++index)
    {
      OVRInput.OVRControllerBase controller = OVRInput.controllers[index];
      if (OVRInput.ShouldResolveController(controller.controllerType, controllerMask))
      {
        OVRInput.RawNearTouch rawNearTouch = rawMask | controller.ResolveToRawMask(virtualMask);
        if (((OVRInput.RawNearTouch) controller.currentState.NearTouches & rawNearTouch) != OVRInput.RawNearTouch.None)
          return false;
        if (((OVRInput.RawNearTouch) controller.currentState.NearTouches & rawNearTouch) == OVRInput.RawNearTouch.None && ((OVRInput.RawNearTouch) controller.previousState.NearTouches & rawNearTouch) != OVRInput.RawNearTouch.None)
          resolvedNearTouchUp = true;
      }
    }
    return resolvedNearTouchUp;
  }

  public static float Get(OVRInput.Axis1D virtualMask, OVRInput.Controller controllerMask = OVRInput.Controller.Active) => OVRInput.GetResolvedAxis1D(virtualMask, OVRInput.RawAxis1D.None, controllerMask);

  public static float Get(OVRInput.RawAxis1D rawMask, OVRInput.Controller controllerMask = OVRInput.Controller.Active) => OVRInput.GetResolvedAxis1D(OVRInput.Axis1D.None, rawMask, controllerMask);

  private static float GetResolvedAxis1D(
    OVRInput.Axis1D virtualMask,
    OVRInput.RawAxis1D rawMask,
    OVRInput.Controller controllerMask)
  {
    float a = 0.0f;
    if ((controllerMask & OVRInput.Controller.Active) != OVRInput.Controller.None)
      controllerMask |= OVRInput.activeControllerType;
    for (int index = 0; index < OVRInput.controllers.Count; ++index)
    {
      OVRInput.OVRControllerBase controller = OVRInput.controllers[index];
      if (OVRInput.ShouldResolveController(controller.controllerType, controllerMask))
      {
        OVRInput.RawAxis1D rawAxis1D = rawMask | controller.ResolveToRawMask(virtualMask);
        if ((OVRInput.RawAxis1D.LIndexTrigger & rawAxis1D) != OVRInput.RawAxis1D.None)
        {
          float num = controller.currentState.LIndexTrigger;
          if (controller.shouldApplyDeadzone)
            num = OVRInput.CalculateDeadzone(num, OVRInput.AXIS_DEADZONE_THRESHOLD);
          a = OVRInput.CalculateAbsMax(a, num);
        }
        if ((OVRInput.RawAxis1D.RIndexTrigger & rawAxis1D) != OVRInput.RawAxis1D.None)
        {
          float num = controller.currentState.RIndexTrigger;
          if (controller.shouldApplyDeadzone)
            num = OVRInput.CalculateDeadzone(num, OVRInput.AXIS_DEADZONE_THRESHOLD);
          a = OVRInput.CalculateAbsMax(a, num);
        }
        if ((OVRInput.RawAxis1D.LHandTrigger & rawAxis1D) != OVRInput.RawAxis1D.None)
        {
          float num = controller.currentState.LHandTrigger;
          if (controller.shouldApplyDeadzone)
            num = OVRInput.CalculateDeadzone(num, OVRInput.AXIS_DEADZONE_THRESHOLD);
          a = OVRInput.CalculateAbsMax(a, num);
        }
        if ((OVRInput.RawAxis1D.RHandTrigger & rawAxis1D) != OVRInput.RawAxis1D.None)
        {
          float num = controller.currentState.RHandTrigger;
          if (controller.shouldApplyDeadzone)
            num = OVRInput.CalculateDeadzone(num, OVRInput.AXIS_DEADZONE_THRESHOLD);
          a = OVRInput.CalculateAbsMax(a, num);
        }
      }
    }
    return a;
  }

  public static Vector2 Get(OVRInput.Axis2D virtualMask, OVRInput.Controller controllerMask = OVRInput.Controller.Active) => OVRInput.GetResolvedAxis2D(virtualMask, OVRInput.RawAxis2D.None, controllerMask);

  public static Vector2 Get(OVRInput.RawAxis2D rawMask, OVRInput.Controller controllerMask = OVRInput.Controller.Active) => OVRInput.GetResolvedAxis2D(OVRInput.Axis2D.None, rawMask, controllerMask);

  private static Vector2 GetResolvedAxis2D(
    OVRInput.Axis2D virtualMask,
    OVRInput.RawAxis2D rawMask,
    OVRInput.Controller controllerMask)
  {
    Vector2 a = Vector2.zero;
    if ((controllerMask & OVRInput.Controller.Active) != OVRInput.Controller.None)
      controllerMask |= OVRInput.activeControllerType;
    for (int index = 0; index < OVRInput.controllers.Count; ++index)
    {
      OVRInput.OVRControllerBase controller = OVRInput.controllers[index];
      if (OVRInput.ShouldResolveController(controller.controllerType, controllerMask))
      {
        OVRInput.RawAxis2D rawAxis2D = rawMask | controller.ResolveToRawMask(virtualMask);
        if ((OVRInput.RawAxis2D.LThumbstick & rawAxis2D) != OVRInput.RawAxis2D.None)
        {
          Vector2 vector2 = new Vector2(controller.currentState.LThumbstick.x, controller.currentState.LThumbstick.y);
          if (controller.shouldApplyDeadzone)
            vector2 = OVRInput.CalculateDeadzone(vector2, OVRInput.AXIS_DEADZONE_THRESHOLD);
          a = OVRInput.CalculateAbsMax(a, vector2);
        }
        if ((OVRInput.RawAxis2D.LTouchpad & rawAxis2D) != OVRInput.RawAxis2D.None)
        {
          Vector2 b = new Vector2(controller.currentState.LTouchpad.x, controller.currentState.LTouchpad.y);
          a = OVRInput.CalculateAbsMax(a, b);
        }
        if ((OVRInput.RawAxis2D.RThumbstick & rawAxis2D) != OVRInput.RawAxis2D.None)
        {
          Vector2 vector2 = new Vector2(controller.currentState.RThumbstick.x, controller.currentState.RThumbstick.y);
          if (controller.shouldApplyDeadzone)
            vector2 = OVRInput.CalculateDeadzone(vector2, OVRInput.AXIS_DEADZONE_THRESHOLD);
          a = OVRInput.CalculateAbsMax(a, vector2);
        }
        if ((OVRInput.RawAxis2D.RTouchpad & rawAxis2D) != OVRInput.RawAxis2D.None)
        {
          Vector2 b = new Vector2(controller.currentState.RTouchpad.x, controller.currentState.RTouchpad.y);
          a = OVRInput.CalculateAbsMax(a, b);
        }
      }
    }
    return a;
  }

  public static OVRInput.Controller GetConnectedControllers() => OVRInput.connectedControllerTypes;

  public static bool IsControllerConnected(OVRInput.Controller controller) => (OVRInput.connectedControllerTypes & controller) == controller;

  public static OVRInput.Controller GetActiveController() => OVRInput.activeControllerType;

  public static void SetControllerVibration(
    float frequency,
    float amplitude,
    OVRInput.Controller controllerMask = OVRInput.Controller.Active)
  {
    if ((controllerMask & OVRInput.Controller.Active) != OVRInput.Controller.None)
      controllerMask |= OVRInput.activeControllerType;
    for (int index = 0; index < OVRInput.controllers.Count; ++index)
    {
      OVRInput.OVRControllerBase controller = OVRInput.controllers[index];
      if (OVRInput.ShouldResolveController(controller.controllerType, controllerMask))
        controller.SetControllerVibration(frequency, amplitude);
    }
  }

  public static void RecenterController(OVRInput.Controller controllerMask = OVRInput.Controller.Active)
  {
    if ((controllerMask & OVRInput.Controller.Active) != OVRInput.Controller.None)
      controllerMask |= OVRInput.activeControllerType;
    for (int index = 0; index < OVRInput.controllers.Count; ++index)
    {
      OVRInput.OVRControllerBase controller = OVRInput.controllers[index];
      if (OVRInput.ShouldResolveController(controller.controllerType, controllerMask))
        controller.RecenterController();
    }
  }

  public static bool GetControllerWasRecentered(OVRInput.Controller controllerMask = OVRInput.Controller.Active)
  {
    if ((controllerMask & OVRInput.Controller.Active) != OVRInput.Controller.None)
      controllerMask |= OVRInput.activeControllerType;
    bool controllerWasRecentered = false;
    for (int index = 0; index < OVRInput.controllers.Count; ++index)
    {
      OVRInput.OVRControllerBase controller = OVRInput.controllers[index];
      if (OVRInput.ShouldResolveController(controller.controllerType, controllerMask))
        controllerWasRecentered |= controller.WasRecentered();
    }
    return controllerWasRecentered;
  }

  public static byte GetControllerRecenterCount(OVRInput.Controller controllerMask = OVRInput.Controller.Active)
  {
    if ((controllerMask & OVRInput.Controller.Active) != OVRInput.Controller.None)
      controllerMask |= OVRInput.activeControllerType;
    byte controllerRecenterCount = 0;
    for (int index = 0; index < OVRInput.controllers.Count; ++index)
    {
      OVRInput.OVRControllerBase controller = OVRInput.controllers[index];
      if (OVRInput.ShouldResolveController(controller.controllerType, controllerMask))
      {
        controllerRecenterCount = controller.GetRecenterCount();
        break;
      }
    }
    return controllerRecenterCount;
  }

  public static byte GetControllerBatteryPercentRemaining(OVRInput.Controller controllerMask = OVRInput.Controller.Active)
  {
    if ((controllerMask & OVRInput.Controller.Active) != OVRInput.Controller.None)
      controllerMask |= OVRInput.activeControllerType;
    byte percentRemaining = 0;
    for (int index = 0; index < OVRInput.controllers.Count; ++index)
    {
      OVRInput.OVRControllerBase controller = OVRInput.controllers[index];
      if (OVRInput.ShouldResolveController(controller.controllerType, controllerMask))
      {
        percentRemaining = controller.GetBatteryPercentRemaining();
        break;
      }
    }
    return percentRemaining;
  }

  private static Vector2 CalculateAbsMax(Vector2 a, Vector2 b) => (double) a.sqrMagnitude >= (double) b.sqrMagnitude ? a : b;

  private static float CalculateAbsMax(float a, float b) => ((double) a >= 0.0 ? (double) a : -(double) a) >= ((double) b >= 0.0 ? (double) b : -(double) b) ? a : b;

  private static Vector2 CalculateDeadzone(Vector2 a, float deadzone)
  {
    if ((double) a.sqrMagnitude <= (double) deadzone * (double) deadzone)
      return Vector2.zero;
    a *= (float) (((double) a.magnitude - (double) deadzone) / (1.0 - (double) deadzone));
    return (double) a.sqrMagnitude > 1.0 ? a.normalized : a;
  }

  private static float CalculateDeadzone(float a, float deadzone)
  {
    float num = (double) a >= 0.0 ? a : -a;
    if ((double) num <= (double) deadzone)
      return 0.0f;
    a *= (float) (((double) num - (double) deadzone) / (1.0 - (double) deadzone));
    if ((double) a * (double) a <= 1.0)
      return a;
    return (double) a < 0.0 ? -1f : 1f;
  }

  private static bool ShouldResolveController(
    OVRInput.Controller controllerType,
    OVRInput.Controller controllerMask)
  {
    bool flag = false;
    if ((controllerType & controllerMask) == controllerType)
      flag = true;
    if ((controllerMask & OVRInput.Controller.Touch) == OVRInput.Controller.Touch && (controllerType & OVRInput.Controller.Touch) != OVRInput.Controller.None && (controllerType & OVRInput.Controller.Touch) != OVRInput.Controller.Touch)
      flag = false;
    return flag;
  }

  [System.Flags]
  public enum Button
  {
    None = 0,
    One = 1,
    Two = 2,
    Three = 4,
    Four = 8,
    Start = 256, // 0x00000100
    Back = 512, // 0x00000200
    PrimaryShoulder = 4096, // 0x00001000
    PrimaryIndexTrigger = 8192, // 0x00002000
    PrimaryHandTrigger = 16384, // 0x00004000
    PrimaryThumbstick = 32768, // 0x00008000
    PrimaryThumbstickUp = 65536, // 0x00010000
    PrimaryThumbstickDown = 131072, // 0x00020000
    PrimaryThumbstickLeft = 262144, // 0x00040000
    PrimaryThumbstickRight = 524288, // 0x00080000
    PrimaryTouchpad = 1024, // 0x00000400
    SecondaryShoulder = 1048576, // 0x00100000
    SecondaryIndexTrigger = 2097152, // 0x00200000
    SecondaryHandTrigger = 4194304, // 0x00400000
    SecondaryThumbstick = 8388608, // 0x00800000
    SecondaryThumbstickUp = 16777216, // 0x01000000
    SecondaryThumbstickDown = 33554432, // 0x02000000
    SecondaryThumbstickLeft = 67108864, // 0x04000000
    SecondaryThumbstickRight = 134217728, // 0x08000000
    SecondaryTouchpad = 2048, // 0x00000800
    DpadUp = 16, // 0x00000010
    DpadDown = 32, // 0x00000020
    DpadLeft = 64, // 0x00000040
    DpadRight = 128, // 0x00000080
    Up = 268435456, // 0x10000000
    Down = 536870912, // 0x20000000
    Left = 1073741824, // 0x40000000
    Right = -2147483648, // 0x80000000
    Any = Right | Left | Down | Up | DpadRight | DpadLeft | DpadDown | DpadUp | SecondaryTouchpad | SecondaryThumbstickRight | SecondaryThumbstickLeft | SecondaryThumbstickDown | SecondaryThumbstickUp | SecondaryThumbstick | SecondaryHandTrigger | SecondaryIndexTrigger | SecondaryShoulder | PrimaryTouchpad | PrimaryThumbstickRight | PrimaryThumbstickLeft | PrimaryThumbstickDown | PrimaryThumbstickUp | PrimaryThumbstick | PrimaryHandTrigger | PrimaryIndexTrigger | PrimaryShoulder | Back | Start | Four | Three | Two | One, // 0xFFFFFFFF
  }

  [System.Flags]
  public enum RawButton
  {
    None = 0,
    A = 1,
    B = 2,
    X = 256, // 0x00000100
    Y = 512, // 0x00000200
    Start = 1048576, // 0x00100000
    Back = 2097152, // 0x00200000
    LShoulder = 2048, // 0x00000800
    LIndexTrigger = 268435456, // 0x10000000
    LHandTrigger = 536870912, // 0x20000000
    LThumbstick = 1024, // 0x00000400
    LThumbstickUp = 16, // 0x00000010
    LThumbstickDown = 32, // 0x00000020
    LThumbstickLeft = 64, // 0x00000040
    LThumbstickRight = 128, // 0x00000080
    LTouchpad = 1073741824, // 0x40000000
    RShoulder = 8,
    RIndexTrigger = 67108864, // 0x04000000
    RHandTrigger = 134217728, // 0x08000000
    RThumbstick = 4,
    RThumbstickUp = 4096, // 0x00001000
    RThumbstickDown = 8192, // 0x00002000
    RThumbstickLeft = 16384, // 0x00004000
    RThumbstickRight = 32768, // 0x00008000
    RTouchpad = -2147483648, // 0x80000000
    DpadUp = 65536, // 0x00010000
    DpadDown = 131072, // 0x00020000
    DpadLeft = 262144, // 0x00040000
    DpadRight = 524288, // 0x00080000
    Any = -1, // 0xFFFFFFFF
  }

  [System.Flags]
  public enum Touch
  {
    None = 0,
    One = 1,
    Two = 2,
    Three = 4,
    Four = 8,
    PrimaryIndexTrigger = 8192, // 0x00002000
    PrimaryThumbstick = 32768, // 0x00008000
    PrimaryThumbRest = 4096, // 0x00001000
    PrimaryTouchpad = 1024, // 0x00000400
    SecondaryIndexTrigger = 2097152, // 0x00200000
    SecondaryThumbstick = 8388608, // 0x00800000
    SecondaryThumbRest = 1048576, // 0x00100000
    SecondaryTouchpad = 2048, // 0x00000800
    Any = -1, // 0xFFFFFFFF
  }

  [System.Flags]
  public enum RawTouch
  {
    None = 0,
    A = 1,
    B = 2,
    X = 256, // 0x00000100
    Y = 512, // 0x00000200
    LIndexTrigger = 4096, // 0x00001000
    LThumbstick = 1024, // 0x00000400
    LThumbRest = 2048, // 0x00000800
    LTouchpad = 1073741824, // 0x40000000
    RIndexTrigger = 16, // 0x00000010
    RThumbstick = 4,
    RThumbRest = 8,
    RTouchpad = -2147483648, // 0x80000000
    Any = -1, // 0xFFFFFFFF
  }

  [System.Flags]
  public enum NearTouch
  {
    None = 0,
    PrimaryIndexTrigger = 1,
    PrimaryThumbButtons = 2,
    SecondaryIndexTrigger = 4,
    SecondaryThumbButtons = 8,
    Any = -1, // 0xFFFFFFFF
  }

  [System.Flags]
  public enum RawNearTouch
  {
    None = 0,
    LIndexTrigger = 1,
    LThumbButtons = 2,
    RIndexTrigger = 4,
    RThumbButtons = 8,
    Any = -1, // 0xFFFFFFFF
  }

  [System.Flags]
  public enum Axis1D
  {
    None = 0,
    PrimaryIndexTrigger = 1,
    PrimaryHandTrigger = 4,
    SecondaryIndexTrigger = 2,
    SecondaryHandTrigger = 8,
    Any = -1, // 0xFFFFFFFF
  }

  [System.Flags]
  public enum RawAxis1D
  {
    None = 0,
    LIndexTrigger = 1,
    LHandTrigger = 4,
    RIndexTrigger = 2,
    RHandTrigger = 8,
    Any = -1, // 0xFFFFFFFF
  }

  [System.Flags]
  public enum Axis2D
  {
    None = 0,
    PrimaryThumbstick = 1,
    PrimaryTouchpad = 4,
    SecondaryThumbstick = 2,
    SecondaryTouchpad = 8,
    Any = -1, // 0xFFFFFFFF
  }

  [System.Flags]
  public enum RawAxis2D
  {
    None = 0,
    LThumbstick = 1,
    LTouchpad = 4,
    RThumbstick = 2,
    RTouchpad = 8,
    Any = -1, // 0xFFFFFFFF
  }

  [System.Flags]
  public enum Controller
  {
    None = 0,
    LTouch = 1,
    RTouch = 2,
    Touch = RTouch | LTouch, // 0x00000003
    Remote = 4,
    Gamepad = 16, // 0x00000010
    Touchpad = 134217728, // 0x08000000
    LTrackedRemote = 16777216, // 0x01000000
    RTrackedRemote = 33554432, // 0x02000000
    Active = -2147483648, // 0x80000000
    All = -1, // 0xFFFFFFFF
  }

  private abstract class OVRControllerBase
  {
    public OVRInput.Controller controllerType;
    public OVRInput.OVRControllerBase.VirtualButtonMap buttonMap = new OVRInput.OVRControllerBase.VirtualButtonMap();
    public OVRInput.OVRControllerBase.VirtualTouchMap touchMap = new OVRInput.OVRControllerBase.VirtualTouchMap();
    public OVRInput.OVRControllerBase.VirtualNearTouchMap nearTouchMap = new OVRInput.OVRControllerBase.VirtualNearTouchMap();
    public OVRInput.OVRControllerBase.VirtualAxis1DMap axis1DMap = new OVRInput.OVRControllerBase.VirtualAxis1DMap();
    public OVRInput.OVRControllerBase.VirtualAxis2DMap axis2DMap = new OVRInput.OVRControllerBase.VirtualAxis2DMap();
    public OVRPlugin.ControllerState4 previousState;
    public OVRPlugin.ControllerState4 currentState;
    public bool shouldApplyDeadzone = true;

    public OVRControllerBase()
    {
      this.ConfigureButtonMap();
      this.ConfigureTouchMap();
      this.ConfigureNearTouchMap();
      this.ConfigureAxis1DMap();
      this.ConfigureAxis2DMap();
    }

    public virtual OVRInput.Controller Update()
    {
      OVRPlugin.ControllerState4 controllerState4 = OVRPlugin.GetControllerState4((uint) this.controllerType);
      if ((double) controllerState4.LIndexTrigger >= (double) OVRInput.AXIS_AS_BUTTON_THRESHOLD)
        controllerState4.Buttons |= 268435456U;
      if ((double) controllerState4.LHandTrigger >= (double) OVRInput.AXIS_AS_BUTTON_THRESHOLD)
        controllerState4.Buttons |= 536870912U;
      if ((double) controllerState4.LThumbstick.y >= (double) OVRInput.AXIS_AS_BUTTON_THRESHOLD)
        controllerState4.Buttons |= 16U;
      if ((double) controllerState4.LThumbstick.y <= -(double) OVRInput.AXIS_AS_BUTTON_THRESHOLD)
        controllerState4.Buttons |= 32U;
      if ((double) controllerState4.LThumbstick.x <= -(double) OVRInput.AXIS_AS_BUTTON_THRESHOLD)
        controllerState4.Buttons |= 64U;
      if ((double) controllerState4.LThumbstick.x >= (double) OVRInput.AXIS_AS_BUTTON_THRESHOLD)
        controllerState4.Buttons |= 128U;
      if ((double) controllerState4.RIndexTrigger >= (double) OVRInput.AXIS_AS_BUTTON_THRESHOLD)
        controllerState4.Buttons |= 67108864U;
      if ((double) controllerState4.RHandTrigger >= (double) OVRInput.AXIS_AS_BUTTON_THRESHOLD)
        controllerState4.Buttons |= 134217728U;
      if ((double) controllerState4.RThumbstick.y >= (double) OVRInput.AXIS_AS_BUTTON_THRESHOLD)
        controllerState4.Buttons |= 4096U;
      if ((double) controllerState4.RThumbstick.y <= -(double) OVRInput.AXIS_AS_BUTTON_THRESHOLD)
        controllerState4.Buttons |= 8192U;
      if ((double) controllerState4.RThumbstick.x <= -(double) OVRInput.AXIS_AS_BUTTON_THRESHOLD)
        controllerState4.Buttons |= 16384U;
      if ((double) controllerState4.RThumbstick.x >= (double) OVRInput.AXIS_AS_BUTTON_THRESHOLD)
        controllerState4.Buttons |= 32768U;
      this.previousState = this.currentState;
      this.currentState = controllerState4;
      return (OVRInput.Controller) this.currentState.ConnectedControllers & this.controllerType;
    }

    public virtual void SetControllerVibration(float frequency, float amplitude) => OVRPlugin.SetControllerVibration((uint) this.controllerType, frequency, amplitude);

    public virtual void RecenterController() => OVRPlugin.RecenterTrackingOrigin(OVRPlugin.RecenterFlags.Controllers);

    public virtual bool WasRecentered() => false;

    public virtual byte GetRecenterCount() => 0;

    public virtual byte GetBatteryPercentRemaining() => 0;

    public abstract void ConfigureButtonMap();

    public abstract void ConfigureTouchMap();

    public abstract void ConfigureNearTouchMap();

    public abstract void ConfigureAxis1DMap();

    public abstract void ConfigureAxis2DMap();

    public OVRInput.RawButton ResolveToRawMask(OVRInput.Button virtualMask) => this.buttonMap.ToRawMask(virtualMask);

    public OVRInput.RawTouch ResolveToRawMask(OVRInput.Touch virtualMask) => this.touchMap.ToRawMask(virtualMask);

    public OVRInput.RawNearTouch ResolveToRawMask(OVRInput.NearTouch virtualMask) => this.nearTouchMap.ToRawMask(virtualMask);

    public OVRInput.RawAxis1D ResolveToRawMask(OVRInput.Axis1D virtualMask) => this.axis1DMap.ToRawMask(virtualMask);

    public OVRInput.RawAxis2D ResolveToRawMask(OVRInput.Axis2D virtualMask) => this.axis2DMap.ToRawMask(virtualMask);

    public class VirtualButtonMap
    {
      public OVRInput.RawButton None;
      public OVRInput.RawButton One;
      public OVRInput.RawButton Two;
      public OVRInput.RawButton Three;
      public OVRInput.RawButton Four;
      public OVRInput.RawButton Start;
      public OVRInput.RawButton Back;
      public OVRInput.RawButton PrimaryShoulder;
      public OVRInput.RawButton PrimaryIndexTrigger;
      public OVRInput.RawButton PrimaryHandTrigger;
      public OVRInput.RawButton PrimaryThumbstick;
      public OVRInput.RawButton PrimaryThumbstickUp;
      public OVRInput.RawButton PrimaryThumbstickDown;
      public OVRInput.RawButton PrimaryThumbstickLeft;
      public OVRInput.RawButton PrimaryThumbstickRight;
      public OVRInput.RawButton PrimaryTouchpad;
      public OVRInput.RawButton SecondaryShoulder;
      public OVRInput.RawButton SecondaryIndexTrigger;
      public OVRInput.RawButton SecondaryHandTrigger;
      public OVRInput.RawButton SecondaryThumbstick;
      public OVRInput.RawButton SecondaryThumbstickUp;
      public OVRInput.RawButton SecondaryThumbstickDown;
      public OVRInput.RawButton SecondaryThumbstickLeft;
      public OVRInput.RawButton SecondaryThumbstickRight;
      public OVRInput.RawButton SecondaryTouchpad;
      public OVRInput.RawButton DpadUp;
      public OVRInput.RawButton DpadDown;
      public OVRInput.RawButton DpadLeft;
      public OVRInput.RawButton DpadRight;
      public OVRInput.RawButton Up;
      public OVRInput.RawButton Down;
      public OVRInput.RawButton Left;
      public OVRInput.RawButton Right;

      public OVRInput.RawButton ToRawMask(OVRInput.Button virtualMask)
      {
        OVRInput.RawButton rawMask = OVRInput.RawButton.None;
        if (virtualMask == OVRInput.Button.None)
          return OVRInput.RawButton.None;
        if ((virtualMask & OVRInput.Button.One) != OVRInput.Button.None)
          rawMask |= this.One;
        if ((virtualMask & OVRInput.Button.Two) != OVRInput.Button.None)
          rawMask |= this.Two;
        if ((virtualMask & OVRInput.Button.Three) != OVRInput.Button.None)
          rawMask |= this.Three;
        if ((virtualMask & OVRInput.Button.Four) != OVRInput.Button.None)
          rawMask |= this.Four;
        if ((virtualMask & OVRInput.Button.Start) != OVRInput.Button.None)
          rawMask |= this.Start;
        if ((virtualMask & OVRInput.Button.Back) != OVRInput.Button.None)
          rawMask |= this.Back;
        if ((virtualMask & OVRInput.Button.PrimaryShoulder) != OVRInput.Button.None)
          rawMask |= this.PrimaryShoulder;
        if ((virtualMask & OVRInput.Button.PrimaryIndexTrigger) != OVRInput.Button.None)
          rawMask |= this.PrimaryIndexTrigger;
        if ((virtualMask & OVRInput.Button.PrimaryHandTrigger) != OVRInput.Button.None)
          rawMask |= this.PrimaryHandTrigger;
        if ((virtualMask & OVRInput.Button.PrimaryThumbstick) != OVRInput.Button.None)
          rawMask |= this.PrimaryThumbstick;
        if ((virtualMask & OVRInput.Button.PrimaryThumbstickUp) != OVRInput.Button.None)
          rawMask |= this.PrimaryThumbstickUp;
        if ((virtualMask & OVRInput.Button.PrimaryThumbstickDown) != OVRInput.Button.None)
          rawMask |= this.PrimaryThumbstickDown;
        if ((virtualMask & OVRInput.Button.PrimaryThumbstickLeft) != OVRInput.Button.None)
          rawMask |= this.PrimaryThumbstickLeft;
        if ((virtualMask & OVRInput.Button.PrimaryThumbstickRight) != OVRInput.Button.None)
          rawMask |= this.PrimaryThumbstickRight;
        if ((virtualMask & OVRInput.Button.PrimaryTouchpad) != OVRInput.Button.None)
          rawMask |= this.PrimaryTouchpad;
        if ((virtualMask & OVRInput.Button.SecondaryShoulder) != OVRInput.Button.None)
          rawMask |= this.SecondaryShoulder;
        if ((virtualMask & OVRInput.Button.SecondaryIndexTrigger) != OVRInput.Button.None)
          rawMask |= this.SecondaryIndexTrigger;
        if ((virtualMask & OVRInput.Button.SecondaryHandTrigger) != OVRInput.Button.None)
          rawMask |= this.SecondaryHandTrigger;
        if ((virtualMask & OVRInput.Button.SecondaryThumbstick) != OVRInput.Button.None)
          rawMask |= this.SecondaryThumbstick;
        if ((virtualMask & OVRInput.Button.SecondaryThumbstickUp) != OVRInput.Button.None)
          rawMask |= this.SecondaryThumbstickUp;
        if ((virtualMask & OVRInput.Button.SecondaryThumbstickDown) != OVRInput.Button.None)
          rawMask |= this.SecondaryThumbstickDown;
        if ((virtualMask & OVRInput.Button.SecondaryThumbstickLeft) != OVRInput.Button.None)
          rawMask |= this.SecondaryThumbstickLeft;
        if ((virtualMask & OVRInput.Button.SecondaryThumbstickRight) != OVRInput.Button.None)
          rawMask |= this.SecondaryThumbstickRight;
        if ((virtualMask & OVRInput.Button.SecondaryTouchpad) != OVRInput.Button.None)
          rawMask |= this.SecondaryTouchpad;
        if ((virtualMask & OVRInput.Button.DpadUp) != OVRInput.Button.None)
          rawMask |= this.DpadUp;
        if ((virtualMask & OVRInput.Button.DpadDown) != OVRInput.Button.None)
          rawMask |= this.DpadDown;
        if ((virtualMask & OVRInput.Button.DpadLeft) != OVRInput.Button.None)
          rawMask |= this.DpadLeft;
        if ((virtualMask & OVRInput.Button.DpadRight) != OVRInput.Button.None)
          rawMask |= this.DpadRight;
        if ((virtualMask & OVRInput.Button.Up) != OVRInput.Button.None)
          rawMask |= this.Up;
        if ((virtualMask & OVRInput.Button.Down) != OVRInput.Button.None)
          rawMask |= this.Down;
        if ((virtualMask & OVRInput.Button.Left) != OVRInput.Button.None)
          rawMask |= this.Left;
        if ((virtualMask & OVRInput.Button.Right) != OVRInput.Button.None)
          rawMask |= this.Right;
        return rawMask;
      }
    }

    public class VirtualTouchMap
    {
      public OVRInput.RawTouch None;
      public OVRInput.RawTouch One;
      public OVRInput.RawTouch Two;
      public OVRInput.RawTouch Three;
      public OVRInput.RawTouch Four;
      public OVRInput.RawTouch PrimaryIndexTrigger;
      public OVRInput.RawTouch PrimaryThumbstick;
      public OVRInput.RawTouch PrimaryThumbRest;
      public OVRInput.RawTouch PrimaryTouchpad;
      public OVRInput.RawTouch SecondaryIndexTrigger;
      public OVRInput.RawTouch SecondaryThumbstick;
      public OVRInput.RawTouch SecondaryThumbRest;
      public OVRInput.RawTouch SecondaryTouchpad;

      public OVRInput.RawTouch ToRawMask(OVRInput.Touch virtualMask)
      {
        OVRInput.RawTouch rawMask = OVRInput.RawTouch.None;
        if (virtualMask == OVRInput.Touch.None)
          return OVRInput.RawTouch.None;
        if ((virtualMask & OVRInput.Touch.One) != OVRInput.Touch.None)
          rawMask |= this.One;
        if ((virtualMask & OVRInput.Touch.Two) != OVRInput.Touch.None)
          rawMask |= this.Two;
        if ((virtualMask & OVRInput.Touch.Three) != OVRInput.Touch.None)
          rawMask |= this.Three;
        if ((virtualMask & OVRInput.Touch.Four) != OVRInput.Touch.None)
          rawMask |= this.Four;
        if ((virtualMask & OVRInput.Touch.PrimaryIndexTrigger) != OVRInput.Touch.None)
          rawMask |= this.PrimaryIndexTrigger;
        if ((virtualMask & OVRInput.Touch.PrimaryThumbstick) != OVRInput.Touch.None)
          rawMask |= this.PrimaryThumbstick;
        if ((virtualMask & OVRInput.Touch.PrimaryThumbRest) != OVRInput.Touch.None)
          rawMask |= this.PrimaryThumbRest;
        if ((virtualMask & OVRInput.Touch.PrimaryTouchpad) != OVRInput.Touch.None)
          rawMask |= this.PrimaryTouchpad;
        if ((virtualMask & OVRInput.Touch.SecondaryIndexTrigger) != OVRInput.Touch.None)
          rawMask |= this.SecondaryIndexTrigger;
        if ((virtualMask & OVRInput.Touch.SecondaryThumbstick) != OVRInput.Touch.None)
          rawMask |= this.SecondaryThumbstick;
        if ((virtualMask & OVRInput.Touch.SecondaryThumbRest) != OVRInput.Touch.None)
          rawMask |= this.SecondaryThumbRest;
        if ((virtualMask & OVRInput.Touch.SecondaryTouchpad) != OVRInput.Touch.None)
          rawMask |= this.SecondaryTouchpad;
        return rawMask;
      }
    }

    public class VirtualNearTouchMap
    {
      public OVRInput.RawNearTouch None;
      public OVRInput.RawNearTouch PrimaryIndexTrigger;
      public OVRInput.RawNearTouch PrimaryThumbButtons;
      public OVRInput.RawNearTouch SecondaryIndexTrigger;
      public OVRInput.RawNearTouch SecondaryThumbButtons;

      public OVRInput.RawNearTouch ToRawMask(OVRInput.NearTouch virtualMask)
      {
        OVRInput.RawNearTouch rawMask = OVRInput.RawNearTouch.None;
        if (virtualMask == OVRInput.NearTouch.None)
          return OVRInput.RawNearTouch.None;
        if ((virtualMask & OVRInput.NearTouch.PrimaryIndexTrigger) != OVRInput.NearTouch.None)
          rawMask |= this.PrimaryIndexTrigger;
        if ((virtualMask & OVRInput.NearTouch.PrimaryThumbButtons) != OVRInput.NearTouch.None)
          rawMask |= this.PrimaryThumbButtons;
        if ((virtualMask & OVRInput.NearTouch.SecondaryIndexTrigger) != OVRInput.NearTouch.None)
          rawMask |= this.SecondaryIndexTrigger;
        if ((virtualMask & OVRInput.NearTouch.SecondaryThumbButtons) != OVRInput.NearTouch.None)
          rawMask |= this.SecondaryThumbButtons;
        return rawMask;
      }
    }

    public class VirtualAxis1DMap
    {
      public OVRInput.RawAxis1D None;
      public OVRInput.RawAxis1D PrimaryIndexTrigger;
      public OVRInput.RawAxis1D PrimaryHandTrigger;
      public OVRInput.RawAxis1D SecondaryIndexTrigger;
      public OVRInput.RawAxis1D SecondaryHandTrigger;

      public OVRInput.RawAxis1D ToRawMask(OVRInput.Axis1D virtualMask)
      {
        OVRInput.RawAxis1D rawMask = OVRInput.RawAxis1D.None;
        if (virtualMask == OVRInput.Axis1D.None)
          return OVRInput.RawAxis1D.None;
        if ((virtualMask & OVRInput.Axis1D.PrimaryIndexTrigger) != OVRInput.Axis1D.None)
          rawMask |= this.PrimaryIndexTrigger;
        if ((virtualMask & OVRInput.Axis1D.PrimaryHandTrigger) != OVRInput.Axis1D.None)
          rawMask |= this.PrimaryHandTrigger;
        if ((virtualMask & OVRInput.Axis1D.SecondaryIndexTrigger) != OVRInput.Axis1D.None)
          rawMask |= this.SecondaryIndexTrigger;
        if ((virtualMask & OVRInput.Axis1D.SecondaryHandTrigger) != OVRInput.Axis1D.None)
          rawMask |= this.SecondaryHandTrigger;
        return rawMask;
      }
    }

    public class VirtualAxis2DMap
    {
      public OVRInput.RawAxis2D None;
      public OVRInput.RawAxis2D PrimaryThumbstick;
      public OVRInput.RawAxis2D PrimaryTouchpad;
      public OVRInput.RawAxis2D SecondaryThumbstick;
      public OVRInput.RawAxis2D SecondaryTouchpad;

      public OVRInput.RawAxis2D ToRawMask(OVRInput.Axis2D virtualMask)
      {
        OVRInput.RawAxis2D rawMask = OVRInput.RawAxis2D.None;
        if (virtualMask == OVRInput.Axis2D.None)
          return OVRInput.RawAxis2D.None;
        if ((virtualMask & OVRInput.Axis2D.PrimaryThumbstick) != OVRInput.Axis2D.None)
          rawMask |= this.PrimaryThumbstick;
        if ((virtualMask & OVRInput.Axis2D.PrimaryTouchpad) != OVRInput.Axis2D.None)
          rawMask |= this.PrimaryTouchpad;
        if ((virtualMask & OVRInput.Axis2D.SecondaryThumbstick) != OVRInput.Axis2D.None)
          rawMask |= this.SecondaryThumbstick;
        if ((virtualMask & OVRInput.Axis2D.SecondaryTouchpad) != OVRInput.Axis2D.None)
          rawMask |= this.SecondaryTouchpad;
        return rawMask;
      }
    }
  }

  private class OVRControllerTouch : OVRInput.OVRControllerBase
  {
    public OVRControllerTouch() => this.controllerType = OVRInput.Controller.Touch;

    public override void ConfigureButtonMap()
    {
      this.buttonMap.None = OVRInput.RawButton.None;
      this.buttonMap.One = OVRInput.RawButton.A;
      this.buttonMap.Two = OVRInput.RawButton.B;
      this.buttonMap.Three = OVRInput.RawButton.X;
      this.buttonMap.Four = OVRInput.RawButton.Y;
      this.buttonMap.Start = OVRInput.RawButton.Start;
      this.buttonMap.Back = OVRInput.RawButton.None;
      this.buttonMap.PrimaryShoulder = OVRInput.RawButton.None;
      this.buttonMap.PrimaryIndexTrigger = OVRInput.RawButton.LIndexTrigger;
      this.buttonMap.PrimaryHandTrigger = OVRInput.RawButton.LHandTrigger;
      this.buttonMap.PrimaryThumbstick = OVRInput.RawButton.LThumbstick;
      this.buttonMap.PrimaryThumbstickUp = OVRInput.RawButton.LThumbstickUp;
      this.buttonMap.PrimaryThumbstickDown = OVRInput.RawButton.LThumbstickDown;
      this.buttonMap.PrimaryThumbstickLeft = OVRInput.RawButton.LThumbstickLeft;
      this.buttonMap.PrimaryThumbstickRight = OVRInput.RawButton.LThumbstickRight;
      this.buttonMap.PrimaryTouchpad = OVRInput.RawButton.None;
      this.buttonMap.SecondaryShoulder = OVRInput.RawButton.None;
      this.buttonMap.SecondaryIndexTrigger = OVRInput.RawButton.RIndexTrigger;
      this.buttonMap.SecondaryHandTrigger = OVRInput.RawButton.RHandTrigger;
      this.buttonMap.SecondaryThumbstick = OVRInput.RawButton.RThumbstick;
      this.buttonMap.SecondaryThumbstickUp = OVRInput.RawButton.RThumbstickUp;
      this.buttonMap.SecondaryThumbstickDown = OVRInput.RawButton.RThumbstickDown;
      this.buttonMap.SecondaryThumbstickLeft = OVRInput.RawButton.RThumbstickLeft;
      this.buttonMap.SecondaryThumbstickRight = OVRInput.RawButton.RThumbstickRight;
      this.buttonMap.SecondaryTouchpad = OVRInput.RawButton.None;
      this.buttonMap.DpadUp = OVRInput.RawButton.None;
      this.buttonMap.DpadDown = OVRInput.RawButton.None;
      this.buttonMap.DpadLeft = OVRInput.RawButton.None;
      this.buttonMap.DpadRight = OVRInput.RawButton.None;
      this.buttonMap.Up = OVRInput.RawButton.LThumbstickUp;
      this.buttonMap.Down = OVRInput.RawButton.LThumbstickDown;
      this.buttonMap.Left = OVRInput.RawButton.LThumbstickLeft;
      this.buttonMap.Right = OVRInput.RawButton.LThumbstickRight;
    }

    public override void ConfigureTouchMap()
    {
      this.touchMap.None = OVRInput.RawTouch.None;
      this.touchMap.One = OVRInput.RawTouch.A;
      this.touchMap.Two = OVRInput.RawTouch.B;
      this.touchMap.Three = OVRInput.RawTouch.X;
      this.touchMap.Four = OVRInput.RawTouch.Y;
      this.touchMap.PrimaryIndexTrigger = OVRInput.RawTouch.LIndexTrigger;
      this.touchMap.PrimaryThumbstick = OVRInput.RawTouch.LThumbstick;
      this.touchMap.PrimaryThumbRest = OVRInput.RawTouch.LThumbRest;
      this.touchMap.PrimaryTouchpad = OVRInput.RawTouch.None;
      this.touchMap.SecondaryIndexTrigger = OVRInput.RawTouch.RIndexTrigger;
      this.touchMap.SecondaryThumbstick = OVRInput.RawTouch.RThumbstick;
      this.touchMap.SecondaryThumbRest = OVRInput.RawTouch.RThumbRest;
      this.touchMap.SecondaryTouchpad = OVRInput.RawTouch.None;
    }

    public override void ConfigureNearTouchMap()
    {
      this.nearTouchMap.None = OVRInput.RawNearTouch.None;
      this.nearTouchMap.PrimaryIndexTrigger = OVRInput.RawNearTouch.LIndexTrigger;
      this.nearTouchMap.PrimaryThumbButtons = OVRInput.RawNearTouch.LThumbButtons;
      this.nearTouchMap.SecondaryIndexTrigger = OVRInput.RawNearTouch.RIndexTrigger;
      this.nearTouchMap.SecondaryThumbButtons = OVRInput.RawNearTouch.RThumbButtons;
    }

    public override void ConfigureAxis1DMap()
    {
      this.axis1DMap.None = OVRInput.RawAxis1D.None;
      this.axis1DMap.PrimaryIndexTrigger = OVRInput.RawAxis1D.LIndexTrigger;
      this.axis1DMap.PrimaryHandTrigger = OVRInput.RawAxis1D.LHandTrigger;
      this.axis1DMap.SecondaryIndexTrigger = OVRInput.RawAxis1D.RIndexTrigger;
      this.axis1DMap.SecondaryHandTrigger = OVRInput.RawAxis1D.RHandTrigger;
    }

    public override void ConfigureAxis2DMap()
    {
      this.axis2DMap.None = OVRInput.RawAxis2D.None;
      this.axis2DMap.PrimaryThumbstick = OVRInput.RawAxis2D.LThumbstick;
      this.axis2DMap.PrimaryTouchpad = OVRInput.RawAxis2D.None;
      this.axis2DMap.SecondaryThumbstick = OVRInput.RawAxis2D.RThumbstick;
      this.axis2DMap.SecondaryTouchpad = OVRInput.RawAxis2D.None;
    }
  }

  private class OVRControllerLTouch : OVRInput.OVRControllerBase
  {
    public OVRControllerLTouch() => this.controllerType = OVRInput.Controller.LTouch;

    public override void ConfigureButtonMap()
    {
      this.buttonMap.None = OVRInput.RawButton.None;
      this.buttonMap.One = OVRInput.RawButton.X;
      this.buttonMap.Two = OVRInput.RawButton.Y;
      this.buttonMap.Three = OVRInput.RawButton.None;
      this.buttonMap.Four = OVRInput.RawButton.None;
      this.buttonMap.Start = OVRInput.RawButton.Start;
      this.buttonMap.Back = OVRInput.RawButton.None;
      this.buttonMap.PrimaryShoulder = OVRInput.RawButton.None;
      this.buttonMap.PrimaryIndexTrigger = OVRInput.RawButton.LIndexTrigger;
      this.buttonMap.PrimaryHandTrigger = OVRInput.RawButton.LHandTrigger;
      this.buttonMap.PrimaryThumbstick = OVRInput.RawButton.LThumbstick;
      this.buttonMap.PrimaryThumbstickUp = OVRInput.RawButton.LThumbstickUp;
      this.buttonMap.PrimaryThumbstickDown = OVRInput.RawButton.LThumbstickDown;
      this.buttonMap.PrimaryThumbstickLeft = OVRInput.RawButton.LThumbstickLeft;
      this.buttonMap.PrimaryThumbstickRight = OVRInput.RawButton.LThumbstickRight;
      this.buttonMap.PrimaryTouchpad = OVRInput.RawButton.None;
      this.buttonMap.SecondaryShoulder = OVRInput.RawButton.None;
      this.buttonMap.SecondaryIndexTrigger = OVRInput.RawButton.None;
      this.buttonMap.SecondaryHandTrigger = OVRInput.RawButton.None;
      this.buttonMap.SecondaryThumbstick = OVRInput.RawButton.None;
      this.buttonMap.SecondaryThumbstickUp = OVRInput.RawButton.None;
      this.buttonMap.SecondaryThumbstickDown = OVRInput.RawButton.None;
      this.buttonMap.SecondaryThumbstickLeft = OVRInput.RawButton.None;
      this.buttonMap.SecondaryThumbstickRight = OVRInput.RawButton.None;
      this.buttonMap.SecondaryTouchpad = OVRInput.RawButton.None;
      this.buttonMap.DpadUp = OVRInput.RawButton.None;
      this.buttonMap.DpadDown = OVRInput.RawButton.None;
      this.buttonMap.DpadLeft = OVRInput.RawButton.None;
      this.buttonMap.DpadRight = OVRInput.RawButton.None;
      this.buttonMap.Up = OVRInput.RawButton.LThumbstickUp;
      this.buttonMap.Down = OVRInput.RawButton.LThumbstickDown;
      this.buttonMap.Left = OVRInput.RawButton.LThumbstickLeft;
      this.buttonMap.Right = OVRInput.RawButton.LThumbstickRight;
    }

    public override void ConfigureTouchMap()
    {
      this.touchMap.None = OVRInput.RawTouch.None;
      this.touchMap.One = OVRInput.RawTouch.X;
      this.touchMap.Two = OVRInput.RawTouch.Y;
      this.touchMap.Three = OVRInput.RawTouch.None;
      this.touchMap.Four = OVRInput.RawTouch.None;
      this.touchMap.PrimaryIndexTrigger = OVRInput.RawTouch.LIndexTrigger;
      this.touchMap.PrimaryThumbstick = OVRInput.RawTouch.LThumbstick;
      this.touchMap.PrimaryThumbRest = OVRInput.RawTouch.LThumbRest;
      this.touchMap.PrimaryTouchpad = OVRInput.RawTouch.None;
      this.touchMap.SecondaryIndexTrigger = OVRInput.RawTouch.None;
      this.touchMap.SecondaryThumbstick = OVRInput.RawTouch.None;
      this.touchMap.SecondaryThumbRest = OVRInput.RawTouch.None;
      this.touchMap.SecondaryTouchpad = OVRInput.RawTouch.None;
    }

    public override void ConfigureNearTouchMap()
    {
      this.nearTouchMap.None = OVRInput.RawNearTouch.None;
      this.nearTouchMap.PrimaryIndexTrigger = OVRInput.RawNearTouch.LIndexTrigger;
      this.nearTouchMap.PrimaryThumbButtons = OVRInput.RawNearTouch.LThumbButtons;
      this.nearTouchMap.SecondaryIndexTrigger = OVRInput.RawNearTouch.None;
      this.nearTouchMap.SecondaryThumbButtons = OVRInput.RawNearTouch.None;
    }

    public override void ConfigureAxis1DMap()
    {
      this.axis1DMap.None = OVRInput.RawAxis1D.None;
      this.axis1DMap.PrimaryIndexTrigger = OVRInput.RawAxis1D.LIndexTrigger;
      this.axis1DMap.PrimaryHandTrigger = OVRInput.RawAxis1D.LHandTrigger;
      this.axis1DMap.SecondaryIndexTrigger = OVRInput.RawAxis1D.None;
      this.axis1DMap.SecondaryHandTrigger = OVRInput.RawAxis1D.None;
    }

    public override void ConfigureAxis2DMap()
    {
      this.axis2DMap.None = OVRInput.RawAxis2D.None;
      this.axis2DMap.PrimaryThumbstick = OVRInput.RawAxis2D.LThumbstick;
      this.axis2DMap.PrimaryTouchpad = OVRInput.RawAxis2D.None;
      this.axis2DMap.SecondaryThumbstick = OVRInput.RawAxis2D.None;
      this.axis2DMap.SecondaryTouchpad = OVRInput.RawAxis2D.None;
    }
  }

  private class OVRControllerRTouch : OVRInput.OVRControllerBase
  {
    public OVRControllerRTouch() => this.controllerType = OVRInput.Controller.RTouch;

    public override void ConfigureButtonMap()
    {
      this.buttonMap.None = OVRInput.RawButton.None;
      this.buttonMap.One = OVRInput.RawButton.A;
      this.buttonMap.Two = OVRInput.RawButton.B;
      this.buttonMap.Three = OVRInput.RawButton.None;
      this.buttonMap.Four = OVRInput.RawButton.None;
      this.buttonMap.Start = OVRInput.RawButton.None;
      this.buttonMap.Back = OVRInput.RawButton.None;
      this.buttonMap.PrimaryShoulder = OVRInput.RawButton.None;
      this.buttonMap.PrimaryIndexTrigger = OVRInput.RawButton.RIndexTrigger;
      this.buttonMap.PrimaryHandTrigger = OVRInput.RawButton.RHandTrigger;
      this.buttonMap.PrimaryThumbstick = OVRInput.RawButton.RThumbstick;
      this.buttonMap.PrimaryThumbstickUp = OVRInput.RawButton.RThumbstickUp;
      this.buttonMap.PrimaryThumbstickDown = OVRInput.RawButton.RThumbstickDown;
      this.buttonMap.PrimaryThumbstickLeft = OVRInput.RawButton.RThumbstickLeft;
      this.buttonMap.PrimaryThumbstickRight = OVRInput.RawButton.RThumbstickRight;
      this.buttonMap.PrimaryTouchpad = OVRInput.RawButton.None;
      this.buttonMap.SecondaryShoulder = OVRInput.RawButton.None;
      this.buttonMap.SecondaryIndexTrigger = OVRInput.RawButton.None;
      this.buttonMap.SecondaryHandTrigger = OVRInput.RawButton.None;
      this.buttonMap.SecondaryThumbstick = OVRInput.RawButton.None;
      this.buttonMap.SecondaryThumbstickUp = OVRInput.RawButton.None;
      this.buttonMap.SecondaryThumbstickDown = OVRInput.RawButton.None;
      this.buttonMap.SecondaryThumbstickLeft = OVRInput.RawButton.None;
      this.buttonMap.SecondaryThumbstickRight = OVRInput.RawButton.None;
      this.buttonMap.SecondaryTouchpad = OVRInput.RawButton.None;
      this.buttonMap.DpadUp = OVRInput.RawButton.None;
      this.buttonMap.DpadDown = OVRInput.RawButton.None;
      this.buttonMap.DpadLeft = OVRInput.RawButton.None;
      this.buttonMap.DpadRight = OVRInput.RawButton.None;
      this.buttonMap.Up = OVRInput.RawButton.RThumbstickUp;
      this.buttonMap.Down = OVRInput.RawButton.RThumbstickDown;
      this.buttonMap.Left = OVRInput.RawButton.RThumbstickLeft;
      this.buttonMap.Right = OVRInput.RawButton.RThumbstickRight;
    }

    public override void ConfigureTouchMap()
    {
      this.touchMap.None = OVRInput.RawTouch.None;
      this.touchMap.One = OVRInput.RawTouch.A;
      this.touchMap.Two = OVRInput.RawTouch.B;
      this.touchMap.Three = OVRInput.RawTouch.None;
      this.touchMap.Four = OVRInput.RawTouch.None;
      this.touchMap.PrimaryIndexTrigger = OVRInput.RawTouch.RIndexTrigger;
      this.touchMap.PrimaryThumbstick = OVRInput.RawTouch.RThumbstick;
      this.touchMap.PrimaryThumbRest = OVRInput.RawTouch.RThumbRest;
      this.touchMap.PrimaryTouchpad = OVRInput.RawTouch.None;
      this.touchMap.SecondaryIndexTrigger = OVRInput.RawTouch.None;
      this.touchMap.SecondaryThumbstick = OVRInput.RawTouch.None;
      this.touchMap.SecondaryThumbRest = OVRInput.RawTouch.None;
      this.touchMap.SecondaryTouchpad = OVRInput.RawTouch.None;
    }

    public override void ConfigureNearTouchMap()
    {
      this.nearTouchMap.None = OVRInput.RawNearTouch.None;
      this.nearTouchMap.PrimaryIndexTrigger = OVRInput.RawNearTouch.RIndexTrigger;
      this.nearTouchMap.PrimaryThumbButtons = OVRInput.RawNearTouch.RThumbButtons;
      this.nearTouchMap.SecondaryIndexTrigger = OVRInput.RawNearTouch.None;
      this.nearTouchMap.SecondaryThumbButtons = OVRInput.RawNearTouch.None;
    }

    public override void ConfigureAxis1DMap()
    {
      this.axis1DMap.None = OVRInput.RawAxis1D.None;
      this.axis1DMap.PrimaryIndexTrigger = OVRInput.RawAxis1D.RIndexTrigger;
      this.axis1DMap.PrimaryHandTrigger = OVRInput.RawAxis1D.RHandTrigger;
      this.axis1DMap.SecondaryIndexTrigger = OVRInput.RawAxis1D.None;
      this.axis1DMap.SecondaryHandTrigger = OVRInput.RawAxis1D.None;
    }

    public override void ConfigureAxis2DMap()
    {
      this.axis2DMap.None = OVRInput.RawAxis2D.None;
      this.axis2DMap.PrimaryThumbstick = OVRInput.RawAxis2D.RThumbstick;
      this.axis2DMap.PrimaryTouchpad = OVRInput.RawAxis2D.None;
      this.axis2DMap.SecondaryThumbstick = OVRInput.RawAxis2D.None;
      this.axis2DMap.SecondaryTouchpad = OVRInput.RawAxis2D.None;
    }
  }

  private class OVRControllerRemote : OVRInput.OVRControllerBase
  {
    public OVRControllerRemote() => this.controllerType = OVRInput.Controller.Remote;

    public override void ConfigureButtonMap()
    {
      this.buttonMap.None = OVRInput.RawButton.None;
      this.buttonMap.One = OVRInput.RawButton.Start;
      this.buttonMap.Two = OVRInput.RawButton.Back;
      this.buttonMap.Three = OVRInput.RawButton.None;
      this.buttonMap.Four = OVRInput.RawButton.None;
      this.buttonMap.Start = OVRInput.RawButton.Start;
      this.buttonMap.Back = OVRInput.RawButton.Back;
      this.buttonMap.PrimaryShoulder = OVRInput.RawButton.None;
      this.buttonMap.PrimaryIndexTrigger = OVRInput.RawButton.None;
      this.buttonMap.PrimaryHandTrigger = OVRInput.RawButton.None;
      this.buttonMap.PrimaryThumbstick = OVRInput.RawButton.None;
      this.buttonMap.PrimaryThumbstickUp = OVRInput.RawButton.None;
      this.buttonMap.PrimaryThumbstickDown = OVRInput.RawButton.None;
      this.buttonMap.PrimaryThumbstickLeft = OVRInput.RawButton.None;
      this.buttonMap.PrimaryThumbstickRight = OVRInput.RawButton.None;
      this.buttonMap.PrimaryTouchpad = OVRInput.RawButton.None;
      this.buttonMap.SecondaryShoulder = OVRInput.RawButton.None;
      this.buttonMap.SecondaryIndexTrigger = OVRInput.RawButton.None;
      this.buttonMap.SecondaryHandTrigger = OVRInput.RawButton.None;
      this.buttonMap.SecondaryThumbstick = OVRInput.RawButton.None;
      this.buttonMap.SecondaryThumbstickUp = OVRInput.RawButton.None;
      this.buttonMap.SecondaryThumbstickDown = OVRInput.RawButton.None;
      this.buttonMap.SecondaryThumbstickLeft = OVRInput.RawButton.None;
      this.buttonMap.SecondaryThumbstickRight = OVRInput.RawButton.None;
      this.buttonMap.SecondaryTouchpad = OVRInput.RawButton.None;
      this.buttonMap.DpadUp = OVRInput.RawButton.DpadUp;
      this.buttonMap.DpadDown = OVRInput.RawButton.DpadDown;
      this.buttonMap.DpadLeft = OVRInput.RawButton.DpadLeft;
      this.buttonMap.DpadRight = OVRInput.RawButton.DpadRight;
      this.buttonMap.Up = OVRInput.RawButton.DpadUp;
      this.buttonMap.Down = OVRInput.RawButton.DpadDown;
      this.buttonMap.Left = OVRInput.RawButton.DpadLeft;
      this.buttonMap.Right = OVRInput.RawButton.DpadRight;
    }

    public override void ConfigureTouchMap()
    {
      this.touchMap.None = OVRInput.RawTouch.None;
      this.touchMap.One = OVRInput.RawTouch.None;
      this.touchMap.Two = OVRInput.RawTouch.None;
      this.touchMap.Three = OVRInput.RawTouch.None;
      this.touchMap.Four = OVRInput.RawTouch.None;
      this.touchMap.PrimaryIndexTrigger = OVRInput.RawTouch.None;
      this.touchMap.PrimaryThumbstick = OVRInput.RawTouch.None;
      this.touchMap.PrimaryThumbRest = OVRInput.RawTouch.None;
      this.touchMap.PrimaryTouchpad = OVRInput.RawTouch.None;
      this.touchMap.SecondaryIndexTrigger = OVRInput.RawTouch.None;
      this.touchMap.SecondaryThumbstick = OVRInput.RawTouch.None;
      this.touchMap.SecondaryThumbRest = OVRInput.RawTouch.None;
      this.touchMap.SecondaryTouchpad = OVRInput.RawTouch.None;
    }

    public override void ConfigureNearTouchMap()
    {
      this.nearTouchMap.None = OVRInput.RawNearTouch.None;
      this.nearTouchMap.PrimaryIndexTrigger = OVRInput.RawNearTouch.None;
      this.nearTouchMap.PrimaryThumbButtons = OVRInput.RawNearTouch.None;
      this.nearTouchMap.SecondaryIndexTrigger = OVRInput.RawNearTouch.None;
      this.nearTouchMap.SecondaryThumbButtons = OVRInput.RawNearTouch.None;
    }

    public override void ConfigureAxis1DMap()
    {
      this.axis1DMap.None = OVRInput.RawAxis1D.None;
      this.axis1DMap.PrimaryIndexTrigger = OVRInput.RawAxis1D.None;
      this.axis1DMap.PrimaryHandTrigger = OVRInput.RawAxis1D.None;
      this.axis1DMap.SecondaryIndexTrigger = OVRInput.RawAxis1D.None;
      this.axis1DMap.SecondaryHandTrigger = OVRInput.RawAxis1D.None;
    }

    public override void ConfigureAxis2DMap()
    {
      this.axis2DMap.None = OVRInput.RawAxis2D.None;
      this.axis2DMap.PrimaryThumbstick = OVRInput.RawAxis2D.None;
      this.axis2DMap.PrimaryTouchpad = OVRInput.RawAxis2D.None;
      this.axis2DMap.SecondaryThumbstick = OVRInput.RawAxis2D.None;
      this.axis2DMap.SecondaryTouchpad = OVRInput.RawAxis2D.None;
    }
  }

  private class OVRControllerGamepadPC : OVRInput.OVRControllerBase
  {
    public OVRControllerGamepadPC() => this.controllerType = OVRInput.Controller.Gamepad;

    public override void ConfigureButtonMap()
    {
      this.buttonMap.None = OVRInput.RawButton.None;
      this.buttonMap.One = OVRInput.RawButton.A;
      this.buttonMap.Two = OVRInput.RawButton.B;
      this.buttonMap.Three = OVRInput.RawButton.X;
      this.buttonMap.Four = OVRInput.RawButton.Y;
      this.buttonMap.Start = OVRInput.RawButton.Start;
      this.buttonMap.Back = OVRInput.RawButton.Back;
      this.buttonMap.PrimaryShoulder = OVRInput.RawButton.LShoulder;
      this.buttonMap.PrimaryIndexTrigger = OVRInput.RawButton.LIndexTrigger;
      this.buttonMap.PrimaryHandTrigger = OVRInput.RawButton.None;
      this.buttonMap.PrimaryThumbstick = OVRInput.RawButton.LThumbstick;
      this.buttonMap.PrimaryThumbstickUp = OVRInput.RawButton.LThumbstickUp;
      this.buttonMap.PrimaryThumbstickDown = OVRInput.RawButton.LThumbstickDown;
      this.buttonMap.PrimaryThumbstickLeft = OVRInput.RawButton.LThumbstickLeft;
      this.buttonMap.PrimaryThumbstickRight = OVRInput.RawButton.LThumbstickRight;
      this.buttonMap.PrimaryTouchpad = OVRInput.RawButton.None;
      this.buttonMap.SecondaryShoulder = OVRInput.RawButton.RShoulder;
      this.buttonMap.SecondaryIndexTrigger = OVRInput.RawButton.RIndexTrigger;
      this.buttonMap.SecondaryHandTrigger = OVRInput.RawButton.None;
      this.buttonMap.SecondaryThumbstick = OVRInput.RawButton.RThumbstick;
      this.buttonMap.SecondaryThumbstickUp = OVRInput.RawButton.RThumbstickUp;
      this.buttonMap.SecondaryThumbstickDown = OVRInput.RawButton.RThumbstickDown;
      this.buttonMap.SecondaryThumbstickLeft = OVRInput.RawButton.RThumbstickLeft;
      this.buttonMap.SecondaryThumbstickRight = OVRInput.RawButton.RThumbstickRight;
      this.buttonMap.SecondaryTouchpad = OVRInput.RawButton.None;
      this.buttonMap.DpadUp = OVRInput.RawButton.DpadUp;
      this.buttonMap.DpadDown = OVRInput.RawButton.DpadDown;
      this.buttonMap.DpadLeft = OVRInput.RawButton.DpadLeft;
      this.buttonMap.DpadRight = OVRInput.RawButton.DpadRight;
      this.buttonMap.Up = OVRInput.RawButton.LThumbstickUp;
      this.buttonMap.Down = OVRInput.RawButton.LThumbstickDown;
      this.buttonMap.Left = OVRInput.RawButton.LThumbstickLeft;
      this.buttonMap.Right = OVRInput.RawButton.LThumbstickRight;
    }

    public override void ConfigureTouchMap()
    {
      this.touchMap.None = OVRInput.RawTouch.None;
      this.touchMap.One = OVRInput.RawTouch.None;
      this.touchMap.Two = OVRInput.RawTouch.None;
      this.touchMap.Three = OVRInput.RawTouch.None;
      this.touchMap.Four = OVRInput.RawTouch.None;
      this.touchMap.PrimaryIndexTrigger = OVRInput.RawTouch.None;
      this.touchMap.PrimaryThumbstick = OVRInput.RawTouch.None;
      this.touchMap.PrimaryThumbRest = OVRInput.RawTouch.None;
      this.touchMap.PrimaryTouchpad = OVRInput.RawTouch.None;
      this.touchMap.SecondaryIndexTrigger = OVRInput.RawTouch.None;
      this.touchMap.SecondaryThumbstick = OVRInput.RawTouch.None;
      this.touchMap.SecondaryThumbRest = OVRInput.RawTouch.None;
      this.touchMap.SecondaryTouchpad = OVRInput.RawTouch.None;
    }

    public override void ConfigureNearTouchMap()
    {
      this.nearTouchMap.None = OVRInput.RawNearTouch.None;
      this.nearTouchMap.PrimaryIndexTrigger = OVRInput.RawNearTouch.None;
      this.nearTouchMap.PrimaryThumbButtons = OVRInput.RawNearTouch.None;
      this.nearTouchMap.SecondaryIndexTrigger = OVRInput.RawNearTouch.None;
      this.nearTouchMap.SecondaryThumbButtons = OVRInput.RawNearTouch.None;
    }

    public override void ConfigureAxis1DMap()
    {
      this.axis1DMap.None = OVRInput.RawAxis1D.None;
      this.axis1DMap.PrimaryIndexTrigger = OVRInput.RawAxis1D.LIndexTrigger;
      this.axis1DMap.PrimaryHandTrigger = OVRInput.RawAxis1D.None;
      this.axis1DMap.SecondaryIndexTrigger = OVRInput.RawAxis1D.RIndexTrigger;
      this.axis1DMap.SecondaryHandTrigger = OVRInput.RawAxis1D.None;
    }

    public override void ConfigureAxis2DMap()
    {
      this.axis2DMap.None = OVRInput.RawAxis2D.None;
      this.axis2DMap.PrimaryThumbstick = OVRInput.RawAxis2D.LThumbstick;
      this.axis2DMap.PrimaryTouchpad = OVRInput.RawAxis2D.None;
      this.axis2DMap.SecondaryThumbstick = OVRInput.RawAxis2D.RThumbstick;
      this.axis2DMap.SecondaryTouchpad = OVRInput.RawAxis2D.None;
    }
  }

  private class OVRControllerGamepadMac : OVRInput.OVRControllerBase
  {
    private bool initialized;
    private const string DllName = "OVRGamepad";

    public OVRControllerGamepadMac()
    {
      this.controllerType = OVRInput.Controller.Gamepad;
      this.initialized = OVRInput.OVRControllerGamepadMac.OVR_GamepadController_Initialize();
    }

    ~OVRControllerGamepadMac()
    {
      if (!this.initialized)
        return;
      OVRInput.OVRControllerGamepadMac.OVR_GamepadController_Destroy();
    }

    public override OVRInput.Controller Update()
    {
      if (!this.initialized)
        return OVRInput.Controller.None;
      OVRPlugin.ControllerState4 controllerState4 = new OVRPlugin.ControllerState4();
      if (OVRInput.OVRControllerGamepadMac.OVR_GamepadController_Update())
        controllerState4.ConnectedControllers = 16U;
      if (OVRInput.OVRControllerGamepadMac.OVR_GamepadController_GetButton(0))
        controllerState4.Buttons |= 1U;
      if (OVRInput.OVRControllerGamepadMac.OVR_GamepadController_GetButton(1))
        controllerState4.Buttons |= 2U;
      if (OVRInput.OVRControllerGamepadMac.OVR_GamepadController_GetButton(2))
        controllerState4.Buttons |= 256U;
      if (OVRInput.OVRControllerGamepadMac.OVR_GamepadController_GetButton(3))
        controllerState4.Buttons |= 512U;
      if (OVRInput.OVRControllerGamepadMac.OVR_GamepadController_GetButton(4))
        controllerState4.Buttons |= 65536U;
      if (OVRInput.OVRControllerGamepadMac.OVR_GamepadController_GetButton(5))
        controllerState4.Buttons |= 131072U;
      if (OVRInput.OVRControllerGamepadMac.OVR_GamepadController_GetButton(6))
        controllerState4.Buttons |= 262144U;
      if (OVRInput.OVRControllerGamepadMac.OVR_GamepadController_GetButton(7))
        controllerState4.Buttons |= 524288U;
      if (OVRInput.OVRControllerGamepadMac.OVR_GamepadController_GetButton(8))
        controllerState4.Buttons |= 1048576U;
      if (OVRInput.OVRControllerGamepadMac.OVR_GamepadController_GetButton(9))
        controllerState4.Buttons |= 2097152U;
      if (OVRInput.OVRControllerGamepadMac.OVR_GamepadController_GetButton(10))
        controllerState4.Buttons |= 1024U;
      if (OVRInput.OVRControllerGamepadMac.OVR_GamepadController_GetButton(11))
        controllerState4.Buttons |= 4U;
      if (OVRInput.OVRControllerGamepadMac.OVR_GamepadController_GetButton(12))
        controllerState4.Buttons |= 2048U;
      if (OVRInput.OVRControllerGamepadMac.OVR_GamepadController_GetButton(13))
        controllerState4.Buttons |= 8U;
      controllerState4.LThumbstick.x = OVRInput.OVRControllerGamepadMac.OVR_GamepadController_GetAxis(0);
      controllerState4.LThumbstick.y = OVRInput.OVRControllerGamepadMac.OVR_GamepadController_GetAxis(1);
      controllerState4.RThumbstick.x = OVRInput.OVRControllerGamepadMac.OVR_GamepadController_GetAxis(2);
      controllerState4.RThumbstick.y = OVRInput.OVRControllerGamepadMac.OVR_GamepadController_GetAxis(3);
      controllerState4.LIndexTrigger = OVRInput.OVRControllerGamepadMac.OVR_GamepadController_GetAxis(4);
      controllerState4.RIndexTrigger = OVRInput.OVRControllerGamepadMac.OVR_GamepadController_GetAxis(5);
      if ((double) controllerState4.LIndexTrigger >= (double) OVRInput.AXIS_AS_BUTTON_THRESHOLD)
        controllerState4.Buttons |= 268435456U;
      if ((double) controllerState4.LHandTrigger >= (double) OVRInput.AXIS_AS_BUTTON_THRESHOLD)
        controllerState4.Buttons |= 536870912U;
      if ((double) controllerState4.LThumbstick.y >= (double) OVRInput.AXIS_AS_BUTTON_THRESHOLD)
        controllerState4.Buttons |= 16U;
      if ((double) controllerState4.LThumbstick.y <= -(double) OVRInput.AXIS_AS_BUTTON_THRESHOLD)
        controllerState4.Buttons |= 32U;
      if ((double) controllerState4.LThumbstick.x <= -(double) OVRInput.AXIS_AS_BUTTON_THRESHOLD)
        controllerState4.Buttons |= 64U;
      if ((double) controllerState4.LThumbstick.x >= (double) OVRInput.AXIS_AS_BUTTON_THRESHOLD)
        controllerState4.Buttons |= 128U;
      if ((double) controllerState4.RIndexTrigger >= (double) OVRInput.AXIS_AS_BUTTON_THRESHOLD)
        controllerState4.Buttons |= 67108864U;
      if ((double) controllerState4.RHandTrigger >= (double) OVRInput.AXIS_AS_BUTTON_THRESHOLD)
        controllerState4.Buttons |= 134217728U;
      if ((double) controllerState4.RThumbstick.y >= (double) OVRInput.AXIS_AS_BUTTON_THRESHOLD)
        controllerState4.Buttons |= 4096U;
      if ((double) controllerState4.RThumbstick.y <= -(double) OVRInput.AXIS_AS_BUTTON_THRESHOLD)
        controllerState4.Buttons |= 8192U;
      if ((double) controllerState4.RThumbstick.x <= -(double) OVRInput.AXIS_AS_BUTTON_THRESHOLD)
        controllerState4.Buttons |= 16384U;
      if ((double) controllerState4.RThumbstick.x >= (double) OVRInput.AXIS_AS_BUTTON_THRESHOLD)
        controllerState4.Buttons |= 32768U;
      this.previousState = this.currentState;
      this.currentState = controllerState4;
      return (OVRInput.Controller) this.currentState.ConnectedControllers & this.controllerType;
    }

    public override void ConfigureButtonMap()
    {
      this.buttonMap.None = OVRInput.RawButton.None;
      this.buttonMap.One = OVRInput.RawButton.A;
      this.buttonMap.Two = OVRInput.RawButton.B;
      this.buttonMap.Three = OVRInput.RawButton.X;
      this.buttonMap.Four = OVRInput.RawButton.Y;
      this.buttonMap.Start = OVRInput.RawButton.Start;
      this.buttonMap.Back = OVRInput.RawButton.Back;
      this.buttonMap.PrimaryShoulder = OVRInput.RawButton.LShoulder;
      this.buttonMap.PrimaryIndexTrigger = OVRInput.RawButton.LIndexTrigger;
      this.buttonMap.PrimaryHandTrigger = OVRInput.RawButton.None;
      this.buttonMap.PrimaryThumbstick = OVRInput.RawButton.LThumbstick;
      this.buttonMap.PrimaryThumbstickUp = OVRInput.RawButton.LThumbstickUp;
      this.buttonMap.PrimaryThumbstickDown = OVRInput.RawButton.LThumbstickDown;
      this.buttonMap.PrimaryThumbstickLeft = OVRInput.RawButton.LThumbstickLeft;
      this.buttonMap.PrimaryThumbstickRight = OVRInput.RawButton.LThumbstickRight;
      this.buttonMap.PrimaryTouchpad = OVRInput.RawButton.None;
      this.buttonMap.SecondaryShoulder = OVRInput.RawButton.RShoulder;
      this.buttonMap.SecondaryIndexTrigger = OVRInput.RawButton.RIndexTrigger;
      this.buttonMap.SecondaryHandTrigger = OVRInput.RawButton.None;
      this.buttonMap.SecondaryThumbstick = OVRInput.RawButton.RThumbstick;
      this.buttonMap.SecondaryThumbstickUp = OVRInput.RawButton.RThumbstickUp;
      this.buttonMap.SecondaryThumbstickDown = OVRInput.RawButton.RThumbstickDown;
      this.buttonMap.SecondaryThumbstickLeft = OVRInput.RawButton.RThumbstickLeft;
      this.buttonMap.SecondaryThumbstickRight = OVRInput.RawButton.RThumbstickRight;
      this.buttonMap.SecondaryTouchpad = OVRInput.RawButton.None;
      this.buttonMap.DpadUp = OVRInput.RawButton.DpadUp;
      this.buttonMap.DpadDown = OVRInput.RawButton.DpadDown;
      this.buttonMap.DpadLeft = OVRInput.RawButton.DpadLeft;
      this.buttonMap.DpadRight = OVRInput.RawButton.DpadRight;
      this.buttonMap.Up = OVRInput.RawButton.LThumbstickUp;
      this.buttonMap.Down = OVRInput.RawButton.LThumbstickDown;
      this.buttonMap.Left = OVRInput.RawButton.LThumbstickLeft;
      this.buttonMap.Right = OVRInput.RawButton.LThumbstickRight;
    }

    public override void ConfigureTouchMap()
    {
      this.touchMap.None = OVRInput.RawTouch.None;
      this.touchMap.One = OVRInput.RawTouch.None;
      this.touchMap.Two = OVRInput.RawTouch.None;
      this.touchMap.Three = OVRInput.RawTouch.None;
      this.touchMap.Four = OVRInput.RawTouch.None;
      this.touchMap.PrimaryIndexTrigger = OVRInput.RawTouch.None;
      this.touchMap.PrimaryThumbstick = OVRInput.RawTouch.None;
      this.touchMap.PrimaryThumbRest = OVRInput.RawTouch.None;
      this.touchMap.PrimaryTouchpad = OVRInput.RawTouch.None;
      this.touchMap.SecondaryIndexTrigger = OVRInput.RawTouch.None;
      this.touchMap.SecondaryThumbstick = OVRInput.RawTouch.None;
      this.touchMap.SecondaryThumbRest = OVRInput.RawTouch.None;
      this.touchMap.SecondaryTouchpad = OVRInput.RawTouch.None;
    }

    public override void ConfigureNearTouchMap()
    {
      this.nearTouchMap.None = OVRInput.RawNearTouch.None;
      this.nearTouchMap.PrimaryIndexTrigger = OVRInput.RawNearTouch.None;
      this.nearTouchMap.PrimaryThumbButtons = OVRInput.RawNearTouch.None;
      this.nearTouchMap.SecondaryIndexTrigger = OVRInput.RawNearTouch.None;
      this.nearTouchMap.SecondaryThumbButtons = OVRInput.RawNearTouch.None;
    }

    public override void ConfigureAxis1DMap()
    {
      this.axis1DMap.None = OVRInput.RawAxis1D.None;
      this.axis1DMap.PrimaryIndexTrigger = OVRInput.RawAxis1D.LIndexTrigger;
      this.axis1DMap.PrimaryHandTrigger = OVRInput.RawAxis1D.None;
      this.axis1DMap.SecondaryIndexTrigger = OVRInput.RawAxis1D.RIndexTrigger;
      this.axis1DMap.SecondaryHandTrigger = OVRInput.RawAxis1D.None;
    }

    public override void ConfigureAxis2DMap()
    {
      this.axis2DMap.None = OVRInput.RawAxis2D.None;
      this.axis2DMap.PrimaryThumbstick = OVRInput.RawAxis2D.LThumbstick;
      this.axis2DMap.PrimaryTouchpad = OVRInput.RawAxis2D.None;
      this.axis2DMap.SecondaryThumbstick = OVRInput.RawAxis2D.RThumbstick;
      this.axis2DMap.SecondaryTouchpad = OVRInput.RawAxis2D.None;
    }

    public override void SetControllerVibration(float frequency, float amplitude)
    {
      float frequency1 = frequency * 200f;
      OVRInput.OVRControllerGamepadMac.OVR_GamepadController_SetVibration(0, amplitude, frequency1);
    }

    [DllImport("OVRGamepad", CallingConvention = CallingConvention.Cdecl)]
    private static extern bool OVR_GamepadController_Initialize();

    [DllImport("OVRGamepad", CallingConvention = CallingConvention.Cdecl)]
    private static extern bool OVR_GamepadController_Destroy();

    [DllImport("OVRGamepad", CallingConvention = CallingConvention.Cdecl)]
    private static extern bool OVR_GamepadController_Update();

    [DllImport("OVRGamepad", CallingConvention = CallingConvention.Cdecl)]
    private static extern float OVR_GamepadController_GetAxis(int axis);

    [DllImport("OVRGamepad", CallingConvention = CallingConvention.Cdecl)]
    private static extern bool OVR_GamepadController_GetButton(int button);

    [DllImport("OVRGamepad", CallingConvention = CallingConvention.Cdecl)]
    private static extern bool OVR_GamepadController_SetVibration(
      int node,
      float strength,
      float frequency);

    private enum AxisGPC
    {
      None = -1, // 0xFFFFFFFF
      LeftXAxis = 0,
      LeftYAxis = 1,
      RightXAxis = 2,
      RightYAxis = 3,
      LeftTrigger = 4,
      RightTrigger = 5,
      DPad_X_Axis = 6,
      DPad_Y_Axis = 7,
      Max = 8,
    }

    public enum ButtonGPC
    {
      None = -1, // 0xFFFFFFFF
      A = 0,
      B = 1,
      X = 2,
      Y = 3,
      Up = 4,
      Down = 5,
      Left = 6,
      Right = 7,
      Start = 8,
      Back = 9,
      LStick = 10, // 0x0000000A
      RStick = 11, // 0x0000000B
      LeftShoulder = 12, // 0x0000000C
      RightShoulder = 13, // 0x0000000D
      Max = 14, // 0x0000000E
    }
  }

  private class OVRControllerGamepadAndroid : OVRInput.OVRControllerBase
  {
    public OVRControllerGamepadAndroid() => this.controllerType = OVRInput.Controller.Gamepad;

    public override void ConfigureButtonMap()
    {
      this.buttonMap.None = OVRInput.RawButton.None;
      this.buttonMap.One = OVRInput.RawButton.A;
      this.buttonMap.Two = OVRInput.RawButton.B;
      this.buttonMap.Three = OVRInput.RawButton.X;
      this.buttonMap.Four = OVRInput.RawButton.Y;
      this.buttonMap.Start = OVRInput.RawButton.Start;
      this.buttonMap.Back = OVRInput.RawButton.Back;
      this.buttonMap.PrimaryShoulder = OVRInput.RawButton.LShoulder;
      this.buttonMap.PrimaryIndexTrigger = OVRInput.RawButton.LIndexTrigger;
      this.buttonMap.PrimaryHandTrigger = OVRInput.RawButton.None;
      this.buttonMap.PrimaryThumbstick = OVRInput.RawButton.LThumbstick;
      this.buttonMap.PrimaryThumbstickUp = OVRInput.RawButton.LThumbstickUp;
      this.buttonMap.PrimaryThumbstickDown = OVRInput.RawButton.LThumbstickDown;
      this.buttonMap.PrimaryThumbstickLeft = OVRInput.RawButton.LThumbstickLeft;
      this.buttonMap.PrimaryThumbstickRight = OVRInput.RawButton.LThumbstickRight;
      this.buttonMap.PrimaryTouchpad = OVRInput.RawButton.None;
      this.buttonMap.SecondaryShoulder = OVRInput.RawButton.RShoulder;
      this.buttonMap.SecondaryIndexTrigger = OVRInput.RawButton.RIndexTrigger;
      this.buttonMap.SecondaryHandTrigger = OVRInput.RawButton.None;
      this.buttonMap.SecondaryThumbstick = OVRInput.RawButton.RThumbstick;
      this.buttonMap.SecondaryThumbstickUp = OVRInput.RawButton.RThumbstickUp;
      this.buttonMap.SecondaryThumbstickDown = OVRInput.RawButton.RThumbstickDown;
      this.buttonMap.SecondaryThumbstickLeft = OVRInput.RawButton.RThumbstickLeft;
      this.buttonMap.SecondaryThumbstickRight = OVRInput.RawButton.RThumbstickRight;
      this.buttonMap.SecondaryTouchpad = OVRInput.RawButton.None;
      this.buttonMap.DpadUp = OVRInput.RawButton.DpadUp;
      this.buttonMap.DpadDown = OVRInput.RawButton.DpadDown;
      this.buttonMap.DpadLeft = OVRInput.RawButton.DpadLeft;
      this.buttonMap.DpadRight = OVRInput.RawButton.DpadRight;
      this.buttonMap.Up = OVRInput.RawButton.LThumbstickUp;
      this.buttonMap.Down = OVRInput.RawButton.LThumbstickDown;
      this.buttonMap.Left = OVRInput.RawButton.LThumbstickLeft;
      this.buttonMap.Right = OVRInput.RawButton.LThumbstickRight;
    }

    public override void ConfigureTouchMap()
    {
      this.touchMap.None = OVRInput.RawTouch.None;
      this.touchMap.One = OVRInput.RawTouch.None;
      this.touchMap.Two = OVRInput.RawTouch.None;
      this.touchMap.Three = OVRInput.RawTouch.None;
      this.touchMap.Four = OVRInput.RawTouch.None;
      this.touchMap.PrimaryIndexTrigger = OVRInput.RawTouch.None;
      this.touchMap.PrimaryThumbstick = OVRInput.RawTouch.None;
      this.touchMap.PrimaryThumbRest = OVRInput.RawTouch.None;
      this.touchMap.PrimaryTouchpad = OVRInput.RawTouch.None;
      this.touchMap.SecondaryIndexTrigger = OVRInput.RawTouch.None;
      this.touchMap.SecondaryThumbstick = OVRInput.RawTouch.None;
      this.touchMap.SecondaryThumbRest = OVRInput.RawTouch.None;
      this.touchMap.SecondaryTouchpad = OVRInput.RawTouch.None;
    }

    public override void ConfigureNearTouchMap()
    {
      this.nearTouchMap.None = OVRInput.RawNearTouch.None;
      this.nearTouchMap.PrimaryIndexTrigger = OVRInput.RawNearTouch.None;
      this.nearTouchMap.PrimaryThumbButtons = OVRInput.RawNearTouch.None;
      this.nearTouchMap.SecondaryIndexTrigger = OVRInput.RawNearTouch.None;
      this.nearTouchMap.SecondaryThumbButtons = OVRInput.RawNearTouch.None;
    }

    public override void ConfigureAxis1DMap()
    {
      this.axis1DMap.None = OVRInput.RawAxis1D.None;
      this.axis1DMap.PrimaryIndexTrigger = OVRInput.RawAxis1D.LIndexTrigger;
      this.axis1DMap.PrimaryHandTrigger = OVRInput.RawAxis1D.None;
      this.axis1DMap.SecondaryIndexTrigger = OVRInput.RawAxis1D.RIndexTrigger;
      this.axis1DMap.SecondaryHandTrigger = OVRInput.RawAxis1D.None;
    }

    public override void ConfigureAxis2DMap()
    {
      this.axis2DMap.None = OVRInput.RawAxis2D.None;
      this.axis2DMap.PrimaryThumbstick = OVRInput.RawAxis2D.LThumbstick;
      this.axis2DMap.PrimaryTouchpad = OVRInput.RawAxis2D.None;
      this.axis2DMap.SecondaryThumbstick = OVRInput.RawAxis2D.RThumbstick;
      this.axis2DMap.SecondaryTouchpad = OVRInput.RawAxis2D.None;
    }
  }

  private class OVRControllerTouchpad : OVRInput.OVRControllerBase
  {
    private OVRPlugin.Vector2f moveAmount;
    private float maxTapMagnitude = 0.1f;
    private float minMoveMagnitude = 0.15f;

    public OVRControllerTouchpad() => this.controllerType = OVRInput.Controller.Touchpad;

    public override OVRInput.Controller Update()
    {
      int num = (int) base.Update();
      if (OVRInput.GetDown(OVRInput.RawTouch.LTouchpad, OVRInput.Controller.Touchpad))
        this.moveAmount = this.currentState.LTouchpad;
      if (!OVRInput.GetUp(OVRInput.RawTouch.LTouchpad, OVRInput.Controller.Touchpad))
        return (OVRInput.Controller) num;
      this.moveAmount.x = this.previousState.LTouchpad.x - this.moveAmount.x;
      this.moveAmount.y = this.previousState.LTouchpad.y - this.moveAmount.y;
      Vector2 vector2 = new Vector2(this.moveAmount.x, this.moveAmount.y);
      float magnitude = vector2.magnitude;
      if ((double) magnitude < (double) this.maxTapMagnitude)
      {
        this.currentState.Buttons |= 1048576U;
        this.currentState.Buttons |= 1073741824U;
        return (OVRInput.Controller) num;
      }
      if ((double) magnitude < (double) this.minMoveMagnitude)
        return (OVRInput.Controller) num;
      vector2.Normalize();
      if ((double) Mathf.Abs(vector2.x) > (double) Mathf.Abs(vector2.y))
      {
        if ((double) vector2.x < 0.0)
        {
          this.currentState.Buttons |= 262144U;
          return (OVRInput.Controller) num;
        }
        this.currentState.Buttons |= 524288U;
        return (OVRInput.Controller) num;
      }
      if ((double) vector2.y < 0.0)
      {
        this.currentState.Buttons |= 131072U;
        return (OVRInput.Controller) num;
      }
      this.currentState.Buttons |= 65536U;
      return (OVRInput.Controller) num;
    }

    public override void ConfigureButtonMap()
    {
      this.buttonMap.None = OVRInput.RawButton.None;
      this.buttonMap.One = OVRInput.RawButton.LTouchpad;
      this.buttonMap.Two = OVRInput.RawButton.Back;
      this.buttonMap.Three = OVRInput.RawButton.None;
      this.buttonMap.Four = OVRInput.RawButton.None;
      this.buttonMap.Start = OVRInput.RawButton.Start;
      this.buttonMap.Back = OVRInput.RawButton.Back;
      this.buttonMap.PrimaryShoulder = OVRInput.RawButton.None;
      this.buttonMap.PrimaryIndexTrigger = OVRInput.RawButton.None;
      this.buttonMap.PrimaryHandTrigger = OVRInput.RawButton.None;
      this.buttonMap.PrimaryThumbstick = OVRInput.RawButton.None;
      this.buttonMap.PrimaryThumbstickUp = OVRInput.RawButton.None;
      this.buttonMap.PrimaryThumbstickDown = OVRInput.RawButton.None;
      this.buttonMap.PrimaryThumbstickLeft = OVRInput.RawButton.None;
      this.buttonMap.PrimaryThumbstickRight = OVRInput.RawButton.None;
      this.buttonMap.PrimaryTouchpad = OVRInput.RawButton.LTouchpad;
      this.buttonMap.SecondaryShoulder = OVRInput.RawButton.None;
      this.buttonMap.SecondaryIndexTrigger = OVRInput.RawButton.None;
      this.buttonMap.SecondaryHandTrigger = OVRInput.RawButton.None;
      this.buttonMap.SecondaryThumbstick = OVRInput.RawButton.None;
      this.buttonMap.SecondaryThumbstickUp = OVRInput.RawButton.None;
      this.buttonMap.SecondaryThumbstickDown = OVRInput.RawButton.None;
      this.buttonMap.SecondaryThumbstickLeft = OVRInput.RawButton.None;
      this.buttonMap.SecondaryThumbstickRight = OVRInput.RawButton.None;
      this.buttonMap.SecondaryTouchpad = OVRInput.RawButton.None;
      this.buttonMap.DpadUp = OVRInput.RawButton.DpadUp;
      this.buttonMap.DpadDown = OVRInput.RawButton.DpadDown;
      this.buttonMap.DpadLeft = OVRInput.RawButton.DpadLeft;
      this.buttonMap.DpadRight = OVRInput.RawButton.DpadRight;
      this.buttonMap.Up = OVRInput.RawButton.DpadUp;
      this.buttonMap.Down = OVRInput.RawButton.DpadDown;
      this.buttonMap.Left = OVRInput.RawButton.DpadLeft;
      this.buttonMap.Right = OVRInput.RawButton.DpadRight;
    }

    public override void ConfigureTouchMap()
    {
      this.touchMap.None = OVRInput.RawTouch.None;
      this.touchMap.One = OVRInput.RawTouch.LTouchpad;
      this.touchMap.Two = OVRInput.RawTouch.None;
      this.touchMap.Three = OVRInput.RawTouch.None;
      this.touchMap.Four = OVRInput.RawTouch.None;
      this.touchMap.PrimaryIndexTrigger = OVRInput.RawTouch.None;
      this.touchMap.PrimaryThumbstick = OVRInput.RawTouch.None;
      this.touchMap.PrimaryThumbRest = OVRInput.RawTouch.None;
      this.touchMap.PrimaryTouchpad = OVRInput.RawTouch.LTouchpad;
      this.touchMap.SecondaryIndexTrigger = OVRInput.RawTouch.None;
      this.touchMap.SecondaryThumbstick = OVRInput.RawTouch.None;
      this.touchMap.SecondaryThumbRest = OVRInput.RawTouch.None;
      this.touchMap.SecondaryTouchpad = OVRInput.RawTouch.None;
    }

    public override void ConfigureNearTouchMap()
    {
      this.nearTouchMap.None = OVRInput.RawNearTouch.None;
      this.nearTouchMap.PrimaryIndexTrigger = OVRInput.RawNearTouch.None;
      this.nearTouchMap.PrimaryThumbButtons = OVRInput.RawNearTouch.None;
      this.nearTouchMap.SecondaryIndexTrigger = OVRInput.RawNearTouch.None;
      this.nearTouchMap.SecondaryThumbButtons = OVRInput.RawNearTouch.None;
    }

    public override void ConfigureAxis1DMap()
    {
      this.axis1DMap.None = OVRInput.RawAxis1D.None;
      this.axis1DMap.PrimaryIndexTrigger = OVRInput.RawAxis1D.None;
      this.axis1DMap.PrimaryHandTrigger = OVRInput.RawAxis1D.None;
      this.axis1DMap.SecondaryIndexTrigger = OVRInput.RawAxis1D.None;
      this.axis1DMap.SecondaryHandTrigger = OVRInput.RawAxis1D.None;
    }

    public override void ConfigureAxis2DMap()
    {
      this.axis2DMap.None = OVRInput.RawAxis2D.None;
      this.axis2DMap.PrimaryThumbstick = OVRInput.RawAxis2D.None;
      this.axis2DMap.PrimaryTouchpad = OVRInput.RawAxis2D.LTouchpad;
      this.axis2DMap.SecondaryThumbstick = OVRInput.RawAxis2D.None;
      this.axis2DMap.SecondaryTouchpad = OVRInput.RawAxis2D.None;
    }
  }

  private class OVRControllerLTrackedRemote : OVRInput.OVRControllerBase
  {
    private bool emitSwipe;
    private OVRPlugin.Vector2f moveAmount;
    private float minMoveMagnitude = 0.3f;

    public OVRControllerLTrackedRemote() => this.controllerType = OVRInput.Controller.LTrackedRemote;

    public override void ConfigureButtonMap()
    {
      this.buttonMap.None = OVRInput.RawButton.None;
      this.buttonMap.One = OVRInput.RawButton.LTouchpad;
      this.buttonMap.Two = OVRInput.RawButton.Back;
      this.buttonMap.Three = OVRInput.RawButton.None;
      this.buttonMap.Four = OVRInput.RawButton.None;
      this.buttonMap.Start = OVRInput.RawButton.Start;
      this.buttonMap.Back = OVRInput.RawButton.Back;
      this.buttonMap.PrimaryShoulder = OVRInput.RawButton.None;
      this.buttonMap.PrimaryIndexTrigger = OVRInput.RawButton.LIndexTrigger;
      this.buttonMap.PrimaryHandTrigger = OVRInput.RawButton.LHandTrigger;
      this.buttonMap.PrimaryThumbstick = OVRInput.RawButton.None;
      this.buttonMap.PrimaryThumbstickUp = OVRInput.RawButton.None;
      this.buttonMap.PrimaryThumbstickDown = OVRInput.RawButton.None;
      this.buttonMap.PrimaryThumbstickLeft = OVRInput.RawButton.None;
      this.buttonMap.PrimaryThumbstickRight = OVRInput.RawButton.None;
      this.buttonMap.PrimaryTouchpad = OVRInput.RawButton.LTouchpad;
      this.buttonMap.SecondaryShoulder = OVRInput.RawButton.None;
      this.buttonMap.SecondaryIndexTrigger = OVRInput.RawButton.None;
      this.buttonMap.SecondaryHandTrigger = OVRInput.RawButton.None;
      this.buttonMap.SecondaryThumbstick = OVRInput.RawButton.None;
      this.buttonMap.SecondaryThumbstickUp = OVRInput.RawButton.None;
      this.buttonMap.SecondaryThumbstickDown = OVRInput.RawButton.None;
      this.buttonMap.SecondaryThumbstickLeft = OVRInput.RawButton.None;
      this.buttonMap.SecondaryThumbstickRight = OVRInput.RawButton.None;
      this.buttonMap.SecondaryTouchpad = OVRInput.RawButton.None;
      this.buttonMap.DpadUp = OVRInput.RawButton.DpadUp;
      this.buttonMap.DpadDown = OVRInput.RawButton.DpadDown;
      this.buttonMap.DpadLeft = OVRInput.RawButton.DpadLeft;
      this.buttonMap.DpadRight = OVRInput.RawButton.DpadRight;
      this.buttonMap.Up = OVRInput.RawButton.DpadUp;
      this.buttonMap.Down = OVRInput.RawButton.DpadDown;
      this.buttonMap.Left = OVRInput.RawButton.DpadLeft;
      this.buttonMap.Right = OVRInput.RawButton.DpadRight;
    }

    public override void ConfigureTouchMap()
    {
      this.touchMap.None = OVRInput.RawTouch.None;
      this.touchMap.One = OVRInput.RawTouch.LTouchpad;
      this.touchMap.Two = OVRInput.RawTouch.None;
      this.touchMap.Three = OVRInput.RawTouch.None;
      this.touchMap.Four = OVRInput.RawTouch.None;
      this.touchMap.PrimaryIndexTrigger = OVRInput.RawTouch.LIndexTrigger;
      this.touchMap.PrimaryThumbstick = OVRInput.RawTouch.None;
      this.touchMap.PrimaryThumbRest = OVRInput.RawTouch.None;
      this.touchMap.PrimaryTouchpad = OVRInput.RawTouch.LTouchpad;
      this.touchMap.SecondaryIndexTrigger = OVRInput.RawTouch.None;
      this.touchMap.SecondaryThumbstick = OVRInput.RawTouch.None;
      this.touchMap.SecondaryThumbRest = OVRInput.RawTouch.None;
      this.touchMap.SecondaryTouchpad = OVRInput.RawTouch.None;
    }

    public override void ConfigureNearTouchMap()
    {
      this.nearTouchMap.None = OVRInput.RawNearTouch.None;
      this.nearTouchMap.PrimaryIndexTrigger = OVRInput.RawNearTouch.None;
      this.nearTouchMap.PrimaryThumbButtons = OVRInput.RawNearTouch.None;
      this.nearTouchMap.SecondaryIndexTrigger = OVRInput.RawNearTouch.None;
      this.nearTouchMap.SecondaryThumbButtons = OVRInput.RawNearTouch.None;
    }

    public override void ConfigureAxis1DMap()
    {
      this.axis1DMap.None = OVRInput.RawAxis1D.None;
      this.axis1DMap.PrimaryIndexTrigger = OVRInput.RawAxis1D.LIndexTrigger;
      this.axis1DMap.PrimaryHandTrigger = OVRInput.RawAxis1D.LHandTrigger;
      this.axis1DMap.SecondaryIndexTrigger = OVRInput.RawAxis1D.None;
      this.axis1DMap.SecondaryHandTrigger = OVRInput.RawAxis1D.None;
    }

    public override void ConfigureAxis2DMap()
    {
      this.axis2DMap.None = OVRInput.RawAxis2D.None;
      this.axis2DMap.PrimaryThumbstick = OVRInput.RawAxis2D.None;
      this.axis2DMap.PrimaryTouchpad = OVRInput.RawAxis2D.LTouchpad;
      this.axis2DMap.SecondaryThumbstick = OVRInput.RawAxis2D.None;
      this.axis2DMap.SecondaryTouchpad = OVRInput.RawAxis2D.None;
    }

    public override OVRInput.Controller Update()
    {
      int num = (int) base.Update();
      if (OVRInput.GetDown(OVRInput.RawTouch.LTouchpad, OVRInput.Controller.LTrackedRemote))
      {
        this.emitSwipe = true;
        this.moveAmount = this.currentState.LTouchpad;
      }
      if (OVRInput.GetDown(OVRInput.RawButton.LTouchpad, OVRInput.Controller.LTrackedRemote))
        this.emitSwipe = false;
      if (!OVRInput.GetUp(OVRInput.RawTouch.LTouchpad, OVRInput.Controller.LTrackedRemote))
        return (OVRInput.Controller) num;
      if (!this.emitSwipe)
        return (OVRInput.Controller) num;
      this.emitSwipe = false;
      this.moveAmount.x = this.previousState.LTouchpad.x - this.moveAmount.x;
      this.moveAmount.y = this.previousState.LTouchpad.y - this.moveAmount.y;
      Vector2 vector2 = new Vector2(this.moveAmount.x, this.moveAmount.y);
      if ((double) vector2.magnitude < (double) this.minMoveMagnitude)
        return (OVRInput.Controller) num;
      vector2.Normalize();
      if ((double) Mathf.Abs(vector2.x) > (double) Mathf.Abs(vector2.y))
      {
        if ((double) vector2.x < 0.0)
        {
          this.currentState.Buttons |= 262144U;
          return (OVRInput.Controller) num;
        }
        this.currentState.Buttons |= 524288U;
        return (OVRInput.Controller) num;
      }
      if ((double) vector2.y < 0.0)
      {
        this.currentState.Buttons |= 131072U;
        return (OVRInput.Controller) num;
      }
      this.currentState.Buttons |= 65536U;
      return (OVRInput.Controller) num;
    }

    public override bool WasRecentered() => (int) this.currentState.LRecenterCount != (int) this.previousState.LRecenterCount;

    public override byte GetRecenterCount() => this.currentState.LRecenterCount;

    public override byte GetBatteryPercentRemaining() => this.currentState.LBatteryPercentRemaining;
  }

  private class OVRControllerRTrackedRemote : OVRInput.OVRControllerBase
  {
    private bool emitSwipe;
    private OVRPlugin.Vector2f moveAmount;
    private float minMoveMagnitude = 0.3f;

    public OVRControllerRTrackedRemote() => this.controllerType = OVRInput.Controller.RTrackedRemote;

    public override void ConfigureButtonMap()
    {
      this.buttonMap.None = OVRInput.RawButton.None;
      this.buttonMap.One = OVRInput.RawButton.RTouchpad;
      this.buttonMap.Two = OVRInput.RawButton.Back;
      this.buttonMap.Three = OVRInput.RawButton.None;
      this.buttonMap.Four = OVRInput.RawButton.None;
      this.buttonMap.Start = OVRInput.RawButton.Start;
      this.buttonMap.Back = OVRInput.RawButton.Back;
      this.buttonMap.PrimaryShoulder = OVRInput.RawButton.None;
      this.buttonMap.PrimaryIndexTrigger = OVRInput.RawButton.RIndexTrigger;
      this.buttonMap.PrimaryHandTrigger = OVRInput.RawButton.RHandTrigger;
      this.buttonMap.PrimaryThumbstick = OVRInput.RawButton.None;
      this.buttonMap.PrimaryThumbstickUp = OVRInput.RawButton.None;
      this.buttonMap.PrimaryThumbstickDown = OVRInput.RawButton.None;
      this.buttonMap.PrimaryThumbstickLeft = OVRInput.RawButton.None;
      this.buttonMap.PrimaryThumbstickRight = OVRInput.RawButton.None;
      this.buttonMap.PrimaryTouchpad = OVRInput.RawButton.RTouchpad;
      this.buttonMap.SecondaryShoulder = OVRInput.RawButton.None;
      this.buttonMap.SecondaryIndexTrigger = OVRInput.RawButton.None;
      this.buttonMap.SecondaryHandTrigger = OVRInput.RawButton.None;
      this.buttonMap.SecondaryThumbstick = OVRInput.RawButton.None;
      this.buttonMap.SecondaryThumbstickUp = OVRInput.RawButton.None;
      this.buttonMap.SecondaryThumbstickDown = OVRInput.RawButton.None;
      this.buttonMap.SecondaryThumbstickLeft = OVRInput.RawButton.None;
      this.buttonMap.SecondaryThumbstickRight = OVRInput.RawButton.None;
      this.buttonMap.SecondaryTouchpad = OVRInput.RawButton.None;
      this.buttonMap.DpadUp = OVRInput.RawButton.DpadUp;
      this.buttonMap.DpadDown = OVRInput.RawButton.DpadDown;
      this.buttonMap.DpadLeft = OVRInput.RawButton.DpadLeft;
      this.buttonMap.DpadRight = OVRInput.RawButton.DpadRight;
      this.buttonMap.Up = OVRInput.RawButton.DpadUp;
      this.buttonMap.Down = OVRInput.RawButton.DpadDown;
      this.buttonMap.Left = OVRInput.RawButton.DpadLeft;
      this.buttonMap.Right = OVRInput.RawButton.DpadRight;
    }

    public override void ConfigureTouchMap()
    {
      this.touchMap.None = OVRInput.RawTouch.None;
      this.touchMap.One = OVRInput.RawTouch.RTouchpad;
      this.touchMap.Two = OVRInput.RawTouch.None;
      this.touchMap.Three = OVRInput.RawTouch.None;
      this.touchMap.Four = OVRInput.RawTouch.None;
      this.touchMap.PrimaryIndexTrigger = OVRInput.RawTouch.RIndexTrigger;
      this.touchMap.PrimaryThumbstick = OVRInput.RawTouch.None;
      this.touchMap.PrimaryThumbRest = OVRInput.RawTouch.None;
      this.touchMap.PrimaryTouchpad = OVRInput.RawTouch.RTouchpad;
      this.touchMap.SecondaryIndexTrigger = OVRInput.RawTouch.None;
      this.touchMap.SecondaryThumbstick = OVRInput.RawTouch.None;
      this.touchMap.SecondaryThumbRest = OVRInput.RawTouch.None;
      this.touchMap.SecondaryTouchpad = OVRInput.RawTouch.None;
    }

    public override void ConfigureNearTouchMap()
    {
      this.nearTouchMap.None = OVRInput.RawNearTouch.None;
      this.nearTouchMap.PrimaryIndexTrigger = OVRInput.RawNearTouch.None;
      this.nearTouchMap.PrimaryThumbButtons = OVRInput.RawNearTouch.None;
      this.nearTouchMap.SecondaryIndexTrigger = OVRInput.RawNearTouch.None;
      this.nearTouchMap.SecondaryThumbButtons = OVRInput.RawNearTouch.None;
    }

    public override void ConfigureAxis1DMap()
    {
      this.axis1DMap.None = OVRInput.RawAxis1D.None;
      this.axis1DMap.PrimaryIndexTrigger = OVRInput.RawAxis1D.RIndexTrigger;
      this.axis1DMap.PrimaryHandTrigger = OVRInput.RawAxis1D.RHandTrigger;
      this.axis1DMap.SecondaryIndexTrigger = OVRInput.RawAxis1D.None;
      this.axis1DMap.SecondaryHandTrigger = OVRInput.RawAxis1D.None;
    }

    public override void ConfigureAxis2DMap()
    {
      this.axis2DMap.None = OVRInput.RawAxis2D.None;
      this.axis2DMap.PrimaryThumbstick = OVRInput.RawAxis2D.None;
      this.axis2DMap.PrimaryTouchpad = OVRInput.RawAxis2D.RTouchpad;
      this.axis2DMap.SecondaryThumbstick = OVRInput.RawAxis2D.None;
      this.axis2DMap.SecondaryTouchpad = OVRInput.RawAxis2D.None;
    }

    public override OVRInput.Controller Update()
    {
      int num = (int) base.Update();
      if (OVRInput.GetDown(OVRInput.RawTouch.RTouchpad, OVRInput.Controller.RTrackedRemote))
      {
        this.emitSwipe = true;
        this.moveAmount = this.currentState.RTouchpad;
      }
      if (OVRInput.GetDown(OVRInput.RawButton.RTouchpad, OVRInput.Controller.RTrackedRemote))
        this.emitSwipe = false;
      if (!OVRInput.GetUp(OVRInput.RawTouch.RTouchpad, OVRInput.Controller.RTrackedRemote))
        return (OVRInput.Controller) num;
      if (!this.emitSwipe)
        return (OVRInput.Controller) num;
      this.emitSwipe = false;
      this.moveAmount.x = this.previousState.RTouchpad.x - this.moveAmount.x;
      this.moveAmount.y = this.previousState.RTouchpad.y - this.moveAmount.y;
      Vector2 vector2 = new Vector2(this.moveAmount.x, this.moveAmount.y);
      if ((double) vector2.magnitude < (double) this.minMoveMagnitude)
        return (OVRInput.Controller) num;
      vector2.Normalize();
      if ((double) Mathf.Abs(vector2.x) > (double) Mathf.Abs(vector2.y))
      {
        if ((double) vector2.x < 0.0)
        {
          this.currentState.Buttons |= 262144U;
          return (OVRInput.Controller) num;
        }
        this.currentState.Buttons |= 524288U;
        return (OVRInput.Controller) num;
      }
      if ((double) vector2.y < 0.0)
      {
        this.currentState.Buttons |= 131072U;
        return (OVRInput.Controller) num;
      }
      this.currentState.Buttons |= 65536U;
      return (OVRInput.Controller) num;
    }

    public override bool WasRecentered() => (int) this.currentState.RRecenterCount != (int) this.previousState.RRecenterCount;

    public override byte GetRecenterCount() => this.currentState.RRecenterCount;

    public override byte GetBatteryPercentRemaining() => this.currentState.RBatteryPercentRemaining;
  }
}
