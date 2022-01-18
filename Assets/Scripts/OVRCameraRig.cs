// Decompiled with JetBrains decompiler
// Type: OVRCameraRig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.XR;

[ExecuteInEditMode]
public class OVRCameraRig : MonoBehaviour
{
  public bool usePerEyeCameras;
  public bool useFixedUpdateForTracking;
  protected bool _skipUpdate;
  protected readonly string trackingSpaceName = "TrackingSpace";
  protected readonly string trackerAnchorName = "TrackerAnchor";
  protected readonly string leftEyeAnchorName = "LeftEyeAnchor";
  protected readonly string centerEyeAnchorName = "CenterEyeAnchor";
  protected readonly string rightEyeAnchorName = "RightEyeAnchor";
  protected readonly string leftHandAnchorName = "LeftHandAnchor";
  protected readonly string rightHandAnchorName = "RightHandAnchor";
  protected Camera _centerEyeCamera;
  protected Camera _leftEyeCamera;
  protected Camera _rightEyeCamera;

  public Camera leftEyeCamera => !this.usePerEyeCameras ? this._centerEyeCamera : this._leftEyeCamera;

  public Camera rightEyeCamera => !this.usePerEyeCameras ? this._centerEyeCamera : this._rightEyeCamera;

  public Transform trackingSpace { get; private set; }

  public Transform leftEyeAnchor { get; private set; }

  public Transform centerEyeAnchor { get; private set; }

  public Transform rightEyeAnchor { get; private set; }

  public Transform leftHandAnchor { get; private set; }

  public Transform rightHandAnchor { get; private set; }

  public Transform trackerAnchor { get; private set; }

  public event Action<OVRCameraRig> UpdatedAnchors;

  protected virtual void Awake()
  {
    if (Application.isBatchMode)
      return;
    this._skipUpdate = true;
    this.EnsureGameObjectIntegrity();
  }

  protected virtual void Start()
  {
    if (Application.isBatchMode)
      return;
    this.UpdateAnchors();
  }

  protected virtual void FixedUpdate()
  {
    if (Application.isBatchMode || !this.useFixedUpdateForTracking)
      return;
    this.UpdateAnchors();
  }

  protected virtual void Update()
  {
    if (Application.isBatchMode)
      return;
    this._skipUpdate = false;
    if (this.useFixedUpdateForTracking)
      return;
    this.UpdateAnchors();
  }

