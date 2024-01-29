namespace Application.Common.Models
{
    public class Student
    {
        //public string InstituteCode { get; set; }
        public string StudentUniqueId { get; set; }
        public string ArabicFirstName { get; set; }
        public string ArabicSecondName { get; set; }
        public string ArabicThirdName { get; set; }
        public string ArabicFourthName { get; set; }
        public string EnglishFirstName { get; set; }
        public string EnglishSecondName { get; set; }
        public string EnglishThirdName { get; set; }
        public string EnglishFourthName { get; set; }
        public string IdentityTypeCode { get; set; }
        public string IdentityNumber { get; set; }
        public string BirthDate { get; set; }
        public string GenderCode { get; set; }
        public string NationalityCode { get; set; }
        public string IsSpecialNeeds { get; set; }
        public string SpecialNeedsTypeCode { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public List<AcademicDetail> AcademicDetails { get; set; }
        public List<HighschoolCertificate> HighschoolCertificate { get; set; }
        public string IsHighToMigrated { get; set; }
    }
}
