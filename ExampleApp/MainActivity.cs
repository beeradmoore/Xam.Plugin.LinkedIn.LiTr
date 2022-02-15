//using System;
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
using Java.IO;
using Xam.Plugin.LinkedIn.LiTr.Utils;
using Android.Net;
using Android.Media;
using Java.Util.Concurrent;
using Xamarin.Essentials;
using Com.Google.Android.Exoplayer2.UI;
using Xam.Plugin.LinkedIn.LiTr.Analytics;
using Com.Google.Android.Exoplayer2;
using Xam.Plugin.LinkedIn.LiTr.Codec;
using Xam.Plugin.LinkedIn.LiTr.Render;

namespace LiTrExample
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, ITransformationListener
    {
        Context context;
        SourceMedia _sourceMedia;
        TargetMedia _targetMedia;
        TransformationState _transformationState;
        TrimConfig _trimConfig;
        MediaTransformer _mediaTransformer;
        PlayerView _playerView;
        SimpleExoPlayer _player;

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

            _playerView = FindViewById<PlayerView>(Resource.Id.playerView);

            _sourceMedia = new SourceMedia();
            _targetMedia = new TargetMedia();
            _transformationState = new TransformationState();
            _trimConfig = new TrimConfig();

            _mediaTransformer = new MediaTransformer(this);


            var builder = new SimpleExoPlayer.Builder(this);
            _player = builder.Build();

            _player.PlayWhenReady = true;
            _playerView.Player = _player;
        }

        private async void ProcessButton_Click(object sender, System.EventArgs e)
        {
            if (sender is Button processButton)
            {
                processButton.Enabled = false;
            }

            Uri uri = null;
            try
            {
                var video = await MediaPicker.PickVideoAsync();
                var sourceFile = new File(video.FullPath);
                uri = Android.Net.Uri.FromFile(sourceFile);
                _sourceMedia.uri = uri;
                System.Diagnostics.Debug.WriteLine($"PickVideoAsync COMPLETED: {video.FullPath}");
            }
            catch (FeatureNotSupportedException err)
            {
                // Feature is not supported on the device
                System.Diagnostics.Debug.WriteLine($"PickVideoAsync FeatureNotSupportedException: {err.Message}");

            }
            catch (PermissionException err)
            {
                // Permissions not granted
                System.Diagnostics.Debug.WriteLine($"PickVideoAsync THREW PermissionException: {err.Message}");

            }
            catch (System.Exception err)
            {
                System.Diagnostics.Debug.WriteLine($"PickVideoAsync THREW: {err.Message}");
            }


            //            updateSourceMedia(sourceMedia, uri);
            //           updateTrimConfig(binding.getTrimConfig(), sourceMedia);
            var targetFile = new File(TransformationUtil.GetTargetFileDirectory(ApplicationContext), "transcoded_" + TransformationUtil.GetDisplayName(this, _sourceMedia.uri));
            //var targetFile = new File(FileSystem.CacheDirectory, "transcoded_" + TransformationUtil.GetDisplayName(this, _sourceMedia.uri) + ".mp4");

            // picked media
            updateSourceMedia(_sourceMedia, uri);


            //updateTrimConfig(_trimConfig, _sourceMedia);
            _trimConfig.setTrimEnd(_sourceMedia.duration * 0.5f);
            _trimConfig.setEnabled(true);

            _targetMedia.SetTargetFile(targetFile);
            _targetMedia.setTracks(_sourceMedia.tracks);
            _targetMedia.writeToWav = true;

            //ApplyWatermark(_sourceMedia, _targetMedia, _trimConfig, _transformationState);

            TranscodeAudio(_sourceMedia, _targetMedia, _trimConfig, _transformationState);

        }

        protected void updateSourceMedia(SourceMedia sourceMedia, Android.Net.Uri uri)
        {
            sourceMedia.uri = uri;
            sourceMedia.size = TranscoderUtils.GetSize(this, uri);
            sourceMedia.duration = getMediaDuration(uri) / 1000f;

            try
            {
                MediaExtractor mediaExtractor = new MediaExtractor();
                mediaExtractor.SetDataSource(this, uri, null);
                sourceMedia.tracks = new List<MediaTrackFormat>(mediaExtractor.TrackCount);

                for (int track = 0; track < mediaExtractor.TrackCount; track++)
                {
                    MediaFormat mediaFormat = mediaExtractor.GetTrackFormat(track);
                    var mimeType = mediaFormat.GetString(MediaFormat.KeyMime);
                    if (mimeType == null)
                    {
                        continue;
                    }

                    if (mimeType.StartsWith("video"))
                    {
                        VideoTrackFormat videoTrack = new VideoTrackFormat(track, mimeType);
                        videoTrack.width = getInt(mediaFormat, MediaFormat.KeyWidth);
                        videoTrack.height = getInt(mediaFormat, MediaFormat.KeyHeight);
                        videoTrack.duration = getLong(mediaFormat, MediaFormat.KeyDuration);
                        videoTrack.frameRate = MediaFormatUtils.GetFrameRate(mediaFormat, new Java.Lang.Integer(-1)).IntValue();
                        videoTrack.keyFrameInterval = MediaFormatUtils.GetIFrameInterval(mediaFormat, new Java.Lang.Integer(-1)).IntValue();
                        videoTrack.rotation = getInt(mediaFormat, TrackMetadataUtil.KEY_ROTATION, 0);
                        videoTrack.bitrate = getInt(mediaFormat, MediaFormat.KeyBitRate);
                        sourceMedia.tracks.Add(videoTrack);
                    }
                    else if (mimeType.StartsWith("audio"))
                    {
                        AudioTrackFormat audioTrack = new AudioTrackFormat(track, mimeType);
                        audioTrack.channelCount = getInt(mediaFormat, MediaFormat.KeyChannelCount);
                        audioTrack.samplingRate = getInt(mediaFormat, MediaFormat.KeySampleRate);
                        audioTrack.duration = getLong(mediaFormat, MediaFormat.KeyDuration);
                        audioTrack.bitrate = getInt(mediaFormat, MediaFormat.KeyBitRate);
                        sourceMedia.tracks.Add(audioTrack);
                    }
                    else
                    {
                        sourceMedia.tracks.Add(new GenericTrackFormat(track, mimeType));
                    }
                }
            }
            catch (IOException ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to extract sourceMedia: {ex.Message}");
            }

            sourceMedia.NotifyChange();
        }

        private long getMediaDuration(Uri uri)
        {
            var mediaMetadataRetriever = new MediaMetadataRetriever();
            mediaMetadataRetriever.SetDataSource(this, uri);
            var durationStr = mediaMetadataRetriever.ExtractMetadata(MetadataKey.Duration);
            return long.Parse(durationStr);
        }

        private int getInt(MediaFormat mediaFormat, string key)
        {
            return getInt(mediaFormat, key, -1);
        }

        private int getInt(MediaFormat mediaFormat, string key, int defaultValue)
        {
            if (mediaFormat.ContainsKey(key))
            {
                return mediaFormat.GetInteger(key);
            }
            return defaultValue;
        }

        private long getLong(MediaFormat mediaFormat, string key)
        {
            if (mediaFormat.ContainsKey(key))
            {
                return mediaFormat.GetLong(key);
            }
            return -1;
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

            transformationState.requestId = UUID.RandomUUID().ToString();

            /*
            MediaTransformationListener transformationListener = new MediaTransformationListener(this,
                    transformationState.requestId,
                    transformationState,
                    targetMedia);
            */

            var resourceId = Resource.Drawable.test_image_1;
            var watermarkUri = new Uri.Builder()
                .Scheme(ContentResolver.SchemeAndroidResource)
                .Authority(Resources.GetResourcePackageName(resourceId))
                .AppendPath(Resources.GetResourceTypeName(resourceId))
                .AppendPath(Resources.GetResourceEntryName(resourceId))
                .Build();



            List<IGlFilter> watermarkImageFilter = null;
            foreach (TargetTrack targetTrack in targetMedia.tracks)
            {

                if (targetTrack is TargetVideoTrack targetVideoTrack) 
                {
                    targetVideoTrack.overlay = watermarkUri;

                    watermarkImageFilter = createGlFilters(
                        sourceMedia,
                        (TargetVideoTrack)targetTrack,
                        0.2f,
                        new PointF(0.8f, 0.8f),
                        0);
                    break;
                }
            }

            
            MediaRange mediaRange = trimConfig.enabled
                    ? new MediaRange(
                    TimeUnit.Milliseconds.ToMicros((long)(trimConfig.range[0] * 1000)),
                    TimeUnit.Milliseconds.ToMicros((long)(trimConfig.range[1] * 1000)))
                    : new MediaRange(0, long.MaxValue);
            TransformationOptions transformationOptions = new TransformationOptions.Builder()
                    .SetGranularity(MediaTransformer.GranularityDefault)
                    .SetVideoFilters(watermarkImageFilter)
                    .SetSourceMediaRange(mediaRange)
                    .Build();

            _mediaTransformer.Transform(
                    _transformationState.requestId,
                    _sourceMedia.uri,
                    targetMedia.targetFile.Path,
                    null,
                    null,
                    this,
                    transformationOptions);
      

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
                catch (System.Exception err)
                {
                    System.Diagnostics.Debug.WriteLine($"Failed to extract audio track metadata: {err.Message}");
                }
            }

            return glFilters;
        }


        public void TranscodeAudio(SourceMedia sourceMedia,
                              TargetMedia targetMedia,
                              TrimConfig trimConfig,
                              TransformationState transformationState)
        {


            if (targetMedia.targetFile.Exists())
            {
                targetMedia.targetFile.Delete();
            }

            transformationState.requestId = UUID.RandomUUID().ToString();

            /*
            MediaTransformationListener transformationListener = new MediaTransformationListener(context,
                    transformationState.requestId,
                    transformationState,
                    targetMedia);
            */

            MediaRange mediaRange = trimConfig.enabled
                    ? new MediaRange(
                    TimeUnit.Milliseconds.ToMicros((long)(trimConfig.range[0] * 1000)),
                    TimeUnit.Milliseconds.ToMicros((long)(trimConfig.range[1] * 1000)))
                    : new MediaRange(0, long.MaxValue);

            try
            {
                var targetMimeType = targetMedia.writeToWav ? "audio/raw" : "audio/mp4a-latm";
                IMediaTarget mediaTarget;

                if (targetMedia.writeToWav)
                {
                    mediaTarget = new WavMediaTarget(targetMedia.targetFile.Path);
                }
                else
                {
                    mediaTarget = new MediaMuxerMediaTarget(targetMedia.targetFile.Path, 1, 0, (int)MuxerOutputType.Mpeg4);
                }
                var mediaSource = new MediaExtractorMediaSource(context, sourceMedia.uri, mediaRange);
                List<TrackTransform> trackTransforms = new List<TrackTransform>(1);

                foreach (TargetTrack targetTrack in targetMedia.tracks)
                { 
                    if (targetTrack.format is AudioTrackFormat trackFormat)
                    {
                        MediaFormat mediaFormat = MediaFormat.CreateAudioFormat(
                                targetMimeType,
                                trackFormat.samplingRate,
                                trackFormat.channelCount);
                        mediaFormat.SetInteger(MediaFormat.KeyBitRate, trackFormat.bitrate);
                        mediaFormat.SetLong(MediaFormat.KeyDuration, trackFormat.duration);

                        IEncoder encoder;
                        if (targetMedia.writeToWav)
                        {
                            encoder = new PassthroughBufferEncoder(8192);
                        }
                        else
                        {
                            encoder = new MediaCodecEncoder();
                        }
                        //IEncoder encoder = targetMedia.writeToWav ? new PassthroughBufferEncoder(8192) : new MediaCodecEncoder();
                        TrackTransform trackTransform = new TrackTransform.Builder(mediaSource, targetTrack.sourceTrackIndex, mediaTarget)
                                .SetTargetTrack(0)
                                .SetDecoder(new MediaCodecDecoder())
                                .SetEncoder(encoder)
                                .SetRenderer(new AudioRenderer(encoder))
                                .SetTargetFormat(mediaFormat)
                                .Build();


                        trackTransforms.Add(trackTransform);
                        break;
                    }
                }

                _mediaTransformer.Transform(
                    transformationState.requestId,
                    trackTransforms,
                    this,
                    MediaTransformer.GranularityDefault);
            }
            catch (System.Exception err)
            {
                System.Diagnostics.Debug.WriteLine($"Exception when trying to transcode audio: {err.Message}");
            }
        }



        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        #region ITransformationListener
        public void OnCancelled(string id, IList<TrackTransformationInfo> trackTransformationInfos)
        {
            System.Diagnostics.Debug.WriteLine($"OnCancelled: {id}");
        }

        public void OnCompleted(string id, IList<TrackTransformationInfo> trackTransformationInfos)
        {
            System.Diagnostics.Debug.WriteLine($"OnCompleted: {id}");

            var uri = Uri.FromFile(_targetMedia.targetFile);

            /*
            await Share.RequestAsync(new ShareTextRequest
            {
                Uri = _targetMedia.targetFile.AbsolutePath,
                Title = "Share Created Video"
            });
            */

            var progressiveMediaSource = new Com.Google.Android.Exoplayer2.Source.ProgressiveMediaSource.Factory(new Com.Google.Android.Exoplayer2.Upstream.DefaultDataSourceFactory(this, "Exoplayer-local"));
            var mediaSource = progressiveMediaSource.CreateMediaSource(uri);
            _player.Prepare(mediaSource);
        }

        public void OnError(string id, Java.Lang.Throwable cause, IList<TrackTransformationInfo> trackTransformationInfos)
        {
            System.Diagnostics.Debug.WriteLine($"OnError: {id}, {cause.Message}");
        }

        public void OnProgress(string id, float progress)
        {
            System.Diagnostics.Debug.WriteLine($"OnProgress: {id}, {progress}");
        }

        public void OnStarted(string id)
        {
            System.Diagnostics.Debug.WriteLine($"OnStarted: {id}");
        }
        #endregion
    }
}
