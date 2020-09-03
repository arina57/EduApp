using System;
using CoreGraphics;
using Foundation;
using SharedActivities.Core.ViewModels.Exercises;
using UIKit;

namespace SharedActivities.iOS.Views.Exercises.OptionQuiz.DialogueOptionQuiz {
    public partial class DialogueOptionQuizCell : UICollectionViewCell {
        public static readonly NSString Key = new NSString("DialogueOptionQuizCell");
        public static readonly UINib Nib;

        static DialogueOptionQuizCell() {
            Nib = UINib.FromName("DialogueOptionQuizCell", NSBundle.MainBundle);
        }

        protected DialogueOptionQuizCell(IntPtr handle) : base(handle) {
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
