// Decompiled with JetBrains decompiler
// Type: PhysicsDebugger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using PhysicsDebug;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[AddComponentMenu("Physics/Physics Debugger")]
public class PhysicsDebugger : MonoBehaviour
{
  [Header("Line Settings")]
  [Tooltip("Time in seconds before all debug lines get destroyed.")]
  public float debugDuration = 10f;
  [Tooltip("The width of all debug lines.")]
  public float lineWidth = 0.05f;
  [Tooltip("The length of collision normals will be multiplied by this number when drawn.")]
  public float lineLengthMultiplier = 1f;
  [Tooltip("If true, debug lines for this object's collision normals will be drawn in world space.")]
  public bool drawWorldCollisionNormals = true;
  [Tooltip("If true, debug lines for this object's collision normals will be drawn in local space.")]
  public bool drawLocalCollisionNormals;
  [Tooltip("If true, debug lines will be scaled by impact force, i.e. the faster the object collides, the longer the line.")]
  public bool scaleLinesByForce = true;
  [Tooltip("If true, all debug lines will be colored based on the objects velocity in a gradient between \"slowSpeedColor\" and \"fastSpeedColor\". slowSpeedColor will start at slowSpeedValue and fade to fastSpeedColor as the object's velocity approaches fastSpeedValue.")]
  public bool useVelocityDebugColor = true;
  [Header("Trail Settings")]
  [Tooltip("If true, this object will leave a trail. The trail's color can be mapped to the objects velocity using the \"useVelocityDebugColor\" option.")]
  public bool useTrail;
  [Tooltip("If true, the trail will be colored by velocity, otherwise use default color.")]
  public bool useVelocityTrailColor = true;
  [Tooltip("Time in seconds before the trail starts to disappear.")]
  public float trailDuration = 8f;
  [Header("Trajectory Settings")]
  [Tooltip("If true, a line displaying this objected probable trajectory will be drawn.")]
  public bool drawTrajectory;
  [Range(0.7f, 0.9999f)]
  [Tooltip("The accuracy of the trajectory. This controls the distance between steps in calculation.")]
  public float accuracy = 0.985f;
  [Tooltip("Limit on how many steps the trajectory can take before stopping.")]
  public int iterationLimit = 150;
  [Tooltip("Stop the trajectory where the line hits an object? Objects can be set to ignore this collision by putting them on the Ignore Raycast layer.")]
  public bool stopOnCollision = true;
  [Tooltip("If true, the trajectory line will be colored by velocity, otherwise use default color. This option is separate from \"useVelocityDebugColor\" because it's slower to draw the trajectory line with this option enabled, so keep that in mind.")]
  public bool useVelocityForColor;
  [Header("Colors/Mats")]
  [Tooltip("The lower speed bound for determining velocity debug color.")]
  public float slowSpeedValue;
  [Tooltip("The upper speed bound for determining velocity debug color.")]
  public float fastSpeedValue = 15f;
  [Tooltip("The lower bound color to use for debug lines when using the \"useVelocityDebugColor\" option.")]
  public Color slowSpeedColor = new Color(0.2941177f, 1f, 0.2941177f);
  [Tooltip("The upper bound color to use for debug lines when using the \"useVelocityDebugColor\" option.")]
  public Color fastSpeedColor = new Color(1f, 0.2941177f, 0.2941177f);
  [Tooltip("The color to use for debug lines when not using the \"useVelocityDebugColor\" option.")]
  public Color defaultColor = new Color(0.8235294f, 0.2745098f, 1f);
  [Tooltip("Material to use for the debug line renderer. This is best left on Sprites/Default, but a custom one can be added if desired.")]
  public Material debugLineMaterial;
  [HideInInspector]
  public Color velociColor;
  private Color lastVColor = Color.black;
  private int collisionCount;
  private Rigidbody rb;
  private Vector3 lastPos = Vector3.zero;
  private Transform lineParent;
  private List<Vector3> predictionPoints = new List<Vector3>();
  private Vector3 vel = Vector3.zero;
  private Vector3 pos = Vector3.zero;
  private bool twoDim;
  private GameObject trajectoryLineObj;
  private float trajectoryDuration = 0.05f;

