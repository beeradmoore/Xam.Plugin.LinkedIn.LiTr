

using AndroidX.DataBinding;
using Java.Lang;

namespace LiTrExample.Data
{
    public class TransformationState : BaseObservable
    {
        public const int MAX_PROGRESS = 100;

        public enum State
        {
            STATE_IDLE = 0,
            STATE_RUNNING = 1,
            STATE_COMPLETED = 3,
            STATE_CANCELLED = 4,
            STATE_ERROR = 5,
        };

        public string requestId;

        public State state;
        public int progress;
        public String stats;

        public TransformationState()
        {
            state = State.STATE_IDLE;
            progress = 0;
            stats = null;
        }

        public void setState(State state)
        {
            this.state = state;
            NotifyChange();
        }

        public void setProgress(int progress)
        {
            this.progress = progress;
            NotifyChange();
        }

        public void setStats(String stats)
        {
            this.stats = stats;
            NotifyChange();
        }
    }
}
