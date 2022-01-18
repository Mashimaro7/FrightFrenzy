// Decompiled with JetBrains decompiler
// Type: Robot_KP_ff
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class Robot_KP_ff : RobotInterface3D
{
  private bool init_done;
  [Header("Bot float settings")]
  public float intake_speed = -750f;
  public float forearm_min;
  public float forearm_max = 180f;
  public float forearm_speed = 200f;
  public float platform_wheel_speed = 400f;
  [Header("Bot Linked Objects")]
  public HingeJoint intake_collector;
  public HingeJoint forearm;
  public HingeJoint bucket;
  public HingeJoint left_platform_wheel;
  public HingeJoint right_platform_wheel;
  public ballshooting ramp_force_box;
  public float ramp_speed = 1.5f;
  public HingeJoint out_flap;
  public float flap_speed = 200f;
  [Header("Animation Aid")]
  public Transform intake_spinner1_zaxis;
  private AudioManager audio_manager;
  private Robot_KP_ff.intakeStates intake_statemachine;
  private Robot_KP_ff.intakeStates old_intake_statemachine;
  public double delta_angle_display;
  private bool sound_shooting;
  private float spring_target_pos;

  public void Awake() => this.info = "Gamepad Up/Down: Move Arm\nGamepad Left/Right: Rotate Bucket\nButton A: Toggle freight release flap\nButton B: Toggle intake reverse\nButton Y: Reverse intake while held\nButton X: Toggle intake\nLeft/Right trigger: Rotate left/right platform wheels" + this.info;

  public override void Start() => base.Start();

  public override void RobotFixedUpdate()
  {
    if (!(bool) (Object) this.rb_body || !(bool) (Object) this.myRobotID || !this.init_done)
      return;
    int num = this.isKinematic ? 1 : 0;
  }

  public override void Update_Robot()
  {
    if (!this.init_done)
    {
      this.audio_manager = this.GetComponentInChildren<AudioManager>();
      this.init_done = true;
    }
    this.DoAnimations(this.isKinematic);
    if (this.isKinematic)
      return;
    if (this.gamepad1_dpad_down)
    {
      double num1 = (double) this.MoveHinge(this.forearm, this.forearm_min, this.forearm_speed * this.turning_scaler);
      double num2 = (double) this.MoveHinge(this.bucket, this.forearm.spring.targetPosition, 2f * this.forearm_speed);
      if ((bool) (Object) this.audio_manager && !this.audio_manager.IsSoundStarted("arm"))
        this.audio_manager.Play("arm", 0.5f);
      this.spring_target_pos = 0.0f;
    }
    else if (this.gamepad1_dpad_up)
    {
      double num3 = (double) this.MoveHinge(this.forearm, this.forearm_max, this.forearm_speed * this.turning_scaler);
      double num4 = (double) this.MoveHinge(this.bucket, this.forearm.spring.targetPosition, 2f * this.forearm_speed);
      if ((bool) (Object) this.audio_manager && !this.audio_manager.IsSoundStarted("arm"))
        this.audio_manager.Play("arm", 0.5f);
    }
    else if ((bool) (Object) this.audio_manager && this.audio_manager.IsSoundStarted("arm"))
      this.audio_manager.Stop("arm", 0.5f);
    if (this.intake_statemachine == Robot_KP_ff.intakeStates.on)
    {
      JointMotor motor = this.intake_collector.motor;
      if ((double) motor.targetVelocity != (double) this.intake_speed)
      {
        motor.targetVelocity = this.intake_speed;
        this.ramp_force_box.speed = this.ramp_speed;
        this.intake_collector.motor = motor;
        this.intake_collector.useMotor = true;
        if ((bool) (Object) this.audio_manager && !this.audio_manager.IsSoundStarted("intake"))
          this.audio_manager.Play("intake", 0.3f);
      }
    }
    if (this.gamepad1_y || this.intake_statemachine == Robot_KP_ff.intakeStates.reverse)
    {
      JointMotor motor = this.intake_collector.motor;
      if ((double) motor.targetVelocity != -1.0 * (double) this.intake_speed)
      {
        motor.targetVelocity = -1f * this.intake_speed;
        this.ramp_force_box.speed = -1f * this.ramp_speed;
        this.intake_collector.motor = motor;
        this.intake_collector.useMotor = true;
        if ((bool) (Object) this.audio_manager && !this.audio_manager.IsSoundStarted("intake"))
          this.audio_manager.Play("intake", 0.3f);
      }
      if (this.gamepad1_y)
        this.intake_statemachine = Robot_KP_ff.intakeStates.off;
    }
    if (!this.gamepad1_y && this.intake_statemachine == Robot_KP_ff.intakeStates.off)
    {
      JointMotor motor = this.intake_collector.motor;
      if ((double) motor.targetVelocity != 0.0)
      {
        motor.targetVelocity = 0.0f;
        this.ramp_force_box.speed = 0.0f;
        this.intake_collector.spring = this.intake_collector.spring with
        {
          targetPosition = this.intake_collector.angle
        };
        this.intake_collector.motor = motor;
        this.intake_collector.useMotor = false;
        if ((bool) (Object) this.audio_manager && this.audio_manager.IsSoundStarted("intake"))
          this.audio_manager.Stop("intake", 0.3f);
      }
    }
    if (this.gamepad1_x_changed && this.gamepad1_x)
      this.intake_statemachine = this.intake_statemachine != Robot_KP_ff.intakeStates.on ? Robot_KP_ff.intakeStates.on : Robot_KP_ff.intakeStates.off;
    if (this.gamepad1_b_changed && this.gamepad1_b)
      this.intake_statemachine = this.intake_statemachine != Robot_KP_ff.intakeStates.reverse ? Robot_KP_ff.intakeStates.reverse : Robot_KP_ff.intakeStates.off;
    if (this.gamepad1_a_changed && this.gamepad1_a)
      this.spring_target_pos = (double) this.spring_target_pos != 0.0 ? 0.0f : -60f;
    double num = (double) this.MoveHinge(this.out_flap, this.spring_target_pos, this.flap_speed);
    bool flag = false;
    if ((double) this.gamepad1_left_trigger > 0.0)
    {
      this.left_platform_wheel.motor = this.left_platform_wheel.motor with
      {
        targetVelocity = this.gamepad1_left_trigger * this.platform_wheel_speed * this.turning_scaler
      };
      flag = true;
    }
    else
      this.left_platform_wheel.motor = this.left_platform_wheel.motor with
      {
        targetVelocity = 0.0f
      };
    if ((double) this.gamepad1_right_trigger > 0.0)
    {
      this.right_platform_wheel.motor = this.right_platform_wheel.motor with
      {
        targetVelocity = this.gamepad1_right_trigger * this.platform_wheel_speed * this.turning_scaler
      };
      flag = true;
    }
    else
      this.right_platform_wheel.motor = this.right_platform_wheel.motor with
      {
        targetVelocity = 0.0f
      };
    if (flag)
    {
      if (!(bool) (Object) this.audio_manager || this.audio_manager.IsSoundStarted("wheels"))
        return;
      this.audio_manager.Play("wheels", 0.5f);
    }
    else
    {
      if (!(bool) (Object) this.audio_manager || !this.audio_manager.IsSoundStarted("wheels"))
        return;
      this.audio_manager.Stop("wheels", 0.2f);
    }
  }

  public void DoAnimations(bool client_mode = false)
  {
  }

  private float MoveHingeCJ(ConfigurableJoint hinge, float target, float speed)
  {
    SoftJointLimit softJointLimit1 = hinge.lowAngularXLimit;
    double limit1 = (double) softJointLimit1.limit;
    softJointLimit1 = hinge.highAngularXLimit;
    double limit2 = (double) softJointLimit1.limit;
    if (limit1 != limit2)
    {
      SoftJointLimit softJointLimit2 = hinge.highAngularXLimit;
      if ((double) softJointLimit2.limit < (double) target)
      {
        softJointLimit2 = hinge.highAngularXLimit;
        target = softJointLimit2.limit;
      }
      softJointLimit2 = hinge.lowAngularXLimit;
      if ((double) softJointLimit2.limit > (double) target)
      {
        softJointLimit2 = hinge.lowAngularXLimit;
        target = softJointLimit2.limit;
      }
    }
    Quaternion targetRotation = hinge.targetRotation;
    if ((double) MyUtils.AngleWrap(targetRotation.eulerAngles.x) == (double) target)
      return target;
    targetRotation = hinge.targetRotation;
    float num1 = MyUtils.AngleWrap(targetRotation.eulerAngles.x);
    float num2 = this.MoveTowards(num1, target, num1, Time.deltaTime * speed);
    targetRotation = hinge.targetRotation;
    Vector3 eulerAngles = targetRotation.eulerAngles with
    {
      x = num2
    };
    hinge.targetRotation = Quaternion.Euler(eulerAngles);
    return num2;
  }

  public override string GetStates() => base.GetStates() + ";";

  public override void SetStates(string instring) => base.SetStates(instring.Split(';')[0]);

  private enum intakeStates
  {
    off,
    on,
    reverse,
  }
}
