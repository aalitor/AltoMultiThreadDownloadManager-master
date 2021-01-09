

using System;
using System.ComponentModel;

namespace AltoMultiThreadDownloadManager.Helpers
{
    /// <summary>
    /// Provides method to raise event
    /// </summary>
    internal static class EventHelper
    {
        /// <summary>
        /// Raises event if not null
        /// </summary>
        /// <typeparam name="T">Event arguments class derived from EventArgs</typeparam>
        /// <param name="ev">Event handler to raise</param>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event arguments class object</param>
        public static void Raise<T>(this EventHandler<T> ev, object sender, T e) where T : EventArgs
        {

            if (ev == null)
                return;
            ev(sender, e);

        }
        public static void Raise<T>(this EventHandler<T> ev, object sender, T e, AsyncOperation aop) where T : EventArgs
        {
            if (aop != null)
            {
                aop.Post(new System.Threading.SendOrPostCallback(delegate
                    {
                        ev.Raise(sender, e);
                    }), null);
            }
        }
    }
}
