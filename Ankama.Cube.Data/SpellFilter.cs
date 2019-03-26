using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public abstract class SpellFilter : IEditableContent
	{
		public override string ToString()
		{
			return GetType().Name;
		}

		public static SpellFilter FromJsonToken(JToken token)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			if ((int)token.get_Type() != 1)
			{
				Debug.LogWarning((object)("Malformed token : type Object expected, but " + token.get_Type() + " found"));
				return null;
			}
			JObject val = Extensions.Value<JObject>((IEnumerable<JToken>)token);
			JToken val2 = default(JToken);
			if (!val.TryGetValue("type", ref val2))
			{
				Debug.LogWarning((object)"Malformed json: no 'type' property in object of class SpellFilter");
				return null;
			}
			string text = Extensions.Value<string>((IEnumerable<JToken>)val2);
			SpellFilter spellFilter;
			switch (text)
			{
			case "SpellGiveReservePointFilter":
				spellFilter = new SpellGiveReservePointFilter();
				break;
			case "SpellElementsFilter":
				spellFilter = new SpellElementsFilter();
				break;
			case "SpellTagsFilter":
				spellFilter = new SpellTagsFilter();
				break;
			case "SpecificSpellFilter":
				spellFilter = new SpecificSpellFilter();
				break;
			default:
				Debug.LogWarning((object)("Unknown type: " + text));
				return null;
			}
			spellFilter.PopulateFromJson(val);
			return spellFilter;
		}

		public static SpellFilter FromJsonProperty(JObject jsonObject, string propertyName, SpellFilter defaultValue = null)
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

		public virtual void PopulateFromJson(JObject jsonObject)
		{
		}

		public abstract bool Accept(int spellInstanceId, SpellDefinition spellDef);
	}
}
