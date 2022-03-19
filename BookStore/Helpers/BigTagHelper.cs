using Microsoft.AspNetCore.Razor.TagHelpers;

namespace BookStore.Helpers
{
    //A code big attribute & big tag ne support karse
    //[HtmlTargetElement("big", Attributes = "big")]
    //[HtmlTargetElement(Attributes ="big")]

    //[HtmlTargetElement("big")]
    [HtmlTargetElement(Attributes = "big")]
    public class BigTagHelper:TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "h1";
            output.Attributes.RemoveAll("big");
            output.PreContent.SetContent("Hello ");
            output.PostContent.SetContent(" Good By");
            output.Attributes.SetAttribute("class", "text-success");
        }
    }
}
