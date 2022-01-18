// Decompiled with JetBrains decompiler
// Type: Robot_goBilda
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class Robot_goBilda : RobotInterface3D
{
  private bool init_done;
  [Header("Bot float settings")]
  public float intake_speed = -750f;
  public float platform_wheel_speed = 400f;
  [Header("Bot Linked Objects")]
  public HingeJoint intake_collector;
  public HingeJoint bucket;
  public HingeJoint left_platform_wheel;
  public HingeJoint right_platform_wheel;
  public ConfigurableJoint slide_last;
  public Transform slide2;
  public Transform slide0;
  public float slide_max = 1f;
  public float slide_min;
  public float slide_speed = 10f;
  public float bucket_min = -40f;
  public float bucket_max = 180f;
  public float bucket_speed = 200f;
  [Header("Animation Aid")]
  public Transform intake_spinner1_zaxis;
  private AudioManager audio_manager;
  private Robot_goBilda.intakeStates intake_statemachine;
  private Robot_goBilda.intakeStates old_intake_statemachine;
  private Robot_goBilda.bucketStates bucket_statemachine = Robot_goBilda.bucketStates.intake;
  public double delta_angle_display;
  private bool sound_shooting;

  public void Awake()
  {
    this.info = "Gamepad Up/Down: Move Lift\nGamepad Left/Right: Rotate Bucket\nButton A: Rotate Bucket to loading position\nButton B: Reverse intake while held\nButton X: Toggle intake\nButton Y: Rotate Bucket to max out position\nLeft / Right trigger: Rotate left/ right platform wheels" + this.info;
    this.valid_DriveTrains.Add("6-Wheel Tank");
  }

  public override void Init_Robot()
  {
    base.Init_Robot();
    if (!(this.DriveTrain != "6-Wheel Tank"))
      return;
    string[] strArray = new string[4]
    {
      "TRWheelSpring",
      "TLWheelSpring",
      "BLWheelSpring",
      "BRWheelSpring"
    };
    foreach (string n in strArray)
    {
      ConfigurableJoint component = this.transform.Find(n).GetComponent<ConfigurableJoint>();
      JointDrive xDrive = component.xDrive;
      xDrive.positionSpring *= 4f;
      component.xDrive = xDrive;
    }
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
      if ((bool) (Object) this.audio_manager && !this.audio_manager.IsSoundStarted("arm"))
        this.audio_manager.Play("arm", 0.5f);
      if ((double) this.MoveSlide(this.slide_last, RobotInterface3D.Axis.x, this.slide_min, this.slide_speed * this.turning_scaler) < ((double) this.slide_min + (double) this.slide_max) * 0.25)
      {
        double num = (double) this.MoveHinge(this.bucket, this.bucket_min, this.bucket_speed * this.turning_scaler);
      }
    }
    else if (this.gamepad1_dpad_up)
    {
      if ((bool) (Object) this.audio_manager && !this.audio_manager.IsSoundStarted("arm"))
        this.audio_manager.Play("arm", 0.5f);
      double num1 = (double) this.MoveSlide(this.slide_last, RobotInterface3D.Axis.x, this.slide_max, this.slide_speed * this.turning_scaler);
      if ((double) this.bucket.spring.targetPosition < (double) this.slide_min + 45.0)
      {
        double num2 = (double) this.MoveHinge(this.bucket, this.bucket_min + 45f, this.bucket_speed);
      }
    }
    else if ((bool) (Object) this.audio_manager && this.audio_manager.IsSoundStarted("arm"))
      this.audio_manager.Stop("arm", 0.5f);
    if (this.intake_statemachine == Robot_goBilda.intakeStates.on)
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
    }
    if (this.gamepad1_b || this.intake_statemachine == Robot_goBilda.intakeStates.reverse)
    {
      JointMotor motor = this.intake_collector.motor;
      if ((double) motor.targetVelocity != -1.0 * (double) this.intake_speed)
      {
        motor.targetVelocity = -1f * this.intake_speed;
        this.intake_collector.motor = motor;
        this.intake_collector.useMotor = true;
        if ((bool) (Object) this.audio_manager && !this.audio_manager.IsSoundStarted("intake"))
          this.audio_manager.Play("intake", 0.3f);
      }
      if (this.gamepad1_b)
        this.intake_statemachine = Robot_goBilda.intakeStates.off;
    }
    if (!this.gamepad1_b && this.intake_statemachine == Robot_goBilda.intakeStates.off)
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
      this.intake_statemachine = this.intake_statemachine != Robot_goBilda.intakeStates.on ? Robot_goBilda.intakeStates.on : Robot_goBilda.intakeStates.off;
    if (this.gamepad1_dpad_right)
    {
      this.bucket_statemachine = Robot_goBilda.bucketStates.manual;
      if ((bool) (Object) this.audio_manager && !this.audio_manager.IsSoundStarted("bucket"))
        this.audio_manager.Play("bucket", 0.5f);
      double num = (double) this.MoveHinge(this.bucket, this.bucket_max, this.bucket_speed * this.turning_scaler);
    }
    else if (this.gamepad1_dpad_left)
    {
      this.bucket_statemachine = Robot_goBilda.bucketStates.manual;
      if ((bool) (Object) this.audio_manager && !this.audio_manager.IsSoundStarted("bucket"))
        this.audio_manager.Play("bucket", 0.5f);
      double num = (double) this.MoveHinge(this.bucket, this.bucket_min, this.bucket_speed * this.turning_scaler);
    }
    else if (this.bucket_statemachine == Robot_goBilda.bucketStates.manual && (bool) (Object) this.audio_manager && this.audio_manager.IsSoundStarted("bucket"))
      this.audio_manager.Stop("bucket", 0.5f);
    if (this.gamepad1_y)
    {
      this.bucket_statemachine = Robot_goBilda.bucketStates.drop;
      if ((bool) (Object) this.audio_manager && !this.audio_manager.IsSoundStarted("bucket"))
        this.audio_manager.Play("bucket", 0.5f);
    }
    if (this.gamepad1_a)
    {
      this.bucket_statemachine = Robot_goBilda.bucketStates.intake;
      if ((bool) (Object) this.audio_manager && !this.audio_manager.IsSoundStarted("bucket"))
        this.audio_manager.Play("bucket", 0.5f);
    }
    switch (this.bucket_statemachine)
    {
      case Robot_goBilda.bucketStates.intake:
        if ((double) this.MoveHinge(this.bucket, this.bucket_min, this.bucket_speed) == (double) this.bucket_min)
        {
          this.bucket_statemachine = Robot_goBilda.bucketStates.manual;
          break;
        }
        break;
      case Robot_goBilda.bucketStates.drop:
        if ((double) this.MoveHinge(this.bucket, this.bucket_max, this.bucket_speed) == (double) this.bucket_max)
        {
          this.bucket_statemachine = Robot_goBilda.bucketStates.manual;
          break;
        }
        break;
    }
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
    float num = -1f * (this.slide_last.transform.position - this.slide0.transform.position).magnitude;
    Vector3 localPosition = this.slide2.localPosition;
    float x = (Quaternion.Inverse(this.slide0.localRotation) * this.slide0.transform.localPosition).x;
    this.slide2.localPosition = this.slide2.localRotation * (Quaternion.Inverse(this.slide2.localRotation) * localPosition) with
    {
      x = (x + num * 0.5f)
    };
  }

  public override string GetStates() => base.GetStates() + ";";

  public override void SetStates(string instring) => base.SetStates(instring.Split(';')[0]);

  private enum intakeStates
  {
    off,
    on,
    reverse,
  }

  private enum bucketStates
  {
    manual,
    intake,
    drop,
  }
}
