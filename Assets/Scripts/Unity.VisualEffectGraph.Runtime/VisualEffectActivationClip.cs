using System;
using UnityEngine.Playables;
using UnityEngine;

[Serializable]
public class VisualEffectActivationClip : PlayableAsset
{
	public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
	{
		return default(Playable);
	}

	public VisualEffectActivationBehaviour activationBehavior;
}
