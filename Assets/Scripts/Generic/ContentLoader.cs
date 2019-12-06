using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using UnityEngine;

namespace Generic
{
	/// <summary>
	/// Responsible for rolling through the Content directory and generating each pack there.
	/// </summary>
	public static class ContentLoader
	{
		private static DirectoryInfo[] directories => FileLocation.ContentDir.GetDirectories("*", SearchOption.TopDirectoryOnly);
		public static List<ContentPack> Packs = new List<ContentPack>();

		public static void Log()
		{
			Debug.Log(string.Join("\n", directories.ToList()));
		}

		public static void LoadPacks()
		{
			foreach (DirectoryInfo dir in directories)
			{
				Debug.LogFormat("Loading pack using directory \\{0}\\", dir.Name);
				Packs.Add(new ContentPack(dir));
			}
			Debug.LogWarningFormat("All entries in the database: ");
			Database<Warden.BeastData>.LogAll();
		}
	}
}
