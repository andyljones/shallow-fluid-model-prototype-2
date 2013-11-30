using System.Collections.Generic;
using NUnit.Framework;
using Simulator.ShallowFluid;
using Simulator.ShallowFluid.MultigridSolver.ResidualTransferer;

namespace Tests.SimulatorTests.ShallowFluidTests.MultigridSolverTests.ResidualTransfererTests
{
    [TestFixture]
    public class ResidualTransfererTests
    {
        [Test]
        public void Transfer_OnTwoSingletonGraphsOfTheSameNode_ShouldTransferTheValueOnTheFineToTheCoarse()
        {
            var coarseGraph = new Graph<int> { { 0, new List<int>() } };
            var transferer = new ResidualTransferer<int> { CoarseGraph = coarseGraph };

            var fineField = new ScalarField<int> {{0, 1f}};
            var coarseField = new ScalarField<int> {{0, 2f}};

            transferer.Transfer(fineField, ref coarseField);

            Assert.That(coarseField[0], Is.EqualTo(1f));
        }
    }
}
