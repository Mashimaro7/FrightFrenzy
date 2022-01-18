// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets.Effects.Explosive
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;
using UnityStandardAssets.Utility;

namespace UnityStandardAssets.Effects
{
  public class Explosive : MonoBehaviour
  {
    public Transform explosionPrefab;
    public float detonationImpactVelocity = 10f;
    public float sizeMultiplier = 1f;
    public bool reset = true;
    public float resetTimeDelay = 10f;
    private bool m_Exploded;
    private ObjectResetter m_ObjectResetter;

    private void Start() => this.m_ObjectResetter = this.GetComponent<ObjectResetter>();

    private IEnumerator OnCollisionEnter(Collision col)
    {
      Explosive explosive = this;
      if (explosive.enabled && col.contacts.Length != 0 && ((double) Vector3.Project(col.relativeVelocity, col.contacts[0].normal).magnitude > (double) explosive.detonationImpactVelocity || explosive.m_Exploded) && !explosive.m_Exploded)
      {
        Object.Instantiate<Transform>(explosive.explosionPrefab, col.contacts[0].point, Quaternion.LookRotation(col.contacts[0].normal));
        explosive.m_Exploded = true;
        explosive.SendMessage("Immobilize");
        if (explosive.reset)
          explosive.m_ObjectResetter.DelayedReset(explosive.resetTimeDelay);
      }
      yield return (object) null;
    }

    public void Reset() => this.m_Exploded = false;
  }
}
