using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Helpers
{
    public class CustomEmailTagHelper : TagHelper
    {
        //MyEmail is work as my-email in html
        public string MyEmail { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";
            //output.Attributes.SetAttribute("href", "mailto:tirthshah@gmail.com");
            output.Attributes.SetAttribute("href", $"mailto:{MyEmail}");
            //SetHtmlContent ma HTML Pass Karay
            //output.Content.SetHtmlContent("<h6>html-myemail</h6>");
            //SetContent ma HTML as Text work Karse
            output.Content.SetContent("set-myemail");
            output.Attributes.Add("id", "my-email-id");
        }
    }
}

