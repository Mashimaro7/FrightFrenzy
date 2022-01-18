// Decompiled with JetBrains decompiler
// Type: Robot_GF_Test1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System.Diagnostics;
using UnityEngine;

public class Robot_GF_Test1 : RobotInterface3D
{
  public HingeJoint GF_armJoint;
  public HingeJoint GF_collector1;
  public HingeJoint GF_collector2;
  public HingeJoint GF_collector3;
  public ConfigurableJoint mainExtensionJoint;
  public ConfigurableJoint doorToMainStorage;
  public ConfigurableJoint mainStorageJoint;
  private long stateStartTime;
  private Stopwatch dumperStateTimer = new Stopwatch();
  private Robot_GF_Test1.dumperStates dumperState;
  private Stopwatch mainExtensionStateTimer = new Stopwatch();
  private Robot_GF_Test1.mainExtensionStates mainExtensionState;
  private Stopwatch mainStorageStateTimer = new Stopwatch();
  private Robot_GF_Test1.mainStorageStates mainStorageState;

  public void Awake() => this.info = "<Missing Robot Specific Function: TBD>" + this.info;

  public override void Update_Robot()
  {
    if (this.gamepad1_left_bumper)
      this.doorToMainStorage.targetPosition = new Vector3(-0.2f, 0.0f, 0.0f);
    if (this.gamepad1_right_bumper)
      this.doorToMainStorage.targetPosition = new Vector3(0.0f, 0.0f, 0.0f);
    if (this.gamepad1_dpad_up)
    {
      this.mainExtensionState = Robot_GF_Test1.mainExtensionStates.goingForwards;
      this.mainExtensionStateTimer.Reset();
      this.mainExtensionStateTimer.Start();
    }
    if (this.gamepad1_dpad_down)
    {
      this.mainExtensionState = Robot_GF_Test1.mainExtensionStates.resetting;
      this.mainExtensionStateTimer.Reset();
      this.mainExtensionStateTimer.Start();
    }
    if (this.mainExtensionState == Robot_GF_Test1.mainExtensionStates.goingForwards)
    {
      double num = 1.5;
      double x = (double) this.mainExtensionStateTimer.ElapsedMilliseconds / 1500.0 * num;
      if (x > num)
        x = num;
      this.mainExtensionJoint.targetPosition = new Vector3((float) x, 0.0f, 0.0f);
    }
    if (this.mainExtensionState == Robot_GF_Test1.mainExtensionStates.resetting)
    {
      double x = (1.0 - (double) this.mainExtensionStateTimer.ElapsedMilliseconds / 1500.0) * 1.5;
      if (x < 0.0)
        x = 0.0;
      this.mainExtensionJoint.targetPosition = new Vector3((float) x, 0.0f, 0.0f);
    }
    if (this.gamepad1_x)
    {
      this.dumperState = Robot_GF_Test1.dumperStates.dumping;
      this.dumperStateTimer.Reset();
      this.dumperStateTimer.Start();
    }
    if (this.gamepad1_b)
    {
      this.dumperState = Robot_GF_Test1.dumperStates.resetting;
      this.dumperStateTimer.Reset();
      this.dumperStateTimer.Start();
    }
    if (this.gamepad1_a)
    {
      this.GF_collector1.motor = this.GF_collector1.motor with
      {
        targetVelocity = 1000f
      };
      this.GF_collector2.motor = this.GF_collector2.motor with
      {
        targetVelocity = -1000f
      };
      this.GF_collector3.motor = this.GF_collector3.motor with
      {
        targetVelocity = -750f
      };
    }
    if (this.gamepad1_y)
    {
      this.GF_collector1.motor = this.GF_collector1.motor with
      {
        targetVelocity = -1000f
      };
      this.GF_collector2.motor = this.GF_collector2.motor with
      {
        targetVelocity = 1000f
      };
      this.GF_collector3.motor = this.GF_collector3.motor with
      {
        targetVelocity = 750f
      };
    }
    if (this.dumperState == Robot_GF_Test1.dumperStates.dumping)
    {
      JointSpring spring = this.GF_armJoint.spring;
      double num1 = 145.0;
      double num2 = (double) (145L * this.dumperStateTimer.ElapsedMilliseconds) / 1500.0;
      if (num2 > num1)
        num2 = num1;
      spring.targetPosition = (float) (int) num2;
      this.GF_armJoint.spring = spring;
    }
    if (this.dumperState == Robot_GF_Test1.dumperStates.resetting)
    {
      JointSpring spring = this.GF_armJoint.spring;
      double num = 145.0 * (1.0 - (double) this.dumperStateTimer.ElapsedMilliseconds / 1500.0);
      if (num < 0.0)
        num = 0.0;
      spring.targetPosition = (float) (int) num;
      this.GF_armJoint.spring = spring;
    }
    if (this.gamepad1_dpad_right)
    {
      this.mainStorageState = Robot_GF_Test1.mainStorageStates.goingForwards;
      this.mainStorageStateTimer.Reset();
      this.mainStorageStateTimer.Start();
    }
    if (this.gamepad1_dpad_left)
    {
      this.mainStorageState = Robot_GF_Test1.mainStorageStates.resetting;
      this.mainStorageStateTimer.Reset();
      this.mainStorageStateTimer.Start();
    }
    if (this.mainStorageState == Robot_GF_Test1.mainStorageStates.goingForwards)
    {
      double num = 1.5;
      double x = (double) this.mainStorageStateTimer.ElapsedMilliseconds / 1500.0 * num;
      if (x > num)
        x = num;
      this.mainStorageJoint.targetPosition = new Vector3((float) x, 0.0f, 0.0f);
    }
    if (this.mainStorageState != Robot_GF_Test1.mainStorageStates.resetting)
      return;
    double x1 = (1.0 - (double) this.mainStorageStateTimer.ElapsedMilliseconds / 1500.0) * 1.5;
    if (x1 < 0.0)
      x1 = 0.0;
    this.mainStorageJoint.targetPosition = new Vector3((float) x1, 0.0f, 0.0f);
  }

  public enum dumperStates
  {
    dumping,
    resetting,
  }

  public enum mainExtensionStates
  {
    goingForwards,
    resetting,
  }

  public enum mainStorageStates
  {
    goingForwards,
    resetting,
  }
}