  private void Reset() => this.debugLineMaterial = new Material(Shader.Find("Sprites/Default"));

  private void Start()
  {
    this.rb = this.GetComponent<Rigidbody>();
    this.lastVColor = this.slowSpeedColor;
    this.lastPos = this.transform.position;
    if ((bool) (Object) this.rb)
      return;
    Debug.Log((object) (this.gameObject.name + ": Physics Debugger does not work without a rigidBody!"));
  }

  private void LateUpdate()
  {
    if (!(bool) (Object) this.rb)
      return;
    float percentage = this.VelocityPercentage(this.rb.velocity.magnitude);
    this.lastVColor = this.velociColor;
    this.velociColor = this.GradientPercent(this.slowSpeedColor, this.fastSpeedColor, percentage);
    if ((double) percentage > 0.0)
    {
      if (this.useTrail)
      {
        if (this.useVelocityTrailColor)
          this.DebugTwoLine(this.lastPos, this.transform.position, this.lastVColor, this.velociColor);
        else
          this.DebugTwoLine(this.lastPos, this.transform.position, this.defaultColor, this.defaultColor);
      }
      if (this.drawTrajectory)
      {
        this.pos = this.transform.position;
        this.vel = this.rb.velocity;
        this.trajectoryDuration = Time.unscaledDeltaTime;
        this.PerformPrediction();
      }
    }
    this.lastPos = this.transform.position;
  }

  public Color GetVelocicolor(float inVel) => (bool) (Object) this.rb ? this.GradientPercent(this.slowSpeedColor, this.fastSpeedColor, this.VelocityPercentage(inVel)) : Color.clear;

  private void OnCollisionEnter(Collision collision)
  {
    ++this.collisionCount;
    float magnitude = collision.relativeVelocity.magnitude;
    if ((double) magnitude < (double) this.slowSpeedValue)
      return;
    foreach (ContactPoint contact in collision.contacts)
    {
      Vector3 normal1 = contact.normal;
      if (this.scaleLinesByForce)
        normal1 *= magnitude / 2f;
      Vector3 normal2 = normal1 * this.lineLengthMultiplier;
      if (this.drawWorldCollisionNormals)
        this.DebugLine(contact.point, normal2, this.defaultColor, magnitude);
      if (this.drawLocalCollisionNormals)
        this.DebugLine(contact.point, normal2, this.defaultColor, magnitude, false);
    }
  }

  private void DebugLine(Vector3 point, Vector3 normal, Color color, float force, bool world = true)
  {
    Vector3 position1 = point;
    Vector3 vector3 = point + normal;
    if (!world)
    {
      Vector3 position2 = point + -normal;
      position1 = this.transform.InverseTransformPoint(position1);
      vector3 = this.transform.InverseTransformPoint(position2);
    }
    if (!(bool) (Object) this.lineParent)
    {
      this.lineParent = this.transform.Find(this.transform.name + "_Lines");
      if (!(bool) (Object) this.lineParent)
      {
        this.lineParent = new GameObject(this.transform.name + "_Lines").transform;
        this.lineParent.SetParent(this.transform);
        this.lineParent.localPosition = Vector3.zero;
        this.lineParent.localScale = Vector3.one;
        this.lineParent.localEulerAngles = Vector3.zero;
      }
    }
    GameObject gameObject = new GameObject(this.transform.name + "_DebugLine");
    gameObject.transform.SetParent(this.lineParent);
    gameObject.transform.localPosition = Vector3.zero;
    gameObject.transform.localScale = Vector3.one;
    gameObject.transform.localEulerAngles = Vector3.zero;
    LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
    lineRenderer.useWorldSpace = world;
    lineRenderer.receiveShadows = false;
    lineRenderer.sharedMaterial = this.debugLineMaterial;
    lineRenderer.startWidth = this.lineWidth;
    lineRenderer.endWidth = this.lineWidth;
    lineRenderer.shadowCastingMode = ShadowCastingMode.Off;
    if (this.useVelocityDebugColor)
    {
      Color color1 = this.GradientPercent(this.slowSpeedColor, this.fastSpeedColor, this.VelocityPercentage(force));
      lineRenderer.startColor = color1;
      lineRenderer.endColor = color1;
    }
    else
    {
      lineRenderer.startColor = color;
      lineRenderer.endColor = color;
    }
    lineRenderer.SetPositions(new Vector3[2]
    {
      position1,
      vector3
    });
    DelayDestroy delayDestroy = gameObject.AddComponent<DelayDestroy>();
    delayDestroy.delay = this.debugDuration;
    delayDestroy.StartUp();
  }

