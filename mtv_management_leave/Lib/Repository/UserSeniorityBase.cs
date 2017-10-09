using mtv_management_leave.Lib.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using mtv_management_leave.Models;
using mtv_management_leave.Models.Entity;

namespace mtv_management_leave.Lib.Repository
{
    public class UserSeniorityBase : Base, IUserSeniority
    {
        LeaveManagementContext context;

        public UserSeniorityBase()
        {
        }

        public void GenerateUserSeniority(int year)
        {
            InitContext(out context);
            DisposeContext(context);
        }

        public void GenerateUserSeniority(int year, List<int> lstUid)
        {
            generate(year, lstUid);
        }

        public List<UserSeniority> GetUserSeniority(int year)
        {
            return getData(year, null);
        }

        public List<UserSeniority> GetUserSeniority(int year, List<int> lstUid)
        {
            return getData(year, lstUid);
        }

        #region Private method
        private List<UserSeniority> getData(int year, List<int> lstUid)
        {
            List<UserSeniority> lstResult = new List<UserSeniority>();
            InitContext(out context);
            var query = context.UserSeniorities.Where(m => m.Year == year);
            if (lstUid != null && lstUid.Count > 0)
            {
                query = query.Where(m => lstUid.Contains(m.Uid));

            }
            lstResult = query.ToList();
            DisposeContext(context);
            return lstResult;
        }

        private void generate(int year, List<int> lstUid)
        {
            InitContext(out context);
            var queryListProfile = context.Users.Where(m => m.DateResign == null || m.DateResign.Value.Year >= year).Select(m => new { m.Id, m.DateBeginWork, m.DateResign });
            var queryListUserSeniorityDB = context.UserSeniorities.Where(m => m.Year == year);
            if (lstUid != null && lstUid.Count > 0)
            {
                queryListProfile = queryListProfile.Where(m => lstUid.Contains(m.Id));
                queryListUserSeniorityDB = queryListUserSeniorityDB.Where(m => lstUid.Contains(m.Uid));
            }
            var lstProfile = queryListProfile.ToList();
            var listUserSeniorityDB = queryListUserSeniorityDB.ToList();
            DateTime beginYear = new DateTime(year, 1, 1);
            DateTime endYear = new DateTime(year, 12, 1);
            foreach (var user in lstProfile)
            {
                if (user.DateBeginWork == null)
                    continue;
                UserSeniority userS = new UserSeniority();
                userS.Uid = user.Id;
                for (DateTime month = beginYear; month <= endYear; month = month.AddMonths(1))
                {
                    int daySeniority = 0;
                    if (user.DateResign != null && new DateTime(user.DateResign.Value.Year, user.DateResign.Value.Month, 1) < month)
                    {
                        daySeniority = 0;
                    }
                    else
                    {
                        DateTime yearlater = new DateTime(user.DateBeginWork.Value.Year + 5, user.DateBeginWork.Value.Month, 1);
                        if(month >= yearlater)
                        {
                            daySeniority += 3;
                            bool isfirst = true;
                            while (month >= yearlater)
                            {
                                if (isfirst)
                                {
                                    isfirst = false;
                                    yearlater = yearlater.AddYears(1);
                                    continue;
                                }
                                daySeniority++;
                                yearlater = yearlater.AddYears(1);
                            }

                        }
                    }
                    switch (month.Month)
                    {
                        case 1: userS.Month1 = daySeniority; break;
                        case 2: userS.Month2 = daySeniority; break;
                        case 3: userS.Month3 = daySeniority; break;
                        case 4: userS.Month4 = daySeniority; break;
                        case 5: userS.Month5 = daySeniority; break;
                        case 6: userS.Month6 = daySeniority; break;
                        case 7: userS.Month7 = daySeniority; break;
                        case 8: userS.Month8 = daySeniority; break;
                        case 9: userS.Month9 = daySeniority; break;
                        case 10: userS.Month10 = daySeniority; break;
                        case 11: userS.Month11 = daySeniority; break;
                        case 12: userS.Month12 = daySeniority; break;
                    }
                    var DataDB = listUserSeniorityDB.Where(m => m.Uid == user.Id).FirstOrDefault();
                    if(DataDB== null)
                    {
                        context.UserSeniorities.Add(userS);
                    }
                    else
                    {
                        DataDB.Month1 = userS.Month1;
                        DataDB.Month2 = userS.Month2;
                        DataDB.Month3 = userS.Month3;
                        DataDB.Month4 = userS.Month4;
                        DataDB.Month5 = userS.Month5;
                        DataDB.Month6 = userS.Month6;
                        DataDB.Month7 = userS.Month7;
                        DataDB.Month8 = userS.Month8;
                        DataDB.Month9 = userS.Month9;
                        DataDB.Month10 = userS.Month10;
                        DataDB.Month11 = userS.Month11;
                        DataDB.Month12 = userS.Month12;
                    }
                }
            }
            context.SaveChanges();
            DisposeContext(context);
        }

        public void SaveUserSeniority(UserSeniority UserSeniorityInput)
        {
            InitContext(out context);
            var inDB = context.UserSeniorities.Where(m => m.Year == UserSeniorityInput.Year && m.Uid == UserSeniorityInput.Uid).FirstOrDefault();
            if(inDB!= null)
            {
                inDB.Month1 = UserSeniorityInput.Month1;
                inDB.Month2 = UserSeniorityInput.Month2;
                inDB.Month3 = UserSeniorityInput.Month3;
                inDB.Month4 = UserSeniorityInput.Month4;
                inDB.Month5 = UserSeniorityInput.Month5;
                inDB.Month6 = UserSeniorityInput.Month6;
                inDB.Month7 = UserSeniorityInput.Month7;
                inDB.Month8 = UserSeniorityInput.Month8;
                inDB.Month9 = UserSeniorityInput.Month9;
                inDB.Month10 = UserSeniorityInput.Month10;
                inDB.Month11 = UserSeniorityInput.Month11;
                inDB.Month12 = UserSeniorityInput.Month12;
            }
            else
            {
                context.UserSeniorities.Add(UserSeniorityInput);
            }
            context.SaveChanges();
            DisposeContext(context);
                
        }
    }

    #endregion

}