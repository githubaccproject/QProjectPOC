using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.CustomerMembershipRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace TestProject.RepositoriesUnitTest
{
    [TestClass]
    public class CustomerMembershipRepositoryTests
    {
        private ICustomerMembershipRepository _customerMembershipRepository;
        private DbContextOptions<ApplicationDBContext> _dbContextOptions;

        [TestInitialize]
        public void TestInitialize()
        {
            // Set up the in-memory database for testing
            _dbContextOptions = new DbContextOptionsBuilder<ApplicationDBContext>()
                .UseInMemoryDatabase(databaseName: "TestDB")
                .Options;

            // Initialize the CustomerMembershipRepository with the in-memory database
            using (var dbContext = new ApplicationDBContext(_dbContextOptions,null))
            {
                dbContext.Database.EnsureCreated();
            }

            _customerMembershipRepository = new CustomerMembershipRepository(new ApplicationDBContext(_dbContextOptions,null));
        }

        [TestMethod]
        public async Task Add_ValidCustomerMembership_AddsCustomerMembershipToDatabase()
        {
            // Arrange
            var customerMembership = new CustomerMembership { Id = 1, IsActive = true, CustomerId = 1 };

            // Act
            _customerMembershipRepository.AddAsync(customerMembership);
            _customerMembershipRepository.SaveChangesAsync();

            // Assert
            using (var dbContext = new ApplicationDBContext(_dbContextOptions,null))
            {
                var result = await dbContext.CustomerMembership.FirstOrDefaultAsync(cm => cm.Id == customerMembership.Id);
                Assert.IsNotNull(result);
                Assert.AreEqual(true, result.IsActive);
                Assert.AreEqual(1, result.CustomerId);
            }
        }

        [TestMethod]
        public async Task Update_ValidCustomerMembership_UpdatesCustomerMembershipInDatabase()
        {
            // Arrange
            var customerMembership = new CustomerMembership { Id = 1, IsActive = true, CustomerId = 1 };
            using (var dbContext = new ApplicationDBContext(_dbContextOptions,null))
            {
                dbContext.CustomerMembership.Add(customerMembership);
                dbContext.SaveChanges();
            }

            // Act
            customerMembership.IsActive = false;
            _customerMembershipRepository.Update(customerMembership);
            _customerMembershipRepository.SaveChangesAsync();

            // Assert
            using (var dbContext = new ApplicationDBContext(_dbContextOptions,null))
            {
                var result = await dbContext.CustomerMembership.FirstOrDefaultAsync(cm => cm.Id == customerMembership.Id);
                Assert.IsNotNull(result);
                Assert.AreEqual(false, result.IsActive);
            }
        }

        [TestMethod]
        public async Task Delete_ValidCustomerMembership_DeletesCustomerMembershipFromDatabase()
        {
            // Arrange
            var customerMembership = new CustomerMembership { Id = 1, IsActive = true, CustomerId = 1 };
            using (var dbContext = new ApplicationDBContext(_dbContextOptions,null))
            {
                dbContext.CustomerMembership.Add(customerMembership);
                dbContext.SaveChanges();
            }

            // Act
            _customerMembershipRepository.Delete(customerMembership);
            _customerMembershipRepository.SaveChangesAsync();

            // Assert
            using (var dbContext = new ApplicationDBContext(_dbContextOptions,null))
            {
                var result = await dbContext.CustomerMembership.FirstOrDefaultAsync(cm => cm.Id == customerMembership.Id);
                Assert.IsNull(result);
            }
        }

        [TestCleanup]
        public void TestCleanup()
        {
            // Clean up the in-memory database after each test
            using (var dbContext = new ApplicationDBContext(_dbContextOptions,null))
            {
                dbContext.Database.EnsureDeleted();
            }
        }
    }


}
