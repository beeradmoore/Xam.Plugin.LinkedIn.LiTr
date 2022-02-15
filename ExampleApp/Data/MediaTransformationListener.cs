//using System;
using System.Collections.Generic;
using Android.Content;
using Android.Text;
using Java.Lang;
using LiTrExample.Utils;
using Xam.Plugin.LinkedIn.LiTr;
using Xam.Plugin.LinkedIn.LiTr.Analytics;

namespace LiTrExample.Data
{
    public class MediaTransformationListener : Java.Lang.Object, ITransformationListener
    {
        Context context;
        string requestId;
        TransformationState transformationState;
        SharedMediaStoragePublisher publisher;
        TargetMedia targetMedia;

        public MediaTransformationListener(Context context, string requestId, TransformationState transformationState, TargetMedia targetMedia) : this(context, requestId, transformationState, targetMedia, new SharedMediaStoragePublisher(context))
        {

        }

        public MediaTransformationListener(Context context,
                                   string requestId,
                                   TransformationState transformationState,
                                   TargetMedia targetMedia,
                                   SharedMediaStoragePublisher publisher)
        {
            this.context = context;
            this.requestId = requestId;
            this.transformationState = transformationState;
            this.targetMedia = targetMedia;
            this.publisher = publisher;
        }



        public void OnStarted(string id)
        {
            System.Diagnostics.Debug.WriteLine($"OnStarted: {id}");
            if (TextUtils.Equals(requestId, id))
            {
                transformationState.setState(TransformationState.State.STATE_RUNNING);
            }
        }

        public void OnProgress(string id, float progress)
        {
            System.Diagnostics.Debug.WriteLine($"OnProgress: {id}, {progress}");
            if (TextUtils.Equals(requestId, id))
            {
                transformationState.setProgress((int)(progress * TransformationState.MAX_PROGRESS));
            }
        }

        public void OnCompleted(string id, IList<TrackTransformationInfo> trackTransformationInfos)
        {
            System.Diagnostics.Debug.WriteLine($"OnCompleted: {id}");
            if (TextUtils.Equals(requestId, id))
            {
                transformationState.setState(TransformationState.State.STATE_COMPLETED);
                transformationState.setProgress(TransformationState.MAX_PROGRESS);
                transformationState.setStats(TrackMetadataUtil.printTransformationStats(context, trackTransformationInfos));
                //publisher.publish(targetMedia.targetFile, false, (file, contentUri)->targetMedia.setContentUri(contentUri));
            }
        }


        public void OnCancelled(string id, IList<TrackTransformationInfo> trackTransformationInfos)
        {
            System.Diagnostics.Debug.WriteLine($"OnCancelled: {id}");
            if (TextUtils.Equals(requestId, id))
            {
                transformationState.setState(TransformationState.State.STATE_CANCELLED);
                transformationState.setStats(TrackMetadataUtil.printTransformationStats(context, trackTransformationInfos));
            }
        }

        public void OnError(string id, Throwable cause, IList<TrackTransformationInfo> trackTransformationInfos)
        {
            System.Diagnostics.Debug.WriteLine($"OnError: {id}, {cause.Message}");
            if (TextUtils.Equals(requestId, id))
            {
                transformationState.setState(TransformationState.State.STATE_ERROR);
                transformationState.setStats(TrackMetadataUtil.printTransformationStats(context, trackTransformationInfos));
            }
        }
    }
}
