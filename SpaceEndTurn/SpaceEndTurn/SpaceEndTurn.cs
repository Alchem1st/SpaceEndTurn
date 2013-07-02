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

        private static void spacelog(string line)
        {
            System.IO.StreamWriter dbug = new System.IO.StreamWriter(new System.IO.FileStream("C:\\SpaceEndTurn\\log.txt",System.IO.FileMode.Append));
            dbug.WriteLine(string.Concat(System.DateTime.Now.ToShortDateString(), " ", System.DateTime.Now.ToLongTimeString(), " ", line));
            dbug.Close();
            dbug.Dispose();
        }

        public static MethodDefinition[] GetHooks(TypeDefinitionCollection scrollsTypes, int version)
        {
            try
            {
                return new MethodDefinition[] {
					scrollsTypes["BattleMode"].Methods.GetMethod("handleInput")[0],
                    scrollsTypes["BattleMode"].Methods.GetMethod("Start")[0]
				};
            }
            catch
            {
                spacelog("Caught an error in MethodDefinition");
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
                        spacelog(string.Concat("Caught an error in AfterInvoke:"));
                        spacelog(e.ToString());
                    }
                }
            }
            if (info.targetMethod == "Start")
            {
                /*spacelog("reached start hook");
                List<ICommListener> commlist;
                commlist = (List<ICommListener>)typeof(Communicator).GetField("callBackTargets", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(App.Communicator);
                fbmode = (BattleMode)commlist[commlist.Count - 1];
                spacelog("found battlemode");*/
            }
            return;
        }

        private BattleMode fbmode;
        private void endTurn(BattleMode bmode)
        {
            //throw new Exception();
            MethodInfo etmethod = typeof(BattleMode).GetMethod("endTurn", BindingFlags.Instance | BindingFlags.NonPublic);
            etmethod.Invoke(bmode, null);
        }
    }
}
