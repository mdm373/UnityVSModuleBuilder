using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityVSModuleBuilder.FileSystem;
using UnityVSModuleBuilder.Implement;
using UnityVSModuleBuilder.Overlay;
using UnityVSModuleBuilder.TemplateCopy;

namespace UnityVSModuleBuilder
{
    public class TemplateProjectFactory
    {
        public static BuildProjectRequest GetNewRequest(String projectName, String copyLocation)
        {
            BuildProjectRequestImpl request = new BuildProjectRequestImpl();
            request.SetProjectName(projectName);
            request.SetCopyLocation(copyLocation);
            return request;
        }

        public static TemplateProjectBuilder GetNewTemplateProjectBuilder()
        {
            FileSystemController fileSystem = new FileSystemControllerImpl();
            TemplateCopyController copyController = new TemplateCopyControllerImpl(fileSystem);
            ProjectNameOverlay nameOverlay = new ProjectNameOverlayImpl();
            return TemplateProjectBuilderImpl.GetInstance(copyController, nameOverlay);
        }
    }
}
