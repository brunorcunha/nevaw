using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class DiscardSpellEffectDefinition : EffectExecutionDefinition
	{
		private bool m_randomly;

		private DynamicValue m_count;

		private List<SpellFilter> m_spellFilters;

		public bool randomly => m_randomly;

		public DynamicValue count => m_count;

		public IReadOnlyList<SpellFilter> spellFilters => m_spellFilters;

		public override string ToString()
		{
			return string.Format("{0} discards {1} spells{2}", m_executionTargetSelector, count, (m_condition == null) ? "" : $" if {m_condition}");
		}

		public new static DiscardSpellEffectDefinition FromJsonToken(JToken token)
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
			DiscardSpellEffectDefinition discardSpellEffectDefinition = new DiscardSpellEffectDefinition();
			discardSpellEffectDefinition.PopulateFromJson(jsonObject);
			return discardSpellEffectDefinition;
		}

		public static DiscardSpellEffectDefinition FromJsonProperty(JObject jsonObject, string propertyName, DiscardSpellEffectDefinition defaultValue = null)
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

		public override void PopulateFromJson(JObject jsonObject)
		{
			base.PopulateFromJson(jsonObject);
			m_randomly = Serialization.JsonTokenValue<bool>(jsonObject, "randomly", true);
			m_count = DynamicValue.FromJsonProperty(jsonObject, "count");
			JArray val = Serialization.JsonArray(jsonObject, "spellFilters");
			m_spellFilters = new List<SpellFilter>((val != null) ? val.get_Count() : 0);
			if (val != null)
			{
				foreach (JToken item in val)
				{
					m_spellFilters.Add(SpellFilter.FromJsonToken(item));
				}
			}
		}
	}
}
