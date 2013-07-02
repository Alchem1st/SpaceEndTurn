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

        private void spacelog(string line)
        {
            System.IO.StreamWriter dbug = new System.IO.StreamWriter(new System.IO.FileStream("C:\\SpaceEndTurn\\log.txt",System.IO.FileMode.Append));
            dbug.WriteLine(line);
            dbug.Close();
            dbug.Dispose();
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
            if (Input.GetKeyDown(KeyCode.Space))
            {
                try
                {
                    endTurn((BattleMode)info.target);
                }
                catch 
                {
                    spacelog(string.Concat(System.DateTime.Now.ToLongTimeString(), " Caught an error in AfterInvoke"));
                }
            }
            return;
        }

        private void endTurn(BattleMode bmode)
        {
            throw new Exception();
            MethodInfo etmethod = typeof(BattleMode).GetMethod("endTurn", BindingFlags.NonPublic);
            etmethod.Invoke(bmode, null);
        }
    }
}
