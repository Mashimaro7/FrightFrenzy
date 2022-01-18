// Decompiled with JetBrains decompiler
// Type: TiltWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class TiltWindow : MonoBehaviour
{
  public Vector2 range = new Vector2(5f, 3f);
  private Transform mTrans;
  private Quaternion mStart;
  private Vector2 mRot = Vector2.zero;

  private void Start()
  {
    this.mTrans = this.transform;
    this.mStart = this.mTrans.localRotation;
  }

  private void Update()
  {
    Vector3 mousePosition = Input.mousePosition;
    float num1 = (float) Screen.width * 0.5f;
    float num2 = (float) Screen.height * 0.5f;
    this.mRot = Vector2.Lerp(this.mRot, new Vector2(Mathf.Clamp((mousePosition.x - num1) / num1, -1f, 1f), Mathf.Clamp((mousePosition.y - num2) / num2, -1f, 1f)), Time.deltaTime * 5f);
    this.mTrans.localRotation = this.mStart * Quaternion.Euler(-this.mRot.y * this.range.y, this.mRot.x * this.range.x, 0.0f);
  }
}
