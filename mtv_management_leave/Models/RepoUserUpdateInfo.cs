using System;

namespace mtv_management_leave.Models
{
    public class RepoUserUpdateInfo
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public DateTime? DateBeginProbation { get; set; }
        public DateTime? DateBeginWork { get; set; }
        public DateTime? DateResign { get; set; }
        public DateTime? DateOfBirth { get; set; }

    }
}