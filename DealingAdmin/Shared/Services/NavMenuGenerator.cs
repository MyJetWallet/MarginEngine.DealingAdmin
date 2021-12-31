using System.Collections.Generic;

namespace DealingAdmin.Shared.Services
{
    public static class NavMenuGenerator
    {
        //oi pack
        private static readonly List<NavMenuItem> MenuItems = new()
        {
            NavMenuItem.Create("ClientView", "Client View", "people"),
            NavMenuItem.Create("Orders", "Orders", "list_alt"),
            NavMenuItem.Create("Candles", "Candles", "candlestick_chart"),
            NavMenuItem.Create("EmergencyTools", "Emergency Tools", "warning_amber"),
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