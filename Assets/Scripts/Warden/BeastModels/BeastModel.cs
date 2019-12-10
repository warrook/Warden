using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using UnityEngine;

namespace Warden
{
	public class BeastModel : MonoBehaviour
	{
		protected List<FileInfo> Textures;

		public virtual void Setup(XmlNode props)
		{
			Textures = new List<FileInfo>();
			foreach (XmlNode li in props["textures"])
			{
				if (File.Exists(li.InnerText))
					Textures.Add(new FileInfo(li.InnerText));
			}
				
				
		}
	}
}
