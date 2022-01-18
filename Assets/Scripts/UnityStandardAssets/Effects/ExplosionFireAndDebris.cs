// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets.Effects.ExplosionFireAndDebris
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

namespace UnityStandardAssets.Effects
{
  public class ExplosionFireAndDebris : MonoBehaviour
  {
    public Transform[] debrisPrefabs;
    public Transform firePrefab;
    public int numDebrisPieces;
    public int numFires;

    private IEnumerator Start()
    {
      ExplosionFireAndDebris explosionFireAndDebris = this;
      float multiplier = explosionFireAndDebris.GetComponent<ParticleSystemMultiplier>().multiplier;
      for (int index = 0; (double) index < (double) explosionFireAndDebris.numDebrisPieces * (double) multiplier; ++index)
      {
        Transform debrisPrefab = explosionFireAndDebris.debrisPrefabs[Random.Range(0, explosionFireAndDebris.debrisPrefabs.Length)];
        Vector3 vector3 = explosionFireAndDebris.transform.position + Random.insideUnitSphere * 3f * multiplier;
        Quaternion rotation1 = Random.rotation;
        Vector3 position = vector3;
        Quaternion rotation2 = rotation1;
        Object.Instantiate<Transform>(debrisPrefab, position, rotation2);
      }
      yield return (object) null;
      float num = 10f * multiplier;
      foreach (Collider collider in Physics.OverlapSphere(explosionFireAndDebris.transform.position, num))
      {
        if (explosionFireAndDebris.numFires > 0)
        {
          Ray ray = new Ray(explosionFireAndDebris.transform.position, collider.transform.position - explosionFireAndDebris.transform.position);
          RaycastHit hitInfo;
          if (collider.Raycast(ray, out hitInfo, num))
          {
            explosionFireAndDebris.AddFire(collider.transform, hitInfo.point, hitInfo.normal);
            --explosionFireAndDebris.numFires;
          }
        }
      }
      for (float maxDistance = 0.0f; explosionFireAndDebris.numFires > 0 && (double) maxDistance < (double) num; maxDistance += num * 0.1f)
      {
        RaycastHit hitInfo;
        if (Physics.Raycast(new Ray(explosionFireAndDebris.transform.position + Vector3.up, Random.onUnitSphere), out hitInfo, maxDistance))
        {
          explosionFireAndDebris.AddFire((Transform) null, hitInfo.point, hitInfo.normal);
          --explosionFireAndDebris.numFires;
        }
      }
    }

    private void AddFire(Transform t, Vector3 pos, Vector3 normal)
    {
      pos += normal * 0.5f;
      Object.Instantiate<Transform>(this.firePrefab, pos, Quaternion.identity).parent = t;
    }
  }
}
