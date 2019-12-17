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
		public override void Setup(XmlNode props, GameObject holder)
		{
			base.Setup(props, holder);

			//GameObject leaf = new GameObject(this.GetType().Name);
			//leaf.transform.SetParent(Pivot.transform);
			//SpriteRenderer r = leaf.AddComponent<SpriteRenderer>();

			//byte[] bytes = System.IO.File.ReadAllBytes(Textures.First().FullName);
			//Texture2D texture = new Texture2D(1, 1);
			//texture.LoadImage(bytes);
			//r.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
			////r.material = Resources.Load<Material>("Outline");


			//leaf.transform.localScale = new Vector3(0.5f, 0.5f);
			//leaf.transform.localPosition = new Vector3(0f, (r.sprite.texture.height * 0.25f) * 0.01f); //100 pixels per unit
			//Leaves.Add(leaf);
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
