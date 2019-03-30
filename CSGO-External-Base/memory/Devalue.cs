using System;
using System.Collections.Generic;
using System.Text;

namespace CSGO_External_Base.memory
{
    class Devalue<T>
    {
        public readonly int Address;
        private readonly bool Force;
        private T _value;
        private byte[] bytes = new byte[1] { 0 };
        private int Refresh;
        private int Sleep;

        public Devalue(int address, bool force = false, int sleep = 10)
        {
            Address = address;
            Force = force;
            Refresh = -1;
            Sleep = sleep;
        }

        public Devalue(IntPtr address, bool force = false, int sleep = 10)
        {
            Address = (int)address;
            Force = force;
            Refresh = -1;
            Sleep = sleep;
        }

        public T Value
        {
            get
            {
                _value = MemoryUtil.Read<T>((IntPtr)Address);

                return _value;
            }
            set
            {
                MemoryUtil.Write<T>((IntPtr)Address, value);
            }
        }
    }
}
