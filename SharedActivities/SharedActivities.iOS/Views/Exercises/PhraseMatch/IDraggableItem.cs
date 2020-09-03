using System;
namespace SharedActivities.iOS.Views.Exercises.PhraseMatch {
    public interface IDraggableItem {
        void DroppedInMainPhrase(int mainPhraseId);

        void DroppedInMatchPhases();

        string Text { get; }
    }
}
