using UnityVSModuleBuilder.Overlay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityVSModuleBuilder.TemplateCopy;

namespace UnityVSModuleBuilder.Implement
{
    public class TemplateProjectBuilderImpl : TemplateProjectBuilder
    {
        private readonly ProjectNameOverlay nameOverlay;
        private readonly TemplateCopyController copyController;

        private TemplateProjectBuilderImpl(TemplateCopyController copyController, ProjectNameOverlay nameOverlay)
        {
            this.copyController = copyController;
            this.nameOverlay = nameOverlay;
        }

        public static TemplateProjectBuilderImpl GetInstance(TemplateCopyController copyController, ProjectNameOverlay nameOverlay)
        {
            return new TemplateProjectBuilderImpl(copyController, nameOverlay);
        }

        public BuildProjectResponse DoBuild(BuildProjectRequest request)
        {
            bool isSuccessful = copyController.CopyTemplate(request.GetCopyLocation());
            if (isSuccessful)
            {
                isSuccessful = nameOverlay.Overlay(request.GetProjectName());
            }
            
            return BuildProjectResponseImpl.GetInstance(isSuccessful);
        }

    }
}
