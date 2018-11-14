using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Newtonsoft.Json.Linq;
using Shouldly;
using Swashbuckle.AspNetCore.Filters.Test.TestFixtures.Fakes;
using Swashbuckle.AspNetCore.Filters.Test.TestFixtures.Fakes.Examples;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using Xunit;
using System;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Serialization;
using NSubstitute;
using FakeControllers = Swashbuckle.AspNetCore.Filters.Test.TestFixtures.Fakes.FakeControllers;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Reflection;

namespace Swashbuckle.AspNetCore.Filters.Test
{
    public class AccessDocumentFilterTests
    {
        private readonly IHttpContextAccessor httpContext;
        private readonly IDocumentFilter sut;

        public AccessDocumentFilterTests()
        {
            httpContext = Substitute.For<IHttpContextAccessor>();
            sut = new AccessDocumentFilter(httpContext);
        }

        [Fact]
        public void Apply_SetsResponseDescriptions()
        {
            // Arrange
            var document = new SwaggerDocument();
            var filterContext = FilterContextFor<FakeControllers.SwaggerResponseController>();

            // Act
            sut.Apply(document, filterContext);

            // Assert
            var schema = filterContext.SchemaRegistry.Definitions["PersonResponse"];
            schema.Properties["first"].Description.ShouldBe("The first name of the person");
            schema.Properties["last"].Description.ShouldBe("The last name of the person");
            schema.Properties["age"].Description.ShouldBe("His age, in years");
        }
        private DocumentFilterContext FilterContextFor<TController>()
        {
            return new DocumentFilterContext(
                null,
                new[]
                {
                    new ApiDescription
                    {
                        ActionDescriptor = new ControllerActionDescriptor
                        {
                            ControllerTypeInfo = typeof(TController).GetTypeInfo(),
                            ControllerName = typeof(TController).Name
                        }
                    },
                },
                null);
        }
    }
}
