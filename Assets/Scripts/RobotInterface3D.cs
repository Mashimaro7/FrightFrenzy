// Decompiled with JetBrains decompiler
// Type: RobotInterface3D
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TMPro;
using UnityEngine;

public class RobotInterface3D : MonoBehaviour
{
  private bool DEBUG;
  public bool deleted;
  public GameObject wheelTL;
  public GameObject wheelTR;
  public GameObject wheelBL;
  public GameObject wheelBR;
  public GameObject wheelML;
  public GameObject wheelMR;
  private ConfigurableJoint wheelTL_joint;
  private Rigidbody wheelTL_body;
  private ConfigurableJoint wheelTR_joint;
  private Rigidbody wheelTR_body;
  private ConfigurableJoint wheelBL_joint;
  private Rigidbody wheelBL_body;
  private ConfigurableJoint wheelBR_joint;
  private Rigidbody wheelBR_body;
  private ConfigurableJoint wheelML_joint;
  private Rigidbody wheelML_body;
  private ConfigurableJoint wheelMR_joint;
  private Rigidbody wheelMR_body;
  public Vector3 centerOfMass;
  public float rot_inertia_scaler = 1f;
  public bool rot_inertia_scale_only_body = true;
  private float max_speed_multiplier = 1000f;
  public float turning_overide;
  public List<string> valid_DriveTrains = new List<string>()
  {
    "Tank",
    "Mecanum"
  };
  public string DriveTrain = "Tank";
  public bool DriveTrain_lock;
  public float total_weight = 1f;
  public bool total_weight_lock;
  public bool fieldcentric;
  public bool fieldcentric_lock;
  public bool activebreaking;
  public bool activebreaking_lock;
  public bool tankcontrol;
  public bool tankcontrol_lock;
  public bool lock_all_parameters;
  public Quaternion fieldcentric_rotation;
  public RobotID myRobotID;
  public bool is_FRC;
  protected bool isKinematic;
  public bool isSpectator;
  protected GameObject highlitecircle;
  protected GameObject selectcircle;
  protected ProgressMeter progressmeter;
  public bool disabledTopObjects;
  public bool pauseUpdates;
  public int robot_color = -1;
  public RobotSkin robotskin;
  private Dictionary<int, RobotInterface3D.ObjPos> starting_positions = new Dictionary<int, RobotInterface3D.ObjPos>();
  private Dictionary<int, bool> kinematic_states = new Dictionary<int, bool>();
  private Dictionary<int, CollisionDetectionMode> collision_states = new Dictionary<int, CollisionDetectionMode>();
  public string info = "\nLeft/Right Bumper = Precision Mode (while held) \nLeft Joystick Y-axis = Forward/Backwards\nLeft Joystick X-axis = Strafe\nRight Joystick X-axis = Turn/Rotate\nPress in left joystick = Switch Camera\nPress in right joystick = Restart Position\nBack button = Start Timer\nStart button = Stop Timer";
  public bool doinit;
  public Rigidbody rb_body;
  private FileStream mylogfile;
  private UnicodeEncoding encoding;
  [HideInInspector]
  public bool gamepad1_a;
  [HideInInspector]
  public bool gamepad1_x;
  [HideInInspector]
  public bool gamepad1_y;
  [HideInInspector]
  public bool gamepad1_b;
  public float gamepad1_right_stick_x;
  public float gamepad1_right_stick_y;
  public float gamepad1_left_stick_x;
  public float gamepad1_left_stick_y;
  [HideInInspector]
  public bool gamepad1_dpad_down;
  [HideInInspector]
  public bool gamepad1_dpad_up;
  [HideInInspector]
  public bool gamepad1_dpad_right;
  [HideInInspector]
  public bool gamepad1_dpad_left;
  [HideInInspector]
  public bool gamepad1_right_bumper;
  [HideInInspector]
  public bool gamepad1_left_bumper;
  [HideInInspector]
  public float gamepad1_left_trigger;
  [HideInInspector]
  public float gamepad1_right_trigger;
  [HideInInspector]
  public bool gamepad1_stop;
  [HideInInspector]
  public bool gamepad1_restart;
  [HideInInspector]
  public bool gamepad1_reset;
  [HideInInspector]
  public bool gamepad1_a_changed;
  [HideInInspector]
  public bool gamepad1_b_changed;
  [HideInInspector]
  public bool gamepad1_x_changed;
  [HideInInspector]
  public bool gamepad1_y_changed;
  [HideInInspector]
  public bool gamepad1_right_bumper_changed;
  [HideInInspector]
  public bool gamepad1_left_bumper_changed;
  [HideInInspector]
  public bool gamepad1_a_previous;
  [HideInInspector]
  public bool gamepad1_b_previous;
  [HideInInspector]
  public bool gamepad1_x_previous;
  [HideInInspector]
  public bool gamepad1_y_previous;
  [HideInInspector]
  public bool gamepad1_right_bumper_previous;
  [HideInInspector]
  public bool gamepad1_left_bumper_previous;
  [HideInInspector]
  public bool gamepad1_dpad_down_old;
  [HideInInspector]
  public bool gamepad1_dpad_up_old;
  [HideInInspector]
  public bool gamepad1_dpad_left_old;
  [HideInInspector]
  public bool gamepad1_dpad_right_old;
  [HideInInspector]
  public bool gamepad1_stop_old;
  [HideInInspector]
  public bool gamepad1_restart_old;
  [HideInInspector]
  public bool gamepad1_reset_old;
  [HideInInspector]
  public bool gamepad1_dpad_down_changed;
  [HideInInspector]
  public bool gamepad1_dpad_up_changed;
  [HideInInspector]
  public bool gamepad1_dpad_left_changed;
  [HideInInspector]
  public bool gamepad1_dpad_right_changed;
  [HideInInspector]
  public bool gamepad1_stop_changed;
  [HideInInspector]
  public bool gamepad1_restart_changed;
  [HideInInspector]
  public bool gamepad1_reset_changed;
  [HideInInspector]
  public float gamepad1_right_stick_y_old;
  [HideInInspector]
  public float gamepad1_right_stick_x_old;
  [HideInInspector]
  public float gamepad1_left_stick_x_old;
  [HideInInspector]
  public float gamepad1_left_stick_y_old;
  public long time_last_button_activitiy;
  public float turning_scaler = 1f;
  private double Deg2Rad = Math.PI / 180.0;
  [Tooltip("Maximum speed in ft/s")]
  public float max_speed = 20f;
  public bool max_speed_lock;
  public float max_speed_corr = 128.8f;
  [Tooltip("Maximum acceleration in ft/s2")]
  public float max_acceleration = 5f;
  public bool max_acceleration_lock;
  public float max_torque = 1000f;
  [Tooltip("Allows turnign to be faster/slower")]
  public float turn_scale = 1f;
  public bool turn_scale_lock;
  private float motor_force_scalar = 1f;
  public float friction_torque_scaler = 1f;
  private Vector3 forceTL;
  private Vector3 forceBL;
  private Vector3 forceTR;
  private Vector3 forceBR;
  private JointDrive xdriveTL;
  private JointDrive yzdriveTL;
  private JointDrive xdriveTR;
  private JointDrive yzdriveTR;
  private JointDrive xdriveBL;
  private JointDrive yzdriveBL;
  private JointDrive xdriveBR;
  private JointDrive yzdriveBR;
  private JointDrive xdriveML;
  private JointDrive yzdriveML;
  private JointDrive xdriveMR;
  private JointDrive yzdriveMR;
  private Vector3 straffing_vec = new Vector3(0.0f, 0.0f, 1f);
  public Vector3 TL_current;
  public Vector3 TL_goaly;
  private Vector3 diagonal_rotation = new Vector3(1f, 0.0f, 1f);
  public float ML_torque_multiplier = 1f;
  public float MR_torque_multiplier = 1f;
  private float averageMovementForceMagnitude;
  private float averageVelocityMagnitude;
  private float max_torque_saved;
  private float max_speed_saved;
  public float stick_multiplier = 1f;
  private List<Renderer> saved_renderers = new List<Renderer>();
  private List<Material> saved_materials = new List<Material>();
  private Material material_translucent;
  private bool invisible;
  private Vector3 point1;
  public float time1;
  private Vector3 point2;
  private float time2 = 0.05f;
  private Vector3 point3;
  private float time3 = 0.1f;
  public float[] lastacc = new float[5];
  private int countdown;
  public float velocity1;
  public float velocity2;
  public float accel;
  public float maxvel;
  public float timemaxvel;
  public float maxacc;
  public float timemaxacc;
  private bool accreset = true;
  public float old_rot_intertia_scaler;
  private bool logging;
  private float logtime;
  private List<RobotInterface3D.HingeData> hinge_list = new List<RobotInterface3D.HingeData>();
  private int hinge_index;
  private List<RobotInterface3D.SlideData> slide_list = new List<RobotInterface3D.SlideData>();
  private int slide_index;
  public float reset_duration = -1f;
  private Dictionary<string, Material> material_cache = new Dictionary<string, Material>();
  private float progress_value;
  public bool hold_position;
  public List<RobotInterface3D> enemies = new List<RobotInterface3D>();
  private List<int> enemies_collisions = new List<int>();
  private bool disabledTopObjects_old;

  private void OnEnable() => this.time_last_button_activitiy = MyUtils.GetTimeMillis();

  public virtual void Start()
  {
    this.Initialize();
    foreach (Transform componentsInChild in this.GetComponentsInChildren<Transform>(true))
      this.starting_positions[componentsInChild.GetInstanceID()] = new RobotInterface3D.ObjPos()
      {
        pos = componentsInChild.localPosition,
        rot = componentsInChild.localRotation
      };
  }

  public void ResetPosition(float x_off = 0.0f, float y_off = 0.0f, float z_off = 0.0f)
  {
    foreach (Transform componentsInChild in this.GetComponentsInChildren<Transform>(true))
    {
      int instanceId = componentsInChild.GetInstanceID();
      if (this.starting_positions.ContainsKey(instanceId) && !((UnityEngine.Object) componentsInChild == (UnityEngine.Object) this.transform))
      {
        componentsInChild.localPosition = !((UnityEngine.Object) componentsInChild.parent == (UnityEngine.Object) this.transform) ? this.starting_positions[instanceId].pos : this.starting_positions[instanceId].pos + new Vector3(x_off, y_off, z_off);
        componentsInChild.localRotation = this.starting_positions[instanceId].rot;
      }
    }
  }

  private void WriteLog(string text)
  {
    if (!this.DEBUG || this.mylogfile == null)
      return;
    byte[] bytes = this.encoding.GetBytes(text);
    this.mylogfile.Write(bytes, 0, bytes.Length);
  }

  private void OnDestroy()
  {
    if (this.mylogfile == null)
      return;
    this.mylogfile.Close();
  }

  public void SetSpeedParametersToGlobals()
  {
    if (this.lock_all_parameters)
      return;
    this.max_speed = GLOBALS.speed;
    this.max_acceleration = GLOBALS.acceleration;
    this.total_weight = GLOBALS.weight;
  }

