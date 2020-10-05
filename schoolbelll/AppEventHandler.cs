using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace schoolbelll
{
    public class AppEventHandler
    {
        public static event EventHandler ScheduleAdded;

        public static void OnScheduleAddedEvent(object sender, EventArgs args)
        {
            ScheduleAdded?.Invoke(sender, args);
        }
    }
}
