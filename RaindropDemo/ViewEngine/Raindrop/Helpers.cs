using System.Collections.Generic;
using System.Web.Mvc;

namespace Raindrop
{
    partial class Raindrop
    {
        private class Helpers
        {
            /// <summary>
            /// Handles the case where a tag that requires a key is missing it
            /// by either doing nothing or crashing (depending on value of
            /// Settings.MissingKeyFailMode).
            /// </summary>
            /// <param name="key">The key that was missing.</param>
            public static void KeyMissing(string key)
            {
                if (Settings.FailMode == Settings.MissingKeyFailMode.Crash)
                {
                    string msg = string.Format(
                        "Missing key: '{0}'",
                        key);
                    throw new RaindropException(
                        msg,
                        null,
                        0,
                        ErrorCode.MissingKey);
                }
            }

            public static bool Pass(
                ViewDataDictionary data,
                string param)
            {
                // If the element doesn't exist, it's false.
                if (!data.ContainsKey(param))
                {
                    KeyMissing(param);
                }

                // If the element is a bool, it's equal to itself.
                else if (data[param] is bool)
                {
                    return (bool)data[param];
                }

                // If the element is an IEnumerable with contents, it's true.
                else if (data[param] is IEnumerable<ViewDataDictionary>)
                {
                    // IEnumerable doesn't expose a Count variable, so test if it's
                    // populated by trying to enumerate it and return true on
                    // the first element.
                    IEnumerable<ViewDataDictionary> dummy =
                        (IEnumerable<ViewDataDictionary>)data[param];

                    // Loop will be skipped if dummy is empty.
                    foreach (ViewDataDictionary vdd in dummy)
                    {
                        return true;
                    }

                    // The IEnumerable was empty.
                    return false;
                }

                // In all other cases, assume false.
                return false;
            }
        }
    }
}