  public void SetUserParameters(
    float new_max_speed = -1f,
    float new_max_acceleration = -1f,
    float new_total_weight = -1f,
    string new_DriveTrain = "",
    float new_turn_scale = -1f,
    int new_field_centric = -1,
    int new_active_breaking = -1,
    int new_tank_control = -1)
  {
    if (!this.max_speed_lock && !this.lock_all_parameters)
      this.max_speed = (double) new_max_speed < 0.0 ? GLOBALS.speed : new_max_speed;
    if (!this.max_acceleration_lock && !this.lock_all_parameters)
      this.max_acceleration = (double) new_max_acceleration < 0.0 ? GLOBALS.acceleration : new_max_acceleration;
    if (!this.total_weight_lock && !this.lock_all_parameters)
      this.total_weight = (double) new_total_weight < 0.0 ? GLOBALS.weight : new_total_weight;
    if (!this.DriveTrain_lock)
    {
      this.DriveTrain = new_DriveTrain.Length < 1 ? GLOBALS.DriveTrain : new_DriveTrain;
      if (this.DriveTrain == "6-Wheel Tank")
      {
        if ((bool) (UnityEngine.Object) this.wheelML)
          this.wheelML.SetActive(true);
        if ((bool) (UnityEngine.Object) this.wheelMR)
          this.wheelMR.SetActive(true);
      }
      else
      {
        if ((bool) (UnityEngine.Object) this.wheelML)
          this.wheelML.SetActive(false);
        if ((bool) (UnityEngine.Object) this.wheelMR)
          this.wheelMR.SetActive(false);
      }
    }
    if (!this.turn_scale_lock)
      this.turn_scale = (double) new_turn_scale < 0.0 ? GLOBALS.turning_scaler : new_turn_scale;
    if (!this.fieldcentric_lock)
      this.fieldcentric = new_field_centric < 0 ? GLOBALS.fieldcentric : new_field_centric == 1;
    if (!this.activebreaking_lock)
      this.activebreaking = new_active_breaking < 0 ? GLOBALS.activebreaking : new_active_breaking == 1;
    if (this.tankcontrol_lock)
      return;
    this.tankcontrol = new_tank_control < 0 ? GLOBALS.tankcontrol : new_tank_control == 1;
  }

  public void Initialize()
  {
    if (this.DEBUG && this.mylogfile == null)
    {
      this.mylogfile = new FileStream(Application.persistentDataPath + "\\FTCsim debug.csv", FileMode.Append);
      this.encoding = new UnicodeEncoding();
      this.WriteLog("Log file started " + (object) DateTime.Now + "\n");
    }
    if (!(bool) (UnityEngine.Object) this.transform.Find("Body"))
      return;
    this.rb_body = this.transform.Find("Body").GetComponent<Rigidbody>();
    if (!(bool) (UnityEngine.Object) this.rb_body || !this.rb_body.gameObject.activeSelf)
      return;
    this.rb_body.centerOfMass = this.centerOfMass;
    this.SetTotalWeight(this.total_weight, this.rot_inertia_scaler);
    this.max_speed_corr = (float) ((double) this.max_speed * 12.0 * 2.0 / 3.43339991569519);
    this.max_torque = (float) ((double) this.total_weight * ((double) this.max_acceleration + (double) GLOBALS.friction) / 75.0) / this.max_speed_corr;
    this.friction_torque_scaler = this.total_weight / 75f;
    if (this.isSpectator)
      return;
    this.InitWheel(this.wheelTL, ref this.wheelTL_joint, ref this.wheelTL_body);
    this.InitWheel(this.wheelTR, ref this.wheelTR_joint, ref this.wheelTR_body);
    this.InitWheel(this.wheelBL, ref this.wheelBL_joint, ref this.wheelBL_body);
    this.InitWheel(this.wheelBR, ref this.wheelBR_joint, ref this.wheelBR_body);
    if ((bool) (UnityEngine.Object) this.wheelML)
      this.InitWheel(this.wheelML, ref this.wheelML_joint, ref this.wheelML_body);
    if ((bool) (UnityEngine.Object) this.wheelMR)
      this.InitWheel(this.wheelMR, ref this.wheelMR_joint, ref this.wheelMR_body);
    if (this.DriveTrain == "Tank")
    {
      this.wheelBL.GetComponent<ConfigurableJoint>().angularYMotion = ConfigurableJointMotion.Locked;
      this.wheelBL.GetComponent<ConfigurableJoint>().angularZMotion = ConfigurableJointMotion.Locked;
      this.wheelBR.GetComponent<ConfigurableJoint>().angularYMotion = ConfigurableJointMotion.Locked;
      this.wheelBR.GetComponent<ConfigurableJoint>().angularZMotion = ConfigurableJointMotion.Locked;
    }
    else if (this.DriveTrain == "6-Wheel Tank")
    {
      if ((bool) (UnityEngine.Object) this.wheelML)
      {
        this.wheelML.GetComponent<ConfigurableJoint>().angularYMotion = ConfigurableJointMotion.Locked;
        this.wheelML.GetComponent<ConfigurableJoint>().angularZMotion = ConfigurableJointMotion.Locked;
      }
      if ((bool) (UnityEngine.Object) this.wheelMR)
      {
        this.wheelMR.GetComponent<ConfigurableJoint>().angularYMotion = ConfigurableJointMotion.Locked;
        this.wheelMR.GetComponent<ConfigurableJoint>().angularZMotion = ConfigurableJointMotion.Locked;
      }
      this.wheelTL.GetComponent<ConfigurableJoint>().angularYMotion = ConfigurableJointMotion.Free;
      this.wheelTL.GetComponent<ConfigurableJoint>().angularZMotion = ConfigurableJointMotion.Free;
      this.wheelTR.GetComponent<ConfigurableJoint>().angularYMotion = ConfigurableJointMotion.Free;
      this.wheelTR.GetComponent<ConfigurableJoint>().angularZMotion = ConfigurableJointMotion.Free;
      this.wheelBL.GetComponent<ConfigurableJoint>().angularYMotion = ConfigurableJointMotion.Free;
      this.wheelBL.GetComponent<ConfigurableJoint>().angularZMotion = ConfigurableJointMotion.Free;
      this.wheelBR.GetComponent<ConfigurableJoint>().angularYMotion = ConfigurableJointMotion.Free;
      this.wheelBR.GetComponent<ConfigurableJoint>().angularZMotion = ConfigurableJointMotion.Free;
    }
    else
    {
      this.wheelBL.GetComponent<ConfigurableJoint>().angularYMotion = ConfigurableJointMotion.Free;
      this.wheelBL.GetComponent<ConfigurableJoint>().angularZMotion = ConfigurableJointMotion.Free;
      this.wheelBR.GetComponent<ConfigurableJoint>().angularYMotion = ConfigurableJointMotion.Free;
      this.wheelBR.GetComponent<ConfigurableJoint>().angularZMotion = ConfigurableJointMotion.Free;
    }
    if ((UnityEngine.Object) this.rb_body.gameObject.GetComponent<PassCollisionsUp>() == (UnityEngine.Object) null)
      this.rb_body.gameObject.AddComponent<PassCollisionsUp>().owner = this;
    if ((UnityEngine.Object) this.progressmeter == (UnityEngine.Object) null)
    {
      GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(UnityEngine.Resources.Load("Prefabs/ProgressMeter") as GameObject, this.rb_body.transform);
      Vector3 localScale = gameObject.transform.localScale;
      localScale.x *= 1f / this.rb_body.transform.localScale.x;
      localScale.y *= 1f / this.rb_body.transform.localScale.y;
      localScale.z *= 1f / this.rb_body.transform.localScale.z;
      gameObject.transform.localScale = localScale * (this.is_FRC ? 1.3f : 1f);
      this.progressmeter = gameObject.GetComponent<ProgressMeter>();
    }
    if ((UnityEngine.Object) this.highlitecircle == (UnityEngine.Object) null)
    {
      GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(UnityEngine.Resources.Load("Prefabs/HighliteCircle") as GameObject, this.rb_body.transform);
      Vector3 localScale = gameObject.transform.localScale;
      localScale.x *= 1f / this.rb_body.transform.localScale.x;
      localScale.y *= 1f / this.rb_body.transform.localScale.y;
      localScale.z *= 1f / this.rb_body.transform.localScale.z;
      gameObject.transform.localScale = localScale * (this.is_FRC ? 1.3f : 1f);
      this.highlitecircle = gameObject;
      this.highlitecircle.SetActive(false);
    }
    if ((UnityEngine.Object) this.selectcircle == (UnityEngine.Object) null)
    {
      GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(UnityEngine.Resources.Load("Prefabs/SelectCircle") as GameObject, this.rb_body.transform);
      Vector3 localScale = gameObject.transform.localScale;
      localScale.x *= 1f / this.rb_body.transform.localScale.x;
      localScale.y *= 1f / this.rb_body.transform.localScale.y;
      localScale.z *= 1f / this.rb_body.transform.localScale.z;
      gameObject.transform.localScale = localScale * (this.is_FRC ? 1.3f : 1f);
      this.selectcircle = gameObject;
      this.selectcircle.SetActive(false);
    }
    this.myRobotID = this.gameObject.GetComponent<RobotID>();
    if ((UnityEngine.Object) this.myRobotID != (UnityEngine.Object) null)
    {
      this.myRobotID.is_red = this.myRobotID.starting_pos.StartsWith("Red");
      if (this.robot_color != -1)
        this.SetRobotColor(this.robot_color, true);
      else
        this.SetColorFromPosition(this.myRobotID.starting_pos);
    }
    this.Init_Robot();
  }

  private void InitWheel(
    GameObject wheel,
    ref ConfigurableJoint wheel_joint,
    ref Rigidbody wheel_body)
  {
    wheel_joint = wheel.GetComponent<ConfigurableJoint>();
    wheel_body = wheel_joint.GetComponent<Rigidbody>();
    wheel_joint.configuredInWorldSpace = false;
    wheel_body.maxAngularVelocity = this.max_speed_multiplier * this.max_speed_corr;
    JointDrive angularXdrive = wheel_joint.angularXDrive with
    {
      maximumForce = 9.999999E+27f,
      positionSpring = 0.0f
    };
    wheel_joint.angularXDrive = angularXdrive;
    JointDrive angularYzDrive = wheel_joint.angularYZDrive with
    {
      maximumForce = 9.999999E+27f,
      positionSpring = 0.0f
    };
    wheel_joint.angularYZDrive = angularYzDrive;
  }

