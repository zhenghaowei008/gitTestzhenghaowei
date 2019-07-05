using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace OutScript
{
	/// <summary>
	/// 特殊道具引导
	/// </summary>
	public class TutorialSpecialBase : TutorialEntityBase {
		public override void Process (CellEntity clickedCell)
		{
			base.Process (clickedCell); 
		}

		protected override IEnumerator ExcuteStep01 ()
		{
			//Debug.Break ();
			tutorialManager.SetBlockImage (true);
			tutorialManager.SetReceiveShow (false);
			tutorialManager.SetReceiveNext (false);
		
			//yield return TaskManager.Instance.GetWaitForSecond (GameConst.timeShowBackWithNone);

			List<int> tempCellPos=new List<int>();

			List<Cell> highLightCells = MatchInfoManager.specialInfos [0].cells;
			for (int i = 0; i < highLightCells.Count; i++) {
				tempCellPos.Add (highLightCells[i].X);
				tempCellPos.Add (highLightCells[i].Y);
			}
			tutorialManager.ShowBlackWithCells(tempCellPos.ToArray(),out dialogPos);

			yield return TaskManager.Instance.GetWaitForSecond (GameConst.waitShowBlackWithCells );
			tutorialManager.ShowSkipBtn ();
			tutorialManager.dialogPanel.gameObject.transform.localPosition = new Vector3 (20,200f,0);
			tutorialManager.ShowDialog (dialogPos,GetRealMessage(),true,false,false);

			yield return TaskManager.Instance.GetWaitForSecond (GameConst.waitShowDia0);
			if (highLightCells.Count != 0) {
				Cell fingerCell = GetFingerPosByCells (highLightCells);
				if (fingerCell != null) {
					tutorialManager.ShowFingerAnimation (new int[]{fingerCell.X,fingerCell.Y});
				} else {
					Logger.LogError ("没有找到合适的格子！！！请检查，正常不会走到这里！！！！");
					tutorialManager.ShowFingerAnimation (new int[]{highLightCells[0].X,highLightCells[0].Y});
				}
			}

			yield return TaskManager.Instance.GetWaitForSecond (GameConst.waitShowDia0);
			tutorialManager.SetBlockImage (false);
			tutorialManager.SetReceiveShow (false);
			tutorialManager.SetReceiveNext (false);
		}

		protected override IEnumerator ExcuteStep02 ()
		{
			tutorialManager.HideFingerAnimation ();
			tutorialManager.HideDialog (true);
			//tutorialManager.dialogPanel.ExitRoleAnim ();
			tutorialManager.HideBlackWithCells();
			tutorialManager.HideSkipBtn ();

			yield return TaskManager.Instance.GetWaitForSecond (GameConst.timeHideBackWithCells);
			RemoveSelfAndSaveDB ();

			SaveSDKData ();
		}

		public virtual string GetRealMessage()
		{
			return string.Empty;
		}

		public virtual int GetRealSpecialLevel()
		{
			return 0;
		 }

		protected override void SaveDBTutorial ()
		{
			TutorialDataManager.SaveDBTutorialSpecialLevel (GetRealSpecialLevel());

			//不可移动位置  点击跳过时也需弹提示
			TutorialPanel.Instance.tutorialTipPart.ShowTip (GetNextTipMessage());
		}

		public virtual Vector3 GetRealPos()
		{
			return new Vector3 (0,150,0);
		}

		public virtual string GetNextTipMessage()
		{
			return "";
		}

		private void SaveSDKData()
		{
			#if USE_SDK 
			switch(GetRealSpecialLevel())
			{
			case 1:
				PluginUser.getInstance().LogGuide(ESDKTutorialType.SpecialProp,userLevel.ToString(),"1",SDKUtil.cliVersion,DBCache.Inst.userMsg);
				break;
			case 2:
				PluginUser.getInstance().LogGuide(ESDKTutorialType.SpecialProp,userLevel.ToString(),"2",SDKUtil.cliVersion,DBCache.Inst.userMsg);
				break;
			case 3:
				PluginUser.getInstance().LogGuide(ESDKTutorialType.SpecialProp,userLevel.ToString(),"3",SDKUtil.cliVersion,DBCache.Inst.userMsg);
				break;
			}
			#endif 
		}

	}
}

