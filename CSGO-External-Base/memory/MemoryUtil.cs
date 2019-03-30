using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CSGO_External_Base.memory
{
    class MemoryUtil {

    public static IntPtr pHandle;
    public static String processName;
    const int PROCESS_ALL_ACCESS = 0x1F0FFF;

    [DllImport("kernel32.dll")]
    public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] buffer, int size,
        int lpNumberOfBytesWritten);

    [DllImport("kernel32.dll")]
    public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

    [DllImport("kernel32.dll")]
    public static extern bool ReadProcessMemory(
      IntPtr Handle,
      IntPtr Address,
      byte[] buffer,
      int Size,
      int BytesRead = 0);


    public static IntPtr GetProcessHandle(String name)
    {
        processName = name;
        try
        {
            pHandle = OpenProcess(PROCESS_ALL_ACCESS, false, System.Diagnostics.Process.GetProcessesByName(name).FirstOrDefault().Id);
            return MemoryUtil.pHandle;
        }
        catch
        {
            return IntPtr.Zero;
        }
    }

    public static IntPtr GetModule(String name)
    {
        Process p = Process.GetProcessesByName(processName).First();
        if (p.Modules.Count > 0)
        {
            foreach (ProcessModule m in p.Modules)
            {
               if (m.ModuleName.Equals(name))
                {
                    return m.BaseAddress;
                }
            }
        }

        return IntPtr.Zero;
    }

    public static void Write<T>(IntPtr Address, T value)
    {
        int size = Marshal.SizeOf<T>();
        byte[] buffer = new byte[size];
        var ptr = Marshal.AllocHGlobal((int)size);
        Marshal.StructureToPtr(value, ptr, true);
        Marshal.Copy(ptr, buffer, 0, (int)size);
        Marshal.FreeHGlobal(ptr);
        WriteProcessMemory(pHandle, Address, buffer, size, 0);
    }

    public static void WriteBytes(int address, byte[] value)
    {
        var nBytesRead = uint.MinValue;
        WriteProcessMemory(pHandle, (IntPtr)address, value, (int)value.Length, 0);
    }


    public static string ReadString(int address, int bufferSize, Encoding enc)
    {
        var buffer = new byte[bufferSize];
        ReadProcessMemory(pHandle, (IntPtr)address, buffer, bufferSize);
        var text = enc.GetString(buffer);
        if (text.Contains('\0'))
            text = text.Substring(0, text.IndexOf('\0'));

        return text;
    }

    public static T Read<T>(IntPtr address)
    {
        byte[] buffer = new byte[Marshal.SizeOf<T>()];
        ReadProcessMemory(pHandle, address, buffer, Marshal.SizeOf<T>());
        return GetStructure<T>(buffer);
    }

    public static byte[] ReadBytes(int address, int length)
    {
        var buffer = new byte[length];
        ReadProcessMemory(pHandle, (IntPtr)address, buffer, length);
        return buffer;
    }

    public static T GetStructure<T>(byte[] bytes)
    {
        var handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
        var structure = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
        handle.Free();
        return structure;
    }
}
}
