using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Media;
using Android.OS;
using Xam.Plugin.LinkedIn.LiTr.Analytics;
using System;
using System.Text;

namespace LiTrExample.Utils
{
    public class TrackMetadataUtil
    {
        /*
        private const String TAG = TrackMetadataUtil.class.getSimpleName();
           */
        private static string KEY_ROTATION => Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.M ? MediaFormat.KeyRotation : "rotation-degrees";
     


        public static Java.Lang.String printTransformationStats(Context context, IList<TrackTransformationInfo> stats)
        {
            if (stats == null || stats.Any() == false)
            {
                return new Java.Lang.String(context.GetString(Resource.String.no_transformation_stats));
            }

            StringBuilder statsStringBuilder = new StringBuilder();
            for (int track = 0; track < stats.Count; track++)
            {
                statsStringBuilder.AppendLine(context.GetString(Resource.String.stats_track, track));

                TrackTransformationInfo trackTransformationInfo = stats[track];
                MediaFormat sourceFormat = trackTransformationInfo.SourceFormat;
                String mimeType = null;
                if (sourceFormat.ContainsKey(MediaFormat.KeyMime))
                {
                    mimeType = sourceFormat.GetString(MediaFormat.KeyMime);
                }
                if (mimeType != null && mimeType.StartsWith("video"))
                {
                    statsStringBuilder.AppendLine(context.GetString(Resource.String.stats_source_format))
                                      .AppendLine(printVideoMetadata(context, sourceFormat))
                                      .AppendLine(context.GetString(Resource.String.stats_target_format))
                                      .AppendLine(printVideoMetadata(context, trackTransformationInfo.TargetFormat));
                }
                else if (mimeType != null && mimeType.StartsWith("audio"))
                {
                    statsStringBuilder.AppendLine(context.GetString(Resource.String.stats_source_format))
                                      .AppendLine(printAudioMetadata(context, sourceFormat))
                                      .AppendLine(context.GetString(Resource.String.stats_target_format))
                                      .AppendLine(printAudioMetadata(context, trackTransformationInfo.TargetFormat));
                }
                else if (mimeType != null && mimeType.StartsWith("image"))
                {
                    statsStringBuilder.AppendLine(context.GetString(Resource.String.stats_source_format))
                                      .AppendLine(printImageMetadata(context, sourceFormat))
                                      .AppendLine(context.GetString(Resource.String.stats_target_format))
                                      .AppendLine(printImageMetadata(context, trackTransformationInfo.TargetFormat));
                }
                else
                {
                    statsStringBuilder.AppendLine(context.GetString(Resource.String.stats_source_format))
                                      .AppendLine(printGenericMetadata(context, sourceFormat))
                                      .AppendLine(context.GetString(Resource.String.stats_target_format))
                                      .AppendLine(printGenericMetadata(context, trackTransformationInfo.TargetFormat));
                }
                statsStringBuilder.AppendLine(context.GetString(Resource.String.stats_decoder, trackTransformationInfo.DecoderCodec))
                                  .AppendLine(context.GetString(Resource.String.stats_encoder, trackTransformationInfo.EncoderCodec))
                                  .AppendLine(context.GetString(Resource.String.stats_transformation_duration, trackTransformationInfo.Duration))
                                  .AppendLine("\n");
            }
            return new Java.Lang.String(statsStringBuilder.ToString());
        }


        
        private static String printVideoMetadata(Context context, MediaFormat mediaFormat)
        {
            if (mediaFormat == null)
            {
                return "\n";
            }
            StringBuilder stringBuilder = new StringBuilder();
            if (mediaFormat.ContainsKey(MediaFormat.KeyMime))
            {
                stringBuilder.AppendLine(context.GetString(Resource.String.stats_mime_type, mediaFormat.GetString(MediaFormat.KeyMime)));
            }
            if (mediaFormat.ContainsKey(MediaFormat.KeyWidth))
            {
                stringBuilder.AppendLine(context.GetString(Resource.String.stats_width, mediaFormat.GetInteger(MediaFormat.KeyWidth)));
            }
            if (mediaFormat.ContainsKey(MediaFormat.KeyHeight))
            {
                stringBuilder.AppendLine(context.GetString(Resource.String.stats_height, mediaFormat.GetInteger(MediaFormat.KeyHeight)));
            }
            if (mediaFormat.ContainsKey(MediaFormat.KeyBitRate))
            {
                stringBuilder.AppendLine(context.GetString(Resource.String.stats_bitrate, mediaFormat.GetInteger(MediaFormat.KeyBitRate)));
            }
            if (mediaFormat.ContainsKey(MediaFormat.KeyDuration))
            {
                stringBuilder.AppendLine(context.GetString(Resource.String.stats_duration, mediaFormat.GetLong(MediaFormat.KeyDuration)));
            }
            if (mediaFormat.ContainsKey(MediaFormat.KeyFrameRate))
            {
                stringBuilder.AppendLine(context.GetString(Resource.String.stats_frame_rate, MediaFormatUtils.getFrameRate(mediaFormat, new Java.Lang.Integer(0)).IntValue()));
            }
            if (mediaFormat.ContainsKey(MediaFormat.KeyIFrameInterval))
            {
                stringBuilder.AppendLine(context.GetString(Resource.String.stats_key_frame_interval, MediaFormatUtils.getIFrameInterval(mediaFormat, new Java.Lang.Integer(0)).IntValue()));
            }
            if (mediaFormat.ContainsKey(KEY_ROTATION))
            {
                stringBuilder.AppendLine(context.GetString(Resource.String.stats_rotation, mediaFormat.GetInteger(KEY_ROTATION)));
            }
            return stringBuilder.ToString();
        }


