using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace Portal.Model
{
    public partial class SurveyQuestion
    {
        private string _answer;

        public SurveyQuestion()
        {
            PossibleAnswers = new List<SurveyAnswer>();
            ResponseAnswers = new List<SurveyResponseAnswer>();
            Errors = new List<RuleViolation>();
        }

        public int SurveyQuestionID { get; set; }
        public int SurveyPageID { get; set; }
        public string QuestionName { get; set; }
        public string QuestionText { get; set; }
        public string QuestionType { get; set; }
        public string LayoutType { get; set; }
        public int SortOrder { get; set; }
        public int? MaxLength { get; set; }
        public bool IsRequired { get; set; }
        public bool IsVisible { get; set; }
        public bool IsDisabled { get; set; }
        public ICollection<SurveyAnswer> PossibleAnswers { get; set; }

        [IgnoreDataMember]
        public ICollection<SurveyResponseAnswer> ResponseAnswers { get; set; }

        [IgnoreDataMember]
        public SurveyPage SurveyPage { get; set; }

        [NotMapped]
        public int SurveyID { get; set; }

        [NotMapped]
        public string PageName { get; set; }

        [NotMapped]
        public bool IsPreSelected { get; set; }

        [NotMapped]
        public bool IsTrigger { get; set; }

        [NotMapped]
        public bool HasTrigger
        {
            get
            {
                return IsTrigger || PossibleAnswers.Any(a => a.IsTrigger);
            }
        }

        [NotMapped]
        public bool HasAnswer
        {
            get
            {
                return Answers.Any();
            }
        }

        [NotMapped]
        public string Answer
        {
            get
            {
                if (AnswerType == AnswerType.Text)
                    return _answer;

                if (AnswerType == AnswerType.Select)
                {
                    var selectedAnswer = PossibleAnswers.FirstOrDefault(a => a.IsSelected);

                    if (selectedAnswer != null)
                        return selectedAnswer.AnswerText;

                    var defaultAnswer = PossibleAnswers.FirstOrDefault(a => a.AnswerText == "None Selected");

                    if (defaultAnswer != null)
                        return defaultAnswer.AnswerText;
                }

                return null;
            }
            set
            {
                _answer = value;
            }
        }

        [NotMapped]
        public List<string> Answers
        {
            get
            {
                var list = new List<string>();

                if (AnswerType == AnswerType.Text && !string.IsNullOrEmpty(Answer))
                {
                    list.Add(Answer);
                }

                if (AnswerType == AnswerType.Select || AnswerType == AnswerType.MultiSelect || InputType == InputType.MultiText)
                {
                    list.AddRange(PossibleAnswers.Where(a => a.IsSelected).Select(a => a.AnswerText));
                }

                return list;
            }
            set { }
        }

        [NotMapped]
        public List<RuleViolation> Errors { get; set; }

        [NotMapped]
        public AnswerType AnswerType
        {
            get
            {
                switch (InputType)
                {
                    case InputType.Text:
                    case InputType.TextArea:
                    case InputType.Percent:
                    case InputType.Number:
                    case InputType.Date:
                    case InputType.MultiText:
                        {
                            return AnswerType.Text;
                        }
                    case InputType.Select:
                    case InputType.RadioButtonList:
                        {
                            return AnswerType.Select;
                        }
                    case InputType.CheckBoxList:
                        {
                            return AnswerType.MultiSelect;
                        }
                    default:
                        return AnswerType.None;
                }
            }
        }

        [NotMapped]
        public InputType InputType
        {
            get
            {
                InputType inputType;

                if (!Enum.TryParse(QuestionType, true, out inputType))
                    inputType = InputType.None;

                return inputType;
            }
        }

        [NotMapped]
        public LayoutType Layout
        {
            get
            {
                if (string.IsNullOrEmpty(LayoutType))
                    return Model.LayoutType.None;

                switch (LayoutType.ToLower())
                {
                    case "questionleft":
                        return Model.LayoutType.Left;
                    case "questionright":
                        return Model.LayoutType.Right;
                    case "section":
                        return Model.LayoutType.Section;
                    case "/section":
                        return Model.LayoutType.EndSection;
                    default:
                        return Model.LayoutType.None;
                }
            }
        }

        public void ClearSelectedAnswers()
        {
            foreach (var answer in PossibleAnswers)
            {
                answer.IsSelected = false;
            }
        }

        public bool SelectPossibleAnswer(string text)
        {
            var answer = PossibleAnswers.FirstOrDefault(a => a.AnswerText == text);

            if (answer != null)
            {
                answer.IsSelected = true;
            }

            return (answer != null);
        }

        public bool PreSelectPossibleAnswer(string text)
        {
            var answer = PossibleAnswers.FirstOrDefault(a => a.AnswerText == text);

            if (answer != null)
            {
                answer.IsSelected = true;
                IsPreSelected = true;
            }

            return (answer != null);
        }

        public SurveyAnswer FindPossibleAnswer(string text)
        {
            return PossibleAnswers.FirstOrDefault(a => a.AnswerText == text);
        }

        public void AddError(string field, string message)
        {
            Errors.Add(new RuleViolation(message, field));
        }

        public int ParseInt(string text)
        {
            int i;
            return Int32.TryParse(text, out i) ? i : 0;
        }

        public int ParseInt()
        {
            return ParseInt(Answer);
        }

        public float ParseFloat(string text)
        {
            float f;
            return float.TryParse(text, out f) ? f : 0;
        }

        public float ParseFloat()
        {
            return ParseFloat(Answer);
        }

        public bool IsMatch(string pattern, string text)
        {
            var r = new Regex(pattern, RegexOptions.Multiline);
            return r.IsMatch(text);
        }

        public bool IsInteger(string text)
        {
            int i;
            return Int32.TryParse(text, out i);
        }

        public bool IsInteger()
        {
            return IsInteger(Answer);
        }

        public bool IsNumeric(string text)
        {
            float f;
            return float.TryParse(text, out f);
        }

        public bool IsNumeric()
        {
            return IsNumeric(Answer);
        }

        public bool IsBetween(string text, float min, float max)
        {
            float f;
            if (float.TryParse(text, out f))
            {
                if (f > min && f < max)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsBetween(float min, float max)
        {
            return IsBetween(Answer, min, max);
        }

        public bool IsPercent(string text)
        {
            float f;
            if (float.TryParse(text, out f))
            {
                if (f >= 0f && f <= 100f)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsPercent()
        {
            return IsPercent(Answer);
        }

        public override string ToString()
        {
            return string.Format("Name: {0}; Visible: {1}; Disabled {2}; HasAnswer: {3}; IsRequired: {4}", QuestionText, IsVisible, IsDisabled, HasAnswer, IsRequired);
        }
    }

    public enum AnswerType
    {
        Text,
        Select,
        MultiSelect,
        None
    }

    public enum InputType
    {
        None,
        CheckBoxList,
        RadioButtonList,
        Date,
        Group,
        MultiText,
        Number,
        Percent,
        Select,
        Text,
        TextArea
    }

    public enum LayoutType
    {
        Left,
        Right,
        Section,
        EndSection,
        None
    }
}
