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
using System;
using System.Collections.Generic;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using Rock.Lava;

namespace Rock.Tests.Integration.Lava
{
    public class MockFileProvider : IFileProvider, ILavaFileSystem
    {
        private Dictionary<string, MockFileInfo> _files = new Dictionary<string, MockFileInfo>();

        public MockFileProvider()
        {
        }

        public IDirectoryContents GetDirectoryContents( string subpath )
        {
            throw new NotImplementedException();
        }

        public IFileInfo GetFileInfo( string path )
        {
            if ( _files.ContainsKey( path ) )
            {
                return _files[path];
            }
            else
            {
                return null;
            }
        }

        public MockFileProvider Add( string path, string content )
        {
            _files[path] = new MockFileInfo( path, content );
            return this;
        }

        public IChangeToken Watch( string filter )
        {
            throw new NotImplementedException();
        }

        public string ReadTemplateFile( ILavaContext context, string templateName )
        {
            throw new NotImplementedException();
        }
    }
}
