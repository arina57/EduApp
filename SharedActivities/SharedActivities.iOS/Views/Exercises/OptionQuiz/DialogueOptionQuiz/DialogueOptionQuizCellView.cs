using System;
using CoreGraphics;
using Foundation;
using SharedActivities.Core.ViewModels.Exercises;
using UIKit;

namespace SharedActivities.iOS.Views.Exercises.OptionQuiz.DialogueOptionQuiz {
    public partial class DialogueOptionQuizCellView : UICollectionViewCell {
        public static readonly NSString Key = new NSString(nameof(DialogueOptionQuizCellView));
        public static readonly UINib Nib = UINib.FromName(Key, NSBundle.MainBundle);

        static DialogueOptionQuizCellView() {
        }

        protected DialogueOptionQuizCellView(IntPtr handle) : base(handle) {
            // Note: this .ctor should not contain any initialization logic.
        }


        public void Setup(DialogueOptionQuizViewModel logic, int position) {

            CharacterImageView.Subviews.ReleaseChildren();
            CharacterNameLabel.Text = logic.GetRoleName(position);

            var characterLottie = CharacterImageView.AddLottieToView(logic.GetRoleImageJson(position));

            if (logic.IsCurrentRole(position)) {
                characterLottie.Transform = CGAffineTransform.MakeScale(0.8f, 0.8f);
                characterLottie.Alpha = 0.5f;
                UIView.Animate(0.5, () => {
                    characterLottie.Transform = CGAffineTransform.MakeScale(1f, 1f);
                    characterLottie.Alpha = 1f;
                }, () => {
                    UIView.AnimateKeyframes(1, 0, UIViewKeyframeAnimationOptions.Repeat, () => {
                        UIView.AddKeyframeWithRelativeStartTime(0, 0.5, () => {
                            characterLottie.Transform = CGAffineTransform.MakeScale(0.98f, 0.98f);
                        });
                        UIView.AddKeyframeWithRelativeStartTime(0.5, 0.5, () => {
                            characterLottie.Transform = CGAffineTransform.MakeScale(1f, 1f);
                        });
                    }, new UICompletionHandler((target) => { }));
                });

            } else {
                UIView.Animate(0.5, () => {
                    characterLottie.Transform = CGAffineTransform.MakeScale(0.8f, 0.8f);
                    characterLottie.Alpha = 0.5f;
                });
            }
        }
    }
}
