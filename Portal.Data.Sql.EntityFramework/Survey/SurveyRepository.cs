using System;
using Portal.Infrastructure.Helpers;
using Portal.Model;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using RefactorThis.GraphDiff;

namespace Portal.Data.Sql.EntityFramework
{
    public class SurveyRepository : EntityRepository<MasterContext>, ISurveyRepository
    {
        public Survey GetSurveyByID(int surveyId)
        {
            return FindBy<Survey>(s => s.SurveyID == surveyId).IncludeAll().FirstOrDefault().EnsureOrder();
        }

        public Survey GetSurveyByName(string surveyName)
        {
            return FindBy<Survey>(s => s.SurveyName == surveyName).IncludeAll().FirstOrDefault().EnsureOrder();
        }

        public void UpdateSurvey(Survey survey, int auditUserId)
        {
            Context.UpdateGraph(survey, map => map
                .OwnedCollection(a => a.Pages,
                    b => b.OwnedCollection(c => c.Questions,
                        d => d.OwnedCollection(e => e.PossibleAnswers, f => f.OwnedCollection(g => g.SuggestedContents)))
                        .OwnedCollection(h => h.ScoreRanges)));

            SetAuditFields(auditUserId);
            Save();
        }

        public SurveyResponse GetSurveyResponse(string surveyName, int userId)
        {
            return FindBy<SurveyResponse>(s => s.Survey.SurveyName == surveyName && s.UserID == userId)
                            .AsNoTracking()
                            .Include(s => s.Answers)
                            .FirstOrDefault();
        }

        public void CreateSurveyResponse(SurveyResponse surveyResponse)
        {
            Add(surveyResponse);
            Save();
        }

        public void UpdateSurveyResponse(SurveyResponse surveyResponse)
        {
            Context.UpdateGraph(surveyResponse, r => r.OwnedCollection(a => a.Answers));
            SetAuditFields(surveyResponse.ModifyUserID);
            Save();
        }

        public void UpdateSurveyResponseHistory(List<SurveyResponseHistory> surveyResponseHistories)
        {
            if (surveyResponseHistories.IsNullOrEmpty())
                return;

            var surveyResponseHistory = surveyResponseHistories.First();
            var surveyPageIdList = surveyResponseHistories.Select(h => h.SurveyPageID).ToList().ToCsv();
            var scoreList = surveyResponseHistories.Select(h => h.Score).ToList().ToCsv();
            var percentCompleteList = surveyResponseHistories.Select(h => h.PercentComplete).ToList().ToCsv();
            var existingResponseHistories = FindBy<SurveyResponseHistory>(h => h.SurveyResponseID == surveyResponseHistory.SurveyResponseID
                                                                                && h.ResponseDate.Equals(surveyResponseHistory.ResponseDate)).ToList();
            var auditType = existingResponseHistories.Any() ? AuditTypes.Update : AuditTypes.Insert;

            // TODO: Correctly set SurveyResponseID to RelatedKey fields
            Audit<SurveyResponseHistory>(auditType, surveyResponseHistory.SurveyResponseID, surveyResponseHistory.ModifyUserID, 
                new { SurveyResponseHistories = existingResponseHistories }, new { SurveyResponseHistories = surveyResponseHistories },
                () => Context.Database.ExecuteSqlCommand(
                            "dbo.SurveyResponseHistoryUpdate" +
                            "   @surveyResponseID, @surveyPageIdList, @scoreList, @percentCompleteList, @responseDate, @modifyUser, @modifyDate, @modifyDateUTC",
                            new SqlParameter("@surveyResponseID", surveyResponseHistory.SurveyResponseID),
                            new SqlParameter("@surveyPageIdList", surveyPageIdList),
                            new SqlParameter("@scoreList", scoreList),
                            new SqlParameter("@percentCompleteList", percentCompleteList),
                            new SqlParameter("@responseDate", surveyResponseHistory.ResponseDate),
                            new SqlParameter("@modifyUser", surveyResponseHistory.ModifyUserID),
                            new SqlParameter("@modifyDate", surveyResponseHistory.ModifyDate),
                            new SqlParameter("@modifyDateUTC", surveyResponseHistory.ModifyDateUtc)));
        }

        private void SetAuditFields(int auditUserId)
        {
            // Ensure audit fields are set on owned collections
            Context.ChangeTracker.Entries()
                .Where(e => e.Entity is Auditable && (e.State == EntityState.Added || e.State == EntityState.Deleted || e.State == EntityState.Modified))
                .ForEach(s =>
                {
                    var entity = (Auditable)s.Entity;

                    if (s.State == EntityState.Added)
                    {
                        entity.CreateUserID = auditUserId;
                        entity.CreateDate = DateTime.Now;
                        entity.CreateDateUtc = DateTime.UtcNow;
                    }

                    entity.ModifyUserID = auditUserId;
                    entity.ModifyDate = DateTime.Now;
                    entity.ModifyDateUtc = DateTime.UtcNow;
                });            
        }
    }

    public static class SurveyExtensions
    {
        public static IQueryable<Survey> IncludeAll(this IQueryable<Survey> survey)
        {
            return survey
                    .AsNoTracking()
                    .Include("Pages")
                    .Include("Pages.Questions")
                    .Include("Pages.Questions.PossibleAnswers")
                    .Include("Pages.Questions.PossibleAnswers.SuggestedContents")
                    .Include("Pages.ScoreRanges");
        }

        public static Survey EnsureOrder(this Survey survey)
        {
            if (survey == null)
                return null;

            survey.Pages = survey.Pages.OrderBy(p => p.SortOrder).ToList();

            foreach (var page in survey.Pages)
            {
                page.Questions = page.Questions.OrderBy(q => q.SortOrder).ToList();

                foreach (var question in page.Questions)
                {
                    question.PossibleAnswers = question.PossibleAnswers.OrderBy(a => a.SortOrder).ToList();
                }
            }

            return survey;
        }
    }
}