  public void updateGamepadVars()
  {
    this.gamepad1_a = GLOBALS.JoystickMap["Jcontrolls_A"].GetButton();
    this.gamepad1_b = GLOBALS.JoystickMap["Jcontrolls_B"].GetButton();
    this.gamepad1_x = GLOBALS.JoystickMap["Jcontrolls_X"].GetButton();
    this.gamepad1_y = GLOBALS.JoystickMap["Jcontrolls_Y"].GetButton();
    this.gamepad1_right_stick_y = GLOBALS.JoystickMap["Jcontrolls_right_y"].GetAxis();
    this.gamepad1_right_stick_x = GLOBALS.JoystickMap["Jcontrolls_turn"].GetAxis() * this.turning_scaler;
    this.gamepad1_left_stick_x = GLOBALS.JoystickMap["Jcontrolls_move_lr"].GetAxis() * this.turning_scaler;
    this.gamepad1_left_stick_y = GLOBALS.JoystickMap["Jcontrolls_move_ud"].GetAxis();
    this.gamepad1_dpad_down = (double) GLOBALS.JoystickMap["Jcontrolls_dpad_ud"].GetAxis() > 0.5;
    this.gamepad1_dpad_up = (double) GLOBALS.JoystickMap["Jcontrolls_dpad_ud"].GetAxis() < -0.5;
    this.gamepad1_dpad_left = (double) GLOBALS.JoystickMap["Jcontrolls_dpad_lr"].GetAxis() < -0.5;
    this.gamepad1_dpad_right = (double) GLOBALS.JoystickMap["Jcontrolls_dpad_lr"].GetAxis() > 0.5;
    this.gamepad1_right_bumper = GLOBALS.JoystickMap["Jcontrolls_RB"].GetButton();
    this.gamepad1_left_bumper = GLOBALS.JoystickMap["Jcontrolls_LB"].GetButton();
    this.gamepad1_left_trigger = GLOBALS.JoystickMap["Jcontrolls_LTR"].GetAxis();
    this.gamepad1_right_trigger = GLOBALS.JoystickMap["Jcontrolls_RTR"].GetAxis();
    this.gamepad1_stop = GLOBALS.JoystickMap["Jcontrolls_stop"].GetButton();
    this.gamepad1_restart = GLOBALS.JoystickMap["Jcontrolls_restart"].GetButton();
    if (GLOBALS.keyboard_inuse)
      return;
    if (Input.GetKey(KeyCode.LeftShift))
      this.gamepad1_left_bumper = Input.GetKey(KeyCode.LeftShift);
    if (Input.GetKey(KeyCode.RightShift))
      this.gamepad1_right_bumper = Input.GetKey(KeyCode.RightShift);
    try
    {
      if (Input.GetKey(GLOBALS.KeyboardMap["controlls_A"].key))
        this.gamepad1_a = true;
    }
    catch (Exception ex)
    {
    }
    try
    {
      if (Input.GetKey(GLOBALS.KeyboardMap["controlls_B"].key))
        this.gamepad1_b = true;
    }
    catch (Exception ex)
    {
    }
    try
    {
      if (Input.GetKey(GLOBALS.KeyboardMap["controlls_X"].key))
        this.gamepad1_x = true;
    }
    catch (Exception ex)
    {
    }
    try
    {
      if (Input.GetKey(GLOBALS.KeyboardMap["controlls_Y"].key))
        this.gamepad1_y = true;
    }
    catch (Exception ex)
    {
    }
    try
    {
      if (Input.GetKey(GLOBALS.KeyboardMap["controlls_LD"].key))
        this.gamepad1_right_trigger = 1f;
    }
    catch (Exception ex)
    {
    }
    try
    {
      if (Input.GetKey(GLOBALS.KeyboardMap["controlls_RD"].key))
        this.gamepad1_left_trigger = 1f;
    }
    catch (Exception ex)
    {
    }
    try
    {
      if (Input.GetKey(GLOBALS.KeyboardMap["controlls_dpad_u"].key))
        this.gamepad1_dpad_up = true;
    }
    catch (Exception ex)
    {
    }
    try
    {
      if (Input.GetKey(GLOBALS.KeyboardMap["controlls_dpad_d"].key))
        this.gamepad1_dpad_down = true;
    }
    catch (Exception ex)
    {
    }
    try
    {
      if (Input.GetKey(GLOBALS.KeyboardMap["controlls_dpad_l"].key))
        this.gamepad1_dpad_left = true;
    }
    catch (Exception ex)
    {
    }
    try
    {
      if (Input.GetKey(GLOBALS.KeyboardMap["controlls_dpad_r"].key))
        this.gamepad1_dpad_right = true;
    }
    catch (Exception ex)
    {
    }
    try
    {
      if (Input.GetKey(GLOBALS.KeyboardMap["controlls_move_u"].key))
        this.gamepad1_left_stick_y = -1f;
    }
    catch (Exception ex)
    {
    }
    try
    {
      if (Input.GetKey(GLOBALS.KeyboardMap["controlls_move_d"].key))
        this.gamepad1_left_stick_y = 1f;
    }
    catch (Exception ex)
    {
    }
    try
    {
      if (Input.GetKey(GLOBALS.KeyboardMap["controlls_move_l"].key))
        this.gamepad1_left_stick_x = -1f * this.turning_scaler;
    }
    catch (Exception ex)
    {
    }
    try
    {
      if (Input.GetKey(GLOBALS.KeyboardMap["controlls_move_r"].key))
        this.gamepad1_left_stick_x = 1f * this.turning_scaler;
    }
    catch (Exception ex)
    {
    }
    try
    {
      if (Input.GetKey(GLOBALS.KeyboardMap["controlls_turn_l"].key))
        this.gamepad1_right_stick_x = -1f * this.turning_scaler;
    }
    catch (Exception ex)
    {
    }
    try
    {
      if (Input.GetKey(GLOBALS.KeyboardMap["controlls_turn_r"].key))
        this.gamepad1_right_stick_x = 1f * this.turning_scaler;
    }
    catch (Exception ex)
    {
    }
    try
    {
      if (Input.GetKey(GLOBALS.KeyboardMap["controlls_stop"].key))
        this.gamepad1_stop = true;
    }
    catch (Exception ex)
    {
    }
    try
    {
      if (Input.GetKey(GLOBALS.KeyboardMap["controlls_restart"].key))
        this.gamepad1_restart = true;
    }
    catch (Exception ex)
    {
    }
    try
    {
      if (Input.GetKey(GLOBALS.KeyboardMap["controlls_reset"].key))
        this.gamepad1_reset = true;
    }
    catch (Exception ex)
    {
    }
    try
    {
      if (Input.GetKey(GLOBALS.KeyboardMap["controlls_rightstick_up"].key))
        this.gamepad1_right_stick_y = -1f;
    }
    catch (Exception ex)
    {
    }
    try
    {
      if (!Input.GetKey(GLOBALS.KeyboardMap["controlls_rightstick_down"].key))
        return;
      this.gamepad1_right_stick_y = 1f;
    }
    catch (Exception ex)
    {
    }
  }

  public void InputsChanges()
  {
    this.gamepad1_a_changed = this.gamepad1_a != this.gamepad1_a_previous;
    int num1 = 0 | (this.gamepad1_a_changed ? 1 : 0);
    this.gamepad1_b_changed = this.gamepad1_b != this.gamepad1_b_previous;
    int num2 = this.gamepad1_b_changed ? 1 : 0;
    int num3 = num1 | num2;
    this.gamepad1_x_changed = this.gamepad1_x != this.gamepad1_x_previous;
    int num4 = this.gamepad1_x_changed ? 1 : 0;
    int num5 = num3 | num4;
    this.gamepad1_y_changed = this.gamepad1_y != this.gamepad1_y_previous;
    int num6 = this.gamepad1_y_changed ? 1 : 0;
    int num7 = num5 | num6 | ((double) Math.Abs(this.gamepad1_right_stick_y - this.gamepad1_right_stick_y_old) > 0.00999999977648258 ? 1 : 0) | ((double) Math.Abs(this.gamepad1_right_stick_x - this.gamepad1_right_stick_x_old) > 0.00999999977648258 ? 1 : 0) | ((double) Math.Abs(this.gamepad1_left_stick_x - this.gamepad1_left_stick_x_old) > 0.00999999977648258 ? 1 : 0) | ((double) Math.Abs(this.gamepad1_left_stick_y - this.gamepad1_left_stick_y_old) > 0.00999999977648258 ? 1 : 0);
    this.gamepad1_dpad_down_changed = this.gamepad1_dpad_down != this.gamepad1_dpad_down_old;
    int num8 = this.gamepad1_dpad_down_changed ? 1 : 0;
    int num9 = num7 | num8;
    this.gamepad1_dpad_up_changed = this.gamepad1_dpad_up != this.gamepad1_dpad_up_old;
    int num10 = this.gamepad1_dpad_up_changed ? 1 : 0;
    int num11 = num9 | num10;
    this.gamepad1_dpad_left_changed = this.gamepad1_dpad_left != this.gamepad1_dpad_left_old;
    int num12 = this.gamepad1_dpad_left_changed ? 1 : 0;
    int num13 = num11 | num12;
    this.gamepad1_dpad_right_changed = this.gamepad1_dpad_right != this.gamepad1_dpad_right_old;
    int num14 = this.gamepad1_dpad_right_changed ? 1 : 0;
    int num15 = num13 | num14;
    this.gamepad1_stop_changed = this.gamepad1_stop != this.gamepad1_stop_old;
    int num16 = this.gamepad1_stop_changed ? 1 : 0;
    int num17 = num15 | num16;
    this.gamepad1_restart_changed = this.gamepad1_restart != this.gamepad1_restart_old;
    int num18 = this.gamepad1_restart_changed ? 1 : 0;
    int num19 = num17 | num18;
    this.gamepad1_reset_changed = this.gamepad1_reset != this.gamepad1_reset_old;
    int num20 = this.gamepad1_reset_changed ? 1 : 0;
    int num21 = num19 | num20;
    this.gamepad1_right_bumper_changed = this.gamepad1_right_bumper != this.gamepad1_right_bumper_previous;
    int num22 = this.gamepad1_right_bumper_changed ? 1 : 0;
    int num23 = num21 | num22;
    this.gamepad1_left_bumper_changed = this.gamepad1_left_bumper != this.gamepad1_left_bumper_previous;
    int num24 = this.gamepad1_left_bumper_changed ? 1 : 0;
    int num25 = num23 | num24;
    this.gamepad1_a_previous = this.gamepad1_a;
    this.gamepad1_b_previous = this.gamepad1_b;
    this.gamepad1_x_previous = this.gamepad1_x;
    this.gamepad1_y_previous = this.gamepad1_y;
    this.gamepad1_right_bumper_previous = this.gamepad1_right_bumper;
    this.gamepad1_left_bumper_previous = this.gamepad1_left_bumper;
    this.gamepad1_dpad_down_old = this.gamepad1_dpad_down;
    this.gamepad1_dpad_up_old = this.gamepad1_dpad_up;
    this.gamepad1_dpad_left_old = this.gamepad1_dpad_left;
    this.gamepad1_dpad_right_old = this.gamepad1_dpad_right;
    this.gamepad1_stop_old = this.gamepad1_stop;
    this.gamepad1_restart_old = this.gamepad1_restart;
    this.gamepad1_reset_old = this.gamepad1_reset;
    this.turning_scaler = 1f;
    if (this.gamepad1_left_bumper || this.gamepad1_right_bumper)
      this.turning_scaler = 0.3f;
    if (num25 == 0 && this.time_last_button_activitiy != 0L)
      return;
    this.time_last_button_activitiy = MyUtils.GetTimeMillis();
  }

