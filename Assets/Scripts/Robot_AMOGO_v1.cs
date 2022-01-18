// Decompiled with JetBrains decompiler
// Type: Robot_AMOGO_v1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class Robot_AMOGO_v1 : RobotInterface3D
{
  public HingeJoint intake_collector;
  public HingeJoint intake_collector2;
  public float intake_speed = -1000f;
  private Robot_AMOGO_v1.intakeStates intake_statemachine;
  private Robot_AMOGO_v1.intakeStates old_intake_statemachine;
  public ballshooting ramp_force_box;
  public float ramp_speed = 1.5f;
  private bool a_button_last;
  public ConfigurableJoint lift_arm;
  public ConfigurableJoint lift_inner;
  public ConfigurableJoint lift_middle;
  public float lift_speed = 1f;
  public float lift_max = -2f;
  public float lift_min;
  private bool x_button_last;
  public HingeJoint goal_pivot;
  public float goalp_max2;
  public float goalp_min2 = 45f;
  public float goalp_speed = 200f;
  public float goalp_dump = 30f;
  public HingeJoint platform_arms;
  public float platarms_min;
  public float platarms_max = 45f;
  public float platarms_speed = 100f;
  public float lt;
  public float rt;
  private bool arm_played;
  private bool movedLift_sound;
  private Sound_Controller_FRC_Shooter sound_controller;
  private AudioManager audio_manager;
  private bool init_done;

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
      this.intake_statemachine = this.intake_statemachine != Robot_AMOGO_v1.intakeStates.onNormal ? Robot_AMOGO_v1.intakeStates.onNormal : Robot_AMOGO_v1.intakeStates.off;
    this.a_button_last = this.gamepad1_a;
    if (this.gamepad1_y)
      this.intake_statemachine = Robot_AMOGO_v1.intakeStates.reverse;
    else if (this.intake_statemachine == Robot_AMOGO_v1.intakeStates.reverse)
      this.intake_statemachine = Robot_AMOGO_v1.intakeStates.onNormal;
    if (this.intake_statemachine != this.old_intake_statemachine)
    {
      if (this.intake_statemachine == Robot_AMOGO_v1.intakeStates.off)
      {
        if ((bool) (Object) this.sound_controller)
          this.sound_controller.revdown();
      }
      else if (this.old_intake_statemachine == Robot_AMOGO_v1.intakeStates.off && (bool) (Object) this.sound_controller)
        this.sound_controller.revup();
    }
    this.old_intake_statemachine = this.intake_statemachine;
    if (this.intake_statemachine == Robot_AMOGO_v1.intakeStates.onNormal)
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
    if (this.intake_statemachine == Robot_AMOGO_v1.intakeStates.off)
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
    if (this.intake_statemachine == Robot_AMOGO_v1.intakeStates.reverse)
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
    }
    if (this.gamepad1_dpad_down)
    {
      flag1 = true;
      float x = this.MoveTowards(this.lift_max, this.lift_min, this.lift_arm.targetPosition.x, Time.deltaTime * this.lift_speed * this.turning_scaler);
      this.lift_arm.targetPosition = new Vector3(x, 0.0f, 0.0f);
      this.lift_inner.targetPosition = new Vector3((float) ((double) x * 2.0 / 3.0), 0.0f, 0.0f);
      this.lift_middle.targetPosition = new Vector3((float) ((double) x * 1.0 / 3.0), 0.0f, 0.0f);
    }
    if (flag1 && (double) this.lift_arm.targetPosition.x != (double) this.lift_max && (double) this.lift_arm.targetPosition.x != (double) this.lift_min)
    {
      if (!this.movedLift_sound)
      {
        if ((bool) (Object) this.audio_manager && !this.audio_manager.IsSoundStarted("hangerraise"))
          this.audio_manager.Play("hangerraise");
        this.movedLift_sound = true;
      }
    }
    else
    {
      if ((bool) (Object) this.audio_manager && this.audio_manager.IsSoundStarted("hangerraise"))
        this.audio_manager.Stop("hangerraise", 0.3f);
      this.movedLift_sound = false;
    }
    bool flag2 = false;
    if (this.gamepad1_dpad_left && (double) this.goal_pivot.spring.targetPosition != (double) this.goalp_max2)
    {
      double num = (double) this.MoveHinge(this.goal_pivot, this.goalp_max2, this.goalp_speed * this.turning_scaler);
      flag2 = true;
    }
    if (this.gamepad1_dpad_right && (double) this.goal_pivot.spring.targetPosition != (double) this.goalp_min2)
    {
      double num = (double) this.MoveHinge(this.goal_pivot, this.goalp_min2, this.goalp_speed * this.turning_scaler);
      flag2 = true;
    }
    if (this.gamepad1_x && (double) this.goal_pivot.spring.targetPosition != (double) this.goalp_dump)
    {
      double num = (double) this.MoveHinge(this.goal_pivot, this.goalp_dump, this.goalp_speed * this.turning_scaler);
      flag2 = true;
    }
    else if ((double) this.goal_pivot.spring.targetPosition > (double) this.goalp_max2)
    {
      double num = (double) this.MoveHinge(this.goal_pivot, this.goalp_max2, this.goalp_speed * this.turning_scaler);
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
    this.x_button_last = this.gamepad1_x;
    bool flag3 = false;
    float num1 = 0.0f;
    if ((double) this.gamepad1_left_trigger > 0.0)
    {
      flag3 = true;
      num1 = this.MoveHinge(this.platform_arms, this.platarms_min, this.platarms_speed * this.gamepad1_left_trigger);
    }
    if ((double) this.gamepad1_right_trigger > 0.0)
    {
      flag3 = true;
      num1 = this.MoveHinge(this.platform_arms, this.platarms_max, this.platarms_speed * this.gamepad1_right_trigger);
    }
    this.lt = this.gamepad1_left_trigger;
    this.rt = this.gamepad1_right_trigger;
    if (flag3 && (double) num1 != (double) this.platarms_min && (double) num1 != (double) this.platarms_max)
    {
      if (!(bool) (Object) this.audio_manager || this.audio_manager.IsSoundStarted("adjustangle"))
        return;
      this.audio_manager.Play("adjustangle");
    }
    else
    {
      if (!(bool) (Object) this.audio_manager || !this.audio_manager.IsSoundStarted("adjustangle"))
        return;
      this.audio_manager.Stop("adjustangle", 0.3f);
    }
  }

  private enum intakeStates
  {
    off,
    onNormal,
    reverse,
  }
}
