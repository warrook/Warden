using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Generic.UI
{
	public class UIBattleButton : MonoBehaviour, IPointerClickHandler
	{
		public Action OnChoose;
		private float radius;

		private void Awake()
		{
			radius = gameObject.transform.Find("Background").GetComponent<Image>().mainTexture.width * 0.5f;
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			float distance = Vector2.Distance(gameObject.transform.position, eventData.position);
			if (distance < radius)
				OnChoose();
		}
	}
}
