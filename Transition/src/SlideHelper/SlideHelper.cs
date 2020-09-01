﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Skclusive.Core.Component;

namespace Skclusive.Material.Transition
{
    public class SlideHelper
    {
        private IJSRuntime JSRuntime { get; }

        public SlideHelper(IJSRuntime jsruntime)
        {
            JSRuntime = jsruntime;
        }

        public async Task<string> GetSlideTranslateValueAsync(Placement placement, ElementReference? element)
        {
            if (element.HasValue)
            {
                return await JSRuntime.InvokeAsync<string>("Skclusive.Material.Transition.getSlideTranslateValue", placement.ToString().ToLower(), element);
            }

            return null;
        }

        public async Task SetSlideTranslateValueAsync(Placement placement, ElementReference? element)
        {
            if (element.HasValue)
            {
                await JSRuntime.InvokeVoidAsync("Skclusive.Material.Transition.setSlideTranslateValue", placement.ToString().ToLower(), element);
            }
        }
    }
}