        private static String printAudioMetadata(Context context, MediaFormat mediaFormat)
        {
            if (mediaFormat == null)
            {
                return "\n";
            }
            StringBuilder stringBuilder = new StringBuilder();
            if (mediaFormat.ContainsKey(MediaFormat.KeyMime))
            {
                stringBuilder.AppendLine(context.GetString(Resource.String.stats_mime_type, mediaFormat.GetString(MediaFormat.KeyMime)));
            }
            if (mediaFormat.ContainsKey(MediaFormat.KeyChannelCount))
            {
                stringBuilder.AppendLine(context.GetString(Resource.String.stats_channel_count, mediaFormat.GetInteger(MediaFormat.KeyChannelCount)));
            }
            if (mediaFormat.ContainsKey(MediaFormat.KeyBitRate))
            {
                stringBuilder.AppendLine(context.GetString(Resource.String.stats_bitrate, mediaFormat.GetInteger(MediaFormat.KeyBitRate)));
            }
            if (mediaFormat.ContainsKey(MediaFormat.KeyDuration))
            {
                stringBuilder.AppendLine(context.GetString(Resource.String.stats_duration, mediaFormat.GetLong(MediaFormat.KeyDuration)));
            }
            if (mediaFormat.ContainsKey(MediaFormat.KeySampleRate))
            {
                stringBuilder.AppendLine(context.GetString(Resource.String.stats_sampling_rate, mediaFormat.GetInteger(MediaFormat.KeySampleRate)));
            }
            return stringBuilder.ToString();
        }


        private static String printImageMetadata(Context context, MediaFormat mediaFormat)
        {
            if (mediaFormat == null)
            {
                return "\n";
            }
            StringBuilder stringBuilder = new StringBuilder();
            if (mediaFormat.ContainsKey(MediaFormat.KeyMime))
            {
                stringBuilder.AppendLine(context.GetString(Resource.String.stats_mime_type, mediaFormat.GetString(MediaFormat.KeyMime)));
            }
            if (mediaFormat.ContainsKey(MediaFormat.KeyWidth))
            {
                stringBuilder.AppendLine(context.GetString(Resource.String.stats_width, mediaFormat.GetInteger(MediaFormat.KeyWidth)));
            }
            if (mediaFormat.ContainsKey(MediaFormat.KeyHeight))
            {
                stringBuilder.AppendLine(context.GetString(Resource.String.stats_height, mediaFormat.GetInteger(MediaFormat.KeyHeight)));
            }
            return stringBuilder.ToString();
        }


        private static String printGenericMetadata(Context context, MediaFormat mediaFormat)
        {
            if (mediaFormat == null)
            {
                return "\n";
            }
            if (mediaFormat.ContainsKey(MediaFormat.KeyMime))
            {
                return context.GetString(Resource.String.stats_mime_type, mediaFormat.GetString(MediaFormat.KeyMime));
            }
            return "\n";
        }
    }
}
