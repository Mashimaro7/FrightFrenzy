// Decompiled with JetBrains decompiler
// Type: OVRGrabber
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (Rigidbody))]
public class OVRGrabber : MonoBehaviour
{
  public float grabBegin = 0.55f;
  public float grabEnd = 0.35f;
  [SerializeField]
  protected bool m_parentHeldObject;
  [SerializeField]
  protected Transform m_gripTransform;
  [SerializeField]
  protected Collider[] m_grabVolumes;
  [SerializeField]
  protected OVRInput.Controller m_controller;
  [SerializeField]
  protected Transform m_parentTransform;
  protected bool m_grabVolumeEnabled = true;
  protected Vector3 m_lastPos;
  protected Quaternion m_lastRot;
  protected Quaternion m_anchorOffsetRotation;
  protected Vector3 m_anchorOffsetPosition;
  protected float m_prevFlex;
  protected OVRGrabbable m_grabbedObj;
  protected Vector3 m_grabbedObjectPosOff;
  protected Quaternion m_grabbedObjectRotOff;
  protected Dictionary<OVRGrabbable, int> m_grabCandidates = new Dictionary<OVRGrabbable, int>();
  protected bool operatingWithoutOVRCameraRig = true;

  public OVRGrabbable grabbedObject => this.m_grabbedObj;

  public void ForceRelease(OVRGrabbable grabbable)
  {
    if ((!((UnityEngine.Object) this.m_grabbedObj != (UnityEngine.Object) null) ? 0 : ((UnityEngine.Object) this.m_grabbedObj == (UnityEngine.Object) grabbable ? 1 : 0)) == 0)
      return;
    this.GrabEnd();
  }

  protected virtual void Awake()
  {
    this.m_anchorOffsetPosition = this.transform.localPosition;
    this.m_anchorOffsetRotation = this.transform.localRotation;
    OVRCameraRig ovrCameraRig = (OVRCameraRig) null;
    if ((UnityEngine.Object) this.transform.parent != (UnityEngine.Object) null && (UnityEngine.Object) this.transform.parent.parent != (UnityEngine.Object) null)
      ovrCameraRig = this.transform.parent.parent.GetComponent<OVRCameraRig>();
    if (!((UnityEngine.Object) ovrCameraRig != (UnityEngine.Object) null))
      return;
    ovrCameraRig.UpdatedAnchors += (Action<OVRCameraRig>) (r => this.OnUpdatedAnchors());
    this.operatingWithoutOVRCameraRig = false;
  }

  protected virtual void Start()
  {
    this.m_lastPos = this.transform.position;
    this.m_lastRot = this.transform.rotation;
    if (!((UnityEngine.Object) this.m_parentTransform == (UnityEngine.Object) null))
      return;
    if ((UnityEngine.Object) this.gameObject.transform.parent != (UnityEngine.Object) null)
    {
      this.m_parentTransform = this.gameObject.transform.parent.transform;
    }
    else
    {
      this.m_parentTransform = new GameObject().transform;
      this.m_parentTransform.position = Vector3.zero;
      this.m_parentTransform.rotation = Quaternion.identity;
    }
  }

  private void FixedUpdate()
  {
    if (!this.operatingWithoutOVRCameraRig)
      return;
    this.OnUpdatedAnchors();
  }

