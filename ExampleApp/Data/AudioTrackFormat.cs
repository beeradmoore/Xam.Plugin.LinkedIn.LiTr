

using Java.Lang;

namespace LiTrExample.Data
{
    public class AudioTrackFormat : MediaTrackFormat
    {
        public int channelCount;
        public int samplingRate;
        public int bitrate;
        public long duration;

        public AudioTrackFormat(int index, string mimeType) : base(index, mimeType)
        {
            
        }

        public AudioTrackFormat(AudioTrackFormat audioTrackFormat) : base(audioTrackFormat)            
        {
            this.channelCount = audioTrackFormat.channelCount;
            this.samplingRate = audioTrackFormat.samplingRate;
            this.bitrate = audioTrackFormat.bitrate;
            this.duration = audioTrackFormat.duration;
        }
    }
}
