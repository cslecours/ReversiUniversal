using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;

namespace ReversiUniversal
{
    class PlayerNamesViewModel : ViewModelBase
    {
        private string blackName;
        private string whiteName;
        public string BlackName { get { return blackName; } set { Set(() => BlackName, ref blackName, value); } }
        public string WhiteName { get { return whiteName; } set { Set(() => WhiteName, ref whiteName, value); } }
    }
}
