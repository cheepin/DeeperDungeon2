using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UnityStandardAssets.CrossPlatformInput
{
	public class Joystick : util.Singleton<Joystick>,IPointerDownHandler, IPointerUpHandler, IDragHandler
	{
		public enum AxisOption
		{
			// Options for which axes to use
			Both, // Use both
			OnlyHorizontal, // Only horizontal
			OnlyVertical // Only vertical
		}

		public int MovementRange = 100;
		public AxisOption axesToUse = AxisOption.Both; // The options for the axes that the still will use
		public string horizontalAxisName = "Horizontal"; // The name given to the horizontal axis for the cross platform input
		public string verticalAxisName = "Vertical"; // The name given to the vertical axis for the cross platform input

		Vector3 m_StartPos;
		bool m_UseX; // Toggle for using the x axis
		bool m_UseY; // Toggle for using the Y axis
		CrossPlatformInputManager.VirtualAxis m_HorizontalVirtualAxis; // Reference to the joystick in the cross platform input
		CrossPlatformInputManager.VirtualAxis m_VerticalVirtualAxis; // Reference to the joystick in the cross platform input
		static bool created = false;
		static Vector2 originalPos = new Vector2(-100,-100);
		UnityAction<Scene> restorePos;

        void Start()
        {
			if(!created)
			{
				CreateVirtualAxes();
				created = true;
			}
			restorePos = (scene)=>{RestorePos();};
			SceneManager.sceneUnloaded += restorePos;
			if(originalPos == new Vector2(-100, -100))
			{
				originalPos = transform.position;
			}
			Instance.transform.position = originalPos;
			Instance.m_StartPos = transform.position;


		}

		void UpdateVirtualAxes(Vector3 value)
		{
			var delta = Instance.m_StartPos - value;
			delta.y = -delta.y;
			delta /= MovementRange;
			if (m_UseX)
			{
				m_HorizontalVirtualAxis.Update(-delta.x);
			}

			if (m_UseY)
			{
				m_VerticalVirtualAxis.Update(delta.y);
			}
		}

		void CreateVirtualAxes()
		{
			// set axes to use
			m_UseX = (axesToUse == AxisOption.Both || axesToUse == AxisOption.OnlyHorizontal);
			m_UseY = (axesToUse == AxisOption.Both || axesToUse == AxisOption.OnlyVertical);

			// create new axes based on axes to use
			if (m_UseX)
			{
				m_HorizontalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(horizontalAxisName);
				CrossPlatformInputManager.RegisterVirtualAxis(m_HorizontalVirtualAxis);
			}
			if (m_UseY)
			{
				m_VerticalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(verticalAxisName);
				CrossPlatformInputManager.RegisterVirtualAxis(m_VerticalVirtualAxis);
			}
		}


		public void OnDrag(PointerEventData data)
		{
			Vector3 newPos = Vector3.zero;
			if (m_UseX)
			{
				int delta = (int)(data.position.x - m_StartPos.x);
				newPos.x = delta;
			}

			if (m_UseY)
			{
				int delta = (int)(data.position.y - m_StartPos.y);
				newPos.y = delta;
			}
			Instance.transform.position = Vector3.ClampMagnitude(new Vector3 (newPos.x, newPos.y,0),100)+m_StartPos;
			UpdateVirtualAxes(Instance.transform.position);
		}


		public void OnPointerUp(PointerEventData data)
		{
			RestorePos();
		}
		
		void RestorePos()
		{
			Instance.transform.position = m_StartPos;
			UpdateVirtualAxes(Instance.transform.position);
		}


		protected override void OnInstanceDestroy()
		{
			SceneManager.sceneUnloaded -= restorePos;
			created = false;
			CrossPlatformInputManager.UnRegisterVirtualAxis(horizontalAxisName);
			CrossPlatformInputManager.UnRegisterVirtualAxis(verticalAxisName);

				//base.OnDestroy();
		}

		public void OnPointerDown(PointerEventData eventData)
		{
		}
	}
}