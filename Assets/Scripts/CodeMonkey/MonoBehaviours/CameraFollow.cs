// Decompiled with JetBrains decompiler
// Type: CodeMonkey.MonoBehaviours.CameraFollow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

namespace CodeMonkey.MonoBehaviours
{
  public class CameraFollow : MonoBehaviour
  {
    private Camera myCamera;
    private Func<Vector3> GetCameraFollowPositionFunc;
    private Func<float> GetCameraZoomFunc;

    public void Setup(Func<Vector3> GetCameraFollowPositionFunc, Func<float> GetCameraZoomFunc)
    {
      this.GetCameraFollowPositionFunc = GetCameraFollowPositionFunc;
      this.GetCameraZoomFunc = GetCameraZoomFunc;
    }

    private void Start() => this.myCamera = this.transform.GetComponent<Camera>();

    public void SetCameraFollowPosition(Vector3 cameraFollowPosition) => this.SetGetCameraFollowPositionFunc((Func<Vector3>) (() => cameraFollowPosition));

    public void SetGetCameraFollowPositionFunc(Func<Vector3> GetCameraFollowPositionFunc) => this.GetCameraFollowPositionFunc = GetCameraFollowPositionFunc;

    public void SetCameraZoom(float cameraZoom) => this.SetGetCameraZoomFunc((Func<float>) (() => cameraZoom));

    public void SetGetCameraZoomFunc(Func<float> GetCameraZoomFunc) => this.GetCameraZoomFunc = GetCameraZoomFunc;

    private void Update()
    {
      this.HandleMovement();
      this.HandleZoom();
    }

    private void HandleMovement()
    {
      Vector3 vector3 = this.GetCameraFollowPositionFunc() with
      {
        z = this.transform.position.z
      };
      Vector3 normalized = (vector3 - this.transform.position).normalized;
      float num1 = Vector3.Distance(vector3, this.transform.position);
      float num2 = 3f;
      if ((double) num1 <= 0.0)
        return;
      Vector3 a = this.transform.position + normalized * num1 * num2 * Time.deltaTime;
      if ((double) Vector3.Distance(a, vector3) > (double) num1)
        a = vector3;
      this.transform.position = a;
    }

    private void HandleZoom()
    {
      float num1 = this.GetCameraZoomFunc();
      float num2 = num1 - this.myCamera.orthographicSize;
      float num3 = 1f;
      this.myCamera.orthographicSize += num2 * num3 * Time.deltaTime;
      if ((double) num2 > 0.0)
      {
        if ((double) this.myCamera.orthographicSize <= (double) num1)
          return;
        this.myCamera.orthographicSize = num1;
      }
      else
      {
        if ((double) this.myCamera.orthographicSize >= (double) num1)
          return;
        this.myCamera.orthographicSize = num1;
      }
    }
  }
}
