

using Java.Lang;

namespace LiTrExample.Data
{
    public class MediaTrackFormat
    {
        public int index;
        public string mimeType;

        public MediaTrackFormat(int index, string mimeType)
        {
            this.index = index;
            this.mimeType = mimeType;
        }

        public MediaTrackFormat(MediaTrackFormat mediaTrackFormat)
        {
            this.index = mediaTrackFormat.index;
            this.mimeType = mediaTrackFormat.mimeType;
        }
    }
}
