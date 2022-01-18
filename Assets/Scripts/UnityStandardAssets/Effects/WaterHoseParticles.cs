// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets.Effects.WaterHoseParticles
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace UnityStandardAssets.Effects
{
  public class WaterHoseParticles : MonoBehaviour
  {
    public static float lastSoundTime;
    public float force = 1f;
    private List<ParticleCollisionEvent> m_CollisionEvents = new List<ParticleCollisionEvent>();
    private ParticleSystem m_ParticleSystem;

    private void Start() => this.m_ParticleSystem = this.GetComponent<ParticleSystem>();

    private void OnParticleCollision(GameObject other)
    {
      int collisionEvents = ParticlePhysicsExtensions.GetCollisionEvents(this.m_ParticleSystem, other, this.m_CollisionEvents);
      for (int index = 0; index < collisionEvents; ++index)
      {
        if ((double) Time.time > (double) WaterHoseParticles.lastSoundTime + 0.200000002980232)
          WaterHoseParticles.lastSoundTime = Time.time;
        Rigidbody component = this.m_CollisionEvents[index].colliderComponent.GetComponent<Rigidbody>();
        if ((Object) component != (Object) null)
        {
          Vector3 velocity = this.m_CollisionEvents[index].velocity;
          component.AddForce(velocity * this.force, ForceMode.Impulse);
        }
        other.BroadcastMessage("Extinguish", SendMessageOptions.DontRequireReceiver);
      }
    }
  }
}
