using System.Collections.Generic;
using FakeItEasy;
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
            var coarseGraph = new Graph<int> { { 0, new List<int>() } };
            var geometry = A.Fake<IGeometry<int>>();
            A.CallTo(() => geometry.Graph).Returns(coarseGraph);
            var transferer = new SolutionTransferer<int>(geometry);

            var fineField = new ScalarField<int> {{0, 1f}};
            var coarseField = new ScalarField<int> {{0, 2f}};

            transferer.Transfer(fineField, ref coarseField);

            Assert.That(coarseField[0], Is.EqualTo(1f));
        }
    }
}
