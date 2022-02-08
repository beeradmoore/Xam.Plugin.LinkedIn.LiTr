using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.AppCompat.App;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.Snackbar;
using Android.Widget;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;
using Xam.Plugin.LinkedIn.LiTr;
using Xam.Plugin.LinkedIn.LiTr.IO;
using Java.Util;
using LiTrExample.Data;
using Xam.Plugin.LinkedIn.LiTr.Filter;
using System.Collections.Generic;
using Android.Graphics;
using Android.Content;
using LiTrExample.Utils;

namespace LiTrExample
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        Context context;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            context = this;

            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            var processButton = FindViewById<Button>(Resource.Id.processButton);
            processButton.Click += ProcessButton_Click;
        }

        private void ProcessButton_Click(object sender, EventArgs e)
        {
            if (sender is Button processButton)
            {
                processButton.Enabled = false;
            }


            /*

          //    : new MediaMuxerMediaTarget(targetMedia.targetFile.getPath(), 1, 0, MediaMuxer.OutputFormat.MUXER_OUTPUT_MPEG_4);
            MediaSource mediaSource = new MediaExtractorMediaSource(this, sourceMedia.uri,  mediaRange);
            List<TrackTransform> trackTransforms = new ArrayList<>(1);

            for (TargetTrack targetTrack : targetMedia.tracks)
            {
                if (targetTrack.format instanceof AudioTrackFormat) {
                AudioTrackFormat trackFormat = (AudioTrackFormat)targetTrack.format;
                MediaFormat mediaFormat = MediaFormat.createAudioFormat(
                        targetMimeType,
                        trackFormat.samplingRate,
                        trackFormat.channelCount);
                mediaFormat.setInteger(MediaFormat.KEY_BIT_RATE, trackFormat.bitrate);
                mediaFormat.setLong(MediaFormat.KEY_DURATION, trackFormat.duration);

                Encoder encoder = targetMedia.writeToWav ? new PassthroughBufferEncoder(8192) : new MediaCodecEncoder();
                TrackTransform trackTransform = new TrackTransform.Builder(mediaSource, targetTrack.sourceTrackIndex, mediaTarget)
                        .setTargetTrack(0)
                        .setDecoder(new MediaCodecDecoder())
                        .setEncoder(encoder)
                        .setRenderer(new AudioRenderer(encoder))
                        .setTargetFormat(mediaFormat)
                        .build();

                trackTransforms.add(trackTransform);


                mediaTransformer.transform(
                    transformationState.requestId,
                    trackTransforms,
                    transformationListener,
                MediaTransformer.GRANULARITY_DEFAULT);
            */

        }


        
        void ApplyWatermark(SourceMedia sourceMedia,
                              TargetMedia targetMedia,
                              TrimConfig trimConfig,
                              TransformationState transformationState)
        {
            if (targetMedia.targetFile.Exists())
            {
                targetMedia.targetFile.Delete();
            }

            transformationState.requestId = new Java.Lang.String(UUID.RandomUUID().ToString());
            MediaTransformationListener transformationListener = new MediaTransformationListener(this,
                    transformationState.requestId,
                    transformationState,
                    targetMedia);

            List<IGlFilter> watermarkImageFilter = null;
            foreach (TargetTrack targetTrack in targetMedia.tracks)
            {
                if (targetTrack is TargetVideoTrack)
                {
                    watermarkImageFilter = createGlFilters(
                        sourceMedia,
                        (TargetVideoTrack)targetTrack,
                        0.2f,
                        new PointF(0.8f, 0.8f),
                        0);
                    break;
                }
            }

            /*
        MediaRange mediaRange = trimConfig.enabled
                ? new MediaRange(
                TimeUnit.MILLISECONDS.toMicros((long)(trimConfig.range.get(0) * 1000)),
                TimeUnit.MILLISECONDS.toMicros((long)(trimConfig.range.get(1) * 1000)))
                : new MediaRange(0, Long.MAX_VALUE);
        TransformationOptions transformationOptions = new TransformationOptions.Builder()
                .setGranularity(MediaTransformer.GRANULARITY_DEFAULT)
                .setVideoFilters(watermarkImageFilter)
                .setSourceMediaRange(mediaRange)
                .build();

        mediaTransformer.transform(
                transformationState.requestId,
                sourceMedia.uri,
                targetMedia.targetFile.getPath(),
                null,
                null,
                transformationListener,
                transformationOptions);
            */


        }


        private List<IGlFilter> createGlFilters(SourceMedia sourceMedia,
                                           TargetVideoTrack targetTrack,
                                           float overlayWidth,
                                           PointF position,
                                           float rotation)
        {
            List<IGlFilter> glFilters = null;
            if (targetTrack != null && targetTrack.overlay != null)
            {
                try
                {
                    Bitmap bitmap = BitmapFactory.DecodeStream(context.ContentResolver.OpenInputStream(targetTrack.overlay));
                    if (bitmap != null)
                    {
                        float overlayHeight;
                        VideoTrackFormat sourceVideoTrackFormat = (VideoTrackFormat)sourceMedia.tracks[targetTrack.sourceTrackIndex];
                        if (sourceVideoTrackFormat.rotation == 90 || sourceVideoTrackFormat.rotation == 270)
                        {
                            float overlayWidthPixels = overlayWidth * sourceVideoTrackFormat.height;
                            float overlayHeightPixels = overlayWidthPixels * bitmap.Height / bitmap.Width;
                            overlayHeight = overlayHeightPixels / sourceVideoTrackFormat.width;
                        }
                        else
                        {
                            float overlayWidthPixels = overlayWidth * sourceVideoTrackFormat.width;
                            float overlayHeightPixels = overlayWidthPixels * bitmap.Height / bitmap.Width;
                            overlayHeight = overlayHeightPixels / sourceVideoTrackFormat.height;
                        }

                        PointF size = new PointF(overlayWidth, overlayHeight);

                        IGlFilter filter = TransformationUtil.createGlFilter(context,
                                                                            targetTrack.overlay,
                                                                            size,
                                                                            position,
                                                                            rotation);
                        if (filter != null)
                        {
                            glFilters = new List<IGlFilter>();
                            glFilters.Add(filter);
                        }
                    }
                }
                catch (Exception err)
                {
                    System.Diagnostics.Debug.WriteLine($"Failed to extract audio track metadata: {err.Message}");
                }
            }

            return glFilters;
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
