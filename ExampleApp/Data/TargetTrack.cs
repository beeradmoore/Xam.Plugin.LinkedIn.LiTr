using System;
using AndroidX.DataBinding;

namespace LiTrExample.Data
{
    public class TargetTrack : BaseObservable
    {
        public int sourceTrackIndex;
        public bool shouldInclude;
        public bool shouldTranscode;
        public MediaTrackFormat format;

        public TargetTrack(int sourceTrackIndex, bool shouldInclude, bool shouldTranscode, MediaTrackFormat format)
        {
            this.sourceTrackIndex = sourceTrackIndex;
            this.shouldInclude = shouldInclude;
            this.shouldTranscode = shouldTranscode;
            this.format = format;
        }
    }
}
