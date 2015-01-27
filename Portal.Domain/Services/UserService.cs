using System.Data.Entity;
using System.Web.UI.WebControls;
using Portal.Data;
using Portal.Infrastructure.Caching;
using Portal.Infrastructure.Helpers;
using Portal.Model;
using Portal.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Portal.Domain.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ICacheStorage _cacheStorage;

        public UserService(IUserRepository userRepository, ICacheStorage cacheStorage)
        {
            _userRepository = userRepository;
            _cacheStorage = cacheStorage;
        }

        public User GetUserByUserId(int userId)
        {
            var request = new UserRequest
            {
                UserID = userId,
                IncludeApplicationRoles = true,
                IncludeGroups = true,
                IncludeObjectCache = true,
                IncludeAffiliates = true,
                IncludeAffiliateLogos = true
            };

            return GetUser(request);
        }

        public User GetUser(UserRequest criteria)
        {
            var response = GetUsers(criteria);

            return response.Users.FirstOrDefault();
        }

        public UserResponse GetUsers(UserRequest request)
        {
            var response = new UserResponse();

            var query = _userRepository.GetAll<User>();

            if (!string.IsNullOrEmpty(request.FirstName))
                query = query.Where(u => u.DisplayFirstName.StartsWith(request.FirstName));

            if (!string.IsNullOrEmpty(request.LastName))
                query = query.Where(u => u.DisplayLastName.StartsWith(request.LastName));

            if (!string.IsNullOrEmpty(request.Name))
                query = query.Where(u => u.DisplayName.Contains(request.Name));

            if (request.UserID > 0)
                query = query.Where(u => u.UserID == request.UserID);

            if (request.UserStatusID > 0)
                query = query.Where(u => u.UserStatusID == request.UserStatusID);

            if (!string.IsNullOrEmpty(request.ProfileID))
                query = query.Where(u => u.ProfileID == request.ProfileID);

            if (request.AffiliateID > 0)
                query = query.Where(u => u.AffiliateID == request.AffiliateID);

            if (request.UserIDs != null && request.UserIDs.Any())
                query = query.Where(u => request.UserIDs.Contains(u.UserID));

            if (!request.MemberGroupIDs.IsNullOrEmpty())
                query = query.Where(u => u.Groups.Any(g => request.MemberGroupIDs.Contains(g.GroupID)));

            if (request.ExcludeMemberGroupID > 0)
                query = query.Where(u => u.Groups.All(g => g.GroupID != request.ExcludeMemberGroupID));

            if (!request.AccessibleGroupIDs.IsNullOrEmpty())
                query = query.Where(u => u.AccessibleGroups.Any(g => request.AccessibleGroupIDs.Contains(g.GroupID)));

            if (request.ExcludeAccessibleGroupID > 0)
                query = query.Where(u => u.AccessibleGroups.All(g => g.GroupID != request.ExcludeAccessibleGroupID));

            if (request.ProfileTypeID >= 0)
                query = query.Where(u => u.ProfileTypeID == request.ProfileTypeID);

            if (!request.IncludeTerminated && !request.UserID.HasValue)
                query = query.Where(u => u.TerminateDate == null);

            if (request.IncludeGroups)
                query = query.Include("Groups");

            if (request.IncludeAccessibleGroups)
                query = query.Include("AccessibleGroups")
                             .Include("AccessibleGroups.Group");

            if (request.IncludeBranchInfo)
                query = query.Include("Branches");

            if (request.IncludeLicenseInfo)
                query = query.Include("Licenses");

            if (request.IncludeApplicationRoles)
                query = query.Include("ApplicationRoles")
                             .Include("ApplicationRoles.ApplicationRole")
                             .Include("ApplicationRoles.ApplicationAccess");

            if (request.IncludeAffiliates)
            {
                query = query.Include("Affiliate");

                if (request.IncludeAffiliateLogos)
                    query = query.Include("Affiliate.Logos");

                if (request.IncludeAffiliateFeatures)
                    query = query.Include("Affiliate.Features")
                                 .Include("Affiliate.Features.Settings");
            }

            if(request.IncludeObjectCache)
                query = query.Include("ObjectCache");

            query = string.IsNullOrWhiteSpace(request.SortExpression) ? query.OrderBy(u => u.DisplayName) : query.SortBy(request.SortExpression);

            if (request.Skip > 0 || request.Take > 0)
            {
                response.TotalRecordCount = query.Count();

                if (request.Skip > 0)
                    query = query.Skip(request.Skip);

                if (request.Take > 0)
                    query = query.Take(request.Take);

                response.Users = query.ToList();
            }
            else
            {
                response.Users = query.ToList();
                response.TotalRecordCount = response.Users.Count();
            }

            return response;
        }

        public IEnumerable<Affiliate> GetAffiliates(AffiliateRequest request = null)
        {
            const string cacheKey = "UserService.Affiliates";

            request = request ?? new AffiliateRequest();

            return request.UseCache ? _cacheStorage.Retrieve(cacheKey, () => GetAffiliatesInternal(request)) : GetAffiliatesInternal(request);

        }

        public IEnumerable<ProfileType> GetProfileTypes()
        {
            const string cacheKey = "UserService.ProfileTypes";
            return _cacheStorage.Retrieve(cacheKey, () => _userRepository.GetAll<ProfileType>().ToList());
        }

        public IEnumerable<UserStatus> GetUserStatuses()
        {
            const string cacheKey = "UserService.UserStatuses";
            return _cacheStorage.Retrieve(cacheKey, () => _userRepository.GetAll<UserStatus>().OrderBy(u => u.Name).ToList());
        }

        public IEnumerable<ApplicationRole> GetApplicationRoles()
        {
            return _userRepository.GetAll<ApplicationRole>()
                        .Include("ApplicationRoleAccesses")
                        .Include("ApplicationRoleAccesses.ApplicationAccess")
                        .ToList();
        }

        public void UpdateUserRoles(int userId, IEnumerable<ApplicationRole> roles, int auditUserId)
        {
            var existingRoles = _userRepository.FindBy<ApplicationRoleUser>(r => r.UserID == userId).ToList();

            foreach (var role in roles)
            {
                var existingRole = existingRoles.FirstOrDefault(r => r.ApplicationRoleID == role.ApplicationRoleID);

                if (role.Enabled)
                {
                    if (existingRole != null)
                    {
                        existingRole.ApplicationAccessID = role.ApplicationAccessID;

                        UpdateAuditData(existingRole, auditUserId);

                        _userRepository.Update(existingRole);
                        _userRepository.Save();
                    }
                    else
                    {
                        var userRole = new ApplicationRoleUser()
                        {
                            UserID = userId,
                            ApplicationRoleID = role.ApplicationRoleID,
                            ApplicationAccessID = role.ApplicationAccessID
                        };

                        AddAuditData(userRole, auditUserId);

                        _userRepository.Add(userRole);
                        _userRepository.Save();
                    }
                }
                else if (existingRole != null)
                {
                    UpdateAuditData(existingRole, auditUserId);

                    _userRepository.Delete(existingRole);
                    _userRepository.Save();
                }
            }
        }

        public void SaveUserObjectCache(ObjectCache objectCache)
        {
            var item = _userRepository.FindBy<ObjectCache>(o => o.UserID == objectCache.UserID && o.Key == objectCache.Key).FirstOrDefault();

            if (item == null)
            {
                objectCache.CreateDate = objectCache.ModifyDate = DateTime.Now;

                _userRepository.Add(objectCache);
            }
            else
            {
                item.ValueSerialized = objectCache.ValueSerialized;
                item.ModifyDate = objectCache.ModifyDate = DateTime.Now;

                _userRepository.Update(item);
            }

            _userRepository.Save();
        }

        public void UpdateAffiliate(ref Affiliate affiliate)
        {
            var incomingAffiliate = affiliate;

            var existingAffiliate = _userRepository.FindBy<Affiliate>(a => a.AffiliateID == incomingAffiliate.AffiliateID).FirstOrDefault();

            if (existingAffiliate == null)
                throw new Exception("Invalid Affiliate ID");

            affiliate = _userRepository.SaveGraph(incomingAffiliate);

            _cacheStorage.ClearNamespace("UserService.Affiliates");
        }

        #region Private Methods

        private IEnumerable<Affiliate> GetAffiliatesInternal(AffiliateRequest request)
        {
            var query = _userRepository.GetAll<Affiliate>()
                                       .Include("Logos");

            if (request.AffiliateID > 0)
                query = query.Where(a => a.AffiliateID == request.AffiliateID);

            return request.IncludeUserCount ? GetAffiliateUserCounts(query).ToList() : query.ToList();            
        }

        private IEnumerable<Affiliate> GetAffiliateUserCounts(IQueryable<Affiliate> query)
        {
            query = query.Include(a => a.Users);

            var affiliates = query.Select(a => new AffiliatePresentation
            {
                Affiliate = a,
                Logos = a.Logos,
                UserCount = a.Users.Count()
            }).ToList();

            foreach (var affiliate in affiliates)
            {
                affiliate.Affiliate.Logos = affiliate.Logos;
                affiliate.Affiliate.UserCount = affiliate.UserCount;
            }

            return affiliates.Select(g => g.Affiliate);
        }
        #endregion
    }
}
