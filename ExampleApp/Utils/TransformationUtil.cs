﻿


using Android.Content;
using Android.Graphics;
using Android.Net;
using Android.OS;
using Android.Provider;
using Android.Text;
using Java.IO;
using Xam.Plugin.LinkedIn.LiTr.Filter;
using Xam.Plugin.LinkedIn.LiTr.Filter.Video.GL;

namespace LiTrExample.Utils
{
    public class TransformationUtil
    {
        private TransformationUtil()
        {
        }

        public static IGlFilter createGlFilter(Context context, Uri overlayUri, PointF size, PointF position, float rotation)
        {
            IGlFilter filter = null;

            Transform transform = new Transform(size, position, rotation);

            try
            {
                if (TextUtils.Equals(context.ContentResolver.GetType(overlayUri), "image/gif"))
                {
                    //IAnimationFrameProvider x;
                    /*
                    ContentResolver contentResolver = context.ApplicationContext.ContentResolver;
                    InputStream inputStream = contentResolver.OpenInputStream(overlayUri);
                    BitmapPool bitmapPool = new LruBitmapPool(10);
                    GifBitmapProvider gifBitmapProvider = new GifBitmapProvider(bitmapPool);
                    GifDecoder gifDecoder = new StandardGifDecoder(gifBitmapProvider);
                    gifDecoder.read(inputStream, (int)TranscoderUtils.getSize(context, overlayUri));

                    AnimationFrameProvider animationFrameProvider = new AnimationFrameProvider() {
                        @Override
                        public int getFrameCount()
                        {
                            return gifDecoder.getFrameCount();
                        }

                        @Nullable
                        @Override
                            public Bitmap getNextFrame()
                        {
                            return gifDecoder.getNextFrame();
                        }

                        @Override
                            public long getNextFrameDurationNs()
                        {
                            return TimeUnit.MILLISECONDS.toNanos(gifDecoder.getNextDelay());
                        }

                        @Override
                            public void advance()
                        {
                            gifDecoder.advance();
                        }
                    };
                    filter = new FrameSequenceAnimationOverlayFilter(animationFrameProvider, transform);
                    */
                }
                else
                {
                    filter = new BitmapOverlayFilter(context.ApplicationContext, overlayUri, transform);
                }

            }
            catch (System.Exception err)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to create a GlFilter: {err.Message}");
            }

            return filter;
        }

        public static File GetTargetFileDirectory(Context context)
        {
            return context.GetExternalFilesDir(Environment.DirectoryMovies);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                return context.NoBackupFilesDir;
            }
            else
            {
                return context.FilesDir;
            }
        }

        public static string GetDisplayName(Context context,  Uri uri)
        {
            var name = SystemClock.ElapsedRealtime().ToString();

            var projection = new string[] { MediaStore.IMediaColumns.DisplayName };
            var cursor = context.ContentResolver.Query(uri, projection, null, null, null);
            if (cursor != null)
            {
                if (cursor.MoveToFirst())
                {
                    name = cursor.GetString(0);
                }
                cursor.Close();
            }

            return name;
        }
    }
}
