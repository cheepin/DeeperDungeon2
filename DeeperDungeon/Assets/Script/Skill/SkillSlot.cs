using System;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.U2D;
using moving.player;
using skill.spell;

namespace skill
{

	[Serializable]
	public class SlotData : util.ConvertableBase64
	{
		public SlotData(string slotNumber) : base(slotNumber)
		{
		}

		public string CurrentSettedSkillName;
	}



	public class SkillSlot : HoldButton, IDragHandler, IEndDragHandler
	{

		[SerializeField]
		string SlotNumber;
		[SerializeField]
		Vector2 SelectSlotPanelPosition;
		[SerializeField]
		SpriteAtlas spriteAtras;

		public SlotData slotData;
		Func<Player, int, bool> spell;
		GameObject slotPanel = null;

		public void OnDrag(PointerEventData eventData)
		{
		}

		public void Save()
		{
			slotData.SaveData();
		}

		public void OnDestroy()
		{
		}

		public bool Load()
		{
			var slotDataTemp = new SlotData(SlotNumber);
			slotData = (SlotData)slotDataTemp.LoadData();
			if(slotData.CurrentSettedSkillName != "empty")
			{
				var baseSpell = (BaseSpell)Activator.CreateInstance(Type.GetType("skill.spell."+slotData.CurrentSettedSkillName));
				spell = baseSpell.spellAction;
				GetComponent<Image>().sprite = ResourceLoader.Instance.EmblemTexturePacker[slotData.CurrentSettedSkillName];
				return true;
			}
			else
			{
				return false;
			}
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			//---マウスカーソルの下にSelectableSkillオブジェクトがあったらそのスキルネームと画像を取得
			eventData.hovered.Where((x) => x.tag == "SelectableSkill")
			.ToList()
			.ForEach((x) =>
			{
				//---スキルボタンの取得　スキルネームとスペルを取得
				SkillButton skillButton = x.GetComponent<SkillButton>();
				slotData.CurrentSettedSkillName = skillButton.SkillData.skillName;
				spell = SkillManager.GetSkillData<SkillData>(slotData.CurrentSettedSkillName).spell;

				//---取得した位置を元にこのスロットのモノを置く
				Sprite setImage = x.GetComponent<Image>().sprite;
					GetComponent<Image>().sprite = setImage;

				SoundManager.SkillSet();
			
			
			});
			Destroy(slotPanel);
		}

		protected override void Start()
		{
			//---ロードかどうか
			if(dungeon.DungeonManager.Instance.FromLoad)
			{
				if(!Load())
					slotData = new SlotData(SlotNumber)
					{
						CurrentSettedSkillName = "empty"
					};
			}
			else
			{
				slotData = new SlotData(SlotNumber)
				{
					CurrentSettedSkillName = "empty"
				};
			}
			chargeTime = 15;
			doHoldFuncBeforeUp = true;
			holdFunc = InstantiateSlotPanel;
			clickFunc = CastSkill;
			GetComponent<Button>().onClick.AddListener(CastSkill);
			base.Start();
		}

		void CastSkill()
		{
			//---スロットパネルが表示されていたら消す
			if(slotPanel != null)
				Destroy(slotPanel);
			if(slotData.CurrentSettedSkillName != null && slotData.CurrentSettedSkillName != "empty")
				SkillManager.CastActiveSpell(spell, slotData.CurrentSettedSkillName);
		}

		void InstantiateSlotPanel()
		{
			var slotPanelRes = Resources.Load("UIElem/SkillSlotSetPanel");
			slotPanel = Instantiate(slotPanelRes, transform.parent) as GameObject;
			slotPanel.GetComponent<RectTransform>().anchoredPosition = SelectSlotPanelPosition;
		}
		public  override void  OnPointerDown(PointerEventData eventData)
		{
			if(button.IsInteractable())
				pressed = true;
			if(slotData.CurrentSettedSkillName =="empty")
				SoundManager.SkillSet2();
		}
	}

}