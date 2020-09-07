using System;
using System.Collections.Generic;
using System.Linq;
using Android.Util;
using SharedActivities.Droid.CustomViews;

namespace SharedActivities.Droid.Helpers {

    public class FontTextViewResizingSyncer {
        List<FontTextView> fontTextViews = new List<FontTextView>();

        public FontTextViewResizingSyncer() {
        }

        public FontTextViewResizingSyncer(IEnumerable<FontTextView> fontTextViews) {
            SetSyncedItems(fontTextViews);
        }

        public void AddSyncedItem(FontTextView fontTextView) {
            fontTextViews.Add(fontTextView);
            fontTextView.TextSizeChanged += this.FontTextView_TextSizeChanged;
        }

        public void RemoveSyncedItem(FontTextView fontTextView) {
            fontTextViews.Remove(fontTextView);
            fontTextView.TextSizeChanged -= this.FontTextView_TextSizeChanged;
        }



        public void SetSyncedItems(IEnumerable<FontTextView> fontTextViews) {
            foreach (FontTextView fontTextView in this.fontTextViews) {
                fontTextView.TextSizeChanged -= this.FontTextView_TextSizeChanged;
            }

            this.fontTextViews = fontTextViews.ToList();
            foreach (FontTextView fontTextView in this.fontTextViews) {
                fontTextView.TextSizeChanged += this.FontTextView_TextSizeChanged;
            }
        }

        private void FontTextView_TextSizeChanged(object sender, EventArgs e) {
            //var fontTextView = (FontTextView)sender;
            float smallestFont = int.MaxValue;
            foreach (FontTextView syncedFontTextView in fontTextViews) {
                smallestFont = syncedFontTextView.TextSize < smallestFont ? syncedFontTextView.TextSize : smallestFont;
            }
            foreach (FontTextView answerButton in fontTextViews) {
                if (answerButton.TextSize != int.MaxValue && answerButton.TextSize != smallestFont) {
                    answerButton.SetAutoSizeTextTypeUniformWithPresetSizes(new int[] { (int)smallestFont }, (int)ComplexUnitType.Px);
                }
            }
        }
    }
}