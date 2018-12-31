using ApprovalTests.Reporters;
using Xunit;

[assembly: CollectionBehavior(CollectionBehavior.CollectionPerAssembly, DisableTestParallelization = true)]
[assembly: UseReporter(typeof(DiffReporter), typeof(AllFailingTestsClipboardReporter))]