/*using BusinessObject.Models;
using DataAccess;
using DataAccess.inter;
using HTUnitTests.In;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTUnitTests.DAO
{
    public class IngredientDAOTests
    {
        private IngredientDAO _ingredientDAO;
        private readonly IDbContextFactory _dbContextFactory;

        public IngredientDAOTests()
        {
            _dbContextFactory = new InMemoryDbContextFactory();
            _ingredientDAO = IngredientDAO.GetInstance(_dbContextFactory);
        }

        [Fact]
        public async Task CreateIngredientModelAsync_ValidIngredient_ReturnsCreatedIngredient()
        {
            // Arrange
            var newIngredient = new Ingredient
            {
                Name = "Tomato",
                Description = "Fresh and juicy"
            };

            // Act
            var createdIngredient = await _ingredientDAO.CreateIngredientModelAsync(newIngredient);

            // Assert
            Assert.NotNull(createdIngredient);
            Assert.Equal(newIngredient.Name, createdIngredient.Name);
            Assert.Equal(newIngredient.Description, createdIngredient.Description);
        }

        [Fact]
        public async Task UpdateIngredientAsync_ExistingIngredient_UpdatesAndReturnsUpdatedIngredient()
        {
            // Arrange
            var ingredient = new Ingredient { Name = "Onion", Description = "Crunchy and spicy" };
            var createdIngredient = await _ingredientDAO.CreateIngredientModelAsync(ingredient);

            // Update ingredient
            createdIngredient.Name = "Red Onion";
            createdIngredient.Description = "Sweet and crunchy";

            // Act
            var updatedIngredient = await _ingredientDAO.UpdateIngredientAsync(createdIngredient);

            // Assert
            Assert.NotNull(updatedIngredient);
            Assert.Equal("Red Onion", updatedIngredient.Name);
            Assert.Equal("Sweet and crunchy", updatedIngredient.Description);
        }
    }
}*/