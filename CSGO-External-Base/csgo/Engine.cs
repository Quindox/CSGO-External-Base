using System;
using System.Collections.Generic;
using System.Text;

namespace CSGO_External_Base.csgo
{
    class Engine
    {
        private IntPtr module;

        public Engine(IntPtr module)
        {
            this.module = module;
        }
    }
}
