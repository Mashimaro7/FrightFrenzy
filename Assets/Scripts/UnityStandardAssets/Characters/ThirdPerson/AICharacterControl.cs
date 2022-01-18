// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets.Characters.ThirdPerson.AICharacterControl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.AI;

namespace UnityStandardAssets.Characters.ThirdPerson
{
  [RequireComponent(typeof (NavMeshAgent))]
  [RequireComponent(typeof (ThirdPersonCharacter))]
  public class AICharacterControl : MonoBehaviour
  {
    public Transform target;

    public NavMeshAgent agent { get; private set; }

    public ThirdPersonCharacter character { get; private set; }

    private void Start()
    {
      this.agent = this.GetComponentInChildren<NavMeshAgent>();
      this.character = this.GetComponent<ThirdPersonCharacter>();
      this.agent.updateRotation = false;
      this.agent.updatePosition = true;
    }

    private void Update()
    {
      if ((Object) this.target != (Object) null)
        this.agent.SetDestination(this.target.position);
      if ((double) this.agent.remainingDistance > (double) this.agent.stoppingDistance)
        this.character.Move(this.agent.desiredVelocity, false, false);
      else
        this.character.Move(Vector3.zero, false, false);
    }

    public void SetTarget(Transform target) => this.target = target;
  }
}
