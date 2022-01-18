// Decompiled with JetBrains decompiler
// Type: CodeMonkey.MonoBehaviours.CameraFollowSetup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

namespace CodeMonkey.MonoBehaviours
{
  public class CameraFollowSetup : MonoBehaviour
  {
    [SerializeField]
    private CameraFollow cameraFollow;
    [SerializeField]
    private Transform followTransform;
    [SerializeField]
    private float zoom;

    private void Start()
    {
      if ((UnityEngine.Object) this.followTransform == (UnityEngine.Object) null)
      {
        Debug.LogError((object) "followTransform is null! Intended?");
        this.cameraFollow.Setup((Func<Vector3>) (() => Vector3.zero), (Func<float>) (() => this.zoom));
      }
      else
        this.cameraFollow.Setup((Func<Vector3>) (() => this.followTransform.position), (Func<float>) (() => this.zoom));
    }
  }
}
