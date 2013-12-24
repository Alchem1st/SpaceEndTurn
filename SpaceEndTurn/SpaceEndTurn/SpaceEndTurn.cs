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

        public override void BeforeInvoke(InvocationInfo info)
        {
            return;
        }
        public override void AfterInvoke(InvocationInfo info, ref object returnValue)
        {
            try
            {
                if (info.targetMethod == "handleInput")
                {
                    bool chatopen = (bool)typeof(BattleMode).GetField("showChatInput", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(info.target);
                    if (!chatopen)
                    {
                        if (Input.GetKeyDown(KeyCode.Space))
                        {
                            endTurn((BattleMode)info.target);
                        }
                    }
                }
            }
            catch { }
            return;
        }

        private void endTurn(BattleMode bmode)
        {
            MethodInfo etmethod = typeof(BattleMode).GetMethod("endTurn", BindingFlags.Instance | BindingFlags.NonPublic);
            etmethod.Invoke(bmode, null);
        }
    }
}
