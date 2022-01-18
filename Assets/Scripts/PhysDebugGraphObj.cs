// Decompiled with JetBrains decompiler
// Type: PhysDebugGraphObj
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[AddComponentMenu("Physics/Physics Debug Graph Obj")]
public class PhysDebugGraphObj : MonoBehaviour
{
  [HideInInspector]
  public GameObject gO;
  [HideInInspector]
  public Rigidbody rb;
  [HideInInspector]
  public string name = "";
  [HideInInspector]
  public List<float> velocityGraph = new List<float>();
  [HideInInspector]
  public List<float> accelerationGraph = new List<float>();
  [HideInInspector]
  public List<Vector3> velocityAxisGraph = new List<Vector3>();
  [HideInInspector]
  public List<Vector3> gGraph = new List<Vector3>();
  [HideInInspector]
  public Vector3 lastVelocity = Vector3.zero;
  [HideInInspector]
  public int graphResolution = 100;
  [HideInInspector]
  public int dataRate = 60;
  private int frameCounter;
  [HideInInspector]
  public int collisionCount;
  [HideInInspector]
  public bool colliding;
  [HideInInspector]
  public string collidingObject = "";
  [HideInInspector]
  public float collisionForce;
  [HideInInspector]
  public float minCollisionForce = float.PositiveInfinity;
  [HideInInspector]
  public float maxCollisionForce;
  [HideInInspector]
  public Vector3 collisionVelocity = Vector3.zero;
  [HideInInspector]
  public float collisionDuration;
  [HideInInspector]
  public float noCollisionDuration;
  private float lastCollideTime;
  private float lastNoCollideTime;
  [HideInInspector]
  public int triggerCount;
  [HideInInspector]
  public bool triggering;
  [HideInInspector]
  public string triggeringObject = "";
  [HideInInspector]
  public float triggerDuration;
  private float lastTriggerTime;
  [HideInInspector]
  public float minSpeed = float.PositiveInfinity;
  [HideInInspector]
  public float maxSpeed;
  [HideInInspector]
  public Vector3 currentVelocity = Vector3.zero;
  [HideInInspector]
  public Vector3 currentAcceleration = Vector3.zero;
  [HideInInspector]
  public Vector3 minVelocity = Vector3.zero;
  [HideInInspector]
  public Vector3 maxVelocity = Vector3.zero;

  public void Initialize()
  {
    this.gO = this.gameObject;
    this.rb = this.gO.GetComponent<Rigidbody>();
    this.name = this.gO.name;
  }

  private void FixedUpdate()
  {
    this.colliding = false;
    this.triggering = false;
    ++this.frameCounter;
    if (this.frameCounter % (61 - this.dataRate) != 0)
      return;
    this.frameCounter = 0;
    this.getData();
  }

  private void OnCollisionEnter(Collision collisionInfo)
  {
    ++this.collisionCount;
    this.collisionForce = collisionInfo.relativeVelocity.magnitude;
    this.collisionVelocity = collisionInfo.relativeVelocity;
    this.collidingObject = collisionInfo.gameObject.name;
    this.lastCollideTime = Time.time;
    this.lastNoCollideTime = Time.time;
    if ((double) this.collisionForce < (double) this.minCollisionForce)
      this.minCollisionForce = this.collisionForce;
    if ((double) this.collisionForce <= (double) this.maxCollisionForce)
      return;
    this.maxCollisionForce = this.collisionForce;
  }

  private void OnCollisionStay(Collision collisionInfo)
  {
    this.colliding = true;
    this.collidingObject = collisionInfo.gameObject.name;
    this.collisionDuration = Time.time - this.lastCollideTime;
    this.lastNoCollideTime = Time.time;
  }

  private void OnCollisionExit(Collision collisionInfo)
  {
    this.collisionDuration = Time.time - this.lastCollideTime;
    this.lastNoCollideTime = Time.time;
  }

  private void OnTriggerEnter(Collider other)
  {
    ++this.triggerCount;
    this.triggeringObject = other.name;
    this.lastTriggerTime = Time.time;
  }

  private void OnTriggerStay(Collider other)
  {
    this.triggering = true;
    this.triggeringObject = other.name;
    this.triggerDuration = Time.time - this.lastTriggerTime;
  }

  private void OnTriggerExit(Collider other) => this.triggerDuration = Time.time - this.lastTriggerTime;

  public void getData()
  {
    if (!(bool) (Object) this.rb)
      return;
    this.currentVelocity = this.rb.velocity;
    this.currentAcceleration = this.lastVelocity - this.currentVelocity;
    if (this.accelerationGraph.Count > this.graphResolution)
      this.accelerationGraph.RemoveRange(0, this.accelerationGraph.Count - this.graphResolution);
    if (this.velocityGraph.Count > 0)
      this.accelerationGraph.Add(this.velocityGraph.Last<float>() - this.currentVelocity.magnitude);
    else
      this.accelerationGraph.Add(0.0f);
    if (this.velocityGraph.Count > this.graphResolution)
      this.velocityGraph.RemoveRange(0, this.velocityGraph.Count - this.graphResolution);
    this.velocityGraph.Add(this.currentVelocity.magnitude);
    if (this.velocityAxisGraph.Count > this.graphResolution)
      this.velocityAxisGraph.RemoveRange(0, this.velocityAxisGraph.Count - this.graphResolution);
    this.velocityAxisGraph.Add(this.currentVelocity);
    if (this.gGraph.Count > this.graphResolution)
      this.gGraph.RemoveRange(0, this.gGraph.Count - this.graphResolution);
    this.gGraph.Add(this.currentAcceleration);
    if ((double) this.currentVelocity.magnitude < (double) this.minSpeed)
      this.minSpeed = this.currentVelocity.magnitude;
    if ((double) this.currentVelocity.magnitude > (double) this.maxSpeed)
      this.maxSpeed = this.currentVelocity.magnitude;
    if ((double) this.currentVelocity.x < (double) this.minVelocity.x)
      this.minVelocity.x = this.currentVelocity.x;
    if ((double) this.currentVelocity.y < (double) this.minVelocity.y)
      this.minVelocity.y = this.currentVelocity.y;
    if ((double) this.currentVelocity.z < (double) this.minVelocity.z)
      this.minVelocity.z = this.currentVelocity.z;
    if ((double) this.currentVelocity.x > (double) this.maxVelocity.x)
      this.maxVelocity.x = this.currentVelocity.x;
    if ((double) this.currentVelocity.y > (double) this.maxVelocity.y)
      this.maxVelocity.y = this.currentVelocity.y;
    if ((double) this.currentVelocity.z > (double) this.maxVelocity.z)
      this.maxVelocity.z = this.currentVelocity.z;
    this.noCollisionDuration = Time.time - this.lastNoCollideTime;
    this.lastVelocity = this.currentVelocity;
  }
}
