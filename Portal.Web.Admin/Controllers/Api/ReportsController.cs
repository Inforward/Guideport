using System;
using System.Linq;
using System.Web.Http;
using Portal.Infrastructure.Logging;
using Portal.Infrastructure.Helpers;
using Portal.Model;
using Portal.Model.Report;
using Portal.Services.Contracts;
using Portal.Web.Admin.Models;
using Portal.Web.Common.Filters.Api;

namespace Portal.Web.Admin.Controllers.Api
{
    [RoutePrefix("api/reports")]
    [PortalAuthorize(PortalRoleValues.Reporting)]
    public class ReportsController : BaseApiController
    {
        #region Private Members

        private readonly IReportService _reportService;
        private readonly IGroupService _groupService;
        private readonly IAffiliateService _affiliateService;

        #endregion

        #region Constructor

        public ReportsController(IUserService userService, ILogger logger, IReportService reportService, IGroupService groupService, IAffiliateService affiliateService) 
            : base(userService, logger)
        {
            _reportService = reportService;
            _groupService = groupService;
            _affiliateService = affiliateService;
        }

        #endregion

        #region Actions

        [HttpGet]
        [Route("{reportId:int}")]
        public ReportViewModel GetReport(int reportId)
        {
            var view = _reportService.GetViews().FirstOrDefault(v => v.ViewID == reportId);

            if(view == null)
                throw new Exception("Invalid Report ID" + reportId);

            return ToReportViewModel(view);
        }

        [HttpPost]
        [Route("")]
        public dynamic ExecuteReport([FromBody]ReportRequest request)
        {
            var response = _reportService.GetViewResult(request);

            return new
            {
                response.TotalRowCount,
                response.Results
            };
        }

        #endregion

        #region Private Methods

        private ReportViewModel ToReportViewModel(View view)
        {
            var viewModel = new ReportViewModel()
            {
                ReportID = view.ViewID,
                FullName = view.FullName,
                IsPageable = view.IsPageable,
                IsSortable = view.IsSortable,
                PageSize = view.PageSize ?? 50,
                Columns = view.ViewColumns.OrderBy(vc => vc.Ordinal).ToList()
            };

            foreach (var filter in view.Filters)
            {
                switch ((Model.Report.Filters) filter.FilterID)
                {
                    case Model.Report.Filters.BrokerDealer:
                        {
                            filter.Options = _affiliateService.GetAffiliates(new AffiliateRequest()).OrderBy(a => a.Name).Select(a => new FilterOption() { Name = a.Name, Value = a.AffiliateID.ToString() }).ToList();
                            filter.Options.Insert(0, new FilterOption() { Value = string.Empty, Name = filter.Label });
                            filter.DefaultValue = filter.Value = string.Empty;
                        }
                        break;
                    case Model.Report.Filters.Year:
                        {
                            const int startYear = 2014;

                            for (var year = startYear; year < DateTime.Now.Year + 5; year++)
                            {
                                filter.Options.Add(new FilterOption() { Name = year.ToString(), Value = year.ToString() });
                            }

                            filter.DefaultValue = filter.Value = DateTime.Now.Year.ToString();
                            filter.Options.Insert(0, new FilterOption() { Value = string.Empty, Name = filter.Label });
                        }
                        break;
                    case Model.Report.Filters.Group:
                        {
                            var isRestricted = CurrentUser.IsRestricted(PortalRoleValues.Reporting);
                            var groups = isRestricted
                                            ? _groupService.GetAccessibleGroups(CurrentUser.UserID)
                                            : _groupService.GetGroups(new GroupRequest()).Groups;

                            filter.Options = groups.OrderBy(g => g.Name).Select(g => new FilterOption() { Name = g.Name, Value = g.GroupID.ToString() }).ToList();

                            if (!isRestricted)
                            {
                                // Unrestricted Access so give them a blank option which in turn makes the group selection optional
                                filter.Options.Insert(0, new FilterOption() {Value = string.Empty, Name = filter.Label});
                                filter.DefaultValue = filter.Value = string.Empty;
                            }
                            else
                            {
                                if (filter.Options.Any())
                                {
                                    // Restricted access, default to all accessible group ids
                                    var allGroupIdsList = filter.Options.ToCsv(o => o.Value);
                                    filter.Options.Insert(0, new FilterOption() { Value = allGroupIdsList, Name = filter.Label });
                                    filter.DefaultValue = filter.Value = allGroupIdsList;
                                }
                                else
                                {
                                    // Restricted access and no groups, give them a default bogus option so no results will come back.
                                    filter.Options.Insert(0, new FilterOption() { Value = "-1", Name = filter.Label });
                                    filter.DefaultValue = filter.Value = filter.Options[0].Value;
                                }
                            }
                        }
                        break;
                    case Model.Report.Filters.StartDate:
                        {
                            filter.DefaultValue = filter.Value = DateTime.Today.AddYears(-1).ToString("MM/dd/yyyy");
                        }
                        break;
                    case Model.Report.Filters.EndDate:
                        {
                            filter.DefaultValue = filter.Value = DateTime.Today.ToString("MM/dd/yyyy");
                        }
                        break;
                    case Model.Report.Filters.ExcludeAdvisorsWithNoData:
                        {
                            filter.DefaultValue = filter.Value = true;
                        }
                        break;
                }

                viewModel.Filters.Add(filter);
            }

            return viewModel;
        }

        #endregion
    }
}