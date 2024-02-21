using Tracer_Formating.Utils.Serialization;

namespace Tests
{
    [TestClass]
    public class SerializaationTests
    {
        private static TracerResult? tracerResult;

        [TestInitialize]
        public void TestInitialize()
        {
            List<ThreadNode> threadNodes = new List<ThreadNode>();
            ThreadNode threadNode = new ThreadNode();
            threadNode.tid = 1;
            threadNode.duration = 48;
            MethodNode methodNode = new MethodNode(), methodNode1 = new MethodNode(), methodNode2 = new MethodNode();
            methodNode._name = "MyMethod";
            methodNode._class = "Foo";
            methodNode.duration = 48;

            methodNode1._name = "InnerMethod";
            methodNode1._class = "Bar";
            methodNode1.duration = 16;
            methodNode1.methods = [];

            methodNode2._name = "InnerMethod";
            methodNode2._class = "Bar";
            methodNode2.duration = 23;
            methodNode2.methods = [];

            methodNode.methods = [];
            methodNode.methods.Add(methodNode1);
            methodNode.methods.Add(methodNode2);

            threadNode.methods = [];
            threadNode.methods.Add(methodNode);
            threadNodes.Add(threadNode);
            tracerResult = new TracerResult(threadNodes);
        }

        [TestMethod]
        public void TestJSONSerialization()
        {
            JSON json = new JSON();
            string actual = json.Serialize(tracerResult);
            string expected = "{\r\n  \"Threads\": [\r\n    {\r\n      \"Thread_id\": 1,\r\n      \"Thread_duration\": 48,\r\n      \"Nested_methods\": [\r\n        {\r\n          \"Method_name\": \"MyMethod\",\r\n          \"Method_class\": \"Foo\",\r\n          \"Method_duration\": 48,\r\n          \"Nested_methods\": [\r\n            {\r\n              \"Method_name\": \"InnerMethod\",\r\n              \"Method_class\": \"Bar\",\r\n              \"Method_duration\": 16,\r\n              \"Nested_methods\": []\r\n            },\r\n            {\r\n              \"Method_name\": \"InnerMethod\",\r\n              \"Method_class\": \"Bar\",\r\n              \"Method_duration\": 23,\r\n              \"Nested_methods\": []\r\n            }\r\n          ]\r\n        }\r\n      ]\r\n    }\r\n  ]\r\n}";
            actual.Should().Be(expected);
        }

        [TestMethod]
        public void TestXMLSerialization()
        {
            XML xml = new XML();
            string actual = xml.Serialize(tracerResult);
            string expected = "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<TracerResult xmlns=\"http://schemas.datacontract.org/2004/07/Tracer_Formating.Tracer\">\r\n  <ThreadNode Thread_id=\"1\" Thread_duration=\"48\">\r\n    <MethodNode Method_name=\"MyMethod\" Method_duration=\"48\" Method_class=\"Foo\">\r\n      <MethodNode Method_name=\"InnerMethod\" Method_duration=\"16\" Method_class=\"Bar\" />\r\n      <MethodNode Method_name=\"InnerMethod\" Method_duration=\"23\" Method_class=\"Bar\" />\r\n    </MethodNode>\r\n  </ThreadNode>\r\n</TracerResult>";
            actual.Should().Be(expected);
        }
    }
}