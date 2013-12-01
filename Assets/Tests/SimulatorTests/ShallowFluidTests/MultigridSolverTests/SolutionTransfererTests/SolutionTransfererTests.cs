using System.Collections.Generic;
using NUnit.Framework;
using Simulator.ShallowFluid;
using Simulator.ShallowFluid.MultigridSolver.ResidualTransferer;

namespace Tests.SimulatorTests.ShallowFluidTests.MultigridSolverTests.SolutionTransfererTests
{
    [TestFixture]
    public class SolutionTransfererTests
    {
        [Test]
        public void Transfer_OnTwoSingletonGraphsOfTheSameNode_ShouldTransferTheValueOnTheFineToTheCoarse()
        {
            var interpolationGraph = new Graph<int> { { 0, new List<int> {0} } };
            var transferer = new SolutionTransferer<int>(interpolationGraph);

            var fineField = new ScalarField<int> {{0, 1f}};
            var coarseField = new ScalarField<int> {{0, 2f}};

            transferer.Transfer(fineField, ref coarseField);

            Assert.That(coarseField[0], Is.EqualTo(1f));
        }
    }
}
