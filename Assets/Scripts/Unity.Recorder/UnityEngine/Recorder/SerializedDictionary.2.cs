using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.Recorder
{
	[Serializable]
	internal class SerializedDictionary<TKey, TValue>
	{
		[SerializeField]
		private List<TKey> m_Keys;
		[SerializeField]
		private List<TValue> m_Values;
	}
}
