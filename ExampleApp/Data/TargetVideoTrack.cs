

using Android.Net;

namespace LiTrExample.Data
{
    public class TargetVideoTrack : TargetTrack
    {
        public bool shouldApplyOverlay;
        public Uri overlay;


        public TargetVideoTrack(int sourceTrackIndex,
                                bool shouldInclude,
                                bool shouldTranscode,
                                VideoTrackFormat format) : base(sourceTrackIndex, shouldInclude, shouldTranscode, format)
        {
            
        }

        public VideoTrackFormat getTrackFormat()
        {
            return (VideoTrackFormat)format;
        }
    }
}
