using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace IWander.Memory
{
  public class Mem<T> : IDisposable
    where T : unmanaged
  {
    private bool _disposed = false;
    private IntPtr _native;
    private long _length;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Mem(long length)
      :this(length, false)
    {

    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe Mem(long length, bool init)
    {
      var size = sizeof(T) * length;
      _native = Marshal.AllocHGlobal(new IntPtr(size));
      _length = length;
      InitMemory();

      void InitMemory()
      {
        if(!init)
        {
          return;
        }
        byte* pointer = (byte*)_native.ToPointer();
        for(var i = 0; i < size; i++)
        {
          pointer[i] = 0;
        }
      }
    }

    public unsafe T this[int index]
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => ((T*)_native)[index];
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      set => ((T*)_native)[index] = value;
    }

    public long Length => _length;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected virtual void Dispose(bool disposing)
    {
      if(_disposed)
      {
        return;
      }

      if(disposing)
      {
        Marshal.FreeHGlobal(_native);
        _native = IntPtr.Zero;
        _length = 0;
      }

      _disposed = true;
    }
  }
}