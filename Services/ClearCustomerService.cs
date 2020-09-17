using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Services.Customers;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Messages;
using Nop.Services.Stores;
using System.Collections.Generic;
using System.Linq;

namespace Nop.Plugin.Widgets.ClearCustomerList.Services
{
    public class ClearCustomerService : IClearCustomerService
    {
        
        private readonly ICustomerService _customerService;
        private readonly INewsLetterSubscriptionService _newsLetterSubscriptionService;
        private readonly ILocalizationService _localizationService;
        private readonly INotificationService _notificationService;
        private readonly IWorkContext _workContext;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly IStoreService _storeService;

        public ClearCustomerService(ICustomerService customerService,
            INewsLetterSubscriptionService newsLetterSubscriptionService,
            ILocalizationService localizationService,
            INotificationService notificationService,
            IWorkContext workContext,
            ICustomerActivityService customerActivityService,
            IStoreService storeService)
        {
            _customerService = customerService;
            _newsLetterSubscriptionService = newsLetterSubscriptionService;
            _localizationService = localizationService;
            _notificationService = notificationService;
            _workContext = workContext;
            _customerActivityService = customerActivityService;
            _storeService = storeService;
        }

        public void DeleteSelectedList(IList<int> selectedIds)
        {
            var customers = new List<Customer>();
            customers.AddRange(_customerService.GetCustomersByIds(selectedIds.ToArray()));
            for (var i = 0; i < customers.Count; i++)
            {
                var customer = customers[i];
                if (_customerService.IsAdmin(customer))
                {
                    _notificationService.ErrorNotification(_localizationService.GetResource("Admin.ClearCustomerList.DeleteAdministrator"));
                    return;
                }
                if(customer.Id != _workContext.CurrentCustomer.Id)
                    _customerService.DeleteCustomer(customer);

                //activity log
                _customerActivityService.InsertActivity("DeleteCustomer",
                       string.Format(_localizationService.GetResource("ActivityLog.DeleteCustomer"), customer.Id), customer);

                //remove newsletter subscription (if exists)
                foreach (var store in _storeService.GetAllStores())
                {
                    var subscription = _newsLetterSubscriptionService.GetNewsLetterSubscriptionByEmailAndStoreId(customer.Email, store.Id);
                    if (subscription != null)
                        _newsLetterSubscriptionService.DeleteNewsLetterSubscription(subscription);
                }

            }

        }
    }
}
