// Decompiled with JetBrains decompiler
// Type: Sound
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

[Serializable]
public class Sound
{
  [SerializeField]
  public string name;
  [SerializeField]
  private AudioClip clip;
  [SerializeField]
  private float volume;
  private float initial_volume;
  [SerializeField]
  private float pitch;
  [SerializeField]
  private float spatialBlend;
  [SerializeField]
  private bool loop;
  public Transform sourceLocation;
  [HideInInspector]
  private AudioSource source;
  public bool started;

  public AudioClip _clip
  {
    get => !((UnityEngine.Object) this.source == (UnityEngine.Object) null) ? this.source.clip : this.clip;
    set
    {
      if ((UnityEngine.Object) this.source == (UnityEngine.Object) null)
        this.clip = value;
      else
        this.source.clip = value;
    }
  }

  public float _volume
  {
    get => (UnityEngine.Object) this.source != (UnityEngine.Object) null ? this.source.volume : this.volume;
    set
    {
      if ((bool) (UnityEngine.Object) this.source)
        this.source.volume = value;
      else
        this.volume = value;
    }
  }

  public float get_init_volume() => this.initial_volume;

  public float _pitch
  {
    get => !((UnityEngine.Object) this.source == (UnityEngine.Object) null) ? this.source.pitch : this.pitch;
    set
    {
      if ((UnityEngine.Object) this.source == (UnityEngine.Object) null)
        this.pitch = value;
      else
        this.source.pitch = value;
    }
  }

  public float _spatialBlend
  {
    get => !((UnityEngine.Object) this.source == (UnityEngine.Object) null) ? this.source.spatialBlend : this.spatialBlend;
    set
    {
      if ((UnityEngine.Object) this.source == (UnityEngine.Object) null)
        this.spatialBlend = value;
      else
        this.source.spatialBlend = value;
    }
  }

  public bool _loop
  {
    get => !((UnityEngine.Object) this.source == (UnityEngine.Object) null) ? this.source.loop : this.loop;
    set
    {
      if ((UnityEngine.Object) this.source == (UnityEngine.Object) null)
        this.loop = value;
      else
        this.source.loop = value;
    }
  }

  public bool isPlaying => this.source.isPlaying;

  public float time
  {
    get => this.source.time;
    set => this.source.time = value;
  }

  public void Play() => this.source.Play();

  public void Stop() => this.source.Stop();

  public Sound Clone() => new Sound()
  {
    name = this.name,
    clip = this.clip,
    volume = this.volume,
    pitch = this.pitch,
    loop = this.loop,
    source = this.source,
    spatialBlend = this.spatialBlend
  };

  public void Init(AudioManager myManager)
  {
    if ((UnityEngine.Object) this.sourceLocation == (UnityEngine.Object) null)
      this.sourceLocation = myManager.transform;
    this.source = this.sourceLocation.gameObject.AddComponent<AudioSource>();
    this.source.playOnAwake = false;
    this._clip = this.clip;
    this._volume = this.volume;
    this.initial_volume = this.volume;
    this._pitch = this.pitch;
    this._loop = this.loop;
    this._spatialBlend = this.spatialBlend;
    this.source.minDistance = GLOBALS.SOUND_MIN_DISTANCE;
    this.source.maxDistance = GLOBALS.SOUND_MAX_DISTANCE;
    this.source.rolloffMode = AudioRolloffMode.Linear;
  }
}
