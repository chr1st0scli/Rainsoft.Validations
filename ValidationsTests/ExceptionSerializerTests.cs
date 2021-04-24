using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Rainsoft.Validations.Core;
using Xunit;

namespace ValidationsTests
{
    public class ExceptionSerializerTests
    {
        [Fact]
        public void SerializeDeserialize_InvalidRuleException_Correctly()
        {
            // Arrange
            const string MSG = "error";
            var ex = new InvalidRuleException(MSG) { RuleType = typeof(int), TargetType = typeof(string) };

            // Act
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, ex);
                ms.Flush();
                ms.Position = 0;
                ex = (InvalidRuleException)formatter.Deserialize(ms);
            }

            // Assert
            Assert.Equal(MSG, ex.Message);
            Assert.Equal(typeof(int), ex.RuleType);
            Assert.Equal(typeof(string), ex.TargetType);
        }
    }
}