  private Vector3 TransformWorldVelToWheelVel(Vector3 worldvector)
  {
    Vector3 vector3 = this.rb_body.transform.InverseTransformDirection(worldvector);
    return new Vector3(-1f * vector3.z, vector3.y, vector3.x);
  }

  private void ApplyWheelSpringForces(
    Vector3 TL_goal,
    Vector3 TR_goal,
    Vector3 BL_goal,
    Vector3 BR_goal)
  {
    if (!(bool) (UnityEngine.Object) this.rb_body || !this.rb_body.gameObject.activeSelf)
      return;
    this.averageMovementForceMagnitude = 0.0f;
    this.averageVelocityMagnitude = 0.0f;
    if (this.hold_position)
      return;
    Vector3 wheelVel1 = this.TransformWorldVelToWheelVel(this.wheelTL.GetComponent<Rigidbody>().angularVelocity);
    Vector3 wheelVel2 = this.TransformWorldVelToWheelVel(this.wheelTR.GetComponent<Rigidbody>().angularVelocity);
    Vector3 wheelVel3 = this.TransformWorldVelToWheelVel(this.wheelBL.GetComponent<Rigidbody>().angularVelocity);
    Vector3 wheelVel4 = this.TransformWorldVelToWheelVel(this.wheelBR.GetComponent<Rigidbody>().angularVelocity);
    Vector3 vector3_1 = new Vector3();
    Vector3 vector3_2 = new Vector3();
    if ((bool) (UnityEngine.Object) this.wheelML && (bool) (UnityEngine.Object) this.wheelMR && this.DriveTrain == "6-Wheel Tank")
    {
      vector3_1 = this.TransformWorldVelToWheelVel(this.wheelML.GetComponent<Rigidbody>().angularVelocity);
      vector3_2 = this.TransformWorldVelToWheelVel(this.wheelMR.GetComponent<Rigidbody>().angularVelocity);
    }
    this.TL_current = wheelVel1;
    this.TL_goaly = TL_goal;
    float num1 = this.max_torque;
    float num2 = this.max_torque;
    float num3 = this.max_torque;
    float num4 = this.max_torque;
    float num5 = this.max_torque;
    float num6 = this.max_torque;
    float num7 = 0.0f;
    if (!this.activebreaking && (double) TL_goal.magnitude < 0.899999976158142 * (double) wheelVel1.magnitude && (double) (TL_goal - wheelVel1).magnitude < 1.10000002384186 * (double) wheelVel1.magnitude)
    {
      num7 = GLOBALS.regen_breaking;
      num1 = 0.0f;
    }
    float num8 = 0.0f;
    if (!this.activebreaking && (double) TR_goal.magnitude < 0.899999976158142 * (double) wheelVel2.magnitude && (double) (TR_goal - wheelVel2).magnitude < 1.10000002384186 * (double) wheelVel2.magnitude)
    {
      num8 = GLOBALS.regen_breaking;
      num2 = 0.0f;
    }
    float num9 = 0.0f;
    if (!this.activebreaking && (double) BL_goal.magnitude < 0.899999976158142 * (double) wheelVel3.magnitude && (double) (BL_goal - wheelVel3).magnitude < 1.10000002384186 * (double) wheelVel3.magnitude)
    {
      num9 = GLOBALS.regen_breaking;
      num3 = 0.0f;
    }
    float num10 = 0.0f;
    if (!this.activebreaking && (double) BR_goal.magnitude < 0.899999976158142 * (double) wheelVel4.magnitude && (double) (BR_goal - wheelVel4).magnitude < 1.10000002384186 * (double) wheelVel4.magnitude)
    {
      num10 = GLOBALS.regen_breaking;
      num4 = 0.0f;
    }
    if (this.DriveTrain == "Tank")
    {
      TL_goal.x = 0.0f;
      TL_goal.y = 0.0f;
      TL_goal.z = 0.0f;
      TR_goal.x = 0.0f;
      TR_goal.y = 0.0f;
      TR_goal.z = 0.0f;
      num1 = 0.0f;
      num2 = 0.0f;
      num3 *= 2f;
      num4 *= 2f;
    }
    Vector3 vector3_3 = new Vector3();
    Vector3 vector3_4 = new Vector3();
    float num11 = 0.0f;
    float num12 = 0.0f;
    this.ML_torque_multiplier = 1f;
    this.MR_torque_multiplier = 1f;
    if (this.DriveTrain == "6-Wheel Tank")
    {
      TL_goal.y = 0.0f;
      TL_goal.z = 0.0f;
      TR_goal.y = 0.0f;
      TR_goal.z = 0.0f;
      BL_goal.y = 0.0f;
      BL_goal.z = 0.0f;
      BR_goal.y = 0.0f;
      BR_goal.z = 0.0f;
      TL_goal.x = BL_goal.x;
      TR_goal.x = BR_goal.x;
      vector3_3 = (TL_goal + BL_goal) / 2f;
      vector3_4 = (TR_goal + BR_goal) / 2f;
      num1 *= 1f;
      num3 *= 1f;
      num2 *= 1f;
      num4 *= 1f;
      num5 = (float) (((double) num1 + (double) num3) / 2.0);
      num6 = (float) (((double) num2 + (double) num4) / 2.0);
      if (!this.activebreaking && (double) vector3_3.magnitude < 0.899999976158142 * (double) vector3_1.magnitude && (double) (vector3_3 - vector3_1).magnitude < 1.10000002384186 * (double) vector3_1.magnitude)
      {
        num11 = GLOBALS.regen_breaking;
        num5 = 0.0f;
      }
      if (!this.activebreaking && (double) vector3_4.magnitude < 0.899999976158142 * (double) vector3_2.magnitude && (double) (vector3_4 - vector3_2).magnitude < 1.10000002384186 * (double) vector3_2.magnitude)
      {
        num12 = GLOBALS.regen_breaking;
        num6 = 0.0f;
      }
      if ((double) TL_goal.magnitude > 0.200000002980232 * (double) this.max_speed_corr && (double) wheelVel1.magnitude < 0.0500000007450581 * (double) this.max_speed_corr)
        num1 *= (float) (1.0 + (1.0 - (double) wheelVel1.magnitude / (0.0500000007450581 * (double) this.max_speed_corr)));
      if ((double) TR_goal.magnitude > 0.200000002980232 * (double) this.max_speed_corr && (double) wheelVel2.magnitude < 0.0500000007450581 * (double) this.max_speed_corr)
        num2 *= (float) (1.0 + (1.0 - (double) wheelVel2.magnitude / (0.0500000007450581 * (double) this.max_speed_corr)));
      if ((double) vector3_3.magnitude > 0.200000002980232 * (double) this.max_speed_corr && (double) vector3_1.magnitude < 0.0500000007450581 * (double) this.max_speed_corr)
        num5 *= (float) (1.0 + (1.0 - (double) vector3_1.magnitude / (0.0500000007450581 * (double) this.max_speed_corr)));
      if ((double) vector3_4.magnitude > 0.200000002980232 * (double) this.max_speed_corr && (double) vector3_2.magnitude < 0.0500000007450581 * (double) this.max_speed_corr)
        num6 *= (float) (1.0 + (1.0 - (double) vector3_2.magnitude / (0.0500000007450581 * (double) this.max_speed_corr)));
      if ((double) BL_goal.magnitude > 0.200000002980232 * (double) this.max_speed_corr && (double) wheelVel3.magnitude < 0.0500000007450581 * (double) this.max_speed_corr)
        num3 *= (float) (1.0 + (1.0 - (double) wheelVel3.magnitude / (0.0500000007450581 * (double) this.max_speed_corr)));
      if ((double) BR_goal.magnitude > 0.200000002980232 * (double) this.max_speed_corr && (double) wheelVel4.magnitude < 0.0500000007450581 * (double) this.max_speed_corr)
        num4 *= (float) (1.0 + (1.0 - (double) wheelVel4.magnitude / (0.0500000007450581 * (double) this.max_speed_corr)));
    }
    if (this.DriveTrain == "Mecanum")
    {
      float num13 = (float) Math.Abs(Math.Cos(2.0 * (double) Vector3.Angle(TL_goal, this.diagonal_rotation) * 3.14159250259399 / 180.0));
      float num14 = (float) Math.Abs(Math.Cos(2.0 * (double) Vector3.Angle(TR_goal, this.diagonal_rotation) * 3.14159250259399 / 180.0));
      float num15 = (float) Math.Abs(Math.Cos(2.0 * (double) Vector3.Angle(BL_goal, this.diagonal_rotation) * 3.14159250259399 / 180.0));
      float num16 = (float) Math.Abs(Math.Cos(2.0 * (double) Vector3.Angle(BR_goal, this.diagonal_rotation) * 3.14159250259399 / 180.0));
      TL_goal *= (float) (1.0 / (1.0 + 0.699999988079071 * (double) num13));
      TR_goal *= (float) (1.0 / (1.0 + 0.699999988079071 * (double) num14));
      BL_goal *= (float) (1.0 / (1.0 + 0.699999988079071 * (double) num15));
      BR_goal *= (float) (1.0 / (1.0 + 0.699999988079071 * (double) num16));
      num1 *= (float) Math.Pow(1.0 + 0.699999988079071 * (double) num13, 2.0);
      num2 *= (float) Math.Pow(1.0 + 0.699999988079071 * (double) num14, 2.0);
      num3 *= (float) Math.Pow(1.0 + 0.699999988079071 * (double) num15, 2.0);
      num4 *= (float) Math.Pow(1.0 + 0.699999988079071 * (double) num16, 2.0);
    }
    this.xdriveTL = this.wheelTL_joint.angularXDrive;
    this.xdriveTL.positionDamper = num1;
    this.wheelTL_joint.angularXDrive = this.xdriveTL;
    this.yzdriveTL = this.wheelTL_joint.angularYZDrive;
    this.yzdriveTL.positionDamper = num1;
    this.wheelTL_joint.angularYZDrive = this.yzdriveTL;
    this.xdriveTR = this.wheelTR_joint.angularXDrive;
    this.xdriveTR.positionDamper = num2;
    this.wheelTR_joint.angularXDrive = this.xdriveTR;
    this.yzdriveTR = this.wheelTR_joint.angularYZDrive;
    this.yzdriveTR.positionDamper = num2;
    this.wheelTR_joint.angularYZDrive = this.yzdriveTR;
    this.xdriveBL = this.wheelBL_joint.angularXDrive;
    this.xdriveBL.positionDamper = num3;
    this.wheelBL_joint.angularXDrive = this.xdriveBL;
    this.yzdriveBL = this.wheelBL_joint.angularYZDrive;
    this.yzdriveBL.positionDamper = num3;
    this.wheelBL_joint.angularYZDrive = this.yzdriveBL;
    this.xdriveBR = this.wheelBR_joint.angularXDrive;
    this.xdriveBR.positionDamper = num4;
    this.wheelBR_joint.angularXDrive = this.xdriveBR;
    this.yzdriveBR = this.wheelBR_joint.angularYZDrive;
    this.yzdriveBR.positionDamper = num4;
    this.wheelBR_joint.angularYZDrive = this.yzdriveBR;
    this.wheelTL_joint.targetAngularVelocity = TL_goal;
    this.wheelTR_joint.targetAngularVelocity = TR_goal;
    this.wheelBL_joint.targetAngularVelocity = BL_goal;
    this.wheelBR_joint.targetAngularVelocity = BR_goal;
    if (this.DriveTrain == "6-Wheel Tank")
    {
      this.xdriveML = this.wheelML_joint.angularXDrive;
      this.xdriveML.positionDamper = num5;
      this.wheelML_joint.angularXDrive = this.xdriveML;
      this.yzdriveML = this.wheelML_joint.angularYZDrive;
      this.yzdriveML.positionDamper = num5;
      this.wheelML_joint.angularYZDrive = this.yzdriveML;
      this.xdriveMR = this.wheelMR_joint.angularXDrive;
      this.xdriveMR.positionDamper = num6;
      this.wheelMR_joint.angularXDrive = this.xdriveMR;
      this.yzdriveMR = this.wheelMR_joint.angularYZDrive;
      this.yzdriveMR.positionDamper = num6;
      this.wheelMR_joint.angularYZDrive = this.yzdriveMR;
      this.wheelML_joint.targetAngularVelocity = vector3_3;
      this.wheelMR_joint.targetAngularVelocity = vector3_4;
    }
    this.averageMovementForceMagnitude = (float) (((double) TL_goal.magnitude + (double) TR_goal.magnitude + (double) BL_goal.magnitude + (double) BR_goal.magnitude) / 4.0) / this.max_speed_corr;
    this.averageVelocityMagnitude = (float) (((double) wheelVel1.magnitude + (double) wheelVel2.magnitude + (double) wheelVel3.magnitude + (double) wheelVel4.magnitude) / 4.0) / this.max_speed_corr;
    this.wheelTL_body.AddTorque(-this.wheelTL_body.angularVelocity.normalized * this.friction_torque_scaler * (GLOBALS.friction + num7 * this.max_acceleration));
    this.wheelBL_body.AddTorque(-this.wheelBL_body.angularVelocity.normalized * this.friction_torque_scaler * (GLOBALS.friction + num9 * this.max_acceleration));
    this.wheelTR_body.AddTorque(-this.wheelTR_body.angularVelocity.normalized * this.friction_torque_scaler * (GLOBALS.friction + num8 * this.max_acceleration));
    Rigidbody wheelBrBody = this.wheelBR_body;
    Vector3 angularVelocity = this.wheelBR_body.angularVelocity;
    Vector3 torque1 = -angularVelocity.normalized * this.friction_torque_scaler * (GLOBALS.friction + num10 * this.max_acceleration);
    wheelBrBody.AddTorque(torque1);
    if ((bool) (UnityEngine.Object) this.wheelML_body && (bool) (UnityEngine.Object) this.wheelMR_body && this.DriveTrain == "6-Wheel Tank")
    {
      Rigidbody wheelMlBody = this.wheelML_body;
      angularVelocity = this.wheelML_body.angularVelocity;
      Vector3 torque2 = -angularVelocity.normalized * this.friction_torque_scaler * (GLOBALS.friction + num11 * this.max_acceleration);
      wheelMlBody.AddTorque(torque2);
      Rigidbody wheelMrBody = this.wheelMR_body;
      angularVelocity = this.wheelMR_body.angularVelocity;
      Vector3 torque3 = -angularVelocity.normalized * this.friction_torque_scaler * (GLOBALS.friction + num12 * this.max_acceleration);
      wheelMrBody.AddTorque(torque3);
    }
    if (!(this.DriveTrain == "Mecanum"))
      return;
    Vector3 vector3_5 = new Vector3(-1f * this.rb_body.transform.InverseTransformDirection(this.wheelTL_body.angularVelocity).x, 0.0f, 0.0f);
    this.wheelTL_body.AddTorque(this.rb_body.transform.TransformDirection(vector3_5.normalized) * GLOBALS.strafing_friction * GLOBALS.friction * this.friction_torque_scaler);
    vector3_5 = new Vector3(-1f * this.rb_body.transform.InverseTransformDirection(this.wheelTR_body.angularVelocity).x, 0.0f, 0.0f);
    this.wheelTR_body.AddTorque(this.rb_body.transform.TransformDirection(vector3_5.normalized) * GLOBALS.strafing_friction * GLOBALS.friction * this.friction_torque_scaler);
    vector3_5 = new Vector3(-1f * this.rb_body.transform.InverseTransformDirection(this.wheelBR_body.angularVelocity).x, 0.0f, 0.0f);
    this.wheelBR_body.AddTorque(this.rb_body.transform.TransformDirection(vector3_5.normalized) * GLOBALS.strafing_friction * GLOBALS.friction * this.friction_torque_scaler);
    vector3_5 = new Vector3(-1f * this.rb_body.transform.InverseTransformDirection(this.wheelBL_body.angularVelocity).x, 0.0f, 0.0f);
    this.wheelBL_body.AddTorque(this.rb_body.transform.TransformDirection(vector3_5.normalized) * GLOBALS.strafing_friction * GLOBALS.friction * this.friction_torque_scaler);
  }

