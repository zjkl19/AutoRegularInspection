using AutoRegularInspection.Services;
using System.IO;
using Xunit;

namespace AutoRegularInspectionTestProject.Services
{
    public class ImageLocatorTests
    {
        [Fact]
        public void FindBest_SameExtension_NoConflictWarning()
        {
            var tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDir);
            File.WriteAllText(Path.Combine(tempDir, "DSC02740.JPG"), "x");

            try
            {
                var result = ImageLocator.FindBest("DSC02740", tempDir);
                var warning = ImageLocator.BuildConflictWarning("DSC02740", result);

                Assert.NotNull(result.SelectedPath);
                Assert.EndsWith(".JPG", result.SelectedPath);
                Assert.Null(warning);
            }
            finally
            {
                Directory.Delete(tempDir, true);
            }
        }

        [Fact]
        public void FindBest_DifferentExtensions_PicksPriorityAndWarns()
        {
            var tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDir);
            File.WriteAllText(Path.Combine(tempDir, "DSC00001.jpg"), "x");
            File.WriteAllText(Path.Combine(tempDir, "DSC00001.png"), "x");

            try
            {
                var result = ImageLocator.FindBest("DSC00001", tempDir);
                var warning = ImageLocator.BuildConflictWarning("DSC00001", result);

                Assert.NotNull(result.SelectedPath);
                Assert.EndsWith(".jpg", result.SelectedPath);
                Assert.False(string.IsNullOrWhiteSpace(warning));
            }
            finally
            {
                Directory.Delete(tempDir, true);
            }
        }
    }
}
