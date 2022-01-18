// Decompiled with JetBrains decompiler
// Type: cameraOrbit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class cameraOrbit : MonoBehaviour
{
  public bool enabled = true;
  public Transform orbitAround;
  public float speed = 10f;
  public float aspect_ratio;
  private float elapsedTime;

  private void Start()
  {
    if ((double) this.aspect_ratio <= 0.0)
      return;
    Camera component = this.GetComponent<Camera>();
    if ((Object) component == (Object) null)
      return;
    component.aspect = this.aspect_ratio;
  }

  private void Update()
  {
    if (!this.enabled)
      return;
    float deltaTime = Time.deltaTime;
    this.elapsedTime += Time.deltaTime;
    this.transform.RotateAround(this.orbitAround.position, Vector3.up, this.speed * deltaTime);
  }
}
