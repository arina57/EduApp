﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SharedActivities.Core.Resx {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class String {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal String() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("SharedActivities.Core.Resx.String", typeof(String).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Check Answers.
        /// </summary>
        public static string CheckAnswersButton {
            get {
                return ResourceManager.GetString("CheckAnswersButton", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Good.
        /// </summary>
        public static string FeedbackGood {
            get {
                return ResourceManager.GetString("FeedbackGood", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Better luck next time.
        /// </summary>
        public static string FeedbackNotGood {
            get {
                return ResourceManager.GetString("FeedbackNotGood", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Perfect.
        /// </summary>
        public static string FeedbackPerfect {
            get {
                return ResourceManager.GetString("FeedbackPerfect", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} Attempt Bonus x{1}.
        /// </summary>
        public static string NthAttemptBonus {
            get {
                return ResourceManager.GetString("NthAttemptBonus", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Perfect Score Bonus: +{0}.
        /// </summary>
        public static string PerfectScoreBonus {
            get {
                return ResourceManager.GetString("PerfectScoreBonus", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to +{0} points.
        /// </summary>
        public static string PlusPoints {
            get {
                return ResourceManager.GetString("PlusPoints", resourceCulture);
            }
        }
    }
}
