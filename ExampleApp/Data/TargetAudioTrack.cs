using System;
namespace LiTrExample.Data
{
    public class TargetAudioTrack : TargetTrack
    {
        public TargetAudioTrack(int sourceTrackIndex,
                         bool shouldInclude,
                         bool shouldTranscode,
                         AudioTrackFormat format) : base(sourceTrackIndex, shouldInclude, shouldTranscode, format)
        {
            
        }

        public AudioTrackFormat getTrackFormat()
        {
            return (AudioTrackFormat)format;
        }
    }
}