  private void ApplyWheelSpringForces_oldversion(
    Vector3 TL_goal,
    Vector3 TR_goal,
    Vector3 BL_goal,
    Vector3 BR_goal)
  {
    if (this.hold_position)
      return;
    Vector3 wheelVel1 = this.TransformWorldVelToWheelVel(this.wheelTL.GetComponent<Rigidbody>().angularVelocity);
    Vector3 wheelVel2 = this.TransformWorldVelToWheelVel(this.wheelTR.GetComponent<Rigidbody>().angularVelocity);
    Vector3 wheelVel3 = this.TransformWorldVelToWheelVel(this.wheelBL.GetComponent<Rigidbody>().angularVelocity);
    Vector3 wheelVel4 = this.TransformWorldVelToWheelVel(this.wheelBR.GetComponent<Rigidbody>().angularVelocity);
    float num1 = this.max_torque;
    float num2 = this.max_torque;
    float num3 = this.max_torque;
    float num4 = this.max_torque;
    float num5 = 0.0f;
    if ((double) TL_goal.magnitude < 0.899999976158142 * (double) wheelVel1.magnitude && (double) (TL_goal - wheelVel1).magnitude < 1.10000002384186 * (double) wheelVel1.magnitude)
    {
      num5 = GLOBALS.regen_breaking;
      num1 = 0.0f;
    }
    float num6 = 0.0f;
    if ((double) TR_goal.magnitude < 0.899999976158142 * (double) wheelVel2.magnitude && (double) (TR_goal - wheelVel2).magnitude < 1.10000002384186 * (double) wheelVel2.magnitude)
    {
      num6 = GLOBALS.regen_breaking;
      num2 = 0.0f;
    }
    float num7 = 0.0f;
    if ((double) BL_goal.magnitude < 0.899999976158142 * (double) wheelVel3.magnitude && (double) (BL_goal - wheelVel3).magnitude < 1.10000002384186 * (double) wheelVel3.magnitude)
    {
      num7 = GLOBALS.regen_breaking;
      num3 = 0.0f;
    }
    float num8 = 0.0f;
    if ((double) BR_goal.magnitude < 0.899999976158142 * (double) wheelVel4.magnitude && (double) (BR_goal - wheelVel4).magnitude < 1.10000002384186 * (double) wheelVel4.magnitude)
    {
      num8 = GLOBALS.regen_breaking;
      num4 = 0.0f;
    }
    if (this.DriveTrain == "Tank" || this.DriveTrain == "6-Wheel Tank")
    {
      TL_goal.x = 0.0f;
      TL_goal.y = 0.0f;
      TL_goal.z = 0.0f;
      TR_goal.x = 0.0f;
      TR_goal.y = 0.0f;
      TR_goal.z = 0.0f;
      num1 = (float) ((double) this.motor_force_scalar * (double) this.max_torque * 0.200000002980232);
      num2 = (float) ((double) this.motor_force_scalar * (double) this.max_torque * 0.200000002980232);
    }
    this.xdriveTL = this.wheelTL_joint.angularXDrive;
    this.xdriveTL.positionDamper = this.motor_force_scalar * num1;
    this.wheelTL_joint.angularXDrive = this.xdriveTL;
    this.yzdriveTL = this.wheelTL_joint.angularYZDrive;
    this.yzdriveTL.positionDamper = this.motor_force_scalar * num1;
    this.wheelTL_joint.angularYZDrive = this.yzdriveTL;
    this.xdriveTR = this.wheelTR_joint.angularXDrive;
    this.xdriveTR.positionDamper = this.motor_force_scalar * num2;
    this.wheelTR_joint.angularXDrive = this.xdriveTR;
    this.yzdriveTR = this.wheelTR_joint.angularYZDrive;
    this.yzdriveTR.positionDamper = this.motor_force_scalar * num2;
    this.wheelTR_joint.angularYZDrive = this.yzdriveTR;
    this.xdriveBL = this.wheelBL_joint.angularXDrive;
    this.xdriveBL.positionDamper = this.motor_force_scalar * num3;
    this.wheelBL_joint.angularXDrive = this.xdriveBL;
    this.yzdriveBL = this.wheelBL_joint.angularYZDrive;
    this.yzdriveBL.positionDamper = this.motor_force_scalar * num3;
    this.wheelBL_joint.angularYZDrive = this.yzdriveBL;
    this.xdriveBR = this.wheelBR_joint.angularXDrive;
    this.xdriveBR.positionDamper = this.motor_force_scalar * num4;
    this.wheelBR_joint.angularXDrive = this.xdriveBR;
    this.yzdriveBR = this.wheelBR_joint.angularYZDrive;
    this.yzdriveBR.positionDamper = this.motor_force_scalar * num4;
    this.wheelBR_joint.angularYZDrive = this.yzdriveBR;
    this.wheelTL_joint.targetAngularVelocity = TL_goal;
    this.wheelTR_joint.targetAngularVelocity = TR_goal;
    this.wheelBL_joint.targetAngularVelocity = BL_goal;
    this.wheelBR_joint.targetAngularVelocity = BR_goal;
    Rigidbody wheelTlBody = this.wheelTL_body;
    Vector3 angularVelocity = this.wheelTL_body.angularVelocity;
    Vector3 torque1 = -angularVelocity.normalized * this.friction_torque_scaler * (GLOBALS.friction + num5 * this.max_acceleration);
    wheelTlBody.AddTorque(torque1);
    Rigidbody wheelBlBody = this.wheelBL_body;
    angularVelocity = this.wheelBL_body.angularVelocity;
    Vector3 torque2 = -angularVelocity.normalized * this.friction_torque_scaler * (GLOBALS.friction + num7 * this.max_acceleration);
    wheelBlBody.AddTorque(torque2);
    Rigidbody wheelTrBody = this.wheelTR_body;
    angularVelocity = this.wheelTR_body.angularVelocity;
    Vector3 torque3 = -angularVelocity.normalized * this.friction_torque_scaler * (GLOBALS.friction + num6 * this.max_acceleration);
    wheelTrBody.AddTorque(torque3);
    Rigidbody wheelBrBody = this.wheelBR_body;
    angularVelocity = this.wheelBR_body.angularVelocity;
    Vector3 torque4 = -angularVelocity.normalized * this.friction_torque_scaler * (GLOBALS.friction + num8 * this.max_acceleration);
    wheelBrBody.AddTorque(torque4);
    Vector3 vector3 = new Vector3(-1f * this.rb_body.transform.InverseTransformDirection(this.wheelTL_body.angularVelocity).x, 0.0f, 0.0f);
    this.wheelTL_body.AddTorque(this.rb_body.transform.TransformDirection(vector3.normalized) * GLOBALS.strafing_friction * GLOBALS.friction * this.friction_torque_scaler);
    vector3 = new Vector3(-1f * this.rb_body.transform.InverseTransformDirection(this.wheelTR_body.angularVelocity).x, 0.0f, 0.0f);
    this.wheelTR_body.AddTorque(this.rb_body.transform.TransformDirection(vector3.normalized) * GLOBALS.strafing_friction * GLOBALS.friction * this.friction_torque_scaler);
    vector3 = new Vector3(-1f * this.rb_body.transform.InverseTransformDirection(this.wheelBR_body.angularVelocity).x, 0.0f, 0.0f);
    this.wheelBR_body.AddTorque(this.rb_body.transform.TransformDirection(vector3.normalized) * GLOBALS.strafing_friction * GLOBALS.friction * this.friction_torque_scaler);
    vector3 = new Vector3(-1f * this.rb_body.transform.InverseTransformDirection(this.wheelBL_body.angularVelocity).x, 0.0f, 0.0f);
    this.wheelBL_body.AddTorque(this.rb_body.transform.TransformDirection(vector3.normalized) * GLOBALS.strafing_friction * GLOBALS.friction * this.friction_torque_scaler);
  }

