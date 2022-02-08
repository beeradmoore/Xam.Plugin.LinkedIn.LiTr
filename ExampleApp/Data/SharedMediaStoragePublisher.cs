using System;
using Android.Content;
using Android.OS;
using Java.IO;
using Java.Util.Concurrent;

namespace LiTrExample.Data
{
    public class SharedMediaStoragePublisher
    {
        Context context;
        IExecutorService executor;
        Handler handler;

        public SharedMediaStoragePublisher(Context context)
        {
            this.context = context;
            executor = Executors.NewSingleThreadExecutor();
            handler = Handler.CreateAsync(Looper.MainLooper);
        }


        public void pubilsh(File file, bool keepOriginal, MediaPublishedListener listener)
        {
            /*
            executor.execute {
                val contentUri = storeVideoInternal(file, keepOriginal)
                handler.post {
                        listener.onPublished(file, contentUri)
                }
            }
            */
        }

        /*
         * class SharedMediaStoragePublisher @JvmOverloads constructor(
  
        ) {

    private val resolver: ContentResolver
        get() = context.contentResolver

   

    @WorkerThread
    private fun storeVideoInternal(file: File, keepOriginal: Boolean): Uri? {
        if (!file.exists()) return null

        val newFileName = file.name

        val values = ContentValues().apply {
            put(MediaStore.Video.Media.MIME_TYPE, MIME_TYPE_MPEG_4)
            put(MediaStore.Video.Media.TITLE, newFileName)
            put(MediaStore.Video.Media.DISPLAY_NAME, newFileName)
            put(MediaStore.Video.Media.DATE_TAKEN, System.currentTimeMillis())
            if (isAndroidQ) {
                put(MediaStore.Video.Media.RELATIVE_PATH, relativePathMovies)
                put(MediaStore.Video.Media.IS_PENDING, 1)
            }
        }


        val collectionUrl = if (isAndroidQ) {
            MediaStore.Video.Media.getContentUri(MediaStore.VOLUME_EXTERNAL_PRIMARY)
        } else {
            MediaStore.Video.Media.EXTERNAL_CONTENT_URI
        }

        var contentUri: Uri? = null

        runCatching {
            resolver.insert(collectionUrl, values)?.also { insertedUri ->
                contentUri = insertedUri
                resolver.openOutputStream(insertedUri)?.use { output ->
                    FileInputStream(file).use { input ->
                        input.copyTo(output)
                    }
                } ?: throw IOException("Failed to copy video to output stream")
            } ?: throw IOException("Failed to insert video to MediaStore")

        }.onFailure {
            Log.e(TAG, "Error copying file to MediaStore", it)
            contentUri?.let { copyFailedUri ->
                resolver.delete(copyFailedUri, null, null)
            }
        }

        if (!keepOriginal) {
            runCatching { file.delete() }.onFailure {
                Log.e(TAG, "Unable to delete original video file $file from system", it)
            }
        }

        if (isAndroidQ) {
            contentUri?.let {
                // Since we're done writing to the Uri, this tells MediaStore that other apps can use the content now.
                values.clear()
                values.put(MediaStore.Video.Media.IS_PENDING, 0)
                runCatching {
                    resolver.update(it, values, null, null)
                }.onFailure {
                    Log.e(TAG, "Could not update MediaStore for $file", it)
                }
            }
        }

        return contentUri
    }
        */

        public interface MediaPublishedListener
        {
            public void onPublished(File file, Uri contentUri);
        }

        /*
    companion object {
        private val TAG = SharedMediaStoragePublisher::class.qualifiedName
        private const val MIME_TYPE_MPEG_4 = "video/mp4"

        private val isAndroidQ
            @ChecksSdkIntAtLeast(api = Build.VERSION_CODES.Q)
            get() = Build.VERSION.SDK_INT >= Build.VERSION_CODES.Q


        private val moviesDirectory = Environment.DIRECTORY_MOVIES
        private val litrDirectory = "LiTr"
        private val relativePathMovies = "${moviesDirectory}/${litrDirectory}"

    }
}
        */
    }
}
