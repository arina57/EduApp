using System;
using System.Collections.Generic;

namespace SharedActivities.Core.Models {
    public interface IGapFillModel : IActivityDataModel {
        IEnumerable<string> Phrases { get; }
    }
}
