// Decompiled with JetBrains decompiler
// Type: OVRGearVrControllerTest
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class OVRGearVrControllerTest : MonoBehaviour
{
  public UnityEngine.UI.Text uiText;
  private List<OVRGearVrControllerTest.BoolMonitor> monitors;
  private StringBuilder data;
  private static string prevConnected = "";
  private static OVRGearVrControllerTest.BoolMonitor controllers = new OVRGearVrControllerTest.BoolMonitor("Controllers Changed", (OVRGearVrControllerTest.BoolMonitor.BoolGenerator) (() => OVRInput.GetConnectedControllers().ToString() != OVRGearVrControllerTest.prevConnected));

  private void Start()
  {
    if ((Object) this.uiText != (Object) null)
      this.uiText.supportRichText = false;
    this.data = new StringBuilder(2048);
    this.monitors = new List<OVRGearVrControllerTest.BoolMonitor>()
    {
      new OVRGearVrControllerTest.BoolMonitor("WasRecentered", (OVRGearVrControllerTest.BoolMonitor.BoolGenerator) (() => OVRInput.GetControllerWasRecentered())),
      new OVRGearVrControllerTest.BoolMonitor("One", (OVRGearVrControllerTest.BoolMonitor.BoolGenerator) (() => OVRInput.Get(OVRInput.Button.One))),
      new OVRGearVrControllerTest.BoolMonitor("OneDown", (OVRGearVrControllerTest.BoolMonitor.BoolGenerator) (() => OVRInput.GetDown(OVRInput.Button.One))),
      new OVRGearVrControllerTest.BoolMonitor("OneUp", (OVRGearVrControllerTest.BoolMonitor.BoolGenerator) (() => OVRInput.GetUp(OVRInput.Button.One))),
      new OVRGearVrControllerTest.BoolMonitor("One (Touch)", (OVRGearVrControllerTest.BoolMonitor.BoolGenerator) (() => OVRInput.Get(OVRInput.Touch.One))),
      new OVRGearVrControllerTest.BoolMonitor("OneDown (Touch)", (OVRGearVrControllerTest.BoolMonitor.BoolGenerator) (() => OVRInput.GetDown(OVRInput.Touch.One))),
      new OVRGearVrControllerTest.BoolMonitor("OneUp (Touch)", (OVRGearVrControllerTest.BoolMonitor.BoolGenerator) (() => OVRInput.GetUp(OVRInput.Touch.One))),
      new OVRGearVrControllerTest.BoolMonitor("Two", (OVRGearVrControllerTest.BoolMonitor.BoolGenerator) (() => OVRInput.Get(OVRInput.Button.Two))),
      new OVRGearVrControllerTest.BoolMonitor("TwoDown", (OVRGearVrControllerTest.BoolMonitor.BoolGenerator) (() => OVRInput.GetDown(OVRInput.Button.Two))),
      new OVRGearVrControllerTest.BoolMonitor("TwoUp", (OVRGearVrControllerTest.BoolMonitor.BoolGenerator) (() => OVRInput.GetUp(OVRInput.Button.Two))),
      new OVRGearVrControllerTest.BoolMonitor("PrimaryIndexTrigger", (OVRGearVrControllerTest.BoolMonitor.BoolGenerator) (() => OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))),
      new OVRGearVrControllerTest.BoolMonitor("PrimaryIndexTriggerDown", (OVRGearVrControllerTest.BoolMonitor.BoolGenerator) (() => OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))),
      new OVRGearVrControllerTest.BoolMonitor("PrimaryIndexTriggerUp", (OVRGearVrControllerTest.BoolMonitor.BoolGenerator) (() => OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger))),
      new OVRGearVrControllerTest.BoolMonitor("PrimaryIndexTrigger (Touch)", (OVRGearVrControllerTest.BoolMonitor.BoolGenerator) (() => OVRInput.Get(OVRInput.Touch.PrimaryIndexTrigger))),
      new OVRGearVrControllerTest.BoolMonitor("PrimaryIndexTriggerDown (Touch)", (OVRGearVrControllerTest.BoolMonitor.BoolGenerator) (() => OVRInput.GetDown(OVRInput.Touch.PrimaryIndexTrigger))),
      new OVRGearVrControllerTest.BoolMonitor("PrimaryIndexTriggerUp (Touch)", (OVRGearVrControllerTest.BoolMonitor.BoolGenerator) (() => OVRInput.GetUp(OVRInput.Touch.PrimaryIndexTrigger))),
      new OVRGearVrControllerTest.BoolMonitor("PrimaryHandTrigger", (OVRGearVrControllerTest.BoolMonitor.BoolGenerator) (() => OVRInput.Get(OVRInput.Button.PrimaryHandTrigger))),
      new OVRGearVrControllerTest.BoolMonitor("PrimaryHandTriggerDown", (OVRGearVrControllerTest.BoolMonitor.BoolGenerator) (() => OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger))),
      new OVRGearVrControllerTest.BoolMonitor("PrimaryHandTriggerUp", (OVRGearVrControllerTest.BoolMonitor.BoolGenerator) (() => OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger))),
      new OVRGearVrControllerTest.BoolMonitor("Up", (OVRGearVrControllerTest.BoolMonitor.BoolGenerator) (() => OVRInput.Get(OVRInput.Button.Up))),
      new OVRGearVrControllerTest.BoolMonitor("Down", (OVRGearVrControllerTest.BoolMonitor.BoolGenerator) (() => OVRInput.Get(OVRInput.Button.Down))),
      new OVRGearVrControllerTest.BoolMonitor("Left", (OVRGearVrControllerTest.BoolMonitor.BoolGenerator) (() => OVRInput.Get(OVRInput.Button.Left))),
      new OVRGearVrControllerTest.BoolMonitor("Right", (OVRGearVrControllerTest.BoolMonitor.BoolGenerator) (() => OVRInput.Get(OVRInput.Button.Right))),
      new OVRGearVrControllerTest.BoolMonitor("Touchpad (Click)", (OVRGearVrControllerTest.BoolMonitor.BoolGenerator) (() => OVRInput.Get(OVRInput.Button.PrimaryTouchpad))),
      new OVRGearVrControllerTest.BoolMonitor("TouchpadDown (Click)", (OVRGearVrControllerTest.BoolMonitor.BoolGenerator) (() => OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad))),
      new OVRGearVrControllerTest.BoolMonitor("TouchpadUp (Click)", (OVRGearVrControllerTest.BoolMonitor.BoolGenerator) (() => OVRInput.GetUp(OVRInput.Button.PrimaryTouchpad))),
      new OVRGearVrControllerTest.BoolMonitor("Touchpad (Touch)", (OVRGearVrControllerTest.BoolMonitor.BoolGenerator) (() => OVRInput.Get(OVRInput.Touch.PrimaryTouchpad))),
      new OVRGearVrControllerTest.BoolMonitor("TouchpadDown (Touch)", (OVRGearVrControllerTest.BoolMonitor.BoolGenerator) (() => OVRInput.GetDown(OVRInput.Touch.PrimaryTouchpad))),
      new OVRGearVrControllerTest.BoolMonitor("TouchpadUp (Touch)", (OVRGearVrControllerTest.BoolMonitor.BoolGenerator) (() => OVRInput.GetUp(OVRInput.Touch.PrimaryTouchpad))),
      new OVRGearVrControllerTest.BoolMonitor(nameof (Start), (OVRGearVrControllerTest.BoolMonitor.BoolGenerator) (() => OVRInput.Get(OVRInput.RawButton.Start))),
      new OVRGearVrControllerTest.BoolMonitor("StartDown", (OVRGearVrControllerTest.BoolMonitor.BoolGenerator) (() => OVRInput.GetDown(OVRInput.RawButton.Start))),
      new OVRGearVrControllerTest.BoolMonitor("StartUp", (OVRGearVrControllerTest.BoolMonitor.BoolGenerator) (() => OVRInput.GetUp(OVRInput.RawButton.Start))),
      new OVRGearVrControllerTest.BoolMonitor("Back", (OVRGearVrControllerTest.BoolMonitor.BoolGenerator) (() => OVRInput.Get(OVRInput.RawButton.Back))),
      new OVRGearVrControllerTest.BoolMonitor("BackDown", (OVRGearVrControllerTest.BoolMonitor.BoolGenerator) (() => OVRInput.GetDown(OVRInput.RawButton.Back))),
      new OVRGearVrControllerTest.BoolMonitor("BackUp", (OVRGearVrControllerTest.BoolMonitor.BoolGenerator) (() => OVRInput.GetUp(OVRInput.RawButton.Back))),
      new OVRGearVrControllerTest.BoolMonitor("A", (OVRGearVrControllerTest.BoolMonitor.BoolGenerator) (() => OVRInput.Get(OVRInput.RawButton.A))),
      new OVRGearVrControllerTest.BoolMonitor("ADown", (OVRGearVrControllerTest.BoolMonitor.BoolGenerator) (() => OVRInput.GetDown(OVRInput.RawButton.A))),
      new OVRGearVrControllerTest.BoolMonitor("AUp", (OVRGearVrControllerTest.BoolMonitor.BoolGenerator) (() => OVRInput.GetUp(OVRInput.RawButton.A)))
    };
  }

  private void Update()
  {
    OVRInput.Controller activeController = OVRInput.GetActiveController();
    this.data.Length = 0;
    this.data.AppendFormat("RecenterCount: {0}\n", (object) OVRInput.GetControllerRecenterCount());
    this.data.AppendFormat("Battery: {0}\n", (object) OVRInput.GetControllerBatteryPercentRemaining());
    this.data.AppendFormat("Framerate: {0:F2}\n", (object) OVRPlugin.GetAppFramerate());
    this.data.AppendFormat("Active: {0}\n", (object) activeController.ToString());
    string str = OVRInput.GetConnectedControllers().ToString();
    this.data.AppendFormat("Connected: {0}\n", (object) str);
    this.data.AppendFormat("PrevConnected: {0}\n", (object) OVRGearVrControllerTest.prevConnected);
    OVRGearVrControllerTest.controllers.Update();
    OVRGearVrControllerTest.controllers.AppendToStringBuilder(ref this.data);
    OVRGearVrControllerTest.prevConnected = str;
    Quaternion controllerRotation = OVRInput.GetLocalControllerRotation(activeController);
    this.data.AppendFormat("Orientation: ({0:F2}, {1:F2}, {2:F2}, {3:F2})\n", (object) controllerRotation.x, (object) controllerRotation.y, (object) controllerRotation.z, (object) controllerRotation.w);
    Vector3 controllerAngularVelocity = OVRInput.GetLocalControllerAngularVelocity(activeController);
    this.data.AppendFormat("AngVel: ({0:F2}, {1:F2}, {2:F2})\n", (object) controllerAngularVelocity.x, (object) controllerAngularVelocity.y, (object) controllerAngularVelocity.z);
    Vector3 angularAcceleration = OVRInput.GetLocalControllerAngularAcceleration(activeController);
    this.data.AppendFormat("AngAcc: ({0:F2}, {1:F2}, {2:F2})\n", (object) angularAcceleration.x, (object) angularAcceleration.y, (object) angularAcceleration.z);
    Vector3 controllerPosition = OVRInput.GetLocalControllerPosition(activeController);
    this.data.AppendFormat("Position: ({0:F2}, {1:F2}, {2:F2})\n", (object) controllerPosition.x, (object) controllerPosition.y, (object) controllerPosition.z);
    Vector3 controllerVelocity = OVRInput.GetLocalControllerVelocity(activeController);
    this.data.AppendFormat("Vel: ({0:F2}, {1:F2}, {2:F2})\n", (object) controllerVelocity.x, (object) controllerVelocity.y, (object) controllerVelocity.z);
    Vector3 controllerAcceleration = OVRInput.GetLocalControllerAcceleration(activeController);
    this.data.AppendFormat("Acc: ({0:F2}, {1:F2}, {2:F2})\n", (object) controllerAcceleration.x, (object) controllerAcceleration.y, (object) controllerAcceleration.z);
    Vector2 vector2_1 = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);
    this.data.AppendFormat("PrimaryTouchpad: ({0:F2}, {1:F2})\n", (object) vector2_1.x, (object) vector2_1.y);
    Vector2 vector2_2 = OVRInput.Get(OVRInput.Axis2D.SecondaryTouchpad);
    this.data.AppendFormat("SecondaryTouchpad: ({0:F2}, {1:F2})\n", (object) vector2_2.x, (object) vector2_2.y);
    this.data.AppendFormat("PrimaryIndexTriggerAxis1D: ({0:F2})\n", (object) OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger));
    this.data.AppendFormat("PrimaryHandTriggerAxis1D: ({0:F2})\n", (object) OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger));
    for (int index = 0; index < this.monitors.Count; ++index)
    {
      this.monitors[index].Update();
      this.monitors[index].AppendToStringBuilder(ref this.data);
    }
    if (!((Object) this.uiText != (Object) null))
      return;
    this.uiText.text = this.data.ToString();
  }

  public class BoolMonitor
  {
    private string m_name = "";
    private OVRGearVrControllerTest.BoolMonitor.BoolGenerator m_generator;
    private bool m_prevValue;
    private bool m_currentValue;
    private bool m_currentValueRecentlyChanged;
    private float m_displayTimeout;
    private float m_displayTimer;

    public BoolMonitor(
      string name,
      OVRGearVrControllerTest.BoolMonitor.BoolGenerator generator,
      float displayTimeout = 0.5f)
    {
      this.m_name = name;
      this.m_generator = generator;
      this.m_displayTimeout = displayTimeout;
    }

    public void Update()
    {
      this.m_prevValue = this.m_currentValue;
      this.m_currentValue = this.m_generator();
      if (this.m_currentValue != this.m_prevValue)
      {
        this.m_currentValueRecentlyChanged = true;
        this.m_displayTimer = this.m_displayTimeout;
      }
      if ((double) this.m_displayTimer <= 0.0)
        return;
      this.m_displayTimer -= Time.deltaTime;
      if ((double) this.m_displayTimer > 0.0)
        return;
      this.m_currentValueRecentlyChanged = false;
      this.m_displayTimer = 0.0f;
    }

    public void AppendToStringBuilder(ref StringBuilder sb)
    {
      sb.Append(this.m_name);
      if (this.m_currentValue && this.m_currentValueRecentlyChanged)
        sb.Append(": *True*\n");
      else if (this.m_currentValue)
        sb.Append(":  True \n");
      else if (!this.m_currentValue && this.m_currentValueRecentlyChanged)
      {
        sb.Append(": *False*\n");
      }
      else
      {
        if (this.m_currentValue)
          return;
        sb.Append(":  False \n");
      }
    }

    public delegate bool BoolGenerator();
  }
}
