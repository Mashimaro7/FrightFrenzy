// Decompiled with JetBrains decompiler
// Type: PhysicsDebug.MouseOrbit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace PhysicsDebug
{
  [AddComponentMenu("Camera-Control/Mouse Orbit with zoom")]
  public class MouseOrbit : MonoBehaviour
  {
    public Transform target;
    public float distance = 5f;
    public float xSpeed = 120f;
    public float ySpeed = 120f;
    public float smoothSpeed = 16f;
    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;
    public float distanceMin = 0.5f;
    public float distanceMax = 15f;
    private float x;
    private float y;
    private float prevRealTime;
    private float thisRealTime;

    private void Start()
    {
      Vector3 eulerAngles = this.transform.eulerAngles;
      this.x = eulerAngles.y;
      this.y = eulerAngles.x;
      if (!(bool) (Object) this.GetComponent<Rigidbody>())
        return;
      this.GetComponent<Rigidbody>().freezeRotation = true;
    }

    private void Update()
    {
      this.prevRealTime = this.thisRealTime;
      this.thisRealTime = Time.realtimeSinceStartup;
      RaycastHit hitInfo;
      if (!Input.GetMouseButtonDown(2) || !Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo) || !(bool) (Object) hitInfo.collider.transform.parent || !(hitInfo.collider.transform.parent.name == "Targets"))
        return;
      this.target = hitInfo.collider.transform;
    }

    public float deltaTime => (double) Time.timeScale > 0.0 ? Time.deltaTime / Time.timeScale : Time.realtimeSinceStartup - this.prevRealTime;

    private void LateUpdate()
    {
      if (!(bool) (Object) this.target)
        return;
      if (Input.GetMouseButton(1))
      {
        this.x += (float) ((double) Input.GetAxis("Mouse X") * (double) this.xSpeed * 0.0199999995529652);
        this.y -= (float) ((double) Input.GetAxis("Mouse Y") * (double) this.ySpeed * 0.0199999995529652);
        this.y = MouseOrbit.ClampAngle(this.y, this.yMinLimit, this.yMaxLimit);
      }
      Quaternion b1 = Quaternion.Euler(this.y, this.x, 0.0f);
      this.distance = Mathf.Clamp(this.distance - Input.GetAxis("Mouse ScrollWheel") * 15f, this.distanceMin, this.distanceMax);
      Vector3 vector3 = new Vector3(0.0f, 0.0f, -this.distance);
      Vector3 b2 = b1 * vector3 + this.target.position;
      this.transform.rotation = Quaternion.Slerp(this.transform.rotation, b1, this.deltaTime * this.smoothSpeed);
      this.transform.position = Vector3.Lerp(this.transform.position, b2, this.deltaTime * this.smoothSpeed);
    }

    public static float ClampAngle(float angle, float min, float max)
    {
      if ((double) angle < -360.0)
        angle += 360f;
      if ((double) angle > 360.0)
        angle -= 360f;
      return Mathf.Clamp(angle, min, max);
    }
  }
}
