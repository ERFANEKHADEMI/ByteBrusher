﻿using ByteBrusher.Core.Exceptions;
using ByteBrusher.Core.File.FileTypes.Interface;

namespace ByteBrusher.Core.File.FileTypes
{
    public class Unspecified : IFileType
    {
        public string Name => throw new FileTypeNotSpecifiedException("File wasn't recognized with the suffixes in the appsettings.json List");
    }
}