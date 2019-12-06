using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using UnityEngine;

namespace Generic
{
	public class ContentPack
	{
		private string name; //Is this necessary when RootDir.Name works?
		private DirectoryInfo rootDir;
		private string dataFolder;
		private string assembliesFolder; //TODO: Load assemblies

		public string Name => name;
		public string RootDir => rootDir.FullName;
		public string DataFolder
		{
			get
			{
				if (dataFolder == null)
					dataFolder = Path.Combine(RootDir, "Data" + Path.DirectorySeparatorChar);
				return dataFolder;
			}
		}
		public string AssembliesFolder
		{
			get
			{
				if (assembliesFolder == null)
					assembliesFolder = Path.Combine(RootDir, "Assemblies" + Path.DirectorySeparatorChar);
				return assembliesFolder;
			}
		}

		public ContentPack(DirectoryInfo directory)
		{
			Debug.Log(directory);
			rootDir = directory;
			name = directory.Name;

			LoadData();
		}

		public void LoadData()
		{
			CustomXmlReader.ReadXml(new DirectoryInfo(DataFolder), this);
			//Database<Warden.MoveData>.LogAll();
			//Database<Warden.BeastData>.LogAll();
		}
	}
}
