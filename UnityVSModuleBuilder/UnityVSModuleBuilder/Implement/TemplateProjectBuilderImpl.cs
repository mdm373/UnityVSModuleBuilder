using UnityVSModuleBuilder.Overlay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityVSModuleBuilder.Implement
{
    public class TemplateProjectBuilderImpl
    {
        private ProjectNameOverlay nameOverlay;

        private TemplateProjectBuilderImpl(ProjectNameOverlay nameOverlay)
        {
            this.nameOverlay = nameOverlay;
        }

        public static TemplateProjectBuilderImpl GetInstance(ProjectNameOverlay nameOverlay)
        {
            return new TemplateProjectBuilderImpl(nameOverlay);
        }

        public BuildProjectResponse DoBuild(BuildProjectRequest request)
        {
            bool isNameOverlaySuccess = nameOverlay.Overlay(request.GetProjectName());
            return BuildProjectResponseImpl.GetInstance(isNameOverlaySuccess);
        }

    }
}
