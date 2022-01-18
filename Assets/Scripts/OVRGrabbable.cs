// Decompiled with JetBrains decompiler
// Type: OVRGrabbable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class OVRGrabbable : MonoBehaviour
{
  [SerializeField]
  protected bool m_allowOffhandGrab = true;
  [SerializeField]
  protected bool m_snapPosition;
  [SerializeField]
  protected bool m_snapOrientation;
  [SerializeField]
  protected Transform m_snapOffset;
  [SerializeField]
  protected Collider[] m_grabPoints;
  protected bool m_grabbedKinematic;
  protected Collider m_grabbedCollider;
  protected OVRGrabber m_grabbedBy;

  public bool allowOffhandGrab => this.m_allowOffhandGrab;

  public bool isGrabbed => (UnityEngine.Object) this.m_grabbedBy != (UnityEngine.Object) null;

  public bool snapPosition => this.m_snapPosition;

  public bool snapOrientation => this.m_snapOrientation;

  public Transform snapOffset => this.m_snapOffset;

  public OVRGrabber grabbedBy => this.m_grabbedBy;

  public Transform grabbedTransform => this.m_grabbedCollider.transform;

  public Rigidbody grabbedRigidbody => this.m_grabbedCollider.attachedRigidbody;

  public Collider[] grabPoints => this.m_grabPoints;

  public virtual void GrabBegin(OVRGrabber hand, Collider grabPoint)
  {
    this.m_grabbedBy = hand;
    this.m_grabbedCollider = grabPoint;
    this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
  }

  public virtual void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity)
  {
    Rigidbody component = this.gameObject.GetComponent<Rigidbody>();
    component.isKinematic = this.m_grabbedKinematic;
    component.velocity = linearVelocity;
    component.angularVelocity = angularVelocity;
    this.m_grabbedBy = (OVRGrabber) null;
    this.m_grabbedCollider = (Collider) null;
  }

  private void Awake()
  {
    if (this.m_grabPoints.Length != 0)
      return;
    Collider component = this.GetComponent<Collider>();
    this.m_grabPoints = !((UnityEngine.Object) component == (UnityEngine.Object) null) ? new Collider[1]
    {
      component
    } : throw new ArgumentException("Grabbables cannot have zero grab points and no collider -- please add a grab point or collider.");
  }

  protected virtual void Start() => this.m_grabbedKinematic = this.GetComponent<Rigidbody>().isKinematic;

  private void OnDestroy()
  {
    if (!((UnityEngine.Object) this.m_grabbedBy != (UnityEngine.Object) null))
      return;
    this.m_grabbedBy.ForceRelease(this);
  }
}
