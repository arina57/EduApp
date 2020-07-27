using System;
namespace SharedActivities.Core {
    public class ReplaceTextClickedEventArgs : EventArgs {



        public ReplaceTextClickedEventArgs(int index, TagFinder tagFinder) {
            Index = index;
            this.TagFinder = tagFinder;
        }

        public int Index { get; private set; }
        public string Text => TagFinder.Text;


        public TagFinder TagFinder { get; private set; }
    }
}
