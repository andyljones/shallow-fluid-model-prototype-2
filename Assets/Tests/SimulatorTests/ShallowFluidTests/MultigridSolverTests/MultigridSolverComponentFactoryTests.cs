using FakeItEasy;
using NUnit.Framework;
using Simulator.ShallowFluid.MultigridSolver;
using Simulator.ShallowFluid.MultigridSolver.Interpolator;
using Simulator.ShallowFluid.MultigridSolver.Relaxer;
using Simulator.ShallowFluid.MultigridSolver.ResidualTransferer;

namespace Tests.SimulatorTests.ShallowFluidTests.MultigridSolverTests
{
    [TestFixture]
    public class MultigridSolverComponentFactoryTests
    {
        [Test]
        public void CoarsenedGeometries_OfATwoNodeGraph_ShouldBeATwoNodeAndAOneNodeGraph()
        {
            var coarsener = A.Fake<IGraphCoarsener<int>>().GetType();
            var relaxer = A.Fake<IRelaxationCalculator<int>>();
            var interpolator = A.Fake<IInterpolator<int>>();
            var transferer = A.Fake<ISolutionTransferer<int>>();
        }
    }
}
