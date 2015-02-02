using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityVSModuleBuilder.CommandLine
{
    internal class ArgHelper
    {
       private List<string> args;
       private readonly Dictionary<String, String> argMap = new Dictionary<String, String>();

        public ArgHelper(string[] args)
        {
            this.args = new List<String>(args);
            BuildArgMap();
            
        }

        private void BuildArgMap()
        {
            String argEntry = null;
            foreach (String arg in args)
            {
                String cleanArg = arg.Trim();
                cleanArg = cleanArg.Replace("\"", String.Empty);
                cleanArg = cleanArg.Replace("\r", String.Empty);
                cleanArg = cleanArg.Replace("\n", String.Empty);
                cleanArg = cleanArg.Replace("\t", String.Empty);
                if (cleanArg.StartsWith(ProgramConstants.ARG_IDENTIFIER))
                {
                    cleanArg = cleanArg.ToUpper();
                    cleanArg = cleanArg.Remove(0, 1);
                    argEntry = cleanArg;
                    argMap[argEntry] = String.Empty;
                }
                else if (argEntry != null)
                {
                    argMap[argEntry] = argMap[argEntry] + " " + cleanArg;
                }
            }
        }

        public bool HasArg(String identifier)
        {
            identifier = identifier.ToUpper();
            return argMap.ContainsKey(identifier);
        }
        public String GetArgValue(String identifier)
        {
            String value = String.Empty;
            identifier = identifier.ToUpper();
            if (argMap.ContainsKey(identifier))
            {
                value = argMap[identifier].Trim();
            }
            return value;
        }

        public bool IsHelpScreenRequested()
        {
            bool isRequested = false;
            foreach(String arg in args){
                String cleanArg = arg.ToUpper();
                cleanArg = cleanArg.Trim();
                if (ProgramConstants.HELP_IDENTIFIER.Equals(cleanArg))
                {
                    isRequested = true;
                    break;
                }
            }
            return isRequested;
        }
    }
}
