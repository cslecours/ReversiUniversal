using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReversiUniversal
{
    class PlayerSettingsChangedMessage
    {
        public string BlackName { get; set; }

        public string WhiteName { get; set; }
    }
}
