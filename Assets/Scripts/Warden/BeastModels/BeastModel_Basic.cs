using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UnityEngine;

namespace Warden
{
	public class BeastModel_Basic : BeastModel
	{
		public override void Setup(Beast beast, GameObject holder)
		{
			base.Setup(beast, holder);

			GameObject leaf = GameObject.CreatePrimitive(PrimitiveType.Quad);
			leaf.transform.SetParent(Pivot.transform);
			leaf.name = this.GetType().Name;
			MeshRenderer r = leaf.GetComponent<MeshRenderer>();

			byte[] bytes = System.IO.File.ReadAllBytes(Textures.First().FullName);
			Texture2D texture = new Texture2D(1, 1);
			texture.LoadImage(bytes);

			r.material = Resources.Load<Material>("BeastLeaf");
			r.material.mainTexture = texture;

			leaf.transform.localScale = new Vector3((texture.width * 0.5f) * 0.01f, (texture.width * 0.5f) * 0.01f);
			leaf.transform.localPosition = new Vector3(0f, (texture.height * 0.25f) * 0.01f); //100 pixels per unit
			Leaves.Add(leaf);
		}

		private void Update()
		{
			//this.Leaves.First().transform.Rotate(Vector3.up, 1f);
		}
	}
}
