using Java.Lang;

namespace LiTrExample.Data
{
    public class VideoTrackFormat : MediaTrackFormat
    {
        public int width;
        public int height;
        public int bitrate;
        public int frameRate;
        public int keyFrameInterval;
        public long duration;
        public int rotation;


        public VideoTrackFormat(int index, string mimeType) : base(index, mimeType)
        {

        }

        public VideoTrackFormat(VideoTrackFormat videoTrackFormat) : base(videoTrackFormat)
        {
            this.width = videoTrackFormat.width;
            this.height = videoTrackFormat.height;
            this.bitrate = videoTrackFormat.bitrate;
            this.frameRate = videoTrackFormat.frameRate;
            this.keyFrameInterval = videoTrackFormat.keyFrameInterval;
            this.duration = videoTrackFormat.duration;
            this.rotation = videoTrackFormat.rotation;
        }
    }
}
