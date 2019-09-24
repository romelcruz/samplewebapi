using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SampleWebApi.Controllers;
using SampleWebApi.Models;
using System;
using System.Collections.Generic;
using Xunit;

namespace sampleWebApiTest
{
    public class TrainingControllerTest
    {
        private TrainingContext context;
        private TrainingsController myController;

        public TrainingControllerTest()
        {
            var options = new DbContextOptionsBuilder<TrainingContext>()
                      .UseInMemoryDatabase(Guid.NewGuid().ToString())
                      .Options;
            context = new TrainingContext(options);
            myController = new TrainingsController(context);
        }

        [Fact]
        public async void Get_returns_list_of_trainings()
        {
            //Arrange
            var sampleTraining1 = new Training() { Id = 1, StartDate = new DateTime(2019, 09, 20), EndDate = new DateTime(2019, 09, 21), TrainingName = "Asp.net Training" };
            var sampleTraining2 = new Training() { Id = 2, StartDate = new DateTime(2019, 09, 21), EndDate = new DateTime(2019, 09, 22), TrainingName = "Web Api training" };
            context.AddRange(sampleTraining1, sampleTraining2);
            context.SaveChanges();

            //Act
            var taskResult = await myController.GetTrainings();

            //Assert
            Assert.IsType<ActionResult<IEnumerable<Training>>>(taskResult);
        }

        [Fact]
        public async void Posted_Training_are_retrievable_on_get()
        {
            //Arrange
            var sampleTraining1 = new Training() { Id = 3, StartDate = new DateTime(2019, 09, 20), EndDate = new DateTime(2019, 09, 21), TrainingName = "SSRS Training" };

            //Act
            var taskResult = await myController.PostTraining(sampleTraining1);
            var getResult = await myController.GetTraining(sampleTraining1.Id);

            //Assert
            Assert.Equal(getResult.Value.TrainingName, sampleTraining1.TrainingName);
        }

        [Fact]
        public async void PutTraining_updates_database_record()
        {
            //Arrange
            var sampleTraining1 = new Training() { Id = 3, StartDate = new DateTime(2019, 09, 19), EndDate = new DateTime(2019, 09, 22), TrainingName = "SSIS Training" };

            //Act
            var taskResult = await myController.PutTraining(3, sampleTraining1);
            var getResult = await myController.GetTraining(sampleTraining1.Id);

            //Assert
            Assert.Equal(getResult.Value.TrainingName, sampleTraining1.TrainingName);
        }

        [Fact]
        public async void DeleteTraining_removes_record_from_database()
        {
            //Arrange
            int idToDelete = 3;
            var sampleTraining1 = new Training() { Id = idToDelete, StartDate = new DateTime(2019, 09, 19), EndDate = new DateTime(2019, 09, 22), TrainingName = "C# Training" };
            context.Add(sampleTraining1);
            context.SaveChanges();

            var expectedType = typeof(NotFoundResult);

            //Act
            var taskResult = await myController.DeleteTraining(idToDelete);
            var getResult = await myController.GetTraining(idToDelete);

            //Assert
            Assert.Equal(taskResult.Value.TrainingName, sampleTraining1.TrainingName);
            Assert.IsType(expectedType, getResult.Result);
        }

        [Fact]
        public async void End_date_before_start_date_returns_bad_request_400()
        {
            //Arrange
            var sampleTraining1 = new Training() { Id = 5, StartDate = new DateTime(2019, 09, 22), EndDate = new DateTime(2019, 09, 19), TrainingName = "WPF Training" };

            var expectedType = typeof(BadRequestResult);

            //Act
            var taskResult = await myController.PostTraining(sampleTraining1);

            //Assert
            Assert.IsType(expectedType, taskResult.Result);
        }
    }
}
