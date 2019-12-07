using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using UnityEngine;
using Warden;

namespace Generic
{
	public static class CustomXmlReader
	{
		private static XmlReaderSettings settings = new XmlReaderSettings()
		{
			IgnoreComments = true,
			IgnoreWhitespace = true
		};
		//private static DirectoryInfo directory = new DirectoryInfo(Path.Combine(Application.dataPath, "XML" + Path.DirectorySeparatorChar));
		private static string currentPack;

		/// <summary>
		/// Reads all XML files in directory, and adds the entries to the database.
		/// </summary>
		/// <param name="directory"></param>
		public static void ReadXml(DirectoryInfo directory, ContentPack pack)
		{
			currentPack = pack.Name;
			FileInfo[] files = directory.GetFiles("*.xml", SearchOption.AllDirectories);
			foreach (FileInfo file in files)
			{

				Debug.Log("Reading " + file.Name + " in:\n" + pack.RootDir);
				XmlReader reader = XmlReader.Create(file.FullName, settings);
				XmlDocument document = new XmlDocument();
				try
				{
					document.Load(reader);
				}
				catch (Exception e)
				{
					Debug.LogError("Failed to load XML: " + e);
					break;
				}
				finally
				{
					reader.Close();
				}

				XmlNode root = document.DocumentElement;
				switch (root.Name)
				{
					case "MoveData":
						ReadMoveData(root);
						break;
					case "BeastData":
						ReadBeastData(root);
						break;
					case "AbilityData": //Probably not 'ability' -- 'aura' or 'skill' or 'passive'
						break;
				}
			}
		}

		private static void ReadMoveData(XmlNode root)
		{
			MoveData data = new MoveData
			{
				dataName = currentPack + "." + GetContent(root, "dataName"),
				Name = GetContent(root, "Name"),
				Essence = GetValidContent(root, "Essence", "Essence"),
				Focus = GetValidContent(root, "Focus", "Focus"),
				Stat = GetValidContent(root, "Stat", "Stat"),
				//MoveType = currentPack + "." + GetContent(root, "MoveType", "MoveType"),
				MoveProps = root["MoveProps"]
			};

			string moveType = GetContent(root, "MoveType", "MoveType");

			if (Type.GetType(Constants.CorePackName + "." + moveType) != null)
				data.MoveType = Constants.CorePackName + "." + moveType;
			else if (Type.GetType(currentPack + "." + moveType) != null)
				data.MoveType = currentPack + "." + moveType;
			else
				Debug.LogWarningFormat("MoveType given for {0} does not exist in pack namespace.", data.dataName);

			Database<MoveData>.Add(data);
		}

		private static void MakeMove(MoveData data)
		{
			MoveType move = (MoveType)Activator.CreateInstance(Type.GetType(data.MoveType), data);
			move.OnUse();
		}

		private static void ReadBeastData(XmlNode root)
		{
			BeastData data = new BeastData
			{
				dataName = currentPack + "." + GetContent(root, "dataName"),
				Name = GetContent(root, "Name"),
				Description = GetContent(root, "Description"),
			};

			//Essences
			data.Essences = new List<string>();
			foreach (XmlNode node in root["Essences"].ChildNodes)
				data.Essences.Add(Constants.GetValid("Essence_" + node.InnerXml));

			//Stats
			data.Stats = new Dictionary<string, Stat>();
			foreach (XmlNode node in root["Stats"].ChildNodes)
			{
				try
				{
					Stat temp = new Stat(float.Parse(GetContent(node, "Base")), GetContent(node, "Grade", "Grade"));
					data.Stats.Add("Stat_" + node.Name, temp);
				}
				catch (ArgumentException e)
				{
					Debug.LogWarningFormat("Stats in {0} contains duplicate entry.\n" + e, data.dataName);
				}
			}

			//LearnSet
			data.LearnSet = new SortedDictionary<int, string>();
			foreach (XmlNode node in root["LearnSet"].ChildNodes)
			{
				try
				{
					data.LearnSet.Add(int.Parse(GetContent(node, "level")), currentPack + "." + GetContent(node, "dataName"));
				}
				catch (ArgumentException e)
				{
					Debug.LogWarningFormat("Tried to add duplicate entry {0} into {1}'s learnset.\n" + e, GetContent(node, "dataName"), data.dataName);
				}
			}

			Database<BeastData>.Add(data);
		}

		public static string GetContent(XmlNode node, string key)
		{
			return node[key].InnerXml;
		}

		public static string GetContent(XmlNode node, string key, string prefix)
		{
			prefix += "_";
			return prefix + GetContent(node, key);
		}

		public static string GetValidContent(XmlNode node, string key, string prefix)
		{
			return Constants.GetValid(GetContent(node, key, prefix));
		}
	}
}
