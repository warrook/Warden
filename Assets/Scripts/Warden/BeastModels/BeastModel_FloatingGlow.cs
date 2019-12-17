using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UnityEngine;

namespace Warden
{
	public class BeastModel_FloatingGlow : BeastModel_Floating
	{
		public override void Setup(XmlNode props, GameObject holder)
		{
			base.Setup(props, holder);

			GameObject lightObject = new GameObject("Glow");
			lightObject.transform.localPosition = Leaves.First().transform.localPosition;
			lightObject.transform.SetParent(Leaves.First().transform);
			int[] colors = Array.ConvertAll(props["color"].InnerText.Split(','), int.Parse);

			Color color = new Color(colors[0] / 255f, colors[1] / 255f, colors[2] / 255f);
			float intensity = float.Parse(props["intensity"].InnerText);
			Light light = lightObject.AddComponent<Light>();
			light.color = color;
			light.intensity = intensity;
			//Debug.Log(lightObject.transform.localPosition);
		}
	}
}
