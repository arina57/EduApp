using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SharedActivities.Core {
	/// <summary>
	/// Finds tags in text with a regex pattern and divides up text in matches and non-matches.
	/// Only works with Regex expression that has a single caputre group.
	/// </summary>
	public class TagFinder {
		private List<TextLocation> textLocations;
		public ReadOnlyCollection<TextLocation> TextLocations => textLocations.AsReadOnly();
		/// <summary>
		/// Plain text version of the string, with the capture pattern / tags removed
		/// </summary>
		public string Text { get; private set; }
		/// <summary>
		/// String with tags still in it.
		/// </summary>
		public string SourceText { get; private set; }
		/// <summary>
		/// Regex to find.
		/// </summary>
		public Regex TextPattern { get; private set; } = new Regex(@"\{(.*?)\}");

		public string TaggedText { get; private set; }

		public ReadOnlyCollection<string> OriginalTaggedTextValues { get; private set; }
		public ReadOnlyCollection<TextLocation> MatchTextLocations => textLocations.Where(location => location.IsAMatch).ToList().AsReadOnly();

		public int MatchCount => OriginalTaggedTextValues.Count;

		public TagFinder(string sourceText, Regex pattern) {
			TextPattern = pattern;
			SourceText = sourceText;
			MakeTextLocations();
			OriginalTaggedTextValues = TextLocations.Where(location => location.IsAMatch).Select(location => location.Value).ToList().AsReadOnly();

		}

		public TagFinder(string sourceText) {
			SourceText = sourceText;
			MakeTextLocations();
			OriginalTaggedTextValues = TextLocations.Where(location => location.IsAMatch).Select(location => location.Value).ToList().AsReadOnly();

		}

		/// <summary>
		/// replaces the value of the match at the indicated index.
		/// Rebuilds the Text value from TextLocations. 
		/// </summary>
		/// <param name="indexToReplace"></param>
		/// <param name="textToReplace"></param>
		public void ReplaceTextAtLocation(int indexToReplace = -1, string textToReplace = "") {
			var stringBuilder = new StringBuilder();
			var taggedStringBuilder = new StringBuilder();
			int startIndex = 0;
			foreach (var location in TextLocations) {
				location.Start = startIndex;
				if (indexToReplace != -1 && location.IsAMatch && location.MatchNumber == indexToReplace && !string.IsNullOrEmpty(textToReplace)) {
					location.Value = textToReplace;
				}
				startIndex = location.End + 1;
				if (location.IsAMatch) {
					taggedStringBuilder.Append(location.MatchedString);
				} else {
					taggedStringBuilder.Append(location.Value);
				}

				stringBuilder.Append(location.Value);
			}
			TaggedText = taggedStringBuilder.ToString();
			Text = stringBuilder.ToString();
		}


		/// <summary>
		/// Makes all the TextLocations from the regex pattern.
		/// </summary>
		private void MakeTextLocations() {
			textLocations = new List<TextLocation>();
			MatchCollection matches = TextPattern.Matches(SourceText);
			TextPattern.Replace(SourceText, "");
			int currentPartIndex = 0;
			//If there are no matches then add the whole string unmodified to a single TextLocation
			if (matches.Count < 1) {
				var replacebleTextLocation = new TextLocation(SourceText, 0);
				textLocations.Add(replacebleTextLocation);
				Text = SourceText;
				TaggedText = SourceText;
				//Text = SourceText;
			} else {
				for (int matchNumber = 0; matchNumber < matches.Count; matchNumber++) {
					var match = matches[matchNumber]; //current match
					var capture = match.Groups[1]; //first capture group for that match. Other capture groups are ignored

					//find the string between the last match and the current match
					string leadingPartOfText = SourceText.ToString().Substring(currentPartIndex, match.Index - currentPartIndex);
					//If it exists add it to the TextLocations
					if (leadingPartOfText.Length > 0) {
						var leadingTextLocation = new TextLocation(leadingPartOfText, currentPartIndex);
						textLocations.Add(leadingTextLocation); //add the string before the current match
																//Text += leadingTextLocation.Value;
					}
					var matchTextLocation = new TextLocation(capture.Value, match.Value, matchNumber); //make a location out of the match
					textLocations.Add(matchTextLocation); //add the match
					currentPartIndex = match.Index + match.Value.Length; //make the current part of the string the end of the match.
				}
				//After all the matches have been collected, see if there is any trailing text after the last match.
				if (currentPartIndex < SourceText.Length) {
					string trailingPartOfText = SourceText.Substring(currentPartIndex, SourceText.Length - currentPartIndex);
					if (trailingPartOfText.Length > 0) {
						var trailingTextLocation = new TextLocation(trailingPartOfText, currentPartIndex);
						textLocations.Add(trailingTextLocation);
						//Text += trailingTextLocation.Value;
					}
				}
			}

			//Do this to find the start and end locations, because new string has different lenght and location.
			//Probabaly more efficent way to do this but this is easier. O(N)
			ReplaceTextAtLocation();
		}

		/// <summary>
		/// Part of text to indicate it Start, End and Length in the original text as well as if it is a match.
		/// </summary>
		public class TextLocation {
			public int Start { get; set; }
			public int End {
				get { return Start + Length - 1; }
			}
			/// <summary>
			/// starting index of the text value within the full text.
			/// </summary>
			public int MatchNumber { get; set; } = -1;
			/// <summary>
			/// Lenght of the text value
			/// </summary>
			public int Length { get { return Value.Length; } }
			/// <summary>
			/// Value of the capture group
			/// </summary>
			/// 
			private string value;
			public string Value {
				get => value;
				set {
					MatchedString = MatchedString.Replace(this.value, value);
					this.value = value;
				}
			}

			/// <summary>
			/// Value of the whole matching text
			/// </summary>
			public string MatchedString { get; private set; } = String.Empty;


			public bool IsAMatch { get { return MatchNumber > -1; } }

			public TextLocation(string value, int start) {
				this.value = value;
				this.Start = start;
			}
			public TextLocation(string value, string matchedString, int matchNumber) {
				this.value = value;
				this.MatchedString = matchedString;
				this.MatchNumber = matchNumber;

			}
		}

		public TextLocation GetTextLocationAtPosition(int position) {
			foreach (var location in TextLocations) {
				if (position >= location.Start && position <= location.End) {
					return location;
				}
			}
			return null;
		}
	}
}
