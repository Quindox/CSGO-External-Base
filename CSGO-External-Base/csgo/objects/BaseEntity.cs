using CSGO_External_Base.memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSGO_External_Base.csgo.objects
{
    class BaseEntity
    {
        public Devalue<IntPtr> Base;

        public BaseEntity(IntPtr adress)
        {
            Base = new Devalue<IntPtr>(adress);
        }
    }
}
