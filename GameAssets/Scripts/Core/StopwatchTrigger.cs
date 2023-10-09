using System;

namespace Core
{
    public class StopwatchTrigger
    {
        public float TriggerTime;
        public Action TriggerAction;

        public StopwatchTrigger(float triggerTime, Action triggerAction)
        {
            TriggerTime = triggerTime;
            TriggerAction = triggerAction;
        }
    }
}
