using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityVSModuleBuilder.FileSystem;
using UnityVSModuleBuilder.Implement;
using UnityVSModuleBuilder.Overlay;
using UnityVSModuleBuilder.TemplateCopy;

namespace UnityVSModuleBuilder
{
    public class TemplateProjectFactory
    {


        public static TemplateProjectBuilder GetNewTemplateProjectBuilder()
        {
            FileSystemController fileSystem = new FileSystemControllerImpl();
            TemplateCopyController copyController = new TemplateCopyControllerImpl(fileSystem);
            List<DefinedOverlay> overlays = new List<DefinedOverlay>();
            overlays.Add(new ProjectNameOverlay(fileSystem));
            overlays.Add(new CompanyNameOverlayImpl(fileSystem));
            overlays.Add(new UnityLocationOverlay(fileSystem));
            overlays.Add(new CompanyShortNameOverlay(fileSystem));
            overlays.Add(new ModuleRepoOverlay(fileSystem));
            overlays.Add(new EditorManagedCodeOverlay(fileSystem));
            return TemplateProjectBuilderImpl.GetInstance(copyController, overlays);
        }
    }
}
