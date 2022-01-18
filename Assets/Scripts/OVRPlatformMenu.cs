// Decompiled with JetBrains decompiler
// Type: OVRPlatformMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OVRPlatformMenu : MonoBehaviour
{
  private OVRInput.RawButton inputCode = OVRInput.RawButton.Back;
  public OVRPlatformMenu.eHandler shortPressHandler;
  public Func<bool> OnShortPress;
  private static Stack<string> sceneStack = new Stack<string>();

  private OVRPlatformMenu.eBackButtonAction HandleBackButtonState()
  {
    OVRPlatformMenu.eBackButtonAction backButtonAction = OVRPlatformMenu.eBackButtonAction.NONE;
    if (OVRInput.GetDown(this.inputCode))
      backButtonAction = OVRPlatformMenu.eBackButtonAction.SHORT_PRESS;
    return backButtonAction;
  }

  private void Awake()
  {
    if (this.shortPressHandler == OVRPlatformMenu.eHandler.RetreatOneLevel && this.OnShortPress == null)
      this.OnShortPress = new Func<bool>(OVRPlatformMenu.RetreatOneLevel);
    if (!OVRManager.isHmdPresent)
      this.enabled = false;
    else
      OVRPlatformMenu.sceneStack.Push(SceneManager.GetActiveScene().name);
  }

  private void ShowConfirmQuitMenu()
  {
  }

  private static bool RetreatOneLevel()
  {
    if (OVRPlatformMenu.sceneStack.Count <= 1)
      return true;
    SceneManager.LoadSceneAsync(OVRPlatformMenu.sceneStack.Pop());
    return false;
  }

  private void Update()
  {
  }

  public enum eHandler
  {
    ShowConfirmQuit,
    RetreatOneLevel,
  }

  private enum eBackButtonAction
  {
    NONE,
    SHORT_PRESS,
  }
}
