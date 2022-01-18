// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets._2D.CameraFollow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace UnityStandardAssets._2D
{
  public class CameraFollow : MonoBehaviour
  {
    public float xMargin = 1f;
    public float yMargin = 1f;
    public float xSmooth = 8f;
    public float ySmooth = 8f;
    public Vector2 maxXAndY;
    public Vector2 minXAndY;
    private Transform m_Player;

    private void Awake() => this.m_Player = GameObject.FindGameObjectWithTag("Player").transform;

    private bool CheckXMargin() => (double) Mathf.Abs(this.transform.position.x - this.m_Player.position.x) > (double) this.xMargin;

    private bool CheckYMargin() => (double) Mathf.Abs(this.transform.position.y - this.m_Player.position.y) > (double) this.yMargin;

    private void Update() => this.TrackPlayer();

    private void TrackPlayer()
    {
      float num1 = this.transform.position.x;
      float num2 = this.transform.position.y;
      if (this.CheckXMargin())
        num1 = Mathf.Lerp(this.transform.position.x, this.m_Player.position.x, this.xSmooth * Time.deltaTime);
      if (this.CheckYMargin())
        num2 = Mathf.Lerp(this.transform.position.y, this.m_Player.position.y, this.ySmooth * Time.deltaTime);
      this.transform.position = new Vector3(Mathf.Clamp(num1, this.minXAndY.x, this.maxXAndY.x), Mathf.Clamp(num2, this.minXAndY.y, this.maxXAndY.y), this.transform.position.z);
    }
  }
}
