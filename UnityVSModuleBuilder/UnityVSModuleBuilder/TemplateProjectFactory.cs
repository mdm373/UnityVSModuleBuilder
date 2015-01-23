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
        public static BuildProjectRequest GetNewRequest(
            String projectName, 
            String copyLocation,
            String companyName)
        {
            BuildProjectRequestImpl request = new BuildProjectRequestImpl();
            request.SetProjectName(projectName);
            request.SetCopyLocation(copyLocation);
            request.SetCompanyName(companyName);
            return request;
        }

        public static TemplateProjectBuilder GetNewTemplateProjectBuilder()
        {
            FileSystemController fileSystem = new FileSystemControllerImpl();
            TemplateCopyController copyController = new TemplateCopyControllerImpl(fileSystem);
            List<DefinedOverlay> overlays = new List<DefinedOverlay>();
            overlays.Add(new ProjectNameOverlay(fileSystem));
            overlays.Add(new CompanyNameOverlayImpl(fileSystem));
            return TemplateProjectBuilderImpl.GetInstance(copyController, overlays);
        }
    }
}