  private void DebugTwoLine(Vector3 startPoint, Vector3 endPoint, Color sColor, Color eColor)
  {
    if (!(bool) (Object) this.lineParent)
    {
      this.lineParent = this.transform.Find(this.transform.name + "_Lines");
      if (!(bool) (Object) this.lineParent)
      {
        this.lineParent = new GameObject(this.transform.name + "_Lines").transform;
        this.lineParent.SetParent(this.transform);
        this.lineParent.localPosition = Vector3.zero;
        this.lineParent.localScale = Vector3.one;
        this.lineParent.localEulerAngles = Vector3.zero;
      }
    }
    GameObject gameObject = new GameObject(this.transform.name + "_DebugLine");
    gameObject.transform.SetParent(this.lineParent);
    gameObject.transform.localPosition = Vector3.zero;
    gameObject.transform.localScale = Vector3.one;
    gameObject.transform.localEulerAngles = Vector3.zero;
    LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
    lineRenderer.useWorldSpace = true;
    lineRenderer.receiveShadows = false;
    lineRenderer.sharedMaterial = this.debugLineMaterial;
    lineRenderer.startWidth = this.lineWidth;
    lineRenderer.endWidth = this.lineWidth;
    lineRenderer.shadowCastingMode = ShadowCastingMode.Off;
    lineRenderer.startColor = sColor;
    lineRenderer.endColor = eColor;
    lineRenderer.SetPositions(new Vector3[2]
    {
      startPoint,
      endPoint
    });
    DelayDestroy delayDestroy = gameObject.AddComponent<DelayDestroy>();
    delayDestroy.delay = this.trailDuration;
    delayDestroy.StartUp();
  }

  public float VelocityPercentage(float force)
  {
    float num = this.fastSpeedValue - this.slowSpeedValue;
    return Mathf.Clamp01((force - this.slowSpeedValue) / num);
  }

  public Color GradientPercent(Color lower, Color upper, float percentage) => new Color(lower.r + percentage * (upper.r - lower.r), lower.g + percentage * (upper.g - lower.g), lower.b + percentage * (upper.b - lower.b));

  private float RangePercent(float min, float max, float percentage) => Mathf.Clamp(min + percentage * (max - min), min, max);

  private void PerformPrediction()
  {
    Vector3 zero = Vector3.zero;
    bool flag = false;
    int num1 = 0;
    float num2 = 1f - this.accuracy;
    Vector3 vector3 = Physics.gravity * num2;
    float num3 = Mathf.Clamp01((float) (1.0 - (double) this.rb.drag * (double) num2));
    this.predictionPoints.Clear();
    for (; !flag && num1 < this.iterationLimit; ++num1)
    {
      this.vel += vector3;
      this.vel *= num3;
      Vector3 b = this.pos + this.vel * num2;
      Vector3 direction = b - this.pos;
      this.predictionPoints.Add(this.pos);
      float num4 = Vector3.Distance(this.pos, b);
      if (this.stopOnCollision)
      {
        if (this.twoDim)
        {
          RaycastHit2D raycastHit2D = Physics2D.Raycast((Vector2) this.pos, (Vector2) direction, num4);
          if ((bool) raycastHit2D && (bool) (Object) raycastHit2D.collider.transform && (Object) raycastHit2D.collider.transform != (Object) this.transform)
          {
            flag = true;
            this.predictionPoints.Add((Vector3) raycastHit2D.point);
          }
        }
        else
        {
          UnityEngine.RaycastHit hitInfo;
          if (Physics.Raycast(new Ray(this.pos, direction), out hitInfo, num4))
          {
            flag = true;
            this.predictionPoints.Add(hitInfo.point);
          }
        }
      }
      this.pos = b;
    }
    if (this.useVelocityForColor)
      this.LineDebugColored(this.predictionPoints);
    else
      this.LineDebug(this.predictionPoints);
  }

