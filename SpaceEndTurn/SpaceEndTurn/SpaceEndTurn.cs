using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScrollsModLoader.Interfaces;
using Mono.Cecil;
using UnityEngine;
using System.Reflection;

namespace SpaceEndTurnMod
{
    public class SpaceEndTurn : BaseMod
    {
        public static string GetName()
        {
            return "SpaceEndTurn";
        }
        public static int GetVersion()
        {
            return 1;
        }

        public static MethodDefinition[] GetHooks(TypeDefinitionCollection scrollsTypes, int version)
        {
            try
            {
                return new MethodDefinition[] {
					scrollsTypes["BattleMode"].Methods.GetMethod("handleInput")[0]
				};
            }
            catch
            {
                return new MethodDefinition[] { };
            }
        }

        public override bool BeforeInvoke(InvocationInfo info, out object returnValue)
        {
            returnValue = null;
            return false;
        }
        public override void AfterInvoke(InvocationInfo info, ref object returnValue)
        {
            if (info.targetMethod == "handleInput")
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    try
                    {

                        endTurn((BattleMode)info.target);
                    }
                    catch (Exception e)
                    {
                    }
                }
            }
            return;
        }

        private void endTurn(BattleMode bmode)
        {
            //throw new Exception();
            MethodInfo etmethod = typeof(BattleMode).GetMethod("endTurn", BindingFlags.Instance | BindingFlags.NonPublic);
            etmethod.Invoke(bmode, null);
        }
    }
}
