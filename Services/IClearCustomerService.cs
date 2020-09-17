using System.Collections.Generic;

namespace Nop.Plugin.Widgets.ClearCustomerList.Services
{
    public interface IClearCustomerService
    {
        void DeleteSelectedList(IList<int> selectedIds);
    }
}
