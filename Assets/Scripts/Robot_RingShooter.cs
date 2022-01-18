// Decompiled with JetBrains decompiler
// Type: Robot_RingShooter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class Robot_RingShooter : RobotInterface3D
{
  private bool init_done;
  [Header("Bot float settings")]
  public float intake_speed = -750f;
  public float ring_feeder_speed = 1.5f;
  public float ring_out_speed = 2f;
  public float aim_tilt_speed = -300f;
  public float aim_target_pos = 10f;
  public float outhinge_min = -30f;
  public float outhinge_max = 30f;
  public float ring_velocity = 6f;
  public float bucket_load_pos;
  public float bucket_shoot_pos = 25f;
  public float bucket_speed = 2f;
  public float forearm_min;
  public float forearm_max = 180f;
  public float forearm_speed = 200f;
  public float finger_min;
  public float finger_max = -120f;
  public float finger_speed = 200f;
  [Header("Bot Linked Objects")]
  public HingeJoint intake_collector;
  public HingeJoint intake_collector2;
  public HingeJoint intake_collector3;
  public HingeJoint bucket;
  public HingeJoint ring_aimer;
  public ballshooting ring_out_forcer;
  public ballshooting ring_feeder;
  public GameObject indicator;
  private float indicator_position;
  public HingeJoint forearm;
  public HingeJoint finger1;
  public HingeJoint finger2;
  public Transform flywheel;
  public float flywheel_speed = 1f;
  [Header("Animation Aid")]
  public Transform intake_spinner1_zaxis;
  public Transform intake_spinner2_zaxis;
  public Transform intake_spinner3_zaxis;
  private bool a_button_last;
  private Vector3 indicator_starting_pos;
  private Sound_Controller_FRC_Shooter sound_controller;
  private AudioManager audio_manager;
  private Robot_RingShooter.intakeStates intake_statemachine;
  private Robot_RingShooter.intakeStates old_intake_statemachine;
  private bool movedLift_sound;
  private Robot_RingShooter.bucketStates bucket_statemachine;
  private Robot_RingShooter.bucketStates old_bucket_statemachine;
  public double delta_angle_display;
  private bool sound_shooting;

  public void Awake() => this.info = "<Missing Robot Specific Function: TBD>" + this.info;

  public override void Start()
  {
    base.Start();
    this.indicator_starting_pos = this.indicator.transform.localPosition;
  }

  public override void RobotFixedUpdate()
  {
    if (!(bool) (Object) this.rb_body || !(bool) (Object) this.myRobotID || !this.init_done || this.isKinematic || !(bool) (Object) this.indicator)
      return;
    this.indicator.transform.localRotation = this.rb_body.transform.localRotation;
    this.indicator.transform.localPosition = this.rb_body.transform.localPosition + this.rb_body.transform.localRotation * (this.indicator_starting_pos + this.indicator_position * new Vector3(0.0f, 0.0f, -0.818f));
  }

  public override void Update_Robot()
  {
    if (!this.init_done)
    {
      this.ring_out_forcer.speed = this.ring_out_speed;
      this.sound_controller = this.GetComponentInChildren<Sound_Controller_FRC_Shooter>();
      this.audio_manager = this.GetComponentInChildren<AudioManager>();
      this.init_done = true;
    }
    this.DoAnimations(this.isKinematic);
    if (this.isKinematic)
      return;
    if (this.gamepad1_y || this.gamepad1_x)
      this.intake_statemachine = Robot_RingShooter.intakeStates.reverse;
    else if (this.intake_statemachine == Robot_RingShooter.intakeStates.reverse)
    {
      this.intake_statemachine = Robot_RingShooter.intakeStates.onNormal;
      this.bucket_statemachine = Robot_RingShooter.bucketStates.loading;
    }
    if (this.gamepad1_a && !this.a_button_last)
    {
      if (this.intake_statemachine == Robot_RingShooter.intakeStates.onNormal)
      {
        this.intake_statemachine = Robot_RingShooter.intakeStates.off;
      }
      else
      {
        this.intake_statemachine = Robot_RingShooter.intakeStates.onNormal;
        this.bucket_statemachine = Robot_RingShooter.bucketStates.loading;
      }
    }
    this.a_button_last = this.gamepad1_a;
    if (this.intake_statemachine != this.old_intake_statemachine)
    {
      if (this.intake_statemachine == Robot_RingShooter.intakeStates.off)
      {
        if ((bool) (Object) this.sound_controller)
          this.sound_controller.revdown();
      }
      else if (this.old_intake_statemachine == Robot_RingShooter.intakeStates.off && (bool) (Object) this.sound_controller)
        this.sound_controller.revup();
    }
    this.old_intake_statemachine = this.intake_statemachine;
    bool flag1 = false;
    float num1 = 0.0f;
    if (this.gamepad1_dpad_down)
    {
      flag1 = true;
      num1 = this.MoveHinge(this.ring_aimer, this.outhinge_min, this.aim_tilt_speed * this.turning_scaler);
    }
    if (this.gamepad1_dpad_up)
    {
      flag1 = true;
      num1 = this.MoveHinge(this.ring_aimer, this.outhinge_max, this.aim_tilt_speed * this.turning_scaler);
    }
    if (this.gamepad1_b)
    {
      flag1 = true;
      num1 = this.MoveHinge(this.ring_aimer, this.aim_target_pos, this.aim_tilt_speed);
    }
    if (flag1 && (double) num1 != (double) this.outhinge_min && (double) num1 != (double) this.outhinge_max && (double) num1 != (double) this.aim_target_pos)
    {
      if ((bool) (Object) this.audio_manager && !this.audio_manager.IsSoundStarted("adjustangle"))
        this.audio_manager.Play("adjustangle", 0.1f);
    }
    else if ((bool) (Object) this.audio_manager && this.audio_manager.IsSoundStarted("adjustangle"))
      this.audio_manager.Stop("adjustangle", 0.3f);
    bool flag2 = false;
    if (this.gamepad1_dpad_left)
    {
      double num2 = (double) this.MoveHinge(this.forearm, this.forearm_min, this.forearm_speed);
      flag2 = true;
    }
    if (this.gamepad1_dpad_right)
    {
      double num3 = (double) this.MoveHinge(this.forearm, this.forearm_max, this.forearm_speed);
      flag2 = true;
    }
    bool flag3 = false;
    if ((double) this.gamepad1_right_trigger > 0.5)
    {
      double num4 = (double) this.MoveHinge(this.finger1, this.finger_min, this.finger_speed);
      double num5 = (double) this.MoveHinge(this.finger2, this.finger_min, this.finger_speed);
      flag3 = true;
    }
    if ((double) this.gamepad1_left_trigger > 0.5)
    {
      double num6 = (double) this.MoveHinge(this.finger1, this.finger_max, this.finger_speed);
      double num7 = (double) this.MoveHinge(this.finger2, this.finger_max, this.finger_speed);
      flag3 = true;
    }
    if (flag3 || flag2 && (double) this.forearm.spring.targetPosition != (double) this.forearm_min && (double) this.forearm.spring.targetPosition != (double) this.forearm_max)
    {
      if (!this.movedLift_sound)
      {
        if ((bool) (Object) this.audio_manager && !this.audio_manager.IsSoundStarted("hangerraise"))
          this.audio_manager.Play("hangerraise", 0.1f);
        this.movedLift_sound = true;
      }
    }
    else
    {
      if ((bool) (Object) this.audio_manager && this.audio_manager.IsSoundStarted("hangerraise"))
        this.audio_manager.Stop("hangerraise", 0.3f);
      this.movedLift_sound = false;
    }
    if (this.gamepad1_x)
    {
      this.ring_feeder.speed = this.ring_feeder_speed;
      this.bucket_statemachine = Robot_RingShooter.bucketStates.shooting;
      this.intake_statemachine = Robot_RingShooter.intakeStates.off;
      if (!this.sound_shooting)
      {
        if ((bool) (Object) this.audio_manager)
          this.audio_manager.Play("shooting", 0.3f);
        this.sound_shooting = true;
      }
    }
    else
    {
      this.ring_feeder.speed = 0.0f;
      if (this.intake_statemachine == Robot_RingShooter.intakeStates.reverse)
      {
        double ringFeederSpeed = (double) this.ring_feeder_speed;
      }
      if (this.sound_shooting)
      {
        if ((bool) (Object) this.audio_manager)
          this.audio_manager.Stop("shooting", 0.3f);
        this.sound_shooting = false;
      }
    }
    if (this.gamepad1_y && this.gamepad1_x)
      this.bucket_statemachine = Robot_RingShooter.bucketStates.other;
    if (this.intake_statemachine == Robot_RingShooter.intakeStates.onNormal)
    {
      this.intake_collector.motor = this.intake_collector.motor with
      {
        targetVelocity = this.intake_speed
      };
      if ((bool) (Object) this.intake_collector2)
        this.intake_collector2.motor = this.intake_collector2.motor with
        {
          targetVelocity = 2f * this.intake_speed
        };
      if ((bool) (Object) this.intake_collector3)
        this.intake_collector3.motor = this.intake_collector3.motor with
        {
          targetVelocity = 3f * this.intake_speed
        };
    }
    if (this.intake_statemachine == Robot_RingShooter.intakeStates.off)
    {
      this.intake_collector.motor = this.intake_collector.motor with
      {
        targetVelocity = 0.0f
      };
      if ((bool) (Object) this.intake_collector2)
        this.intake_collector2.motor = this.intake_collector2.motor with
        {
          targetVelocity = 0.0f
        };
      if ((bool) (Object) this.intake_collector3)
        this.intake_collector3.motor = this.intake_collector3.motor with
        {
          targetVelocity = 0.0f
        };
    }
    if (this.intake_statemachine == Robot_RingShooter.intakeStates.reverse)
    {
      this.intake_collector.motor = this.intake_collector.motor with
      {
        targetVelocity = -1f * this.intake_speed
      };
      if ((bool) (Object) this.intake_collector2)
        this.intake_collector2.motor = this.intake_collector2.motor with
        {
          targetVelocity = -1f * this.intake_speed
        };
      if ((bool) (Object) this.intake_collector3)
        this.intake_collector3.motor = this.intake_collector3.motor with
        {
          targetVelocity = -1f * this.intake_speed
        };
    }
    if (!(bool) (Object) this.indicator.GetComponent<interpolation>() || !this.indicator.GetComponent<interpolation>().enabled)
    {
      this.indicator_position = (float) (((double) this.ring_aimer.spring.targetPosition - (double) this.outhinge_min) / ((double) this.outhinge_max - (double) this.outhinge_min));
      this.RobotFixedUpdate();
    }
    float num8 = (double) this.bucket.spring.targetPosition > (double) this.bucket_shoot_pos ? 2f : 1f;
    if (this.bucket_statemachine == Robot_RingShooter.bucketStates.loading)
    {
      double num9 = (double) this.MoveHinge(this.bucket, this.bucket_load_pos, num8 * this.bucket_speed);
    }
    else if (this.bucket_statemachine == Robot_RingShooter.bucketStates.shooting)
    {
      double num10 = (double) this.MoveHinge(this.bucket, this.bucket_shoot_pos, num8 * this.bucket_speed);
    }
    else
    {
      if (this.bucket_statemachine != Robot_RingShooter.bucketStates.other)
        return;
      double num11 = (double) this.MoveHinge(this.bucket, this.bucket_shoot_pos + 90f, num8 * this.bucket_speed);
    }
  }

  public void DoAnimations(bool client_mode = false)
  {
    if ((bool) (Object) this.flywheel)
      this.flywheel.transform.localRotation = this.flywheel.transform.localRotation * Quaternion.identity with
      {
        y = this.flywheel_speed * Time.deltaTime
      };
    if (this.intake_statemachine == Robot_RingShooter.intakeStates.off)
      return;
    float yAngle = this.intake_speed * Time.deltaTime;
    if (this.intake_statemachine == Robot_RingShooter.intakeStates.reverse)
      yAngle *= -1f;
    if ((bool) (Object) this.intake_spinner1_zaxis)
      this.intake_spinner1_zaxis.Rotate(0.0f, yAngle, 0.0f, Space.Self);
    if ((bool) (Object) this.intake_spinner2_zaxis)
      this.intake_spinner2_zaxis.Rotate(0.0f, yAngle, 0.0f, Space.Self);
    if (!(bool) (Object) this.intake_spinner3_zaxis)
      return;
    this.intake_spinner3_zaxis.Rotate(0.0f, yAngle, 0.0f, Space.Self);
  }

  private new float MoveHinge(HingeJoint hinge, float target, float speed)
  {
    JointLimits limits1 = hinge.limits;
    double max = (double) limits1.max;
    limits1 = hinge.limits;
    double min = (double) limits1.min;
    if (max != min)
    {
      JointLimits limits2 = hinge.limits;
      if ((double) limits2.max < (double) target)
      {
        limits2 = hinge.limits;
        target = limits2.max;
      }
      limits2 = hinge.limits;
      if ((double) limits2.min > (double) target)
      {
        limits2 = hinge.limits;
        target = limits2.min;
      }
    }
    if ((double) hinge.spring.targetPosition == (double) target)
      return target;
    float targetPosition = hinge.spring.targetPosition;
    float num = this.MoveTowards(targetPosition, target, targetPosition, Time.deltaTime * speed);
    JointSpring spring = hinge.spring with
    {
      targetPosition = num
    };
    hinge.spring = spring;
    return num;
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

  public override string GetStates() => base.GetStates() + ";" + (object) (int) this.intake_statemachine;

  public override void SetStates(string instring)
  {
    string[] strArray = instring.Split(';');
    base.SetStates(strArray[0]);
    if (strArray.Length <= 1)
      return;
    this.intake_statemachine = (Robot_RingShooter.intakeStates) int.Parse(strArray[1]);
  }

  private enum intakeStates
  {
    off,
    onNormal,
    reverse,
  }

  private enum bucketStates
  {
    loading,
    shooting,
    other,
  }
}
