

using System.Collections.Generic;
using AndroidX.DataBinding;

namespace LiTrExample.Data
{
    public class TrimConfig : BaseObservable
    {
        /*
        public final RangeSlider.OnChangeListener onValueChangeListener = (slider, value, fromUser)-> {
            range = slider.getValues();
        };
        */

        public bool enabled;
        public List<float> range = new List<float>(2);

        public TrimConfig()
        {
            range.Add(0f);
            range.Add(1f);
        }

        public bool getEnabled()
        {
            return enabled;
        }

        /*
        @BindingAdapter(value = "onChangeListener")
        public static void setOnChangeListener(RangeSlider rangeSlider, RangeSlider.OnChangeListener onChangeListener)
        {
            rangeSlider.addOnChangeListener(onChangeListener);
        }
        */

        public void setEnabled(bool enabled)
        {
            this.enabled = enabled;
            NotifyChange();
        }

        public void setTrimEnd(float trimEnd)
        {
            range[1] = trimEnd;
            NotifyChange();
        }
    }
}