  public float getAverageMovementForceMagnitude() => this.averageMovementForceMagnitude;

  public float getAveragVelocityMagnitude() => this.averageVelocityMagnitude;

  public void TweakPerformance(float speed_multiplier_new, float torque_multiplier_new)
  {
    float num1 = speed_multiplier_new;
    float num2 = torque_multiplier_new;
    if ((double) speed_multiplier_new != 0.0)
      num2 /= speed_multiplier_new;
    if ((double) torque_multiplier_new != 0.0)
      num1 /= (float) (((double) num2 - 1.0) * 0.0500000007450581 + 1.0);
    if ((double) num2 == 0.0)
    {
      if ((double) this.max_torque_saved != 0.0)
      {
        this.max_torque = this.max_torque_saved;
        this.max_torque_saved = 0.0f;
      }
    }
    else
    {
      if ((double) this.max_torque_saved == 0.0)
        this.max_torque_saved = this.max_torque;
      this.max_torque *= num2;
    }
    if ((double) num1 == 0.0)
    {
      if ((double) this.max_speed_saved == 0.0)
        return;
      this.max_speed_corr = this.max_speed_saved;
      this.max_speed_saved = 0.0f;
    }
    else
    {
      if ((double) this.max_speed_saved == 0.0)
        this.max_speed_saved = this.max_speed_corr;
      this.max_speed_corr *= num1;
    }
  }

  public void ScaleStickControls(float scaler) => this.stick_multiplier = scaler;

  public void TurnOffRenderers(bool translucent)
  {
    if (this.saved_renderers.Count <= 0)
    {
      foreach (Renderer componentsInChild in this.transform.GetComponentsInChildren<Renderer>())
      {
        if (componentsInChild.enabled)
        {
          this.saved_renderers.Add(componentsInChild);
          this.saved_materials.Add(componentsInChild.material);
        }
      }
      this.material_translucent = (Material) UnityEngine.Resources.Load("Gray_Translucent", typeof (Material));
    }
    foreach (Renderer savedRenderer in this.saved_renderers)
    {
      if ((bool) (UnityEngine.Object) savedRenderer && !((UnityEngine.Object) savedRenderer.material == (UnityEngine.Object) this.material_translucent))
      {
        if (!translucent)
        {
          savedRenderer.enabled = false;
        }
        else
        {
          savedRenderer.enabled = true;
          if (!(savedRenderer.name == "Protractor") && !(savedRenderer.name == "Indicator"))
            savedRenderer.material = this.material_translucent;
        }
      }
    }
    this.invisible = true;
  }

  public void TurnOnRenderers()
  {
    if (this.saved_renderers.Count <= 0)
      return;
    for (int index = 0; index < this.saved_renderers.Count; ++index)
    {
      if ((bool) (UnityEngine.Object) this.saved_renderers[index])
      {
        this.saved_renderers[index].enabled = true;
        if (!((UnityEngine.Object) this.saved_renderers[index].material == (UnityEngine.Object) this.saved_materials[index]))
        {
          UnityEngine.Object.Destroy((UnityEngine.Object) this.saved_renderers[index].material);
          this.saved_renderers[index].material = this.saved_materials[index];
        }
      }
    }
    this.invisible = false;
  }

  public virtual void UpdateMovement()
  {
    if (!(bool) (UnityEngine.Object) this.rb_body || !this.rb_body.gameObject.activeSelf)
      return;
    double num1 = 0.0;
    if ((double) Math.Abs(this.gamepad1_left_stick_y) < 1.0 / 1000.0)
      this.gamepad1_left_stick_y = 0.0f;
    if ((double) Math.Abs(this.gamepad1_left_stick_x) < 1.0 / 1000.0)
      this.gamepad1_left_stick_x = 0.0f;
    if ((double) Math.Abs(this.gamepad1_right_stick_y) < 1.0 / 1000.0)
      this.gamepad1_right_stick_y = 0.0f;
    if ((double) Math.Abs(this.gamepad1_right_stick_x) < 1.0 / 1000.0)
      this.gamepad1_right_stick_x = 0.0f;
    if (this.isSpectator || this.isKinematic)
      return;
    float x1 = this.stick_multiplier * this.gamepad1_left_stick_y;
    float x2 = this.stick_multiplier * this.gamepad1_left_stick_x;
    float num2 = this.stick_multiplier * this.gamepad1_right_stick_y;
    double stickMultiplier = (double) this.stick_multiplier;
    double gamepad1RightStickX = (double) this.gamepad1_right_stick_x;
    float num3 = this.gamepad1_right_stick_x;
    if ((double) num3 > 0.200000002980232)
      num3 *= 1f;
    if (this.tankcontrol)
      num3 = (float) (((double) num2 - (double) x1) / 2.0);
    double num4;
    if (!this.tankcontrol)
    {
      if ((double) x2 != 0.0 || (double) x1 != 0.0)
        num1 = Math.Atan2(-1.0 * (double) x2, -1.0 * (double) x1);
      num4 = Math.Sqrt(Math.Pow((double) x1, 2.0) + Math.Pow((double) x2, 2.0));
    }
    else
    {
      double num5 = ((double) x1 + (double) num2) / 2.0;
      if (num5 != 0.0)
        num1 = Math.Atan2(0.0, -1.0 * num5);
      num4 = Math.Abs(num5);
    }
    if (this.fieldcentric && this.DriveTrain != "Tank" && this.DriveTrain != "6-Wheel Tank")
    {
      Quaternion quaternion = this.rb_body.transform.rotation * Quaternion.Inverse(this.fieldcentric_rotation);
      num1 += (double) quaternion.eulerAngles.y * Math.PI / 180.0 + Math.PI / 2.0;
    }
    if (this.DriveTrain == "Tank" || this.DriveTrain == "6-Wheel Tank")
    {
      num4 = Math.Abs(Math.Cos(num1) * num4);
      num1 = num1 > this.Deg2Rad * 90.0 || num1 < -1.0 * this.Deg2Rad * 90.0 ? Math.PI : 0.0;
    }
    if (num4 > 1.0)
      num4 = 1.0;
    Vector3 vector3_1 = new Vector3((float) (num4 * Math.Cos(num1)), 0.0f, (float) ((double) GLOBALS.StrafeSpeedScale * num4 * Math.Sin(num1)));
    float num6 = 1f;
    if ((double) Math.Abs(this.turning_overide) > 0.00999999977648258)
      num3 = this.turning_overide;
    if ((double) Math.Abs(num3) < 0.01)
      num6 = 0.0f;
    Vector3 vector3_2 = new Vector3(0.0f, 0.0f, 0.0f);
    Vector3 vector3_3 = new Vector3(0.0f, 0.0f, 0.0f);
    Vector3 vector3_4 = new Vector3(0.0f, 0.0f, 0.0f);
    Vector3 vector3_5 = new Vector3(0.0f, 0.0f, 0.0f);
    if (this.DriveTrain == "Tank" || this.DriveTrain == "6-Wheel Tank")
    {
      vector3_4 = new Vector3(-1f * num3 * this.turn_scale, 0.0f, 0.0f);
      vector3_5 = new Vector3(num3 * this.turn_scale, 0.0f, 0.0f);
    }
    else
    {
      float num7 = num6 * 1f;
      vector3_2 = new Vector3(num7 * num3 * this.turn_scale, 0.0f, -1f * num7 * num3 * this.turn_scale);
      vector3_3 = new Vector3(-1f * num7 * num3 * this.turn_scale, 0.0f, -1f * num7 * num3 * this.turn_scale);
      vector3_4 = new Vector3(-1f * num7 * num3 * this.turn_scale, 0.0f, num7 * num3 * this.turn_scale);
      vector3_5 = new Vector3(num7 * num3 * this.turn_scale, 0.0f, num7 * num3 * this.turn_scale);
    }
    double num8 = 1.0 / (double) Math.Max(Math.Max(Math.Max(Math.Max((vector3_1 + vector3_2).magnitude, (vector3_1 + vector3_3).magnitude), (vector3_1 + vector3_4).magnitude), (vector3_1 + vector3_5).magnitude), 1f);
    Vector3 vector3_6 = (float) num8 * (vector3_1 + vector3_2);
    Vector3 vector3_7 = (float) num8 * (vector3_1 + vector3_5);
    Vector3 vector3_8 = (float) num8 * (vector3_1 + vector3_3);
    Vector3 vector3_9 = (float) num8 * (vector3_1 + vector3_4);
    this.forceTL = vector3_6;
    this.forceBL = vector3_7;
    this.forceTR = vector3_8;
    this.forceBR = vector3_9;
    Vector3 vector3_10 = new Vector3(1.1E-20f, 1.2E-20f, 1.3E-20f);
    this.ApplyWheelSpringForces(this.forceTL * this.max_speed_corr + vector3_10, this.forceTR * this.max_speed_corr + vector3_10, this.forceBL * this.max_speed_corr + vector3_10, this.forceBR * this.max_speed_corr + vector3_10);
  }

  private void FixedUpdate()
  {
    this.MoveAllHinges();
    this.MoveAllSlides();
    this.UpdateMovement();
    this.RobotFixedUpdate();
  }

  public virtual void RobotFixedUpdate()
  {
  }

