using UnityEngine;
using System;

namespace UnityEngine.Recorder
{
	public class RecorderBindings : MonoBehaviour
	{
		[Serializable]
		private class PropertyObjects : SerializedDictionary<string, Object>
		{
		}

		[SerializeField]
		private PropertyObjects m_References;
	}
}
