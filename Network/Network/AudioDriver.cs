using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;

namespace Network
{
    class AudioDriver
    {

        public Dictionary<string, double> ids = new Dictionary<string, double> {
            // {"abc",1}, {"def",2}, {"ghi",3}
                {"C[0]",   16.35 }, //    |    2109.89    |
                {"C^#[0]/D^b[0]",   17.32 }, //    |    1991.47    |
                {"D[0]",   18.35 }, //    |    1879.69    |
                {"D^#[0]/E^b[0]",   19.45 }, //    |    1774.20    |
                {"E[0]",   20.60 }, //    |    1674.62    |
                {"F[0]",   21.83 }, //    |    1580.63    |
                {"F^#[0]/G^b[0]",   23.12 }, //    |    1491.91    |
                {"G[0]",   24.50 }, //    |    1408.18    |
                {"G^#[0]/A^b[0]",   25.96 }, //    |    1329.14    |
                {"A[0]",   27.50 }, //    |    1254.55    |
                {"A^#[0]/B^b[0]",   29.14 }, //    |    1184.13    |
                {"B[0]",   30.87 }, //    |    1117.67    |
                {"C[1]",   32.70 }, //    |    1054.94    |
                {"C^#[1]/D^b[1]",   34.65 }, //    |    995.73     |
                {"D[1]",   36.71 }, //    |    939.85     |
                {"D^#[1]/E^b[1]",   38.89 }, //    |    887.10     |
                {"E[1]",   41.20 }, //    |    837.31     |
                {"F[1]",   43.65 }, //    |    790.31     |
                {"F^#[1]/G^b[1]",   46.25 }, //    |    745.96     |
                {"G[1]",   49.00 }, //    |    704.09     |
                {"G^#[1]/A^b[1]",   51.91 }, //    |    664.57     |
                {"A[1]",   55.00 }, //    |    627.27     |
                {"A^#[1]/B^b[1]",   58.27 }, //    |    592.07     |
                {"B[1]",   61.74 }, //    |    558.84     |
                {"C[2]",   65.41 }, //    |    527.47     |
                {"C^#[2]/D^b[2]",   69.30 }, //    |    497.87     |
                {"D[2]",   73.42 }, //    |    469.92     |
                {"D^#[2]/E^b[2]",   77.78 }, //    |    443.55     |
                {"E[2]",   82.41 }, //    |    418.65     |
                {"F[2]",   87.31 }, //    |    395.16     |
                {"F^#[2]/G^b[2]",   92.50 }, //    |    372.98     |
                {"G[2]",   98.00 }, //    |    352.04     |
                {"G^#[2]/A^b[2]",   103.83}, //    |    332.29     |
                {"A[2]",   110.00}, //    |    313.64     |
                {"A^#[2]/B^b[2]",   116.54}, //    |    296.03     |
                {"B[2]",   123.47}, //    |    279.42     |
                {"C[3]",   130.81}, //    |    263.74     |
                {"C^#[3]/D^b[3]",   138.59}, //    |    248.93     |
                {"D[3]",   146.83}, //    |    234.96     |
                {"D^#[3]/E^b[3]",   155.56}, //    |    221.77     |
                {"E[3]",   164.81}, //    |    209.33     |
                {"F[3]",   174.61}, //    |    197.58     |
                {"F^#[3]/G^b[3]",   185.00}, //    |    186.49     |
                {"G[3]",   196.00}, //    |    176.02     |
                {"G^#[3]/A^b[3]",   207.65}, //    |    166.14     |
                {"A[3]",   220.00}, //    |    156.82     |
                {"A^#[3]/B^b[3]",   233.08}, //    |    148.02     |
                {"B[3]",   246.94}, //    |    139.71     |
                {"C[4]",   261.63}, //    |    131.87     |
                {"C^#[4]/D^b[4]",   277.18}, //    |    124.47     |
                {"D[4]",   293.66}, //    |    117.48     |
                {"D^#[4]/E^b[4]",   311.13}, //    |    110.89     |
                {"E[4]",   329.63}, //    |    104.66     |
                {"F[4]",   349.23}, //    |     98.79     |
                {"F^#[4]/G^b[4]",   369.99}, //    |     93.24     |
                {"G[4]",   392.00}, //    |     88.01     |
                {"G^#[4]/A^b[4]",   415.30}, //    |     83.07     |
                {"A[4]",   440.00}, //    |     78.41     |
                {"A^#[4]/B^b[4]",   466.16}, //    |     74.01     |
                {"B[4]",   493.88}, //    |     69.85     |
                {"C[5]",   523.25}, //    |     65.93     |
                {"C^#[5]/D^b[5]",   554.37}, //    |     62.23     |
                {"D[5]",   587.33}, //    |     58.74     |
                {"D^#[5]/E^b[5]",   622.25}, //    |     55.44     |
                {"E[5]",   659.25}, //    |     52.33     |
                {"F[5]",   698.46}, //    |     49.39     |
                {"F^#[5]/G^b[5]",   739.99}, //    |     46.62     |
                {"G[5]",   783.99}, //    |     44.01     |
                {"G^#[5]/A^b[5]",   830.61}, //    |     41.54     |
                {"A[5]",   880.00}, //    |     39.20     |
                {"A^#[5]/B^b[5]",   932.33}, //    |     37.00     |
                {"B[5]",   987.77}, //    |     34.93     |
                {"C[6]",  1046.50}, //    |     32.97     |
                {"C^#[6]/D^b[6] ",  1108.73}, //    |     31.12     |
                {"D[6]",  1174.66}, //    |     29.37     |
                {"D^#[6]/E^b[6]",  1244.51}, //    |     27.72     |
                {"E[6]",  1318.51}, //    |     26.17     |
                {"F[6]",  1396.91}, //    |     24.70     |
                {"F^#[6]/G^b[6]",  1479.98}, //    |     23.31     |
                {"G[6]",  1567.98}, //    |     22.00     |
                {"G^#[6]/A^b[6]",  1661.22}, //    |     20.77     |
                {"A[6]",  1760.00}, //    |     19.60     |
                {"A^#[6]/B^b[6]",  1864.66}, //    |     18.50     |
                {"B[6]",  1975.53}, //    |     17.46     |
                {"C[7]",  2093.00}, //    |     16.48     |
                {"C^#[7]/D^b[7]" ,  2217.46}, //    |     15.56     |
                {"D[7]",  2349.32}, //    |     14.69     |
                {"D^#[7]/E^b[7]",  2489.02}, //    |     13.86     |
                {"E[7]",  2637.02}, //    |     13.08     |
                {"F[7]",  2793.83}, //    |     12.35     |
                {"F^#[7]/G^b[7]",  2959.96}, //    |     11.66     |
                {"G[7]",  3135.96}, //    |     11.00     |
                {"G^#[7]/A^b[7]",  3322.44}, //    |     10.38     |
                {"A[7]",  3520.00}, //    |     9.80      |
                {"A^#[7]/B^b[7]",  3729.31}, //    |     9.25      |
                {"B[7]",  3951.07}, //    |     8.73      |
                {"C[8]",  4186.01}, //    |     8.24      |
                {"C^#[8]/D^b[8]",  4434.92}, //    |     7.78      |
                {"D[8]",  4698.63}, //    |     7.34      |
                {"D^#[8]/E^b[8]",  4978.03}, //    |     6.93      |
                {"E[8]      ",  5274.04}, //    |     6.54      |
                {"F[8]",  5587.65}, //    |     6.17      |
                {"F^#[8]/G^b[8]",  5919.91}, //    |     5.83      |
                {"G[8]",  6271.93}, //    |     5.50      |
                {"G^#[8]/A^b[8] ",  6644.88}, //    |     5.19      |
                {"A[8]",  7040.00}, //    |     4.90      |
                {"A^#[8]/B^b[8]",  7458.62}, //    |     4.63      |
                {"B[8]",  7902.13} //    |     4.37      |
        };

