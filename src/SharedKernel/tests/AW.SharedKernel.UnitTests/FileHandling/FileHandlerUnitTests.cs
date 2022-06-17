﻿using AW.SharedKernel.FileHandling;
using AW.SharedKernel.UnitTesting;
using FluentAssertions;
using System.IO;
using System.Reflection;
using Xunit;

namespace AW.SharedKernel.UnitTests.FileHandling
{
    public class FileHandlerUnitTests
    {
        public class FileExists
        {
            [Theory, AutoMoqData]
            public void FileExists_ReturnsTrue(
                FileHandler sut            
            )
            {
                //Arrange

                //Act
                var result = sut.FileExists(Assembly.GetExecutingAssembly().Location);

                //Assert
                result.Should().Be(true);
            }

            [Theory, AutoMoqData]
            public void FileExists_ReturnsFalse(
                FileHandler sut,
                string fileName
            )
            {
                //Arrange

                //Act
                var result = sut.FileExists(fileName);

                //Assert
                result.Should().Be(false);
            }
        }
        
        public class WriteFile
        {
            [Theory, AutoMoqData]
            public void FileExists_ReturnsFalse(
                FileHandler sut,
                byte[] bytes
            )
            {
                //Arrange
                var fileName = Path.Combine(
                    Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                    "writeFile.txt"
                );

                //Act
                sut.WriteFile(fileName, bytes);

                //Assert
            }
        }
    }
}