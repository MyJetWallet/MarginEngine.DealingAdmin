using System.Collections.Generic;

namespace DealingAdmin.Shared.Services
{
    public static class NavMenuGenerator
    {
        //oi pack
        private static readonly List<NavMenuItem> MenuItems = new()
        {
            NavMenuItem.Create("Orders", "Orders", "list"),
            NavMenuItem.Create("Candles", "Candles", "bar-chart"),
            NavMenuItem.Create("EmergencyTools", "Emergency Tools", "tool"),
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