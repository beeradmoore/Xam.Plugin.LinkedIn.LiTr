using System;
using Android.Media;
using Android.OS;
using Java.Lang;

namespace LiTrExample.Utils
{
    public class MediaFormatUtils
    {

        internal static Number getIFrameInterval(MediaFormat format, Number defaultValue)
        {
            return getNumber(format, MediaFormat.KeyIFrameInterval) ?? defaultValue;
        }

        internal static Number getFrameRate(MediaFormat format, Number defaultValue)
        {
            return getNumber(format, MediaFormat.KeyFrameRate) ?? defaultValue;
        }


        internal static Number getNumber(MediaFormat format, string key)
        {
            if (format.ContainsKey(key))
            {
                if (Build.VERSION.SdkInt >= BuildVersionCodes.Q)
                {
                    return format.GetNumber(key);
                }

                try
                {
                    return (Number)format.GetInteger(key);
                }
                catch (System.Exception )
                {
                    try
                    {
                        return (Number)format.GetFloat(key);
                    }
                    catch (System.Exception )
                    {

                    }
                }
            }
            return null;
        }


        /*
        return when
        {
            !format.containsKey(key)-> null
                Build.VERSION.SDK_INT >= Build.VERSION_CODES.Q->format.getNumber(key)
                else ->runCatching
                {
                    format.getInteger(key)
                }
                .recoverCatching
                {
                    format.getFloat(key)
                }.getOrNull()
            }
        }
        */

    }
}
