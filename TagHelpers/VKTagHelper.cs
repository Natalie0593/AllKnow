using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogHost.TagHelpers
{
    public class VKTagHelper : TagHelper
    {
        private const string address = "https://vk.com/id359277526";
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";
            output.Attributes.SetAttribute("href", address);
            output.Content.SetContent("Developer");
        }
    }
}
