

using System.Collections.Generic;
using Android.Net;
using AndroidX.DataBinding;

namespace LiTrExample.Data
{
    public class SourceMedia : BaseObservable
    {
        public Uri uri;
        public long size;
        public float duration;

        public List<MediaTrackFormat> tracks = new List<MediaTrackFormat>();
    }
}
