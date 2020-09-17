using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Widgets.ClearCustomerList.Services;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using System.Collections.Generic;
using System.Linq;

namespace Nop.Plugin.Widgets.ClearCustomerList.Controllers
{
    [AuthorizeAdmin]
    [Area(AreaNames.Admin)]
    public class ClearCustomerListController : BasePluginController
    {
        private readonly IPermissionService _permissionService;
        private readonly IClearCustomerService _clearCustomerService;

        public ClearCustomerListController(
            IPermissionService permissionService,
            IClearCustomerService clearCustomerService
            )
        {
            _permissionService = permissionService;
            _clearCustomerService = clearCustomerService;
        }

        public virtual IActionResult DeleteSelected(ICollection<int> selectedIds)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomers))
                return AccessDeniedView();

            if (selectedIds != null)
            {
                _clearCustomerService.DeleteSelectedList(selectedIds.ToList());
            }
            return Json(new { Result = true });
        }

    }
}
