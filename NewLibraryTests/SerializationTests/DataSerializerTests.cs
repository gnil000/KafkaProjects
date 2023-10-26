using NewLibrary.SerializerDesirializer;
using NewLibraryTests.ModelsForTest;
using Newtonsoft.Json;
using System.Runtime.ExceptionServices;
using System.Text;

namespace NewLibraryTests.SerializationTests
{
    [TestClass]
    public class DataSerializerTests
    {

        [TestMethod]
        public void Deserialize_DataByte_DataJsonReturned()
        {
            string first = "any string";
            int second = 85395;
            DateTime third = new DateTime(2023, 10, 14);
			Any serializer = new Any() { FirstProperty = first, SecondProperty = second, ThirdProperty= third};
			var obj = JsonConvert.SerializeObject(serializer);
			var bytes = Encoding.UTF8.GetBytes(obj);

			DataDesirializer<Any> desirializer = new DataDesirializer<Any>() {  };


        }

    }
}
