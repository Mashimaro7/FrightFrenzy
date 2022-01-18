// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets.Utility.WaypointCircuit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

namespace UnityStandardAssets.Utility
{
  public class WaypointCircuit : MonoBehaviour
  {
    public WaypointCircuit.WaypointList waypointList = new WaypointCircuit.WaypointList();
    [SerializeField]
    private bool smoothRoute = true;
    private int numPoints;
    private Vector3[] points;
    private float[] distances;
    public float editorVisualisationSubsteps = 100f;
    private int p0n;
    private int p1n;
    private int p2n;
    private int p3n;
    private float i;
    private Vector3 P0;
    private Vector3 P1;
    private Vector3 P2;
    private Vector3 P3;

    public float Length { get; private set; }

    public Transform[] Waypoints => this.waypointList.items;

    private void Awake()
    {
      if (this.Waypoints.Length > 1)
        this.CachePositionsAndDistances();
      this.numPoints = this.Waypoints.Length;
    }

    public WaypointCircuit.RoutePoint GetRoutePoint(float dist)
    {
      Vector3 routePosition = this.GetRoutePosition(dist);
      Vector3 vector3 = this.GetRoutePosition(dist + 0.1f) - routePosition;
      return new WaypointCircuit.RoutePoint(routePosition, vector3.normalized);
    }

    public Vector3 GetRoutePosition(float dist)
    {
      int index = 0;
      if ((double) this.Length == 0.0)
        this.Length = this.distances[this.distances.Length - 1];
      dist = Mathf.Repeat(dist, this.Length);
      while ((double) this.distances[index] < (double) dist)
        ++index;
      this.p1n = (index - 1 + this.numPoints) % this.numPoints;
      this.p2n = index;
      this.i = Mathf.InverseLerp(this.distances[this.p1n], this.distances[this.p2n], dist);
      if (this.smoothRoute)
      {
        this.p0n = (index - 2 + this.numPoints) % this.numPoints;
        this.p3n = (index + 1) % this.numPoints;
        this.p2n %= this.numPoints;
        this.P0 = this.points[this.p0n];
        this.P1 = this.points[this.p1n];
        this.P2 = this.points[this.p2n];
        this.P3 = this.points[this.p3n];
        return this.CatmullRom(this.P0, this.P1, this.P2, this.P3, this.i);
      }
      this.p1n = (index - 1 + this.numPoints) % this.numPoints;
      this.p2n = index;
      return Vector3.Lerp(this.points[this.p1n], this.points[this.p2n], this.i);
    }

    private Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float i) => 0.5f * (2f * p1 + (-p0 + p2) * i + (2f * p0 - 5f * p1 + 4f * p2 - p3) * i * i + (-p0 + 3f * p1 - 3f * p2 + p3) * i * i * i);

    private void CachePositionsAndDistances()
    {
      this.points = new Vector3[this.Waypoints.Length + 1];
      this.distances = new float[this.Waypoints.Length + 1];
      float num = 0.0f;
      for (int index = 0; index < this.points.Length; ++index)
      {
        Transform waypoint1 = this.Waypoints[index % this.Waypoints.Length];
        Transform waypoint2 = this.Waypoints[(index + 1) % this.Waypoints.Length];
        if ((UnityEngine.Object) waypoint1 != (UnityEngine.Object) null && (UnityEngine.Object) waypoint2 != (UnityEngine.Object) null)
        {
          Vector3 position1 = waypoint1.position;
          Vector3 position2 = waypoint2.position;
          this.points[index] = this.Waypoints[index % this.Waypoints.Length].position;
          this.distances[index] = num;
          num += (position1 - position2).magnitude;
        }
      }
    }

    private void OnDrawGizmos() => this.DrawGizmos(false);

    private void OnDrawGizmosSelected() => this.DrawGizmos(true);

    private void DrawGizmos(bool selected)
    {
      this.waypointList.circuit = this;
      if (this.Waypoints.Length <= 1)
        return;
      this.numPoints = this.Waypoints.Length;
      this.CachePositionsAndDistances();
      this.Length = this.distances[this.distances.Length - 1];
      Gizmos.color = selected ? Color.yellow : new Color(1f, 1f, 0.0f, 0.5f);
      Vector3 from = this.Waypoints[0].position;
      if (this.smoothRoute)
      {
        for (float num = 0.0f; (double) num < (double) this.Length; num += this.Length / this.editorVisualisationSubsteps)
        {
          Vector3 routePosition = this.GetRoutePosition(num + 1f);
          Gizmos.DrawLine(from, routePosition);
          from = routePosition;
        }
        Gizmos.DrawLine(from, this.Waypoints[0].position);
      }
      else
      {
        for (int index = 0; index < this.Waypoints.Length; ++index)
        {
          Vector3 position = this.Waypoints[(index + 1) % this.Waypoints.Length].position;
          Gizmos.DrawLine(from, position);
          from = position;
        }
      }
    }

    [Serializable]
    public class WaypointList
    {
      public WaypointCircuit circuit;
      public Transform[] items = new Transform[0];
    }

    public struct RoutePoint
    {
      public Vector3 position;
      public Vector3 direction;

      public RoutePoint(Vector3 position, Vector3 direction)
      {
        this.position = position;
        this.direction = direction;
      }
    }
  }
}
