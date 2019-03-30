using System;
using System.Collections.Generic;
using System.Text;

namespace CSGO_External_Base.csgo
{
    class Client
    {
        private IntPtr module;

        public Client(IntPtr module)
        {
            this.module = module;
        }
    }
}
