// Decompiled with JetBrains decompiler
// Type: ChainPhysics
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class ChainPhysics : MonoBehaviour
{
  private void OnTriggerStay(Collider collision)
  {
    GameObject gameObject = collision.gameObject;
    gameElement gameElement = gameObject.GetComponent<gameElement>();
    if (!(bool) (Object) gameElement)
      gameElement = gameObject.GetComponentInParent<gameElement>();
    if (!(bool) (Object) gameElement || gameElement.type == ElementType.Stone || gameElement.type == ElementType.Off || gameElement.type == ElementType.NoRigidBody)
      return;
    Rigidbody component = gameElement.gameObject.GetComponent<Rigidbody>();
    if ((Object) component == (Object) null)
      return;
    Vector3 velocity = component.velocity;
    velocity.x *= 0.9f;
    velocity.z *= 0.9f;
    if ((double) velocity.y > 0.0)
      velocity.y *= 0.9f;
    component.velocity = velocity;
  }
}
