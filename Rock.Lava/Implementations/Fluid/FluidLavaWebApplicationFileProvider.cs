using System;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

namespace Rock.Lava
{
    /// <summary>
    /// A file provider for a web application file system that is compatible with the Fluid framework.
    /// </summary>
    public class FluidLavaWebApplicationFileProvider : ILavaFileSystem, IFileProvider
    {
        public IDirectoryContents GetDirectoryContents( string subpath )
        {
            throw new NotImplementedException();
        }

        public IFileInfo GetFileInfo( string subpath )
        {
            throw new NotImplementedException();
        }

        public IChangeToken Watch( string filter )
        {
            throw new NotImplementedException();
        }

        #region ILavaFileSystem

        public string ReadTemplateFile( ILavaContext context, string templateName )
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}