        public static void PlayBeep(UInt16 frequency, int msDuration, UInt16 volume = 16383)
        {
            var mStrm = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(mStrm);

            const double TAU = 2 * Math.PI;
            int formatChunkSize = 16;
            int headerSize = 8;
            short formatType = 1;
            short tracks = 1;
            int samplesPerSecond = 44100;
            short bitsPerSample = 16;
            short frameSize = (short)(tracks * ((bitsPerSample + 7) / 8));
            int bytesPerSecond = samplesPerSecond * frameSize;
            int waveSize = 4;
            int samples = (int)((decimal)samplesPerSecond * msDuration / 1000);
            int dataChunkSize = samples * frameSize;
            int fileSize = waveSize + headerSize + formatChunkSize + headerSize + dataChunkSize;
            // var encoding = new System.Text.UTF8Encoding();
            writer.Write(0x46464952); // = encoding.GetBytes("RIFF")
            writer.Write(fileSize);
            writer.Write(0x45564157); // = encoding.GetBytes("WAVE")
            writer.Write(0x20746D66); // = encoding.GetBytes("fmt ")
            writer.Write(formatChunkSize);
            writer.Write(formatType);
            writer.Write(tracks);
            writer.Write(samplesPerSecond);
            writer.Write(bytesPerSecond);
            writer.Write(frameSize);
            writer.Write(bitsPerSample);
            writer.Write(0x61746164); // = encoding.GetBytes("data")
            writer.Write(dataChunkSize);
            {
                double theta = frequency * TAU / (double)samplesPerSecond;
                // 'volume' is UInt16 with range 0 thru Uint16.MaxValue ( = 65 535)
                // we need 'amp' to have the range of 0 thru Int16.MaxValue ( = 32 767)
                double amp = volume >> 2; // so we simply set amp = volume / 2
                for (int step = 0; step < samples; step++)
                {
                    short s = (short)(amp * Math.Sin(theta * (double)step));
                    writer.Write(s);
                }
            }

            mStrm.Seek(0, SeekOrigin.Begin);
            new System.Media.SoundPlayer(mStrm).Play();
            writer.Close();
            mStrm.Close();
        } // public static void PlayBeep(UInt16 frequency, int msDuration, UInt16 volume = 16383)
    }
}
