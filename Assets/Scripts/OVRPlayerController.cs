// Decompiled with JetBrains decompiler
// Type: OVRPlayerController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

[RequireComponent(typeof (CharacterController))]
public class OVRPlayerController : MonoBehaviour
{
  public float Acceleration = 0.1f;
  public float Damping = 0.3f;
  public float BackAndSideDampen = 0.5f;
  public float JumpForce = 0.3f;
  public float RotationAmount = 1.5f;
  public float RotationRatchet = 45f;
  [Tooltip("The player will rotate in fixed steps if Snap Rotation is enabled.")]
  public bool SnapRotation = true;
  [Tooltip("How many fixed speeds to use with linear movement? 0=linear control")]
  public int FixedSpeedSteps;
  public bool HmdResetsY = true;
  public bool HmdRotatesY = true;
  public float GravityModifier = 0.379f;
  public bool useProfileData = true;
  [NonSerialized]
  public float CameraHeight;
  [NonSerialized]
  public bool Teleported;
  public bool EnableLinearMovement = true;
  public bool EnableRotation = true;
  protected CharacterController Controller;
  protected OVRCameraRig CameraRig;
  private float MoveScale = 1f;
  private Vector3 MoveThrottle = Vector3.zero;
  private float FallSpeed;
  private OVRPose? InitialPose;
  private float MoveScaleMultiplier = 1f;
  private float RotationScaleMultiplier = 1f;
  private bool SkipMouseRotation = true;
  private bool HaltUpdateMovement;
  private bool prevHatLeft;
  private bool prevHatRight;
  private float SimulationRate = 60f;
  private float buttonRotation;
  private bool ReadyToSnapTurn;

  public event Action<Transform> TransformUpdated;

  public event Action CameraUpdated;

  public event Action PreCharacterMove;

  public float InitialYRotation { get; private set; }

  private void Start() => this.CameraRig.transform.localPosition = this.CameraRig.transform.localPosition with
  {
    z = OVRManager.profile.eyeDepth
  };

  private void Awake()
  {
    this.Controller = this.gameObject.GetComponent<CharacterController>();
    if ((UnityEngine.Object) this.Controller == (UnityEngine.Object) null)
      Debug.LogWarning((object) "OVRPlayerController: No CharacterController attached.");
    OVRCameraRig[] componentsInChildren = this.gameObject.GetComponentsInChildren<OVRCameraRig>();
    if (componentsInChildren.Length == 0)
      Debug.LogWarning((object) "OVRPlayerController: No OVRCameraRig attached.");
    else if (componentsInChildren.Length > 1)
      Debug.LogWarning((object) "OVRPlayerController: More then 1 OVRCameraRig attached.");
    else
      this.CameraRig = componentsInChildren[0];
    this.InitialYRotation = this.transform.rotation.eulerAngles.y;
  }

  private void OnEnable()
  {
    OVRManager.display.RecenteredPose += new Action(this.ResetOrientation);
    if (!((UnityEngine.Object) this.CameraRig != (UnityEngine.Object) null))
      return;
    this.CameraRig.UpdatedAnchors += new Action<OVRCameraRig>(this.UpdateTransform);
  }

