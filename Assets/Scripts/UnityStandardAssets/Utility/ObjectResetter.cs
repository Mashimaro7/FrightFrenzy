// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets.Utility.ObjectResetter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityStandardAssets.Utility
{
  public class ObjectResetter : MonoBehaviour
  {
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private List<Transform> originalStructure;
    private Rigidbody Rigidbody;

    private void Start()
    {
      this.originalStructure = new List<Transform>((IEnumerable<Transform>) this.GetComponentsInChildren<Transform>());
      this.originalPosition = this.transform.position;
      this.originalRotation = this.transform.rotation;
      this.Rigidbody = this.GetComponent<Rigidbody>();
    }

    public void DelayedReset(float delay) => this.StartCoroutine(this.ResetCoroutine(delay));

    public IEnumerator ResetCoroutine(float delay)
    {
      ObjectResetter objectResetter = this;
      yield return (object) new WaitForSeconds(delay);
      foreach (Transform componentsInChild in objectResetter.GetComponentsInChildren<Transform>())
      {
        if (!objectResetter.originalStructure.Contains(componentsInChild))
          componentsInChild.parent = (Transform) null;
      }
      objectResetter.transform.position = objectResetter.originalPosition;
      objectResetter.transform.rotation = objectResetter.originalRotation;
      if ((bool) (Object) objectResetter.Rigidbody)
      {
        objectResetter.Rigidbody.velocity = Vector3.zero;
        objectResetter.Rigidbody.angularVelocity = Vector3.zero;
      }
      objectResetter.SendMessage("Reset");
    }
  }
}
