using System.Collections.Generic;
using Glass.Mapper.Sc;
using Glass.Mapper.Sc.Web;
using FluentAssertions;
using Helixbase.Foundation.XConnect.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Ploeh.AutoFixture;
using System.Threading.Tasks;
using Sitecore.XConnect.Collection.Model;
using System;

namespace Helixbase.Foundation.XConnect.Tests.Repositories
{
    [TestClass]
    public class ContactRepositoryTests
    {
        private IRequestContext _requestContext;
        private IContactRepository _contactRepository;
        private ILogRepository _logRepository;
        private IInteractionRepository _interactionRepository;
        private IReferenceRepository _referenceRepository;
        private IConfigurationBuilder _configurationBuilder;
        [TestInitialize]
        public void Setup()
        {
            _requestContext = Substitute.For<IRequestContext>();
            _logRepository = Substitute.For<ILogRepository>();
            _configurationBuilder = Substitute.For<ConfigurationBuilder>();
            _contactRepository = new ContactRepository(_logRepository);
            _interactionRepository = new InteractionRepository(_logRepository);
            _referenceRepository = new ReferenceRepository(_logRepository, _configurationBuilder);

        }

        [TestMethod]
        public async Task CreateContactAction_GivenModel_ReturnsCreatedItem()
        {
            var cfg = new ConfigurationBuilder().GetClientConfiguration(
                "https://helixbasexconnect.dev.local",
                "https://helixbasexconnect.dev.local",
                "https://helixbasexconnect.dev.local",
                "368779A310FBF4EDDC259092C5EDD0377B637EF9");

           await cfg.InitializeAsync(); ;
            var createdModel =await _contactRepository.CreateContact(cfg,"test","13693129369",
                new Sitecore.XConnect.Collection.Model.PersonalInformation()
                { FirstName="b",
                    Nickname="test"
                });

            var contact = await _contactRepository.GetContact(cfg,"test", "13693129369");
            Assert.IsNotNull(createdModel);
            foreach(var i in contact.Identifiers)
            {
                createdModel.Identifier.Should().Be(i.Identifier);
                break;
            }
       
        }
        [TestMethod]
        public async Task UpdateContactAction_GivenModel_ReturnsCreatedItem()
        {
            var cfg = new ConfigurationBuilder().GetClientConfiguration(
                "https://helixbasexconnect.dev.local",
                "https://helixbasexconnect.dev.local",
                "https://helixbasexconnect.dev.local",
                "368779A310FBF4EDDC259092C5EDD0377B637EF9");

            await cfg.InitializeAsync();

            PersonalInformation updatedPersonalInformation = new PersonalInformation()
            {
                JobTitle = "Senior Programmer Writer",
                 Birthdate= DateTime.Now,
                  Gender="man",
                   Nickname="aaaa"

            };


           var updateModel = await _contactRepository.UpdateContact(cfg, "test", "13693129369",
                 updatedPersonalInformation);

            var contact = await _contactRepository.GetContact(cfg, "test", "13693129369");
            Assert.IsNotNull(updateModel);
          
                updateModel.Personal().JobTitle.Should().Be(updatedPersonalInformation.JobTitle);
            var ipInfo = new IpInfo("127.0.0.1") { BusinessName = "Home" };
            var interaction = await _interactionRepository.RegisterGoalInteraction(cfg, contact,
                "{15C3060B-CADC-47CA-B1FB-6931A43D48A2}", "{28A7C944-B8B6-45AD-A635-6F72E8F81F69}",
                ipInfo);

            Assert.IsNotNull(interaction);

            var definition = await _referenceRepository.GetDefinition(
                "goal test" ,
                "{28A7C944-B8B6-45AD-A635-6F72E8F81F69}",

                "https://helixbasexconnect.dev.local",


                "368779A310FBF4EDDC259092C5EDD0377B637EF9");
            if (definition == null)
            {
                definition = await _referenceRepository.CreateDefinition(
                   "goal test" ,
                    "{28A7C944-B8B6-45AD-A635-6F72E8F81F69}",
                   "	Instant Demo",
                   "https://helixbasexconnect.dev.local",
                  "368779A310FBF4EDDC259092C5EDD0377B637EF9");
            }
            Assert.IsNotNull(definition);


            var contactwithinteractions = await _contactRepository.GetContactWithInteractions(cfg, "test","13693129369", 
                
                DateTime.MinValue, DateTime.MaxValue);
            Assert.IsNotNull(contactwithinteractions);

            var startDate = DateTime.Now.AddDays(-7);
            var endDate = startDate.AddDays(10);
            var interactionswithsearch = await _interactionRepository.SearchInteractionsByDate(cfg, startDate, endDate);
            Assert.IsNotNull(interactionswithsearch);

 

   

        }
        [TestMethod]
        public async Task DeleteContactAction_GivenModel_ReturnsCreatedItem()
        {
            var cfg = new ConfigurationBuilder().GetClientConfiguration(
                "https://helixbasexconnect.dev.local",
                "https://helixbasexconnect.dev.local",
                "https://helixbasexconnect.dev.local",
                "368779A310FBF4EDDC259092C5EDD0377B637EF9");

            await cfg.InitializeAsync();

            PersonalInformation updatedPersonalInformation = new PersonalInformation()
            {
                JobTitle = "Senior Programmer Writer",
                Birthdate = DateTime.Now,
                Gender = "man",
                Nickname = "aaaa"

            };


            var deleteModel = await _contactRepository.DeleteContact(cfg, "test", "13693129369"
                  );

     
            Assert.IsNotNull(deleteModel);

         


        }

    }
}
