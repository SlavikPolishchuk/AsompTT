using AsompTTViewStatus.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AsompTTViewStatus.TagHelpers
{
    public class PageLinkTagHelper : TagHelper
    {
        private IUrlHelperFactory urlHelperFactory;
        public PageLinkTagHelper(IUrlHelperFactory helperFactory)
        {
            urlHelperFactory = helperFactory;
        }
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }
        public PageViewModel PageModel { get; set; }
        public string PageAction { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);
            output.TagName = "div";

            // ����� ������ ����� ������������ ������ ul
            TagBuilder tag = new TagBuilder("ul");
            tag.AddCssClass("pagination");

            // ��������� ��� ������ - �� �������, ���������� � ���������
            TagBuilder currentItem = CreateTag(PageModel.Page, urlHelper);

            // ������� ������ �� ���������� ��������, ���� ��� ����
   

            if (PageModel.HasPrevPage)
            {
                TagBuilder prevItem = CreateTag(PageModel.Page - 1, urlHelper);
                tag.InnerHtml.AppendHtml(prevItem);
            }

            tag.InnerHtml.AppendHtml(currentItem);
            // ������� ������ �� ��������� ��������, ���� ��� ����
            if (PageModel.HasPageNext)
            {
                TagBuilder nextItem = CreateTag(PageModel.Page + 1, urlHelper);
                tag.InnerHtml.AppendHtml(nextItem);
            }

      
            output.Content.AppendHtml(tag);
        }

        TagBuilder CreateTag(int pageNumber, IUrlHelper urlHelper)
        {
            TagBuilder item = new TagBuilder("li");
            TagBuilder link = new TagBuilder("a");
            if (pageNumber == this.PageModel.Page)
            {
                item.AddCssClass("active");
            }
            else
            {
                link.Attributes["href"] = urlHelper.Action(PageAction, new { page = pageNumber });
            }
            item.AddCssClass("page-item");
            link.AddCssClass("page-link");
            link.InnerHtml.Append(pageNumber.ToString());
            item.InnerHtml.AppendHtml(link);
            return item;
        }
    }
}
