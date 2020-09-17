using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Widgets.ClearCustomerList.Components
{
    [ViewComponent(Name = "ClearCustomerListButton")]
    public class ClearCustomerListViewComponent : NopViewComponent
    {
        
        public ClearCustomerListViewComponent(
            
            )
        {
            
        }


        public IViewComponentResult Invoke(string widgetZone)
        {
         
            return View("~/Plugins/Widgets.ClearCustomerList/Views/Button.cshtml");
        }

    }
}
