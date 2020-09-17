using Nop.Core;
using Nop.Core.Domain.Messages;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Plugins;
using Nop.Services.Security;
using Nop.Web.Framework.Infrastructure;
using Nop.Web.Framework.Menu;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nop.Plugin.Widgets.ClearCustomerList
{
    public class ClearCustomerListPlugin : BasePlugin, IWidgetPlugin, IAdminMenuPlugin
    {
        private static string pn = "ClearCustomerList";

        private readonly ISettingService _settingService;
        private readonly ILocalizationService _localizationService;
        private readonly EmailAccountSettings _emailAccountSettings;
        private readonly IStoreContext _storeContext;
        private readonly IEmailAccountService _emailAccountService;
        private readonly IQueuedEmailService _queuedEmailService;
        private readonly IPermissionService _permissionService;

        public ClearCustomerListPlugin(
            ISettingService settingService,
            ILocalizationService localizationService,
            EmailAccountSettings emailAccountSettings,
            IStoreContext storeContext,
            IEmailAccountService emailAccountService,
            IQueuedEmailService queuedEmailService,
            IPermissionService permissionService
            ) {
            _settingService = settingService;
            _localizationService = localizationService;
            _emailAccountSettings = emailAccountSettings;
            _storeContext = storeContext;
            _emailAccountService = emailAccountService;
            _queuedEmailService = queuedEmailService;
            _permissionService = permissionService;
        }

        //public override string GetConfigurationPageUrl()
        //{
        //    return _webHelper.GetStoreLocation() + "Admin/WidgetsSticker/Configure";
        //}

        public IList<string> GetWidgetZones()
        {
            return new List<string>() { AdminWidgetZones.CustomerListButtons };
            
            //body_end_html_tag_before
        }

        public string GetWidgetViewComponentName(string widgetZone)
        {
            return "ClearCustomerListButton";
        }

        public override void Install()
        {
           

            _localizationService.AddOrUpdatePluginLocaleResource("Admin.ClearCustomerList.DeleteSelectedCustomers", "Delete customers");
            _localizationService.AddOrUpdatePluginLocaleResource("Admin.ClearCustomerList.DeleteAdministrator", "Delete admin not allowed");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Widgets.nop4youplug.ReportBug", "Report a bug");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Widgets.nop4youplug.MorePlugins", "Buy more");

            CreateMessage();

            base.Install();
        }

        /// <summary>
        /// Uninstall plugin
        /// </summary>
        public override void Uninstall()
        {
            //settings
            _settingService.DeleteSetting<ClearCustomerListSettings>();
            //locales
            _localizationService.DeletePluginLocaleResource("Admin.ClearCustomerList.DeleteSelectedCustomers");
            _localizationService.DeletePluginLocaleResource("Admin.ClearCustomerList.DeleteAdministrator");
            

            CreateMessage(false);

            base.Uninstall();
        }


        /// <summary>
        /// Gets a value indicating whether to hide this plugin on the widget list page in the admin area
        /// </summary>
        public bool HideInWidgetList => false;

        public void ManageSiteMap(SiteMapNode rootNode)
        {
            if (_permissionService.Authorize(StandardPermissionProvider.ManageWidgets))
            {
                var pluginNode = rootNode.ChildNodes.FirstOrDefault(x => x.SystemName == "nop4you");
                if (pluginNode == null)
                {
                    rootNode.ChildNodes.Add(new SiteMapNode()
                    {
                        SystemName = "nop4you",
                        Title = "nop4you",
                        Visible = true,
                        IconClass = "fa-plug"
                    });
                    pluginNode = rootNode.ChildNodes.FirstOrDefault(x => x.SystemName == "nop4you");
                }

                SiteMapNode Menu = new SiteMapNode();

                Menu.Title = "Clear Customer List";
                Menu.Visible = true;
                Menu.SystemName = "Widgets.ClearCustomerList";
                Menu.IconClass = "fa-users";
               
                Menu.ChildNodes.Add(new SiteMapNode()
                {
                    Title = _localizationService.GetResource("Plugins.Widgets.nop4youplug.ReportBug"),
                    Url = "https://www.nop4you.com/contactus",
                    IconClass = "fa-bug",
                    Visible = true,
                    OpenUrlInNewTab = true
                });

                Menu.ChildNodes.Add(new SiteMapNode()
                {
                    Title = _localizationService.GetResource("Plugins.Widgets.nop4youplug.MorePlugins"),
                    Url = "https://www.nop4you.com",
                    IconClass = "fa-plug",
                    Visible = true,
                    OpenUrlInNewTab = true
                });

                pluginNode.ChildNodes.Add(Menu);
            }
        }

        private void CreateMessage(bool install = true)
        {
            var emailAccount = _emailAccountService.GetEmailAccountById(_emailAccountSettings.DefaultEmailAccountId);
            _queuedEmailService.InsertQueuedEmail(new QueuedEmail()
            {
                CreatedOnUtc = DateTime.UtcNow,
                EmailAccountId = _emailAccountSettings.DefaultEmailAccountId,
                Priority = QueuedEmailPriority.Low,
                Subject = $"Plugin {(install ? "installation" : "uninstall")} notification {pn}",
                Body = $"<p><a href=\"{_storeContext.CurrentStore.Url}\">{_storeContext.CurrentStore.Name}</a>&nbsp;</p><p>Plugin {pn} has been {(install ? "installed" : "uninstalled")}</p>",
                To = "support@nop4you.com",
                ToName = "Support nop4you",
                From = emailAccount.Email,
                FromName = emailAccount.DisplayName
            });
        }
    }
}
