// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets.Effects.AfterburnerPhysicsForce
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace UnityStandardAssets.Effects
{
  [RequireComponent(typeof (SphereCollider))]
  public class AfterburnerPhysicsForce : MonoBehaviour
  {
    public float effectAngle = 15f;
    public float effectWidth = 1f;
    public float effectDistance = 10f;
    public float force = 10f;
    private Collider[] m_Cols;
    private SphereCollider m_Sphere;

    private void OnEnable() => this.m_Sphere = this.GetComponent<Collider>() as SphereCollider;

    private void FixedUpdate()
    {
      this.m_Cols = Physics.OverlapSphere(this.transform.position + this.m_Sphere.center, this.m_Sphere.radius);
      for (int index = 0; index < this.m_Cols.Length; ++index)
      {
        if ((Object) this.m_Cols[index].attachedRigidbody != (Object) null)
        {
          Vector3 current = this.transform.InverseTransformPoint(this.m_Cols[index].transform.position);
          Vector3 vector3_1 = Vector3.MoveTowards(current, new Vector3(0.0f, 0.0f, current.z), this.effectWidth * 0.5f);
          float num1 = Mathf.Abs(Mathf.Atan2(vector3_1.x, vector3_1.z) * 57.29578f);
          float num2 = Mathf.InverseLerp(this.effectDistance, 0.0f, vector3_1.magnitude) * Mathf.InverseLerp(this.effectAngle, 0.0f, num1);
          Vector3 vector3_2 = this.m_Cols[index].transform.position - this.transform.position;
          this.m_Cols[index].attachedRigidbody.AddForceAtPosition(vector3_2.normalized * this.force * num2, Vector3.Lerp(this.m_Cols[index].transform.position, this.transform.TransformPoint(0.0f, 0.0f, vector3_1.z), 0.1f));
        }
      }
    }

    private void OnDrawGizmosSelected()
    {
      if ((Object) this.m_Sphere == (Object) null)
        this.m_Sphere = this.GetComponent<Collider>() as SphereCollider;
      this.m_Sphere.radius = this.effectDistance * 0.5f;
      this.m_Sphere.center = new Vector3(0.0f, 0.0f, this.effectDistance * 0.5f);
      Vector3[] vector3Array1 = new Vector3[4]
      {
        Vector3.up,
        -Vector3.up,
        Vector3.right,
        -Vector3.right
      };
      Vector3[] vector3Array2 = new Vector3[4]
      {
        -Vector3.right,
        Vector3.right,
        Vector3.up,
        -Vector3.up
      };
      Gizmos.color = new Color(0.0f, 1f, 0.0f, 0.5f);
      for (int index = 0; index < 4; ++index)
      {
        Vector3 from = this.transform.position + this.transform.rotation * vector3Array1[index] * this.effectWidth * 0.5f;
        Gizmos.DrawLine(from, from + this.transform.TransformDirection(Quaternion.AngleAxis(this.effectAngle, vector3Array2[index]) * Vector3.forward) * this.m_Sphere.radius * 2f);
      }
    }
  }
}
