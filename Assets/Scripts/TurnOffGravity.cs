// Decompiled with JetBrains decompiler
// Type: TurnOffGravity
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class TurnOffGravity : MonoBehaviour
{
  private void Start()
  {
  }

  private void Update()
  {
  }

  public void OnTriggerEnter(Collider collision)
  {
    RobotInterface3D componentInParent = collision.transform.GetComponentInParent<RobotInterface3D>();
    if (!(bool) (Object) componentInParent)
      return;
    foreach (Rigidbody componentsInChild in componentInParent.GetComponentsInChildren<Rigidbody>())
    {
      componentsInChild.useGravity = false;
      componentsInChild.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
      if (componentsInChild.transform.name.Contains("Wheel") || componentsInChild.transform.name.Contains("wheel"))
        componentsInChild.constraints = RigidbodyConstraints.FreezeAll;
    }
  }
}
