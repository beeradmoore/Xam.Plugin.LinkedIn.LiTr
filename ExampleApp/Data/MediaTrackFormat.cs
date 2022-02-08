

using Java.Lang;

namespace LiTrExample.Data
{
    public class MediaTrackFormat
    {
        public int index;
        public String mimeType;

        public MediaTrackFormat(int index, String mimeType)
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
