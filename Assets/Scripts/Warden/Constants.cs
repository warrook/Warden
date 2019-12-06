using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warden
{
	public static class Constants
	{
		public static readonly string CorePackName = typeof(Beast).Namespace;

		public static readonly List<string> Stat = new List<string>()
		{
			"Stat_Power",
			"Stat_Defense",
			"Stat_Vitality"
		};

		public static readonly List<string> Essence = new List<string>()
		{
			"Essence_Body",
			"Essence_Spirit",
			"Essence_Death"
		};

		public static readonly List<string> Focus = new List<string>()
		{
			"Focus_Self",
			"Focus_Enemy",
			"Focus_Ally",
			"Focus_AllEnemies",
			"Focus_AllAllies"
		};

		public static readonly Dictionary<string, float> ScaleFactor = new Dictionary<string, float>
		{
			{ "Grade_A", 1.025f },
			{ "Grade_B", 1.02f },
			{ "Grade_C", 1.015f },
			{ "Grade_D", 1.01f },
			{ "Grade_E", 1.005f },
			{ "Grade_F", 1.0f }
		};

		public static bool Validate(string value)
		{
			List<string> collection = GetCollection(value);

			if (collection == null)
				return false;
			return collection.Contains(value);
		}

		/// <summary>
		/// Checks that a given string is in Generic.Constants.
		/// </summary>
		/// <param name="value"></param>
		/// <returns>Given value, or if invalid, attempts to return substitution.</returns>
		public static string GetValid(string value)
		{
			if (Validate(value))
				return value;
			string s = string.Format("Failed to validate string {0} against constants.", value);
			try
			{
				string sub = GetCollection(value).First();
				s = string.Format(s + " Substituting it with {0}", sub);
				UnityEngine.Debug.LogWarning(s);
				return GetCollection(value).First();
			}
			catch
			{
				string sub = value.Substring(0, value.IndexOf('_'));
				s = string.Format(s + " Collection {0} does not exist", sub);
				UnityEngine.Debug.LogWarning(s);
				return null;
			}
		}

		public static List<string> GetCollection(string value) 
		{
			string collection = value.Substring(0, value.IndexOf('_'));

			switch (collection)
			{
				case "Stat":
					return Stat;
				case "Essence":
					return Essence;
				case "Focus":
					return Focus;
				case "Grade":
					return ScaleFactor.Keys.ToList();
			}
			return null;
		}
	}
}
