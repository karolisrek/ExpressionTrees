using ExpressionTrees.Task2.ExpressionMapping.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExpressionTrees.Task2.ExpressionMapping.Tests
{
    [TestClass]
    public class ExpressionMappingTests
    {
        // todo: add as many test methods as you wish, but they should be enough to cover basic scenarios of the mapping generator

        [TestMethod]
        public void TestMethod1()
        {
            var mapGenerator = new MappingGenerator();
            var mapper = mapGenerator.Generate<Foo, Bar>();
            var testData = new Foo()
            {
                LongF = 1,
                LongP = 2,
                StringF = "aa",
                StringP = "bb",
                Number = 3
            };

            var res = mapper.Map(testData);

            Assert.AreEqual(1, res.LongF);
            Assert.AreEqual(2, res.LongP);
            Assert.AreEqual("aa", res.StringF);
            Assert.AreEqual("bb", res.StringP);
            Assert.AreEqual(0, res.Number);
            Assert.AreEqual(null, res.String1);
        }
    }
}
