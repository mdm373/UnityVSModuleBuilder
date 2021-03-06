﻿using System.Collections.Generic;
using UnityVSModuleBuilder.Overlay;
using UnityVSModuleBuilder.TemplateCopy;

namespace UnityVSModuleBuilder.Implement
{
    internal class TemplateProjectBuilderImpl : TemplateProjectBuilder
    {
        private readonly List<DefinedOverlay> overlays;
        private readonly TemplateCopyController copyController;

        private TemplateProjectBuilderImpl(TemplateCopyController copyController, List<DefinedOverlay> overlays)
        {
            this.copyController = copyController;
            this.overlays = overlays;
        }

        public static TemplateProjectBuilderImpl GetInstance(TemplateCopyController copyController, List<DefinedOverlay> overlays)
        {
            return new TemplateProjectBuilderImpl(copyController, overlays);
        }

        public BuildProjectResponse DoBuild(BuildProjectRequest request)
        {
            bool isSuccessful = copyController.CopyAndCleanTemplate(request.GetCopyLocation(), request.GetProjectName());
            if (isSuccessful)
            {
                foreach (DefinedOverlay overlay in overlays)
                {
                    isSuccessful = overlay.Overlay(request);
                    if (!isSuccessful)
                    {
                        break;
                    }
                }
                
            }
            
            return BuildProjectResponseImpl.GetInstance(isSuccessful);
        }

    }
}
