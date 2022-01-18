// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets.Water.MeshContainer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace UnityStandardAssets.Water
{
  public class MeshContainer
  {
    public Mesh mesh;
    public Vector3[] vertices;
    public Vector3[] normals;

    public MeshContainer(Mesh m)
    {
      this.mesh = m;
      this.vertices = m.vertices;
      this.normals = m.normals;
    }

    public void Update()
    {
      this.mesh.vertices = this.vertices;
      this.mesh.normals = this.normals;
    }
  }
}
