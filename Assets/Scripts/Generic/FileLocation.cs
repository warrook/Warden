using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;

namespace Generic
{
	public static class FileLocation
	{
		public static DirectoryInfo RootDir = new DirectoryInfo(Application.dataPath);
		public static DirectoryInfo ContentDir = new DirectoryInfo(Path.Combine(RootDir.ToString(), "Content" + Path.DirectorySeparatorChar));
	}
}
