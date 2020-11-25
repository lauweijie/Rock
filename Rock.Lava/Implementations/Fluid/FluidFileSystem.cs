using System;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

namespace Rock.Lava
{
    /// <summary>
    /// An implementation of a Lava File System for the Fluid framework.
    /// </summary>
    public class FluidFileSystem : IFileProvider, ILavaFileSystem
    {
        private ILavaFileSystem _fileSystem = null;

        public FluidFileSystem( ILavaFileSystem fileSystem )
        {
            _fileSystem = fileSystem;
        }

        public IDirectoryContents GetDirectoryContents( string subpath )
        {
            // Directory listing is not supported.
            return null;

        }

        public IFileInfo GetFileInfo( string subpath )
        {
            var text = _fileSystem.ReadTemplateFile( null, subpath );

            throw new NotImplementedException();
        }

        public string ReadTemplateFile( ILavaContext context, string templateName )
        {
            throw new NotImplementedException();
        }

        public IChangeToken Watch( string filter )
        {
            // File system monitoring is not supported.
            return null;
        }
    }
}