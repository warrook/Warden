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
		private static ContentPack currentPack;

		/// <summary>
		/// Reads all XML files in directory, and adds the entries to the database.
		/// </summary>
		/// <param name="directory"></param>
		public static void ReadXml(DirectoryInfo directory, ContentPack pack)
		{
			currentPack = pack;
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

				//TODO: Need to allow for multiple things in one file
				//Escape apostrophes and quotes
				if (document.DocumentElement.Name != "Data")
					continue;

				
				foreach (XmlNode node in document.DocumentElement.ChildNodes)
				{
					Debug.LogFormat("Reading {2} ['{0}'] in file {1}", node["dataName"].InnerText, file.Name, node.Name);

					switch (node.Name)
					{
						case "MoveData":
							ReadMoveData(node);
							break;
						case "BeastData":
							ReadBeastData(node);
							break;
						case "AbilityData": //Probably not 'ability' -- 'aura' or 'skill' or 'passive'
							break;
					}
				}
			}
		}

		private static void ReadMoveData(XmlNode root)
		{
			MoveData data = new MoveData
			{
				dataName = currentPack.Name + "." + GetContent(root, "dataName"),
				Name = GetContent(root, "Name"),
				Essence = GetValidContent(root, "Essence", "Essence"),
				Focus = GetValidContent(root, "Focus", "Focus"),
				Stat = GetValidContent(root, "Stat", "Stat"),
				MoveType = GetValidClass(GetContent(root, "MoveType", "MoveType")),
				MoveProps = root["MoveProps"]
			};
			
			Database<MoveData>.Add(data);
		}

		//private static void MakeMove(MoveData data)
		//{
		//	MoveType move = (MoveType)Activator.CreateInstance(Type.GetType(data.MoveType), data);
		//	move.OnUse();
		//}

		private static void ReadBeastData(XmlNode root)
		{
			BeastData data = new BeastData
			{
				dataName = currentPack.Name + "." + GetContent(root, "dataName"),
				Name = GetContent(root, "Name"),
				Description = GetContent(root, "Description"),
				ModelClass = GetValidClass(GetContent(root["Model"], "class", "BeastModel")),
				ModelProps = root["Model"]["modelProps"]
			};

			//Adjust texture path for model
			foreach (XmlNode node in data.ModelProps["textures"].ChildNodes)
				node.InnerText = Path.Combine(currentPack.TexturesFolder, "Beasts", node.InnerText + ".png");

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
			//TODO: allow support for inheriting from previous xmute stages (which is a WHOLE OTHER BEAST)
			data.LearnSet = new SortedDictionary<int, string>();
			foreach (XmlNode node in root["LearnSet"].ChildNodes)
			{
				try
				{
					//TODO: Allow specifying in the XML what namespace to use
					data.LearnSet.Add(int.Parse(GetContent(node, "level")), currentPack.Name + "." + GetContent(node, "dataName"));
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

		private static string FormatClassName(string ns, string cl)
		{
			return ns + "." + cl;
		}

		/// <summary>
		/// Verify that the class exists in the given namespace
		/// </summary>
		/// <param name="ns"></param>
		/// <param name="cl"></param>
		/// <returns></returns>
		private static Type GetClassFromName(string ns, string cl)
		{
			return GetClassFromString(FormatClassName(ns, cl));
		}

		private static Type GetClassFromString(string str)
		{
			return Type.GetType(str);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="cl"></param>
		/// <returns></returns>
		public static string GetValidClass(string cl)
		{
			Type t;

			if ((t = GetClassFromName(currentPack.Name, cl)) != null)
				return t.FullName;
			else if ((t = GetClassFromName(Constants.CorePackName, cl)) != null)
				return t.FullName;

			Debug.LogWarningFormat("{0} does not exist within '{1}' namespace.", cl, currentPack.Name);
			return null;
		}
	}
}