  private void Update()
  {
    if (this.doinit)
    {
      this.myRobotID = this.gameObject.GetComponent<RobotID>();
      this.Init_Robot();
      this.doinit = false;
    }
    this.ClearHingeList();
    this.ClearSlideList();
    this.InputsChanges();
    this.Update_Robot();
    if (!this.DEBUG)
      return;
    if ((double) this.old_rot_intertia_scaler != (double) this.rot_inertia_scaler)
    {
      this.old_rot_intertia_scaler = this.rot_inertia_scaler;
      this.SetTotalWeight(this.total_weight, this.rot_inertia_scaler);
    }
    if (Input.GetKey(KeyCode.LeftBracket))
      this.logging = true;
    if (Input.GetKey(KeyCode.RightBracket))
      this.logging = false;
    if (!this.logging || (double) Time.time - (double) this.logtime <= 0.0500000007450581)
      return;
    this.logtime = Time.time;
    this.point1 = this.point2;
    this.point2 = this.point3;
    this.point3 = this.wheelTL_body.transform.position;
    this.time1 = this.time2;
    this.time2 = this.time3;
    this.time3 = Time.fixedTime;
    if ((double) this.time3 - (double) this.timemaxvel > 5.0)
      this.maxvel = 0.0f;
    if ((double) this.time3 - (double) this.timemaxacc > 5.0)
    {
      this.maxacc = 0.0f;
      this.accreset = true;
    }
    this.velocity1 = this.velocity2;
    this.velocity2 = (this.point3 - this.point2).magnitude / (this.time3 - this.time2);
    this.accel = (float) (((double) this.velocity2 - (double) this.velocity1) / ((double) this.time3 - (double) this.time2));
    if ((double) this.maxvel < (double) this.velocity2)
    {
      this.maxvel = this.velocity2;
      this.timemaxvel = this.time3;
    }
    if (this.countdown > 0)
    {
      this.lastacc[this.countdown - 1] = this.accel;
      --this.countdown;
    }
    if ((double) this.maxacc < (double) this.accel)
    {
      if (!this.accreset)
      {
        this.maxacc = this.accel;
        this.lastacc[4] = this.accel;
        this.countdown = 4;
      }
      this.timemaxacc = this.time3;
      this.accreset = false;
    }
    this.WriteLog("\n" + (object) this.time3 + "," + (object) this.point3.x + "," + (object) this.point3.z + "," + (object) this.rb_body.transform.rotation.eulerAngles.y);
  }

  public virtual void Init_Robot()
  {
  }

  public virtual void Update_Robot()
  {
  }

  public virtual void SetName(string name)
  {
    Transform transform1 = this.transform.Find("Body/Nametag");
    if ((bool) (UnityEngine.Object) transform1 && (bool) (UnityEngine.Object) transform1.GetComponent<TextMeshPro>())
      transform1.GetComponent<TextMesh>().text = name;
    bool flag = false;
    List<Transform> all = this.FindAll(this.transform.Find("Body"), "NametagB");
    if (all != null)
    {
      foreach (Transform transform2 in all)
      {
        if (transform2.gameObject.activeSelf && (UnityEngine.Object) transform2.GetComponent<TextMeshPro>() != (UnityEngine.Object) null)
        {
          transform2.GetComponent<TextMeshPro>().text = name;
          flag = true;
        }
      }
    }
    Transform transform3 = this.transform.Find("Body/Nametag");
    if (!flag)
    {
      if (!(bool) (UnityEngine.Object) transform3.GetComponent<TextMeshPro>())
        return;
      transform3.GetComponent<TextMesh>().text = name;
      transform3.gameObject.SetActive(true);
    }
    else
      transform3.gameObject.SetActive(false);
  }

  private List<Transform> FindAll(Transform parent, string startingName)
  {
    if ((UnityEngine.Object) parent == (UnityEngine.Object) null)
      return (List<Transform>) null;
    List<Transform> transformList = new List<Transform>();
    for (int index = 0; index < parent.childCount; ++index)
    {
      if (parent.GetChild(index).name.StartsWith(startingName))
        transformList.Add(parent.GetChild(index));
    }
    return transformList.Count <= 0 ? (List<Transform>) null : transformList;
  }

  public float MoveTowards(float start, float stop, float current, float delta)
  {
    float num = current;
    if ((double) start < (double) stop)
    {
      if ((double) current < (double) stop)
        num = current + delta;
      return (double) num > (double) stop ? stop : num;
    }
    if ((double) current > (double) stop)
      num = current - delta;
    return (double) num < (double) stop ? stop : num;
  }

  private void AddHingeToList(HingeJoint hinge, float target, float speed)
  {
    if (this.hinge_index == this.hinge_list.Count)
      this.hinge_list.Add(new RobotInterface3D.HingeData()
      {
        enable = false,
        myhinge = (HingeJoint) null,
        mytarget = 0.0f,
        myspeed = 0.0f
      });
    this.hinge_list[this.hinge_index] = this.hinge_list[this.hinge_index] with
    {
      enable = true,
      myhinge = hinge,
      mytarget = target,
      myspeed = speed
    };
    ++this.hinge_index;
  }

  private void ClearHingeList() => this.hinge_index = 0;

  public float MoveHinge(HingeJoint hinge, float target, float speed)
  {
    if (!(bool) (UnityEngine.Object) hinge)
      return 0.0f;
    double max = (double) hinge.limits.max;
    JointLimits limits = hinge.limits;
    double min = (double) limits.min;
    if (max != min)
    {
      limits = hinge.limits;
      if ((double) limits.max < (double) target)
      {
        limits = hinge.limits;
        target = limits.max;
      }
      limits = hinge.limits;
      if ((double) limits.min > (double) target)
      {
        limits = hinge.limits;
        target = limits.min;
      }
    }
    if ((double) hinge.spring.targetPosition == (double) target)
      return target;
    this.AddHingeToList(hinge, target, speed);
    return hinge.spring.targetPosition;
  }

  private void MoveAllHinges()
  {
    for (int index = 0; index < this.hinge_index; ++index)
    {
      RobotInterface3D.HingeData hinge = this.hinge_list[index];
      float targetPosition = hinge.myhinge.spring.targetPosition;
      float num = this.MoveTowards(targetPosition, hinge.mytarget, targetPosition, Time.fixedDeltaTime * hinge.myspeed);
      JointSpring spring = hinge.myhinge.spring with
      {
        targetPosition = num
      };
      hinge.myhinge.spring = spring;
    }
  }

  public float MoveSlide(
    ConfigurableJoint joint,
    RobotInterface3D.Axis axis,
    float target,
    float speed)
  {
    if (!(bool) (UnityEngine.Object) joint)
      return 0.0f;
    float num = 0.0f;
    switch (axis)
    {
      case RobotInterface3D.Axis.x:
        num = joint.targetPosition.x;
        break;
      case RobotInterface3D.Axis.y:
        num = joint.targetPosition.y;
        break;
      case RobotInterface3D.Axis.z:
        num = joint.targetPosition.z;
        break;
    }
    if ((double) num == (double) target)
      return target;
    this.AddSlideToList(joint, axis, target, speed);
    return num;
  }

  private void AddSlideToList(
    ConfigurableJoint joint,
    RobotInterface3D.Axis axis,
    float target,
    float speed)
  {
    if (this.slide_index == this.slide_list.Count)
      this.slide_list.Add(new RobotInterface3D.SlideData()
      {
        enable = false,
        myjoint = (ConfigurableJoint) null,
        myaxis = RobotInterface3D.Axis.x,
        mytarget = 0.0f,
        myspeed = 0.0f
      });
    this.slide_list[this.slide_index] = this.slide_list[this.slide_index] with
    {
      enable = true,
      myjoint = joint,
      mytarget = target,
      myspeed = speed
    };
    ++this.slide_index;
  }

  private void MoveAllSlides()
  {
    for (int index = 0; index < this.slide_index; ++index)
    {
      RobotInterface3D.SlideData slide = this.slide_list[index];
      float num1;
      switch (slide.myaxis)
      {
        case RobotInterface3D.Axis.x:
          num1 = slide.myjoint.targetPosition.x;
          break;
        case RobotInterface3D.Axis.y:
          num1 = slide.myjoint.targetPosition.y;
          break;
        default:
          num1 = slide.myjoint.targetPosition.z;
          break;
      }
      float num2 = this.MoveTowards(num1, slide.mytarget, num1, Time.fixedDeltaTime * slide.myspeed);
      Vector3 targetPosition = slide.myjoint.targetPosition;
      switch (slide.myaxis)
      {
        case RobotInterface3D.Axis.x:
          targetPosition.x = num2;
          break;
        case RobotInterface3D.Axis.y:
          targetPosition.y = num2;
          break;
        default:
          targetPosition.z = num2;
          break;
      }
      slide.myjoint.targetPosition = targetPosition;
    }
  }

  private void ClearSlideList() => this.slide_index = 0;

  private void SetTotalWeight(float weight, float inertia_scale = 1f)
  {
    float num1 = 0.0f;
    Rigidbody[] componentsInChildren = this.gameObject.GetComponentsInChildren<Rigidbody>();
    foreach (Rigidbody rigidbody in componentsInChildren)
      num1 += rigidbody.mass;
    float num2 = weight / num1;
    foreach (Rigidbody rigidbody in componentsInChildren)
    {
      rigidbody.mass *= num2;
      if (!this.rot_inertia_scale_only_body || rigidbody.name == "Body")
      {
        rigidbody.ResetInertiaTensor();
        Vector3 inertiaTensor = rigidbody.inertiaTensor;
        inertiaTensor.Scale(new Vector3(inertia_scale, inertia_scale, inertia_scale));
        rigidbody.inertiaTensor = inertiaTensor;
      }
    }
  }

  public void MarkForReset(float duration = 0.0f) => this.reset_duration = duration;

  public bool GetNeedsReset() => (double) this.reset_duration >= 0.0;

  public float GetResetDuration() => (double) this.reset_duration < 0.0 ? 0.0f : this.reset_duration;

  public void SetColorFromPosition(string position)
  {
    if (position.StartsWith("Spectator"))
      return;
    int color;
    if (position.StartsWith("Red"))
    {
      color = 1;
      if ((bool) (UnityEngine.Object) this.myRobotID)
        this.myRobotID.is_holding = false;
    }
    else if (position.StartsWith("Blue"))
    {
      color = 2;
      if ((bool) (UnityEngine.Object) this.myRobotID)
        this.myRobotID.is_holding = false;
    }
    else
    {
      color = 0;
      if ((bool) (UnityEngine.Object) this.myRobotID)
        this.myRobotID.is_holding = true;
    }
    this.SetRobotColor(color);
  }

