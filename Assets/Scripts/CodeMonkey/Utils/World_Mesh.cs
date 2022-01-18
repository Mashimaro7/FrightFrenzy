// Decompiled with JetBrains decompiler
// Type: CodeMonkey.Utils.World_Mesh
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

namespace CodeMonkey.Utils
{
  public class World_Mesh
  {
    private const int sortingOrderDefault = 5000;
    public GameObject gameObject;
    public Transform transform;
    private Material material;
    private Vector3[] vertices;
    private Vector2[] uv;
    private int[] triangles;
    private Mesh mesh;

    public static World_Mesh Create(
      Vector3 position,
      float eulerZ,
      float meshWidth,
      float meshHeight,
      Material material,
      World_Mesh.UVCoords uvCoords,
      int sortingOrderOffset = 0)
    {
      return new World_Mesh((Transform) null, position, Vector3.one, eulerZ, meshWidth, meshHeight, material, uvCoords, sortingOrderOffset);
    }

    public static World_Mesh Create(
      Vector3 lowerLeftCorner,
      float width,
      float height,
      Material material,
      World_Mesh.UVCoords uvCoords,
      int sortingOrderOffset = 0)
    {
      return World_Mesh.Create(lowerLeftCorner, lowerLeftCorner + new Vector3(width, height), material, uvCoords, sortingOrderOffset);
    }

    public static World_Mesh Create(
      Vector3 lowerLeftCorner,
      Vector3 upperRightCorner,
      Material material,
      World_Mesh.UVCoords uvCoords,
      int sortingOrderOffset = 0)
    {
      float meshWidth = upperRightCorner.x - lowerLeftCorner.x;
      float meshHeight = upperRightCorner.y - lowerLeftCorner.y;
      Vector3 vector3 = upperRightCorner - lowerLeftCorner;
      return new World_Mesh((Transform) null, lowerLeftCorner + vector3 * 0.5f, Vector3.one, 0.0f, meshWidth, meshHeight, material, uvCoords, sortingOrderOffset);
    }

    private static int GetSortingOrder(Vector3 position, int offset, int baseSortingOrder = 5000) => (int) ((double) baseSortingOrder - (double) position.y) + offset;

    public World_Mesh(
      Transform parent,
      Vector3 localPosition,
      Vector3 localScale,
      float eulerZ,
      float meshWidth,
      float meshHeight,
      Material material,
      World_Mesh.UVCoords uvCoords,
      int sortingOrderOffset)
    {
      this.material = material;
      this.vertices = new Vector3[4];
      this.uv = new Vector2[4];
      this.triangles = new int[6];
      float x = meshWidth / 2f;
      float y = meshHeight / 2f;
      this.vertices[0] = new Vector3(-x, y);
      this.vertices[1] = new Vector3(x, y);
      this.vertices[2] = new Vector3(-x, -y);
      this.vertices[3] = new Vector3(x, -y);
      if (uvCoords == null)
        uvCoords = new World_Mesh.UVCoords(0, 0, material.mainTexture.width, material.mainTexture.height);
      this.ApplyUVToUVArray(this.GetUVRectangleFromPixels(uvCoords.x, uvCoords.y, uvCoords.width, uvCoords.height, material.mainTexture.width, material.mainTexture.height), ref this.uv);
      this.triangles[0] = 0;
      this.triangles[1] = 1;
      this.triangles[2] = 2;
      this.triangles[3] = 2;
      this.triangles[4] = 1;
      this.triangles[5] = 3;
      this.mesh = new Mesh();
      this.mesh.vertices = this.vertices;
      this.mesh.uv = this.uv;
      this.mesh.triangles = this.triangles;
      this.gameObject = new GameObject("Mesh", new System.Type[2]
      {
        typeof (MeshFilter),
        typeof (MeshRenderer)
      });
      this.gameObject.transform.parent = parent;
      this.gameObject.transform.localPosition = localPosition;
      this.gameObject.transform.localScale = localScale;
      this.gameObject.transform.localEulerAngles = new Vector3(0.0f, 0.0f, eulerZ);
      this.gameObject.GetComponent<MeshFilter>().mesh = this.mesh;
      this.gameObject.GetComponent<MeshRenderer>().material = material;
      this.transform = this.gameObject.transform;
      this.SetSortingOrderOffset(sortingOrderOffset);
    }

    private Vector2 ConvertPixelsToUVCoordinates(
      int x,
      int y,
      int textureWidth,
      int textureHeight)
    {
      return new Vector2((float) x / (float) textureWidth, (float) y / (float) textureHeight);
    }

    private Vector2[] GetUVRectangleFromPixels(
      int x,
      int y,
      int width,
      int height,
      int textureWidth,
      int textureHeight)
    {
      return new Vector2[4]
      {
        this.ConvertPixelsToUVCoordinates(x, y + height, textureWidth, textureHeight),
        this.ConvertPixelsToUVCoordinates(x + width, y + height, textureWidth, textureHeight),
        this.ConvertPixelsToUVCoordinates(x, y, textureWidth, textureHeight),
        this.ConvertPixelsToUVCoordinates(x + width, y, textureWidth, textureHeight)
      };
    }

    private void ApplyUVToUVArray(Vector2[] uv, ref Vector2[] mainUV)
    {
      if (uv == null || uv.Length < 4 || mainUV == null || mainUV.Length < 4)
        throw new Exception();
      mainUV[0] = uv[0];
      mainUV[1] = uv[1];
      mainUV[2] = uv[2];
      mainUV[3] = uv[3];
    }

    public void SetUVCoords(World_Mesh.UVCoords uvCoords)
    {
      this.ApplyUVToUVArray(this.GetUVRectangleFromPixels(uvCoords.x, uvCoords.y, uvCoords.width, uvCoords.height, this.material.mainTexture.width, this.material.mainTexture.height), ref this.uv);
      this.mesh.uv = this.uv;
    }

    public void SetSortingOrderOffset(int sortingOrderOffset) => this.SetSortingOrder(World_Mesh.GetSortingOrder(this.gameObject.transform.position, sortingOrderOffset));

    public void SetSortingOrder(int sortingOrder) => this.gameObject.GetComponent<Renderer>().sortingOrder = sortingOrder;

    public void SetLocalScale(Vector3 localScale) => this.transform.localScale = localScale;

    public void SetPosition(Vector3 localPosition) => this.transform.localPosition = localPosition;

    public void AddPosition(Vector3 addPosition) => this.transform.localPosition += addPosition;

    public Vector3 GetPosition() => this.transform.localPosition;

    public int GetSortingOrder() => this.gameObject.GetComponent<Renderer>().sortingOrder;

    public void Show() => this.gameObject.SetActive(true);

    public void Hide() => this.gameObject.SetActive(false);

    public void DestroySelf() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);

    public class UVCoords
    {
      public int x;
      public int y;
      public int width;
      public int height;

      public UVCoords(int x, int y, int width, int height)
      {
        this.x = x;
        this.y = y;
        this.width = width;
        this.height = height;
      }
    }
  }
}