  private void LineDebug(List<Vector3> pointList)
  {
    this.StopAllCoroutines();
    if ((bool) (Object) this.trajectoryLineObj)
      Object.Destroy((Object) this.trajectoryLineObj);
    this.trajectoryLineObj = new GameObject();
    this.trajectoryLineObj.name = "Trajectory Line";
    this.trajectoryLineObj.transform.SetParent(this.transform);
    LineRenderer lineRenderer = this.trajectoryLineObj.AddComponent<LineRenderer>();
    lineRenderer.startColor = this.defaultColor;
    lineRenderer.endColor = this.defaultColor;
    lineRenderer.startWidth = this.lineWidth;
    lineRenderer.endWidth = this.lineWidth;
    lineRenderer.useWorldSpace = true;
    lineRenderer.receiveShadows = false;
    lineRenderer.shadowCastingMode = ShadowCastingMode.Off;
    Shader shader = Shader.Find("Sprites/Default");
    if ((bool) (Object) shader)
      lineRenderer.sharedMaterial = new Material(shader);
    lineRenderer.positionCount = pointList.Count;
    lineRenderer.SetPositions(pointList.ToArray());
    this.StartCoroutine(this.KillTrajectoryDelay(this.trajectoryDuration));
  }

  private void LineDebugColored(List<Vector3> pointList)
  {
    if (pointList.Count < 2)
      return;
    this.StopAllCoroutines();
    if ((bool) (Object) this.trajectoryLineObj)
      Object.Destroy((Object) this.trajectoryLineObj);
    this.trajectoryLineObj = new GameObject();
    this.trajectoryLineObj.name = "Trajectory Line";
    this.trajectoryLineObj.transform.SetParent(this.transform);
    Color color = this.slowSpeedColor;
    Vector3 position = pointList[0];
    float num = 1f - this.accuracy;
    for (int index = 1; index < pointList.Count; ++index)
    {
      GameObject gameObject = new GameObject();
      gameObject.name = "Trajectory segment";
      gameObject.transform.SetParent(this.trajectoryLineObj.transform);
      Vector3 point = pointList[index];
      Color velocicolor = this.GetVelocicolor((point - position).magnitude / num);
      LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
      lineRenderer.startColor = color;
      lineRenderer.endColor = velocicolor;
      lineRenderer.startWidth = this.lineWidth;
      lineRenderer.endWidth = this.lineWidth;
      lineRenderer.useWorldSpace = true;
      lineRenderer.receiveShadows = false;
      lineRenderer.shadowCastingMode = ShadowCastingMode.Off;
      Shader shader = Shader.Find("Sprites/Default");
      if ((bool) (Object) shader)
        lineRenderer.sharedMaterial = new Material(shader);
      lineRenderer.positionCount = 2;
      lineRenderer.SetPosition(0, position);
      lineRenderer.SetPosition(1, point);
      position = point;
      color = velocicolor;
    }
    this.StartCoroutine(this.KillTrajectoryDelay(this.trajectoryDuration));
  }

  private IEnumerator KillTrajectoryDelay(float delay)
  {
    yield return (object) new WaitForSeconds(delay);
    if ((bool) (Object) this.trajectoryLineObj)
      Object.Destroy((Object) this.trajectoryLineObj);
  }

  private void OnDestroy()
  {
    if (!(bool) (Object) this.trajectoryLineObj)
      return;
    if ((bool) (Object) this.trajectoryLineObj)
      Object.Destroy((Object) this.trajectoryLineObj);
    if ((bool) (Object) this.lineParent)
      Object.Destroy((Object) this.lineParent);
    Resources.UnloadUnusedAssets();
  }
}