  protected virtual void UpdateAnchors()
  {
    if (Application.isBatchMode)
      return;
    this.EnsureGameObjectIntegrity();
    if (!Application.isPlaying)
      return;
    if (this._skipUpdate)
    {
      this.centerEyeAnchor.FromOVRPose(OVRPose.identity, true);
      this.leftEyeAnchor.FromOVRPose(OVRPose.identity, true);
      this.rightEyeAnchor.FromOVRPose(OVRPose.identity, true);
    }
    else
    {
      bool monoscopic = OVRManager.instance.monoscopic;
      OVRPose pose = OVRManager.tracker.GetPose();
      this.trackerAnchor.localRotation = pose.orientation;
      this.centerEyeAnchor.localRotation = InputTracking.GetLocalRotation(XRNode.CenterEye);
      this.leftEyeAnchor.localRotation = monoscopic ? this.centerEyeAnchor.localRotation : InputTracking.GetLocalRotation(XRNode.LeftEye);
      this.rightEyeAnchor.localRotation = monoscopic ? this.centerEyeAnchor.localRotation : InputTracking.GetLocalRotation(XRNode.RightEye);
      this.leftHandAnchor.localRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch);
      this.rightHandAnchor.localRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);
      this.trackerAnchor.localPosition = pose.position;
      this.centerEyeAnchor.localPosition = InputTracking.GetLocalPosition(XRNode.CenterEye);
      this.leftEyeAnchor.localPosition = monoscopic ? this.centerEyeAnchor.localPosition : InputTracking.GetLocalPosition(XRNode.LeftEye);
      this.rightEyeAnchor.localPosition = monoscopic ? this.centerEyeAnchor.localPosition : InputTracking.GetLocalPosition(XRNode.RightEye);
      this.leftHandAnchor.localPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
      this.rightHandAnchor.localPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
      this.RaiseUpdatedAnchorsEvent();
    }
  }

  protected virtual void RaiseUpdatedAnchorsEvent()
  {
    if (this.UpdatedAnchors == null)
      return;
    this.UpdatedAnchors(this);
  }

  public virtual void EnsureGameObjectIntegrity()
  {
    if ((UnityEngine.Object) this.trackingSpace == (UnityEngine.Object) null)
      this.trackingSpace = this.ConfigureAnchor((Transform) null, this.trackingSpaceName);
    if ((UnityEngine.Object) this.leftEyeAnchor == (UnityEngine.Object) null)
      this.leftEyeAnchor = this.ConfigureAnchor(this.trackingSpace, this.leftEyeAnchorName);
    if ((UnityEngine.Object) this.centerEyeAnchor == (UnityEngine.Object) null)
      this.centerEyeAnchor = this.ConfigureAnchor(this.trackingSpace, this.centerEyeAnchorName);
    if ((UnityEngine.Object) this.rightEyeAnchor == (UnityEngine.Object) null)
      this.rightEyeAnchor = this.ConfigureAnchor(this.trackingSpace, this.rightEyeAnchorName);
    if ((UnityEngine.Object) this.leftHandAnchor == (UnityEngine.Object) null)
      this.leftHandAnchor = this.ConfigureAnchor(this.trackingSpace, this.leftHandAnchorName);
    if ((UnityEngine.Object) this.rightHandAnchor == (UnityEngine.Object) null)
      this.rightHandAnchor = this.ConfigureAnchor(this.trackingSpace, this.rightHandAnchorName);
    if ((UnityEngine.Object) this.trackerAnchor == (UnityEngine.Object) null)
      this.trackerAnchor = this.ConfigureAnchor(this.trackingSpace, this.trackerAnchorName);
    if ((UnityEngine.Object) this._centerEyeCamera == (UnityEngine.Object) null || (UnityEngine.Object) this._leftEyeCamera == (UnityEngine.Object) null || (UnityEngine.Object) this._rightEyeCamera == (UnityEngine.Object) null)
    {
      this._centerEyeCamera = this.centerEyeAnchor.GetComponent<Camera>();
      this._leftEyeCamera = this.leftEyeAnchor.GetComponent<Camera>();
      this._rightEyeCamera = this.rightEyeAnchor.GetComponent<Camera>();
      if ((UnityEngine.Object) this._centerEyeCamera == (UnityEngine.Object) null)
      {
        this._centerEyeCamera = this.centerEyeAnchor.gameObject.AddComponent<Camera>();
        this._centerEyeCamera.tag = "MainCamera";
      }
      if ((UnityEngine.Object) this._leftEyeCamera == (UnityEngine.Object) null)
      {
        this._leftEyeCamera = this.leftEyeAnchor.gameObject.AddComponent<Camera>();
        this._leftEyeCamera.tag = "MainCamera";
      }
      if ((UnityEngine.Object) this._rightEyeCamera == (UnityEngine.Object) null)
      {
        this._rightEyeCamera = this.rightEyeAnchor.gameObject.AddComponent<Camera>();
        this._rightEyeCamera.tag = "MainCamera";
      }
      this._centerEyeCamera.stereoTargetEye = StereoTargetEyeMask.Both;
      this._leftEyeCamera.stereoTargetEye = StereoTargetEyeMask.Left;
      this._rightEyeCamera.stereoTargetEye = StereoTargetEyeMask.Right;
    }
    if (this._centerEyeCamera.enabled == this.usePerEyeCameras || this._leftEyeCamera.enabled == !this.usePerEyeCameras || this._rightEyeCamera.enabled == !this.usePerEyeCameras)
      this._skipUpdate = true;
    this._centerEyeCamera.enabled = !this.usePerEyeCameras;
    this._leftEyeCamera.enabled = this.usePerEyeCameras;
    this._rightEyeCamera.enabled = this.usePerEyeCameras;
  }

  protected virtual Transform ConfigureAnchor(Transform root, string name)
  {
    Transform transform = (UnityEngine.Object) root != (UnityEngine.Object) null ? this.transform.Find(root.name + "/" + name) : (Transform) null;
    if ((UnityEngine.Object) transform == (UnityEngine.Object) null)
      transform = this.transform.Find(name);
    if ((UnityEngine.Object) transform == (UnityEngine.Object) null)
      transform = new GameObject(name).transform;
    transform.name = name;
    transform.parent = (UnityEngine.Object) root != (UnityEngine.Object) null ? root : this.transform;
    transform.localScale = Vector3.one;
    transform.localPosition = Vector3.zero;
    transform.localRotation = Quaternion.identity;
    return transform;
  }

  public virtual Matrix4x4 ComputeTrackReferenceMatrix()
  {
    if ((UnityEngine.Object) this.centerEyeAnchor == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "centerEyeAnchor is required");
      return Matrix4x4.identity;
    }
    OVRPose ovrPose1;
    ovrPose1.position = InputTracking.GetLocalPosition(XRNode.Head);
    ovrPose1.orientation = InputTracking.GetLocalRotation(XRNode.Head);
    OVRPose ovrPose2 = ovrPose1.Inverse();
    return this.centerEyeAnchor.localToWorldMatrix * Matrix4x4.TRS(ovrPose2.position, ovrPose2.orientation, Vector3.one);
  }
}
