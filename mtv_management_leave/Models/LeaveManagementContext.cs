using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using mtv_management_leave.Models.Entity;
using mtv_management_leave.Models.Entity.Interface;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Data.Common;
using System.Data.Entity.Infrastructure;

namespace mtv_management_leave.Models
{
    public class LeaveManagementContext : IdentityDbContext<UserInfo, Role, int, UserLogin, UserRole, UserClaim>
    {
        public string NameOrConnectionString { get; private set; }
        public LeaveManagementContext(): base("name=DefaultConnection")
        {
            NameOrConnectionString = "name=DefaultConnection";
        }
        public DbSet<AddLeave> AddLeaves { get; set; }
        public DbSet<Entity.DataBeginYear> DataBeginYears { get; set; }
        public DbSet<Entity.DataInOutRaw> DataInOutRaws { get; set; }
        public DbSet<Entity.InOut> InOuts { get; set; }
        public DbSet<Entity.LeaveMonthly> LeaveMonthlies { get; set; }
        public DbSet<Entity.MasterLeaveDayCompany> MasterLeaveDayCompanies { get; set; }
        public DbSet<Entity.MasterLeaveType> MasterLeaveTypes { get; set; }
        public DbSet<Entity.RegisterLeave> RegisterLeaves { get; set; }
        public DbSet<Entity.UserSeniority> UserSeniorities { get; set; }
        public DbSet<Entity.EmployeeInfo> EmployeeInfos { get; set; }
        public DbSet<Entity.RequestChangeInout> RequestChangeInouts { get; set; }


        


        private void BeforeSaveChange()
        {
            try
            {

                //var GetUserId = System.Web.HttpContext.Current.User.Identity.GetUserId();
                var GetUserId = "1";
                int userId = int.Parse(GetUserId.ToString());
                var entities = ChangeTracker.Entries().Where(x => x.Entity is IEntity
                                                                              && (x.State == EntityState.Added
                                                                              || x.State == EntityState.Modified));

                var dateNow = DateTime.Now;
                foreach (var entity in entities)
                {
                    var baseEntity = entity.Entity as IEntity;
                    if (!(entity.Entity is IEntity)) continue;
                    if (baseEntity.DateCreated == DateTime.MinValue)
                    {
                        switch (entity.State)
                        {
                            case EntityState.Added:
                                baseEntity.UserCreated = userId;
                                baseEntity.UserUpdated = userId;
                                baseEntity.DateCreated = dateNow;
                                baseEntity.DateUpdated = dateNow;
                                break;
                            case EntityState.Modified:
                                baseEntity.UserUpdated = userId;
                                baseEntity.DateUpdated = dateNow;
                                break;
                        }
                        // This is needed when updating a record that was not loaded from the db to avoid min date default
                    }
                }
            }catch(Exception ex)
            {
                Console.WriteLine("Context BeforeSaveChange error");
                Console.WriteLine(ex);
            }
        }

        public override int SaveChanges()
        {
            BeforeSaveChange();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync()
        {
            BeforeSaveChange();
            return base.SaveChangesAsync();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            BeforeSaveChange();
            return base.SaveChangesAsync(cancellationToken);
        }

    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
