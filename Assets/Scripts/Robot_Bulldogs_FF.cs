// Decompiled with JetBrains decompiler
// Type: Robot_Bulldogs_FF
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class Robot_Bulldogs_FF : RobotInterface3D
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
  [Header("Animation Aid")]
  public Transform intake_spinner1_zaxis;
  private bool a_button_last;
  private AudioManager audio_manager;
  private Robot_Bulldogs_FF.intakeStates intake_statemachine;
  private Robot_Bulldogs_FF.intakeStates old_intake_statemachine;
  public double delta_angle_display;
  private bool sound_shooting;

  public void Awake() => this.info = "Gamepad Up/Down: Move Arm\nButton A: Run intake while held\nButton B: Toggle intake reverse\nButton Y: Reverse intake while held\nButton X: Toggle intake\nLeft/Right trigger: Rotate left/right platform wheels" + this.info;

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
    this.a_button_last = this.gamepad1_a;
    if (this.gamepad1_dpad_down)
    {
      double num1 = (double) this.MoveHinge(this.forearm, this.forearm_min, this.forearm_speed * this.turning_scaler);
      double num2 = (double) this.MoveHinge(this.bucket, -1f * this.forearm.spring.targetPosition, 2f * this.forearm_speed);
      if ((bool) (Object) this.audio_manager && !this.audio_manager.IsSoundStarted("arm"))
        this.audio_manager.Play("arm", 0.5f);
    }
    else if (this.gamepad1_dpad_up)
    {
      double num3 = (double) this.MoveHinge(this.forearm, this.forearm_max, this.forearm_speed * this.turning_scaler);
      double num4 = (double) this.MoveHinge(this.bucket, -1f * this.forearm.spring.targetPosition, 2f * this.forearm_speed);
      if ((bool) (Object) this.audio_manager && !this.audio_manager.IsSoundStarted("arm"))
        this.audio_manager.Play("arm", 0.5f);
    }
    else if ((bool) (Object) this.audio_manager && this.audio_manager.IsSoundStarted("arm"))
      this.audio_manager.Stop("arm", 0.5f);
    if (this.gamepad1_a || this.intake_statemachine == Robot_Bulldogs_FF.intakeStates.on)
    {
      JointMotor motor = this.intake_collector.motor;
      if ((double) motor.targetVelocity != (double) this.intake_speed)
      {
        motor.targetVelocity = this.intake_speed;
        this.intake_collector.motor = motor;
        this.intake_collector.useMotor = true;
        if ((bool) (Object) this.audio_manager && !this.audio_manager.IsSoundStarted("intake"))
          this.audio_manager.Play("intake", 0.3f);
      }
      if (this.gamepad1_a)
        this.intake_statemachine = Robot_Bulldogs_FF.intakeStates.off;
    }
    if (this.gamepad1_y || this.intake_statemachine == Robot_Bulldogs_FF.intakeStates.reverse)
    {
      JointMotor motor = this.intake_collector.motor;
      if ((double) motor.targetVelocity != -0.5 * (double) this.intake_speed)
      {
        motor.targetVelocity = -0.5f * this.intake_speed;
        this.intake_collector.motor = motor;
        this.intake_collector.useMotor = true;
        if ((bool) (Object) this.audio_manager && !this.audio_manager.IsSoundStarted("intake"))
          this.audio_manager.Play("intake", 0.3f);
      }
      if (this.gamepad1_y)
        this.intake_statemachine = Robot_Bulldogs_FF.intakeStates.off;
    }
    if (!this.gamepad1_a && !this.gamepad1_y && this.intake_statemachine == Robot_Bulldogs_FF.intakeStates.off)
    {
      JointMotor motor = this.intake_collector.motor;
      if ((double) motor.targetVelocity != 0.0)
      {
        motor.targetVelocity = 0.0f;
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
      this.intake_statemachine = this.intake_statemachine != Robot_Bulldogs_FF.intakeStates.on ? Robot_Bulldogs_FF.intakeStates.on : Robot_Bulldogs_FF.intakeStates.off;
    if (this.gamepad1_b_changed && this.gamepad1_b)
      this.intake_statemachine = this.intake_statemachine != Robot_Bulldogs_FF.intakeStates.reverse ? Robot_Bulldogs_FF.intakeStates.reverse : Robot_Bulldogs_FF.intakeStates.off;
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
