using NUnit.Framework;

namespace SkillApp.Tests
{
    [TestFixture]
    public class Foo : AssertionHelper
    {
        [Test]
        public void IsNull()
        {
            object nada = null;

            // Classic syntax
            Assert.IsNull(nada);

            // Constraint Syntax
            Assert.That(nada, Is.Null);

            // Inherited syntax
            Expect(nada, Null);
        }
    }
}
