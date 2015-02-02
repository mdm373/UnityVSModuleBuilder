using System;
using System.Reflection;
using UnityVSModuleBuilder;

namespace UnityVSModuleBuilder.CommandLine
{
    class Program
    {
        static void Main(string[] args)
        {
            ArgHelper argHelper = new ArgHelper(args);
            BuildProjectRequest request = GetRequest(argHelper);
            if (request != null)
            {
                TemplateProjectBuilder templateProjectBuilder = TemplateProjectFactory.GetNewTemplateProjectBuilder();
                BuildProjectResponse response = templateProjectBuilder.DoBuild(request);
                if (response.IsSuccess())
                {
                    Console.WriteLine("Project Generated.");
                }
                else
                {
                    Console.WriteLine("Project Generation Failed");
                }
            }
        }

        private static BuildProjectRequest GetRequest(ArgHelper argHelper)
        {
            BuildProjectRequest request = null;
            Boolean isHelpRequested = argHelper.IsHelpScreenRequested();
            Boolean isValidBuildRequest = IsValidBuildArgs(argHelper);
            if (isHelpRequested || !isValidBuildRequest)
            {
                PrintHelpScreen();
            }
            else 
            {
                request = GetRequestFromValidArgs(argHelper);
            }
            return request;
        }

        private static BuildProjectRequest GetRequestFromValidArgs(ArgHelper argHelper)
        {
            return new BuildProjectArgRequest(argHelper);
        }

        private static bool IsValidBuildArgs(ArgHelper argHelper)
        {
            bool isValid = argHelper.HasArg(ProgramConstants.PROJECT_NAME_ARG);
            isValid = isValid && argHelper.HasArg(ProgramConstants.COMPANY_NAME_ARG);
            isValid = isValid && argHelper.HasArg(ProgramConstants.COMPANY_SHORT_NAME_ARG);
            isValid = isValid && argHelper.HasArg(ProgramConstants.LOCATION_NAME_ARG);
            isValid = isValid && argHelper.HasArg(ProgramConstants.REPO_NAME_ARG);
            isValid = isValid && argHelper.HasArg(ProgramConstants.UNITY_ARG);
            return isValid;
        }

        private static void PrintHelpScreen()
        {
            String assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            Console.Write(String.Format(ProgramConstants.HELP_FORMAT, assemblyName, ProgramConstants.ARG_IDENTIFIER));
        }

    }
}
