using System;

using ScrollsModLoader.Interfaces;
using UnityEngine;
using Mono.Cecil;

namespace Template.mod
{
	public class TutorialCondomMod : BaseMod, IOkCancelCallback
	{
		bool askForTutorial = true;
		//initialize everything here, Game is loaded at this point
		public TutorialCondomMod ()
		{
		}


		public static string GetName ()
		{
			return "TutorialCondom";
		}

		public static int GetVersion ()
		{
			return 1;
		}

		public static MethodDefinition[] GetHooks (TypeDefinitionCollection scrollsTypes, int version)
		{
			try {
				MethodDefinition[] method = scrollsTypes ["GameActionManager"].Methods.GetMethod ("StartGame");
				if (method.Length == 1) {
					return method;
				} else {
					//Can not Hook into the right Method
					return new MethodDefinition[] { };
				}
			} catch {
				return new MethodDefinition[] {};
			}
		}

		
		public override void BeforeInvoke (InvocationInfo info)
		{
			return;
		}

		#region ICancelCallback implementation

		public void PopupCancel (string popupType)
		{
		}

		public void PopupOk (string popupType)
		{
			askForTutorial = false;
			App.GameActionManager.StartGame (GameActionManager.StartType.START_TUTORIAL);
		}

		#endregion

		public override void AfterInvoke (InvocationInfo info, ref object returnValue)
		{
			return;
		}

		public override void ReplaceMethod (InvocationInfo info, out object returnValue)
		{
			App.Popups.ShowOkCancel (this, "fail", "Tutorial?", "Do you really want to play the Tutorial?", "Yes", "No");
			returnValue = null;
		}

		public override bool WantsToReplace (InvocationInfo info)
		{
			if (info.targetMethod.Equals ("StartGame") && info.arguments [0].Equals (GameActionManager.StartType.START_TUTORIAL)) {
				// Want to Start Tutorial
				if (!askForTutorial) {
					//Already asked for tutorial, so we don't replace this time.
					askForTutorial = true;
					return false;
				} else { //ask For Tutorial => Replace Method
					return true;
				}
			}
			return false;
		}


	}
}

