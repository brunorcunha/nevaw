using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class NamedEffectDefinition : IEditableContent
	{
		private string m_name;

		private List<EffectDefinition> m_effects;

		public string name => m_name;

		public IReadOnlyList<EffectDefinition> effects => m_effects;

		public override string ToString()
		{
			return $"{m_name} ({m_effects.Count} effets)";
		}

		public static NamedEffectDefinition FromJsonToken(JToken token)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			if ((int)token.get_Type() != 1)
			{
				Debug.LogWarning((object)("Malformed token : type Object expected, but " + token.get_Type() + " found"));
				return null;
			}
			JObject jsonObject = Extensions.Value<JObject>((IEnumerable<JToken>)token);
			NamedEffectDefinition namedEffectDefinition = new NamedEffectDefinition();
			namedEffectDefinition.PopulateFromJson(jsonObject);
			return namedEffectDefinition;
		}

		public static NamedEffectDefinition FromJsonProperty(JObject jsonObject, string propertyName, NamedEffectDefinition defaultValue = null)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Invalid comparison between Unknown and I4
			JProperty val = jsonObject.Property(propertyName);
			if (val == null || (int)val.get_Value().get_Type() == 10)
			{
				return defaultValue;
			}
			return FromJsonToken(val.get_Value());
		}

		public void PopulateFromJson(JObject jsonObject)
		{
			m_name = Serialization.JsonTokenValue<string>(jsonObject, "name", (string)null);
			JArray val = Serialization.JsonArray(jsonObject, "effects");
			m_effects = new List<EffectDefinition>((val != null) ? val.get_Count() : 0);
			if (val != null)
			{
				foreach (JToken item in val)
				{
					m_effects.Add(EffectDefinition.FromJsonToken(item));
				}
			}
		}
	}
}
