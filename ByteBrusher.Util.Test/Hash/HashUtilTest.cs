﻿using ByteBrusher.Util.Interface.Hash.Models;
using Moq;
using FluentAssertions;
using NUnit.Framework;
using ByteBrusher.Util.Resource.Hash;

namespace ByteBrusher.Util.Test.Hash;

using Core.File;
using Core.File.FileTypes;
using Core.File.FileTypes.Interface;

public class HashUtilTest
{
    private Mock<IFileAbstraction> _fileStreamMock = new();

    [Test]
    public async Task CompareChecksumAsync_whenMockedStream_IsEqual()
    {
        // Arrange
        _fileStreamMock = new Mock<IFileAbstraction>();
        _fileStreamMock.Setup(x => x.StartStream(It.IsAny<string>()))
                       .Returns(() => new MemoryStream(new byte[0]));
        HashUtil hashUtil = new HashUtil(_fileStreamMock.Object);
        // Act
        bool result = await hashUtil.CompareChecksumAsync("SameFile", "SameFile");

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public async Task HashUtil_WhenFileNotFound_Throw()
    {
        // Arrange
        _fileStreamMock.Setup(x => x.StartStream(It.IsAny<string>())).Throws<FileNotFoundException>();

        // Act
        HashUtil hashUtil = new(_fileStreamMock.Object);

        // Assert
        Assert.ThrowsAsync<FileNotFoundException>(async () => await hashUtil.CompareChecksumAsync("test", "fileToCompare"));
    }

    [Test]
    public async Task GetDuplicatesAsync_ShouldReturnExpectedDuplicates()
    {
        // Arrange
        var files = new List<FoundFile>
        {
            new FoundFile { fileInfo  = new FileInfo("file1.txt"), fileType = new Video() },
            new FoundFile { fileInfo = new FileInfo("file2.txt"), fileType = new Image() },
        };

        _fileStreamMock.Setup(x => x.StartStream(It.IsAny<string>()))
                       .Returns(() => new MemoryStream(new byte[0]));

        // Act
        HashUtil hashUtil = new(_fileStreamMock.Object);
        var result = await hashUtil.GetDuplicatesAsync(files);

        // Assert
        result.Should().HaveCount(2);
        //result.Should().ContainKey("file1.txt");
        //result["file1.txt"].Should().HaveCount(2);
        //result["file1.txt"][0].fileInfo.Name.Should().Be("file2.txt");
    }
}