  private void OnUpdatedAnchors()
  {
    Vector3 controllerPosition = OVRInput.GetLocalControllerPosition(this.m_controller);
    Quaternion controllerRotation = OVRInput.GetLocalControllerRotation(this.m_controller);
    Vector3 vector3 = this.m_parentTransform.TransformPoint(this.m_anchorOffsetPosition + controllerPosition);
    Quaternion rot = this.m_parentTransform.rotation * controllerRotation * this.m_anchorOffsetRotation;
    this.GetComponent<Rigidbody>().MovePosition(vector3);
    this.GetComponent<Rigidbody>().MoveRotation(rot);
    if (!this.m_parentHeldObject)
      this.MoveGrabbedObject(vector3, rot);
    this.m_lastPos = this.transform.position;
    this.m_lastRot = this.transform.rotation;
    float prevFlex = this.m_prevFlex;
    this.m_prevFlex = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, this.m_controller);
    this.CheckForGrabOrRelease(prevFlex);
  }

  private void OnDestroy()
  {
    if (!((UnityEngine.Object) this.m_grabbedObj != (UnityEngine.Object) null))
      return;
    this.GrabEnd();
  }

  private void OnTriggerEnter(Collider otherCollider)
  {
    OVRGrabbable key = otherCollider.GetComponent<OVRGrabbable>() ?? otherCollider.GetComponentInParent<OVRGrabbable>();
    if ((UnityEngine.Object) key == (UnityEngine.Object) null)
      return;
    int num = 0;
    this.m_grabCandidates.TryGetValue(key, out num);
    this.m_grabCandidates[key] = num + 1;
  }

  private void OnTriggerExit(Collider otherCollider)
  {
    OVRGrabbable key = otherCollider.GetComponent<OVRGrabbable>() ?? otherCollider.GetComponentInParent<OVRGrabbable>();
    if ((UnityEngine.Object) key == (UnityEngine.Object) null)
      return;
    int num = 0;
    if (!this.m_grabCandidates.TryGetValue(key, out num))
      return;
    if (num > 1)
      this.m_grabCandidates[key] = num - 1;
    else
      this.m_grabCandidates.Remove(key);
  }

  protected void CheckForGrabOrRelease(float prevFlex)
  {
    if ((double) this.m_prevFlex >= (double) this.grabBegin && (double) prevFlex < (double) this.grabBegin)
    {
      this.GrabBegin();
    }
    else
    {
      if ((double) this.m_prevFlex > (double) this.grabEnd || (double) prevFlex <= (double) this.grabEnd)
        return;
      this.GrabEnd();
    }
  }

  protected virtual void GrabBegin()
  {
    float num = float.MaxValue;
    OVRGrabbable grabbable = (OVRGrabbable) null;
    Collider grabPoint1 = (Collider) null;
    foreach (OVRGrabbable key in this.m_grabCandidates.Keys)
    {
      if ((!key.isGrabbed ? 1 : (key.allowOffhandGrab ? 1 : 0)) != 0)
      {
        for (int index = 0; index < key.grabPoints.Length; ++index)
        {
          Collider grabPoint2 = key.grabPoints[index];
          float sqrMagnitude = (this.m_gripTransform.position - grabPoint2.ClosestPointOnBounds(this.m_gripTransform.position)).sqrMagnitude;
          if ((double) sqrMagnitude < (double) num)
          {
            num = sqrMagnitude;
            grabbable = key;
            grabPoint1 = grabPoint2;
          }
        }
      }
    }
    this.GrabVolumeEnable(false);
    if (!((UnityEngine.Object) grabbable != (UnityEngine.Object) null))
      return;
    if (grabbable.isGrabbed)
      grabbable.grabbedBy.OffhandGrabbed(grabbable);
    this.m_grabbedObj = grabbable;
    this.m_grabbedObj.GrabBegin(this, grabPoint1);
    this.m_lastPos = this.transform.position;
    this.m_lastRot = this.transform.rotation;
    if (this.m_grabbedObj.snapPosition)
    {
      this.m_grabbedObjectPosOff = this.m_gripTransform.localPosition;
      if ((bool) (UnityEngine.Object) this.m_grabbedObj.snapOffset)
      {
        Vector3 position = this.m_grabbedObj.snapOffset.position;
        if (this.m_controller == OVRInput.Controller.LTouch)
          position.x = -position.x;
        this.m_grabbedObjectPosOff += position;
      }
    }
    else
    {
      Vector3 vector3 = this.m_grabbedObj.transform.position - this.transform.position;
      this.m_grabbedObjectPosOff = Quaternion.Inverse(this.transform.rotation) * vector3;
    }
    if (this.m_grabbedObj.snapOrientation)
    {
      this.m_grabbedObjectRotOff = this.m_gripTransform.localRotation;
      if ((bool) (UnityEngine.Object) this.m_grabbedObj.snapOffset)
        this.m_grabbedObjectRotOff = this.m_grabbedObj.snapOffset.rotation * this.m_grabbedObjectRotOff;
    }
    else
      this.m_grabbedObjectRotOff = Quaternion.Inverse(this.transform.rotation) * this.m_grabbedObj.transform.rotation;
    this.MoveGrabbedObject(this.m_lastPos, this.m_lastRot, true);
    if (!this.m_parentHeldObject)
      return;
    this.m_grabbedObj.transform.parent = this.transform;
  }

  protected virtual void MoveGrabbedObject(Vector3 pos, Quaternion rot, bool forceTeleport = false)
  {
    if ((UnityEngine.Object) this.m_grabbedObj == (UnityEngine.Object) null)
      return;
    Rigidbody grabbedRigidbody = this.m_grabbedObj.grabbedRigidbody;
    Vector3 position = pos + rot * this.m_grabbedObjectPosOff;
    Quaternion rot1 = rot * this.m_grabbedObjectRotOff;
    if (forceTeleport)
    {
      grabbedRigidbody.transform.position = position;
      grabbedRigidbody.transform.rotation = rot1;
    }
    else
    {
      grabbedRigidbody.MovePosition(position);
      grabbedRigidbody.MoveRotation(rot1);
    }
  }

  protected void GrabEnd()
  {
    if ((UnityEngine.Object) this.m_grabbedObj != (UnityEngine.Object) null)
    {
      OVRPose ovrPose1 = new OVRPose()
      {
        position = OVRInput.GetLocalControllerPosition(this.m_controller),
        orientation = OVRInput.GetLocalControllerRotation(this.m_controller)
      } * new OVRPose()
      {
        position = this.m_anchorOffsetPosition,
        orientation = this.m_anchorOffsetRotation
      };
      OVRPose ovrPose2 = this.transform.ToOVRPose() * ovrPose1.Inverse();
      this.GrabbableRelease(ovrPose2.orientation * OVRInput.GetLocalControllerVelocity(this.m_controller), ovrPose2.orientation * OVRInput.GetLocalControllerAngularVelocity(this.m_controller));
    }
    this.GrabVolumeEnable(true);
  }

  protected void GrabbableRelease(Vector3 linearVelocity, Vector3 angularVelocity)
  {
    this.m_grabbedObj.GrabEnd(linearVelocity, angularVelocity);
    if (this.m_parentHeldObject)
      this.m_grabbedObj.transform.parent = (Transform) null;
    this.m_grabbedObj = (OVRGrabbable) null;
  }

  protected virtual void GrabVolumeEnable(bool enabled)
  {
    if (this.m_grabVolumeEnabled == enabled)
      return;
    this.m_grabVolumeEnabled = enabled;
    for (int index = 0; index < this.m_grabVolumes.Length; ++index)
      this.m_grabVolumes[index].enabled = this.m_grabVolumeEnabled;
    if (this.m_grabVolumeEnabled)
      return;
    this.m_grabCandidates.Clear();
  }

  protected virtual void OffhandGrabbed(OVRGrabbable grabbable)
  {
    if (!((UnityEngine.Object) this.m_grabbedObj == (UnityEngine.Object) grabbable))
      return;
    this.GrabbableRelease(Vector3.zero, Vector3.zero);
  }
}
