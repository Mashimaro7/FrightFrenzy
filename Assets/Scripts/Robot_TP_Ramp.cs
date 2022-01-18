// Decompiled with JetBrains decompiler
// Type: Robot_TP_Ramp
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class Robot_TP_Ramp : RobotInterface3D
{
  public HingeJoint intake_collector;
  public HingeJoint intake_collector2;
  public float intake_speed = -1000f;
  private Robot_TP_Ramp.intakeStates intake_statemachine;
  private Robot_TP_Ramp.intakeStates old_intake_statemachine;
  public ballshooting ramp_force_box;
  public float ramp_speed = 1.5f;
  private bool a_button_last;
  public ConfigurableJoint lift_arm;
  public ConfigurableJoint lift_inner;
  public ConfigurableJoint lift_middle;
  public HingeJoint lift_pole;
  public float lift_speed = 1f;
  public float lift_max = -2f;
  public float lift_min;
  private Robot_TP_Ramp.poleStates curr_pole_state;
  public float pole_speed = 100f;
  public float pole_max = 140f;
  public float pole_ready = 90f;
  public float lift_deploy_movement = -0.297f;
  public float pole_min;
  public float lift_deploy_target;
  private bool x_button_last;
  public HingeJoint ForkL;
  public HingeJoint ForkR;
  public float fork_min;
  public float fork_max = 100f;
  public float fork_speed = 200f;
  public HingeJoint platform_arms;
  public float platarms_min;
  public float platarms_max = 45f;
  public float platarms_speed = 100f;
  public float lt;
  public float rt;
  private Sound_Controller_FRC_Shooter sound_controller;
  private AudioManager audio_manager;
  private bool init_done;
  private bool arm_played;

  public void Awake() => this.info = "<Missing Robot Specific Function: TBD>" + this.info;

  public override void Update_Robot()
  {
    if (!this.init_done)
    {
      this.sound_controller = this.GetComponentInChildren<Sound_Controller_FRC_Shooter>();
      this.audio_manager = this.GetComponentInChildren<AudioManager>();
      this.init_done = true;
    }
    if (this.gamepad1_a && !this.a_button_last)
      this.intake_statemachine = this.intake_statemachine != Robot_TP_Ramp.intakeStates.onNormal ? Robot_TP_Ramp.intakeStates.onNormal : Robot_TP_Ramp.intakeStates.off;
    this.a_button_last = this.gamepad1_a;
    if (this.gamepad1_y)
      this.intake_statemachine = Robot_TP_Ramp.intakeStates.reverse;
    else if (this.intake_statemachine == Robot_TP_Ramp.intakeStates.reverse)
      this.intake_statemachine = Robot_TP_Ramp.intakeStates.onNormal;
    if (this.intake_statemachine != this.old_intake_statemachine)
    {
      if (this.intake_statemachine == Robot_TP_Ramp.intakeStates.off)
      {
        if ((bool) (Object) this.sound_controller)
          this.sound_controller.revdown();
      }
      else if (this.old_intake_statemachine == Robot_TP_Ramp.intakeStates.off && (bool) (Object) this.sound_controller)
        this.sound_controller.revup();
    }
    this.old_intake_statemachine = this.intake_statemachine;
    if (this.intake_statemachine == Robot_TP_Ramp.intakeStates.onNormal)
    {
      this.intake_collector.motor = this.intake_collector.motor with
      {
        targetVelocity = this.intake_speed
      };
      this.intake_collector2.motor = this.intake_collector2.motor with
      {
        targetVelocity = -4f * this.intake_speed
      };
      this.ramp_force_box.speed = this.ramp_speed;
    }
    if (this.intake_statemachine == Robot_TP_Ramp.intakeStates.off)
    {
      this.intake_collector.motor = this.intake_collector.motor with
      {
        targetVelocity = 0.0f
      };
      this.intake_collector2.motor = this.intake_collector2.motor with
      {
        targetVelocity = 0.0f
      };
      this.ramp_force_box.speed = 0.0f;
    }
    if (this.intake_statemachine == Robot_TP_Ramp.intakeStates.reverse)
    {
      this.intake_collector.motor = this.intake_collector.motor with
      {
        targetVelocity = -1f * this.intake_speed
      };
      this.intake_collector2.motor = this.intake_collector2.motor with
      {
        targetVelocity = -4f * this.intake_speed
      };
      this.ramp_force_box.speed = -1f * this.ramp_speed;
    }
    bool flag1 = false;
    if (this.gamepad1_dpad_up)
    {
      flag1 = true;
      float x = this.MoveTowards(this.lift_min, this.lift_max, this.lift_arm.targetPosition.x, Time.deltaTime * this.lift_speed * this.turning_scaler);
      this.lift_arm.targetPosition = new Vector3(x, 0.0f, 0.0f);
      this.lift_inner.targetPosition = new Vector3((float) ((double) x * 2.0 / 3.0), 0.0f, 0.0f);
      this.lift_middle.targetPosition = new Vector3((float) ((double) x * 1.0 / 3.0), 0.0f, 0.0f);
      if (this.curr_pole_state == Robot_TP_Ramp.poleStates.deploying)
        this.curr_pole_state = Robot_TP_Ramp.poleStates.ready;
    }
    if (this.gamepad1_dpad_down)
    {
      flag1 = true;
      float x = this.MoveTowards(this.lift_max, this.lift_min, this.lift_arm.targetPosition.x, Time.deltaTime * this.lift_speed * this.turning_scaler);
      this.lift_arm.targetPosition = new Vector3(x, 0.0f, 0.0f);
      this.lift_inner.targetPosition = new Vector3((float) ((double) x * 2.0 / 3.0), 0.0f, 0.0f);
      this.lift_middle.targetPosition = new Vector3((float) ((double) x * 1.0 / 3.0), 0.0f, 0.0f);
      if (this.curr_pole_state == Robot_TP_Ramp.poleStates.deploying)
        this.curr_pole_state = Robot_TP_Ramp.poleStates.ready;
    }
    if (flag1 && (double) this.lift_arm.targetPosition.x != (double) this.lift_max && (double) this.lift_arm.targetPosition.x != (double) this.lift_min)
    {
      if ((bool) (Object) this.audio_manager && !this.audio_manager.IsSoundStarted("hangerraise"))
        this.audio_manager.Play("hangerraise");
    }
    else if ((bool) (Object) this.audio_manager && this.audio_manager.IsSoundStarted("hangerraise"))
      this.audio_manager.Stop("hangerraise", 0.3f);
    bool flag2 = false;
    if (this.gamepad1_dpad_left)
    {
      double num1 = (double) this.MoveHinge(this.ForkR, this.fork_min, this.fork_speed * this.turning_scaler);
      double num2 = (double) this.MoveHinge(this.ForkL, -1f * this.fork_min, this.fork_speed * this.turning_scaler);
      flag2 = true;
    }
    if (this.gamepad1_dpad_right)
    {
      double num3 = (double) this.MoveHinge(this.ForkR, this.fork_max, this.fork_speed * this.turning_scaler);
      double num4 = (double) this.MoveHinge(this.ForkL, -1f * this.fork_max, this.fork_speed * this.turning_scaler);
      flag2 = true;
    }
    if (flag2)
    {
      if ((bool) (Object) this.audio_manager && !this.audio_manager.IsSoundStarted("arm"))
      {
        this.audio_manager.Play("arm");
        this.arm_played = true;
      }
    }
    else if (this.arm_played)
    {
      if ((bool) (Object) this.audio_manager && this.audio_manager.IsSoundStarted("arm"))
        this.audio_manager.Stop("arm", 0.3f);
      this.arm_played = false;
    }
    if (this.curr_pole_state == Robot_TP_Ramp.poleStates.moving_to_ready && (double) this.MoveHinge(this.lift_pole, this.pole_ready, this.pole_speed) == (double) this.pole_ready)
      this.curr_pole_state = Robot_TP_Ramp.poleStates.ready;
    if (this.curr_pole_state == Robot_TP_Ramp.poleStates.moving_to_park && (double) this.MoveHinge(this.lift_pole, this.pole_min, this.pole_speed) == (double) this.pole_min)
      this.curr_pole_state = Robot_TP_Ramp.poleStates.parked;
    if (this.curr_pole_state == Robot_TP_Ramp.poleStates.deploying)
    {
      double num = (double) this.MoveHinge(this.lift_pole, this.pole_max, this.pole_speed);
      float x = this.MoveTowards(this.lift_min, this.lift_deploy_target, this.lift_arm.targetPosition.x, Time.deltaTime * this.lift_speed);
      this.lift_arm.targetPosition = new Vector3(x, 0.0f, 0.0f);
      this.lift_inner.targetPosition = new Vector3((float) ((double) x * 2.0 / 3.0), 0.0f, 0.0f);
      this.lift_middle.targetPosition = new Vector3((float) ((double) x * 1.0 / 3.0), 0.0f, 0.0f);
    }
    if (this.gamepad1_x && !this.x_button_last)
      this.curr_pole_state = this.curr_pole_state == Robot_TP_Ramp.poleStates.moving_to_park || this.curr_pole_state == Robot_TP_Ramp.poleStates.parked ? Robot_TP_Ramp.poleStates.moving_to_ready : Robot_TP_Ramp.poleStates.moving_to_park;
    this.x_button_last = this.gamepad1_x;
    if (this.curr_pole_state == Robot_TP_Ramp.poleStates.ready && this.gamepad1_b)
    {
      this.curr_pole_state = Robot_TP_Ramp.poleStates.deploying;
      this.lift_deploy_target = this.lift_deploy_movement + this.lift_arm.targetPosition.x;
    }
    if ((double) this.gamepad1_left_trigger > 0.0)
    {
      double num5 = (double) this.MoveHinge(this.platform_arms, this.platarms_min, this.platarms_speed * this.gamepad1_left_trigger);
    }
    if ((double) this.gamepad1_right_trigger > 0.0)
    {
      double num6 = (double) this.MoveHinge(this.platform_arms, this.platarms_max, this.platarms_speed * this.gamepad1_right_trigger);
    }
    this.lt = this.gamepad1_left_trigger;
    this.rt = this.gamepad1_right_trigger;
  }

  private enum intakeStates
  {
    off,
    onNormal,
    reverse,
  }

  private enum poleStates
  {
    parked,
    moving_to_ready,
    ready,
    deploying,
    moving_to_park,
  }
}
