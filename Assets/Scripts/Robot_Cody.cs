// Decompiled with JetBrains decompiler
// Type: Robot_Cody
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class Robot_Cody : RobotInterface3D
{
  public float intake_speed = -1200f;
  public HingeJoint stopbar;
  public float stopbar_closed;
  public float stopbar_open = -90f;
  public float stopbar_open_reverse = 90f;
  public float stopbar_speed = 100f;
  public HingeJoint intake_collector;
  public HingeJoint bucket;
  public float bucket_start;
  public float bucket_speed = 200f;
  public HingeJoint arm;
  public float arm_speed = 10f;
  public float arm_start;
  public ConfigurableJoint slide_last;
  public Transform slide5;
  public Transform slide4;
  public ConfigurableJoint slide3;
  public Transform slide2;
  public float slide_max = 1.6f;
  public float slide_min;
  public float slide_speed = 1f;
  public float arm_min = -60f;
  public float arm_deployed = 80f;
  public float arm_max = 160f;
  public float arm_speed_start = 30f;
  public float arm_speed_level = 10f;
  public float bucket_min = -90f;
  public float bucket_max = 90f;
  [Header("Turret Options")]
  public HingeJoint turret;
  public float turret_averager = 10f;
  public float turret_angle_correction;
  public float turret_speed = 100f;
  public float turret_distance_offset = 1.42f;
  public Transform team_hub;
  public Transform shared_hub;
  public Transform target_hub;
  public MeshRenderer lightbulb;
  public float[,] arm_bucket_map = new float[10, 2]
  {
    {
      -60f,
      0.0f
    },
    {
      -50f,
      -18f
    },
    {
      -40f,
      -28f
    },
    {
      -30f,
      -35f
    },
    {
      70f,
      60f
    },
    {
      80f,
      60f
    },
    {
      90f,
      50f
    },
    {
      110f,
      30f
    },
    {
      130f,
      10f
    },
    {
      160f,
      -15f
    }
  };
  public float[,] arm_slide_map = new float[9, 2]
  {
    {
      -60f,
      0.0f
    },
    {
      -10f,
      0.42f
    },
    {
      70f,
      0.42f
    },
    {
      80f,
      0.32f
    },
    {
      90f,
      0.18f
    },
    {
      100f,
      0.12f
    },
    {
      120f,
      0.0f
    },
    {
      140f,
      -0.05f
    },
    {
      160f,
      -0.06f
    }
  };
  [Header("Animation Aid")]
  public Transform intake_spinner1_zaxis;
  private AudioManager audio_manager;
  private Robot_Cody.intakeStates intake_statemachine;
  private Robot_Cody.intakeStates old_intake_statemachine;
  private Robot_Cody.armStates arm_state = Robot_Cody.armStates.spawn;
  private float arm_deployed_saved;
  public double delta_angle_display;
  public float turret_target;
  public float turret_curr_position;
  public float turret_distance;
  public float loop_gain = 1f;
  public float loop_slew_limit = 8f;
  private bool sound_shooting;
  public float left_trigger;
  public float right_trigger;
  public bool engage_auto = true;
  public string arm_state_string = "";
  public float arm_pos;
  public Vector3 arm_axis = new Vector3(0.0f, 0.0f, 0.0f);
  public float arm_target;
  private float client_slider_pos;

  public void Awake() => this.info = "Gamepad Up/Down: Move Arm up/down when deployed\nGamepad Left/Right: Move arm extended position towards/away from target\nButton A: Toggle intake, reverse intake while held\n          Arm needs to be retracted for intake to collect\nButton B: Deploy arm\nButton Y: toggle auto-aim\nButton X: Retract arm\nLeft trigger: Switch to team hub, enable auto-aim if disabled\nRight trigger: Switch to shared hub, enable auto-aim if disabled" + this.info;

  public override void Start()
  {
    base.Start();
    this.arm_deployed_saved = this.arm_deployed;
  }

  public override void Init_Robot()
  {
    if (!(bool) (UnityEngine.Object) this.myRobotID)
      return;
    this.audio_manager = this.GetComponentInChildren<AudioManager>();
    string str = "Red";
    if (!this.myRobotID.is_red)
      str = "Blue";
    GameObject gameObject1 = GameObject.Find("AllianceHub" + str);
    if ((bool) (UnityEngine.Object) gameObject1)
      this.team_hub = gameObject1.transform;
    GameObject gameObject2 = GameObject.Find("SharedHub" + str);
    if ((bool) (UnityEngine.Object) gameObject2)
      this.shared_hub = gameObject2.transform;
    this.target_hub = this.team_hub;
    this.turret_distance_offset = 1.42f;
  }

  public override void RobotFixedUpdate()
  {
    if (!(bool) (UnityEngine.Object) this.rb_body || !(bool) (UnityEngine.Object) this.myRobotID || this.isKinematic || !(bool) (UnityEngine.Object) this.team_hub || !(bool) (UnityEngine.Object) this.shared_hub || !(bool) (UnityEngine.Object) this.turret || !this.engage_auto)
      return;
    Vector2 vector2_1;
    vector2_1.x = this.turret.transform.position.x;
    vector2_1.y = this.turret.transform.position.z;
    Vector2 vector2_2;
    vector2_2.x = this.target_hub.position.x;
    vector2_2.y = this.target_hub.position.z;
    Vector2 vector2_3 = vector2_1;
    vector2_3.x -= this.turret.transform.right.x;
    vector2_3.y -= this.turret.transform.right.z;
    this.turret_distance = (vector2_2 - vector2_1).magnitude;
    float degrees_in = Vector2.SignedAngle(vector2_2 - vector2_1, vector2_3 - vector2_1) + this.turret_angle_correction;
    this.delta_angle_display = (double) degrees_in;
    float num = this.loop_gain * MyUtils.AngleWrap(degrees_in) / this.turret_averager;
    if ((double) num < -1.0 * (double) this.loop_slew_limit)
      num = -1f * this.loop_slew_limit;
    if ((double) num > (double) this.loop_slew_limit)
      num = this.loop_slew_limit;
    this.turret_target = this.turret.spring.targetPosition + num;
    if ((double) this.turret_target > 180.0)
      this.turret_target = 180f;
    if ((double) this.turret_target >= -180.0)
      return;
    this.turret_target = -180f;
  }

  public override void Update_Robot()
  {
    this.arm_state_string = this.arm_state.ToString();
    this.arm_pos = Quaternion.Angle(this.arm.connectedBody.rotation, this.arm.transform.rotation);
    this.DoAnimations(this.isKinematic);
    if (this.isKinematic)
      return;
    if (this.intake_statemachine == Robot_Cody.intakeStates.on && !this.gamepad1_a)
    {
      JointMotor motor = this.intake_collector.motor;
      if (this.arm_state == Robot_Cody.armStates.home)
      {
        if ((double) motor.targetVelocity != (double) this.intake_speed)
        {
          motor.targetVelocity = this.intake_speed;
          this.intake_collector.motor = motor;
          this.intake_collector.useMotor = true;
          if ((bool) (UnityEngine.Object) this.audio_manager && !this.audio_manager.IsSoundStarted("intake"))
            this.audio_manager.Play("intake", 0.3f);
        }
      }
      else if ((double) motor.targetVelocity != 0.0)
      {
        motor.targetVelocity = 0.0f;
        this.intake_collector.spring = this.intake_collector.spring with
        {
          targetPosition = this.intake_collector.angle
        };
        this.intake_collector.motor = motor;
        this.intake_collector.useMotor = false;
        if ((bool) (UnityEngine.Object) this.audio_manager && this.audio_manager.IsSoundStarted("intake"))
          this.audio_manager.Stop("intake", 0.3f);
      }
    }
    if (this.gamepad1_a)
    {
      JointMotor motor = this.intake_collector.motor;
      if ((double) motor.targetVelocity != -1.0 * (double) this.intake_speed)
      {
        motor.targetVelocity = -1f * this.intake_speed;
        this.intake_collector.motor = motor;
        this.intake_collector.useMotor = true;
        if ((bool) (UnityEngine.Object) this.audio_manager && !this.audio_manager.IsSoundStarted("intake"))
          this.audio_manager.Play("intake", 0.3f);
      }
    }
    if (!this.gamepad1_a && this.intake_statemachine == Robot_Cody.intakeStates.off)
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
        if ((bool) (UnityEngine.Object) this.audio_manager && this.audio_manager.IsSoundStarted("intake"))
          this.audio_manager.Stop("intake", 0.3f);
      }
    }
    if (this.gamepad1_a_changed && this.gamepad1_a)
    {
      switch (this.intake_statemachine)
      {
        case Robot_Cody.intakeStates.off:
          this.intake_statemachine = Robot_Cody.intakeStates.on;
          break;
        case Robot_Cody.intakeStates.on:
          this.intake_statemachine = Robot_Cody.intakeStates.off;
          break;
        case Robot_Cody.intakeStates.reverse:
          this.intake_statemachine = Robot_Cody.intakeStates.on;
          break;
      }
    }
    if (this.gamepad1_y_changed && this.gamepad1_y)
      this.engage_auto = !this.engage_auto;
    this.left_trigger = this.gamepad1_left_trigger;
    this.right_trigger = this.gamepad1_right_trigger;
    if ((double) this.gamepad1_left_trigger > 0.5)
    {
      this.target_hub = this.team_hub;
      this.engage_auto = true;
      this.arm_deployed_saved = this.arm_deployed;
    }
    if ((double) this.gamepad1_right_trigger > 0.5)
    {
      this.target_hub = this.shared_hub;
      this.engage_auto = true;
      this.arm_deployed_saved = this.arm_max - 30f;
    }
    if (this.lightbulb.enabled != this.engage_auto)
      this.lightbulb.enabled = this.engage_auto;
    bool flag = false;
    switch (this.arm_state)
    {
      case Robot_Cody.armStates.closing:
        if ((double) this.MoveHinge(this.stopbar, this.stopbar_closed, this.stopbar_speed) == (double) this.stopbar_closed)
          this.arm_state = Robot_Cody.armStates.deploying;
        if ((bool) (UnityEngine.Object) this.audio_manager && !this.audio_manager.IsSoundStarted("arm"))
          this.audio_manager.Play("arm", 0.5f);
        flag = true;
        break;
      case Robot_Cody.armStates.deploying:
        if (this.MoveArm(this.arm_deployed_saved))
          this.arm_state = Robot_Cody.armStates.rotatingout;
        flag = true;
        break;
      case Robot_Cody.armStates.rotatingout:
        if ((double) Math.Abs(this.turret.spring.targetPosition - this.turret_target) < 2.0)
        {
          this.arm_state = Robot_Cody.armStates.extending;
          break;
        }
        break;
      case Robot_Cody.armStates.extending:
        if (this.CodeMoveSlide(this.turret_distance - this.turret_distance_offset, this.slide_speed))
        {
          this.arm_state = Robot_Cody.armStates.deployed;
          break;
        }
        break;
      case Robot_Cody.armStates.deployed:
        if (this.gamepad1_dpad_down)
        {
          this.MoveArm(this.arm_max, this.arm_speed_level);
          flag = true;
          break;
        }
        if (this.gamepad1_dpad_up)
        {
          this.MoveArm(this.arm_deployed, this.arm_speed_level);
          flag = true;
          break;
        }
        break;
      case Robot_Cody.armStates.opening:
        if ((double) this.MoveHinge(this.stopbar, this.gamepad1_x ? this.stopbar_open_reverse : this.stopbar_open, this.stopbar_speed) == (double) this.stopbar_open && !this.gamepad1_x)
          this.arm_state = Robot_Cody.armStates.retracting;
        if ((bool) (UnityEngine.Object) this.audio_manager && !this.audio_manager.IsSoundStarted("arm"))
          this.audio_manager.Play("arm", 0.5f);
        flag = true;
        break;
      case Robot_Cody.armStates.retracting:
        this.MoveArm(-10f);
        if (this.CodeMoveSlide(0.0f, this.slide_speed))
        {
          this.arm_state = Robot_Cody.armStates.rotatingin;
          break;
        }
        break;
      case Robot_Cody.armStates.rotatingin:
        this.MoveArm(-10f);
        if ((double) (this.turret_curr_position = this.MoveHinge(this.turret, 0.0f, this.turret_speed)) == 0.0)
        {
          this.arm_state = Robot_Cody.armStates.returning;
          break;
        }
        break;
      case Robot_Cody.armStates.returning:
        if (this.MoveArm(this.arm_min))
          this.arm_state = Robot_Cody.armStates.home;
        flag = true;
        break;
    }
    switch (this.arm_state)
    {
      case Robot_Cody.armStates.deploying:
      case Robot_Cody.armStates.rotatingout:
      case Robot_Cody.armStates.extending:
      case Robot_Cody.armStates.deployed:
      case Robot_Cody.armStates.opening:
        if ((double) Quaternion.Angle(this.arm.connectedBody.rotation, this.arm.transform.rotation) > 40.0)
        {
          this.turret_curr_position = this.MoveHinge(this.turret, this.turret_target, this.turret_speed);
          break;
        }
        break;
    }
    switch (this.arm_state)
    {
      case Robot_Cody.armStates.deployed:
      case Robot_Cody.armStates.opening:
        this.CodeMoveSlide(this.turret_distance - this.turret_distance_offset, this.slide_speed, true);
        break;
    }
    if (this.gamepad1_dpad_left)
      this.turret_distance_offset = this.MoveTowards(this.turret_distance_offset, 2f, this.turret_distance_offset, (float) ((double) Time.deltaTime * (double) this.slide_speed * 0.5) * this.turning_scaler);
    if (this.gamepad1_dpad_right)
      this.turret_distance_offset = this.MoveTowards(this.turret_distance_offset, 0.9f, this.turret_distance_offset, (float) ((double) Time.deltaTime * (double) this.slide_speed * 0.5) * this.turning_scaler);
    if (this.gamepad1_x_changed && this.gamepad1_x)
    {
      if (this.arm_state == Robot_Cody.armStates.deployed)
        this.arm_deployed_saved = this.arm.spring.targetPosition;
      this.arm_state = Robot_Cody.armStates.opening;
    }
    if (this.gamepad1_b_changed && this.gamepad1_b)
      this.arm_state = Robot_Cody.armStates.closing;
    if (flag || !(bool) (UnityEngine.Object) this.audio_manager || !this.audio_manager.IsSoundStarted("arm"))
      return;
    this.audio_manager.Stop("arm", 0.5f);
  }

  public bool MoveArm(float target, float myarm_speed = -1f)
  {
    if ((double) myarm_speed < 0.0)
      myarm_speed = this.arm_speed_start;
    if (!(bool) (UnityEngine.Object) this.arm)
      return false;
    double num1 = (double) this.MoveHinge(this.arm, target, myarm_speed * this.turning_scaler);
    double num2 = (double) this.MoveHinge(this.bucket, this.GetInterpolatedValue(this.arm.spring.targetPosition, this.arm_bucket_map), 10f * myarm_speed);
    if ((bool) (UnityEngine.Object) this.audio_manager && !this.audio_manager.IsSoundStarted("arm"))
      this.audio_manager.Play("arm", 0.5f);
    double num3 = (double) target;
    return num1 == num3;
  }

  public bool CodeMoveSlide(float target, float speed, bool correct_for_height = false)
  {
    if (correct_for_height)
      target += this.GetInterpolatedValue(this.arm.spring.targetPosition, this.arm_slide_map);
    if ((double) target < (double) this.slide_min)
      target = this.slide_min;
    if ((double) target > (double) this.slide_max)
      target = this.slide_max;
    Vector3 targetPosition = this.slide_last.targetPosition;
    if ((double) speed > (double) this.slide_speed * 0.5 && (double) Math.Abs(targetPosition.x - target) < 0.100000001490116)
      speed = (float) (0.5 * (double) this.slide_speed + ((double) speed - 0.5 * (double) this.slide_speed) * (double) Math.Abs(targetPosition.x - target) / 0.100000001490116);
    targetPosition.x = this.MoveSlide(this.slide_last, RobotInterface3D.Axis.x, target, speed);
    double num = (double) this.MoveSlide(this.slide3, RobotInterface3D.Axis.x, (float) ((double) targetPosition.x * 2.0 / 5.0), (float) ((double) speed * 2.0 / 5.0));
    return (double) Math.Abs(targetPosition.x - target) < 0.00999999977648258;
  }

  public void DoAnimations(bool client_mode = false)
  {
    float num = -1f * (this.slide_last.transform.position - this.turret.transform.position).magnitude;
    this.slide5.localPosition = this.slide5.localPosition with
    {
      x = (float) ((double) num * 4.0 / 5.0)
    };
    this.slide4.localPosition = this.slide4.localPosition with
    {
      x = (float) ((double) num * 3.0 / 5.0)
    };
    this.slide2.localPosition = this.slide2.localPosition with
    {
      x = num / 5f
    };
  }

  public override string GetStates() => base.GetStates() + ";" + (this.engage_auto ? "1" : "0");

  public override void SetStates(string instring)
  {
    string[] strArray = instring.Split(';');
    base.SetStates(strArray[0]);
    if (strArray.Length <= 1)
      return;
    this.engage_auto = strArray[1][0] == '1';
    this.lightbulb.enabled = this.engage_auto;
  }

  public float GetInterpolatedValue(float x, float[,] table)
  {
    float x2 = table[0, 0];
    float y2 = table[0, 1];
    for (int index = 1; index < table.Length; ++index)
    {
      float x1 = table[index, 0];
      float y1 = table[index, 1];
      if ((double) x1 >= (double) x && (double) x2 <= (double) x || (double) x1 <= (double) x && (double) x2 >= (double) x)
        return this.LinearInterpolate(x1, y1, x2, y2, x);
      x2 = x1;
      y2 = y1;
    }
    int index1 = table.Length - 1;
    return (double) Math.Abs(x - table[0, 0]) < (double) Math.Abs(table[index1, 0] - x) ? this.LinearInterpolate(table[0, 0], table[0, 1], table[1, 0], table[1, 1], x) : this.LinearInterpolate(table[index1, 0], table[index1, 1], table[index1 - 1, 0], table[index1 - 1, 1], x);
  }

  public float LinearInterpolate(float x1, float y1, float x2, float y2, float x)
  {
    float num = (float) (((double) y2 - (double) y1) / ((double) x2 - (double) x1));
    return (x - x1) * num + y1;
  }

  private enum intakeStates
  {
    off,
    on,
    reverse,
  }

  private enum armStates
  {
    home,
    closing,
    deploying,
    rotatingout,
    extending,
    deployed,
    opening,
    retracting,
    rotatingin,
    returning,
    spawn,
  }
}
