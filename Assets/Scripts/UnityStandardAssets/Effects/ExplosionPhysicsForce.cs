// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets.Effects.ExplosionPhysicsForce
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityStandardAssets.Effects
{
  public class ExplosionPhysicsForce : MonoBehaviour
  {
    public float explosionForce = 4f;

    private IEnumerator Start()
    {
      ExplosionPhysicsForce explosionPhysicsForce = this;
      yield return (object) null;
      float multiplier = explosionPhysicsForce.GetComponent<ParticleSystemMultiplier>().multiplier;
      float num = 10f * multiplier;
      Collider[] colliderArray = Physics.OverlapSphere(explosionPhysicsForce.transform.position, num);
      List<Rigidbody> rigidbodyList = new List<Rigidbody>();
      foreach (Collider collider in colliderArray)
      {
        if ((Object) collider.attachedRigidbody != (Object) null && !rigidbodyList.Contains(collider.attachedRigidbody))
          rigidbodyList.Add(collider.attachedRigidbody);
      }
      foreach (Rigidbody rigidbody in rigidbodyList)
        rigidbody.AddExplosionForce(explosionPhysicsForce.explosionForce * multiplier, explosionPhysicsForce.transform.position, num, 1f * multiplier, ForceMode.Impulse);
    }
  }
}
