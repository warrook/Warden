using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Generic
{
	public static class Database<T> where T : Data, new()
	{
		private static List<T> dataList = new List<T>();
		private static Dictionary<string, T> dataByName = new Dictionary<string, T>();

		public static void Add(T data)
		{
			Debug.LogFormat("Adding data {0} to database", data.dataName);
			try
			{
				dataList.Add(data);
				dataByName.Add(data.dataName, data);
			}
			catch (ArgumentException e)
			{
				Debug.LogWarningFormat("Tried to add duplicate entry {1} into {0} database.\n" + e, typeof(T), data.dataName);
			}
		}

		public static void LogAll()
		{
			Debug.Log(string.Join(",", dataList));
		}

		public static T GetByName(string name)
		{
			try
			{
				return dataByName[name];
			}
			catch (KeyNotFoundException e)
			{
				Debug.LogWarningFormat("No such entry exists in {0} database: {1}\n" + e, typeof(T), name);
				return null;
			}
		}
	}
}
