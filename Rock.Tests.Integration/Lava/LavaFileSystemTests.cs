// <copyright>
// Copyright by the Spark Development Network
//
// Licensed under the Rock Community License (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.rockrms.com/license
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rock.Lava;

namespace Rock.Tests.Integration.Lava
{
    [TestClass]
    public class LavaFileSystemTests : LavaIntegrationTestBase
    {
        #region Constructors

        [ClassInitialize]
        public static void Initialize( TestContext context )
        {
            //_helper.LavaEngine.RegisterSafeType( typeof( TestPerson ) );
            //_helper.LavaEngine.RegisterSafeType( typeof( TestCampus ) );

        }

        #endregion

        /// <summary>
        /// Referencing a valid property of an input object should return the property value.
        /// </summary>
        [TestMethod]
        public void LavaFileSystem_SimpleFileAccess_ReturnsFileContents()
        {
            throw new System.Exception();

            System.Diagnostics.Debug.Print( _helper.GetTestPersonTedDecker().ToString() );

            var mergeValues = new LavaDictionary { { "CurrentPerson", _helper.GetTestPersonTedDecker() } };

            _helper.AssertTemplateOutput( "Decker", "{{ CurrentPerson.LastName }}", mergeValues );
        }

        [TestMethod]
        public void IncludeSatement_ShouldThrowFileNotFoundException_IfTheFileProviderIsNotPresent()
        {
            var fileSystem = GetMockFileProvider();

            _helper.LavaEngine.Initialize( fileSystem );

            var input = @"
{% include '_Partial.liquid' %}
";

            var mergeValues = new LavaDictionary { { "color", "red" }, { "shape", "circle" } };

            var expectedOutput = "";

            _helper.AssertTemplateOutput( expectedOutput, input, mergeValues );


            //var expression = new LiteralExpression( new StringValue( "_Partial.liquid" ) );

            //var sw = new StringWriter();

            //try
            //{
            //    await new IncludeStatement( expression ).WriteToAsync( sw, HtmlEncoder.Default, new TemplateContext() );
            //    Assert.True( false );
            //}
            //catch ( FileNotFoundException )
            //{
            //    return;
            //}

            //Assert.True( false );
        }

        private MockFileProvider GetMockFileProvider()
        {
            var fileProvider = new MockFileProvider();
            fileProvider.Add( "_Partial.liquid", @"{{ 'Partial Content' }}
Partials: '{{ Partials }}'
color: '{{ color }}'
shape: '{{ shape }}'" );

            return fileProvider;
        }

        /*
                [TestMethod]
                public async Task IncludeSatement_ShouldLoadPartial_IfThePartialsFolderExist()
                {
                    var expression = new LiteralExpression( new StringValue( "_Partial.liquid" ) );
                    var sw = new StringWriter();

                    var fileProvider = new MockFileProvider();
                    fileProvider.Add( "_Partial.liquid", @"{{ 'Partial Content' }}
        Partials: '{{ Partials }}'
        color: '{{ color }}'
        shape: '{{ shape }}'" );

                    var context = new TemplateContext
                    {
                        FileProvider = fileProvider
                    };
                    var expectedResult = @"Partial Content
        Partials: ''
        color: ''
        shape: ''";

                    await new IncludeStatement( expression ).WriteToAsync( sw, HtmlEncoder.Default, context );

                    Assert.Equal( expectedResult, sw.ToString() );
                }

                [Fact]
                public async Task IncludeSatement_WithInlinevariableAssignment_ShouldBeEvaluated()
                {
                    var expression = new LiteralExpression( new StringValue( "_Partial.liquid" ) );
                    var assignStatements = new List<AssignStatement>
                    {
                        new AssignStatement("color", new LiteralExpression(new StringValue("blue"))),
                        new AssignStatement("shape", new LiteralExpression(new StringValue("circle")))
                    };
                    var sw = new StringWriter();

                    var fileProvider = new MockFileProvider();
                    fileProvider.Add( "_Partial.liquid", @"{{ 'Partial Content' }}
        Partials: '{{ Partials }}'
        color: '{{ color }}'
        shape: '{{ shape }}'" );

                    var context = new TemplateContext
                    {
                        FileProvider = fileProvider
                    };
                    var expectedResult = @"Partial Content
        Partials: ''
        color: 'blue'
        shape: 'circle'";

                    await new IncludeStatement( expression, assignStatements: assignStatements ).WriteToAsync( sw, HtmlEncoder.Default, context );

                    Assert.Equal( expectedResult, sw.ToString() );
                }

                [TestMethod]
                public async Task IncludeSatement_WithTagParams_ShouldBeEvaluated()
                {
                    var pathExpression = new LiteralExpression( new StringValue( "color" ) );
                    var withExpression = new LiteralExpression( new StringValue( "blue" ) );
                    var sw = new StringWriter();

                    var fileProvider = new MockFileProvider();
                    fileProvider.Add( "color.liquid", @"{{ 'Partial Content' }}
        Partials: '{{ Partials }}'
        color: '{{ color }}'
        shape: '{{ shape }}'" );

                    var context = new TemplateContext
                    {
                        FileProvider = fileProvider
                    };
                    var expectedResult = @"Partial Content
        Partials: ''
        color: 'blue'
        shape: ''";

                    await new IncludeStatement( pathExpression, with: withExpression ).WriteToAsync( sw, HtmlEncoder.Default, context );

                    Assert.Equal( expectedResult, sw.ToString() );
                }

                [TestMethod]
                public async Task IncludeSatement_ShouldLimitRecursion()
                {
                    var expression = new LiteralExpression( new StringValue( "_Partial.liquid" ) );
                    var sw = new StringWriter();

                    var fileProvider = new MockFileProvider();
                    fileProvider.Add( "_Partial.liquid", @"{{ 'Partial Content' }} {% include '_Partial' %}" );

                    var context = new TemplateContext
                    {
                        FileProvider = fileProvider
                    };

                    await Assert.ThrowsAsync<InvalidOperationException>( () => new IncludeStatement( expression ).WriteToAsync( sw, HtmlEncoder.Default, context ).AsTask() );
                }

                */
    }
}
