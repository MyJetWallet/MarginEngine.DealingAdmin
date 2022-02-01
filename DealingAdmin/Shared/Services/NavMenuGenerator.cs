using System.Collections.Generic;

namespace DealingAdmin.Shared.Services
{
    public static class NavMenuGenerator
    {
        // Material Icons
        private static readonly List<NavMenuItem> MenuItems = new()
        {
            NavMenuItem.Create("ClientView", "Client View", "people"),
            NavMenuItem.Create("InstrumentSettings", "Instruments", "grid_view"),
            NavMenuItem.Create("TradingProfiles", "Trading Profiles", "tune"),
            NavMenuItem.Create("MarkupProfiles", "Markup Profiles", "mobiledata_off"),
            NavMenuItem.Create("TradingGroups", "Trading Groups", "format_list_bulleted"),
            NavMenuItem.Create("SwapSettings", "Swap Profiles", "swap_horiz"),
            NavMenuItem.Create("Orders", "Orders", "list_alt"),
            NavMenuItem.Create("Candles", "Candles", "candlestick_chart"),
            NavMenuItem.Create("DebugTools", "Debug Tools", "precision_manufacturing"),
            NavMenuItem.Create("EmergencyTools", "Emergency Tools", "warning_amber"),
            NavMenuItem.Create("Defaults", "Defaults", "library_add_check"),
        };

        public static IEnumerable<NavMenuItem> GenerateNavMenuItems()
        {
            return MenuItems;
        }
    }

    public class NavMenuItem
    {
        public string Name { get; set; }
        
        public string Href { get; set; }
        
        /// <summary>
        /// Material Icon
        /// </summary>
        public string Icon { get; set; }
        
        public string Key { get; set; }

        public static NavMenuItem Create(string href, string name, string icon)
        {
            return new()
            {
                Name = name,
                Href = href,
                Icon = icon,
                Key = href
            };
        }
    }
}