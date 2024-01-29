
using Application.Common.Models;
using Bogus;

namespace Infrastructure.Respos.Dummy
{
    public class DataGenerator
    {
        public List<Student> GenerateStudents(int numberOfStudents)
        {
            // Define the 'Faker' for AcademicDetail
            Faker<AcademicDetail> AcademicDetailGenerator(string studentUniqueId) => new Faker<AcademicDetail>()
             // Use the same studentUniqueId for the AcademicDetail
             .RuleFor(a => a.StudentUniqueId, studentUniqueId)
                .RuleFor(a => a.HasScholarship, f => f.Random.Bool().ToString())
                .RuleFor(a => a.ScholarshipTypeCode, f => f.Random.String2(5))
                .RuleFor(a => a.ScholarShipClassificationCode, f => f.Random.String2(5))
                .RuleFor(a => a.StudentAcademicNumber, f => f.Random.String2(10))
                .RuleFor(a => a.ScientificDegreeCode, f => f.Random.String2(5))
                .RuleFor(a => a.AcademicStatusCode, f => f.Random.String2(5))
                .RuleFor(a => a.StudyLocationCode, f => f.Address.CountryCode())
                .RuleFor(a => a.InstituteCode, f => f.Random.String2(5))
                .RuleFor(a => a.CurrentCollegeCode, f => f.Random.String2(5))
                .RuleFor(a => a.AcceptedCollegeCode, f => f.Random.String2(5))
                .RuleFor(a => a.SectionCode, f => f.Random.String2(5))
                .RuleFor(a => a.MajorCode, f => f.Random.String2(5))
                .RuleFor(a => a.MinorCode, f => f.Random.String2(5))
                .RuleFor(a => a.SpecialtyClassificationCode, f => f.Random.String2(5))
                .RuleFor(a => a.EducationalSubLevelCode, f => f.Random.String2(5))
                .RuleFor(a => a.IncludedSpecializationCode, f => f.Random.String2(5))
                .RuleFor(a => a.StudyProgramPeriodUnitCode, f => f.Random.String2(5))
                .RuleFor(a => a.StudyProgramPeriod, f => f.Date.Future().ToString("yyyy-MM-dd"))
                .RuleFor(a => a.RequestedCreditHoursCount, f => f.Random.Number(1, 20).ToString())
                .RuleFor(a => a.RegisteredCreditHoursCount, f => f.Random.Number(1, 20).ToString())
                .RuleFor(a => a.PassedCreditHoursCount, f => f.Random.Number(1, 20).ToString())
                .RuleFor(a => a.RemainingCreditHoursCount, f => f.Random.Number(1, 20).ToString())
                .RuleFor(a => a.RegistrationStatusCode, f => f.Random.String2(5))
                .RuleFor(a => a.CurrentAcademicYearDate, f => f.Date.Past(1).ToString("yyyy-MM-dd"))
                .RuleFor(a => a.CurrentSemesterCode, f => f.Random.String2(5))
                .RuleFor(a => a.GraduationDate, f => f.Date.Future(1).ToString("yyyy-MM-dd"))
                .RuleFor(a => a.GraduationSemesterCode, f => f.Random.String2(5))
                .RuleFor(a => a.StudyTypeCode, f => f.Random.String2(5))
                .RuleFor(a => a.AdmissionDate, f => f.Date.Past(5).ToString("yyyy-MM-dd"))
                .RuleFor(a => a.HasStudentReward, f => f.Random.Bool().ToString())
                .RuleFor(a => a.StudentRewardAmount, f => f.Finance.Amount(100, 1000).ToString("0.00"))
                .RuleFor(a => a.GPATypeCode, f => f.Random.String2(5))
                .RuleFor(a => a.GPA, f => f.Random.Double(2.0, 4.0).ToString("0.00"))
                .RuleFor(a => a.RatingCode, f => f.Random.String2(5))
                .RuleFor(a => a.HasThesis, f => f.Random.Number(0, 1).ToString())
                .RuleFor(a => a.ThesisTitle, f => f.Lorem.Sentence())
                .RuleFor(a => a.LastAcademicStatusUpdateDate, f => f.Date.Recent().ToString("yyyy-MM-dd"))
                .RuleFor(a => a.DisclaimerDate, f => f.Date.Recent().ToString("yyyy-MM-dd"))
                .RuleFor(a => a.IsLastAcademicDataRecord, f => f.Random.Bool().ToString());

            // Define the 'Faker' for HighschoolCertificate
            Faker<HighschoolCertificate> HighschoolCertificateGenerator(string studentUniqueId) => new Faker<HighschoolCertificate>()
          // Use the same studentUniqueId for the HighschoolCertificate
          .RuleFor(h => h.StudentUniqueId, studentUniqueId)
                .RuleFor(h => h.InstituteCode, f => f.Random.String2(5))
                .RuleFor(h => h.IsObtainedInSaudiArabia, f => f.Random.Bool().ToString())
                .RuleFor(h => h.CityCode, f => f.Address.City())
                .RuleFor(h => h.CertificateTypeCode, f => f.Random.String2(5))
                .RuleFor(h => h.ObtainingYear, f => f.Date.Past(10).Year.ToString())
                .RuleFor(h => h.Percentage, f => f.Random.Double(50, 100).ToString("0.00"));


            // Define the 'Faker' for Student
            var studentFaker = new Faker<Student>()
              .RuleFor(s => s.StudentUniqueId, f => f.Random.Long(1000000000, 9999999999).ToString())
          .RuleFor(s => s.ArabicFirstName, f => f.Name.FirstName())
        .RuleFor(s => s.ArabicSecondName, f => f.Name.FirstName())
        .RuleFor(s => s.ArabicThirdName, f => f.Name.FirstName())
        .RuleFor(s => s.ArabicFourthName, f => f.Name.FirstName())
        .RuleFor(s => s.EnglishFirstName, f => f.Name.FirstName())
        .RuleFor(s => s.EnglishSecondName, f => f.Name.FirstName())
        .RuleFor(s => s.EnglishThirdName, f => f.Name.FirstName())
        .RuleFor(s => s.EnglishFourthName, f => f.Name.FirstName())
        .RuleFor(s => s.IdentityTypeCode, f => f.Random.String2(5))
        .RuleFor(s => s.IdentityNumber, f => f.Random.String2(10))
        .RuleFor(s => s.BirthDate, f => f.Date.Past(20).ToString("yyyy-MM-dd"))
        .RuleFor(s => s.GenderCode, f => f.PickRandom(new[] { "M", "F" }))
        .RuleFor(s => s.NationalityCode, f => f.Address.CountryCode())
        .RuleFor(s => s.IsSpecialNeeds, f => f.Random.Bool().ToString())
        .RuleFor(s => s.IsHighToMigrated, f => f.Random.Bool().ToString())
        .RuleFor(s => s.SpecialNeedsTypeCode, f => f.Random.String2(5))
        .RuleFor(s => s.MobileNumber, f => f.Phone.PhoneNumber())
        .RuleFor(s => s.Email, f => f.Internet.Email())
        .FinishWith((f, s) =>
        {
            // Use the StudentUniqueId for the related AcademicDetails and HighschoolCertificates
            s.AcademicDetails = AcademicDetailGenerator(s.StudentUniqueId).Generate(f.Random.Int(1, 5));
            s.HighschoolCertificate = HighschoolCertificateGenerator(s.StudentUniqueId).Generate(f.Random.Int(1, 5));
        }); // Generates between 1 and 5 HighschoolCertificates

            var students = studentFaker.Generate(numberOfStudents);
            return students;
        }

    }
}





