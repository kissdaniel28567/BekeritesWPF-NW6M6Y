using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace bekeritesWPF.ViewModel {
    public class PlayerValidator {

        public static bool IsCorrectPlayer(String playerName, String playerColor) {
            var colorProperties = typeof(Color).GetProperties(BindingFlags.Static | BindingFlags.Public)
                .Where(p => p.PropertyType == typeof(Color));
            foreach (var prop in colorProperties) {
                if (prop.Name.Equals(playerColor, StringComparison.OrdinalIgnoreCase)) {
                    return !playerColor.Equals("white", StringComparison.CurrentCultureIgnoreCase) && !String.IsNullOrEmpty(playerName);
                }
            }
            return false;
        }
    }
}
