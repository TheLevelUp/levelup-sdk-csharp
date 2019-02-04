#tool "nuget:?package=GitVersion.CommandLine"
#addin "Cake.FileHelpers"
#addin "Cake.AWS.S3"
#tool "nuget:?package=OpenCover"
#tool "nuget:?package=ReportGenerator"
#tool "nuget:?package=coveralls.io"
#addin "nuget:?package=Cake.Coveralls"

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Debug");
var solution = GetFiles("*.sln").First();

//////////////////////////////////////////////////////////////////////
// APPVEYOR SETUP
//////////////////////////////////////////////////////////////////////

if(AppVeyor.IsRunningOnAppVeyor)
{
    Setup(context =>
    {
        var settings = Context.CreateDownloadSettings().SetRegion("us-east-1").SetBucketName("levelup-pos-build");
        System.Threading.Tasks.Task.WaitAll(new[] 
        {
            // Configure nuget sources
            S3Download(new FilePath(EnvironmentVariable("APPDATA") + "\\NuGet\\NuGet.Config"), "NuGet.Config", settings),
            // Retrieve cert
            S3Download(new FilePath(".\\BuildScripts\\Certificates.p12"), "Certificates.p12", settings)
        });
        
        // Pass version info to AppVeyor
        GitVersion(new GitVersionSettings{
            ArgumentCustomization = args => args.Append("-verbosity Warn"),
            OutputType = GitVersionOutput.BuildServer,
            NoFetch = true
        });
    });
}

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    CleanDirectories("**/bin");
    CleanDirectories("**/obj");
});

Task("Restore")
    .IsDependentOn("Clean")
    .Does(() =>
{
    NuGetRestore(solution);
});

Task("Build")
    .IsDependentOn("Restore")
    .Does(() =>
{
    MSBuild(solution, new MSBuildSettings {
        Configuration = configuration,
        EnvironmentVariables = new Dictionary<string, string> 
        {  
            { "GitVersion_NoFetchEnabled", "true" } 
        },
        ArgumentCustomization = arg => arg.AppendSwitch("/p:DebugType","=","Full")
    });
});

Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
{
        OpenCover(
            tool => tool.DotNetCoreVSTest("**/bin/**/*Tests.dll"),
            new FilePath("./coverage.xml"),
            new OpenCoverSettings { ReturnTargetCodeOffset = 0, OldStyle = true }.WithFilter("+[*LevelUp*]LevelUp*").WithFilter("-[*Tests*]*")
        );

        if(AppVeyor.IsRunningOnAppVeyor)
        {
            CoverallsIo("./coverage.xml", new CoverallsIoSettings {
                RepoToken = EnvironmentVariable("COVERALLS_TOKEN")
            });
        }
        else
        {
            ReportGenerator("./coverage.xml", "./coverage", new ReportGeneratorSettings {
                ReportTypes = new[] { ReportGeneratorReportType.HtmlSummary }
            });
        }
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Test");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);