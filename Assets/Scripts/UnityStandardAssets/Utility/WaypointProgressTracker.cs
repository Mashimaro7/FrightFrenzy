// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets.Utility.WaypointProgressTracker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace UnityStandardAssets.Utility
{
  public class WaypointProgressTracker : MonoBehaviour
  {
    [SerializeField]
    private WaypointCircuit circuit;
    [SerializeField]
    private float lookAheadForTargetOffset = 5f;
    [SerializeField]
    private float lookAheadForTargetFactor = 0.1f;
    [SerializeField]
    private float lookAheadForSpeedOffset = 10f;
    [SerializeField]
    private float lookAheadForSpeedFactor = 0.2f;
    [SerializeField]
    private WaypointProgressTracker.ProgressStyle progressStyle;
    [SerializeField]
    private float pointToPointThreshold = 4f;
    public Transform target;
    private float progressDistance;
    private int progressNum;
    private Vector3 lastPosition;
    private float speed;

    public WaypointCircuit.RoutePoint targetPoint { get; private set; }

    public WaypointCircuit.RoutePoint speedPoint { get; private set; }

    public WaypointCircuit.RoutePoint progressPoint { get; private set; }

    private void Start()
    {
      if ((Object) this.target == (Object) null)
        this.target = new GameObject(this.name + " Waypoint Target").transform;
      this.Reset();
    }

    public void Reset()
    {
      this.progressDistance = 0.0f;
      this.progressNum = 0;
      if (this.progressStyle != WaypointProgressTracker.ProgressStyle.PointToPoint)
        return;
      this.target.position = this.circuit.Waypoints[this.progressNum].position;
      this.target.rotation = this.circuit.Waypoints[this.progressNum].rotation;
    }

    private void Update()
    {
      if (this.progressStyle == WaypointProgressTracker.ProgressStyle.SmoothAlongRoute)
      {
        if ((double) Time.deltaTime > 0.0)
          this.speed = Mathf.Lerp(this.speed, (this.lastPosition - this.transform.position).magnitude / Time.deltaTime, Time.deltaTime);
        this.target.position = this.circuit.GetRoutePoint((float) ((double) this.progressDistance + (double) this.lookAheadForTargetOffset + (double) this.lookAheadForTargetFactor * (double) this.speed)).position;
        this.target.rotation = Quaternion.LookRotation(this.circuit.GetRoutePoint((float) ((double) this.progressDistance + (double) this.lookAheadForSpeedOffset + (double) this.lookAheadForSpeedFactor * (double) this.speed)).direction);
        this.progressPoint = this.circuit.GetRoutePoint(this.progressDistance);
        Vector3 lhs = this.progressPoint.position - this.transform.position;
        if ((double) Vector3.Dot(lhs, this.progressPoint.direction) < 0.0)
          this.progressDistance += lhs.magnitude * 0.5f;
        this.lastPosition = this.transform.position;
      }
      else
      {
        if ((double) (this.target.position - this.transform.position).magnitude < (double) this.pointToPointThreshold)
          this.progressNum = (this.progressNum + 1) % this.circuit.Waypoints.Length;
        this.target.position = this.circuit.Waypoints[this.progressNum].position;
        this.target.rotation = this.circuit.Waypoints[this.progressNum].rotation;
        this.progressPoint = this.circuit.GetRoutePoint(this.progressDistance);
        Vector3 lhs = this.progressPoint.position - this.transform.position;
        if ((double) Vector3.Dot(lhs, this.progressPoint.direction) < 0.0)
          this.progressDistance += lhs.magnitude;
        this.lastPosition = this.transform.position;
      }
    }

    private void OnDrawGizmos()
    {
      if (!Application.isPlaying)
        return;
      Gizmos.color = Color.green;
      Gizmos.DrawLine(this.transform.position, this.target.position);
      Gizmos.DrawWireSphere(this.circuit.GetRoutePosition(this.progressDistance), 1f);
      Gizmos.color = Color.yellow;
      Gizmos.DrawLine(this.target.position, this.target.position + this.target.forward);
    }

    public enum ProgressStyle
    {
      SmoothAlongRoute,
      PointToPoint,
    }
  }
}