  public void SetRobotColor(int color, bool force = false)
  {
    if (!force && this.robot_color == color)
      return;
    this.robot_color = color;
    string key1;
    string key2;
    switch (color)
    {
      case 0:
        key1 = "Robot_holding";
        key2 = "BumperHold";
        if ((bool) (UnityEngine.Object) this.progressmeter)
        {
          this.progressmeter.SetColor(new Color(0.1f, 0.1f, 0.1f));
          break;
        }
        break;
      case 1:
        key1 = "Robot_red";
        key2 = "BumperRed";
        if ((bool) (UnityEngine.Object) this.progressmeter)
        {
          this.progressmeter.SetColor(new Color(1f, 0.0f, 0.0f));
          break;
        }
        break;
      case 2:
        key1 = "Robot_blue";
        key2 = "BumperBlue";
        if ((bool) (UnityEngine.Object) this.progressmeter)
        {
          this.progressmeter.SetColor(new Color(0.0f, 0.0f, 1f));
          break;
        }
        break;
      case 3:
        key1 = "Robot_other";
        key2 = "BumperOther";
        if ((bool) (UnityEngine.Object) this.progressmeter)
        {
          this.progressmeter.SetColor(new Color(0.7f, 0.35f, 0.7f));
          break;
        }
        break;
      default:
        key1 = "Robot_holding";
        key2 = "BumperHold";
        if ((bool) (UnityEngine.Object) this.progressmeter)
        {
          this.progressmeter.SetColor(new Color(0.1f, 0.1f, 0.1f));
          break;
        }
        break;
    }
    Material material1;
    if (this.material_cache.ContainsKey(key1))
    {
      material1 = this.material_cache[key1];
    }
    else
    {
      material1 = UnityEngine.Resources.Load(key1, typeof (Material)) as Material;
      this.material_cache[key1] = material1;
    }
    Material material2;
    if (this.material_cache.ContainsKey(key2))
    {
      material2 = this.material_cache[key2];
    }
    else
    {
      material2 = UnityEngine.Resources.Load(key2, typeof (Material)) as Material;
      this.material_cache[key2] = material2;
    }
    foreach (Renderer componentsInChild in this.transform.root.GetComponentsInChildren<Renderer>(true))
    {
      if (componentsInChild.material.name.StartsWith("Robot") && (UnityEngine.Object) componentsInChild.material != (UnityEngine.Object) material1)
      {
        UnityEngine.Object.Destroy((UnityEngine.Object) componentsInChild.material);
        componentsInChild.material = material1;
      }
    }
    foreach (Renderer componentsInChild in this.transform.root.GetComponentsInChildren<Renderer>(true))
    {
      for (int index = 0; index < componentsInChild.materials.Length; ++index)
      {
        if (componentsInChild.materials[index].name.StartsWith("Bumper"))
        {
          Material[] materials = componentsInChild.materials;
          materials[index] = material2;
          componentsInChild.materials = materials;
        }
      }
    }
    UnityEngine.Resources.UnloadUnusedAssets();
  }

  public void SetProgressBar(float value)
  {
    if (!(bool) (UnityEngine.Object) this.rb_body || this.isSpectator)
      return;
    this.progress_value = value;
    this.progressmeter.SetProgress(value);
  }

  public float GetProgressBar() => this.progress_value;

  public void HoldRobot(bool state = true)
  {
    if (!(bool) (UnityEngine.Object) this.rb_body || this.isSpectator)
      return;
    if (!this.hold_position & state)
      this.SetColorFromPosition("Holding");
    else if (!state && this.hold_position)
      this.SetColorFromPosition(this.myRobotID.starting_pos);
    this.hold_position = state;
  }

  public void OverrideColor(int color)
  {
    if (color < 0)
      this.SetColorFromPosition(this.myRobotID.starting_pos);
    else
      this.SetRobotColor(color);
  }

  public void DisableTopObjects()
  {
    this.disabledTopObjects = true;
    for (int index = 0; index < this.transform.childCount; ++index)
      this.transform.GetChild(index).gameObject.SetActive(false);
  }

  public virtual void EnableTopObjects()
  {
    this.disabledTopObjects = false;
    for (int index = 0; index < this.transform.childCount; ++index)
      this.transform.GetChild(index).gameObject.SetActive(true);
  }

  public void OnTriggerEnter(Collider collision)
  {
    if (!(bool) (UnityEngine.Object) this.rb_body || !this.rb_body.gameObject.activeSelf || this.isSpectator)
      return;
    this.RemoveInvalidItems();
    this.AddEnemy(collision);
  }

  public void OnTriggerExit(Collider collision)
  {
    if (!(bool) (UnityEngine.Object) this.rb_body || this.isSpectator)
      return;
    this.RemoveInvalidItems();
    this.RemoveEnemy(collision);
  }

  private bool IsEnemy(Collider collision)
  {
    RobotID component = collision.transform.root.GetComponent<RobotID>();
    return !((UnityEngine.Object) component == (UnityEngine.Object) null) && (bool) (UnityEngine.Object) this.myRobotID && !component.is_holding && !this.myRobotID.is_holding && (!component.is_red || !this.myRobotID.is_red) && (component.is_red || this.myRobotID.is_red);
  }

  private bool AddEnemy(Collider collision)
  {
    Transform root = collision.transform.root;
    RobotID component1 = root.GetComponent<RobotID>();
    if ((UnityEngine.Object) component1 == (UnityEngine.Object) null || !(bool) (UnityEngine.Object) this.myRobotID || component1.is_holding || this.myRobotID.is_holding || component1.is_red && this.myRobotID.is_red || !component1.is_red && !this.myRobotID.is_red)
      return false;
    RobotInterface3D component2 = root.GetComponent<RobotInterface3D>();
    int index = this.enemies.IndexOf(component2);
    if (index >= 0)
    {
      ++this.enemies_collisions[index];
    }
    else
    {
      this.enemies.Add(component2);
      this.enemies_collisions.Add(1);
    }
    return true;
  }

  private bool RemoveEnemy(Collider collision)
  {
    Transform root = collision.transform.root;
    RobotID component = root.GetComponent<RobotID>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null || !(bool) (UnityEngine.Object) this.myRobotID || component.is_holding || this.myRobotID.is_holding || component.is_red && this.myRobotID.is_red || !component.is_red && !this.myRobotID.is_red)
      return false;
    int index = this.enemies.IndexOf(root.GetComponent<RobotInterface3D>());
    if (index >= 0)
    {
      --this.enemies_collisions[index];
      if (this.enemies_collisions[index] <= 0)
      {
        this.enemies.RemoveAt(index);
        this.enemies_collisions.RemoveAt(index);
      }
    }
    return true;
  }

  private void RemoveInvalidItems()
  {
    for (int index = this.enemies.Count - 1; index >= 0; --index)
    {
      if ((UnityEngine.Object) this.enemies[index] == (UnityEngine.Object) null)
      {
        this.enemies.RemoveAt(index);
        this.enemies_collisions.RemoveAt(index);
      }
    }
  }

  public bool GetEnemiesColliding()
  {
    this.RemoveInvalidItems();
    return this.enemies.Count > 0;
  }

  public List<RobotInterface3D> GetAllEnemies()
  {
    this.RemoveInvalidItems();
    return this.enemies;
  }

  public virtual string GetStates()
  {
    string str = this.robot_color.ToString() + ":" + this.progress_value.ToString("F2") + ":" + (this.disabledTopObjects ? "1" : "0") + ":";
    if ((bool) (UnityEngine.Object) this.robotskin)
      str += this.robotskin.GetState();
    return str + ":" + (this.invisible ? "1" : "0") + ":" + this.getAverageMovementForceMagnitude().ToString("F2") + ":" + this.getAveragVelocityMagnitude().ToString("F2") + ":";
  }

  public virtual void SetStates(string instring)
  {
    string[] strArray = instring.Split(':');
    if (strArray.Length != 0 && strArray[0].Length > 0)
      this.SetRobotColor(int.Parse(strArray[0]));
    if (strArray.Length > 1)
      this.SetProgressBar(float.Parse(strArray[1]));
    if (strArray.Length > 2)
    {
      this.disabledTopObjects = strArray[2][0] == '1';
      if (this.disabledTopObjects_old != this.disabledTopObjects)
      {
        if (this.disabledTopObjects)
          this.DisableTopObjects();
        else
          this.EnableTopObjects();
      }
      this.disabledTopObjects_old = this.disabledTopObjects;
    }
    if (strArray.Length > 3 && (bool) (UnityEngine.Object) this.robotskin)
      this.robotskin.SetState(strArray[3]);
    if (strArray.Length > 4 && (UnityEngine.Object) this.myRobotID != (UnityEngine.Object) null)
    {
      bool flag = strArray[4][0] == '1';
      if (flag != this.invisible)
      {
        if (flag)
          this.TurnOffRenderers(GLOBALS.I_AM_RED == this.myRobotID.is_red || GLOBALS.I_AM_SPECTATOR);
        else
          this.TurnOnRenderers();
      }
    }
    if (strArray.Length > 5 && strArray[5].Length > 0)
      this.averageMovementForceMagnitude = float.Parse(strArray[5]);
    if (strArray.Length <= 6 || strArray[6].Length <= 0)
      return;
    this.averageVelocityMagnitude = float.Parse(strArray[6]);
  }

  public void SetKinematic(bool turnon = true)
  {
    if (turnon)
      this.TurnOffPhysics();
    else
      this.TurnOnPhysics();
  }

  public void Highlite(bool state)
  {
    if (!(bool) (UnityEngine.Object) this.highlitecircle)
      return;
    this.highlitecircle.SetActive(state);
  }

  public void Select(bool state)
  {
    if (!(bool) (UnityEngine.Object) this.selectcircle)
      return;
    this.selectcircle.SetActive(state);
  }

  private void TurnOffPhysics()
  {
    this.isKinematic = true;
    if (this.kinematic_states.Count < 1)
      this.RememberPhysics();
    foreach (Rigidbody componentsInChild in this.gameObject.GetComponentsInChildren<Rigidbody>(true))
    {
      componentsInChild.detectCollisions = false;
      componentsInChild.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
      componentsInChild.isKinematic = true;
    }
  }

  private void TurnOnPhysics()
  {
    if (GLOBALS.CLIENT_MODE || this.kinematic_states.Count < 1)
      return;
    foreach (Rigidbody componentsInChild in this.gameObject.GetComponentsInChildren<Rigidbody>(true))
    {
      if (this.kinematic_states.ContainsKey(componentsInChild.GetInstanceID()))
      {
        componentsInChild.isKinematic = this.kinematic_states[componentsInChild.GetInstanceID()];
        componentsInChild.collisionDetectionMode = this.collision_states[componentsInChild.GetInstanceID()];
      }
      else
        componentsInChild.isKinematic = false;
      componentsInChild.detectCollisions = true;
    }
    this.isKinematic = false;
  }

  private void RememberPhysics()
  {
    this.kinematic_states.Clear();
    this.collision_states.Clear();
    foreach (Rigidbody componentsInChild in this.gameObject.GetComponentsInChildren<Rigidbody>(true))
    {
      this.kinematic_states[componentsInChild.GetInstanceID()] = componentsInChild.isKinematic;
      this.collision_states[componentsInChild.GetInstanceID()] = componentsInChild.collisionDetectionMode;
    }
  }

  private struct ObjPos
  {
    public Vector3 pos;
    public Quaternion rot;
  }

  public struct HingeData
  {
    public bool enable;
    public HingeJoint myhinge;
    public float mytarget;
    public float myspeed;
  }

  public enum Axis
  {
    x,
    y,
    z,
  }

  public struct SlideData
  {
    public bool enable;
    public ConfigurableJoint myjoint;
    public RobotInterface3D.Axis myaxis;
    public float mytarget;
    public float myspeed;
  }
}
