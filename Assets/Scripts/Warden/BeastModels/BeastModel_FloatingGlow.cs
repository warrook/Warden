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
		public override void Setup(Beast beast, GameObject holder)
		{
			base.Setup(beast, holder);

			GameObject lightObject = new GameObject("Glow");
			lightObject.transform.localPosition = Leaves.First().transform.localPosition;
			lightObject.transform.SetParent(Leaves.First().transform);
			int[] colors = Array.ConvertAll(beast.data.ModelProps["color"].InnerText.Split(','), int.Parse);

			Color color = new Color(colors[0] / 255f, colors[1] / 255f, colors[2] / 255f);
			float intensity = float.Parse(beast.data.ModelProps["intensity"].InnerText);
			Light light = lightObject.AddComponent<Light>();
			light.color = color;
			light.intensity = intensity;
			//Debug.Log(lightObject.transform.localPosition);
		}
	}
}
