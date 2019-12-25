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
		protected GameObject Pivot;
		protected List<GameObject> Leaves;
		protected List<FileInfo> Textures;

		public Beast Beast;
		public Transform Position => Pivot.transform;

		public virtual void Setup(Beast beast, GameObject holder)
		{
			Textures = new List<FileInfo>();
			foreach (XmlNode li in beast.data.ModelProps["textures"])
			{
				if (File.Exists(li.InnerText))
					Textures.Add(new FileInfo(li.InnerText));
			}
			Pivot = this.gameObject;
			Pivot.transform.SetParent(holder.transform);
			Leaves = new List<GameObject>();
			Beast = beast;
		}
	}

}
