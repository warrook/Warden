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
		public override void Setup(XmlNode props)
		{
			base.Setup(props);
			SpriteRenderer r = this.gameObject.AddComponent<SpriteRenderer>();

			byte[] bytes = System.IO.File.ReadAllBytes(Textures.First().FullName);
			Texture2D texture = new Texture2D(1, 1);
			r.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 1f));
			//this.gameObject.AddComponent<SpriteRenderer>().sprite = Textures.First();
		}
	}
}
