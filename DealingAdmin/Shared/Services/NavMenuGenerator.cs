using System.Collections.Generic;

namespace DealingAdmin.Shared.Services
{
    public static class NavMenuGenerator
    {
        //oi pack
        private static readonly List<NavMenuItem> MenuItems = new()
        {
            NavMenuItem.Create("orders", "Orders", "list"),
            NavMenuItem.Create("candles", "Candles", "bar-chart")
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