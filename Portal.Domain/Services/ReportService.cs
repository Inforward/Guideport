using Portal.Data;
using Portal.Model;
using Portal.Model.Report;
using Portal.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;

namespace Portal.Domain.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;

        public ReportService(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        public IEnumerable<Category> GetViewCategories()
        {
            return _reportRepository.GetAll<Category>()
                                    .Include(v => v.ParentCategory)
                                    .Include(v => v.SubCategories)
                                    .ToList();
        }

        public IEnumerable<View> GetViews()
        {
            return _reportRepository.FindBy<View>(v => v.IsEnabled && v.ViewColumns.Any(vc => vc.IsEnabled))
                                    .Include("Category")
                                    .Include("Category.ParentCategory")
                                    .Include("Filters")
                                    .Include("ViewColumns")
                                    .Include("ViewColumns.Column")
                                    .AsNoTracking()
                                    .ToList();
        }

        public ReportResponse GetViewResult(ReportRequest request)
        {
            if (request == null || request.ViewID <= 0) return null;

            var view = _reportRepository.FindBy<View>(v => v.ViewID == request.ViewID && v.ViewColumns.Any(vc => vc.IsEnabled))
                                        .Include("Filters")
                                        .Include("ViewColumns")
                                        .Include("ViewColumns.Column")
                                        .FirstOrDefault();

            if (view == null)
                throw new Exception(string.Format("Could not find view for ViewID: {0}", request.ViewID));

            foreach (var viewFilter in view.Filters)
            {
                var filter = request.Filters.FirstOrDefault(f => f.FilterID == viewFilter.FilterID);

                if (filter != null)
                {
                    viewFilter.Value = filter.Value;
                }
            }

            if (request.Pager == null)
            {
                request.Pager = new Pager() {Page = 1, PageSize = view.PageSize ?? 50};
            }

            var executeResult = _reportRepository.Execute(view, request.Pager);

            return executeResult.ToReportResponse(view, request.FormatResults);
        }
    }

    public static class ReportServiceExtensions
    {
        public static ReportResponse ToReportResponse(this ExecuteResult executeResult, View view, bool formatResults)
        {
            var response = new ReportResponse()
            {
                View = view, 
                TotalRowCount = executeResult.TotalRowCount
            };

            var results = new List<dynamic>();

            if (executeResult.Rows.Count <= 0) 
                return response;

            foreach (var row in executeResult.Rows)
            {
                IDictionary<string, object> obj = new ExpandoObject();

                for (var j = 0; j < executeResult.Columns.Count; j++)
                {
                    var fieldName = executeResult.Columns[j].DataField;
                    var value = row[j];

                    if (formatResults && value != null && value != DBNull.Value)
                    {
                        var columnDefinition = view.ViewColumns.FirstOrDefault(c => c.DataField == fieldName);

                        if (columnDefinition != null && !string.IsNullOrEmpty(columnDefinition.DataFormat))
                        {
                            value = string.Format(columnDefinition.DataFormat, value);
                        }
                    }

                    obj.Add(fieldName, value);
                }

                results.Add((dynamic)obj);
            }

            response.Results = results;

            return response;
        }
    }
}
