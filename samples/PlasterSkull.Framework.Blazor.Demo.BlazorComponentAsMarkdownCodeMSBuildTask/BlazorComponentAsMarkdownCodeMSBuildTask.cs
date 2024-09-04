using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MSBuildTask = Microsoft.Build.Utilities.Task;

namespace PlasterSkull.Framework.Blazor.Demo.BlazorComponentAsMarkdownCodeMSBuildTask
{
    public class BlazorComponentAsMarkdownCodeMSBuildTask : MSBuildTask
    {
        private const string s_exampleMarker = "Example";

        public override bool Execute()
        {
#if DEBUG
            //if (!Debugger.IsAttached)
            //    Debugger.Launch();
#endif
            Parallel.ForEach(
                Directory
                    .EnumerateFiles(Path.GetDirectoryName(BuildEngine.ProjectFileOfTaskNode), "*.razor", SearchOption.AllDirectories)
                    .Where(f => Path.GetFileNameWithoutExtension(f).EndsWith(s_exampleMarker)),
                async filePath =>
                {
                    var fileText = await TryReadFileAsync(filePath);
                    if (fileText == null)
                        return;

                    var mdWithCodeFilePath = filePath + ".md";
                    var mdValue = $"```csharp\r\n{fileText}\r\n```";
                    File.WriteAllText(mdWithCodeFilePath, mdValue);
                });

            return !Log.HasLoggedErrors;
        }

        private async Task<string> TryReadFileAsync(string filePath)
        {
            var result = string.Empty;

            try
            {
                using (var streamReader = new StreamReader(filePath))
                {
                    result = await streamReader.ReadToEndAsync();
                }
            }
            catch(Exception ex)
            {
                Log.LogError(ex.Message);
            }

            return result;
        }
    }
}
