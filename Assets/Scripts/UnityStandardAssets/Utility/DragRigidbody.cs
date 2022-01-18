// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets.Utility.DragRigidbody
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

namespace UnityStandardAssets.Utility
{
  public class DragRigidbody : MonoBehaviour
  {
    private const float k_Spring = 50f;
    private const float k_Damper = 5f;
    private const float k_Drag = 10f;
    private const float k_AngularDrag = 5f;
    private const float k_Distance = 0.2f;
    private const bool k_AttachToCenterOfMass = false;
    private SpringJoint m_SpringJoint;

    private void Update()
    {
      if (!Input.GetMouseButtonDown(0))
        return;
      Camera camera = this.FindCamera();
      RaycastHit hitInfo = new RaycastHit();
      if (!Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition).origin, camera.ScreenPointToRay(Input.mousePosition).direction, out hitInfo, 100f, -5) || !(bool) (Object) hitInfo.rigidbody || hitInfo.rigidbody.isKinematic)
        return;
      if (!(bool) (Object) this.m_SpringJoint)
      {
        GameObject gameObject = new GameObject("Rigidbody dragger");
        Rigidbody rigidbody = gameObject.AddComponent<Rigidbody>();
        this.m_SpringJoint = gameObject.AddComponent<SpringJoint>();
        rigidbody.isKinematic = true;
      }
      this.m_SpringJoint.transform.position = hitInfo.point;
      this.m_SpringJoint.anchor = Vector3.zero;
      this.m_SpringJoint.spring = 50f;
      this.m_SpringJoint.damper = 5f;
      this.m_SpringJoint.maxDistance = 0.2f;
      this.m_SpringJoint.connectedBody = hitInfo.rigidbody;
      this.StartCoroutine("DragObject", (object) hitInfo.distance);
    }

    private IEnumerator DragObject(float distance)
    {
      float oldDrag = this.m_SpringJoint.connectedBody.drag;
      float oldAngularDrag = this.m_SpringJoint.connectedBody.angularDrag;
      this.m_SpringJoint.connectedBody.drag = 10f;
      this.m_SpringJoint.connectedBody.angularDrag = 5f;
      Camera mainCamera = this.FindCamera();
      while (Input.GetMouseButton(0))
      {
        this.m_SpringJoint.transform.position = mainCamera.ScreenPointToRay(Input.mousePosition).GetPoint(distance);
        yield return (object) null;
      }
      if ((bool) (Object) this.m_SpringJoint.connectedBody)
      {
        this.m_SpringJoint.connectedBody.drag = oldDrag;
        this.m_SpringJoint.connectedBody.angularDrag = oldAngularDrag;
        this.m_SpringJoint.connectedBody = (Rigidbody) null;
      }
    }

    private Camera FindCamera() => (bool) (Object) this.GetComponent<Camera>() ? this.GetComponent<Camera>() : Camera.main;
  }
}
