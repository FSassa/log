using System;

using log4net.Core;
using log4net.Filter;

namespace SSTI.Log.Log4Net.MongoDB.Filters
{
    public class TypeFilter : FilterSkeleton
    {
        public Type Type { get; set; }

        public override FilterDecision Decide(LoggingEvent loggingEvent)
        {
            if (null == loggingEvent)
            {
                throw new ArgumentNullException(nameof(loggingEvent));
            }

            if (Type == loggingEvent.MessageObject.GetType())
            {
                return FilterDecision.Accept;
            }

            return FilterDecision.Deny;
        }
    }
}