using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.IO.Ports;
using System.Threading;

public class SerialFader : IDisposable
{
    static SerialPort _serialPort;

    public SerialFader()
    {
        // Create a new SerialPort object with default settings.
        _serialPort = new SerialPort();

        // Allow the user to set the appropriate properties.
        _serialPort.PortName = "COM3";
        _serialPort.BaudRate = 9600;
        _serialPort.Parity = Parity.None;
        _serialPort.DataBits = 8;
        _serialPort.StopBits = StopBits.One;
        _serialPort.Handshake = Handshake.None;

        // Set the read/write timeouts
        _serialPort.ReadTimeout = 500;
        _serialPort.WriteTimeout = 500;

        _serialPort.Open();
        
        



    }

    public void SendCommand(FadeCommand command)
    {

        _serialPort.Write(command.ToArray(), 0, 4);
    }
    public void SendCommand(OneShotCommand command)
    {

        _serialPort.Write(command.ToArray(), 0, 3);
    }

    #region IDisposable Support
    private bool disposedValue = false; // To detect redundant calls

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                _serialPort.Close();
            }

            // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
            // TODO: set large fields to null.

            disposedValue = true;
        }
    }

    // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
    // ~SerialFader() {
    //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
    //   Dispose(false);
    // }

    // This code added to correctly implement the disposable pattern.
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        Dispose(true);
        // TODO: uncomment the following line if the finalizer is overridden above.
        // GC.SuppressFinalize(this);
    }
    #endregion
}
public class FadeCommand
{
    Pins Pin;
    byte Intensity;
    byte Step;
    byte Delay;

    public FadeCommand(Pins pin, byte intensity, byte step, byte delay)
    {
        Pin = pin;
        Intensity = intensity;
        Step = step;
        Delay = delay;

    }

    public byte[] ToArray()
    {

        return new byte[] { Convert.ToByte(Pin), Intensity, Step, Delay };

    }

}

    public class OneShotCommand
    {
        byte Dst;
        byte Step;
        byte Delay;

        public OneShotCommand(byte dst, byte step, byte delay)
        {
            Dst = dst;
            Step = step;
            Delay = delay;

        }

        public byte[] ToArray()
        {

            return new byte[] { Dst, Step, Delay };

        }
    }

public enum Pins { Warm = 9 , Cold = 3}
