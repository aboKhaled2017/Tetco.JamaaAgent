namespace Domain.Models
{
    public class SchemaStudent
    {
        public string InstituteCode { get; set; }
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
        public string IsHighToMigrated { get; set; }

        #region
        public string HasScholarship { get; set; }
        public string ScholarshipTypeCode { get; set; }
        public string ScholarShipClassificationCode { get; set; }
        public string StudentAcademicNumber { get; set; }
        public string ScientificDegreeCode { get; set; }
        public string AcademicStatusCode { get; set; }
        public string StudyLocationCode { get; set; }
        public string CurrentCollegeCode { get; set; }
        public string AcceptedCollegeCode { get; set; }
        public string SectionCode { get; set; }
        public string MajorCode { get; set; }
        public string MinorCode { get; set; }
        public string SpecialtyClassificationCode { get; set; }
        public string EducationalSubLevelCode { get; set; }
        public string IncludedSpecializationCode { get; set; }
        public string StudyProgramPeriodUnitCode { get; set; }
        public string StudyProgramPeriod { get; set; }
        public string RequestedCreditHoursCount { get; set; }
        public string RegisteredCreditHoursCount { get; set; }
        public string PassedCreditHoursCount { get; set; }
        public string RemainingCreditHoursCount { get; set; }
        public string RegistrationStatusCode { get; set; }
        public string CurrentAcademicYearDate { get; set; }
        public string CurrentSemesterCode { get; set; }
        public string GraduationDate { get; set; }
        public string GraduationSemesterCode { get; set; }
        public string StudyTypeCode { get; set; }
        public string AdmissionDate { get; set; }
        public string HasStudentReward { get; set; }
        public string StudentRewardAmount { get; set; }
        public string GPATypeCode { get; set; }
        public string GPA { get; set; }
        public string RatingCode { get; set; }
        public string HasThesis { get; set; }
        public string ThesisTitle { get; set; }
        public string LastAcademicStatusUpdateDate { get; set; }
        public string DisclaimerDate { get; set; }
        public string IsLastAcademicDataRecord { get; set; }
        #endregion

        #region
        public string IsObtainedInSaudiArabia { get; set; }
        public string CityCode { get; set; }
        public string CertificateTypeCode { get; set; }
        public string ObtainingYear { get; set; }
        public string Percentage { get; set; }
        #endregion

    }
}
