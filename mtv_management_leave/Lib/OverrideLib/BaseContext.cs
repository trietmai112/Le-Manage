using Microsoft.AspNet.Identity;
using mtv_management_leave.Models;
using mtv_management_leave.Models.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web;

namespace mtv_management_leave.Lib.OverrideLib
{
    public class BaseContext : DbContext
    {
        public BaseContext(string v) : base(v)
        {
        }

        public override int SaveChanges()
        {
            var user = System.Web.HttpContext.Current.User.Identity.GetUserId();
            int userId = int.Parse(user);
            List<Object> modifiedEntities = this.ChangeTracker.Entries()
                                            .Where(x => x.State == EntityState.Modified)
                                            .Select(x => x.Entity).ToList();
            List<Object> addEntities = this.ChangeTracker.Entries()
                                            .Where(x => x.State == EntityState.Added)
                                            .Select(x => x.Entity).ToList();
            foreach (var item in modifiedEntities)
            {
                PropertyInfo propUserUpdate = item.GetType().GetProperty("UserUpdated", BindingFlags.Public | BindingFlags.Instance);
                if (null != propUserUpdate && propUserUpdate.CanWrite)
                {
                    //Todo Get UserID 
                    propUserUpdate.SetValue(item, userId, null);
                }
                PropertyInfo propDateUpdate = item.GetType().GetProperty("DateUpdated", BindingFlags.Public | BindingFlags.Instance);
                if (null != propDateUpdate && propDateUpdate.CanWrite)
                {
                    propDateUpdate.SetValue(item, DateTime.Now, null);
                }
            }

            foreach (var item in addEntities)
            {
                PropertyInfo propUserCreated = item.GetType().GetProperty("UserCreated", BindingFlags.Public | BindingFlags.Instance);
                if (null != propUserCreated && propUserCreated.CanWrite)
                {
                    //Todo Get UserID 
                    propUserCreated.SetValue(item, userId, null);
                }
                PropertyInfo propUserUpdate = item.GetType().GetProperty("UserUpdated", BindingFlags.Public | BindingFlags.Instance);
                if (null != propUserUpdate && propUserUpdate.CanWrite)
                {
                    //Todo Get UserID 
                    propUserUpdate.SetValue(item, userId, null);
                }
                PropertyInfo propDateCreated = item.GetType().GetProperty("DateCreated", BindingFlags.Public | BindingFlags.Instance);
                if (null != propDateCreated && propDateCreated.CanWrite)
                {
                    propDateCreated.SetValue(item, DateTime.Now, null);
                }
                PropertyInfo propDateUpdate = item.GetType().GetProperty("DateUpdated", BindingFlags.Public | BindingFlags.Instance);
                if (null != propDateUpdate && propDateUpdate.CanWrite)
                {
                    propDateUpdate.SetValue(item, DateTime.Now, null);
                }
            }

            return base.SaveChanges();
        }
    }
}