  private void OnDisable()
  {
    OVRManager.display.RecenteredPose -= new Action(this.ResetOrientation);
    if (!((UnityEngine.Object) this.CameraRig != (UnityEngine.Object) null))
      return;
    this.CameraRig.UpdatedAnchors -= new Action<OVRCameraRig>(this.UpdateTransform);
  }

  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.Q))
      this.buttonRotation -= this.RotationRatchet;
    if (!Input.GetKeyDown(KeyCode.E))
      return;
    this.buttonRotation += this.RotationRatchet;
  }

  protected virtual void UpdateController()
  {
    if (this.useProfileData)
    {
      if (!this.InitialPose.HasValue)
        this.InitialPose = new OVRPose?(new OVRPose()
        {
          position = this.CameraRig.transform.localPosition,
          orientation = this.CameraRig.transform.localRotation
        });
      Vector3 localPosition = this.CameraRig.transform.localPosition;
      if (OVRManager.instance.trackingOriginType == OVRManager.TrackingOrigin.EyeLevel)
        localPosition.y = OVRManager.profile.eyeHeight - 0.5f * this.Controller.height + this.Controller.center.y;
      else if (OVRManager.instance.trackingOriginType == OVRManager.TrackingOrigin.FloorLevel)
        localPosition.y = (float) -(0.5 * (double) this.Controller.height) + this.Controller.center.y;
      this.CameraRig.transform.localPosition = localPosition;
    }
    else if (this.InitialPose.HasValue)
    {
      this.CameraRig.transform.localPosition = this.InitialPose.Value.position;
      this.CameraRig.transform.localRotation = this.InitialPose.Value.orientation;
      this.InitialPose = new OVRPose?();
    }
    this.CameraHeight = this.CameraRig.centerEyeAnchor.localPosition.y;
    if (this.CameraUpdated != null)
      this.CameraUpdated();
    this.UpdateMovement();
    Vector3 zero = Vector3.zero;
    float num1 = (float) (1.0 + (double) this.Damping * (double) this.SimulationRate * (double) Time.deltaTime);
    this.MoveThrottle.x /= num1;
    this.MoveThrottle.y = (double) this.MoveThrottle.y > 0.0 ? this.MoveThrottle.y / num1 : this.MoveThrottle.y;
    this.MoveThrottle.z /= num1;
    Vector3 motion = zero + this.MoveThrottle * this.SimulationRate * Time.deltaTime;
    if (this.Controller.isGrounded && (double) this.FallSpeed <= 0.0)
      this.FallSpeed = Physics.gravity.y * (this.GravityModifier * (1f / 500f));
    else
      this.FallSpeed += Physics.gravity.y * (this.GravityModifier * (1f / 500f)) * this.SimulationRate * Time.deltaTime;
    motion.y += this.FallSpeed * this.SimulationRate * Time.deltaTime;
    if (this.Controller.isGrounded && (double) this.MoveThrottle.y <= (double) this.transform.lossyScale.y * (1.0 / 1000.0))
    {
      float num2 = Mathf.Max(this.Controller.stepOffset, new Vector3(motion.x, 0.0f, motion.z).magnitude);
      motion -= num2 * Vector3.up;
    }
    if (this.PreCharacterMove != null)
    {
      this.PreCharacterMove();
      this.Teleported = false;
    }
    Vector3 vector3_1 = Vector3.Scale(this.Controller.transform.localPosition + motion, new Vector3(1f, 0.0f, 1f));
    int num3 = (int) this.Controller.Move(motion);
    Vector3 vector3_2 = Vector3.Scale(this.Controller.transform.localPosition, new Vector3(1f, 0.0f, 1f));
    if (!(vector3_1 != vector3_2))
      return;
    this.MoveThrottle += (vector3_2 - vector3_1) / (this.SimulationRate * Time.deltaTime);
  }

  public virtual void UpdateMovement()
  {
    if (this.HaltUpdateMovement)
      return;
    if (this.EnableLinearMovement)
    {
      bool flag1 = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
      bool flag2 = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
      bool flag3 = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
      bool flag4 = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
      bool flag5 = false;
      if (OVRInput.Get(OVRInput.Button.DpadUp))
      {
        flag1 = true;
        flag5 = true;
      }
      if (OVRInput.Get(OVRInput.Button.DpadDown))
      {
        flag4 = true;
        flag5 = true;
      }
      this.MoveScale = 1f;
      if (flag1 & flag2 || flag1 & flag3 || flag4 & flag2 || flag4 & flag3)
        this.MoveScale = 0.7071068f;
      if (!this.Controller.isGrounded)
        this.MoveScale = 0.0f;
      this.MoveScale *= this.SimulationRate * Time.deltaTime;
      float num1 = this.Acceleration * 0.1f * this.MoveScale * this.MoveScaleMultiplier;
      if (flag5 || Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        num1 *= 2f;
      Vector3 eulerAngles = this.transform.rotation.eulerAngles;
      eulerAngles.z = eulerAngles.x = 0.0f;
      Quaternion quaternion = Quaternion.Euler(eulerAngles);
      if (flag1)
        this.MoveThrottle += quaternion * (this.transform.lossyScale.z * num1 * Vector3.forward);
      if (flag4)
        this.MoveThrottle += quaternion * (this.transform.lossyScale.z * num1 * this.BackAndSideDampen * Vector3.back);
      if (flag2)
        this.MoveThrottle += quaternion * (this.transform.lossyScale.x * num1 * this.BackAndSideDampen * Vector3.left);
      if (flag3)
        this.MoveThrottle += quaternion * (this.transform.lossyScale.x * num1 * this.BackAndSideDampen * Vector3.right);
      float num2 = this.Acceleration * 0.1f * this.MoveScale * this.MoveScaleMultiplier * (1f + OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger));
      Vector2 vector2 = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
      if (this.FixedSpeedSteps > 0)
      {
        vector2.y = Mathf.Round(vector2.y * (float) this.FixedSpeedSteps) / (float) this.FixedSpeedSteps;
        vector2.x = Mathf.Round(vector2.x * (float) this.FixedSpeedSteps) / (float) this.FixedSpeedSteps;
      }
      if ((double) vector2.y > 0.0)
        this.MoveThrottle += quaternion * (vector2.y * this.transform.lossyScale.z * num2 * Vector3.forward);
      if ((double) vector2.y < 0.0)
        this.MoveThrottle += quaternion * (Mathf.Abs(vector2.y) * this.transform.lossyScale.z * num2 * this.BackAndSideDampen * Vector3.back);
      if ((double) vector2.x < 0.0)
        this.MoveThrottle += quaternion * (Mathf.Abs(vector2.x) * this.transform.lossyScale.x * num2 * this.BackAndSideDampen * Vector3.left);
      if ((double) vector2.x > 0.0)
        this.MoveThrottle += quaternion * (vector2.x * this.transform.lossyScale.x * num2 * this.BackAndSideDampen * Vector3.right);
    }
    if (!this.EnableRotation)
      return;
    Vector3 eulerAngles1 = this.transform.rotation.eulerAngles;
    float num = this.SimulationRate * Time.deltaTime * this.RotationAmount * this.RotationScaleMultiplier;
    bool flag6 = OVRInput.Get(OVRInput.Button.PrimaryShoulder);
    if (flag6 && !this.prevHatLeft)
      eulerAngles1.y -= this.RotationRatchet;
    this.prevHatLeft = flag6;
    bool flag7 = OVRInput.Get(OVRInput.Button.SecondaryShoulder);
    if (flag7 && !this.prevHatRight)
      eulerAngles1.y += this.RotationRatchet;
    this.prevHatRight = flag7;
    eulerAngles1.y += this.buttonRotation;
    this.buttonRotation = 0.0f;
    if (!this.SkipMouseRotation)
      eulerAngles1.y += (float) ((double) Input.GetAxis("Mouse X") * (double) num * 3.25);
    if (this.SnapRotation)
    {
      if (OVRInput.Get(OVRInput.Button.SecondaryThumbstickLeft))
      {
        if (this.ReadyToSnapTurn)
        {
          eulerAngles1.y -= this.RotationRatchet;
          this.ReadyToSnapTurn = false;
        }
      }
      else if (OVRInput.Get(OVRInput.Button.SecondaryThumbstickRight))
      {
        if (this.ReadyToSnapTurn)
        {
          eulerAngles1.y += this.RotationRatchet;
          this.ReadyToSnapTurn = false;
        }
      }
      else
        this.ReadyToSnapTurn = true;
    }
    else
    {
      Vector2 vector2 = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
      eulerAngles1.y += vector2.x * num;
    }
    this.transform.rotation = Quaternion.Euler(eulerAngles1);
  }

  public void UpdateTransform(OVRCameraRig rig)
  {
    Transform trackingSpace = this.CameraRig.trackingSpace;
    Transform centerEyeAnchor = this.CameraRig.centerEyeAnchor;
    if (this.HmdRotatesY && !this.Teleported)
    {
      Vector3 position = trackingSpace.position;
      Quaternion rotation = trackingSpace.rotation;
      this.transform.rotation = Quaternion.Euler(0.0f, centerEyeAnchor.rotation.eulerAngles.y, 0.0f);
      trackingSpace.position = position;
      trackingSpace.rotation = rotation;
    }
    this.UpdateController();
    if (this.TransformUpdated == null)
      return;
    this.TransformUpdated(trackingSpace);
  }

  public bool Jump()
  {
    if (!this.Controller.isGrounded)
      return false;
    this.MoveThrottle += new Vector3(0.0f, this.transform.lossyScale.y * this.JumpForce, 0.0f);
    return true;
  }

  public void Stop()
  {
    int num = (int) this.Controller.Move(Vector3.zero);
    this.MoveThrottle = Vector3.zero;
    this.FallSpeed = 0.0f;
  }

  public void GetMoveScaleMultiplier(ref float moveScaleMultiplier) => moveScaleMultiplier = this.MoveScaleMultiplier;

  public void SetMoveScaleMultiplier(float moveScaleMultiplier) => this.MoveScaleMultiplier = moveScaleMultiplier;

  public void GetRotationScaleMultiplier(ref float rotationScaleMultiplier) => rotationScaleMultiplier = this.RotationScaleMultiplier;

  public void SetRotationScaleMultiplier(float rotationScaleMultiplier) => this.RotationScaleMultiplier = rotationScaleMultiplier;

  public void GetSkipMouseRotation(ref bool skipMouseRotation) => skipMouseRotation = this.SkipMouseRotation;

  public void SetSkipMouseRotation(bool skipMouseRotation) => this.SkipMouseRotation = skipMouseRotation;

  public void GetHaltUpdateMovement(ref bool haltUpdateMovement) => haltUpdateMovement = this.HaltUpdateMovement;

  public void SetHaltUpdateMovement(bool haltUpdateMovement) => this.HaltUpdateMovement = haltUpdateMovement;

  public void ResetOrientation()
  {
    if (!this.HmdResetsY || this.HmdRotatesY)
      return;
    this.transform.rotation = Quaternion.Euler(this.transform.rotation.eulerAngles with
    {
      y = this.InitialYRotation
    });
  }
}
