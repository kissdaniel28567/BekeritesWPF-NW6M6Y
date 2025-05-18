using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMechanics.Model {
    public class PlayerEventArgs {
        private String? _name1;
        private String? _name2;
        private String? _color1;
        private String? _color2;

        public String? Player1Name {
            get => _name1;
        }

        public String? Player2Name {
            get => _name2;
        }
        public String? Player1Color {
            get => _color1;
        }

        public String? Player2Color {
            get => _color2;
        }

        public PlayerEventArgs(String name1, String name2, String color1, String color2) {
            _name1 = name1;
            _name2 = name2;
            _color1 = color1;
            _color2 = color2;
        }
    }
}
