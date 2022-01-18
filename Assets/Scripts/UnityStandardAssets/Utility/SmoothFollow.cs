// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets.Utility.SmoothFollow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace UnityStandardAssets.Utility
{
  public class SmoothFollow : MonoBehaviour
  {
    [SerializeField]
    private Transform target;
    [SerializeField]
    private float distance = 10f;
    [SerializeField]
    private float height = 5f;
    [SerializeField]
    private float rotationDamping;
    [SerializeField]
    private float heightDamping;

    private void Start()
    {
    }

    private void LateUpdate()
    {
      if (!(bool) (Object) this.target)
        return;
      float y1 = this.target.eulerAngles.y;
      float b = this.target.position.y + this.height;
      float y2 = this.transform.eulerAngles.y;
      float y3 = this.transform.position.y;
      float y4 = Mathf.LerpAngle(y2, y1, this.rotationDamping * Time.deltaTime);
      float y5 = Mathf.Lerp(y3, b, this.heightDamping * Time.deltaTime);
      Quaternion quaternion = Quaternion.Euler(0.0f, y4, 0.0f);
      this.transform.position = this.target.position;
      this.transform.position -= quaternion * Vector3.forward * this.distance;
      this.transform.position = new Vector3(this.transform.position.x, y5, this.transform.position.z);
      this.transform.LookAt(this.target);
    }
  }
}
