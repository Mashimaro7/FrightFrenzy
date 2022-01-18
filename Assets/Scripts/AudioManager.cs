// Decompiled with JetBrains decompiler
// Type: AudioManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
  public List<Sound> sounds;
  private Dictionary<string, Sound> sounds_indexed = new Dictionary<string, Sound>();
  public ArrayList allRamps = new ArrayList();
  private int id;

  private void Awake()
  {
    foreach (Sound sound in this.sounds)
    {
      this.sounds_indexed.Add(sound.name, sound);
      sound.Init(this);
    }
  }

  public void AddSound(Sound newsound)
  {
    if (newsound == null)
      return;
    this.sounds_indexed.Add(newsound.name, newsound);
    newsound.Init(this);
    this.sounds.Add(newsound);
  }

  public bool Play(
    string name,
    float crossFadeTime = 0.0f,
    float volume = -1f,
    bool server_request = false,
    float pitch = -1f)
  {
    Sound sound = this.findSound(name);
    if (sound == null)
      return false;
    sound.started = true;
    if (this.id == 0)
    {
      gameElement component1 = this.GetComponent<gameElement>();
      RobotID component2 = this.GetComponent<RobotID>();
      if ((bool) (Object) component1)
        this.id = component1.id;
      else if ((bool) (Object) component2)
        this.id = -1 * component2.id;
    }
    if (GLOBALS.SERVER_MODE && !server_request)
    {
      if (this.id == 0)
        return false;
      if ((double) volume < 0.0)
        volume = sound.get_init_volume();
      GLOBALS.topserver.playSound(this.id, name, crossFadeTime, volume, pitch);
    }
    return !server_request && GLOBALS.CLIENT_MODE || this.PlayCore(name, crossFadeTime, volume, pitch);
  }

  private bool PlayCore(string name, float crossFadeTime = 0.0f, float volume = -1f, float pitch = -1f)
  {
    if (GLOBALS.HEADLESS_MODE || !GLOBALS.AUDIO || !GLOBALS.ROBOTAUDIO && this.id < 0)
      return false;
    Sound sound = this.findSound(name);
    if (sound == null)
    {
      Debug.Log((object) ("Sound " + name + " not found!"));
      return false;
    }
    if ((double) volume < 0.0)
      volume = sound.get_init_volume();
    sound._volume = volume;
    if ((double) pitch > 0.0)
      sound._pitch = pitch;
    if (sound == null)
      return false;
    for (int index = this.allRamps.Count - 1; index >= 0; --index)
    {
      if (((AudioManager.Ramp) this.allRamps[index]).mySound.name == name)
        this.allRamps.Remove(this.allRamps[index]);
    }
    this.allRamps.Add((object) new AudioManager.Ramp(sound, Time.time, crossFadeTime, sound._volume, true));
    if ((double) crossFadeTime != 0.0)
      sound._volume = 0.0f;
    sound.Play();
    return true;
  }

  public bool Stop(string name, float crossFadeTime, bool server_request = false)
  {
    Sound sound = this.findSound(name);
    if (sound == null)
      return false;
    sound.started = false;
    foreach (AudioManager.Ramp ramp in this.allRamps.ToArray())
    {
      if (ramp.mySound.name == name)
      {
        if (!ramp.up)
          return false;
        if (this.allRamps.Contains((object) ramp))
        {
          ramp.mySound._volume = ramp.initialVolume;
          this.allRamps.Remove((object) ramp);
        }
      }
    }
    if (!server_request && GLOBALS.CLIENT_MODE)
      return true;
    if (GLOBALS.SERVER_MODE)
    {
      if (this.id == 0)
      {
        gameElement component1 = this.GetComponent<gameElement>();
        RobotID component2 = this.GetComponent<RobotID>();
        if ((bool) (Object) component1)
          this.id = component1.id;
        else if ((bool) (Object) component2)
          this.id = -1 * component2.id;
      }
      GLOBALS.topserver.stopSound(this.id, name, crossFadeTime);
    }
    this.allRamps.Add((object) new AudioManager.Ramp(sound, Time.time, crossFadeTime, sound._volume, false));
    return true;
  }

  public bool restartSoundCrossFade(string name, float crossFadeTime)
  {
    Sound sound = this.findSound(name);
    if (sound == null)
      return false;
    sound.time = 0.0f;
    return true;
  }

  private void Update()
  {
    for (int index = 0; index < this.allRamps.Count; ++index)
    {
      AudioManager.Ramp allRamp = (AudioManager.Ramp) this.allRamps[index];
      float num = Time.time - allRamp.timeStartedRamp;
      if (allRamp.up)
      {
        allRamp.mySound._volume = (double) allRamp.rampTime <= 0.0 ? allRamp.initialVolume : allRamp.initialVolume * (num / allRamp.rampTime);
        if ((double) allRamp.mySound._volume >= (double) allRamp.initialVolume)
        {
          allRamp.mySound._volume = allRamp.initialVolume;
          this.allRamps.RemoveAt(index);
          --index;
        }
      }
      else
      {
        allRamp.mySound._volume = (double) allRamp.rampTime <= 0.0 ? 0.0f : allRamp.initialVolume * (float) (1.0 - (double) num / (double) allRamp.rampTime);
        if ((double) allRamp.mySound._volume <= 0.0)
        {
          allRamp.mySound.Stop();
          this.allRamps.RemoveAt(index);
          allRamp.mySound._volume = allRamp.initialVolume;
          --index;
        }
      }
    }
  }

  public Sound findSound(string name) => this.sounds_indexed.ContainsKey(name) ? this.sounds_indexed[name] : (Sound) null;

  public bool SoundIsPlaying(string name)
  {
    foreach (AudioManager.Ramp allRamp in this.allRamps)
    {
      if (allRamp.mySound.name == name)
        return allRamp.up;
    }
    Sound sound = this.findSound(name);
    return sound != null && sound.isPlaying;
  }

  public bool IsSoundStarted(string name)
  {
    Sound sound = this.findSound(name);
    return sound != null && sound.started;
  }

  private struct Ramp
  {
    public Sound mySound;
    public float timeStartedRamp;
    public float rampTime;
    public float initialVolume;
    public bool up;

    public Ramp(Sound s, float startTime, float totalTime, float initialVolume, bool up)
    {
      this.mySound = s;
      this.timeStartedRamp = startTime;
      this.rampTime = totalTime;
      this.initialVolume = initialVolume;
      this.up = up;
    }
  }
}
