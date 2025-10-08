using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.DataAnnotations;
using TitanHelp.Application.DTOs;

namespace TitanHelp.Application.Tests.DTOs
{
    [TestClass]
    public class TicketDtoTests
    {
        #region Constructor and Default Values Tests

        [TestMethod]
        public void TicketDto_DefaultConstructor_SetsDefaultValues()
        {
            // Act
            var dto = new TicketDto();

            // Assert
            Assert.AreEqual(0, dto.Id);
            Assert.AreEqual(string.Empty, dto.Name);
            Assert.AreEqual(string.Empty, dto.ProblemDescription);
            Assert.AreEqual("Open", dto.Status);
            Assert.AreEqual("Medium", dto.Priority);
        }

        [TestMethod]
        public void TicketDto_DefaultStatus_IsOpen()
        {
            // Act
            var dto = new TicketDto();

            // Assert
            Assert.AreEqual("Open", dto.Status);
        }

        [TestMethod]
        public void TicketDto_DefaultPriority_IsMedium()
        {
            // Act
            var dto = new TicketDto();

            // Assert
            Assert.AreEqual("Medium", dto.Priority);
        }

        #endregion

        #region Property Setting Tests

        [TestMethod]
        public void TicketDto_CanSetAllProperties()
        {
            // Arrange
            var date = new DateTime(2024, 1, 1);

            // Act
            var dto = new TicketDto
            {
                Id = 1,
                Name = "Test Ticket",
                Date = date,
                ProblemDescription = "Test Description",
                Status = "Closed",
                Priority = "High"
            };

            // Assert
            Assert.AreEqual(1, dto.Id);
            Assert.AreEqual("Test Ticket", dto.Name);
            Assert.AreEqual(date, dto.Date);
            Assert.AreEqual("Test Description", dto.ProblemDescription);
            Assert.AreEqual("Closed", dto.Status);
            Assert.AreEqual("High", dto.Priority);
        }

        [TestMethod]
        public void TicketDto_Id_CanBeSet()
        {
            // Arrange
            var dto = new TicketDto();

            // Act
            dto.Id = 42;

            // Assert
            Assert.AreEqual(42, dto.Id);
        }

        [TestMethod]
        public void TicketDto_Name_CanBeSet()
        {
            // Arrange
            var dto = new TicketDto();

            // Act
            dto.Name = "New Name";

            // Assert
            Assert.AreEqual("New Name", dto.Name);
        }

        [TestMethod]
        public void TicketDto_Date_CanBeSet()
        {
            // Arrange
            var dto = new TicketDto();
            var testDate = new DateTime(2024, 6, 15);

            // Act
            dto.Date = testDate;

            // Assert
            Assert.AreEqual(testDate, dto.Date);
        }

        [TestMethod]
        public void TicketDto_ProblemDescription_CanBeSet()
        {
            // Arrange
            var dto = new TicketDto();

            // Act
            dto.ProblemDescription = "New Description";

            // Assert
            Assert.AreEqual("New Description", dto.ProblemDescription);
        }

        [TestMethod]
        public void TicketDto_Status_CanBeChanged()
        {
            // Arrange
            var dto = new TicketDto();

            // Act
            dto.Status = "In Progress";

            // Assert
            Assert.AreEqual("In Progress", dto.Status);
        }

        [TestMethod]
        public void TicketDto_Priority_CanBeChanged()
        {
            // Arrange
            var dto = new TicketDto();

            // Act
            dto.Priority = "High";

            // Assert
            Assert.AreEqual("High", dto.Priority);
        }

        #endregion

        #region Validation Tests - Name

        [TestMethod]
        public void TicketDto_Name_Required_FailsValidation()
        {
            // Arrange
            var dto = new TicketDto
            {
                ProblemDescription = "Test",
                Name = "" // Empty name
            };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(dto, context, results, true);

            // Assert
            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Any(r => r.MemberNames.Contains("Name")));
        }

