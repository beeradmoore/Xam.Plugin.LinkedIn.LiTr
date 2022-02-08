using System.Collections.Generic;
using Android.Net;
using AndroidX.DataBinding;
using Java.IO;

namespace LiTrExample.Data
{
    public class TargetMedia : BaseObservable
    {
        public const int DEFAULT_VIDEO_WIDTH = 1280;
        public const int DEFAULT_VIDEO_HEIGHT = 720;
        public const int DEFAULT_VIDEO_BITRATE = 5000000;
        public const int DEFAULT_KEY_FRAME_INTERVAL = 5;
        public const int DEFAULT_AUDIO_BITRATE = 128000;

        public File targetFile;
        private Uri contentUri;

        public List<TargetTrack> tracks = new List<TargetTrack>();
        public Uri backgroundImageUri;
        public Xam.Plugin.LinkedIn.LiTr.Filter.IGlFilter filter;
        public bool writeToWav;


        public void setTracks(List<MediaTrackFormat> sourceTracks)
        {
            tracks = new List<TargetTrack>(sourceTracks.Count);
            foreach (MediaTrackFormat sourceTrackFormat in sourceTracks)
            {
                TargetTrack targetTrack;
                if (sourceTrackFormat is VideoTrackFormat videoTrackFormat)
                {
                    VideoTrackFormat trackFormat = new VideoTrackFormat((VideoTrackFormat)sourceTrackFormat);
                    trackFormat.width = DEFAULT_VIDEO_WIDTH;
                    trackFormat.height = DEFAULT_VIDEO_HEIGHT;
                    trackFormat.bitrate = DEFAULT_VIDEO_BITRATE;
                    trackFormat.keyFrameInterval = DEFAULT_KEY_FRAME_INTERVAL;
                    targetTrack = new TargetVideoTrack(sourceTrackFormat.index, true, false, trackFormat);
                }
                else if (sourceTrackFormat is AudioTrackFormat)
                {
                    AudioTrackFormat trackFormat = new AudioTrackFormat((AudioTrackFormat)sourceTrackFormat);
                    trackFormat.bitrate = DEFAULT_AUDIO_BITRATE;
                    targetTrack = new TargetAudioTrack(sourceTrackFormat.index, true, false, trackFormat);
                }
                else
                {
                    targetTrack = new TargetTrack(sourceTrackFormat.index, true, false, new MediaTrackFormat(sourceTrackFormat));
                }
                tracks.Add(targetTrack);
            }
            NotifyChange();
        }

        public void SetTargetFile(File targetFile)
        {
            this.targetFile = targetFile;
            NotifyChange();
        }


        public int getIncludedTrackCount()
        {
            int trackCount = 0;
            foreach (TargetTrack track in tracks)
            {
                if (track.shouldInclude)
                {
                    trackCount++;
                }
            }
            return trackCount;
        }


        public void setOverlayImageUri(Uri overlayImageUri)
        {
            foreach (TargetTrack targetTrack in tracks)
            {
                if (targetTrack is TargetVideoTrack)
                {
                    ((TargetVideoTrack)targetTrack).overlay = overlayImageUri;
                }
            }
            NotifyChange();
        }

        public Uri getVideoOverlay()
        {
            foreach (TargetTrack targetTrack in tracks)
            {
                if ((targetTrack is TargetVideoTrack) && ((TargetVideoTrack)targetTrack).overlay != null)
                {
                    return ((TargetVideoTrack)targetTrack).overlay;
                }
            }
            return null;
        }


        public Uri getContentUri()
        {
            return contentUri;
        }

        public void setContentUri(Uri storedContentUri)
        {
            this.contentUri = storedContentUri;
            NotifyChange();
        }
    }
}