        [TestMethod]
        public void TicketDto_Name_ExceedsMaxLength_FailsValidation()
        {
            // Arrange
            var dto = new TicketDto
            {
                Name = new string('A', 101), // Exceeds 100 character limit
                ProblemDescription = "Test"
            };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(dto, context, results, true);

            // Assert
            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Any(r =>
                r.MemberNames.Contains("Name") &&
                r.ErrorMessage.Contains("100")));
        }

        [TestMethod]
        public void TicketDto_Name_AtMaxLength_PassesValidation()
        {
            // Arrange
            var dto = new TicketDto
            {
                Name = new string('A', 100), // Exactly 100 characters
                ProblemDescription = "Test"
            };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(dto, context, results, true);

            // Assert
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void TicketDto_Name_WithSpecialCharacters_PassesValidation()
        {
            // Arrange
            var dto = new TicketDto
            {
                Name = "Test-Ticket_#123",
                ProblemDescription = "Test"
            };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(dto, context, results, true);

            // Assert
            Assert.IsTrue(isValid);
        }

        #endregion

        #region Validation Tests - ProblemDescription

        [TestMethod]
        public void TicketDto_ProblemDescription_Required_FailsValidation()
        {
            // Arrange
            var dto = new TicketDto
            {
                Name = "Test",
                ProblemDescription = "" // Empty description
            };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(dto, context, results, true);

            // Assert
            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Any(r => r.MemberNames.Contains("ProblemDescription")));
        }

        [TestMethod]
        public void TicketDto_ProblemDescription_ExceedsMaxLength_FailsValidation()
        {
            // Arrange
            var dto = new TicketDto
            {
                Name = "Test",
                ProblemDescription = new string('B', 1001) // Exceeds 1000 character limit
            };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(dto, context, results, true);

            // Assert
            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Any(r =>
                r.MemberNames.Contains("ProblemDescription") &&
                r.ErrorMessage.Contains("1000")));
        }

        [TestMethod]
        public void TicketDto_ProblemDescription_AtMaxLength_PassesValidation()
        {
            // Arrange
            var dto = new TicketDto
            {
                Name = "Test",
                ProblemDescription = new string('B', 1000) // Exactly 1000 characters
            };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(dto, context, results, true);

            // Assert
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void TicketDto_ProblemDescription_WithMultipleLines_PassesValidation()
        {
            // Arrange
            var dto = new TicketDto
            {
                Name = "Test",
                ProblemDescription = "Line 1\nLine 2\nLine 3"
            };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(dto, context, results, true);

            // Assert
            Assert.IsTrue(isValid);
        }

        #endregion

        #region Full Validation Tests

        [TestMethod]
        public void TicketDto_ValidDto_PassesValidation()
        {
            // Arrange
            var dto = new TicketDto
            {
                Name = "Valid Name",
                ProblemDescription = "Valid Description",
                Status = "Open",
                Priority = "Medium"
            };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(dto, context, results, true);

            // Assert
            Assert.IsTrue(isValid);
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void TicketDto_AllFieldsMissing_FailsValidation()
        {
            // Arrange
            var dto = new TicketDto
            {
                Name = "",
                ProblemDescription = ""
            };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(dto, context, results, true);

            // Assert
            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Count >= 2); // Both Name and ProblemDescription
        }

        [TestMethod]
        public void TicketDto_MinimalValidData_PassesValidation()
        {
            // Arrange
            var dto = new TicketDto
            {
                Name = "A",
                ProblemDescription = "B"
            };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(dto, context, results, true);

            // Assert
            Assert.IsTrue(isValid);
        }

        #endregion

        #region Status and Priority Tests

        [TestMethod]
        public void TicketDto_Status_AcceptsOpenValue()
        {
            // Arrange
            var dto = new TicketDto();

            // Act
            dto.Status = "Open";

            // Assert
            Assert.AreEqual("Open", dto.Status);
        }

        [TestMethod]
        public void TicketDto_Status_AcceptsInProgressValue()
        {
            // Arrange
            var dto = new TicketDto();

            // Act
            dto.Status = "In Progress";

            // Assert
            Assert.AreEqual("In Progress", dto.Status);
        }

        [TestMethod]
        public void TicketDto_Status_AcceptsClosedValue()
        {
            // Arrange
            var dto = new TicketDto();

            // Act
            dto.Status = "Closed";

            // Assert
            Assert.AreEqual("Closed", dto.Status);
        }

        [TestMethod]
        public void TicketDto_Priority_AcceptsLowValue()
        {
            // Arrange
            var dto = new TicketDto();

            // Act
            dto.Priority = "Low";

            // Assert
            Assert.AreEqual("Low", dto.Priority);
        }

        [TestMethod]
        public void TicketDto_Priority_AcceptsMediumValue()
        {
            // Arrange
            var dto = new TicketDto();

            // Act
            dto.Priority = "Medium";

            // Assert
            Assert.AreEqual("Medium", dto.Priority);
        }

        [TestMethod]
        public void TicketDto_Priority_AcceptsHighValue()
        {
            // Arrange
            var dto = new TicketDto();

            // Act
            dto.Priority = "High";

            // Assert
            Assert.AreEqual("High", dto.Priority);
        }

        #endregion

        #region Validation Error Message Tests

        [TestMethod]
        public void TicketDto_Name_RequiredError_ContainsCorrectMessage()
        {
            // Arrange
            var dto = new TicketDto { ProblemDescription = "Test" };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            // Act
            Validator.TryValidateObject(dto, context, results, true);

            // Assert
            var nameError = results.FirstOrDefault(r => r.MemberNames.Contains("Name"));
            Assert.IsNotNull(nameError);
            Assert.IsTrue(nameError.ErrorMessage.Contains("required"),
                $"Expected error message to contain 'required', but got: {nameError.ErrorMessage}");
        }

        [TestMethod]
        public void TicketDto_Name_MaxLengthError_ContainsCorrectMessage()
        {
            // Arrange
            var dto = new TicketDto
            {
                Name = new string('A', 101),
                ProblemDescription = "Test"
            };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            // Act
            Validator.TryValidateObject(dto, context, results, true);

            // Assert
            var nameError = results.FirstOrDefault(r => r.MemberNames.Contains("Name"));
            Assert.IsNotNull(nameError);
            Assert.IsTrue(nameError.ErrorMessage.Contains("100"),
                $"Expected error message to contain '100', but got: {nameError.ErrorMessage}");
        }

        [TestMethod]
        public void TicketDto_ProblemDescription_RequiredError_ContainsCorrectMessage()
        {
            // Arrange
            var dto = new TicketDto { Name = "Test" };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            // Act
            Validator.TryValidateObject(dto, context, results, true);

            // Assert
            var descError = results.FirstOrDefault(r => r.MemberNames.Contains("ProblemDescription"));
            Assert.IsNotNull(descError);
            Assert.IsTrue(descError.ErrorMessage.Contains("required"),
                $"Expected error message to contain 'required', but got: {descError.ErrorMessage}");
        }

        [TestMethod]
        public void TicketDto_ProblemDescription_MaxLengthError_ContainsCorrectMessage()
        {
            // Arrange
            var dto = new TicketDto
            {
                Name = "Test",
                ProblemDescription = new string('B', 1001)
            };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            // Act
            Validator.TryValidateObject(dto, context, results, true);

            // Assert
            var descError = results.FirstOrDefault(r => r.MemberNames.Contains("ProblemDescription"));
            Assert.IsNotNull(descError);
            Assert.IsTrue(descError.ErrorMessage.Contains("1000"),
                $"Expected error message to contain '1000', but got: {descError.ErrorMessage}");
        }

        #endregion

        #region Edge Case Tests

        [TestMethod]
        public void TicketDto_Name_WithWhitespace_IsValid()
        {
            // Arrange
            var dto = new TicketDto
            {
                Name = "   Ticket Name   ",
                ProblemDescription = "Test"
            };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(dto, context, results, true);

            // Assert
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void TicketDto_Date_FutureDate_IsValid()
        {
            // Arrange
            var futureDate = DateTime.Now.AddDays(10);
            var dto = new TicketDto
            {
                Name = "Test",
                ProblemDescription = "Test",
                Date = futureDate
            };

            // Act & Assert
            Assert.AreEqual(futureDate, dto.Date);
        }

        [TestMethod]
        public void TicketDto_Date_PastDate_IsValid()
        {
            // Arrange
            var pastDate = DateTime.Now.AddDays(-10);
            var dto = new TicketDto
            {
                Name = "Test",
                ProblemDescription = "Test",
                Date = pastDate
            };

            // Act & Assert
            Assert.AreEqual(pastDate, dto.Date);
        }

        [TestMethod]
        public void TicketDto_ProblemDescription_WithSpecialCharacters_IsValid()
        {
            // Arrange
            var dto = new TicketDto
            {
                Name = "Test",
                ProblemDescription = "Test @#$%^&*()_+-=[]{}|;:',.<>?/"
            };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(dto, context, results, true);

            // Assert
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void TicketDto_Status_CustomValue_IsAccepted()
        {
            // Arrange
            var dto = new TicketDto();

            // Act
            dto.Status = "Pending Review";

            // Assert
            Assert.AreEqual("Pending Review", dto.Status);
        }

        #endregion
    }
}